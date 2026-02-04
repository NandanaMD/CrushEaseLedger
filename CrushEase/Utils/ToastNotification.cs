using System.Drawing.Drawing2D;

namespace CrushEase.Utils;

/// <summary>
/// Modern toast notification system with slide-in animation
/// </summary>
public class ToastNotification : Form
{
    private readonly Label _messageLabel;
    private readonly PictureBox _iconBox;
    private readonly Button _closeButton;
    private readonly System.Windows.Forms.Timer _autoCloseTimer;
    private readonly System.Windows.Forms.Timer _animationTimer;
    private int _targetY;
    private int _startY;
    private bool _isShowing = true;
    
    public enum ToastType
    {
        Success,
        Error,
        Warning,
        Info
    }
    
    private ToastNotification(string message, ToastType type, int displayDuration = 4000)
    {
        // Form setup
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.ShowInTaskbar = false;
        this.TopMost = true;
        this.Width = 350;
        this.Height = 80;
        this.BackColor = Color.White;
        
        // Set colors based on type
        Color accentColor;
        string iconText;
        
        switch (type)
        {
            case ToastType.Success:
                accentColor = Color.FromArgb(34, 197, 94); // Green
                iconText = "✓";
                break;
            case ToastType.Error:
                accentColor = Color.FromArgb(239, 68, 68); // Red
                iconText = "✕";
                break;
            case ToastType.Warning:
                accentColor = Color.FromArgb(251, 146, 60); // Orange
                iconText = "⚠";
                break;
            case ToastType.Info:
            default:
                accentColor = Color.FromArgb(59, 130, 246); // Blue
                iconText = "ℹ";
                break;
        }
        
        // Left accent bar
        var accentBar = new Panel
        {
            Width = 5,
            Height = this.Height,
            BackColor = accentColor,
            Location = new Point(0, 0)
        };
        this.Controls.Add(accentBar);
        
        // Icon
        _iconBox = new PictureBox
        {
            Location = new Point(15, 20),
            Size = new Size(40, 40),
            BackColor = Color.Transparent
        };
        
        var iconLabel = new Label
        {
            Text = iconText,
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = accentColor,
            AutoSize = false,
            Size = new Size(40, 40),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent
        };
        _iconBox.Controls.Add(iconLabel);
        this.Controls.Add(_iconBox);
        
        // Message label
        _messageLabel = new Label
        {
            Text = message,
            Location = new Point(65, 15),
            Size = new Size(250, 50),
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(55, 65, 81),
            BackColor = Color.Transparent,
            AutoEllipsis = true
        };
        this.Controls.Add(_messageLabel);
        
        // Close button
        _closeButton = new Button
        {
            Text = "×",
            Location = new Point(315, 5),
            Size = new Size(30, 30),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.Gray,
            BackColor = Color.Transparent,
            Cursor = Cursors.Hand
        };
        _closeButton.FlatAppearance.BorderSize = 0;
        _closeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 244, 246);
        _closeButton.Click += (s, e) => Close();
        this.Controls.Add(_closeButton);
        
        // Position at bottom-right of screen
        var screen = Screen.PrimaryScreen.WorkingArea;
        var offsetFromEdge = GetToastCount() * (this.Height + 10);
        
        this.Left = screen.Right - this.Width - 20;
        _targetY = screen.Bottom - this.Height - 20 - offsetFromEdge;
        _startY = screen.Bottom;
        this.Top = _startY;
        
        // Auto-close timer
        _autoCloseTimer = new System.Windows.Forms.Timer();
        _autoCloseTimer.Interval = displayDuration;
        _autoCloseTimer.Tick += (s, e) =>
        {
            _autoCloseTimer.Stop();
            CloseToast();
        };
        
        // Animation timer
        _animationTimer = new System.Windows.Forms.Timer();
        _animationTimer.Interval = 10;
        _animationTimer.Tick += AnimationTimer_Tick;
        
        // Add shadow effect
        this.Paint += ToastNotification_Paint;
    }
    
    private void ToastNotification_Paint(object? sender, PaintEventArgs e)
    {
        // Draw subtle shadow
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        
        // Draw rounded rectangle background
        var rect = new Rectangle(1, 1, this.Width - 3, this.Height - 3);
        using var path = GetRoundedRectangle(rect, 8);
        using var shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0));
        
        g.TranslateTransform(2, 2);
        g.FillPath(shadowBrush, path);
        g.ResetTransform();
        
        // Draw white background
        using var bgBrush = new SolidBrush(Color.White);
        g.FillPath(bgBrush, path);
        
        // Draw border
        using var borderPen = new Pen(Color.FromArgb(229, 231, 235), 1);
        g.DrawPath(borderPen, path);
    }
    
    private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
        path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
        path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
        path.CloseFigure();
        return path;
    }
    
    private void AnimationTimer_Tick(object? sender, EventArgs e)
    {
        if (_isShowing)
        {
            // Slide in
            if (this.Top > _targetY)
            {
                this.Top -= 15;
                if (this.Top <= _targetY)
                {
                    this.Top = _targetY;
                    _animationTimer.Stop();
                    _autoCloseTimer.Start();
                }
            }
        }
        else
        {
            // Slide out
            this.Opacity -= 0.1;
            if (this.Opacity <= 0)
            {
                _animationTimer.Stop();
                this.Dispose();
            }
        }
    }
    
    private void CloseToast()
    {
        _isShowing = false;
        _autoCloseTimer.Stop();
        _animationTimer.Start();
    }
    
    private static int GetToastCount()
    {
        return Application.OpenForms.OfType<ToastNotification>().Count();
    }
    
    /// <summary>
    /// Show a toast notification
    /// </summary>
    public static void Show(string message, ToastType type = ToastType.Info, int duration = 4000)
    {
        var toast = new ToastNotification(message, type, duration);
        toast._animationTimer.Start();
        toast.Show();
    }
    
    /// <summary>
    /// Show success toast
    /// </summary>
    public static void ShowSuccess(string message, int duration = 3000)
    {
        Show(message, ToastType.Success, duration);
    }
    
    /// <summary>
    /// Show error toast
    /// </summary>
    public static void ShowError(string message, int duration = 5000)
    {
        Show(message, ToastType.Error, duration);
    }
    
    /// <summary>
    /// Show warning toast
    /// </summary>
    public static void ShowWarning(string message, int duration = 4000)
    {
        Show(message, ToastType.Warning, duration);
    }
    
    /// <summary>
    /// Show info toast
    /// </summary>
    public static void ShowInfo(string message, int duration = 3000)
    {
        Show(message, ToastType.Info, duration);
    }
}
