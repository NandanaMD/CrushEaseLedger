namespace CrushEase.Models;

/// <summary>
/// Vehicle master record
/// </summary>
public class Vehicle
{
    public int VehicleId { get; set; }
    public string VehicleNo { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Vendor master record
/// </summary>
public class Vendor
{
    public int VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string? Contact { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Buyer master record
/// </summary>
public class Buyer
{
    public int BuyerId { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public string? Contact { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Material master record
/// </summary>
public class Material
{
    public int MaterialId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public string Unit { get; set; } = "Ton";
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Sales transaction record
/// </summary>
public class Sale
{
    public int SaleId { get; set; }
    public DateTime SaleDate { get; set; }
    public int VehicleId { get; set; }
    public int BuyerId { get; set; }
    public int MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties (for display)
    public string? VehicleNo { get; set; }
    public string? BuyerName { get; set; }
    public string? MaterialName { get; set; }
}

/// <summary>
/// Purchase transaction record
/// </summary>
public class Purchase
{
    public int PurchaseId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int VehicleId { get; set; }
    public int VendorId { get; set; }
    public int MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public string? VendorSite { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties (for display)
    public string? VehicleNo { get; set; }
    public string? VendorName { get; set; }
    public string? MaterialName { get; set; }
}

/// <summary>
/// Maintenance transaction record
/// </summary>
public class Maintenance
{
    public int MaintenanceId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public int VehicleId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties (for display)
    public string? VehicleNo { get; set; }
}

/// <summary>
/// Dashboard summary data
/// </summary>
public class DashboardSummary
{
    public decimal TodaySales { get; set; }
    public decimal TodayPurchases { get; set; }
    public decimal TodayMaintenance { get; set; }
    public decimal MonthSales { get; set; }
    public decimal MonthPurchases { get; set; }
    public decimal MonthMaintenance { get; set; }
    public decimal MonthNetProfit { get; set; }
}

/// <summary>
/// Vehicle profit summary for reports
/// </summary>
public class VehicleProfitSummary
{
    public string VehicleNo { get; set; } = string.Empty;
    public decimal TotalSales { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal TotalMaintenance { get; set; }
    public decimal NetProfit { get; set; }
}
