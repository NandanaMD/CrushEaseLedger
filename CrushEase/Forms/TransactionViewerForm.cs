using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

/// <summary>
/// Form for viewing all transactions with filtering and edit/delete capabilities
/// </summary>
public partial class TransactionViewerForm : Form
{
    private string _searchText = "";
    
    public TransactionViewerForm()
    {
        InitializeComponent();
    }
    
    public TransactionViewerForm(string searchText) : this()
    {
        _searchText = searchText ?? "";
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Delete)
        {
            // Handle Delete key based on active tab
            if (tabControl.SelectedTab == tabSales)
            {
                BtnDeleteSale_Click(this, EventArgs.Empty);
            }
            else if (tabControl.SelectedTab == tabPurchases)
            {
                BtnDeletePurchase_Click(this, EventArgs.Empty);
            }
            else if (tabControl.SelectedTab == tabMaintenance)
            {
                BtnDeleteMaintenance_Click(this, EventArgs.Empty);
            }
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
    
    private void TransactionViewerForm_Load(object sender, EventArgs e)
    {
        // Initialize date filters to current month
        dtpSalesFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        dtpSalesTo.Value = DateTime.Now;
        dtpPurchasesFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        dtpPurchasesTo.Value = DateTime.Now;
        dtpMaintenanceFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        dtpMaintenanceTo.Value = DateTime.Now;
        
        // Set initial search text if provided
        if (!string.IsNullOrEmpty(_searchText))
        {
            txtSalesSearch.Text = _searchText;
            txtPurchasesSearch.Text = _searchText;
            txtMaintenanceSearch.Text = _searchText;
        }
        
        // Load dropdown data
        LoadDropdowns();
        
        // Load all transaction data
        LoadAllData();
        
        // Initialize invoice context menus
        InitializeInvoiceContextMenus();
        
        // Apply modern theme
        Utils.ModernTheme.ApplyToForm(this);
    }
    
    private void LoadDropdowns()
    {
        var vehicles = VehicleRepository.GetAll();
        
        // Sales vehicle filter
        cmbSalesVehicle.DataSource = new[] { new Vehicle { VehicleId = 0, VehicleNo = "All Vehicles" } }.Concat(vehicles).ToList();
        cmbSalesVehicle.DisplayMember = "VehicleNo";
        cmbSalesVehicle.ValueMember = "VehicleId";
        
        // Purchases vehicle filter
        cmbPurchasesVehicle.DataSource = new[] { new Vehicle { VehicleId = 0, VehicleNo = "All Vehicles" } }.Concat(vehicles).ToList();
        cmbPurchasesVehicle.DisplayMember = "VehicleNo";
        cmbPurchasesVehicle.ValueMember = "VehicleId";
        
        // Maintenance vehicle filter
        cmbMaintenanceVehicle.DataSource = new[] { new Vehicle { VehicleId = 0, VehicleNo = "All Vehicles" } }.Concat(vehicles).ToList();
        cmbMaintenanceVehicle.DisplayMember = "VehicleNo";
        cmbMaintenanceVehicle.ValueMember = "VehicleId";
    }
    
    private void LoadAllData()
    {
        LoadSales();
        LoadPurchases();
        LoadMaintenance();
    }
    
    private void LoadSales()
    {
        var fromDate = dtpSalesFrom.Value.Date;
        var toDate = dtpSalesTo.Value.Date;
        int? vehicleId = (cmbSalesVehicle.SelectedValue is int value && value > 0) ? value : null;
        
        var sales = SaleRepository.GetAll(fromDate, toDate, vehicleId);
        
        // Filter by search text
        string searchText = txtSalesSearch?.Text?.Trim() ?? "";
        if (!string.IsNullOrEmpty(searchText))
        {
            sales = sales.Where(s => 
                (s.VehicleNo?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (s.BuyerName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (s.MaterialName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                s.Amount.ToString().Contains(searchText)
            ).ToList();
        }
        
        dgvSales.DataSource = sales;
        FormatSalesGrid();
        lblSalesCount.Text = $"Total: {sales.Count} | Amount: ₹{sales.Sum(s => s.Amount):N2}";
    }
    
    private void LoadPurchases()
    {
        var fromDate = dtpPurchasesFrom.Value.Date;
        var toDate = dtpPurchasesTo.Value.Date;
        int? vehicleId = (cmbPurchasesVehicle.SelectedValue is int value && value > 0) ? value : null;
        
        var purchases = PurchaseRepository.GetAll(fromDate, toDate, vehicleId);
        
        // Filter by search text
        string searchText = txtPurchasesSearch?.Text?.Trim() ?? "";
        if (!string.IsNullOrEmpty(searchText))
        {
            purchases = purchases.Where(p => 
                (p.VehicleNo?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.VendorName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.MaterialName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                p.Amount.ToString().Contains(searchText)
            ).ToList();
        }
        
        dgvPurchases.DataSource = purchases;
        FormatPurchasesGrid();
        lblPurchasesCount.Text = $"Total: {purchases.Count} | Amount: ₹{purchases.Sum(p => p.Amount):N2}";
    }
    
    private void LoadMaintenance()
    {
        var fromDate = dtpMaintenanceFrom.Value.Date;
        var toDate = dtpMaintenanceTo.Value.Date;
        int? vehicleId = (cmbMaintenanceVehicle.SelectedValue is int value && value > 0) ? value : null;
        
        var maintenance = MaintenanceRepository.GetAll(fromDate, toDate, vehicleId);
        
        // Filter by search text
        string searchText = txtMaintenanceSearch?.Text?.Trim() ?? "";
        if (!string.IsNullOrEmpty(searchText))
        {
            maintenance = maintenance.Where(m => 
                (m.VehicleNo?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (m.Description?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                m.Amount.ToString().Contains(searchText)
            ).ToList();
        }
        
        dgvMaintenance.DataSource = maintenance;
        FormatMaintenanceGrid();
        lblMaintenanceCount.Text = $"Total: {maintenance.Count} | Amount: ₹{maintenance.Sum(m => m.Amount):N2}";
    }
    
    private void FormatSalesGrid()
    {
        try
        {
            if (dgvSales.Columns.Count == 0) return;
            
            var col = dgvSales.Columns["SaleId"];
            if (col != null)
            {
                col.HeaderText = "ID";
                col.Width = 50;
            }
            
            col = dgvSales.Columns["SaleDate"];
            if (col != null)
            {
                col.HeaderText = "Date";
                col.DefaultCellStyle.Format = "dd-MMM-yyyy";
            }
            
            col = dgvSales.Columns["VehicleNo"];
            if (col != null) col.HeaderText = "Vehicle";
            
            col = dgvSales.Columns["BuyerName"];
            if (col != null) col.HeaderText = "Buyer";
            
            col = dgvSales.Columns["MaterialName"];
            if (col != null) col.HeaderText = "Material";
            
            col = dgvSales.Columns["Quantity"];
            if (col != null) col.Visible = false; // Hide old Quantity
            
            col = dgvSales.Columns["InputUnit"];
            if (col != null)
            {
                col.HeaderText = "Unit";
                col.Width = 50;
            }
            
            col = dgvSales.Columns["InputQuantity"];
            if (col != null)
            {
                col.HeaderText = "Qty";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvSales.Columns["CalculatedCFT"];
            if (col != null)
            {
                col.HeaderText = "CFT";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvSales.Columns["Rate"];
            if (col != null)
            {
                col.HeaderText = "Rate";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvSales.Columns["Amount"];
            if (col != null)
            {
                col.HeaderText = "Amount";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvSales.Columns["VehicleId"];
            if (col != null) col.Visible = false;
            
            col = dgvSales.Columns["BuyerId"];
            if (col != null) col.Visible = false;
            
            col = dgvSales.Columns["MaterialId"];
            if (col != null) col.Visible = false;
            
            col = dgvSales.Columns["CreatedAt"];
            if (col != null) col.Visible = false;
            
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        catch (Exception)
        {
            // Silently ignore formatting errors when no data exists
        }
    }
    
    private void FormatPurchasesGrid()
    {
        try
        {
            if (dgvPurchases.Columns.Count == 0) return;
            
            var col = dgvPurchases.Columns["PurchaseId"];
            if (col != null)
            {
                col.HeaderText = "ID";
                col.Width = 50;
            }
            
            col = dgvPurchases.Columns["PurchaseDate"];
            if (col != null)
            {
                col.HeaderText = "Date";
                col.DefaultCellStyle.Format = "dd-MMM-yyyy";
            }
            
            col = dgvPurchases.Columns["VehicleNo"];
            if (col != null) col.HeaderText = "Vehicle";
            
            col = dgvPurchases.Columns["VendorName"];
            if (col != null) col.HeaderText = "Vendor";
            
            col = dgvPurchases.Columns["MaterialName"];
            if (col != null) col.HeaderText = "Material";
            
            col = dgvPurchases.Columns["VendorSite"];
            if (col != null) col.HeaderText = "Site";
            
            col = dgvPurchases.Columns["Quantity"];
            if (col != null) col.Visible = false; // Hide old Quantity
            
            col = dgvPurchases.Columns["InputUnit"];
            if (col != null)
            {
                col.HeaderText = "Unit";
                col.Width = 50;
            }
            
            col = dgvPurchases.Columns["InputQuantity"];
            if (col != null)
            {
                col.HeaderText = "Qty";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvPurchases.Columns["CalculatedCFT"];
            if (col != null)
            {
                col.HeaderText = "CFT";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvPurchases.Columns["Rate"];
            if (col != null)
            {
                col.HeaderText = "Rate";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvPurchases.Columns["Amount"];
            if (col != null)
            {
                col.HeaderText = "Amount";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvPurchases.Columns["VehicleId"];
            if (col != null) col.Visible = false;
            
            col = dgvPurchases.Columns["VendorId"];
            if (col != null) col.Visible = false;
            
            col = dgvPurchases.Columns["MaterialId"];
            if (col != null) col.Visible = false;
            
            col = dgvPurchases.Columns["CreatedAt"];
            if (col != null) col.Visible = false;
            
            dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        catch (Exception)
        {
            // Silently ignore formatting errors when no data exists
        }
    }
    
    private void FormatMaintenanceGrid()
    {
        try
        {
            if (dgvMaintenance.Columns.Count == 0) return;
            
            var col = dgvMaintenance.Columns["MaintenanceId"];
            if (col != null)
            {
                col.HeaderText = "ID";
                col.Width = 50;
            }
            
            col = dgvMaintenance.Columns["MaintenanceDate"];
            if (col != null)
            {
                col.HeaderText = "Date";
                col.DefaultCellStyle.Format = "dd-MMM-yyyy";
            }
            
            col = dgvMaintenance.Columns["VehicleNo"];
            if (col != null) col.HeaderText = "Vehicle";
            
            col = dgvMaintenance.Columns["Description"];
            if (col != null) col.HeaderText = "Description";
            
            col = dgvMaintenance.Columns["Amount"];
            if (col != null)
            {
                col.HeaderText = "Amount";
                col.DefaultCellStyle.Format = "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            col = dgvMaintenance.Columns["VehicleId"];
            if (col != null) col.Visible = false;
            
            col = dgvMaintenance.Columns["CreatedAt"];
            if (col != null) col.Visible = false;
            
            dgvMaintenance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        catch (Exception)
        {
            // Silently ignore formatting errors when no data exists
        }
    }
    
    // Filter event handlers
    private void BtnApplySalesFilter_Click(object sender, EventArgs e)
    {
        LoadSales();
    }
    
    private void BtnApplyPurchasesFilter_Click(object sender, EventArgs e)
    {
        LoadPurchases();
    }
    
    private void BtnApplyMaintenanceFilter_Click(object sender, EventArgs e)
    {
        LoadMaintenance();
    }
    
    // Search textbox event handlers
    private void TxtSalesSearch_TextChanged(object sender, EventArgs e)
    {
        LoadSales();
    }
    
    private void TxtPurchasesSearch_TextChanged(object sender, EventArgs e)
    {
        LoadPurchases();
    }
    
    private void TxtMaintenanceSearch_TextChanged(object sender, EventArgs e)
    {
        LoadMaintenance();
    }
    
    // Edit handlers
    private void BtnEditSale_Click(object sender, EventArgs e)
    {
        if (dgvSales.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a sale to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var sale = (Sale)dgvSales.SelectedRows[0].DataBoundItem;
        var form = new SaleEntryForm(sale.SaleId);
        form.ShowDialog();
        LoadSales();
    }
    
    private void BtnEditPurchase_Click(object sender, EventArgs e)
    {
        if (dgvPurchases.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a purchase to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var purchase = (Purchase)dgvPurchases.SelectedRows[0].DataBoundItem;
        var form = new PurchaseEntryForm(purchase.PurchaseId);
        form.ShowDialog();
        LoadPurchases();
    }
    
    private void BtnEditMaintenance_Click(object sender, EventArgs e)
    {
        if (dgvMaintenance.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a maintenance record to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var maintenance = (Maintenance)dgvMaintenance.SelectedRows[0].DataBoundItem;
        var form = new MaintenanceEntryForm(maintenance.MaintenanceId);
        form.ShowDialog();
        LoadMaintenance();
    }
    
    // Delete handlers
    private void BtnDeleteSale_Click(object sender, EventArgs e)
    {
        if (dgvSales.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select sale(s) to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        int count = dgvSales.SelectedRows.Count;
        var result = MessageBox.Show(
            $"Are you sure you want to delete {count} sale(s)?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result == DialogResult.Yes)
        {
            try
            {
                int successCount = 0;
                foreach (DataGridViewRow row in dgvSales.SelectedRows)
                {
                    var sale = (Sale)row.DataBoundItem;
                    SaleRepository.Delete(sale.SaleId);
                    successCount++;
                }
                MessageBox.Show($"{successCount} sale(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSales();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete sale(s)");
                MessageBox.Show("Failed to delete sale(s): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnDeletePurchase_Click(object sender, EventArgs e)
    {
        if (dgvPurchases.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select purchase(s) to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        int count = dgvPurchases.SelectedRows.Count;
        var result = MessageBox.Show(
            $"Are you sure you want to delete {count} purchase(s)?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result == DialogResult.Yes)
        {
            try
            {
                int successCount = 0;
                foreach (DataGridViewRow row in dgvPurchases.SelectedRows)
                {
                    var purchase = (Purchase)row.DataBoundItem;
                    PurchaseRepository.Delete(purchase.PurchaseId);
                    successCount++;
                }
                MessageBox.Show($"{successCount} purchase(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPurchases();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete purchase(s)");
                MessageBox.Show("Failed to delete purchase(s): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnDeleteMaintenance_Click(object sender, EventArgs e)
    {
        if (dgvMaintenance.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select maintenance record(s) to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        int count = dgvMaintenance.SelectedRows.Count;
        var result = MessageBox.Show(
            $"Are you sure you want to delete {count} maintenance record(s)?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result == DialogResult.Yes)
        {
            try
            {
                int successCount = 0;
                foreach (DataGridViewRow row in dgvMaintenance.SelectedRows)
                {
                    var maintenance = (Maintenance)row.DataBoundItem;
                    MaintenanceRepository.Delete(maintenance.MaintenanceId);
                    successCount++;
                }
                MessageBox.Show($"{successCount} maintenance record(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMaintenance();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete maintenance record(s)");
                MessageBox.Show("Failed to delete maintenance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    // Double-click to edit
    private void DgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            BtnEditSale_Click(sender, e);
        }
    }
    
    private void DgvPurchases_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            BtnEditPurchase_Click(sender, e);
        }
    }
    
    private void DgvMaintenance_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            BtnEditMaintenance_Click(sender, e);
        }
    }
    
    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        LoadAllData();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }
    
    // Invoice generation methods
    private void GenerateSaleInvoice()
    {
        if (dgvSales.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a sale transaction.", "No Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var selectedSale = dgvSales.SelectedRows[0].DataBoundItem as Sale;
        if (selectedSale == null) return;
        
        Cursor = Cursors.WaitCursor;
        try
        {
            // Reload the sale from database to ensure all data is properly loaded
            var sale = SaleRepository.GetById(selectedSale.SaleId);
            if (sale == null)
            {
                MessageBox.Show("Sale not found.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var result = Services.InvoiceGenerator.GenerateSaleInvoice(sale);
            
            if (result.Success)
            {
                var previewForm = new InvoicePreviewForm(result);
                previewForm.ShowDialog(this);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Invoice Generation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
    
    private void GeneratePurchaseInvoice()
    {
        if (dgvPurchases.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a purchase transaction.", "No Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var selectedPurchase = dgvPurchases.SelectedRows[0].DataBoundItem as Purchase;
        if (selectedPurchase == null) return;
        
        Cursor = Cursors.WaitCursor;
        try
        {
            // Reload the purchase from database to ensure all data is properly loaded
            var purchase = PurchaseRepository.GetById(selectedPurchase.PurchaseId);
            if (purchase == null)
            {
                MessageBox.Show("Purchase not found.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var result = Services.InvoiceGenerator.GeneratePurchaseInvoice(purchase);
            
            if (result.Success)
            {
                var previewForm = new InvoicePreviewForm(result);
                previewForm.ShowDialog(this);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Invoice Generation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
    
    // Add context menus for invoice generation
    private void InitializeInvoiceContextMenus()
    {
        // Sales context menu
        var salesContextMenu = new ContextMenuStrip();
        salesContextMenu.Items.Add("Generate Invoice", null, (s, e) => GenerateSaleInvoice());
        salesContextMenu.Items.Add("-"); // Separator
        salesContextMenu.Items.Add("Edit", null, (s, e) => BtnEditSale_Click(s, e));
        salesContextMenu.Items.Add("Delete", null, (s, e) => BtnDeleteSale_Click(s, e));
        dgvSales.ContextMenuStrip = salesContextMenu;
        
        // Purchases context menu
        var purchasesContextMenu = new ContextMenuStrip();
        purchasesContextMenu.Items.Add("Generate Invoice", null, (s, e) => GeneratePurchaseInvoice());
        purchasesContextMenu.Items.Add("-"); // Separator
        purchasesContextMenu.Items.Add("Edit", null, (s, e) => BtnEditPurchase_Click(s, e));
        purchasesContextMenu.Items.Add("Delete", null, (s, e) => BtnDeletePurchase_Click(s, e));
        dgvPurchases.ContextMenuStrip = purchasesContextMenu;
    }
}

