using CrushEase.Data;
using CrushEase.Utils;

namespace CrushEase.Services;

/// <summary>
/// Handles automatic and manual database backups with sequential numbering
/// Sequential backup system:
/// - CrushEaseBackup_1.db, CrushEaseBackup_2.db, etc.
/// - Highest number = latest backup
/// - Keeps last 30 backups
/// - Backups occur on startup (daily) and after transactions
/// </summary>
public static class BackupService
{
    private static DateTime _lastAutoBackup = DateTime.MinValue;
    private static DateTime _lastTransactionBackup = DateTime.MinValue;
    private static readonly TimeSpan MinBackupInterval = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan MinTransactionBackupInterval = TimeSpan.FromMinutes(2);
    private const int MaxBackupsToKeep = 30;
    private static bool _legacyBackupsCleanedUp = false;
    
    public static void AutoBackup()
    {
        try
        {
            // Clean up legacy timestamp-based backups on first run
            if (!_legacyBackupsCleanedUp)
            {
                CleanupLegacyBackups();
                _legacyBackupsCleanedUp = true;
            }
            
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
            
            // Create sequential backup
            CheckpointDatabase();
            CreateSequentialBackup(dbPath, backupFolder);
            _lastAutoBackup = DateTime.Now;
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
            
            // Create sequential backup
            CheckpointDatabase();
            CreateSequentialBackup(dbPath, backupFolder);
            _lastTransactionBackup = DateTime.Now;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Transaction backup failed");
            // Don't throw - backup failure shouldn't prevent data operations
        }
    }
    
    /// <summary>
    /// Creates a new sequential backup and manages cleanup
    /// </summary>
    private static void CreateSequentialBackup(string dbPath, string backupFolder)
    {
        // Get next backup number
        int nextNumber = GetNextBackupNumber(backupFolder);
        
        // Create new backup with sequential number
        string backupPath = Path.Combine(backupFolder, $"CrushEaseBackup_{nextNumber}.db");
        File.Copy(dbPath, backupPath, true);
        Logger.LogInfo($"Sequential backup created: CrushEaseBackup_{nextNumber}.db");
        
        // Cleanup old backups (keep last 30)
        CleanupOldBackups(backupFolder);
    }
    
    /// <summary>
    /// Gets the next backup number in sequence
    /// </summary>
    private static int GetNextBackupNumber(string backupFolder)
    {
        try
        {
            if (!Directory.Exists(backupFolder))
                return 1;
            
            var backupFiles = Directory.GetFiles(backupFolder, "CrushEaseBackup_*.db");
            if (backupFiles.Length == 0)
                return 1;
            
            int maxNumber = 0;
            foreach (var file in backupFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                // Extract number from "CrushEaseBackup_123"
                string numberPart = fileName.Replace("CrushEaseBackup_", "");
                if (int.TryParse(numberPart, out int number))
                {
                    maxNumber = Math.Max(maxNumber, number);
                }
            }
            
            return maxNumber + 1;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting next backup number");
            return 1;
        }
    }
    
    /// <summary>
    /// Removes old sequential backups, keeping only the last 30
    /// </summary>
    private static void CleanupOldBackups(string backupFolder)
    {
        try
        {
            var backupFiles = Directory.GetFiles(backupFolder, "CrushEaseBackup_*.db")
                .Select(f => new { Path = f, Number = ExtractBackupNumber(f) })
                .Where(x => x.Number > 0)
                .OrderByDescending(x => x.Number)
                .Skip(MaxBackupsToKeep);
            
            foreach (var backup in backupFiles)
            {
                File.Delete(backup.Path);
                Logger.LogInfo($"Deleted old backup: {Path.GetFileName(backup.Path)}");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error cleaning up old backups");
        }
    }
    
    /// <summary>
    /// Extracts the backup number from a filename
    /// </summary>
    private static int ExtractBackupNumber(string filePath)
    {
        try
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string numberPart = fileName.Replace("CrushEaseBackup_", "");
            return int.TryParse(numberPart, out int number) ? number : 0;
        }
        catch
        {
            return 0;
        }
    }
    
    /// <summary>
    /// Cleans up old timestamp-based backup files on first run
    /// </summary>
    private static void CleanupLegacyBackups()
    {
        try
        {
            string backupFolder = Config.BackupFolder;
            if (!Directory.Exists(backupFolder))
                return;
            
            // Find all legacy backup files
            var legacyPatterns = new[] { 
                "crushease_daily_*.db", 
                "crushease_weekly_*.db", 
                "crushease_transaction_*.db",
                "pre_restore_*.db"
            };
            
            int deletedCount = 0;
            foreach (var pattern in legacyPatterns)
            {
                var files = Directory.GetFiles(backupFolder, pattern);
                foreach (var file in files)
                {
                    File.Delete(file);
                    deletedCount++;
                }
            }
            
            if (deletedCount > 0)
            {
                Logger.LogInfo($"Cleaned up {deletedCount} legacy timestamp-based backup files");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error cleaning up legacy backups");
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
            
            var files = Directory.GetFiles(backupFolder, "CrushEaseBackup_*.db")
                .Select(f => new { Path = f, Number = ExtractBackupNumber(f), Info = new FileInfo(f) })
                .Where(x => x.Number > 0)
                .OrderByDescending(x => x.Number);
            
            foreach (var file in files)
            {
                backups.Add(new BackupInfo
                {
                    FilePath = file.Path,
                    FileName = file.Info.Name,
                    BackupNumber = file.Number,
                    CreatedDate = file.Info.CreationTime,
                    Size = file.Info.Length
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
    public int BackupNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public long Size { get; set; }
    
    public string DisplayName => $"Backup #{BackupNumber} ({CreatedDate:dd-MMM-yyyy HH:mm}) - {Size / 1024:N0} KB";
}
