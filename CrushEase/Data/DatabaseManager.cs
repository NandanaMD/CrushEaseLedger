using System.Data.SQLite;
using CrushEase.Utils;

namespace CrushEase.Data;

/// <summary>
/// Core database manager - handles initialization, connections, and integrity
/// </summary>
public static class DatabaseManager
{
    private static readonly object _lock = new object();
    
    public static void Initialize()
    {
        try
        {
            // Ensure data directory exists
            string dataDir = Path.GetDirectoryName(Config.DatabasePath)!;
            Directory.CreateDirectory(dataDir);
            
            bool isNewDatabase = !File.Exists(Config.DatabasePath);
            
            if (isNewDatabase)
            {
                Logger.LogInfo("Creating new database");
                CreateDatabase();
            }
            else
            {
                Logger.LogInfo("Database found, checking integrity");
                
                // Check integrity
                if (!CheckIntegrity())
                {
                    Logger.LogWarning("Database integrity check failed");
                    throw new Exception("Database is corrupted. Please restore from backup.");
                }
                
                // Enable WAL mode
                EnableWALMode();
                
                // Check schema version
                CheckSchemaVersion();
            }
            
            // Perform automatic backup
            Services.BackupService.AutoBackup();
            
            Logger.LogInfo("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Database initialization failed");
            throw;
        }
    }
    
