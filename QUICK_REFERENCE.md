# CrushEase - Developer Quick Reference

## 🚀 Build & Run

```powershell
cd O:\Coding\CrushEase
# Open in Visual Studio and press F5
# OR if .NET SDK installed:
dotnet build
dotnet run --project CrushEase\CrushEase.csproj
```

---

## 📋 Copy-Paste Patterns

### 1. Creating a Master Form (Copy VehicleMasterForm)

**Steps:**
1. Copy `VehicleMasterForm.cs` and `VehicleMasterForm.Designer.cs`
2. Rename to `XxxMasterForm.cs` and `XxxMasterForm.Designer.cs`
3. Find/Replace: `Vehicle` → `Xxx`
4. Update fields in Designer if entity has more than just Name field
5. Update repository calls: `VehicleRepository` → `XxxRepository`

**Example for Vendor:**
```csharp
// In Designer.cs - add Contact and Notes fields
this.txtContact = new TextBox();
this.txtNotes = new TextBox();
// ... layout code ...

// In Form.cs - BtnSave_Click
var vendor = new Vendor
{
    VendorName = txtVendorName.Text.Trim(),
    Contact = txtContact.Text.Trim(),
    Notes = txtNotes.Text.Trim(),
    IsActive = true
};
VendorRepository.Insert(vendor);
```

---

### 2. Creating Transaction Entry Form

**Template Structure:**
```csharp
public partial class SaleEntryForm : Form
{
    private int? _lastVehicleId;
    private int? _lastBuyerId;
    
    private void SaleEntryForm_Load(object sender, EventArgs e)
    {
        dtpDate.Value = DateTime.Today;
        LoadDropdowns();
        
        txtQuantity.TextChanged += (s, e) => CalculateAmount();
        txtRate.TextChanged += (s, e) => CalculateAmount();
    }
    
    private void LoadDropdowns()
    {
        cmbVehicle.DataSource = VehicleRepository.GetAll();
        cmbVehicle.DisplayMember = "VehicleNo";
        cmbVehicle.ValueMember = "VehicleId";
        
        // Restore last selection
        if (_lastVehicleId.HasValue)
            cmbVehicle.SelectedValue = _lastVehicleId;
    }
    
    private void CalculateAmount()
    {
        if (decimal.TryParse(txtQuantity.Text, out var qty) && 
            decimal.TryParse(txtRate.Text, out var rate))
        {
            txtAmount.Text = (qty * rate).ToString("N2");
        }
    }
    
    private bool SaveSale()
    {
        if (!Validate()) return false;
        
        var sale = new Sale
        {
            SaleDate = dtpDate.Value.Date,
            VehicleId = (int)cmbVehicle.SelectedValue,
            BuyerId = (int)cmbBuyer.SelectedValue,
            MaterialId = (int)cmbMaterial.SelectedValue,
            Quantity = decimal.Parse(txtQuantity.Text),
            Rate = decimal.Parse(txtRate.Text),
            Amount = decimal.Parse(txtAmount.Text)
        };
        
        try
        {
            SaleRepository.Insert(sale);
            Logger.LogInfo($"Sale saved: {sale.Amount}");
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            MessageBox.Show("Save failed");
            return false;
        }
    }
    
    private void BtnSaveAndAddAnother_Click(object sender, EventArgs e)
    {
        if (SaveSale())
        {
            // Remember selections
            _lastVehicleId = (int?)cmbVehicle.SelectedValue;
            _lastBuyerId = (int?)cmbBuyer.SelectedValue;
            
            // Clear inputs
            txtQuantity.Clear();
            txtRate.Clear();
            txtAmount.Clear();
            
            // Restore selections
            cmbVehicle.SelectedValue = _lastVehicleId;
            cmbBuyer.SelectedValue = _lastBuyerId;
            
            txtQuantity.Focus();
        }
    }
}
```

---

### 3. Creating Excel Report Generator

**Add to Services/ExcelReportGenerator.cs:**

