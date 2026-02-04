using System.Reflection;
using System.Text.Json;

namespace CrushEase.Services;

/// <summary>
/// Service for managing application version and checking for updates
/// </summary>
public static class VersionService
{
    private const string VERSION_CHECK_URL = "https://api.github.com/repos/NandanaMD/CrushEaseLedger/releases/latest";
    private const string RELEASES_URL = "https://github.com/NandanaMD/CrushEaseLedger/releases";
    
    /// <summary>
    /// Gets the current application version
    /// </summary>
    public static Version CurrentVersion
    {
        get
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version ?? new Version(1, 0, 0);
        }
    }
    
    /// <summary>
    /// Gets the current version as a string (e.g., "1.0.0")
    /// </summary>
    public static string CurrentVersionString => $"{CurrentVersion.Major}.{CurrentVersion.Minor}.{CurrentVersion.Build}";
    
    /// <summary>
    /// Cached latest version information
    /// </summary>
    private static VersionCheckResult? _cachedResult;
    private static DateTime _lastCheckTime = DateTime.MinValue;
    private static readonly TimeSpan CACHE_DURATION = TimeSpan.FromHours(1);
    
    /// <summary>
    /// Checks for updates and returns version status
    /// </summary>
    /// <param name="forceRefresh">Force a fresh check, ignoring cache</param>
    public static async Task<VersionCheckResult> CheckForUpdatesAsync(bool forceRefresh = false)
    {
        // Return cached result if available and not expired
        if (!forceRefresh && _cachedResult != null && 
            DateTime.Now - _lastCheckTime < CACHE_DURATION)
        {
            return _cachedResult;
        }
        
        try
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "CrushEase-Ledger");
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            
            var response = await httpClient.GetStringAsync(VERSION_CHECK_URL);
            var jsonDoc = JsonDocument.Parse(response);
            var root = jsonDoc.RootElement;
            
            // Parse version from tag_name (e.g., "v1.2.0" or "1.2.0")
            var tagName = root.GetProperty("tag_name").GetString() ?? "1.0.0";
            var versionString = tagName.TrimStart('v', 'V');
            
            if (Version.TryParse(versionString, out var latestVersion))
            {
                // Robust version comparison: compare major, minor, build components
                bool isUpdateAvailable = false;
                if (latestVersion.Major > CurrentVersion.Major)
                    isUpdateAvailable = true;
                else if (latestVersion.Major == CurrentVersion.Major && latestVersion.Minor > CurrentVersion.Minor)
                    isUpdateAvailable = true;
                else if (latestVersion.Major == CurrentVersion.Major && latestVersion.Minor == CurrentVersion.Minor && latestVersion.Build > CurrentVersion.Build)
                    isUpdateAvailable = true;
                
                var result = new VersionCheckResult
                {
                    CurrentVersion = CurrentVersion,
                    LatestVersion = latestVersion,
                    IsUpdateAvailable = isUpdateAvailable,
                    IsLatest = !isUpdateAvailable,
                    ReleaseUrl = root.GetProperty("html_url").GetString() ?? RELEASES_URL,
                    ReleaseName = root.GetProperty("name").GetString() ?? "Latest Release",
                    ReleaseNotes = root.GetProperty("body").GetString() ?? "",
                    PublishedAt = root.TryGetProperty("published_at", out var pubDate) 
                        ? DateTime.Parse(pubDate.GetString() ?? DateTime.Now.ToString())
                        : DateTime.Now,
                    CheckSuccessful = true
                };
                
                // Cache the result
                _cachedResult = result;
                _lastCheckTime = DateTime.Now;
                
                return result;
            }
        }
        catch (Exception ex)
        {
            Utils.Logger.LogError(ex, "Failed to check for updates");
        }
        
        // Return fallback result if check failed
        return new VersionCheckResult
        {
            CurrentVersion = CurrentVersion,
            LatestVersion = CurrentVersion,
            IsUpdateAvailable = false,
            IsLatest = true,
            ReleaseUrl = RELEASES_URL,
            ReleaseName = "Version Check Failed",
            ReleaseNotes = "Could not connect to update server.",
            PublishedAt = DateTime.Now,
            CheckSuccessful = false
        };
    }
    
    /// <summary>
    /// Gets a simple version status string
    /// </summary>
    public static async Task<string> GetVersionStatusAsync()
    {
        var result = await CheckForUpdatesAsync();
        
        if (!result.CheckSuccessful)
            return "Version: " + CurrentVersionString;
        
        if (result.IsUpdateAvailable)
            return $"Version: {CurrentVersionString} (Update Available: v{result.LatestVersion})";
        
        return $"Version: {CurrentVersionString} (Latest)";
    }
    
    /// <summary>
    /// Opens the releases page in the default browser
    /// </summary>
    public static void OpenReleasesPage()
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = RELEASES_URL,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
        catch (Exception ex)
        {
            Utils.Logger.LogError(ex, "Failed to open releases page");
        }
    }
    
    /// <summary>
    /// Downloads the latest installer from GitHub releases
    /// </summary>
    /// <param name="progress">Progress callback (0-100)</param>
    /// <returns>Path to downloaded installer, or null if failed</returns>
    public static async Task<string?> DownloadLatestInstallerAsync(IProgress<int>? progress = null)
    {
        try
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "CrushEase-Ledger");
            httpClient.Timeout = TimeSpan.FromMinutes(5); // Longer timeout for download
            
            // Get release info to find installer download URL
            var response = await httpClient.GetStringAsync(VERSION_CHECK_URL);
            var jsonDoc = JsonDocument.Parse(response);
            var root = jsonDoc.RootElement;
            
            // Find the .exe installer in assets
            string? installerUrl = null;
            string installerFileName = "CrushEaseLedger-Setup.exe";
            
            if (root.TryGetProperty("assets", out var assets))
            {
                foreach (var asset in assets.EnumerateArray())
                {
                    var name = asset.GetProperty("name").GetString() ?? "";
                    if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        installerUrl = asset.GetProperty("browser_download_url").GetString();
                        installerFileName = name;
                        break;
                    }
                }
            }
            
            if (string.IsNullOrEmpty(installerUrl))
            {
                Utils.Logger.LogWarning("No installer found in latest release");
                return null;
            }
            
            // Download to temp folder
            var tempPath = Path.Combine(Path.GetTempPath(), installerFileName);
            
            // Delete existing file if present
            if (File.Exists(tempPath))
                File.Delete(tempPath);
            
            // Download with progress tracking
            using var downloadResponse = await httpClient.GetAsync(installerUrl, HttpCompletionOption.ResponseHeadersRead);
            downloadResponse.EnsureSuccessStatusCode();
            
            var totalBytes = downloadResponse.Content.Headers.ContentLength ?? 0;
            var downloadedBytes = 0L;
            
            using var contentStream = await downloadResponse.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
            
            var buffer = new byte[8192];
            int bytesRead;
            
            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await fileStream.WriteAsync(buffer, 0, bytesRead);
                downloadedBytes += bytesRead;
                
                if (totalBytes > 0 && progress != null)
                {
                    var percentComplete = (int)((downloadedBytes * 100) / totalBytes);
                    progress.Report(percentComplete);
                }
            }
            
            Utils.Logger.LogInfo($"Downloaded installer to: {tempPath}");
            return tempPath;
        }
        catch (Exception ex)
        {
            Utils.Logger.LogError(ex, "Failed to download installer");
            return null;
        }
    }
    
    /// <summary>
    /// Launches the installer and optionally closes the current application
    /// </summary>
    /// <param name="installerPath">Path to the installer executable</param>
    /// <param name="closeApp">Whether to close the current app after launching installer</param>
    public static void LaunchInstaller(string installerPath, bool closeApp = true)
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = installerPath,
                UseShellExecute = true,
                Verb = "runas" // Request admin privileges
            };
            
            System.Diagnostics.Process.Start(psi);
            Utils.Logger.LogInfo("Launched installer");
            
            if (closeApp)
            {
                // Give installer time to start
                System.Threading.Thread.Sleep(500);
                Application.Exit();
            }
        }
        catch (Exception ex)
        {
            Utils.Logger.LogError(ex, "Failed to launch installer");
            throw;
        }
    }
}

/// <summary>
/// Result of a version check operation
/// </summary>
public class VersionCheckResult
{
    public Version CurrentVersion { get; set; } = new Version(1, 0, 0);
    public Version LatestVersion { get; set; } = new Version(1, 0, 0);
    public bool IsUpdateAvailable { get; set; }
    public bool IsLatest { get; set; }
    public string ReleaseUrl { get; set; } = "";
    public string ReleaseName { get; set; } = "";
    public string ReleaseNotes { get; set; } = "";
    public DateTime PublishedAt { get; set; }
    public bool CheckSuccessful { get; set; }
    
    /// <summary>
    /// Gets a user-friendly status message
    /// </summary>
    public string GetStatusMessage()
    {
        if (!CheckSuccessful)
            return "Unable to check for updates";
        
        if (IsUpdateAvailable)
            return $"Update available: v{LatestVersion}";
        
        return "You have the latest version";
    }
}
