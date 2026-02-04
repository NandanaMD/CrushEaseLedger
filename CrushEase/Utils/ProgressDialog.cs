namespace CrushEase.Utils;

/// <summary>
/// Modern progress dialog for long-running operations
/// </summary>
public partial class ProgressDialog : Form
{
    private ProgressBar _progressBar;
    private Label _statusLabel;
    private Label _percentLabel;
    private Button _cancelButton;
    private bool _cancellationRequested;
    
    public bool CancellationRequested => _cancellationRequested;
    
    public ProgressDialog(string title, bool allowCancel = false)
    {
        InitializeComponent();
        this.Text = title;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Size = new Size(450, 180);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ControlBox = false;
        
        // Status label
        _statusLabel = new Label
        {
            Location = new Point(20, 20),
            Size = new Size(400, 30),
            Font = new Font("Segoe UI", 10),
            Text = "Initializing..."
        };
        this.Controls.Add(_statusLabel);
        
        // Progress bar
        _progressBar = new ProgressBar
        {
            Location = new Point(20, 60),
            Size = new Size(350, 25),
            Style = ProgressBarStyle.Continuous,
            Minimum = 0,
            Maximum = 100
        };
        this.Controls.Add(_progressBar);
        
        // Percent label
        _percentLabel = new Label
        {
            Location = new Point(375, 60),
            Size = new Size(50, 25),
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleRight,
            Text = "0%"
        };
        this.Controls.Add(_percentLabel);
        
        // Cancel button (if allowed)
        if (allowCancel)
        {
            _cancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(175, 100),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 9)
            };
            _cancelButton.Click += (s, e) =>
            {
                _cancellationRequested = true;
                _cancelButton.Enabled = false;
                _cancelButton.Text = "Cancelling...";
                _statusLabel.Text = "Cancellation requested...";
            };
            this.Controls.Add(_cancelButton);
        }
        
        ModernTheme.ApplyToForm(this);
    }
    
    /// <summary>
    /// Update progress (0-100)
    /// </summary>
    public void UpdateProgress(int percent, string status)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => UpdateProgress(percent, status)));
            return;
        }
        
        _progressBar.Value = Math.Min(100, Math.Max(0, percent));
        _percentLabel.Text = $"{percent}%";
        _statusLabel.Text = status;
        Application.DoEvents();
    }
    
    /// <summary>
    /// Set status message without updating progress
    /// </summary>
    public void SetStatus(string status)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => SetStatus(status)));
            return;
        }
        
        _statusLabel.Text = status;
        Application.DoEvents();
    }
    
    /// <summary>
    /// Complete and close the dialog
    /// </summary>
    public void Complete()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(Complete));
            return;
        }
        
        _progressBar.Value = 100;
        _percentLabel.Text = "100%";
        _statusLabel.Text = "Completed!";
        Application.DoEvents();
        Thread.Sleep(500);
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.ClientSize = new Size(450, 150);
        this.Name = "ProgressDialog";
        this.ResumeLayout(false);
    }
}
