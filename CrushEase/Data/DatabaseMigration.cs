using System.Data.SQLite;
using CrushEase.Utils;

namespace CrushEase.Data;

/// <summary>
/// Handles safe database schema migrations with backward compatibility
/// </summary>
public static class DatabaseMigration
{
    /// <summary>
    /// Current schema version
    /// </summary>
    public const int CURRENT_SCHEMA_VERSION = 3;
    
    /// <summary>
    /// Apply all pending migrations to bring database up to current version
    /// </summary>
    public static void ApplyMigrations(SQLiteConnection connection)
    {
        try
        {
            int currentVersion = GetCurrentSchemaVersion(connection);
            Logger.LogInfo($"Current database schema version: {currentVersion}");
            
            // ALWAYS verify critical tables exist, even if version is current
            Logger.LogInfo("Verifying all required tables exist...");
            EnsureAllRequiredTablesExist(connection);
            
            if (currentVersion >= CURRENT_SCHEMA_VERSION)
            {
                Logger.LogInfo($"Database is up to date (version {currentVersion}/{CURRENT_SCHEMA_VERSION})");
                return;
            }
            
            Logger.LogInfo($"⚠️ MIGRATION REQUIRED: Upgrading database from version {currentVersion} to {CURRENT_SCHEMA_VERSION}");
            
            using var transaction = connection.BeginTransaction();
            try
            {
                // Apply migrations incrementally
                for (int version = currentVersion + 1; version <= CURRENT_SCHEMA_VERSION; version++)
                {
                    Logger.LogInfo($"Applying migration to version {version}...");
                    ApplyMigration(connection, version);
                }
                
                // Update schema version
                UpdateSchemaVersion(connection, CURRENT_SCHEMA_VERSION);
                
                transaction.Commit();
                Logger.LogInfo($"✓ Database migration completed successfully to version {CURRENT_SCHEMA_VERSION}");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Logger.LogError(ex, "Database migration failed - transaction rolled back");
                throw;
            }
            
            // Verify migration succeeded AFTER commit (outside transaction)
            if (CURRENT_SCHEMA_VERSION >= 3)
            {
                if (ColumnExists(connection, "sales", "is_deleted"))
                {
                    Logger.LogInfo("✓ Migration verification: is_deleted column found");
                }
                else
                {
                    Logger.LogError(null, "✗ Migration verification FAILED: is_deleted column not found!");
                    throw new Exception("Migration appeared to succeed but is_deleted column is missing! Database may be corrupted.");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "CRITICAL: Migration process failed!");
            throw;
        }
    }
    
    private static int GetCurrentSchemaVersion(SQLiteConnection connection)
    {
        try
        {
            // Check if app_metadata table exists
            using var checkCmd = new SQLiteCommand(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='app_metadata'",
                connection);
            
            var tableExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
            
            if (!tableExists)
            {
                // Old database without migration system - detect actual version by inspecting schema
                Logger.LogInfo("No app_metadata table found - detecting database version from schema");
                int detectedVersion = DetectSchemaVersion(connection);
                
                // Create app_metadata table for future migrations
                using var createCmd = new SQLiteCommand(
                    "CREATE TABLE app_metadata (key TEXT PRIMARY KEY, value TEXT)",
                    connection);
                createCmd.ExecuteNonQuery();
                
                // Insert detected version
                using var insertCmd = new SQLiteCommand(
                    "INSERT INTO app_metadata (key, value) VALUES ('schema_version', @version)",
                    connection);
                insertCmd.Parameters.AddWithValue("@version", detectedVersion.ToString());
                insertCmd.ExecuteNonQuery();
                
                Logger.LogInfo($"Created app_metadata table, detected database at version {detectedVersion}");
                return detectedVersion;
            }
            
            using var cmd = new SQLiteCommand(
                "SELECT value FROM app_metadata WHERE key = 'schema_version'",
                connection);
            
            var result = cmd.ExecuteScalar()?.ToString();
            if (int.TryParse(result, out int version))
            {
                return version;
            }
            
            // If schema_version key doesn't exist, detect from schema
            int detected = DetectSchemaVersion(connection);
            Logger.LogInfo($"No schema_version in app_metadata, detected version {detected}");
            return detected;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting schema version");
            // On any error, try to detect from schema structure
            return DetectSchemaVersion(connection);
        }
    }
    
    /// <summary>
    /// Detect database schema version by checking which columns/tables exist
    /// </summary>
    private static int DetectSchemaVersion(SQLiteConnection connection)
    {
        try
        {
            // Check for version 3 features (is_deleted column)
            if (TableExists(connection, "sales") && ColumnExists(connection, "sales", "is_deleted"))
            {
                Logger.LogInfo("Detected version 3: is_deleted column exists");
                return 3;
            }
            
            // Check for version 2 features (MT/CFT conversion columns)
            if (TableExists(connection, "materials") && ColumnExists(connection, "materials", "conversion_factor_mt_to_cft"))
            {
                Logger.LogInfo("Detected version 2: conversion_factor_mt_to_cft column exists");
                return 2;
            }
            
            // Default to version 1 (base schema)
            Logger.LogInfo("Detected version 1: base schema");
            return 1;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error detecting schema version");
            // If we can't detect, assume oldest version to trigger all migrations
            return 1;
        }
    }
    
    private static void UpdateSchemaVersion(SQLiteConnection connection, int version)
    {
        // Ensure app_metadata table exists
        using (var createTable = new SQLiteCommand(
            "CREATE TABLE IF NOT EXISTS app_metadata (key TEXT PRIMARY KEY, value TEXT)",
            connection))
        {
            createTable.ExecuteNonQuery();
        }
        
        using var cmd = new SQLiteCommand(
            "INSERT OR REPLACE INTO app_metadata (key, value) VALUES ('schema_version', @version)",
            connection);
        cmd.Parameters.AddWithValue("@version", version.ToString());
        cmd.ExecuteNonQuery();
    }
    
    private static void ApplyMigration(SQLiteConnection connection, int version)
    {
        Logger.LogInfo($"Applying migration to version {version}");
        
        switch (version)
        {
            case 1:
                // Version 1 is the base schema - nothing to migrate
                Logger.LogInfo("Database at version 1 (base schema)");
                break;
            case 2:
                MigrateToVersion2(connection);
                break;
            case 3:
                MigrateToVersion3(connection);
                break;
            default:
                throw new Exception($"Unknown migration version: {version}");
        }
    }
    
    /// <summary>
    /// Version 2: MT/CFT support (already implemented)
    /// </summary>
    private static void MigrateToVersion2(SQLiteConnection connection)
    {
        Logger.LogInfo("Migrating to version 2: Adding MT/CFT support");
        
        // Ensure company_settings table exists (for old databases that might not have it)
        EnsureCompanySettingsDefaultRow(connection);
        
        // Check if columns already exist (safe check)
        if (!ColumnExists(connection, "materials", "conversion_factor_mt_to_cft"))
        {
            ExecuteNonQuery(connection, 
                "ALTER TABLE materials ADD COLUMN conversion_factor_mt_to_cft REAL DEFAULT 1.0");
        }
        
        if (!ColumnExists(connection, "sales", "input_unit"))
        {
            ExecuteNonQuery(connection, 
                "ALTER TABLE sales ADD COLUMN input_unit TEXT DEFAULT 'CFT'");
            ExecuteNonQuery(connection, 
                "ALTER TABLE sales ADD COLUMN input_quantity REAL");
            ExecuteNonQuery(connection, 
                "ALTER TABLE sales ADD COLUMN calculated_cft REAL");
        }
        
        if (!ColumnExists(connection, "purchases", "input_unit"))
        {
            ExecuteNonQuery(connection, 
                "ALTER TABLE purchases ADD COLUMN input_unit TEXT DEFAULT 'CFT'");
            ExecuteNonQuery(connection, 
                "ALTER TABLE purchases ADD COLUMN input_quantity REAL");
            ExecuteNonQuery(connection, 
                "ALTER TABLE purchases ADD COLUMN calculated_cft REAL");
        }
        
        Logger.LogInfo("Version 2 migration completed");
    }
    
    /// <summary>
    /// Version 3: Soft delete, attachments, font settings
    /// </summary>
    private static void MigrateToVersion3(SQLiteConnection connection)
    {
        Logger.LogInfo("Migrating to version 3: Adding soft delete, attachments, font settings");
        
        // Add soft delete columns to all tables
        AddSoftDeleteColumns(connection, "sales");
        AddSoftDeleteColumns(connection, "purchases");
        AddSoftDeleteColumns(connection, "maintenance");
        AddSoftDeleteColumns(connection, "vehicles");
        AddSoftDeleteColumns(connection, "buyers");
        AddSoftDeleteColumns(connection, "vendors");
        AddSoftDeleteColumns(connection, "materials");
        
        // Create attachments table
        if (!TableExists(connection, "attachments"))
        {
            string createAttachmentsTable = @"
                CREATE TABLE attachments (
                    attachment_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    transaction_type TEXT NOT NULL,
                    transaction_id INTEGER NOT NULL,
                    file_name TEXT NOT NULL,
                    file_path TEXT NOT NULL,
                    file_size INTEGER NOT NULL,
                    mime_type TEXT,
                    uploaded_at TEXT DEFAULT CURRENT_TIMESTAMP
                );
                
                CREATE INDEX idx_attachments_transaction ON attachments(transaction_type, transaction_id);
            ";
            ExecuteNonQuery(connection, createAttachmentsTable);
            Logger.LogInfo("Created attachments table");
        }
        
        // Create invoice_metadata table (critical for invoice generation)
        if (!TableExists(connection, "invoice_metadata"))
        {
            string createInvoiceMetadataTable = @"
                CREATE TABLE invoice_metadata (
                    invoice_metadata_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    invoice_number TEXT NOT NULL UNIQUE,
                    transaction_type TEXT NOT NULL,
                    transaction_id INTEGER NOT NULL,
                    generated_date TEXT DEFAULT CURRENT_TIMESTAMP,
                    generated_by TEXT DEFAULT 'System',
                    file_path TEXT
                );
                
                CREATE INDEX idx_invoice_number ON invoice_metadata(invoice_number);
                CREATE INDEX idx_invoice_transaction ON invoice_metadata(transaction_type, transaction_id);
            ";
            ExecuteNonQuery(connection, createInvoiceMetadataTable);
            Logger.LogInfo("Created invoice_metadata table");
        }
        
        // Add font size setting to company_settings
        if (!ColumnExists(connection, "company_settings", "font_size_scale"))
        {
            ExecuteNonQuery(connection, 
                "ALTER TABLE company_settings ADD COLUMN font_size_scale REAL DEFAULT 1.0");
        }
        
        // Ensure company_settings table has a default row (fix for databases that were created without it)
        EnsureCompanySettingsDefaultRow(connection);
        
        Logger.LogInfo("Version 3 migration completed");
    }
    
    private static void AddSoftDeleteColumns(SQLiteConnection connection, string tableName)
    {
        try
        {
            // Check if table exists first
            if (!TableExists(connection, tableName))
            {
                Logger.LogWarning($"Table {tableName} does not exist, skipping soft delete columns");
                return;
            }
            
            if (!ColumnExists(connection, tableName, "is_deleted"))
            {
                ExecuteNonQuery(connection, 
                    $"ALTER TABLE {tableName} ADD COLUMN is_deleted INTEGER DEFAULT 0");
                Logger.LogInfo($"Added is_deleted column to {tableName}");
                
                // Verify the column was added
                if (!ColumnExists(connection, tableName, "is_deleted"))
                {
                    throw new Exception($"Failed to add is_deleted column to {tableName}");
                }
            }
            else
            {
                Logger.LogInfo($"Column is_deleted already exists in {tableName}");
            }
            
            if (!ColumnExists(connection, tableName, "deleted_at"))
            {
                ExecuteNonQuery(connection, 
                    $"ALTER TABLE {tableName} ADD COLUMN deleted_at TEXT");
                Logger.LogInfo($"Added deleted_at column to {tableName}");
                
                // Verify the column was added
                if (!ColumnExists(connection, tableName, "deleted_at"))
                {
                    throw new Exception($"Failed to add deleted_at column to {tableName}");
                }
            }
            else
            {
                Logger.LogInfo($"Column deleted_at already exists in {tableName}");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error adding soft delete columns to {tableName}");
            throw;
        }
    }
    
    private static bool TableExists(SQLiteConnection connection, string tableName)
    {
        using var cmd = new SQLiteCommand(
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@tableName",
            connection);
        cmd.Parameters.AddWithValue("@tableName", tableName);
        
        var count = Convert.ToInt32(cmd.ExecuteScalar());
        return count > 0;
    }
    
    private static void EnsureCompanySettingsDefaultRow(SQLiteConnection connection)
    {
        // First, ensure the company_settings table exists
        if (!TableExists(connection, "company_settings"))
        {
            Logger.LogInfo("Company settings table does not exist - creating it now");
            string createCompanySettingsTable = @"
                CREATE TABLE company_settings (
                    settings_id INTEGER PRIMARY KEY CHECK(settings_id = 1),
                    company_name TEXT NOT NULL,
                    address TEXT,
                    phone TEXT,
                    email TEXT,
                    gst_number TEXT,
                    website TEXT,
                    logo_image BLOB,
                    invoice_prefix TEXT DEFAULT 'INV',
                    payment_terms TEXT DEFAULT 'Payment Due on Receipt',
                    terms_and_conditions TEXT,
                    updated_at TEXT DEFAULT CURRENT_TIMESTAMP,
                    font_size_scale REAL DEFAULT 1.0
                );
            ";
            ExecuteNonQuery(connection, createCompanySettingsTable);
            Logger.LogInfo("Company settings table created");
        }
        
        // Check if company_settings has any rows
        using var checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM company_settings", connection);
        var rowCount = Convert.ToInt32(checkCmd.ExecuteScalar());
        
        if (rowCount == 0)
        {
            Logger.LogInfo("Company settings table is empty - inserting default row");
            using var insertCmd = new SQLiteCommand(@"
                INSERT INTO company_settings 
                (settings_id, company_name, address, phone, email, invoice_prefix, payment_terms, font_size_scale, updated_at)
                VALUES 
                (1, '', '', '', '', 'INV', 'Payment Due on Receipt', 1.0, datetime('now'))",
                connection);
            insertCmd.ExecuteNonQuery();
            Logger.LogInfo("Default company settings row created");
        }
    }
    
    /// <summary>
    /// Ensures ALL required tables exist regardless of schema version
    /// This is a safety net for databases that may have been partially migrated or corrupted
    /// </summary>
    private static void EnsureAllRequiredTablesExist(SQLiteConnection connection)
    {
        try
        {
            // Ensure company_settings exists
            if (!TableExists(connection, "company_settings"))
            {
                Logger.LogInfo("⚠️ CRITICAL: company_settings table missing - creating now");
                EnsureCompanySettingsDefaultRow(connection);
            }
            
            // Ensure invoice_metadata exists
            if (!TableExists(connection, "invoice_metadata"))
            {
                Logger.LogInfo("⚠️ CRITICAL: invoice_metadata table missing - creating now");
                string createInvoiceMetadataTable = @"
                    CREATE TABLE invoice_metadata (
                        invoice_metadata_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        invoice_number TEXT NOT NULL UNIQUE,
                        transaction_type TEXT NOT NULL,
                        transaction_id INTEGER NOT NULL,
                        generated_date TEXT DEFAULT CURRENT_TIMESTAMP,
                        generated_by TEXT DEFAULT 'System',
                        file_path TEXT
                    );
                    
                    CREATE INDEX idx_invoice_number ON invoice_metadata(invoice_number);
                    CREATE INDEX idx_invoice_transaction ON invoice_metadata(transaction_type, transaction_id);
                ";
                ExecuteNonQuery(connection, createInvoiceMetadataTable);
                Logger.LogInfo("✓ invoice_metadata table created");
            }
            
            // Ensure attachments exists
            if (!TableExists(connection, "attachments"))
            {
                Logger.LogInfo("⚠️ CRITICAL: attachments table missing - creating now");
                string createAttachmentsTable = @"
                    CREATE TABLE attachments (
                        attachment_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        transaction_type TEXT NOT NULL,
                        transaction_id INTEGER NOT NULL,
                        file_name TEXT NOT NULL,
                        file_path TEXT NOT NULL,
                        file_size INTEGER NOT NULL,
                        mime_type TEXT,
                        uploaded_at TEXT DEFAULT CURRENT_TIMESTAMP
                    );
                    
                    CREATE INDEX idx_attachments_transaction ON attachments(transaction_type, transaction_id);
                ";
                ExecuteNonQuery(connection, createAttachmentsTable);
                Logger.LogInfo("✓ attachments table created");
            }
            
            Logger.LogInfo("✓ All required tables verified");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to ensure required tables exist");
            throw;
        }
    }
    
    private static bool ColumnExists(SQLiteConnection connection, string tableName, string columnName)
    {
        using var cmd = new SQLiteCommand($"PRAGMA table_info({tableName})", connection);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            if (reader["name"].ToString()?.Equals(columnName, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
        }
        
        return false;
    }
    
    private static void ExecuteNonQuery(SQLiteConnection connection, string sql)
    {
        using var cmd = new SQLiteCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }
}
