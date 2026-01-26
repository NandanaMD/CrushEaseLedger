using CrushEase.Models;
using CrushEase.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CrushEase.Services;

/// <summary>
/// Service for generating Excel reports using EPPlus
/// </summary>
public static class ExcelReportGenerator
{
    static ExcelReportGenerator()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
    
    /// <summary>
    /// Generate Sales Report
    /// </summary>
    public static void GenerateSalesReport(List<Sale> sales, DateTime fromDate, DateTime toDate, string filePath)
    {
        try
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sales Report");
            
            // Title
            worksheet.Cells["A1"].Value = "SALES REPORT";
            worksheet.Cells["A1:G1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            // Date range
            worksheet.Cells["A2"].Value = $"Period: {fromDate:dd-MMM-yyyy} to {toDate:dd-MMM-yyyy}";
            worksheet.Cells["A2:G2"].Merge = true;
            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            // Headers
            int row = 4;
            worksheet.Cells[row, 1].Value = "Date";
            worksheet.Cells[row, 2].Value = "Vehicle";
            worksheet.Cells[row, 3].Value = "Buyer";
            worksheet.Cells[row, 4].Value = "Material";
            worksheet.Cells[row, 5].Value = "Quantity";
            worksheet.Cells[row, 6].Value = "Rate";
            worksheet.Cells[row, 7].Value = "Amount";
            
            // Header formatting
            using (var range = worksheet.Cells[row, 1, row, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            
            // Data rows
            row++;
            decimal totalAmount = 0;
            foreach (var sale in sales)
            {
                worksheet.Cells[row, 1].Value = sale.SaleDate.ToString("dd-MMM-yyyy");
                worksheet.Cells[row, 2].Value = sale.VehicleNo;
                worksheet.Cells[row, 3].Value = sale.BuyerName;
                worksheet.Cells[row, 4].Value = sale.MaterialName;
                worksheet.Cells[row, 5].Value = sale.Quantity;
                worksheet.Cells[row, 6].Value = sale.Rate;
                worksheet.Cells[row, 7].Value = sale.Amount;
                
                worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                
                totalAmount += sale.Amount;
                row++;
            }
            
            // Total row
            worksheet.Cells[row, 1].Value = "TOTAL";
            worksheet.Cells[row, 1, row, 6].Merge = true;
            worksheet.Cells[row, 7].Value = totalAmount;
            using (var range = worksheet.Cells[row, 1, row, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
            
            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
            
            // Set landscape orientation
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            
            // Save
            package.SaveAs(new FileInfo(filePath));
            
            Logger.LogInfo($"Sales report generated: {filePath}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to generate sales report");
            throw;
        }
    }
    
    /// <summary>
    /// Generate Purchase Report
    /// </summary>
    public static void GeneratePurchaseReport(List<Purchase> purchases, DateTime fromDate, DateTime toDate, string filePath)
    {
        try
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Purchase Report");
            
            // Title
            worksheet.Cells["A1"].Value = "PURCHASE REPORT";
            worksheet.Cells["A1:H1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            // Date range
            worksheet.Cells["A2"].Value = $"Period: {fromDate:dd-MMM-yyyy} to {toDate:dd-MMM-yyyy}";
            worksheet.Cells["A2:H2"].Merge = true;
            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            // Headers
            int row = 4;
            worksheet.Cells[row, 1].Value = "Date";
            worksheet.Cells[row, 2].Value = "Vehicle";
            worksheet.Cells[row, 3].Value = "Vendor";
            worksheet.Cells[row, 4].Value = "Material";
            worksheet.Cells[row, 5].Value = "Vendor Site";
            worksheet.Cells[row, 6].Value = "Quantity";
            worksheet.Cells[row, 7].Value = "Rate";
            worksheet.Cells[row, 8].Value = "Amount";
            
            // Header formatting
            using (var range = worksheet.Cells[row, 1, row, 8])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            
            // Data rows
            row++;
            decimal totalAmount = 0;
            foreach (var purchase in purchases)
            {
                worksheet.Cells[row, 1].Value = purchase.PurchaseDate.ToString("dd-MMM-yyyy");
                worksheet.Cells[row, 2].Value = purchase.VehicleNo;
                worksheet.Cells[row, 3].Value = purchase.VendorName;
                worksheet.Cells[row, 4].Value = purchase.MaterialName;
                worksheet.Cells[row, 5].Value = purchase.VendorSite ?? "";
                worksheet.Cells[row, 6].Value = purchase.Quantity;
                worksheet.Cells[row, 7].Value = purchase.Rate;
                worksheet.Cells[row, 8].Value = purchase.Amount;
                
                worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 8].Style.Numberformat.Format = "#,##0.00";
                
                totalAmount += purchase.Amount;
                row++;
            }
            
            // Total row
            worksheet.Cells[row, 1].Value = "TOTAL";
            worksheet.Cells[row, 1, row, 7].Merge = true;
            worksheet.Cells[row, 8].Value = totalAmount;
            using (var range = worksheet.Cells[row, 1, row, 8])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            worksheet.Cells[row, 8].Style.Numberformat.Format = "#,##0.00";
            
            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
            
            // Set landscape orientation
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            
            // Save
            package.SaveAs(new FileInfo(filePath));
            
            Logger.LogInfo($"Purchase report generated: {filePath}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to generate purchase report");
            throw;
        }
    }
    
    /// <summary>
    /// Generate Maintenance Report
    /// </summary>
    public static void GenerateMaintenanceReport(List<Maintenance> maintenance, DateTime fromDate, DateTime toDate, string filePath)
    {
        try
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Maintenance Report");
            
            // Title
            worksheet.Cells["A1"].Value = "MAINTENANCE REPORT";
            worksheet.Cells["A1:D1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            // Date range
            worksheet.Cells["A2"].Value = $"Period: {fromDate:dd-MMM-yyyy} to {toDate:dd-MMM-yyyy}";
            worksheet.Cells["A2:D2"].Merge = true;
            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            // Headers
            int row = 4;
            worksheet.Cells[row, 1].Value = "Date";
            worksheet.Cells[row, 2].Value = "Vehicle";
            worksheet.Cells[row, 3].Value = "Description";
            worksheet.Cells[row, 4].Value = "Amount";
            
            // Header formatting
            using (var range = worksheet.Cells[row, 1, row, 4])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            
            // Data rows
            row++;
            decimal totalAmount = 0;
            foreach (var item in maintenance)
            {
                worksheet.Cells[row, 1].Value = item.MaintenanceDate.ToString("dd-MMM-yyyy");
                worksheet.Cells[row, 2].Value = item.VehicleNo;
                worksheet.Cells[row, 3].Value = item.Description;
                worksheet.Cells[row, 4].Value = item.Amount;
                
                worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                
                totalAmount += item.Amount;
                row++;
            }
            
            // Total row
            worksheet.Cells[row, 1].Value = "TOTAL";
            worksheet.Cells[row, 1, row, 3].Merge = true;
            worksheet.Cells[row, 4].Value = totalAmount;
            using (var range = worksheet.Cells[row, 1, row, 4])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
            
            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
            
            // Set landscape orientation
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            
            // Save
            package.SaveAs(new FileInfo(filePath));
            
            Logger.LogInfo($"Maintenance report generated: {filePath}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to generate maintenance report");
            throw;
        }
    }
    
    /// <summary>
    /// Generate Vehicle Profit Report
    /// </summary>
    public static void GenerateVehicleProfitReport(List<VehicleProfitSummary> summaries, DateTime fromDate, DateTime toDate, string filePath)
    {
        try
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Vehicle Profit Report");
            
            // Title
            worksheet.Cells["A1"].Value = "VEHICLE PROFIT REPORT";
            worksheet.Cells["A1:F1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            // Date range
            worksheet.Cells["A2"].Value = $"Period: {fromDate:dd-MMM-yyyy} to {toDate:dd-MMM-yyyy}";
            worksheet.Cells["A2:F2"].Merge = true;
            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            // Headers
            int row = 4;
            worksheet.Cells[row, 1].Value = "Vehicle";
            worksheet.Cells[row, 2].Value = "Total Sales";
            worksheet.Cells[row, 3].Value = "Total Purchases";
            worksheet.Cells[row, 4].Value = "Total Maintenance";
            worksheet.Cells[row, 5].Value = "Net Profit";
            worksheet.Cells[row, 6].Value = "Status";
            
            // Header formatting
            using (var range = worksheet.Cells[row, 1, row, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            
            // Data rows
            row++;
            decimal grandTotalSales = 0;
            decimal grandTotalPurchases = 0;
            decimal grandTotalMaintenance = 0;
            decimal grandNetProfit = 0;
            
            foreach (var summary in summaries)
            {
                worksheet.Cells[row, 1].Value = summary.VehicleNo;
                worksheet.Cells[row, 2].Value = summary.TotalSales;
                worksheet.Cells[row, 3].Value = summary.TotalPurchases;
                worksheet.Cells[row, 4].Value = summary.TotalMaintenance;
                worksheet.Cells[row, 5].Value = summary.NetProfit;
                worksheet.Cells[row, 6].Value = summary.NetProfit >= 0 ? "Profit" : "Loss";
                
                // Number formatting
                worksheet.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                
                // Color code profit/loss
                if (summary.NetProfit >= 0)
                {
                    worksheet.Cells[row, 5].Style.Font.Color.SetColor(Color.Green);
                    worksheet.Cells[row, 6].Style.Font.Color.SetColor(Color.Green);
                }
                else
                {
                    worksheet.Cells[row, 5].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[row, 6].Style.Font.Color.SetColor(Color.Red);
                }
                
                grandTotalSales += summary.TotalSales;
                grandTotalPurchases += summary.TotalPurchases;
                grandTotalMaintenance += summary.TotalMaintenance;
                grandNetProfit += summary.NetProfit;
                
                row++;
            }
            
            // Grand Total row
            worksheet.Cells[row, 1].Value = "GRAND TOTAL";
            worksheet.Cells[row, 2].Value = grandTotalSales;
            worksheet.Cells[row, 3].Value = grandTotalPurchases;
            worksheet.Cells[row, 4].Value = grandTotalMaintenance;
            worksheet.Cells[row, 5].Value = grandNetProfit;
            worksheet.Cells[row, 6].Value = grandNetProfit >= 0 ? "Profit" : "Loss";
            
            using (var range = worksheet.Cells[row, 1, row, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            
            worksheet.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
            
            // Color code grand total
            if (grandNetProfit >= 0)
            {
                worksheet.Cells[row, 5].Style.Font.Color.SetColor(Color.Green);
                worksheet.Cells[row, 6].Style.Font.Color.SetColor(Color.Green);
            }
            else
            {
                worksheet.Cells[row, 5].Style.Font.Color.SetColor(Color.Red);
                worksheet.Cells[row, 6].Style.Font.Color.SetColor(Color.Red);
            }
            
            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
            
            // Set landscape orientation
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            
            // Save
            package.SaveAs(new FileInfo(filePath));
            
            Logger.LogInfo($"Vehicle profit report generated: {filePath}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to generate vehicle profit report");
            throw;
        }
    }
    
    /// <summary>
    /// Generate Complete Data Export with all reports and master data
    /// </summary>
    public static void GenerateCompleteDataExport(string filePath)
    {
        try
        {
            using var package = new ExcelPackage();
            
            // Get all data
            var vehicles = Data.VehicleRepository.GetAll(false);
            var vendors = Data.VendorRepository.GetAll(false);
            var buyers = Data.BuyerRepository.GetAll(false);
            var materials = Data.MaterialRepository.GetAll(false);
            
            var sales = Data.SaleRepository.GetAll();
            var purchases = Data.PurchaseRepository.GetAll();
            var maintenance = Data.MaintenanceRepository.GetAll();
            
            // Calculate date ranges for reports
            var today = DateTime.Today;
            var startOfYear = new DateTime(today.Year, 1, 1);
            
            // 1. VEHICLES SHEET
            var vehiclesSheet = package.Workbook.Worksheets.Add("Vehicles");
            vehiclesSheet.Cells["A1"].Value = "VEHICLE MASTER DATA";
            vehiclesSheet.Cells["A1:D1"].Merge = true;
            vehiclesSheet.Cells["A1"].Style.Font.Size = 14;
            vehiclesSheet.Cells["A1"].Style.Font.Bold = true;
            vehiclesSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            int row = 3;
            vehiclesSheet.Cells[row, 1].Value = "Vehicle No";
            vehiclesSheet.Cells[row, 2].Value = "Status";
            vehiclesSheet.Cells[row, 3].Value = "Created Date";
            FormatHeaderRow(vehiclesSheet, row, 1, 3);
            
            row++;
            foreach (var vehicle in vehicles)
            {
                vehiclesSheet.Cells[row, 1].Value = vehicle.VehicleNo;
                vehiclesSheet.Cells[row, 2].Value = vehicle.IsActive ? "Active" : "Inactive";
                vehiclesSheet.Cells[row, 3].Value = vehicle.CreatedAt.ToString("dd-MMM-yyyy");
                row++;
            }
            vehiclesSheet.Cells.AutoFitColumns();
            
            // 2. VENDORS SHEET
            var vendorsSheet = package.Workbook.Worksheets.Add("Vendors");
            vendorsSheet.Cells["A1"].Value = "VENDOR MASTER DATA";
            vendorsSheet.Cells["A1:E1"].Merge = true;
            vendorsSheet.Cells["A1"].Style.Font.Size = 14;
            vendorsSheet.Cells["A1"].Style.Font.Bold = true;
            vendorsSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            row = 3;
            vendorsSheet.Cells[row, 1].Value = "Vendor Name";
            vendorsSheet.Cells[row, 2].Value = "Contact";
            vendorsSheet.Cells[row, 3].Value = "Notes";
            vendorsSheet.Cells[row, 4].Value = "Status";
            vendorsSheet.Cells[row, 5].Value = "Created Date";
            FormatHeaderRow(vendorsSheet, row, 1, 5);
            
            row++;
            foreach (var vendor in vendors)
            {
                vendorsSheet.Cells[row, 1].Value = vendor.VendorName;
                vendorsSheet.Cells[row, 2].Value = vendor.Contact ?? "";
                vendorsSheet.Cells[row, 3].Value = vendor.Notes ?? "";
                vendorsSheet.Cells[row, 4].Value = vendor.IsActive ? "Active" : "Inactive";
                vendorsSheet.Cells[row, 5].Value = vendor.CreatedAt.ToString("dd-MMM-yyyy");
                row++;
            }
            vendorsSheet.Cells.AutoFitColumns();
            
            // 3. BUYERS SHEET
            var buyersSheet = package.Workbook.Worksheets.Add("Buyers");
            buyersSheet.Cells["A1"].Value = "BUYER MASTER DATA";
            buyersSheet.Cells["A1:E1"].Merge = true;
            buyersSheet.Cells["A1"].Style.Font.Size = 14;
            buyersSheet.Cells["A1"].Style.Font.Bold = true;
            buyersSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            row = 3;
            buyersSheet.Cells[row, 1].Value = "Buyer Name";
            buyersSheet.Cells[row, 2].Value = "Contact";
            buyersSheet.Cells[row, 3].Value = "Notes";
            buyersSheet.Cells[row, 4].Value = "Status";
            buyersSheet.Cells[row, 5].Value = "Created Date";
            FormatHeaderRow(buyersSheet, row, 1, 5);
            
            row++;
            foreach (var buyer in buyers)
            {
                buyersSheet.Cells[row, 1].Value = buyer.BuyerName;
                buyersSheet.Cells[row, 2].Value = buyer.Contact ?? "";
                buyersSheet.Cells[row, 3].Value = buyer.Notes ?? "";
                buyersSheet.Cells[row, 4].Value = buyer.IsActive ? "Active" : "Inactive";
                buyersSheet.Cells[row, 5].Value = buyer.CreatedAt.ToString("dd-MMM-yyyy");
                row++;
            }
            buyersSheet.Cells.AutoFitColumns();
            
            // 4. MATERIALS SHEET
            var materialsSheet = package.Workbook.Worksheets.Add("Materials");
            materialsSheet.Cells["A1"].Value = "MATERIAL MASTER DATA";
            materialsSheet.Cells["A1:F1"].Merge = true;
            materialsSheet.Cells["A1"].Style.Font.Size = 14;
            materialsSheet.Cells["A1"].Style.Font.Bold = true;
            materialsSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            row = 3;
            materialsSheet.Cells[row, 1].Value = "Material Name";
            materialsSheet.Cells[row, 2].Value = "Unit";
            materialsSheet.Cells[row, 3].Value = "Notes";
            materialsSheet.Cells[row, 4].Value = "Status";
            materialsSheet.Cells[row, 5].Value = "Created Date";
            FormatHeaderRow(materialsSheet, row, 1, 5);
            
            row++;
            foreach (var material in materials)
            {
                materialsSheet.Cells[row, 1].Value = material.MaterialName;
                materialsSheet.Cells[row, 2].Value = material.Unit;
                materialsSheet.Cells[row, 3].Value = material.Notes ?? "";
                materialsSheet.Cells[row, 4].Value = material.IsActive ? "Active" : "Inactive";
                materialsSheet.Cells[row, 5].Value = material.CreatedAt.ToString("dd-MMM-yyyy");
                row++;
            }
            materialsSheet.Cells.AutoFitColumns();
            
            // 5. SALES SHEET
            var salesSheet = package.Workbook.Worksheets.Add("All Sales");
            salesSheet.Cells["A1"].Value = "ALL SALES DATA";
            salesSheet.Cells["A1:G1"].Merge = true;
            salesSheet.Cells["A1"].Style.Font.Size = 14;
            salesSheet.Cells["A1"].Style.Font.Bold = true;
            salesSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            row = 3;
            salesSheet.Cells[row, 1].Value = "Date";
            salesSheet.Cells[row, 2].Value = "Vehicle";
            salesSheet.Cells[row, 3].Value = "Buyer";
            salesSheet.Cells[row, 4].Value = "Material";
            salesSheet.Cells[row, 5].Value = "Quantity";
            salesSheet.Cells[row, 6].Value = "Rate";
            salesSheet.Cells[row, 7].Value = "Amount";
            FormatHeaderRow(salesSheet, row, 1, 7);
            
            row++;
            decimal totalSales = 0;
            foreach (var sale in sales)
            {
                salesSheet.Cells[row, 1].Value = sale.SaleDate.ToString("dd-MMM-yyyy");
                salesSheet.Cells[row, 2].Value = sale.VehicleNo;
                salesSheet.Cells[row, 3].Value = sale.BuyerName;
                salesSheet.Cells[row, 4].Value = sale.MaterialName;
                salesSheet.Cells[row, 5].Value = sale.Quantity;
                salesSheet.Cells[row, 6].Value = sale.Rate;
                salesSheet.Cells[row, 7].Value = sale.Amount;
                
                salesSheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                salesSheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                salesSheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                
                totalSales += sale.Amount;
                row++;
            }
            
            // Total row
            salesSheet.Cells[row, 1].Value = "TOTAL";
            salesSheet.Cells[row, 1, row, 6].Merge = true;
            salesSheet.Cells[row, 7].Value = totalSales;
            FormatTotalRow(salesSheet, row, 1, 7);
            salesSheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
            salesSheet.Cells.AutoFitColumns();
            
            // 6. PURCHASES SHEET
            var purchasesSheet = package.Workbook.Worksheets.Add("All Purchases");
            purchasesSheet.Cells["A1"].Value = "ALL PURCHASES DATA";
            purchasesSheet.Cells["A1:H1"].Merge = true;
            purchasesSheet.Cells["A1"].Style.Font.Size = 14;
            purchasesSheet.Cells["A1"].Style.Font.Bold = true;
            purchasesSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            row = 3;
            purchasesSheet.Cells[row, 1].Value = "Date";
            purchasesSheet.Cells[row, 2].Value = "Vehicle";
            purchasesSheet.Cells[row, 3].Value = "Vendor";
            purchasesSheet.Cells[row, 4].Value = "Material";
            purchasesSheet.Cells[row, 5].Value = "Vendor Site";
            purchasesSheet.Cells[row, 6].Value = "Quantity";
            purchasesSheet.Cells[row, 7].Value = "Rate";
            purchasesSheet.Cells[row, 8].Value = "Amount";
            FormatHeaderRow(purchasesSheet, row, 1, 8);
            
            row++;
            decimal totalPurchases = 0;
            foreach (var purchase in purchases)
            {
                purchasesSheet.Cells[row, 1].Value = purchase.PurchaseDate.ToString("dd-MMM-yyyy");
                purchasesSheet.Cells[row, 2].Value = purchase.VehicleNo;
                purchasesSheet.Cells[row, 3].Value = purchase.VendorName;
                purchasesSheet.Cells[row, 4].Value = purchase.MaterialName;
                purchasesSheet.Cells[row, 5].Value = purchase.VendorSite ?? "";
                purchasesSheet.Cells[row, 6].Value = purchase.Quantity;
                purchasesSheet.Cells[row, 7].Value = purchase.Rate;
                purchasesSheet.Cells[row, 8].Value = purchase.Amount;
                
                purchasesSheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                purchasesSheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                purchasesSheet.Cells[row, 8].Style.Numberformat.Format = "#,##0.00";
                
                totalPurchases += purchase.Amount;
                row++;
            }
            
            // Total row
            purchasesSheet.Cells[row, 1].Value = "TOTAL";
            purchasesSheet.Cells[row, 1, row, 7].Merge = true;
            purchasesSheet.Cells[row, 8].Value = totalPurchases;
            FormatTotalRow(purchasesSheet, row, 1, 8);
            purchasesSheet.Cells[row, 8].Style.Numberformat.Format = "#,##0.00";
            purchasesSheet.Cells.AutoFitColumns();
            
            // 7. MAINTENANCE SHEET
            var maintenanceSheet = package.Workbook.Worksheets.Add("All Maintenance");
            maintenanceSheet.Cells["A1"].Value = "ALL MAINTENANCE DATA";
            maintenanceSheet.Cells["A1:D1"].Merge = true;
            maintenanceSheet.Cells["A1"].Style.Font.Size = 14;
            maintenanceSheet.Cells["A1"].Style.Font.Bold = true;
            maintenanceSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            row = 3;
            maintenanceSheet.Cells[row, 1].Value = "Date";
            maintenanceSheet.Cells[row, 2].Value = "Vehicle";
            maintenanceSheet.Cells[row, 3].Value = "Description";
            maintenanceSheet.Cells[row, 4].Value = "Amount";
            FormatHeaderRow(maintenanceSheet, row, 1, 4);
            
            row++;
            decimal totalMaintenance = 0;
            foreach (var maint in maintenance)
            {
                maintenanceSheet.Cells[row, 1].Value = maint.MaintenanceDate.ToString("dd-MMM-yyyy");
                maintenanceSheet.Cells[row, 2].Value = maint.VehicleNo;
                maintenanceSheet.Cells[row, 3].Value = maint.Description;
                maintenanceSheet.Cells[row, 4].Value = maint.Amount;
                
                maintenanceSheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                
                totalMaintenance += maint.Amount;
                row++;
            }
            
            // Total row
            maintenanceSheet.Cells[row, 1].Value = "TOTAL";
            maintenanceSheet.Cells[row, 1, row, 3].Merge = true;
            maintenanceSheet.Cells[row, 4].Value = totalMaintenance;
            FormatTotalRow(maintenanceSheet, row, 1, 4);
            maintenanceSheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
            maintenanceSheet.Cells.AutoFitColumns();
            
            // 8. SUMMARY SHEET
            var summarySheet = package.Workbook.Worksheets.Add("Summary");
            summarySheet.Cells["A1"].Value = "DATABASE SUMMARY";
            summarySheet.Cells["A1:B1"].Merge = true;
            summarySheet.Cells["A1"].Style.Font.Size = 16;
            summarySheet.Cells["A1"].Style.Font.Bold = true;
            summarySheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            row = 3;
            summarySheet.Cells[row, 1].Value = "Export Date:";
            summarySheet.Cells[row, 2].Value = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            summarySheet.Cells[row, 1].Style.Font.Bold = true;
            row += 2;
            
            summarySheet.Cells[row, 1].Value = "MASTER DATA";
            summarySheet.Cells[row, 1].Style.Font.Bold = true;
            summarySheet.Cells[row, 1].Style.Font.Size = 12;
            row++;
            
            summarySheet.Cells[row, 1].Value = "Total Vehicles:";
            summarySheet.Cells[row, 2].Value = vehicles.Count;
            row++;
            summarySheet.Cells[row, 1].Value = "Total Vendors:";
            summarySheet.Cells[row, 2].Value = vendors.Count;
            row++;
            summarySheet.Cells[row, 1].Value = "Total Buyers:";
            summarySheet.Cells[row, 2].Value = buyers.Count;
            row++;
            summarySheet.Cells[row, 1].Value = "Total Materials:";
            summarySheet.Cells[row, 2].Value = materials.Count;
            row += 2;
            
            summarySheet.Cells[row, 1].Value = "TRANSACTION DATA";
            summarySheet.Cells[row, 1].Style.Font.Bold = true;
            summarySheet.Cells[row, 1].Style.Font.Size = 12;
            row++;
            
            summarySheet.Cells[row, 1].Value = "Total Sales:";
            summarySheet.Cells[row, 2].Value = sales.Count;
            row++;
            summarySheet.Cells[row, 1].Value = "Total Sales Amount:";
            summarySheet.Cells[row, 2].Value = totalSales;
            summarySheet.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
            row++;
            
            summarySheet.Cells[row, 1].Value = "Total Purchases:";
            summarySheet.Cells[row, 2].Value = purchases.Count;
            row++;
            summarySheet.Cells[row, 1].Value = "Total Purchases Amount:";
            summarySheet.Cells[row, 2].Value = totalPurchases;
            summarySheet.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
            row++;
            
            summarySheet.Cells[row, 1].Value = "Total Maintenance:";
            summarySheet.Cells[row, 2].Value = maintenance.Count;
            row++;
            summarySheet.Cells[row, 1].Value = "Total Maintenance Amount:";
            summarySheet.Cells[row, 2].Value = totalMaintenance;
            summarySheet.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
            row += 2;
            
            summarySheet.Cells[row, 1].Value = "NET PROFIT:";
            summarySheet.Cells[row, 1].Style.Font.Bold = true;
            summarySheet.Cells[row, 1].Style.Font.Size = 12;
            summarySheet.Cells[row, 2].Value = totalSales - totalPurchases - totalMaintenance;
            summarySheet.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
            summarySheet.Cells[row, 2].Style.Font.Bold = true;
            summarySheet.Cells[row, 2].Style.Font.Size = 12;
            
            if (totalSales - totalPurchases - totalMaintenance >= 0)
                summarySheet.Cells[row, 2].Style.Font.Color.SetColor(Color.Green);
            else
                summarySheet.Cells[row, 2].Style.Font.Color.SetColor(Color.Red);
            
            summarySheet.Cells.AutoFitColumns();
            
            // Move summary to first position
            package.Workbook.Worksheets.MoveToStart("Summary");
            
            // Save
            package.SaveAs(new FileInfo(filePath));
            
            Logger.LogInfo($"Complete data export generated: {filePath}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to generate complete data export");
            throw;
        }
    }
    
    /// <summary>
    /// Helper method to format header rows
    /// </summary>
    private static void FormatHeaderRow(ExcelWorksheet worksheet, int row, int startCol, int endCol)
    {
        using var range = worksheet.Cells[row, startCol, row, endCol];
        range.Style.Font.Bold = true;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    }
    
    /// <summary>
    /// Helper method to format total rows
    /// </summary>
    private static void FormatTotalRow(ExcelWorksheet worksheet, int row, int startCol, int endCol)
    {
        using var range = worksheet.Cells[row, startCol, row, endCol];
        range.Style.Font.Bold = true;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
    }
}
