using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class MaintenanceReportForm : Form
{
    private List<Vehicle> _vehicles;
    private List<Maintenance> _currentData;
    
    public MaintenanceReportForm()
    {
        InitializeComponent();
        _vehicles = new List<Vehicle>();
        _currentData = new List<Maintenance>();
    }
    
    private void MaintenanceReportForm_Load(object sender, EventArgs e)
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
            
            // Get all maintenance
            _currentData = MaintenanceRepository.GetAll(fromDate, toDate, vehicleId);
            
            // Bind to grid
            dgvReport.DataSource = null;
            dgvReport.DataSource = _currentData;
            
            // Configure columns
            if (dgvReport.Columns.Count > 0)
            {
                dgvReport.Columns["MaintenanceId"].Visible = false;
                dgvReport.Columns["VehicleId"].Visible = false;
                dgvReport.Columns["CreatedAt"].Visible = false;
                
                dgvReport.Columns["MaintenanceDate"].HeaderText = "Date";
                dgvReport.Columns["MaintenanceDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                dgvReport.Columns["VehicleNo"].HeaderText = "Vehicle";
                dgvReport.Columns["Description"].HeaderText = "Description";
                dgvReport.Columns["Amount"].DefaultCellStyle.Format = "N2";
                
                dgvReport.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Update total
            var total = _currentData.Sum(m => m.Amount);
            lblTotal.Text = $"Total: ₹{total:N2}";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load maintenance report");
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
                Title = "Export Maintenance Report",
                FileName = $"MaintenanceReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExcelReportGenerator.GenerateMaintenanceReport(
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
            Logger.LogError(ex, "Failed to export maintenance report");
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
            EditSelectedMaintenance();
        }
    }
    
    public void BtnEdit_Click(object sender, EventArgs e)
    {
        EditSelectedMaintenance();
    }
    
    private void EditSelectedMaintenance()
    {
        if (dgvReport.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a maintenance record to edit", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var maintenance = (Maintenance)dgvReport.SelectedRows[0].DataBoundItem;
        
        // Show edit dialog
        using var editForm = new MaintenanceEditDialog(maintenance, _vehicles);
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Update the maintenance
                maintenance.MaintenanceDate = editForm.MaintenanceDate;
                maintenance.VehicleId = editForm.VehicleId;
                maintenance.Description = editForm.Description;
                maintenance.Amount = editForm.Amount;
                
                MaintenanceRepository.Update(maintenance);
                
                LoadReport();
                MessageBox.Show("Maintenance updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update maintenance");
                MessageBox.Show("Failed to update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    public void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dgvReport.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a maintenance record to delete", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var maintenance = (Maintenance)dgvReport.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete this maintenance?\\n\\nDate: {maintenance.MaintenanceDate:dd-MMM-yyyy}\\nVehicle: {maintenance.VehicleNo}\\nDescription: {maintenance.Description}\\nAmount: ₹{maintenance.Amount:N2}",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            MaintenanceRepository.Delete(maintenance.MaintenanceId);
            LoadReport();
            MessageBox.Show("Maintenance deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete maintenance");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

/// <summary>
/// Dialog for editing maintenance details
/// </summary>
internal class MaintenanceEditDialog : Form
{
    private DateTimePicker dtpMaintenanceDate;
    private ComboBox cmbVehicle;
    private TextBox txtDescription;
    private TextBox txtAmount;
    
    public DateTime MaintenanceDate => dtpMaintenanceDate.Value.Date;
    public int VehicleId => (int)cmbVehicle.SelectedValue;
    public string Description => txtDescription.Text.Trim();
    public decimal Amount => decimal.Parse(txtAmount.Text);
    
    public MaintenanceEditDialog(Maintenance maintenance, List<Vehicle> vehicles)
    {
        InitializeComponent();
        
        // Load master data
        cmbVehicle.DataSource = VehicleRepository.GetAll(activeOnly: false);
        cmbVehicle.DisplayMember = "VehicleNo";
        cmbVehicle.ValueMember = "VehicleId";
        
        // Set current values
        dtpMaintenanceDate.Value = maintenance.MaintenanceDate;
        cmbVehicle.SelectedValue = maintenance.VehicleId;
        txtDescription.Text = maintenance.Description;
        txtAmount.Text = maintenance.Amount.ToString();
    }
    
    private void InitializeComponent()
    {
        this.Width = 500;
        this.Height = 320;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Text = "Edit Maintenance";
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        var y = 20;
        
        var lblDate = new Label { Left = 20, Top = y, Width = 120, Text = "Maintenance Date:" };
        dtpMaintenanceDate = new DateTimePicker { Left = 150, Top = y - 3, Width = 310, Format = DateTimePickerFormat.Short };
        
        y += 35;
        var lblVehicle = new Label { Left = 20, Top = y, Width = 120, Text = "Vehicle:" };
        cmbVehicle = new ComboBox { Left = 150, Top = y - 3, Width = 310, DropDownStyle = ComboBoxStyle.DropDownList };
        
        y += 35;
        var lblDescription = new Label { Left = 20, Top = y, Width = 120, Text = "Description:" };
        txtDescription = new TextBox { Left = 150, Top = y - 3, Width = 310, Height = 60, Multiline = true };
        
        y += 70;
        var lblAmount = new Label { Left = 20, Top = y, Width = 120, Text = "Amount:" };
        txtAmount = new TextBox { Left = 150, Top = y - 3, Width = 150 };
        
        y += 50;
        var btnOK = new Button { Text = "Save", Left = 260, Width = 90, Top = y, DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Cancel", Left = 360, Width = 90, Top = y, DialogResult = DialogResult.Cancel };
        
        btnOK.Click += (s, e) => { 
            if (!ValidateForm()) { 
                ((Form)s).DialogResult = DialogResult.None;
            } 
        };
        
        this.Controls.Add(lblDate);
        this.Controls.Add(dtpMaintenanceDate);
        this.Controls.Add(lblVehicle);
        this.Controls.Add(cmbVehicle);
        this.Controls.Add(lblDescription);
        this.Controls.Add(txtDescription);
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
        
        if (string.IsNullOrWhiteSpace(txtDescription.Text))
        {
            MessageBox.Show("Please enter a description", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        if (!decimal.TryParse(txtAmount.Text, out var amount) || amount <= 0)
        {
            MessageBox.Show("Please enter a valid amount (greater than 0)", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        
        return true;
    }
}
