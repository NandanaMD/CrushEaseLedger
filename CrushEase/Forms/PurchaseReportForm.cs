using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class PurchaseReportForm : Form
{
    private List<Vehicle> _vehicles;
    private List<Material> _materials;
    private List<Vendor> _vendors;
    private List<Purchase> _currentData;
    
    public PurchaseReportForm()
    {
        InitializeComponent();
        _vehicles = new List<Vehicle>();
        _materials = new List<Material>();
        _vendors = new List<Vendor>();
        _currentData = new List<Purchase>();
    }
    
    private void PurchaseReportForm_Load(object sender, EventArgs e)
    {
        // Set default date range to current month
        var today = DateTime.Today;
        dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
        dtpToDate.Value = today;
        
        LoadMasterData();
        LoadReport();
        
        // Enable double-click to edit
        dgvReport.CellDoubleClick += DgvReport_CellDoubleClick;
    }
    
    private void LoadMasterData()
    {
        try
        {
            // Load vehicles
            _vehicles = VehicleRepository.GetAll(activeOnly: true);
            var vehicleList = new List<Vehicle> { new Vehicle { VehicleId = 0, VehicleNo = "All Vehicles" } };
            vehicleList.AddRange(_vehicles);
            cmbVehicle.DataSource = vehicleList;
            cmbVehicle.DisplayMember = "VehicleNo";
            cmbVehicle.ValueMember = "VehicleId";
            cmbVehicle.SelectedIndex = 0;
            
            // Load materials
            _materials = MaterialRepository.GetAll(activeOnly: true);
            var materialList = new List<Material> { new Material { MaterialId = 0, MaterialName = "All Materials" } };
            materialList.AddRange(_materials);
            cmbMaterial.DataSource = materialList;
            cmbMaterial.DisplayMember = "MaterialName";
            cmbMaterial.ValueMember = "MaterialId";
            cmbMaterial.SelectedIndex = 0;
            
            // Load vendors for edit operations
            _vendors = VendorRepository.GetAll(activeOnly: false);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load master data");
            MessageBox.Show("Failed to load data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnGenerate_Click(object sender, EventArgs e)
    {
        LoadReport();
    }
    
    private void LoadReport()
    {
        try
        {
            var fromDate = dtpFromDate.Value.Date;
            var toDate = dtpToDate.Value.Date;
            
            if (fromDate > toDate)
            {
                MessageBox.Show("From Date cannot be greater than To Date", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get vehicle filter
            int? vehicleId = null;
            if (cmbVehicle.SelectedValue is int vid && vid > 0)
                vehicleId = vid;
            
            // Get all purchases
            _currentData = PurchaseRepository.GetAll(fromDate, toDate, vehicleId);
            
            // Apply material filter if needed
            int? materialId = null;
            if (cmbMaterial.SelectedValue is int mid && mid > 0)
            {
                materialId = mid;
                _currentData = _currentData.Where(p => p.MaterialId == materialId).ToList();
            }
            
            // Bind to grid
            dgvReport.DataSource = null;
            dgvReport.DataSource = _currentData;
            
            // Configure columns
            if (dgvReport.Columns.Count > 0)
            {
                dgvReport.Columns["PurchaseId"].Visible = false;
                dgvReport.Columns["VehicleId"].Visible = false;
                dgvReport.Columns["VendorId"].Visible = false;
                dgvReport.Columns["MaterialId"].Visible = false;
                dgvReport.Columns["CreatedAt"].Visible = false;
                
                dgvReport.Columns["PurchaseDate"].HeaderText = "Date";
                dgvReport.Columns["PurchaseDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                dgvReport.Columns["VehicleNo"].HeaderText = "Vehicle";
                dgvReport.Columns["VendorName"].HeaderText = "Vendor";
                dgvReport.Columns["MaterialName"].HeaderText = "Material";
                dgvReport.Columns["VendorSite"].HeaderText = "Vendor Site";
                dgvReport.Columns["Quantity"].DefaultCellStyle.Format = "N2";
                dgvReport.Columns["Rate"].DefaultCellStyle.Format = "N2";
                dgvReport.Columns["Amount"].DefaultCellStyle.Format = "N2";
                
                dgvReport.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvReport.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvReport.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Update total
            var total = _currentData.Sum(p => p.Amount);
            lblTotal.Text = $"Total: ₹{total:N2}";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load purchase report");
            MessageBox.Show("Failed to load report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (_currentData == null || _currentData.Count == 0)
            {
                MessageBox.Show("No data to export", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            using var sfd = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Export Purchase Report",
                FileName = $"PurchaseReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExcelReportGenerator.GeneratePurchaseReport(
                    _currentData,
                    dtpFromDate.Value.Date,
                    dtpToDate.Value.Date,
                    sfd.FileName);
                
                MessageBox.Show("Report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Ask if user wants to open the file
                if (MessageBox.Show("Do you want to open the file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to export purchase report");
            MessageBox.Show("Failed to export report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
    
    private void DgvReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            EditSelectedPurchase();
        }
    }
    
    public void BtnEdit_Click(object sender, EventArgs e)
    {
        EditSelectedPurchase();
    }
    
    private void EditSelectedPurchase()
    {
        if (dgvReport.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a purchase to edit", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var purchase = (Purchase)dgvReport.SelectedRows[0].DataBoundItem;
        
        // Show edit dialog
        using var editForm = new PurchaseEditDialog(purchase, _vehicles, _vendors, _materials);
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Update the purchase
                purchase.PurchaseDate = editForm.PurchaseDate;
                purchase.VehicleId = editForm.VehicleId;
                purchase.VendorId = editForm.VendorId;
                purchase.MaterialId = editForm.MaterialId;
                purchase.Quantity = editForm.Quantity;
                purchase.Rate = editForm.Rate;
                purchase.Amount = editForm.Amount;
                purchase.VendorSite = editForm.VendorSite;
                
                PurchaseRepository.Update(purchase);
                
                LoadReport();
                MessageBox.Show("Purchase updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update purchase");
                MessageBox.Show("Failed to update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    public void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dgvReport.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a purchase to delete", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var purchase = (Purchase)dgvReport.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete this purchase?\\n\\nDate: {purchase.PurchaseDate:dd-MMM-yyyy}\\nVehicle: {purchase.VehicleNo}\\nVendor: {purchase.VendorName}\\nAmount: ₹{purchase.Amount:N2}",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            PurchaseRepository.Delete(purchase.PurchaseId);
            LoadReport();
            MessageBox.Show("Purchase deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete purchase");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

/// <summary>
/// Dialog for editing purchase details
/// </summary>
internal class PurchaseEditDialog : Form
{
    private DateTimePicker dtpPurchaseDate;
    private ComboBox cmbVehicle;
    private ComboBox cmbVendor;
    private ComboBox cmbMaterial;
    private TextBox txtQuantity;
    private TextBox txtRate;
    private TextBox txtAmount;
    private TextBox txtVendorSite;
    
    public DateTime PurchaseDate => dtpPurchaseDate.Value.Date;
    public int VehicleId => (int)cmbVehicle.SelectedValue;
    public int VendorId => (int)cmbVendor.SelectedValue;
    public int MaterialId => (int)cmbMaterial.SelectedValue;
    public decimal Quantity => decimal.Parse(txtQuantity.Text);
    public decimal Rate => decimal.Parse(txtRate.Text);
    public decimal Amount => decimal.Parse(txtAmount.Text);
    public string? VendorSite => string.IsNullOrWhiteSpace(txtVendorSite.Text) ? null : txtVendorSite.Text.Trim();
    
    public PurchaseEditDialog(Purchase purchase, List<Vehicle> vehicles, List<Vendor> vendors, List<Material> materials)
    {
        InitializeComponent();
        
        // Load master data
        cmbVehicle.DataSource = VehicleRepository.GetAll(activeOnly: false);
        cmbVehicle.DisplayMember = "VehicleNo";
        cmbVehicle.ValueMember = "VehicleId";
        
        cmbVendor.DataSource = VendorRepository.GetAll(activeOnly: false);
        cmbVendor.DisplayMember = "VendorName";
        cmbVendor.ValueMember = "VendorId";
        
        cmbMaterial.DataSource = MaterialRepository.GetAll(activeOnly: false);
        cmbMaterial.DisplayMember = "MaterialName";
        cmbMaterial.ValueMember = "MaterialId";
        
        // Set current values
        dtpPurchaseDate.Value = purchase.PurchaseDate;
        cmbVehicle.SelectedValue = purchase.VehicleId;
        cmbVendor.SelectedValue = purchase.VendorId;
        cmbMaterial.SelectedValue = purchase.MaterialId;
        txtQuantity.Text = purchase.Quantity.ToString();
        txtRate.Text = purchase.Rate.ToString();
        txtAmount.Text = purchase.Amount.ToString("N2");
        txtVendorSite.Text = purchase.VendorSite ?? "";
        
        // Auto-calculate amount
        txtQuantity.TextChanged += (s, e) => CalculateAmount();
        txtRate.TextChanged += (s, e) => CalculateAmount();
    }
    
    private void CalculateAmount()
    {
        if (decimal.TryParse(txtQuantity.Text, out var quantity) && 
            decimal.TryParse(txtRate.Text, out var rate))
        {
            txtAmount.Text = (quantity * rate).ToString("N2");
        }
    }
    
    private void InitializeComponent()
    {
        this.Width = 500;
        this.Height = 450;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Text = "Edit Purchase";
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        var y = 20;
        
        var lblDate = new Label { Left = 20, Top = y, Width = 100, Text = "Purchase Date:" };
        dtpPurchaseDate = new DateTimePicker { Left = 130, Top = y - 3, Width = 330, Format = DateTimePickerFormat.Short };
        
        y += 35;
        var lblVehicle = new Label { Left = 20, Top = y, Width = 100, Text = "Vehicle:" };
        cmbVehicle = new ComboBox { Left = 130, Top = y - 3, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
        
        y += 35;
        var lblVendor = new Label { Left = 20, Top = y, Width = 100, Text = "Vendor:" };
        cmbVendor = new ComboBox { Left = 130, Top = y - 3, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
        
        y += 35;
        var lblMaterial = new Label { Left = 20, Top = y, Width = 100, Text = "Material:" };
        cmbMaterial = new ComboBox { Left = 130, Top = y - 3, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
        
        y += 35;
        var lblVendorSite = new Label { Left = 20, Top = y, Width = 100, Text = "Vendor Site:" };
        txtVendorSite = new TextBox { Left = 130, Top = y - 3, Width = 330 };
        
        y += 35;
        var lblQuantity = new Label { Left = 20, Top = y, Width = 100, Text = "Quantity:" };
        txtQuantity = new TextBox { Left = 130, Top = y - 3, Width = 150 };
        
        y += 35;
        var lblRate = new Label { Left = 20, Top = y, Width = 100, Text = "Rate:" };
        txtRate = new TextBox { Left = 130, Top = y - 3, Width = 150 };
        
        y += 35;
        var lblAmount = new Label { Left = 20, Top = y, Width = 100, Text = "Amount:" };
        txtAmount = new TextBox { Left = 130, Top = y - 3, Width = 150, ReadOnly = true };
        
        y += 50;
        var btnOK = new Button { Text = "Save", Left = 260, Width = 90, Top = y, DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Cancel", Left = 360, Width = 90, Top = y, DialogResult = DialogResult.Cancel };
        
        btnOK.Click += (s, e) => { 
            if (!ValidateForm()) { 
                ((Form)s).DialogResult = DialogResult.None;
            } 
        };
        
        this.Controls.Add(lblDate);
        this.Controls.Add(dtpPurchaseDate);
        this.Controls.Add(lblVehicle);
        this.Controls.Add(cmbVehicle);
        this.Controls.Add(lblVendor);
        this.Controls.Add(cmbVendor);
        this.Controls.Add(lblMaterial);
        this.Controls.Add(cmbMaterial);
        this.Controls.Add(lblVendorSite);
        this.Controls.Add(txtVendorSite);
        this.Controls.Add(lblQuantity);
        this.Controls.Add(txtQuantity);
        this.Controls.Add(lblRate);
        this.Controls.Add(txtRate);
        this.Controls.Add(lblAmount);
        this.Controls.Add(txtAmount);
        this.Controls.Add(btnOK);
        this.Controls.Add(btnCancel);
        
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
    }
    
    private bool ValidateForm()
    {
        if (cmbVehicle.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a vehicle", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        if (cmbVendor.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a vendor", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        if (cmbMaterial.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a material", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        if (!decimal.TryParse(txtQuantity.Text, out var quantity) || quantity <= 0)
        {
            MessageBox.Show("Please enter a valid quantity (greater than 0)", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        if (!decimal.TryParse(txtRate.Text, out var rate) || rate <= 0)
        {
            MessageBox.Show("Please enter a valid rate (greater than 0)", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        return true;
    }
}
