using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class MaintenanceEntryForm : Form
{
    private List<Vehicle> _vehicles;
    private int? _editMaintenanceId;
    
    public MaintenanceEntryForm()
    {
        InitializeComponent();
        _vehicles = new List<Vehicle>();
        _editMaintenanceId = null;
    }
    
    public MaintenanceEntryForm(int maintenanceId) : this()
    {
        _editMaintenanceId = maintenanceId;
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // Handle keyboard shortcuts
        switch (keyData)
        {
            case Keys.Control | Keys.S:
                BtnSave_Click(this, EventArgs.Empty);
                return true;
            case Keys.Escape:
                BtnClose_Click(this, EventArgs.Empty);
                return true;
            case Keys.F5:
                ClearForm();
                return true;
            case Keys.Up:
                return HandleArrowKeyNavigation(true);
            case Keys.Down:
                return HandleArrowKeyNavigation(false);
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
    
    private bool HandleArrowKeyNavigation(bool moveUp)
    {
        // Get the currently focused control
        Control? currentControl = this.ActiveControl;
        
        // If inside a ComboBox dropdown, don't interfere
        if (currentControl is ComboBox cmb && cmb.DroppedDown)
            return false;
        
        // Define the navigation order for main input controls
        List<Control> navOrder = new List<Control>
        {
            dtpMaintenanceDate,
            cmbVehicle,
            txtDescription,
            txtAmount
        };
        
        int currentIndex = navOrder.IndexOf(currentControl);
        
        if (currentIndex >= 0)
        {
            int nextIndex = moveUp ? currentIndex - 1 : currentIndex + 1;
            
            if (nextIndex >= 0 && nextIndex < navOrder.Count)
            {
                navOrder[nextIndex].Focus();
                if (navOrder[nextIndex] is TextBox txt)
                    txt.SelectAll();
                return true;
            }
        }
        
        return false;
    }
    
    private void MaintenanceEntryForm_Load(object sender, EventArgs e)
    {
        dtpMaintenanceDate.Value = DateTime.Today;
        LoadMasterData();
        
        if (_editMaintenanceId.HasValue)
        {
            this.Text = "Edit Maintenance";
            LoadMaintenanceForEdit();
        }
        
        // Apply modern theme
        Utils.ModernTheme.ApplyToForm(this);
    }
    
    private void LoadMaintenanceForEdit()
    {
        if (!_editMaintenanceId.HasValue) return;
        
        var maintenance = MaintenanceRepository.GetById(_editMaintenanceId.Value);
        if (maintenance != null)
        {
            dtpMaintenanceDate.Value = maintenance.MaintenanceDate;
            cmbVehicle.SelectedValue = maintenance.VehicleId;
            txtDescription.Text = maintenance.Description;
            txtAmount.Text = maintenance.Amount.ToString("N2");
        }
    }
    
    private void LoadMasterData()
    {
        try
        {
            // Load vehicles
            _vehicles = VehicleRepository.GetAll(activeOnly: true);
            cmbVehicle.DataSource = _vehicles;
            cmbVehicle.DisplayMember = "VehicleNo";
            cmbVehicle.ValueMember = "VehicleId";
            cmbVehicle.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load master data");
            MessageBox.Show("Failed to load data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;
        
        try
        {
            var maintenance = new Maintenance
            {
                MaintenanceDate = dtpMaintenanceDate.Value.Date,
                VehicleId = (int)cmbVehicle.SelectedValue,
                Description = txtDescription.Text.Trim(),
                Amount = decimal.Parse(txtAmount.Text)
            };
            
            if (_editMaintenanceId.HasValue)
            {
                maintenance.MaintenanceId = _editMaintenanceId.Value;
                MaintenanceRepository.Update(maintenance);
                MessageBox.Show("Maintenance updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MaintenanceRepository.Insert(maintenance);
                MessageBox.Show("Maintenance saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            Close();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save maintenance");
            MessageBox.Show("Failed to save maintenance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private bool ValidateForm()
    {
        if (cmbVehicle.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a vehicle", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbVehicle.Focus();
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(txtDescription.Text))
        {
            MessageBox.Show("Please enter a description", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtDescription.Focus();
            return false;
        }
        
        if (!decimal.TryParse(txtAmount.Text, out var amount) || amount <= 0)
        {
            MessageBox.Show("Please enter a valid amount (greater than 0)", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtAmount.Focus();
            return false;
        }
        
        return true;
    }
    
    private void ClearForm()
    {
        dtpMaintenanceDate.Value = DateTime.Today;
        cmbVehicle.SelectedIndex = -1;
        txtDescription.Text = "";
        txtAmount.Text = "";
        cmbVehicle.Focus();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}
