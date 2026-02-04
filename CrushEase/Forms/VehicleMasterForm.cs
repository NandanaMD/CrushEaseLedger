using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class VehicleMasterForm : Form
{
    private List<Vehicle> _vehicles = new();
    
    public VehicleMasterForm()
    {
        InitializeComponent();
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // Handle keyboard shortcuts
        switch (keyData)
        {
            case Keys.Control | Keys.S:
                BtnSave_Click(this, EventArgs.Empty);
                return true;
            case Keys.Delete:
                BtnDelete_Click(this, EventArgs.Empty);
                return true;
            case Keys.Escape:
                BtnClose_Click(this, EventArgs.Empty);
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
        Control? currentControl = this.ActiveControl;
        
        // For Vehicle, only one input field, so arrow keys don't navigate
        // Just return false to allow default behavior
        return false;
    }
    
    private void VehicleMasterForm_Load(object sender, EventArgs e)
    {
        LoadVehicles();
        Utils.ModernTheme.ApplyToForm(this);
    }
    
    private void LoadVehicles()
    {
        try
        {
            _vehicles = VehicleRepository.GetAll(chkShowInactive.Checked == false);
            dgvVehicles.DataSource = _vehicles;
            
            // Hide ID column
            if (dgvVehicles.Columns["VehicleId"] != null)
                dgvVehicles.Columns["VehicleId"].Visible = false;
            
            // Format column headers
            if (dgvVehicles.Columns["VehicleNo"] != null)
                dgvVehicles.Columns["VehicleNo"].HeaderText = "Vehicle Number";
            
            if (dgvVehicles.Columns["IsActive"] != null)
                dgvVehicles.Columns["IsActive"].HeaderText = "Active";
            
            if (dgvVehicles.Columns["CreatedAt"] != null)
            {
                dgvVehicles.Columns["CreatedAt"].HeaderText = "Created Date";
                dgvVehicles.Columns["CreatedAt"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            }
            
            lblStatus.Text = $"{_vehicles.Count} vehicle(s)";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load vehicles");
            MessageBox.Show("Failed to load vehicles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnAdd_Click(object sender, EventArgs e)
    {
        txtVehicleNo.Text = "";
        txtVehicleNo.Focus();
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtVehicleNo.Text))
        {
            MessageBox.Show("Vehicle number is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtVehicleNo.Focus();
            return;
        }
        
        try
        {
            // Check for duplicates
            if (VehicleRepository.Exists(txtVehicleNo.Text.Trim()))
            {
                MessageBox.Show("Vehicle number already exists", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtVehicleNo.Focus();
                return;
            }
            
            var vehicle = new Vehicle
            {
                VehicleNo = txtVehicleNo.Text.Trim().ToUpper(),
                IsActive = true
            };
            
            VehicleRepository.Insert(vehicle);
            
            txtVehicleNo.Text = "";
            LoadVehicles();
            
            MessageBox.Show("Vehicle added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save vehicle");
            MessageBox.Show("Failed to save: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dgvVehicles.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a vehicle to edit", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var vehicle = (Vehicle)dgvVehicles.SelectedRows[0].DataBoundItem;
        
        var newNumber = Prompt.ShowDialog($"Edit Vehicle Number:", "Edit Vehicle", vehicle.VehicleNo);
        if (string.IsNullOrWhiteSpace(newNumber))
            return;
        
        try
        {
            // Check for duplicates (excluding current)
            if (VehicleRepository.Exists(newNumber.Trim(), vehicle.VehicleId))
            {
                MessageBox.Show("Vehicle number already exists", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            vehicle.VehicleNo = newNumber.Trim().ToUpper();
            VehicleRepository.Update(vehicle);
            
            LoadVehicles();
            MessageBox.Show("Vehicle updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update vehicle");
            MessageBox.Show("Failed to update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dgvVehicles.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a vehicle to delete", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var vehicle = (Vehicle)dgvVehicles.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete vehicle '{vehicle.VehicleNo}'?\n\nNote: This will soft-delete (deactivate) the vehicle.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            VehicleRepository.Delete(vehicle.VehicleId);
            LoadVehicles();
            MessageBox.Show("Vehicle deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete vehicle");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void TxtSearch_TextChanged(object sender, EventArgs e)
    {
        var searchText = txtSearch.Text.ToLower();
        
        if (string.IsNullOrWhiteSpace(searchText))
        {
            dgvVehicles.DataSource = _vehicles;
        }
        else
        {
            var filtered = _vehicles.Where(v => v.VehicleNo.ToLower().Contains(searchText)).ToList();
            dgvVehicles.DataSource = filtered;
        }
        
        lblStatus.Text = $"{dgvVehicles.Rows.Count} vehicle(s)";
    }
    
    private void ChkShowInactive_CheckedChanged(object sender, EventArgs e)
    {
        LoadVehicles();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}

/// <summary>
/// Simple prompt dialog helper
/// </summary>
public static class Prompt
{
    public static string ShowDialog(string text, string caption, string defaultValue = "")
    {
        Form prompt = new Form()
        {
            Width = 400,
            Height = 150,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = caption,
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = false
        };
        
        Label textLabel = new Label() { Left = 20, Top = 20, Width = 350, Text = text };
        TextBox textBox = new TextBox() { Left = 20, Top = 45, Width = 350, Text = defaultValue };
        Button confirmation = new Button() { Text = "OK", Left = 220, Width = 70, Top = 75, DialogResult = DialogResult.OK };
        Button cancel = new Button() { Text = "Cancel", Left = 300, Width = 70, Top = 75, DialogResult = DialogResult.Cancel };
        
        confirmation.Click += (sender, e) => { prompt.Close(); };
        cancel.Click += (sender, e) => { prompt.Close(); };
        
        prompt.Controls.Add(textLabel);
        prompt.Controls.Add(textBox);
        prompt.Controls.Add(confirmation);
        prompt.Controls.Add(cancel);
        prompt.AcceptButton = confirmation;
        prompt.CancelButton = cancel;
        
        textBox.SelectAll();
        textBox.Focus();
        
        return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
    }
}
