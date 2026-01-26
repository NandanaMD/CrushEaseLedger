using System.Security.Cryptography;
using System.Text;
using CrushEase.Utils;

namespace CrushEase.Services;

/// <summary>
/// Handles PIN security and authentication
/// </summary>
public static class SecurityService
{
    private const string MASTER_KEY = "C2026EL"; // Hardcoded master key for recovery
    private const string SETTINGS_FILE = "security.dat";
    
    /// <summary>
    /// Check if PIN is set up
    /// </summary>
    public static bool IsPinConfigured()
    {
        string settingsPath = GetSettingsPath();
        return File.Exists(settingsPath);
    }
    
    /// <summary>
    /// Set up new PIN
    /// </summary>
    public static void SetPin(string pin)
    {
        try
        {
            string encrypted = EncryptPin(pin);
            string settingsPath = GetSettingsPath();
            File.WriteAllText(settingsPath, encrypted);
            Logger.LogInfo("PIN configured successfully");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to set PIN");
            throw;
        }
    }
    
    /// <summary>
    /// Verify PIN or master key
    /// </summary>
    public static bool VerifyPin(string pin)
    {
        try
        {
            // Check master key first
            if (pin == MASTER_KEY)
            {
                Logger.LogInfo("Access granted via master key");
                return true;
            }
            
            // Check user PIN
            string settingsPath = GetSettingsPath();
            if (!File.Exists(settingsPath))
                return false;
            
            string encrypted = File.ReadAllText(settingsPath);
            string decrypted = DecryptPin(encrypted);
            
            bool isValid = pin == decrypted;
            if (isValid)
                Logger.LogInfo("Access granted via PIN");
            else
                Logger.LogWarning("Failed PIN attempt");
            
            return isValid;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error verifying PIN");
            return false;
        }
    }
    
    /// <summary>
    /// Get master key (for support purposes)
    /// </summary>
    public static string GetMasterKey()
    {
        return MASTER_KEY;
    }
    
    private static string GetSettingsPath()
    {
        string appData = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CrushEase"
        );
        
        if (!Directory.Exists(appData))
            Directory.CreateDirectory(appData);
        
        return Path.Combine(appData, SETTINGS_FILE);
    }
    
    private static string EncryptPin(string pin)
    {
        // Simple encryption - good enough for internal business tool
        byte[] data = Encoding.UTF8.GetBytes(pin);
        byte[] key = Encoding.UTF8.GetBytes("CrushEase2026Key");
        
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)(data[i] ^ key[i % key.Length]);
        }
        
        return Convert.ToBase64String(data);
    }
    
    private static string DecryptPin(string encrypted)
    {
        byte[] data = Convert.FromBase64String(encrypted);
        byte[] key = Encoding.UTF8.GetBytes("CrushEase2026Key");
        
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)(data[i] ^ key[i % key.Length]);
        }
        
        return Encoding.UTF8.GetString(data);
    }
}
