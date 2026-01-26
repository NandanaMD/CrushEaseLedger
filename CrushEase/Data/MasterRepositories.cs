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
        
        string sql = "SELECT * FROM vehicles" + (activeOnly ? " WHERE is_active = 1" : "") + " ORDER BY vehicle_no";
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
        // Soft delete
        string sql = "UPDATE vehicles SET is_active = 0 WHERE vehicle_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static bool Exists(string vehicleNo, int? excludeId = null)
    {
        string sql = "SELECT COUNT(*) FROM vehicles WHERE LOWER(vehicle_no) = LOWER(@vehicleNo)";
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
        
        string sql = "SELECT * FROM vendors" + (activeOnly ? " WHERE is_active = 1" : "") + " ORDER BY vendor_name";
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
        string sql = "UPDATE vendors SET is_active = 0 WHERE vendor_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static bool Exists(string vendorName, int? excludeId = null)
    {
        string sql = "SELECT COUNT(*) FROM vendors WHERE LOWER(vendor_name) = LOWER(@vendorName)";
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
        
        string sql = "SELECT * FROM buyers" + (activeOnly ? " WHERE is_active = 1" : "") + " ORDER BY buyer_name";
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
        string sql = "UPDATE buyers SET is_active = 0 WHERE buyer_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static bool Exists(string buyerName, int? excludeId = null)
    {
        string sql = "SELECT COUNT(*) FROM buyers WHERE LOWER(buyer_name) = LOWER(@buyerName)";
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
        
        string sql = "SELECT * FROM materials" + (activeOnly ? " WHERE is_active = 1" : "") + " ORDER BY material_name";
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
                CreatedAt = DateTime.Parse(reader.GetString(5))
            });
        }
        
        return materials;
    }
    
    public static int Insert(Material material)
    {
        string sql = "INSERT INTO materials (material_name, unit, notes, is_active) VALUES (@name, @unit, @notes, @isActive); SELECT last_insert_rowid();";
        
        int result = DatabaseManager.ExecuteScalar<int>(sql,
            new SQLiteParameter("@name", material.MaterialName),
            new SQLiteParameter("@unit", material.Unit),
            new SQLiteParameter("@notes", (object?)material.Notes ?? DBNull.Value),
            new SQLiteParameter("@isActive", material.IsActive ? 1 : 0));
        
        Services.BackupService.BackupAfterTransaction();
        return result;
    }
    
    public static void Update(Material material)
    {
        string sql = "UPDATE materials SET material_name = @name, unit = @unit, notes = @notes, is_active = @isActive WHERE material_id = @id";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@name", material.MaterialName),
            new SQLiteParameter("@unit", material.Unit),
            new SQLiteParameter("@notes", (object?)material.Notes ?? DBNull.Value),
            new SQLiteParameter("@isActive", material.IsActive ? 1 : 0),
            new SQLiteParameter("@id", material.MaterialId));
        
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static void Delete(int id)
    {
        string sql = "UPDATE materials SET is_active = 0 WHERE material_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static bool Exists(string materialName, int? excludeId = null)
    {
        string sql = "SELECT COUNT(*) FROM materials WHERE LOWER(material_name) = LOWER(@materialName)";
        if (excludeId.HasValue)
            sql += " AND material_id != @excludeId";
        
        var parameters = excludeId.HasValue 
            ? new[] { new SQLiteParameter("@materialName", materialName), new SQLiteParameter("@excludeId", excludeId.Value) }
            : new[] { new SQLiteParameter("@materialName", materialName) };
        
        return DatabaseManager.ExecuteScalar<long>(sql, parameters) > 0;
    }
}
