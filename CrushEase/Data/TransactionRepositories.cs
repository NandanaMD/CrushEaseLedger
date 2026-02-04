using System.Data.SQLite;
using CrushEase.Models;

namespace CrushEase.Data;

/// <summary>
/// Repository for Sales transaction operations
/// </summary>
public static class SaleRepository
{
    // Helper method to safely convert SQLite numeric values to decimal
    private static decimal SafeGetDecimal(System.Data.SQLite.SQLiteDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return 0m;
            
        var value = reader.GetValue(ordinal);
        return value switch
        {
            long l => (decimal)l,
            int i => (decimal)i,
            double d => (decimal)d,
            float f => (decimal)f,
            decimal dec => dec,
            _ => Convert.ToDecimal(value)
        };
    }
    
    public static List<Sale> GetAll(DateTime? fromDate = null, DateTime? toDate = null, int? vehicleId = null)
    {
        var sales = new List<Sale>();
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = @"
            SELECT s.sale_id, s.sale_date, s.vehicle_id, s.buyer_id, s.material_id, 
                   s.quantity, s.rate, s.amount, s.created_at, s.input_unit, s.input_quantity, s.calculated_cft,
                   v.vehicle_no, b.buyer_name, m.material_name
            FROM sales s
            INNER JOIN vehicles v ON s.vehicle_id = v.vehicle_id
            INNER JOIN buyers b ON s.buyer_id = b.buyer_id
            INNER JOIN materials m ON s.material_id = m.material_id
            WHERE s.is_deleted = 0";
        
        if (fromDate.HasValue)
            sql += " AND s.sale_date >= @fromDate";
        if (toDate.HasValue)
            sql += " AND s.sale_date <= @toDate";
        if (vehicleId.HasValue)
            sql += " AND s.vehicle_id = @vehicleId";
        
        sql += " ORDER BY s.sale_date DESC, s.sale_id DESC";
        
        using var cmd = new SQLiteCommand(sql, connection);
        if (fromDate.HasValue)
            cmd.Parameters.AddWithValue("@fromDate", fromDate.Value.ToString("yyyy-MM-dd"));
        if (toDate.HasValue)
            cmd.Parameters.AddWithValue("@toDate", toDate.Value.ToString("yyyy-MM-dd"));
        if (vehicleId.HasValue)
            cmd.Parameters.AddWithValue("@vehicleId", vehicleId.Value);
        
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            sales.Add(new Sale
            {
                SaleId = reader.GetInt32(0),
                SaleDate = DateTime.Parse(reader.GetString(1)),
                VehicleId = reader.GetInt32(2),
                BuyerId = reader.GetInt32(3),
                MaterialId = reader.GetInt32(4),
                Quantity = SafeGetDecimal(reader, 5),
                Rate = SafeGetDecimal(reader, 6),
                Amount = SafeGetDecimal(reader, 7),
                CreatedAt = DateTime.Parse(reader.GetString(8)),
                InputUnit = reader.IsDBNull(9) ? "CFT" : reader.GetString(9),
                InputQuantity = reader.IsDBNull(10) ? SafeGetDecimal(reader, 5) : SafeGetDecimal(reader, 10),
                CalculatedCFT = reader.IsDBNull(11) ? SafeGetDecimal(reader, 5) : SafeGetDecimal(reader, 11),
                VehicleNo = reader.GetString(12),
                BuyerName = reader.GetString(13),
                MaterialName = reader.GetString(14)
            });
        }
        
