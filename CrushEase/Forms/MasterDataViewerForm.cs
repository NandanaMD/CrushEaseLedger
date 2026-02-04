using CrushEase.Data;
using CrushEase.Models;

namespace CrushEase.Forms;

/// <summary>
/// Form for viewing all master data with filtering capabilities
/// </summary>
public partial class MasterDataViewerForm : Form
{
    public MasterDataViewerForm()
    {
        InitializeComponent();
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // Handle Delete key shortcut
        if (keyData == Keys.Delete)
        {
            // Determine which tab is active and call appropriate delete handler
            if (tabControl.SelectedTab == tabVehicles)
                BtnDeleteVehicle_Click(this, EventArgs.Empty);
            else if (tabControl.SelectedTab == tabVendors)
                BtnDeleteVendor_Click(this, EventArgs.Empty);
            else if (tabControl.SelectedTab == tabBuyers)
                BtnDeleteBuyer_Click(this, EventArgs.Empty);
            else if (tabControl.SelectedTab == tabMaterials)
                BtnDeleteMaterial_Click(this, EventArgs.Empty);
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
    
    private void MasterDataViewerForm_Load(object sender, EventArgs e)
    {
        LoadAllData();
        Utils.ModernTheme.ApplyToForm(this);
    }
    
    private void LoadAllData()
    {
        LoadVehicles();
        LoadVendors();
        LoadBuyers();
        LoadMaterials();
    }
    
    private void LoadVehicles()
    {
        var vehicles = VehicleRepository.GetAll(chkVehiclesActiveOnly.Checked);
        
        // Filter by search text
        if (!string.IsNullOrWhiteSpace(txtVehicleSearch.Text))
        {
            vehicles = vehicles.Where(v => v.VehicleNo.Contains(txtVehicleSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        dgvVehicles.DataSource = vehicles;
        FormatVehiclesGrid();
        lblVehicleCount.Text = $"Total: {vehicles.Count}";
    }
    
    private void LoadVendors()
    {
        var vendors = VendorRepository.GetAll(chkVendorsActiveOnly.Checked);
        
        // Filter by search text
        if (!string.IsNullOrWhiteSpace(txtVendorSearch.Text))
        {
            vendors = vendors.Where(v => 
                v.VendorName.Contains(txtVendorSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                (v.Contact != null && v.Contact.Contains(txtVendorSearch.Text, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
        
        dgvVendors.DataSource = vendors;
        FormatVendorsGrid();
        lblVendorCount.Text = $"Total: {vendors.Count}";
    }
    
    private void LoadBuyers()
    {
        var buyers = BuyerRepository.GetAll(chkBuyersActiveOnly.Checked);
        
        // Filter by search text
        if (!string.IsNullOrWhiteSpace(txtBuyerSearch.Text))
        {
            buyers = buyers.Where(b => 
                b.BuyerName.Contains(txtBuyerSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                (b.Contact != null && b.Contact.Contains(txtBuyerSearch.Text, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
        
        dgvBuyers.DataSource = buyers;
        FormatBuyersGrid();
        lblBuyerCount.Text = $"Total: {buyers.Count}";
    }
    
    private void LoadMaterials()
    {
        var materials = MaterialRepository.GetAll(chkMaterialsActiveOnly.Checked);
        
        // Filter by search text
        if (!string.IsNullOrWhiteSpace(txtMaterialSearch.Text))
        {
            materials = materials.Where(m => 
                m.MaterialName.Contains(txtMaterialSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                m.Unit.Contains(txtMaterialSearch.Text, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
        
        dgvMaterials.DataSource = materials;
        FormatMaterialsGrid();
        lblMaterialCount.Text = $"Total: {materials.Count}";
    }
    
    private void FormatVehiclesGrid()
    {
        if (dgvVehicles.Columns.Count == 0) return;
        
        dgvVehicles.Columns["VehicleId"].Visible = false;
        dgvVehicles.Columns["VehicleNo"].HeaderText = "Vehicle Number";
        dgvVehicles.Columns["IsActive"].HeaderText = "Active";
        dgvVehicles.Columns["CreatedAt"].HeaderText = "Created Date";
        dgvVehicles.Columns["CreatedAt"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm";
        
        dgvVehicles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }
    
    private void FormatVendorsGrid()
    {
        if (dgvVendors.Columns.Count == 0) return;
        
        dgvVendors.Columns["VendorId"].Visible = false;
        dgvVendors.Columns["VendorName"].HeaderText = "Vendor Name";
        dgvVendors.Columns["Contact"].HeaderText = "Contact";
        dgvVendors.Columns["Notes"].HeaderText = "Notes";
        dgvVendors.Columns["IsActive"].HeaderText = "Active";
        dgvVendors.Columns["CreatedAt"].HeaderText = "Created Date";
        dgvVendors.Columns["CreatedAt"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm";
        
        dgvVendors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }
    
    private void FormatBuyersGrid()
    {
        if (dgvBuyers.Columns.Count == 0) return;
        
        dgvBuyers.Columns["BuyerId"].Visible = false;
        dgvBuyers.Columns["BuyerName"].HeaderText = "Buyer Name";
        dgvBuyers.Columns["Contact"].HeaderText = "Contact";
        dgvBuyers.Columns["Notes"].HeaderText = "Notes";
        dgvBuyers.Columns["IsActive"].HeaderText = "Active";
        dgvBuyers.Columns["CreatedAt"].HeaderText = "Created Date";
        dgvBuyers.Columns["CreatedAt"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm";
        
        dgvBuyers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }
    
    private void FormatMaterialsGrid()
    {
        if (dgvMaterials.Columns.Count == 0) return;
        
        dgvMaterials.Columns["MaterialId"].Visible = false;
        dgvMaterials.Columns["MaterialName"].HeaderText = "Material Name";
        dgvMaterials.Columns["Unit"].HeaderText = "Unit";
        dgvMaterials.Columns["Notes"].HeaderText = "Notes";
        dgvMaterials.Columns["IsActive"].HeaderText = "Active";
        dgvMaterials.Columns["CreatedAt"].HeaderText = "Created Date";
        dgvMaterials.Columns["CreatedAt"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm";
        
        dgvMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }
    
    // Filter event handlers
    private void TxtVehicleSearch_TextChanged(object sender, EventArgs e)
    {
        LoadVehicles();
    }
    
    private void ChkVehiclesActiveOnly_CheckedChanged(object sender, EventArgs e)
    {
        LoadVehicles();
    }
    
    private void TxtVendorSearch_TextChanged(object sender, EventArgs e)
    {
        LoadVendors();
    }
    
    private void ChkVendorsActiveOnly_CheckedChanged(object sender, EventArgs e)
    {
        LoadVendors();
    }
    
    private void TxtBuyerSearch_TextChanged(object sender, EventArgs e)
    {
        LoadBuyers();
    }
    
    private void ChkBuyersActiveOnly_CheckedChanged(object sender, EventArgs e)
    {
        LoadBuyers();
    }
    
    private void TxtMaterialSearch_TextChanged(object sender, EventArgs e)
    {
        LoadMaterials();
    }
    
    private void ChkMaterialsActiveOnly_CheckedChanged(object sender, EventArgs e)
    {
        LoadMaterials();
    }
    
    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        LoadAllData();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }
    
    // Delete handlers
    private void BtnDeleteVehicle_Click(object sender, EventArgs e)
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
            Utils.Logger.LogError(ex, "Failed to delete vehicle");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnDeleteVendor_Click(object sender, EventArgs e)
    {
        if (dgvVendors.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a vendor to delete", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var vendor = (Vendor)dgvVendors.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete vendor '{vendor.VendorName}'?\n\nNote: This will soft-delete (deactivate) the vendor.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            VendorRepository.Delete(vendor.VendorId);
            LoadVendors();
            MessageBox.Show("Vendor deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Utils.Logger.LogError(ex, "Failed to delete vendor");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnDeleteBuyer_Click(object sender, EventArgs e)
    {
        if (dgvBuyers.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a buyer to delete", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var buyer = (Buyer)dgvBuyers.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete buyer '{buyer.BuyerName}'?\n\nNote: This will soft-delete (deactivate) the buyer.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            BuyerRepository.Delete(buyer.BuyerId);
            LoadBuyers();
            MessageBox.Show("Buyer deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Utils.Logger.LogError(ex, "Failed to delete buyer");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnDeleteMaterial_Click(object sender, EventArgs e)
    {
        if (dgvMaterials.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a material to delete", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var material = (Material)dgvMaterials.SelectedRows[0].DataBoundItem;
        
        var result = MessageBox.Show(
            $"Are you sure you want to delete material '{material.MaterialName}'?\n\nNote: This will soft-delete (deactivate) the material.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            MaterialRepository.Delete(material.MaterialId);
            LoadMaterials();
            MessageBox.Show("Material deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Utils.Logger.LogError(ex, "Failed to delete material");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
