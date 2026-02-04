using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

/// <summary>
/// Calm, non-intrusive update notification dialog
/// </summary>
public partial class UpdateNotificationForm : Form
{
    private readonly VersionCheckResult _updateInfo;
    private string? _downloadedInstallerPath;
    
    public UpdateNotificationForm(VersionCheckResult updateInfo)
    {
        _updateInfo = updateInfo;
        InitializeComponent();
        InitializeCustomComponents();
        ApplyModernTheme();
    }
    
    private void InitializeCustomComponents()
    {
        this.Text = "Update Available";
        this.Size = new Size(500, 380);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;
        
        // Icon
        var iconLabel = new Label
        {
            Text = "🎉",
            Font = new Font("Segoe UI", 36, FontStyle.Regular),
            Location = new Point(20, 20),
            Size = new Size(60, 60),
            TextAlign = ContentAlignment.MiddleCenter
        };
        
        // Title
        var titleLabel = new Label
        {
            Text = "New Version Available!",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(90, 25),
            Size = new Size(380, 30),
            TextAlign = ContentAlignment.MiddleLeft
        };
        
        // Version info
        var versionLabel = new Label
        {
            Text = $"Version {_updateInfo.LatestVersion} is now available\n(You have {_updateInfo.CurrentVersion})",
            Font = new Font("Segoe UI", 9.5F),
            Location = new Point(90, 55),
            Size = new Size(380, 35),
            TextAlign = ContentAlignment.MiddleLeft
        };
        
        // Release notes
        var notesLabel = new Label
        {
            Text = "What's New:",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Location = new Point(20, 110),
            Size = new Size(460, 25),
            TextAlign = ContentAlignment.MiddleLeft
        };
        
        var notesBox = new TextBox
        {
            Text = _updateInfo.ReleaseNotes,
            Location = new Point(20, 135),
            Size = new Size(460, 120),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Segoe UI", 9F),
            BackColor = Color.FromArgb(250, 250, 250),
            BorderStyle = BorderStyle.FixedSingle
        };
        
        // Progress bar (hidden initially)
        _progressBar = new ProgressBar
        {
            Location = new Point(20, 265),
            Size = new Size(460, 25),
            Visible = false,
            Style = ProgressBarStyle.Continuous
        };
        
        // Status label (hidden initially)
        _statusLabel = new Label
        {
            Text = "Downloading...",
            Location = new Point(20, 265),
            Size = new Size(460, 25),
            TextAlign = ContentAlignment.MiddleLeft,
            Visible = false,
            Font = new Font("Segoe UI", 9F)
        };
        
        // Buttons
        _btnDownload = new Button
        {
            Text = "Download & Install",
            Location = new Point(270, 300),
            Size = new Size(140, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand,
            FlatStyle = FlatStyle.Flat
        };
        _btnDownload.Click += BtnDownload_Click;
        
        _btnLater = new Button
        {
            Text = "Later",
            Location = new Point(420, 300),
            Size = new Size(60, 35),
            Font = new Font("Segoe UI", 10),
            Cursor = Cursors.Hand,
            FlatStyle = FlatStyle.Flat
        };
        _btnLater.Click += (s, e) => this.Close();
        
        var btnViewOnline = new Button
        {
            Text = "View on GitHub",
            Location = new Point(20, 300),
            Size = new Size(120, 35),
            Font = new Font("Segoe UI", 9),
            Cursor = Cursors.Hand,
            FlatStyle = FlatStyle.Flat
        };
        btnViewOnline.Click += (s, e) =>
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _updateInfo.ReleaseUrl,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch { }
        };
        
        // Add controls
        this.Controls.AddRange(new Control[]
        {
            iconLabel, titleLabel, versionLabel, notesLabel, notesBox,
            _progressBar, _statusLabel, _btnDownload, _btnLater, btnViewOnline
        });
    }
    
    private void ApplyModernTheme()
    {
        this.BackColor = Color.White;
        
        _btnDownload.BackColor = Color.FromArgb(0, 120, 215);
        _btnDownload.ForeColor = Color.White;
        _btnDownload.FlatAppearance.BorderSize = 0;
        
        _btnLater.BackColor = Color.FromArgb(240, 240, 240);
        _btnLater.ForeColor = Color.FromArgb(60, 60, 60);
        _btnLater.FlatAppearance.BorderSize = 0;
    }
    
    private async void BtnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            // Disable buttons and show progress
            _btnDownload.Enabled = false;
            _btnLater.Enabled = false;
            _statusLabel.Visible = true;
            _statusLabel.Text = "Downloading update...";
            _progressBar.Visible = true;
            _progressBar.Value = 0;
            
            // Download with progress
            var progress = new Progress<int>(percent =>
            {
                if (_progressBar.InvokeRequired)
                {
                    _progressBar.Invoke(new Action(() =>
                    {
                        _progressBar.Value = Math.Min(percent, 100);
                        _statusLabel.Text = $"Downloading update... {percent}%";
                    }));
                }
                else
                {
                    _progressBar.Value = Math.Min(percent, 100);
                    _statusLabel.Text = $"Downloading update... {percent}%";
                }
            });
            
            _downloadedInstallerPath = await VersionService.DownloadLatestInstallerAsync(progress);
            
            if (string.IsNullOrEmpty(_downloadedInstallerPath))
            {
                MessageBox.Show(
                    "Failed to download the update. Please try again later or download manually from GitHub.",
                    "Download Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                
                _btnDownload.Enabled = true;
                _btnLater.Enabled = true;
                _progressBar.Visible = false;
                _statusLabel.Visible = false;
                return;
            }
            
            // Download complete - offer to install
            _statusLabel.Text = "Download complete!";
            _progressBar.Value = 100;
            
            var result = MessageBox.Show(
                "Update downloaded successfully!\n\n" +
                "The application will close and the installer will launch.\n" +
                "Click OK to proceed with installation.",
                "Ready to Install",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
            );
            
            if (result == DialogResult.OK)
            {
                // Launch installer and close app
                VersionService.LaunchInstaller(_downloadedInstallerPath, closeApp: true);
            }
            else
            {
                MessageBox.Show(
                    $"Installer saved to:\n{_downloadedInstallerPath}\n\n" +
                    "You can run it manually when ready.",
                    "Installation Postponed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.Close();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during update download");
            MessageBox.Show(
                "An error occurred while downloading the update.\n" +
                "Please try again later or download manually from GitHub.",
                "Download Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            
            _btnDownload.Enabled = true;
            _btnLater.Enabled = true;
            _progressBar.Visible = false;
            _statusLabel.Visible = false;
        }
    }
    
    private ProgressBar _progressBar = null!;
    private Label _statusLabel = null!;
    private Button _btnDownload = null!;
    private Button _btnLater = null!;
}
