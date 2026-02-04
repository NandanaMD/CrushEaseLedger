namespace CrushEase.Utils;

/// <summary>
/// Manages font scaling across the application for accessibility
/// </summary>
public static class FontScaleManager
{
    private static float _currentScale = 1.0f;
    
    /// <summary>
    /// Available font scales
    /// </summary>
    public static readonly float[] AvailableScales = { 0.8f, 0.9f, 1.0f, 1.1f, 1.2f, 1.4f };
    
    /// <summary>
    /// Scale names for menu display
    /// </summary>
    public static readonly Dictionary<float, string> ScaleNames = new()
    {
        { 0.8f, "Small" },
        { 0.9f, "Smaller" },
        { 1.0f, "Normal (Default)" },
        { 1.1f, "Larger" },
        { 1.2f, "Large" },
        { 1.4f, "Extra Large" }
    };
    
    public static float CurrentScale => _currentScale;
    
    /// <summary>
    /// Load font scale from settings
    /// </summary>
    public static void Initialize()
    {
        try
        {
            var settings = Data.CompanySettingsRepository.Get();
            if (settings != null && settings.FontSizeScale > 0)
            {
                _currentScale = settings.FontSizeScale;
                Logger.LogInfo($"Font scale initialized to {_currentScale}");
            }
            else
            {
                Logger.LogInfo("No font scale setting found, using default (1.0)");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load font scale settings, using default");
            _currentScale = 1.0f;
        }
    }
    
    /// <summary>
    /// Apply font scale to a form and all its controls
    /// </summary>
    public static void ApplyToForm(Form form)
    {
        if (_currentScale == 1.0f) return;
        
        ApplyToControl(form);
    }
    
    private static void ApplyToControl(Control control)
    {
        // Scale the control's font
        if (control.Font != null)
        {
            float newSize = control.Font.Size * _currentScale;
            control.Font = new Font(control.Font.FontFamily, newSize, control.Font.Style);
        }
        
        // Recursively apply to child controls
        foreach (Control child in control.Controls)
        {
            ApplyToControl(child);
        }
    }
    
    /// <summary>
    /// Change font scale
    /// </summary>
    public static void SetScale(float scale)
    {
        if (!AvailableScales.Contains(scale))
        {
            throw new ArgumentException($"Invalid scale: {scale}. Must be one of: {string.Join(", ", AvailableScales)}");
        }
        
        _currentScale = scale;
        
        // Save to settings
        var settings = Data.CompanySettingsRepository.Get();
        if (settings != null)
        {
            settings.FontSizeScale = scale;
            Data.CompanySettingsRepository.Save(settings);
        }
        else
        {
            // Create default settings
            var newSettings = new Models.CompanySettings
            {
                CompanyName = "",
                FontSizeScale = scale
            };
            Data.CompanySettingsRepository.Save(newSettings);
        }
        
        Logger.LogInfo($"Font scale changed to {scale} ({ScaleNames[scale]})");
    }
    
    /// <summary>
    /// Increase font size
    /// </summary>
    public static void IncreaseSize()
    {
        int currentIndex = Array.IndexOf(AvailableScales, _currentScale);
        if (currentIndex < AvailableScales.Length - 1)
        {
            SetScale(AvailableScales[currentIndex + 1]);
            ToastNotification.ShowInfo($"Font size: {ScaleNames[_currentScale]}");
        }
    }
    
    /// <summary>
    /// Decrease font size
    /// </summary>
    public static void DecreaseSize()
    {
        int currentIndex = Array.IndexOf(AvailableScales, _currentScale);
        if (currentIndex > 0)
        {
            SetScale(AvailableScales[currentIndex - 1]);
            ToastNotification.ShowInfo($"Font size: {ScaleNames[_currentScale]}");
        }
    }
    
    /// <summary>
    /// Reset to default size
    /// </summary>
    public static void ResetSize()
    {
        SetScale(1.0f);
        ToastNotification.ShowInfo("Font size reset to normal");
    }
}
