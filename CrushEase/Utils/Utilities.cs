using Serilog;

namespace CrushEase.Utils;

/// <summary>
/// Centralized logging utility
/// </summary>
public static class Logger
{
    public static void Initialize()
    {
        // Use user's AppData\Local folder for logs
        string logFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CrushEase Ledger",
            "Logs"
        );
        
        Directory.CreateDirectory(logFolder);
        
        string logPath = Path.Combine(logFolder, "crushease.log");
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();
        
        LogInfo("Application started");
    }
    
    public static void LogInfo(string message)
    {
        Log.Information(message);
    }
    
    public static void LogWarning(string message)
    {
        Log.Warning(message);
    }
    
    public static void LogError(Exception ex, string message = "")
    {
        if (string.IsNullOrEmpty(message))
            Log.Error(ex, "Error occurred");
        else
            Log.Error(ex, message);
    }
    
    public static void Close()
    {
        Log.CloseAndFlush();
    }
}

/// <summary>
/// Configuration helper
/// </summary>
public static class Config
{
    // Use user's AppData\Local folder instead of Program Files to avoid permission issues
    private static string AppDataFolder => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CrushEase Ledger"
    );
    
    public static string DatabasePath => Path.Combine(
        AppDataFolder,
        "Data",
        "crushease.db"
    );
    
    public static string BackupFolder => Path.Combine(
        AppDataFolder,
        "Backups"
    );
    
    public static string ConnectionString => $"Data Source={DatabasePath};Version=3;";
    
    public static int SchemaVersion => 1;
}

/// <summary>
/// Validation helpers
/// </summary>
public static class Validator
{
    public static bool IsValidQuantity(decimal quantity, out string? error)
    {
        error = null;
        
        if (quantity <= 0)
        {
            error = "Quantity must be greater than zero";
            return false;
        }
        
        // Check for too many decimal places (max 3 for tonnes)
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(quantity)[3])[2];
        if (decimalPlaces > 3)
        {
            error = "Quantity can have maximum 3 decimal places";
            return false;
        }
        
        return true;
    }
    
    public static bool IsValidRate(decimal rate, out string? error)
    {
        error = null;
        
        if (rate < 0)
        {
            error = "Rate cannot be negative";
            return false;
        }
        
        return true;
    }
    
    public static bool IsValidAmount(decimal amount, out string? error)
    {
        error = null;
        
        if (amount < 0)
        {
            error = "Amount cannot be negative";
            return false;
        }
        
        return true;
    }
    
    public static bool WarnLargeAmount(decimal amount, string type, out string? warning)
    {
        warning = null;
        
        if (type == "Sale" && amount > 100000)
        {
            warning = "Large sale amount detected. Please verify.";
            return true;
        }
        
        if (type == "Maintenance" && amount > 50000)
        {
            warning = "Expensive maintenance detected. Please verify.";
            return true;
        }
        
        return false;
    }
}