        return sales;
    }
    
    public static Sale? GetById(int id)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = @"
            SELECT s.sale_id, s.sale_date, s.vehicle_id, s.buyer_id, s.material_id, 
                   s.quantity, s.rate, s.amount, s.created_at, s.input_unit, s.input_quantity, s.calculated_cft,
                   v.vehicle_no, b.buyer_name, m.material_name
            FROM sales s
            INNER JOIN vehicles v ON s.vehicle_id = v.vehicle_id
            INNER JOIN buyers b ON s.buyer_id = b.buyer_id
            INNER JOIN materials m ON s.material_id = m.material_id
            WHERE s.sale_id = @id AND s.is_deleted = 0";
        
        using var cmd = new SQLiteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            return new Sale
            {
                SaleId = reader.GetInt32(0),
                SaleDate = DateTime.Parse(reader.GetString(1)),
                VehicleId = reader.GetInt32(2),
                BuyerId = reader.GetInt32(3),
                MaterialId = reader.GetInt32(4),
                Quantity = SafeGetDecimal(reader, 5),
                Rate = SafeGetDecimal(reader, 6),
                Amount = SafeGetDecimal(reader, 7),
                CreatedAt = DateTime.Parse(reader.GetString(8)),
                InputUnit = reader.IsDBNull(9) ? "CFT" : reader.GetString(9),
                InputQuantity = reader.IsDBNull(10) ? SafeGetDecimal(reader, 5) : SafeGetDecimal(reader, 10),
                CalculatedCFT = reader.IsDBNull(11) ? SafeGetDecimal(reader, 5) : SafeGetDecimal(reader, 11),
                VehicleNo = reader.GetString(12),
                BuyerName = reader.GetString(13),
                MaterialName = reader.GetString(14)
            };
        }
        
        return null;
    }
    
    public static int Insert(Sale sale)
    {
        string sql = @"
            INSERT INTO sales (sale_date, vehicle_id, buyer_id, material_id, quantity, rate, amount, input_unit, input_quantity, calculated_cft)
            VALUES (@date, @vehicleId, @buyerId, @materialId, @quantity, @rate, @amount, @inputUnit, @inputQuantity, @calculatedCFT);
            SELECT last_insert_rowid();";
        
        int result = DatabaseManager.ExecuteScalar<int>(sql,
            new SQLiteParameter("@date", sale.SaleDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@vehicleId", sale.VehicleId),
            new SQLiteParameter("@buyerId", sale.BuyerId),
            new SQLiteParameter("@materialId", sale.MaterialId),
            new SQLiteParameter("@quantity", sale.Quantity),
            new SQLiteParameter("@rate", sale.Rate),
            new SQLiteParameter("@amount", sale.Amount),
            new SQLiteParameter("@inputUnit", sale.InputUnit),
            new SQLiteParameter("@inputQuantity", sale.InputQuantity),
            new SQLiteParameter("@calculatedCFT", sale.CalculatedCFT));
        
        Services.BackupService.BackupAfterTransaction();
        return result;
    }
    
    public static void Update(Sale sale)
    {
        string sql = @"
            UPDATE sales 
            SET sale_date = @date, 
                vehicle_id = @vehicleId, 
                buyer_id = @buyerId, 
                material_id = @materialId, 
                quantity = @quantity, 
                rate = @rate, 
                amount = @amount,
                input_unit = @inputUnit,
                input_quantity = @inputQuantity,
                calculated_cft = @calculatedCFT
            WHERE sale_id = @id";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@date", sale.SaleDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@vehicleId", sale.VehicleId),
            new SQLiteParameter("@buyerId", sale.BuyerId),
            new SQLiteParameter("@materialId", sale.MaterialId),
            new SQLiteParameter("@quantity", sale.Quantity),
            new SQLiteParameter("@rate", sale.Rate),
            new SQLiteParameter("@amount", sale.Amount),
            new SQLiteParameter("@inputUnit", sale.InputUnit),
            new SQLiteParameter("@inputQuantity", sale.InputQuantity),
            new SQLiteParameter("@calculatedCFT", sale.CalculatedCFT),
            new SQLiteParameter("@id", sale.SaleId));
        
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static void Delete(int id)
    {
        // Soft delete - mark as deleted
        string sql = "UPDATE sales SET is_deleted = 1, deleted_at = @deletedAt WHERE sale_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, 
            new SQLiteParameter("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static List<Sale> GetRecent(int count = 10)
    {
        return GetAll().Take(count).ToList();
    }
    
    public static decimal GetTotalForDateRange(DateTime fromDate, DateTime toDate)
    {
        string sql = "SELECT COALESCE(SUM(amount), 0) FROM sales WHERE is_deleted = 0 AND sale_date >= @fromDate AND sale_date <= @toDate";
        
        return DatabaseManager.ExecuteScalar<decimal>(sql,
            new SQLiteParameter("@fromDate", fromDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@toDate", toDate.ToString("yyyy-MM-dd")));
    }
}

/// <summary>
/// Repository for Purchase transaction operations
/// </summary>
public static class PurchaseRepository
{
    private static decimal SafeGetDecimal(SQLiteDataReader reader, int ordinal)
    {
        var value = reader.GetValue(ordinal);
        return value switch
        {
            long l => (decimal)l,
            int i => (decimal)i,
            double d => (decimal)d,
            float f => (decimal)f,
            decimal dec => dec,
            _ => Convert.ToDecimal(value)
        };
    }

    public static List<Purchase> GetAll(DateTime? fromDate = null, DateTime? toDate = null, int? vehicleId = null)
    {
        var purchases = new List<Purchase>();
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = @"
            SELECT p.purchase_id, p.purchase_date, p.vehicle_id, p.vendor_id, p.material_id, 
                   p.quantity, p.rate, p.amount, p.vendor_site, p.created_at, p.input_unit, p.input_quantity, p.calculated_cft,
                   v.vehicle_no, vd.vendor_name, m.material_name
            FROM purchases p
            INNER JOIN vehicles v ON p.vehicle_id = v.vehicle_id
            INNER JOIN vendors vd ON p.vendor_id = vd.vendor_id
            INNER JOIN materials m ON p.material_id = m.material_id
            WHERE p.is_deleted = 0";
        
        if (fromDate.HasValue)
            sql += " AND p.purchase_date >= @fromDate";
        if (toDate.HasValue)
            sql += " AND p.purchase_date <= @toDate";
        if (vehicleId.HasValue)
            sql += " AND p.vehicle_id = @vehicleId";
        
        sql += " ORDER BY p.purchase_date DESC, p.purchase_id DESC";
        
        using var cmd = new SQLiteCommand(sql, connection);
        if (fromDate.HasValue)
            cmd.Parameters.AddWithValue("@fromDate", fromDate.Value.ToString("yyyy-MM-dd"));
        if (toDate.HasValue)
            cmd.Parameters.AddWithValue("@toDate", toDate.Value.ToString("yyyy-MM-dd"));
        if (vehicleId.HasValue)
            cmd.Parameters.AddWithValue("@vehicleId", vehicleId.Value);
        
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            purchases.Add(new Purchase
            {
                PurchaseId = reader.GetInt32(0),
                PurchaseDate = DateTime.Parse(reader.GetString(1)),
                VehicleId = reader.GetInt32(2),
                VendorId = reader.GetInt32(3),
                MaterialId = reader.GetInt32(4),
                Quantity = SafeGetDecimal(reader, 5),
                Rate = SafeGetDecimal(reader, 6),
                Amount = SafeGetDecimal(reader, 7),
                VendorSite = reader.IsDBNull(8) ? null : reader.GetString(8),
                CreatedAt = DateTime.Parse(reader.GetString(9)),
                InputUnit = reader.IsDBNull(10) ? "CFT" : reader.GetString(10),
                InputQuantity = reader.IsDBNull(11) ? SafeGetDecimal(reader, 5) : SafeGetDecimal(reader, 11),
                CalculatedCFT = reader.IsDBNull(12) ? SafeGetDecimal(reader, 5) : SafeGetDecimal(reader, 12),
                VehicleNo = reader.GetString(13),
                VendorName = reader.GetString(14),
                MaterialName = reader.GetString(15)
            });
        }
        
        return purchases;
    }
    
    public static Purchase? GetById(int id)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = @"
            SELECT p.purchase_id, p.purchase_date, p.vehicle_id, p.vendor_id, p.material_id, 
                   p.quantity, p.rate, p.amount, p.vendor_site, p.created_at, p.input_unit, p.input_quantity, p.calculated_cft,
                   v.vehicle_no, vd.vendor_name, m.material_name
            FROM purchases p
            INNER JOIN vehicles v ON p.vehicle_id = v.vehicle_id
            INNER JOIN vendors vd ON p.vendor_id = vd.vendor_id
            INNER JOIN materials m ON p.material_id = m.material_id
            WHERE p.purchase_id = @id";
        
        using var cmd = new SQLiteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            return new Purchase
            {
                PurchaseId = reader.GetInt32(0),
                PurchaseDate = DateTime.Parse(reader.GetString(1)),
                VehicleId = reader.GetInt32(2),
                VendorId = reader.GetInt32(3),
                MaterialId = reader.GetInt32(4),
                Quantity = SafeGetDecimal(reader, 5),
                Rate = SafeGetDecimal(reader, 6),
                Amount = SafeGetDecimal(reader, 7),
                VendorSite = reader.IsDBNull(8) ? null : reader.GetString(8),
                CreatedAt = DateTime.Parse(reader.GetString(9)),
                InputUnit = reader.IsDBNull(10) ? "CFT" : reader.GetString(10),
                InputQuantity = reader.IsDBNull(11) ? SafeGetDecimal(reader, 5) : SafeGetDecimal(reader, 11),
                CalculatedCFT = reader.IsDBNull(12) ? SafeGetDecimal(reader, 5) : SafeGetDecimal(reader, 12),
                VehicleNo = reader.GetString(13),
                VendorName = reader.GetString(14),
                MaterialName = reader.GetString(15)
            };
        }
        
        return null;
    }
    
    public static int Insert(Purchase purchase)
    {
        string sql = @"
            INSERT INTO purchases (purchase_date, vehicle_id, vendor_id, material_id, quantity, rate, amount, vendor_site, input_unit, input_quantity, calculated_cft)
            VALUES (@date, @vehicleId, @vendorId, @materialId, @quantity, @rate, @amount, @vendorSite, @inputUnit, @inputQuantity, @calculatedCFT);
            SELECT last_insert_rowid();";
        
        int result = DatabaseManager.ExecuteScalar<int>(sql,
            new SQLiteParameter("@date", purchase.PurchaseDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@vehicleId", purchase.VehicleId),
            new SQLiteParameter("@vendorId", purchase.VendorId),
            new SQLiteParameter("@materialId", purchase.MaterialId),
            new SQLiteParameter("@quantity", purchase.Quantity),
            new SQLiteParameter("@rate", purchase.Rate),
            new SQLiteParameter("@amount", purchase.Amount),
            new SQLiteParameter("@vendorSite", (object?)purchase.VendorSite ?? DBNull.Value),
            new SQLiteParameter("@inputUnit", purchase.InputUnit),
            new SQLiteParameter("@inputQuantity", purchase.InputQuantity),
            new SQLiteParameter("@calculatedCFT", purchase.CalculatedCFT));
        
        Services.BackupService.BackupAfterTransaction();
        return result;
    }
    
    public static void Update(Purchase purchase)
    {
        string sql = @"
            UPDATE purchases 
            SET purchase_date = @date, 
                vehicle_id = @vehicleId, 
                vendor_id = @vendorId, 
                material_id = @materialId, 
                quantity = @quantity, 
                rate = @rate, 
                amount = @amount,
                vendor_site = @vendorSite,
                input_unit = @inputUnit,
                input_quantity = @inputQuantity,
                calculated_cft = @calculatedCFT
            WHERE purchase_id = @id";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@date", purchase.PurchaseDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@vehicleId", purchase.VehicleId),
            new SQLiteParameter("@vendorId", purchase.VendorId),
            new SQLiteParameter("@materialId", purchase.MaterialId),
            new SQLiteParameter("@quantity", purchase.Quantity),
            new SQLiteParameter("@rate", purchase.Rate),
            new SQLiteParameter("@amount", purchase.Amount),
            new SQLiteParameter("@vendorSite", (object?)purchase.VendorSite ?? DBNull.Value),
            new SQLiteParameter("@inputUnit", purchase.InputUnit),
            new SQLiteParameter("@inputQuantity", purchase.InputQuantity),
            new SQLiteParameter("@calculatedCFT", purchase.CalculatedCFT),
            new SQLiteParameter("@id", purchase.PurchaseId));
        
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static void Delete(int id)
    {
        // Soft delete - mark as deleted
        string sql = "UPDATE purchases SET is_deleted = 1, deleted_at = @deletedAt WHERE purchase_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, 
            new SQLiteParameter("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static List<Purchase> GetRecent(int count = 10)
    {
        return GetAll().Take(count).ToList();
    }
    
    public static decimal GetTotalForDateRange(DateTime fromDate, DateTime toDate)
    {
        string sql = "SELECT COALESCE(SUM(amount), 0) FROM purchases WHERE is_deleted = 0 AND purchase_date >= @fromDate AND purchase_date <= @toDate";
        
        return DatabaseManager.ExecuteScalar<decimal>(sql,
            new SQLiteParameter("@fromDate", fromDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@toDate", toDate.ToString("yyyy-MM-dd")));
    }
}

/// <summary>
/// Repository for Maintenance transaction operations
/// </summary>
public static class MaintenanceRepository
{
    private static decimal SafeGetDecimal(SQLiteDataReader reader, int ordinal)
    {
        var value = reader.GetValue(ordinal);
        return value switch
        {
            long l => (decimal)l,
            int i => (decimal)i,
            double d => (decimal)d,
            float f => (decimal)f,
            decimal dec => dec,
            _ => Convert.ToDecimal(value)
        };
    }

    public static List<Maintenance> GetAll(DateTime? fromDate = null, DateTime? toDate = null, int? vehicleId = null)
    {
        var maintenance = new List<Maintenance>();
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = @"
            SELECT m.maintenance_id, m.maintenance_date, m.vehicle_id, m.description, 
                   m.amount, m.created_at, v.vehicle_no
            FROM maintenance m
            INNER JOIN vehicles v ON m.vehicle_id = v.vehicle_id
            WHERE m.is_deleted = 0";
        
        if (fromDate.HasValue)
            sql += " AND m.maintenance_date >= @fromDate";
        if (toDate.HasValue)
            sql += " AND m.maintenance_date <= @toDate";
        if (vehicleId.HasValue)
            sql += " AND m.vehicle_id = @vehicleId";
        
        sql += " ORDER BY m.maintenance_date DESC, m.maintenance_id DESC";
        
        using var cmd = new SQLiteCommand(sql, connection);
        if (fromDate.HasValue)
            cmd.Parameters.AddWithValue("@fromDate", fromDate.Value.ToString("yyyy-MM-dd"));
        if (toDate.HasValue)
            cmd.Parameters.AddWithValue("@toDate", toDate.Value.ToString("yyyy-MM-dd"));
        if (vehicleId.HasValue)
            cmd.Parameters.AddWithValue("@vehicleId", vehicleId.Value);
        
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            maintenance.Add(new Maintenance
            {
                MaintenanceId = reader.GetInt32(0),
                MaintenanceDate = DateTime.Parse(reader.GetString(1)),
                VehicleId = reader.GetInt32(2),
                Description = reader.GetString(3),
                Amount = SafeGetDecimal(reader, 4),
                CreatedAt = DateTime.Parse(reader.GetString(5)),
                VehicleNo = reader.GetString(6)
            });
        }
        
        return maintenance;
    }
    
    public static Maintenance? GetById(int id)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = @"
            SELECT m.maintenance_id, m.maintenance_date, m.vehicle_id, m.description, 
                   m.amount, m.created_at, v.vehicle_no
            FROM maintenance m
            INNER JOIN vehicles v ON m.vehicle_id = v.vehicle_id
            WHERE m.maintenance_id = @id";
        
        using var cmd = new SQLiteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            return new Maintenance
            {
                MaintenanceId = reader.GetInt32(0),
                MaintenanceDate = DateTime.Parse(reader.GetString(1)),
                VehicleId = reader.GetInt32(2),
                Description = reader.GetString(3),
                Amount = SafeGetDecimal(reader, 4),
                CreatedAt = DateTime.Parse(reader.GetString(5)),
                VehicleNo = reader.GetString(6)
            };
        }
        
        return null;
    }
    
    public static int Insert(Maintenance maintenance)
    {
        string sql = @"
            INSERT INTO maintenance (maintenance_date, vehicle_id, description, amount)
            VALUES (@date, @vehicleId, @description, @amount);
            SELECT last_insert_rowid();";
        
        int result = DatabaseManager.ExecuteScalar<int>(sql,
            new SQLiteParameter("@date", maintenance.MaintenanceDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@vehicleId", maintenance.VehicleId),
            new SQLiteParameter("@description", maintenance.Description),
            new SQLiteParameter("@amount", maintenance.Amount));
        
        Services.BackupService.BackupAfterTransaction();
        return result;
    }
    
    public static void Update(Maintenance maintenance)
    {
        string sql = @"
            UPDATE maintenance 
            SET maintenance_date = @date, 
                vehicle_id = @vehicleId, 
                description = @description, 
                amount = @amount
            WHERE maintenance_id = @id";
        
        DatabaseManager.ExecuteNonQuery(sql,
            new SQLiteParameter("@date", maintenance.MaintenanceDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@vehicleId", maintenance.VehicleId),
            new SQLiteParameter("@description", maintenance.Description),
            new SQLiteParameter("@amount", maintenance.Amount),
            new SQLiteParameter("@id", maintenance.MaintenanceId));
        
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static void Delete(int id)
    {
        // Soft delete - mark as deleted
        string sql = "UPDATE maintenance SET is_deleted = 1, deleted_at = @deletedAt WHERE maintenance_id = @id";
        DatabaseManager.ExecuteNonQuery(sql, 
            new SQLiteParameter("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new SQLiteParameter("@id", id));
        Services.BackupService.BackupAfterTransaction();
    }
    
    public static List<Maintenance> GetRecent(int count = 10)
    {
        return GetAll().Take(count).ToList();
    }
    
    public static decimal GetTotalForDateRange(DateTime fromDate, DateTime toDate)
    {
        string sql = "SELECT COALESCE(SUM(amount), 0) FROM maintenance WHERE is_deleted = 0 AND maintenance_date >= @fromDate AND maintenance_date <= @toDate";
        
        return DatabaseManager.ExecuteScalar<decimal>(sql,
            new SQLiteParameter("@fromDate", fromDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@toDate", toDate.ToString("yyyy-MM-dd")));
    }
}

/// <summary>
/// Repository for Dashboard queries
/// </summary>
public static class DashboardRepository
{
    public static DashboardSummary GetSummary()
    {
        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);
        
        var summary = new DashboardSummary
        {
            TodaySales = SaleRepository.GetTotalForDateRange(today, today),
            TodayPurchases = PurchaseRepository.GetTotalForDateRange(today, today),
            TodayMaintenance = MaintenanceRepository.GetTotalForDateRange(today, today),
            MonthSales = SaleRepository.GetTotalForDateRange(monthStart, monthEnd),
            MonthPurchases = PurchaseRepository.GetTotalForDateRange(monthStart, monthEnd),
            MonthMaintenance = MaintenanceRepository.GetTotalForDateRange(monthStart, monthEnd)
        };
        
        summary.MonthNetProfit = summary.MonthSales - summary.MonthPurchases - summary.MonthMaintenance;
        
        return summary;
    }
    
    public static List<VehicleProfitSummary> GetVehicleProfitSummary(DateTime fromDate, DateTime toDate)
    {
        var summaries = new List<VehicleProfitSummary>();
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = @"
            SELECT 
                v.vehicle_no,
                COALESCE(SUM(s.amount), 0) AS total_sales,
                COALESCE(SUM(p.amount), 0) AS total_purchases,
                COALESCE(SUM(m.amount), 0) AS total_maintenance
            FROM vehicles v
            LEFT JOIN sales s ON v.vehicle_id = s.vehicle_id 
                AND s.sale_date >= @fromDate AND s.sale_date <= @toDate AND s.is_deleted = 0
            LEFT JOIN purchases p ON v.vehicle_id = p.vehicle_id 
                AND p.purchase_date >= @fromDate AND p.purchase_date <= @toDate AND p.is_deleted = 0
            LEFT JOIN maintenance m ON v.vehicle_id = m.vehicle_id 
                AND m.maintenance_date >= @fromDate AND m.maintenance_date <= @toDate AND m.is_deleted = 0
            WHERE v.is_active = 1 AND v.is_deleted = 0
            GROUP BY v.vehicle_id, v.vehicle_no
            ORDER BY v.vehicle_no";
        
        using var cmd = new SQLiteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd"));
        
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            var summary = new VehicleProfitSummary
            {
                VehicleNo = reader.GetString(0),
                TotalSales = Convert.ToDecimal(reader.GetValue(1)),
                TotalPurchases = Convert.ToDecimal(reader.GetValue(2)),
                TotalMaintenance = Convert.ToDecimal(reader.GetValue(3))
            };
            summary.NetProfit = summary.TotalSales - summary.TotalPurchases - summary.TotalMaintenance;
            
            summaries.Add(summary);
        }
        
        return summaries;
    }
}
