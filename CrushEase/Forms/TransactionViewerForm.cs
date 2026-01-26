using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

/// <summary>
/// Form for viewing all transactions with filtering and edit/delete capabilities
/// </summary>
public partial class TransactionViewerForm : Form
{
    public TransactionViewerForm()
    {
        InitializeComponent();
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
        
        // Load dropdown data
        LoadDropdowns();
        
        // Load all transaction data
        LoadAllData();
        
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
            if (col != null)
            {
                col.HeaderText = "Quantity";
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
            if (col != null)
            {
                col.HeaderText = "Quantity";
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
            MessageBox.Show("Please select a sale to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var sale = (Sale)dgvSales.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete this sale?\n\n" +
            $"Date: {sale.SaleDate:dd-MMM-yyyy}\n" +
            $"Vehicle: {sale.VehicleNo}\n" +
            $"Buyer: {sale.BuyerName}\n" +
            $"Amount: ₹{sale.Amount:N2}",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result == DialogResult.Yes)
        {
            try
            {
                SaleRepository.Delete(sale.SaleId);
                MessageBox.Show("Sale deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSales();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete sale");
                MessageBox.Show("Failed to delete sale: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnDeletePurchase_Click(object sender, EventArgs e)
    {
        if (dgvPurchases.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a purchase to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var purchase = (Purchase)dgvPurchases.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete this purchase?\n\n" +
            $"Date: {purchase.PurchaseDate:dd-MMM-yyyy}\n" +
            $"Vehicle: {purchase.VehicleNo}\n" +
            $"Vendor: {purchase.VendorName}\n" +
            $"Amount: ₹{purchase.Amount:N2}",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result == DialogResult.Yes)
        {
            try
            {
                PurchaseRepository.Delete(purchase.PurchaseId);
                MessageBox.Show("Purchase deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPurchases();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete purchase");
                MessageBox.Show("Failed to delete purchase: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnDeleteMaintenance_Click(object sender, EventArgs e)
    {
        if (dgvMaintenance.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a maintenance record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var maintenance = (Maintenance)dgvMaintenance.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete this maintenance record?\n\n" +
            $"Date: {maintenance.MaintenanceDate:dd-MMM-yyyy}\n" +
            $"Vehicle: {maintenance.VehicleNo}\n" +
            $"Description: {maintenance.Description}\n" +
            $"Amount: ₹{maintenance.Amount:N2}",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result == DialogResult.Yes)
        {
            try
            {
                MaintenanceRepository.Delete(maintenance.MaintenanceId);
                MessageBox.Show("Maintenance record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMaintenance();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete maintenance record");
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
}