```csharp
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CrushEase.Services;

public static class ExcelReportGenerator
{
    static ExcelReportGenerator()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
    
    public static void GenerateSalesReport(List<Sale> sales, DateTime from, DateTime to, string path)
    {
        using var pkg = new ExcelPackage();
        var ws = pkg.Workbook.Worksheets.Add("Sales Report");
        
        // Title
        ws.Cells["A1"].Value = "SALES REPORT";
        ws.Cells["A1"].Style.Font.Size = 16;
        ws.Cells["A1"].Style.Font.Bold = true;
        
        // Date range
        ws.Cells["A2"].Value = $"From {from:dd-MMM-yyyy} to {to:dd-MMM-yyyy}";
        
        // Headers (row 4)
        string[] headers = { "Date", "Vehicle", "Buyer", "Material", "Qty", "Rate", "Amount" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cells[4, i + 1];
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }
        
        // Data
        int row = 5;
        decimal total = 0;
        foreach (var sale in sales)
        {
            ws.Cells[row, 1].Value = sale.SaleDate.ToString("dd-MMM-yyyy");
            ws.Cells[row, 2].Value = sale.VehicleNo;
            ws.Cells[row, 3].Value = sale.BuyerName;
            ws.Cells[row, 4].Value = sale.MaterialName;
            ws.Cells[row, 5].Value = sale.Quantity;
            ws.Cells[row, 6].Value = sale.Rate;
            ws.Cells[row, 7].Value = sale.Amount;
            ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
            total += sale.Amount;
            row++;
        }
        
        // Total
        ws.Cells[row, 6].Value = "TOTAL:";
        ws.Cells[row, 6].Style.Font.Bold = true;
        ws.Cells[row, 7].Value = total;
        ws.Cells[row, 7].Style.Font.Bold = true;
        ws.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
        
        // Formatting
        ws.Cells.AutoFitColumns();
        ws.PrinterSettings.Orientation = eOrientation.Landscape;
        
        pkg.SaveAs(new FileInfo(path));
    }
}
```

---

### 4. Creating Report Form

**Template:**
```csharp
public partial class SalesReportForm : Form
{
    private void SalesReportForm_Load(object sender, EventArgs e)
    {
        // Default to current month
        var now = DateTime.Today;
        dtpFrom.Value = new DateTime(now.Year, now.Month, 1);
        dtpTo.Value = now;
        
        LoadFilters();
    }
    
    private void LoadFilters()
    {
        var vehicles = new List<ComboItem> { new("All", null) };
        vehicles.AddRange(VehicleRepository.GetAll()
            .Select(v => new ComboItem(v.VehicleNo, v.VehicleId)));
        cmbVehicle.DataSource = vehicles;
    }
    
    private void BtnGenerate_Click(object sender, EventArgs e)
    {
        var sales = SaleRepository.GetAll(
            dtpFrom.Value.Date,
            dtpTo.Value.Date,
            cmbVehicle.SelectedValue as int?
        );
        
        dgvPreview.DataSource = sales;
        lblTotal.Text = $"Total: ₹{sales.Sum(s => s.Amount):N2}";
    }
    
    private void BtnExport_Click(object sender, EventArgs e)
    {
        if (dgvPreview.Rows.Count == 0)
        {
            MessageBox.Show("No data");
            return;
        }
        
        using var dlg = new SaveFileDialog();
        dlg.Filter = "Excel|*.xlsx";
        dlg.FileName = $"Sales_{DateTime.Now:yyyyMMdd}.xlsx";
        
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            ExcelReportGenerator.GenerateSalesReport(
                (List<Sale>)dgvPreview.DataSource,
                dtpFrom.Value,
                dtpTo.Value,
                dlg.FileName
            );
            
            MessageBox.Show("Exported successfully!");
            
            if (MessageBox.Show("Open?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo 
                { 
                    FileName = dlg.FileName, 
                    UseShellExecute = true 
                });
            }
        }
    }
}

class ComboItem
{
    public string Display { get; set; }
    public int? Value { get; set; }
    public ComboItem(string d, int? v) { Display = d; Value = v; }
}
```

---

## 🔍 Quick Troubleshooting

### Problem: Form doesn't show in Designer
**Fix:** Rebuild project, close and reopen file

### Problem: "Database is locked"
**Fix:** Already handled with `lock` in DatabaseManager - shouldn't happen

### Problem: "EPPlus license exception"
**Fix:** Add at top of ExcelReportGenerator:
```csharp
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
```

### Problem: Amount calculation wrong
**Fix:** Use `decimal`, not `float`/`double`:
```csharp
decimal qty = decimal.Parse(txtQty.Text);
decimal rate = decimal.Parse(txtRate.Text);
decimal amount = qty * rate; // Correct precision
```

