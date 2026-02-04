using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

/// <summary>
/// About dialog showing version information and update status
/// </summary>
public partial class AboutForm : Form
{
    private Label lblAppName;
    private Label lblVersion;
    private Label lblUpdateStatus;
    private Label lblCopyright;
    private Button btnCheckUpdates;
    private Button btnDownload;
    private Button btnClose;
    private PictureBox picLogo;
    private Panel panelUpdateInfo;
    private Label lblReleaseNotes;
    private ProgressBar progressBar;
    
    private VersionCheckResult? _versionResult;
    
    public AboutForm()
    {
        InitializeComponent();
        LoadVersionInfo();
    }
    
    private void InitializeComponent()
    {
        this.lblAppName = new Label();
        this.lblVersion = new Label();
        this.lblUpdateStatus = new Label();
        this.lblCopyright = new Label();
        this.btnCheckUpdates = new Button();
        this.btnDownload = new Button();
        this.btnClose = new Button();
        this.picLogo = new PictureBox();
        this.panelUpdateInfo = new Panel();
        this.lblReleaseNotes = new Label();
        this.progressBar = new ProgressBar();
        
        ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
        this.panelUpdateInfo.SuspendLayout();
        this.SuspendLayout();
        
        // Form
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(500, 400);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "About CrushEase";
        this.BackColor = ModernTheme.BackgroundColor;
        
        // Logo
        this.picLogo.Location = new Point(180, 20);
        this.picLogo.Name = "picLogo";
        this.picLogo.Size = new Size(140, 140);
        this.picLogo.SizeMode = PictureBoxSizeMode.Zoom;
        this.picLogo.TabIndex = 0;
        this.picLogo.TabStop = false;
        
        // Try to load logo
        try
        {
            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "mainlogo.ico");
            if (File.Exists(logoPath))
            {
                this.picLogo.Image = Image.FromFile(logoPath);
            }
        }
        catch { /* Logo not available */ }
        
        // App Name
        this.lblAppName.AutoSize = false;
        this.lblAppName.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        this.lblAppName.ForeColor = ModernTheme.PrimaryColor;
        this.lblAppName.Location = new Point(20, 170);
        this.lblAppName.Name = "lblAppName";
        this.lblAppName.Size = new Size(460, 35);
        this.lblAppName.TabIndex = 1;
        this.lblAppName.Text = "CrushEase Ledger";
        this.lblAppName.TextAlign = ContentAlignment.MiddleCenter;
        
        // Version Label
        this.lblVersion.AutoSize = false;
        this.lblVersion.Font = new Font("Segoe UI", 11F);
        this.lblVersion.ForeColor = ModernTheme.ForegroundColor;
        this.lblVersion.Location = new Point(20, 210);
        this.lblVersion.Name = "lblVersion";
        this.lblVersion.Size = new Size(460, 25);
        this.lblVersion.TabIndex = 2;
        this.lblVersion.Text = "Version: Loading...";
        this.lblVersion.TextAlign = ContentAlignment.MiddleCenter;
        
        // Update Status Label
        this.lblUpdateStatus.AutoSize = false;
        this.lblUpdateStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.lblUpdateStatus.ForeColor = ModernTheme.SuccessColor;
        this.lblUpdateStatus.Location = new Point(20, 240);
        this.lblUpdateStatus.Name = "lblUpdateStatus";
        this.lblUpdateStatus.Size = new Size(460, 25);
        this.lblUpdateStatus.TabIndex = 3;
        this.lblUpdateStatus.Text = "";
        this.lblUpdateStatus.TextAlign = ContentAlignment.MiddleCenter;
        
        // Progress Bar
        this.progressBar.Location = new Point(100, 270);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new Size(300, 10);
        this.progressBar.TabIndex = 4;
        this.progressBar.Style = ProgressBarStyle.Marquee;
        this.progressBar.Visible = false;
        
        // Update Info Panel
        this.panelUpdateInfo.BackColor = Color.FromArgb(240, 248, 255);
        this.panelUpdateInfo.BorderStyle = BorderStyle.FixedSingle;
        this.panelUpdateInfo.Location = new Point(20, 285);
        this.panelUpdateInfo.Name = "panelUpdateInfo";
        this.panelUpdateInfo.Size = new Size(460, 60);
        this.panelUpdateInfo.TabIndex = 5;
        this.panelUpdateInfo.Visible = false;
        
        // Release Notes Label
        this.lblReleaseNotes.AutoSize = false;
        this.lblReleaseNotes.Font = new Font("Segoe UI", 8.5F);
        this.lblReleaseNotes.Location = new Point(10, 5);
        this.lblReleaseNotes.Name = "lblReleaseNotes";
        this.lblReleaseNotes.Size = new Size(440, 50);
        this.lblReleaseNotes.TabIndex = 0;
        this.lblReleaseNotes.Text = "";
        
        this.panelUpdateInfo.Controls.Add(this.lblReleaseNotes);
        
        // Copyright
        this.lblCopyright.AutoSize = false;
        this.lblCopyright.Font = new Font("Segoe UI", 9F);
        this.lblCopyright.ForeColor = Color.Gray;
        this.lblCopyright.Location = new Point(20, 350);
        this.lblCopyright.Name = "lblCopyright";
        this.lblCopyright.Size = new Size(460, 20);
        this.lblCopyright.TabIndex = 6;
        this.lblCopyright.Text = $"© {DateTime.Now.Year} CrushEase Development. All rights reserved.";
        this.lblCopyright.TextAlign = ContentAlignment.MiddleCenter;
        
