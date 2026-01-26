using CrushEase.Data;
using CrushEase.Utils;

namespace CrushEase.Services;

/// <summary>
/// Handles automatic and manual database backups
/// Auto backups occur:
/// - On application startup (daily/weekly)
/// - Every 5 minutes during runtime
/// - After each transaction/master data change
/// - On application exit
/// </summary>
public static class BackupService
{
    private static DateTime _lastAutoBackup = DateTime.MinValue;
    private static DateTime _lastTransactionBackup = DateTime.MinValue;
    private static readonly TimeSpan MinBackupInterval = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan MinTransactionBackupInterval = TimeSpan.FromMinutes(2);
    
    public static void AutoBackup()
    {
        try
        {
            // Prevent too frequent backups (minimum 30 minutes between scheduled auto backups)
            if (DateTime.Now - _lastAutoBackup < MinBackupInterval)
            {
                Logger.LogInfo("Skipping auto backup - too soon since last backup");
                return;
            }
            
            string dbPath = Config.DatabasePath;
            if (!File.Exists(dbPath))
                return;
            
            string backupFolder = Config.BackupFolder;
            Directory.CreateDirectory(backupFolder);
            
            // Daily backup (keep last 7 days)
            string dailyBackup = Path.Combine(backupFolder, $"crushease_daily_{DateTime.Now:yyyyMMdd}.db");
            if (!File.Exists(dailyBackup))
            {                // Checkpoint WAL to ensure all data is in the main database file
                CheckpointDatabase();
                                File.Copy(dbPath, dailyBackup, true);
                Logger.LogInfo($"Daily backup created: {dailyBackup}");
                _lastAutoBackup = DateTime.Now;
            }
            
            // Cleanup old daily backups (keep last 14 days for better safety)
            var oldDailyBackups = Directory.GetFiles(backupFolder, "crushease_daily_*.db")
                                          .OrderByDescending(f => f)
                                          .Skip(14);
            foreach (var file in oldDailyBackups)
            {
                File.Delete(file);
                Logger.LogInfo($"Deleted old daily backup: {file}");
            }
            
            // Weekly backup (create on Sundays, keep last 8 weeks)
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                string weeklyBackup = Path.Combine(backupFolder, $"crushease_weekly_{DateTime.Now:yyyyMMdd}.db");
                if (!File.Exists(weeklyBackup))
                {// Checkpoint WAL to ensure all data is in the main database file
                    CheckpointDatabase();
                    
                    
                    File.Copy(dbPath, weeklyBackup, true);
                    Logger.LogInfo($"Weekly backup created: {weeklyBackup}");
                    _lastAutoBackup = DateTime.Now;
                }
                
                var oldWeeklyBackups = Directory.GetFiles(backupFolder, "crushease_weekly_*.db")
                                               .OrderByDescending(f => f)
                                               .Skip(8);
                foreach (var file in oldWeeklyBackups)
                {
                    File.Delete(file);
                    Logger.LogInfo($"Deleted old weekly backup: {file}");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Auto backup failed");
            // Don't throw - backup failure shouldn't prevent app from starting
        }
    }
    
    public static bool ManualBackup(string destinationPath)
    {
        try
        {
            string dbPath = Config.DatabasePath;
            if (!File.Exists(dbPath))
                return false;
            
            // Checkpoint WAL to ensure all data is in the main database file
            CheckpointDatabase();
            
            File.Copy(dbPath, destinationPath, true);
            Logger.LogInfo($"Manual backup created: {destinationPath}");
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Manual backup failed");
            return false;
        }
    }
    
    /// <summary>
    /// Backup triggered after transaction/master data changes
    /// Throttled to prevent excessive backups (minimum 2 minutes between)
    /// </summary>
    public static void BackupAfterTransaction()
    {
        try
        {
            // Prevent too frequent backups from rapid data entry
            if (DateTime.Now - _lastTransactionBackup < MinTransactionBackupInterval)
            {
                return; // Skip silently
            }
            
            string dbPath = Config.DatabasePath;
            if (!File.Exists(dbPath))
                return;
            
            string backupFolder = Config.BackupFolder;
            Directory.CreateDirectory(backupFolder);
            
            // Checkpoint WAL to ensure all data is in the main database file
            CheckpointDatabase();
            
            
            // Transaction backup (keep last 10)
            string transactionBackup = Path.Combine(backupFolder, $"crushease_transaction_{DateTime.Now:yyyyMMdd_HHmmss}.db");
            File.Copy(dbPath, transactionBackup, true);
            Logger.LogInfo($"Transaction backup created: {transactionBackup}");
            _lastTransactionBackup = DateTime.Now;
            
            // Cleanup old transaction backups (keep last 10)
            var oldTransactionBackups = Directory.GetFiles(backupFolder, "crushease_transaction_*.db")
                                          .OrderByDescending(f => f)
                                          .Skip(10);
            foreach (var file in oldTransactionBackups)
            {
                File.Delete(file);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Transaction backup failed");
            // Don't throw - backup failure shouldn't prevent data operations
        }
    }
    
    public static bool RestoreBackup(string backupPath)
    {
        try
        {
            string dbPath = Config.DatabasePath;
            
            // Create a safety backup of current database before restore
            string safetyBackup = Path.Combine(
                Config.BackupFolder,
                $"pre_restore_{DateTime.Now:yyyyMMdd_HHmmss}.db"
            );
            if (File.Exists(dbPath))
            {
                File.Copy(dbPath, safetyBackup, true);
                Logger.LogInfo($"Safety backup created before restore: {safetyBackup}");
            }
            
            // Clear all SQLite connection pools to release file locks
            System.Data.SQLite.SQLiteConnection.ClearAllPools();
            
            // Force garbage collection to close any open connections
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            // Wait for file handles to be released
            System.Threading.Thread.Sleep(500);
            
            // Delete WAL files if they exist (SQLite Write-Ahead Log files)
            string walFile = dbPath + "-wal";
            string shmFile = dbPath + "-shm";
            
            int retries = 0;
            while (retries < 3)
            {
                try
                {
                    if (File.Exists(walFile))
                        File.Delete(walFile);
                    if (File.Exists(shmFile))
                        File.Delete(shmFile);
                    break;
                }
                catch
                {
                    retries++;
                    if (retries < 3)
                        System.Threading.Thread.Sleep(200);
                }
            }
            
            // Restore from backup with retry logic
            retries = 0;
            while (retries < 3)
            {
                try
                {
                    File.Copy(backupPath, dbPath, true);
                    Logger.LogInfo($"Database restored from: {backupPath}");
                    return true;
                }
                catch (IOException) when (retries < 2)
                {
                    retries++;
                    System.Threading.Thread.Sleep(500);
                }
            }
            
            throw new Exception("Failed to restore database after multiple attempts");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Restore failed");
            return false;
        }
    }
    
    public static List<BackupInfo> GetAvailableBackups()
    {
        var backups = new List<BackupInfo>();
        
        try
        {
            string backupFolder = Config.BackupFolder;
            if (!Directory.Exists(backupFolder))
                return backups;
            
            var files = Directory.GetFiles(backupFolder, "crushease_*.db")
                                .OrderByDescending(f => f);
            
            foreach (var file in files)
            {
                var info = new FileInfo(file);
                backups.Add(new BackupInfo
                {
                    FilePath = file,
                    FileName = info.Name,
                    CreatedDate = info.CreationTime,
                    Size = info.Length
                });
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to retrieve backup list");
        }
        
        return backups;
    }
    
    /// <summary>
    /// Checkpoint the WAL file to write all pending data to the main database file
    /// </summary>
    private static void CheckpointDatabase()
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            connection.Open();
            using var cmd = new System.Data.SQLite.SQLiteCommand("PRAGMA wal_checkpoint(TRUNCATE);", connection);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "WAL checkpoint failed");
        }
    }
}

public class BackupInfo
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public long Size { get; set; }
    
    public string DisplayName => $"{FileName} ({CreatedDate:dd-MMM-yyyy HH:mm}) - {Size / 1024:N0} KB";
}
