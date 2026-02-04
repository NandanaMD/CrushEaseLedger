using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class SalesReportForm : Form
{
    private List<Vehicle> _vehicles;
    private List<Material> _materials;
    private List<Buyer> _buyers;
    private List<Sale> _currentData;
    
    public SalesReportForm()
    {
        InitializeComponent();
        _vehicles = new List<Vehicle>();
        _materials = new List<Material>();
        _buyers = new List<Buyer>();
        _currentData = new List<Sale>();
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Delete)
        {
            BtnDelete_Click(this, EventArgs.Empty);
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
    
    private void SalesReportForm_Load(object sender, EventArgs e)
    {
        // Set default date range to current month
        var today = DateTime.Today;
        dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
        dtpToDate.Value = today;
        
        LoadMasterData();
        LoadReport();
        
        // Enable double-click to edit
        dgvReport.CellDoubleClick += DgvReport_CellDoubleClick;
        
        // Initialize invoice context menu
        InitializeInvoiceContextMenu();
        
        // Apply modern theme
        Utils.ModernTheme.ApplyToForm(this);
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
            
            // Load buyers
            _buyers = BuyerRepository.GetAll(activeOnly: true);
            var buyerList = new List<Buyer> { new Buyer { BuyerId = 0, BuyerName = "All Buyers" } };
            buyerList.AddRange(_buyers);
            cmbBuyer.DataSource = buyerList;
            cmbBuyer.DisplayMember = "BuyerName";
            cmbBuyer.ValueMember = "BuyerId";
            cmbBuyer.SelectedIndex = 0;
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
            
            // Get all sales
            _currentData = SaleRepository.GetAll(fromDate, toDate, vehicleId);
            
            // Apply material filter if needed
            int? materialId = null;
            if (cmbMaterial.SelectedValue is int mid && mid > 0)
            {
                materialId = mid;
                _currentData = _currentData.Where(s => s.MaterialId == materialId).ToList();
            }
            
            // Apply buyer filter if needed
            int? buyerId = null;
            if (cmbBuyer.SelectedValue is int bid && bid > 0)
            {
                buyerId = bid;
                _currentData = _currentData.Where(s => s.BuyerId == buyerId).ToList();
            }
            
            // Bind to grid
            dgvReport.DataSource = null;
            dgvReport.DataSource = _currentData;
            
            // Configure columns
            if (dgvReport.Columns.Count > 0)
            {
                dgvReport.Columns["SaleId"].Visible = false;
                dgvReport.Columns["VehicleId"].Visible = false;
                dgvReport.Columns["BuyerId"].Visible = false;
                dgvReport.Columns["MaterialId"].Visible = false;
                dgvReport.Columns["CreatedAt"].Visible = false;
                dgvReport.Columns["Quantity"].Visible = false; // Hide old Quantity, show new fields instead
                
                dgvReport.Columns["SaleDate"].HeaderText = "Date";
                dgvReport.Columns["SaleDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                dgvReport.Columns["VehicleNo"].HeaderText = "Vehicle";
                dgvReport.Columns["BuyerName"].HeaderText = "Buyer";
                dgvReport.Columns["MaterialName"].HeaderText = "Material";
                
                // Display new MT/CFT fields
                dgvReport.Columns["InputUnit"].HeaderText = "Unit";
                dgvReport.Columns["InputUnit"].Width = 50;
                dgvReport.Columns["InputQuantity"].HeaderText = "Qty";
                dgvReport.Columns["InputQuantity"].DefaultCellStyle.Format = "N2";
                dgvReport.Columns["InputQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvReport.Columns["CalculatedCFT"].HeaderText = "CFT";
                dgvReport.Columns["CalculatedCFT"].DefaultCellStyle.Format = "N2";
                dgvReport.Columns["CalculatedCFT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
                dgvReport.Columns["Rate"].DefaultCellStyle.Format = "N2";
                dgvReport.Columns["Amount"].DefaultCellStyle.Format = "N2";
                
                dgvReport.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvReport.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Update total
            var total = _currentData.Sum(s => s.Amount);
            lblTotal.Text = $"Total: ₹{total:N2}";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load sales report");
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
                Title = "Export Sales Report",
                FileName = $"SalesReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExcelReportGenerator.GenerateSalesReport(
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
            Logger.LogError(ex, "Failed to export sales report");
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
            EditSelectedSale();
        }
    }
    
    public void BtnEdit_Click(object sender, EventArgs e)
    {
        EditSelectedSale();
    }
    
    private void EditSelectedSale()
    {
        if (dgvReport.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a sale to edit", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var sale = (Sale)dgvReport.SelectedRows[0].DataBoundItem;
        
        // Show edit dialog
        using var editForm = new SaleEditDialog(sale, _vehicles, _buyers, _materials);
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Update the sale
                sale.SaleDate = editForm.SaleDate;
                sale.VehicleId = editForm.VehicleId;
                sale.BuyerId = editForm.BuyerId;
                sale.MaterialId = editForm.MaterialId;
                sale.Quantity = editForm.Quantity;
                sale.Rate = editForm.Rate;
                sale.Amount = editForm.Amount;
                
                SaleRepository.Update(sale);
                
                LoadReport();
                MessageBox.Show("Sale updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update sale");
                MessageBox.Show("Failed to update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    public void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dgvReport.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select sale(s) to delete", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        int count = dgvReport.SelectedRows.Count;
        var result = MessageBox.Show(
            $"Are you sure you want to delete {count} sale(s)?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            int successCount = 0;
            foreach (DataGridViewRow row in dgvReport.SelectedRows)
            {
                var sale = (Sale)row.DataBoundItem;
                SaleRepository.Delete(sale.SaleId);
                successCount++;
            }
            
            LoadReport();
            MessageBox.Show($"{successCount} sale(s) deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete sale(s)");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    // Invoice generation functionality
    private void GenerateInvoiceForSelectedSale()
    {
        if (dgvReport.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a sale transaction.", "No Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var selectedSale = dgvReport.SelectedRows[0].DataBoundItem as Sale;
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
    
    // Add context menu for invoice generation
    private void InitializeInvoiceContextMenu()
    {
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Generate Invoice", null, (s, e) => GenerateInvoiceForSelectedSale());
        contextMenu.Items.Add("-"); // Separator
        contextMenu.Items.Add("Edit", null, (s, e) => BtnEdit_Click(s, e));
        contextMenu.Items.Add("Delete", null, (s, e) => BtnDelete_Click(s, e));
        dgvReport.ContextMenuStrip = contextMenu;
    }
}

/// <summary>
/// Dialog for editing sale details
/// </summary>
internal class SaleEditDialog : Form
{
    private DateTimePicker dtpSaleDate;
    private ComboBox cmbVehicle;
    private ComboBox cmbBuyer;
    private ComboBox cmbMaterial;
    private TextBox txtQuantity;
    private TextBox txtRate;
    private TextBox txtAmount;
    
    public DateTime SaleDate => dtpSaleDate.Value.Date;
    public int VehicleId => (int)cmbVehicle.SelectedValue;
    public int BuyerId => (int)cmbBuyer.SelectedValue;
    public int MaterialId => (int)cmbMaterial.SelectedValue;
    public decimal Quantity => decimal.Parse(txtQuantity.Text);
    public decimal Rate => decimal.Parse(txtRate.Text);
    public decimal Amount => decimal.Parse(txtAmount.Text);
    
    public SaleEditDialog(Sale sale, List<Vehicle> vehicles, List<Buyer> buyers, List<Material> materials)
    {
        InitializeComponent();
        
        // Load master data
        cmbVehicle.DataSource = VehicleRepository.GetAll(activeOnly: false);
        cmbVehicle.DisplayMember = "VehicleNo";
        cmbVehicle.ValueMember = "VehicleId";
        
        cmbBuyer.DataSource = BuyerRepository.GetAll(activeOnly: false);
        cmbBuyer.DisplayMember = "BuyerName";
        cmbBuyer.ValueMember = "BuyerId";
        
        cmbMaterial.DataSource = MaterialRepository.GetAll(activeOnly: false);
        cmbMaterial.DisplayMember = "MaterialName";
        cmbMaterial.ValueMember = "MaterialId";
        
        // Set current values
        dtpSaleDate.Value = sale.SaleDate;
        cmbVehicle.SelectedValue = sale.VehicleId;
        cmbBuyer.SelectedValue = sale.BuyerId;
        cmbMaterial.SelectedValue = sale.MaterialId;
        txtQuantity.Text = sale.Quantity.ToString();
        txtRate.Text = sale.Rate.ToString();
        txtAmount.Text = sale.Amount.ToString("N2");
        
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
        this.Height = 400;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Text = "Edit Sale";
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        var y = 20;
        
        var lblDate = new Label { Left = 20, Top = y, Width = 100, Text = "Sale Date:" };
        dtpSaleDate = new DateTimePicker { Left = 130, Top = y - 3, Width = 330, Format = DateTimePickerFormat.Short };
        
        y += 35;
        var lblVehicle = new Label { Left = 20, Top = y, Width = 100, Text = "Vehicle:" };
        cmbVehicle = new ComboBox { Left = 130, Top = y - 3, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
        
        y += 35;
        var lblBuyer = new Label { Left = 20, Top = y, Width = 100, Text = "Buyer:" };
        cmbBuyer = new ComboBox { Left = 130, Top = y - 3, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
        
        y += 35;
        var lblMaterial = new Label { Left = 20, Top = y, Width = 100, Text = "Material:" };
        cmbMaterial = new ComboBox { Left = 130, Top = y - 3, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
        
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
        this.Controls.Add(dtpSaleDate);
        this.Controls.Add(lblVehicle);
        this.Controls.Add(cmbVehicle);
        this.Controls.Add(lblBuyer);
        this.Controls.Add(cmbBuyer);
        this.Controls.Add(lblMaterial);
        this.Controls.Add(cmbMaterial);
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
        
        if (cmbBuyer.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a buyer", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
