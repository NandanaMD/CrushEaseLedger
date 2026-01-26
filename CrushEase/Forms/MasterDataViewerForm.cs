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
}