    public static SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(Config.ConnectionString);
    }
    
    private static void CreateDatabase()
    {
        SQLiteConnection.CreateFile(Config.DatabasePath);
        
        using var connection = GetConnection();
        connection.Open();
        
        // Enable WAL mode
        using (var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", connection))
        {
            cmd.ExecuteNonQuery();
        }
        
        // Create schema
        CreateSchema(connection);
        
        // Insert schema version
        using (var cmd = new SQLiteCommand(
            "INSERT INTO app_metadata (key, value) VALUES ('schema_version', @version)",
            connection))
        {
            cmd.Parameters.AddWithValue("@version", Config.SchemaVersion.ToString());
            cmd.ExecuteNonQuery();
        }
        
        Logger.LogInfo("Database schema created");
    }
    
    private static void CreateSchema(SQLiteConnection connection)
    {
        string schema = @"
            -- Metadata table
            CREATE TABLE app_metadata (
                key TEXT PRIMARY KEY,
                value TEXT
            );
            
            -- Master tables
            CREATE TABLE vehicles (
                vehicle_id INTEGER PRIMARY KEY AUTOINCREMENT,
                vehicle_no TEXT NOT NULL UNIQUE COLLATE NOCASE,
                is_active INTEGER DEFAULT 1,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP
            );
            
            CREATE TABLE vendors (
                vendor_id INTEGER PRIMARY KEY AUTOINCREMENT,
                vendor_name TEXT NOT NULL COLLATE NOCASE,
                contact TEXT,
                notes TEXT,
                is_active INTEGER DEFAULT 1,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP
            );
            
            CREATE TABLE buyers (
                buyer_id INTEGER PRIMARY KEY AUTOINCREMENT,
                buyer_name TEXT NOT NULL COLLATE NOCASE,
                contact TEXT,
                notes TEXT,
                is_active INTEGER DEFAULT 1,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP
            );
            
            CREATE TABLE materials (
                material_id INTEGER PRIMARY KEY AUTOINCREMENT,
                material_name TEXT NOT NULL COLLATE NOCASE,
                unit TEXT DEFAULT 'Ton',
                notes TEXT,
                is_active INTEGER DEFAULT 1,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP
            );
            
            -- Transaction tables
            CREATE TABLE sales (
                sale_id INTEGER PRIMARY KEY AUTOINCREMENT,
                sale_date TEXT NOT NULL,
                vehicle_id INTEGER NOT NULL,
                buyer_id INTEGER NOT NULL,
                material_id INTEGER NOT NULL,
                quantity REAL NOT NULL CHECK(quantity > 0),
                rate REAL NOT NULL CHECK(rate >= 0),
                amount REAL NOT NULL CHECK(amount >= 0),
                created_at TEXT DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (vehicle_id) REFERENCES vehicles(vehicle_id),
                FOREIGN KEY (buyer_id) REFERENCES buyers(buyer_id),
                FOREIGN KEY (material_id) REFERENCES materials(material_id)
            );
            
            CREATE INDEX idx_sales_date ON sales(sale_date);
            CREATE INDEX idx_sales_vehicle ON sales(vehicle_id);
            
            CREATE TABLE purchases (
                purchase_id INTEGER PRIMARY KEY AUTOINCREMENT,
                purchase_date TEXT NOT NULL,
                vehicle_id INTEGER NOT NULL,
                vendor_id INTEGER NOT NULL,
                material_id INTEGER NOT NULL,
                quantity REAL NOT NULL CHECK(quantity > 0),
                rate REAL NOT NULL CHECK(rate >= 0),
                amount REAL NOT NULL CHECK(amount >= 0),
                vendor_site TEXT,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (vehicle_id) REFERENCES vehicles(vehicle_id),
                FOREIGN KEY (vendor_id) REFERENCES vendors(vendor_id),
                FOREIGN KEY (material_id) REFERENCES materials(material_id)
            );
            
            CREATE INDEX idx_purchases_date ON purchases(purchase_date);
            CREATE INDEX idx_purchases_vehicle ON purchases(vehicle_id);
            
            CREATE TABLE maintenance (
                maintenance_id INTEGER PRIMARY KEY AUTOINCREMENT,
                maintenance_date TEXT NOT NULL,
                vehicle_id INTEGER NOT NULL,
                description TEXT NOT NULL,
                amount REAL NOT NULL CHECK(amount >= 0),
                created_at TEXT DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (vehicle_id) REFERENCES vehicles(vehicle_id)
            );
            
            CREATE INDEX idx_maintenance_date ON maintenance(maintenance_date);
            CREATE INDEX idx_maintenance_vehicle ON maintenance(vehicle_id);
            
            -- Company settings table (single row)
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
                updated_at TEXT DEFAULT CURRENT_TIMESTAMP
            );
            
            -- Invoice metadata for tracking generated invoices
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
        
        using var cmd = new SQLiteCommand(schema, connection);
        cmd.ExecuteNonQuery();
    }
    
    private static bool CheckIntegrity()
    {
        try
        {
            using var connection = GetConnection();
            connection.Open();
            
            using var cmd = new SQLiteCommand("PRAGMA integrity_check;", connection);
            var result = cmd.ExecuteScalar()?.ToString();
            
            return result == "ok";
        }
        catch
        {
            return false;
        }
    }
    
    private static void EnableWALMode()
    {
        using var connection = GetConnection();
        connection.Open();
        
        using var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", connection);
        cmd.ExecuteNonQuery();
    }
    
    private static void CheckSchemaVersion()
    {
        using var connection = GetConnection();
        connection.Open();
        
        using var cmd = new SQLiteCommand(
            "SELECT value FROM app_metadata WHERE key = 'schema_version'",
            connection);
        
        var versionStr = cmd.ExecuteScalar()?.ToString();
        if (int.TryParse(versionStr, out int dbVersion))
        {
            if (dbVersion < Config.SchemaVersion)
            {
                Logger.LogInfo($"Schema upgrade needed: {dbVersion} -> {Config.SchemaVersion}");
                UpgradeSchema(connection, dbVersion, Config.SchemaVersion);
            }
        }
    }
    
    private static void UpgradeSchema(SQLiteConnection connection, int fromVersion, int toVersion)
    {
        Logger.LogInfo($"Starting schema upgrade from version {fromVersion} to {toVersion}");
        
        using var transaction = connection.BeginTransaction();
        try
        {
            // Upgrade from version 1 to version 2
            if (fromVersion == 1 && toVersion >= 2)
            {
                Logger.LogInfo("Applying schema changes for version 2 (MT support)");
                
                // Add ConversionFactor_MT_to_CFT to materials table
                using (var cmd = new SQLiteCommand(
                    "ALTER TABLE materials ADD COLUMN conversion_factor_mt_to_cft REAL DEFAULT 1.0",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                // Add InputUnit, InputQuantity, CalculatedCFT to sales table
                using (var cmd = new SQLiteCommand(
                    "ALTER TABLE sales ADD COLUMN input_unit TEXT DEFAULT 'CFT'",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                using (var cmd = new SQLiteCommand(
                    "ALTER TABLE sales ADD COLUMN input_quantity REAL",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                using (var cmd = new SQLiteCommand(
                    "ALTER TABLE sales ADD COLUMN calculated_cft REAL",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                // Add InputUnit, InputQuantity, CalculatedCFT to purchases table
                using (var cmd = new SQLiteCommand(
                    "ALTER TABLE purchases ADD COLUMN input_unit TEXT DEFAULT 'CFT'",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                using (var cmd = new SQLiteCommand(
                    "ALTER TABLE purchases ADD COLUMN input_quantity REAL",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                using (var cmd = new SQLiteCommand(
                    "ALTER TABLE purchases ADD COLUMN calculated_cft REAL",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                // Populate new fields for existing records (backward compatibility)
                // For existing sales: InputUnit=CFT, InputQuantity=existing quantity, CalculatedCFT=existing quantity
                using (var cmd = new SQLiteCommand(
                    @"UPDATE sales 
                      SET input_unit = 'CFT', 
                          input_quantity = quantity, 
                          calculated_cft = quantity 
                      WHERE input_quantity IS NULL OR calculated_cft IS NULL",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                // For existing purchases: InputUnit=CFT, InputQuantity=existing quantity, CalculatedCFT=existing quantity
                using (var cmd = new SQLiteCommand(
                    @"UPDATE purchases 
                      SET input_unit = 'CFT', 
                          input_quantity = quantity, 
                          calculated_cft = quantity 
                      WHERE input_quantity IS NULL OR calculated_cft IS NULL",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                Logger.LogInfo("Schema version 2 changes applied successfully");
            }
            
            // Upgrade from version 2 to version 3
            if (fromVersion <= 2 && toVersion >= 3)
            {
                Logger.LogInfo("Applying schema changes for version 3 (Invoice system)");
                
                // Add company_settings table
                using (var cmd = new SQLiteCommand(
                    @"CREATE TABLE IF NOT EXISTS company_settings (
                        settings_id INTEGER PRIMARY KEY CHECK(settings_id = 1),
                        company_name TEXT NOT NULL DEFAULT '',
                        address TEXT DEFAULT '',
                        phone TEXT DEFAULT '',
                        email TEXT DEFAULT '',
                        gst_number TEXT,
                        website TEXT,
                        logo_image BLOB,
                        invoice_prefix TEXT DEFAULT 'INV',
                        payment_terms TEXT DEFAULT 'Payment Due on Receipt',
                        terms_and_conditions TEXT,
                        updated_at TEXT DEFAULT CURRENT_TIMESTAMP
                    )",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                // Add invoice_metadata table
                using (var cmd = new SQLiteCommand(
                    @"CREATE TABLE IF NOT EXISTS invoice_metadata (
                        invoice_metadata_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        invoice_number TEXT NOT NULL UNIQUE,
                        transaction_type TEXT NOT NULL,
                        transaction_id INTEGER NOT NULL,
                        generated_date TEXT DEFAULT CURRENT_TIMESTAMP,
                        generated_by TEXT DEFAULT 'System',
                        file_path TEXT
                    )",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                // Create indexes
                using (var cmd = new SQLiteCommand(
                    "CREATE INDEX IF NOT EXISTS idx_invoice_number ON invoice_metadata(invoice_number)",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                using (var cmd = new SQLiteCommand(
                    "CREATE INDEX IF NOT EXISTS idx_invoice_transaction ON invoice_metadata(transaction_type, transaction_id)",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                Logger.LogInfo("Schema version 3 changes applied successfully");
            }
            
            // Update schema version
            using (var cmd = new SQLiteCommand(
                "UPDATE app_metadata SET value = @version WHERE key = 'schema_version'",
                connection))
            {
                cmd.Parameters.AddWithValue("@version", toVersion.ToString());
                cmd.ExecuteNonQuery();
            }
            
            transaction.Commit();
            Logger.LogInfo($"Schema upgrade completed successfully to version {toVersion}");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Logger.LogError(ex, "Schema upgrade failed");
            throw new Exception($"Failed to upgrade database schema from {fromVersion} to {toVersion}", ex);
        }
    }
    
    public static void ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
    {
        lock (_lock)
        {
            using var connection = GetConnection();
            connection.Open();
            
            using var transaction = connection.BeginTransaction();
            try
            {
                using var cmd = new SQLiteCommand(sql, connection);
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
                
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
    
    public static T ExecuteScalar<T>(string sql, params SQLiteParameter[] parameters)
    {
        using var connection = GetConnection();
        connection.Open();
        
        using var cmd = new SQLiteCommand(sql, connection);
        cmd.Parameters.AddRange(parameters);
        
        var result = cmd.ExecuteScalar();
        if (result == null || result == DBNull.Value)
            return default(T)!;
        
        // Handle SQLite type conversions
        if (result is long longValue)
        {
            if (typeof(T) == typeof(decimal))
                return (T)(object)Convert.ToDecimal(longValue);
            if (typeof(T) == typeof(int))
                return (T)(object)Convert.ToInt32(longValue);
        }
        
        if (result is double doubleValue && typeof(T) == typeof(decimal))
        {
            return (T)(object)Convert.ToDecimal(doubleValue);
        }
        
        return (T)result;
    }
}
