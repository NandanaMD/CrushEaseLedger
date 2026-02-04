using System.Data.SQLite;
using CrushEase.Models;

namespace CrushEase.Data;

/// <summary>
/// Repository for Vehicle master operations
/// </summary>
public static class VehicleRepository
{
    public static List<Vehicle> GetAll(bool activeOnly = true)
    {
        var vehicles = new List<Vehicle>();
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = "SELECT * FROM vehicles WHERE is_deleted = 0" + (activeOnly ? " AND is_active = 1" : "") + " ORDER BY vehicle_no";
        using var cmd = new SQLiteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            vehicles.Add(new Vehicle
            {
                VehicleId = reader.GetInt32(0),
                VehicleNo = reader.GetString(1),
                IsActive = reader.GetInt32(2) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(3))
            });
        }
        
        return vehicles;
    }
    
    public static Vehicle? GetById(int id)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        using var cmd = new SQLiteCommand("SELECT * FROM vehicles WHERE vehicle_id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            return new Vehicle
            {
                VehicleId = reader.GetInt32(0),
                VehicleNo = reader.GetString(1),
                IsActive = reader.GetInt32(2) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(3))
            };
        }
        
        return null;
    }
    
    public static int Insert(Vehicle vehicle)
    {
        string sql = "INSERT INTO vehicles (vehicle_no, is_active) VALUES (@vehicleNo, @isActive); SELECT last_insert_rowid();";
        
        int result = DatabaseManager.ExecuteScalar<int>(sql,
            new SQLiteParameter("@vehicleNo", vehicle.VehicleNo),
            new SQLiteParameter("@isActive", vehicle.IsActive ? 1 : 0));
        
        Services.BackupService.BackupAfterTransaction();
        return result;
    }
    
    public static void Update(Vehicle vehicle)
    {
        string sql = "UPDATE vehicles SET vehicle_no = @vehicleNo, is_active = @isActive WHERE vehicle_id = @id";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@vehicleNo", vehicle.VehicleNo),
            new SQLiteParameter("@isActive", vehicle.IsActive ? 1 : 0),
            new SQLiteParameter("@id", vehicle.VehicleId));
        
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static void Delete(int id)
    {
        // Soft delete - mark as deleted
        string sql = "UPDATE vehicles SET is_deleted = 1, deleted_at = @deletedAt WHERE vehicle_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, 
            new SQLiteParameter("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static bool Exists(string vehicleNo, int? excludeId = null)
    {
        string sql = "SELECT COUNT(*) FROM vehicles WHERE LOWER(vehicle_no) = LOWER(@vehicleNo) AND is_deleted = 0";
        if (excludeId.HasValue)
            sql += " AND vehicle_id != @excludeId";
        
        var parameters = excludeId.HasValue 
            ? new[] { new SQLiteParameter("@vehicleNo", vehicleNo), new SQLiteParameter("@excludeId", excludeId.Value) }
            : new[] { new SQLiteParameter("@vehicleNo", vehicleNo) };
        
        return DatabaseManager.ExecuteScalar<long>(sql, parameters) > 0;
    }
}

/// <summary>
/// Repository for Vendor master operations
/// </summary>
public static class VendorRepository
{
    public static List<Vendor> GetAll(bool activeOnly = true)
    {
        var vendors = new List<Vendor>();
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = "SELECT * FROM vendors WHERE is_deleted = 0" + (activeOnly ? " AND is_active = 1" : "") + " ORDER BY vendor_name";
        using var cmd = new SQLiteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            vendors.Add(new Vendor
            {
                VendorId = reader.GetInt32(0),
                VendorName = reader.GetString(1),
                Contact = reader.IsDBNull(2) ? null : reader.GetString(2),
                Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
                IsActive = reader.GetInt32(4) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(5))
            });
        }
        
        return vendors;
    }
    
    public static Vendor? GetById(int id)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        using var cmd = new SQLiteCommand("SELECT * FROM vendors WHERE vendor_id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            return new Vendor
            {
                VendorId = reader.GetInt32(0),
                VendorName = reader.GetString(1),
                Contact = reader.IsDBNull(2) ? null : reader.GetString(2),
                Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
                IsActive = reader.GetInt32(4) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(5))
            };
        }
        
        return null;
    }
    
    public static int Insert(Vendor vendor)
    {
        string sql = "INSERT INTO vendors (vendor_name, contact, notes, is_active) VALUES (@name, @contact, @notes, @isActive); SELECT last_insert_rowid();";
        
        int result = DatabaseManager.ExecuteScalar<int>(sql,
            new SQLiteParameter("@name", vendor.VendorName),
            new SQLiteParameter("@contact", (object?)vendor.Contact ?? DBNull.Value),
            new SQLiteParameter("@notes", (object?)vendor.Notes ?? DBNull.Value),
            new SQLiteParameter("@isActive", vendor.IsActive ? 1 : 0));
        
        Services.BackupService.BackupAfterTransaction();
        return result;
    }
    
    public static void Update(Vendor vendor)
    {
        string sql = "UPDATE vendors SET vendor_name = @name, contact = @contact, notes = @notes, is_active = @isActive WHERE vendor_id = @id";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@name", vendor.VendorName),
            new SQLiteParameter("@contact", (object?)vendor.Contact ?? DBNull.Value),
            new SQLiteParameter("@notes", (object?)vendor.Notes ?? DBNull.Value),
            new SQLiteParameter("@isActive", vendor.IsActive ? 1 : 0),
            new SQLiteParameter("@id", vendor.VendorId));
        
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static void Delete(int id)
    {
        string sql = "UPDATE vendors SET is_deleted = 1, deleted_at = @deletedAt WHERE vendor_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, 
            new SQLiteParameter("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static bool Exists(string vendorName, int? excludeId = null)
    {
        string sql = "SELECT COUNT(*) FROM vendors WHERE LOWER(vendor_name) = LOWER(@vendorName) AND is_deleted = 0";
        if (excludeId.HasValue)
            sql += " AND vendor_id != @excludeId";
        
        var parameters = excludeId.HasValue 
            ? new[] { new SQLiteParameter("@vendorName", vendorName), new SQLiteParameter("@excludeId", excludeId.Value) }
            : new[] { new SQLiteParameter("@vendorName", vendorName) };
        
        return DatabaseManager.ExecuteScalar<long>(sql, parameters) > 0;
    }
}

/// <summary>
/// Repository for Buyer master operations
/// </summary>
public static class BuyerRepository
{
    public static List<Buyer> GetAll(bool activeOnly = true)
    {
        var buyers = new List<Buyer>();
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = "SELECT * FROM buyers WHERE is_deleted = 0" + (activeOnly ? " AND is_active = 1" : "") + " ORDER BY buyer_name";
        using var cmd = new SQLiteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            buyers.Add(new Buyer
            {
                BuyerId = reader.GetInt32(0),
                BuyerName = reader.GetString(1),
                Contact = reader.IsDBNull(2) ? null : reader.GetString(2),
                Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
                IsActive = reader.GetInt32(4) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(5))
            });
        }
        
        return buyers;
    }
    
    public static Buyer? GetById(int id)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        using var cmd = new SQLiteCommand("SELECT * FROM buyers WHERE buyer_id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            return new Buyer
            {
                BuyerId = reader.GetInt32(0),
                BuyerName = reader.GetString(1),
                Contact = reader.IsDBNull(2) ? null : reader.GetString(2),
                Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
                IsActive = reader.GetInt32(4) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(5))
            };
        }
        
        return null;
    }
    
    public static int Insert(Buyer buyer)
    {
        string sql = "INSERT INTO buyers (buyer_name, contact, notes, is_active) VALUES (@name, @contact, @notes, @isActive); SELECT last_insert_rowid();";
        
        int result = DatabaseManager.ExecuteScalar<int>(sql,
            new SQLiteParameter("@name", buyer.BuyerName),
            new SQLiteParameter("@contact", (object?)buyer.Contact ?? DBNull.Value),
            new SQLiteParameter("@notes", (object?)buyer.Notes ?? DBNull.Value),
            new SQLiteParameter("@isActive", buyer.IsActive ? 1 : 0));
        
        Services.BackupService.BackupAfterTransaction();
        return result;
    }
    
    public static void Update(Buyer buyer)
    {
        string sql = "UPDATE buyers SET buyer_name = @name, contact = @contact, notes = @notes, is_active = @isActive WHERE buyer_id = @id";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@name", buyer.BuyerName),
            new SQLiteParameter("@contact", (object?)buyer.Contact ?? DBNull.Value),
            new SQLiteParameter("@notes", (object?)buyer.Notes ?? DBNull.Value),
            new SQLiteParameter("@isActive", buyer.IsActive ? 1 : 0),
            new SQLiteParameter("@id", buyer.BuyerId));
        
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static void Delete(int id)
    {
        string sql = "UPDATE buyers SET is_deleted = 1, deleted_at = @deletedAt WHERE buyer_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, 
            new SQLiteParameter("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static bool Exists(string buyerName, int? excludeId = null)
    {
        string sql = "SELECT COUNT(*) FROM buyers WHERE LOWER(buyer_name) = LOWER(@buyerName) AND is_deleted = 0";
        if (excludeId.HasValue)
            sql += " AND buyer_id != @excludeId";
        
        var parameters = excludeId.HasValue 
            ? new[] { new SQLiteParameter("@buyerName", buyerName), new SQLiteParameter("@excludeId", excludeId.Value) }
            : new[] { new SQLiteParameter("@buyerName", buyerName) };
        
        return DatabaseManager.ExecuteScalar<long>(sql, parameters) > 0;
    }
}

/// <summary>
/// Repository for Material master operations
/// </summary>
public static class MaterialRepository
{
    public static List<Material> GetAll(bool activeOnly = true)
    {
        var materials = new List<Material>();
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = "SELECT * FROM materials WHERE is_deleted = 0" + (activeOnly ? " AND is_active = 1" : "") + " ORDER BY material_name";
        using var cmd = new SQLiteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            materials.Add(new Material
            {
                MaterialId = reader.GetInt32(0),
                MaterialName = reader.GetString(1),
                Unit = reader.GetString(2),
                Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
                IsActive = reader.GetInt32(4) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(5)),
                ConversionFactor_MT_to_CFT = reader.IsDBNull(6) ? 1.0m : Convert.ToDecimal(reader.GetValue(6))
            });
        }
        
        return materials;
    }
    
    public static Material? GetById(int id)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        using var cmd = new SQLiteCommand("SELECT * FROM materials WHERE material_id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            return new Material
            {
                MaterialId = reader.GetInt32(0),
                MaterialName = reader.GetString(1),
                Unit = reader.GetString(2),
                Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
                IsActive = reader.GetInt32(4) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(5)),
                ConversionFactor_MT_to_CFT = reader.IsDBNull(6) ? 1.0m : Convert.ToDecimal(reader.GetValue(6))
            };
        }
        
        return null;
    }
    
    public static int Insert(Material material)
    {
        string sql = "INSERT INTO materials (material_name, unit, notes, is_active, conversion_factor_mt_to_cft) VALUES (@name, @unit, @notes, @isActive, @conversionFactor); SELECT last_insert_rowid();";
        
        int result = DatabaseManager.ExecuteScalar<int>(sql,
            new SQLiteParameter("@name", material.MaterialName),
            new SQLiteParameter("@unit", material.Unit),
            new SQLiteParameter("@notes", (object?)material.Notes ?? DBNull.Value),
            new SQLiteParameter("@isActive", material.IsActive ? 1 : 0),
            new SQLiteParameter("@conversionFactor", material.ConversionFactor_MT_to_CFT));
        
        Services.BackupService.BackupAfterTransaction();
        return result;
    }
    
    public static void Update(Material material)
    {
        string sql = "UPDATE materials SET material_name = @name, unit = @unit, notes = @notes, is_active = @isActive, conversion_factor_mt_to_cft = @conversionFactor WHERE material_id = @id";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@name", material.MaterialName),
            new SQLiteParameter("@unit", material.Unit),
            new SQLiteParameter("@notes", (object?)material.Notes ?? DBNull.Value),
            new SQLiteParameter("@isActive", material.IsActive ? 1 : 0),
            new SQLiteParameter("@conversionFactor", material.ConversionFactor_MT_to_CFT),
            new SQLiteParameter("@id", material.MaterialId));
        
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static void Delete(int id)
    {
        string sql = "UPDATE materials SET is_deleted = 1, deleted_at = @deletedAt WHERE material_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, 
            new SQLiteParameter("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static bool Exists(string materialName, int? excludeId = null)
    {
        string sql = "SELECT COUNT(*) FROM materials WHERE LOWER(material_name) = LOWER(@materialName) AND is_deleted = 0";
        if (excludeId.HasValue)
            sql += " AND material_id != @excludeId";
        
        var parameters = excludeId.HasValue 
            ? new[] { new SQLiteParameter("@materialName", materialName), new SQLiteParameter("@excludeId", excludeId.Value) }
            : new[] { new SQLiteParameter("@materialName", materialName) };
        
        return DatabaseManager.ExecuteScalar<long>(sql, parameters) > 0;
    }
}

/// <summary>
/// Repository for Company Settings operations
/// </summary>
public static class CompanySettingsRepository
{
    public static CompanySettings? Get()
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        using var cmd = new SQLiteCommand("SELECT * FROM company_settings WHERE settings_id = 1", connection);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            // Column indices (0-based):
            // 0: settings_id, 1: company_name, 2: address, 3: phone, 4: email
            // 5: gst_number, 6: website, 7: logo_image, 8: invoice_prefix
            // 9: payment_terms, 10: terms_and_conditions, 11: updated_at, 12: font_size_scale
            
            return new CompanySettings
            {
                SettingsId = reader.GetInt32(0),
                CompanyName = reader.GetString(1),
                Address = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Phone = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Email = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                GSTNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                Website = reader.IsDBNull(6) ? null : reader.GetString(6),
                LogoImage = reader.IsDBNull(7) ? null : (byte[])reader[7],
                InvoicePrefix = reader.GetString(8),
                PaymentTerms = reader.GetString(9),
                TermsAndConditions = reader.IsDBNull(10) ? null : reader.GetString(10),
                UpdatedAt = DateTime.Parse(reader.GetString(11)),
                FontSizeScale = reader.FieldCount > 12 && !reader.IsDBNull(12) ? Convert.ToSingle(reader.GetValue(12)) : 1.0f
            };
        }
        
        return null;
    }
    
    public static void SaveOrUpdate(CompanySettings settings)
    {
        var existing = Get();
        
        if (existing == null)
        {
            // Insert
            string sql = @"INSERT INTO company_settings 
                (settings_id, company_name, address, phone, email, gst_number, website, 
                 logo_image, invoice_prefix, payment_terms, terms_and_conditions, font_size_scale, updated_at)
                VALUES (1, @companyName, @address, @phone, @email, @gstNumber, @website, 
                        @logoImage, @invoicePrefix, @paymentTerms, @termsAndConditions, @fontSizeScale, @updatedAt)";
            
            DatabaseManager.ExecuteNonQuery(sql,
                new SQLiteParameter("@companyName", settings.CompanyName),
                new SQLiteParameter("@address", settings.Address),
                new SQLiteParameter("@phone", settings.Phone),
                new SQLiteParameter("@email", settings.Email),
                new SQLiteParameter("@gstNumber", (object?)settings.GSTNumber ?? DBNull.Value),
                new SQLiteParameter("@website", (object?)settings.Website ?? DBNull.Value),
                new SQLiteParameter("@logoImage", (object?)settings.LogoImage ?? DBNull.Value),
                new SQLiteParameter("@invoicePrefix", settings.InvoicePrefix),
                new SQLiteParameter("@paymentTerms", settings.PaymentTerms),
                new SQLiteParameter("@termsAndConditions", (object?)settings.TermsAndConditions ?? DBNull.Value),
                new SQLiteParameter("@fontSizeScale", settings.FontSizeScale),
                new SQLiteParameter("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            );
        }
        else
        {
            // Update
            string sql = @"UPDATE company_settings SET 
                company_name = @companyName,
                address = @address,
                phone = @phone,
                email = @email,
                gst_number = @gstNumber,
                website = @website,
                logo_image = @logoImage,
                invoice_prefix = @invoicePrefix,
                payment_terms = @paymentTerms,
                terms_and_conditions = @termsAndConditions,
                font_size_scale = @fontSizeScale,
                updated_at = @updatedAt
                WHERE settings_id = 1";
            
            DatabaseManager.ExecuteNonQuery(sql,
                new SQLiteParameter("@companyName", settings.CompanyName),
                new SQLiteParameter("@address", settings.Address),
                new SQLiteParameter("@phone", settings.Phone),
                new SQLiteParameter("@email", settings.Email),
                new SQLiteParameter("@gstNumber", (object?)settings.GSTNumber ?? DBNull.Value),
                new SQLiteParameter("@website", (object?)settings.Website ?? DBNull.Value),
                new SQLiteParameter("@logoImage", (object?)settings.LogoImage ?? DBNull.Value),
                new SQLiteParameter("@invoicePrefix", settings.InvoicePrefix),
                new SQLiteParameter("@paymentTerms", settings.PaymentTerms),
                new SQLiteParameter("@termsAndConditions", (object?)settings.TermsAndConditions ?? DBNull.Value),
                new SQLiteParameter("@fontSizeScale", settings.FontSizeScale),
                new SQLiteParameter("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            );
        }
    }
    
    /// <summary>
    /// Save settings (alias for SaveOrUpdate)
    /// </summary>
    public static void Save(CompanySettings settings)
    {
        SaveOrUpdate(settings);
    }
}

/// <summary>
/// Repository for Invoice Metadata operations
/// </summary>
public static class InvoiceMetadataRepository
{
    public static string GenerateInvoiceNumber(string prefix)
    {
        int year = DateTime.Now.Year;
        
        // Get last invoice number for this year
        string pattern = $"{prefix}-{year}-%";
        string sql = @"SELECT invoice_number FROM invoice_metadata 
                      WHERE invoice_number LIKE @pattern 
                      ORDER BY invoice_metadata_id DESC LIMIT 1";
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        using var cmd = new SQLiteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@pattern", pattern);
        
        var lastInvoice = cmd.ExecuteScalar()?.ToString();
        
        int nextNumber = 1;
        if (!string.IsNullOrEmpty(lastInvoice))
        {
            // Extract number from format: INV-2026-00001
            var parts = lastInvoice.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNumber = lastNum + 1;
            }
        }
        
        return $"{prefix}-{year}-{nextNumber:D5}";
    }
    
    public static void Save(InvoiceMetadata metadata)
    {
        string sql = @"INSERT INTO invoice_metadata 
            (invoice_number, transaction_type, transaction_id, generated_date, generated_by, file_path)
            VALUES (@invoiceNumber, @transactionType, @transactionId, @generatedDate, @generatedBy, @filePath)";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@invoiceNumber", metadata.InvoiceNumber),
            new SQLiteParameter("@transactionType", metadata.TransactionType),
            new SQLiteParameter("@transactionId", metadata.TransactionId),
            new SQLiteParameter("@generatedDate", metadata.GeneratedDate.ToString("yyyy-MM-dd HH:mm:ss")),
            new SQLiteParameter("@generatedBy", metadata.GeneratedBy),
            new SQLiteParameter("@filePath", metadata.FilePath)
        );
    }
    
    public static InvoiceMetadata? GetByTransaction(string transactionType, int transactionId)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = @"SELECT * FROM invoice_metadata 
                      WHERE transaction_type = @type AND transaction_id = @id 
                      ORDER BY invoice_metadata_id DESC LIMIT 1";
        
        using var cmd = new SQLiteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@type", transactionType);
        cmd.Parameters.AddWithValue("@id", transactionId);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            return new InvoiceMetadata
            {
                InvoiceMetadataId = reader.GetInt32(0),
                InvoiceNumber = reader.GetString(1),
                TransactionType = reader.GetString(2),
                TransactionId = reader.GetInt32(3),
                GeneratedDate = DateTime.Parse(reader.GetString(4)),
                GeneratedBy = reader.GetString(5),
                FilePath = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
            };
        }
        
        return null;
    }
}