        // Check Updates Button
        this.btnCheckUpdates.BackColor = ModernTheme.PrimaryColor;
        this.btnCheckUpdates.FlatStyle = FlatStyle.Flat;
        this.btnCheckUpdates.Font = new Font("Segoe UI", 9F);
        this.btnCheckUpdates.ForeColor = Color.White;
        this.btnCheckUpdates.Location = new Point(80, 370);
        this.btnCheckUpdates.Name = "btnCheckUpdates";
        this.btnCheckUpdates.Size = new Size(130, 30);
        this.btnCheckUpdates.TabIndex = 7;
        this.btnCheckUpdates.Text = "Check for Updates";
        this.btnCheckUpdates.UseVisualStyleBackColor = false;
        this.btnCheckUpdates.Click += new EventHandler(this.BtnCheckUpdates_Click!);
        
        // Download Button
        this.btnDownload.BackColor = ModernTheme.SuccessColor;
        this.btnDownload.FlatStyle = FlatStyle.Flat;
        this.btnDownload.Font = new Font("Segoe UI", 9F);
        this.btnDownload.ForeColor = Color.White;
        this.btnDownload.Location = new Point(220, 370);
        this.btnDownload.Name = "btnDownload";
        this.btnDownload.Size = new Size(130, 30);
        this.btnDownload.TabIndex = 8;
        this.btnDownload.Text = "Download Update";
        this.btnDownload.UseVisualStyleBackColor = false;
        this.btnDownload.Visible = false;
        this.btnDownload.Click += new EventHandler(this.BtnDownload_Click!);
        
        // Close Button
        this.btnClose.BackColor = Color.FromArgb(100, 100, 100);
        this.btnClose.FlatStyle = FlatStyle.Flat;
        this.btnClose.Font = new Font("Segoe UI", 9F);
        this.btnClose.ForeColor = Color.White;
        this.btnClose.Location = new Point(360, 370);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new Size(100, 30);
        this.btnClose.TabIndex = 9;
        this.btnClose.Text = "Close";
        this.btnClose.UseVisualStyleBackColor = false;
        this.btnClose.Click += new EventHandler(this.BtnClose_Click!);
        
        // Add controls to form
        this.Controls.Add(this.picLogo);
        this.Controls.Add(this.lblAppName);
        this.Controls.Add(this.lblVersion);
        this.Controls.Add(this.lblUpdateStatus);
        this.Controls.Add(this.progressBar);
        this.Controls.Add(this.panelUpdateInfo);
        this.Controls.Add(this.lblCopyright);
        this.Controls.Add(this.btnCheckUpdates);
        this.Controls.Add(this.btnDownload);
        this.Controls.Add(this.btnClose);
        
        ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
        this.panelUpdateInfo.ResumeLayout(false);
        this.ResumeLayout(false);
    }
    
    private async void LoadVersionInfo()
    {
        // Show current version immediately
        lblVersion.Text = $"Version: {VersionService.CurrentVersionString}";
        
        // Check for updates
        await CheckForUpdatesAsync();
    }
    
    private async Task CheckForUpdatesAsync()
    {
        try
        {
            // Show progress
            progressBar.Visible = true;
            btnCheckUpdates.Enabled = false;
            lblUpdateStatus.Text = "Checking for updates...";
            lblUpdateStatus.ForeColor = Color.Gray;
            
            // Check for updates
            _versionResult = await VersionService.CheckForUpdatesAsync(forceRefresh: true);
            
            // Hide progress
            progressBar.Visible = false;
            btnCheckUpdates.Enabled = true;
            
            // Update UI based on result
            if (!_versionResult.CheckSuccessful)
            {
                lblUpdateStatus.Text = "Unable to check for updates";
                lblUpdateStatus.ForeColor = Color.Orange;
                panelUpdateInfo.Visible = false;
                btnDownload.Visible = false;
            }
            else if (_versionResult.IsUpdateAvailable)
            {
                lblUpdateStatus.Text = $"✓ Update Available: v{_versionResult.LatestVersion}";
                lblUpdateStatus.ForeColor = ModernTheme.WarningColor;
                
                // Show release info
                var releaseInfo = $"Latest: {_versionResult.ReleaseName}\n";
                releaseInfo += $"Released: {_versionResult.PublishedAt:MMM dd, yyyy}\n";
                if (!string.IsNullOrEmpty(_versionResult.ReleaseNotes))
                {
                    var notes = _versionResult.ReleaseNotes;
                    if (notes.Length > 100)
                        notes = notes.Substring(0, 100) + "...";
                    releaseInfo += notes;
                }
                
                lblReleaseNotes.Text = releaseInfo;
                panelUpdateInfo.Visible = true;
                btnDownload.Visible = true;
            }
            else
            {
                lblUpdateStatus.Text = "✓ You have the latest version";
                lblUpdateStatus.ForeColor = ModernTheme.SuccessColor;
                panelUpdateInfo.Visible = false;
                btnDownload.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to check for updates");
            progressBar.Visible = false;
            btnCheckUpdates.Enabled = true;
            lblUpdateStatus.Text = "Error checking for updates";
            lblUpdateStatus.ForeColor = ModernTheme.DangerColor;
        }
    }
    
    private async void BtnCheckUpdates_Click(object sender, EventArgs e)
    {
        await CheckForUpdatesAsync();
    }
    
    private void BtnDownload_Click(object sender, EventArgs e)
    {
        if (_versionResult != null && !string.IsNullOrEmpty(_versionResult.ReleaseUrl))
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _versionResult.ReleaseUrl,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
                
                ToastNotification.Show("Opening download page in your browser...", ToastNotification.ToastType.Info);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to open download page");
                MessageBox.Show("Failed to open download page. Please visit the releases page manually.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            VersionService.OpenReleasesPage();
        }
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