### Problem: Date format inconsistent
**Fix:** Always use ISO format for DB:
```csharp
sale.SaleDate.ToString("yyyy-MM-dd") // DB storage
sale.SaleDate.ToString("dd-MMM-yyyy") // Display
```

---

## 📊 Testing Checklist

### New Form Testing:
- [ ] Form opens without errors
- [ ] All dropdowns populate
- [ ] Save works
- [ ] Validation prevents bad data
- [ ] Error messages are user-friendly
- [ ] Tab order is logical
- [ ] Enter key submits (set AcceptButton)
- [ ] Esc key closes (set CancelButton)

### Report Testing:
- [ ] Preview shows data
- [ ] Filters work
- [ ] Export creates file
- [ ] Excel opens without errors
- [ ] Headers formatted correctly
- [ ] Totals are accurate
- [ ] Print layout is landscape
- [ ] Large datasets don't crash

---

## 🎨 UI Design Standards

### Colors:
```csharp
Color.Green  // Profit, Success
Color.Red    // Loss, Error
Color.Orange // Warning
Color.LightGray // Headers
```

### Fonts:
```csharp
new Font("Segoe UI", 10F, FontStyle.Bold)  // Labels
new Font("Segoe UI", 12F, FontStyle.Bold)  // Key metrics
new Font("Segoe UI", 9F)                   // Regular text
```

### Spacing:
- Form padding: 12px
- Control spacing: 6-10px
- Group box padding: 15px
- Button width: 75-100px
- Button height: 25-30px

---

## 🗂️ File Locations

```
Application Folder
├── Data/
│   └── crushease.db          # Main database
├── Backups/
│   ├── crushease_daily_*.db   # Last 7 days
│   └── crushease_weekly_*.db  # Last 4 weeks
└── Logs/
    └── crushease-*.log        # Daily log files
```

---

## 💾 Database Queries Cheat Sheet

```sql
-- Get today's sales total
SELECT SUM(amount) FROM sales 
WHERE sale_date = date('now');

-- Get monthly profit
SELECT 
  (SELECT SUM(amount) FROM sales WHERE sale_date >= date('now','start of month')) -
  (SELECT SUM(amount) FROM purchases WHERE purchase_date >= date('now','start of month')) -
  (SELECT SUM(amount) FROM maintenance WHERE maintenance_date >= date('now','start of month'));

-- Vehicle profit summary
SELECT 
  v.vehicle_no,
  COALESCE(SUM(s.amount), 0) AS sales,
  COALESCE(SUM(p.amount), 0) AS purchases,
  COALESCE(SUM(m.amount), 0) AS maintenance
FROM vehicles v
LEFT JOIN sales s ON v.vehicle_id = s.vehicle_id
LEFT JOIN purchases p ON v.vehicle_id = p.vehicle_id
LEFT JOIN maintenance m ON v.vehicle_id = m.vehicle_id
GROUP BY v.vehicle_id;
```

---

## ⚡ Performance Tips

1. **Always use date indexes:** Already created on all date columns
2. **Limit preview grids:** Use `.Take(100)` for large datasets
3. **Async for Excel export:** Use `Task.Run(() => Generate())` for 1000+ records
4. **Cache master lists:** Load once, reuse in memory
5. **Use DataBindingSource:** For automatic grid updates

---

## 🎯 Priority Implementation Order

1. ✅ Already done (Infrastructure)
2. **Next:** VendorMasterForm (1 hour)
3. **Next:** BuyerMasterForm (1 hour)
4. **Next:** MaterialMasterForm (1 hour)
5. **Then:** SaleEntryForm (2 hours)
6. **Then:** PurchaseEntryForm (1 hour)
7. **Then:** MaintenanceEntryForm (1 hour)
8. **Then:** ExcelReportGenerator (2 hours)
9. **Then:** All 4 report forms (3 hours)
10. **Finally:** Testing and polish (2 hours)

**Total: ~15 hours of focused work**

---

## 📞 Where to Find Help

- **SQLite docs:** https://www.sqlite.org/lang.html
- **EPPlus wiki:** https://github.com/EPPlusSoftware/EPPlus/wiki
- **WinForms docs:** https://learn.microsoft.com/en-us/dotnet/desktop/winforms/
- **C# reference:** https://learn.microsoft.com/en-us/dotnet/csharp/

---

**Good luck! The foundation is solid. Just follow the patterns.**

*Pro tip: Work on one form at a time, test immediately, commit to version control.*
