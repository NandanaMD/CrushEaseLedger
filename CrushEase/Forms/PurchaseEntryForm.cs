using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class PurchaseEntryForm : Form
{
    private List<Vehicle> _vehicles;
    private List<Vendor> _vendors;
    private List<Material> _materials;
    private int? _editPurchaseId;
    
    public PurchaseEntryForm()
    {
        InitializeComponent();
        _vehicles = new List<Vehicle>();
        _vendors = new List<Vendor>();
        _materials = new List<Material>();
        _editPurchaseId = null;
    }
    
    public PurchaseEntryForm(int purchaseId) : this()
    {
        _editPurchaseId = purchaseId;
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
            case Keys.Control | Keys.N:
                if (!_editPurchaseId.HasValue)
                {
                    BtnSaveAndNew_Click(this, EventArgs.Empty);
                    return true;
                }
                break;
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
            dtpPurchaseDate,
            cmbVehicle,
            cmbVendor,
            cmbMaterial,
            txtQuantity,
            txtRate,
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
    
    private void PurchaseEntryForm_Load(object sender, EventArgs e)
    {
        dtpPurchaseDate.Value = DateTime.Today;
        LoadMasterData();
        
        if (_editPurchaseId.HasValue)
        {
            this.Text = "Edit Purchase";
            btnSaveAndNew.Visible = false;
            LoadPurchaseForEdit();
        }
        
        // Apply modern theme
        Utils.ModernTheme.ApplyToForm(this);
    }
    
    private void LoadPurchaseForEdit()
    {
        if (!_editPurchaseId.HasValue) return;
        
        var purchase = PurchaseRepository.GetById(_editPurchaseId.Value);
        if (purchase != null)
        {
            dtpPurchaseDate.Value = purchase.PurchaseDate;
            cmbVehicle.SelectedValue = purchase.VehicleId;
            cmbVendor.SelectedValue = purchase.VendorId;
            cmbMaterial.SelectedValue = purchase.MaterialId;
            txtQuantity.Text = purchase.Quantity.ToString("N2");
            txtRate.Text = purchase.Rate.ToString("N2");
            txtAmount.Text = purchase.Amount.ToString("N2");
            txtVendorSite.Text = purchase.VendorSite ?? "";
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
            
            // Load vendors
            _vendors = VendorRepository.GetAll(activeOnly: true);
            cmbVendor.DataSource = _vendors;
            cmbVendor.DisplayMember = "VendorName";
            cmbVendor.ValueMember = "VendorId";
            cmbVendor.SelectedIndex = -1;
            
            // Load materials
            _materials = MaterialRepository.GetAll(activeOnly: true);
            cmbMaterial.DataSource = _materials;
            cmbMaterial.DisplayMember = "MaterialName";
            cmbMaterial.ValueMember = "MaterialId";
            cmbMaterial.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load master data");
            MessageBox.Show("Failed to load data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void TxtQuantity_TextChanged(object sender, EventArgs e)
    {
        CalculateAmount();
    }
    
    private void TxtRate_TextChanged(object sender, EventArgs e)
    {
        CalculateAmount();
    }
    
    private void CalculateAmount()
    {
        if (decimal.TryParse(txtQuantity.Text, out var quantity) && 
            decimal.TryParse(txtRate.Text, out var rate))
        {
            txtAmount.Text = (quantity * rate).ToString("N2");
        }
        else
        {
            txtAmount.Text = "0.00";
        }
    }
    
    private void BtnAddVendor_Click(object sender, EventArgs e)
    {
        using var vendorForm = new VendorMasterForm();
        vendorForm.ShowDialog();
        
        // Reload vendors
        var selectedVendorId = cmbVendor.SelectedValue;
        _vendors = VendorRepository.GetAll(activeOnly: true);
        cmbVendor.DataSource = _vendors;
        
        // Try to restore selection
        if (selectedVendorId != null)
        {
            cmbVendor.SelectedValue = selectedVendorId;
        }
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;
        
        try
        {
            var purchase = new Purchase
            {
                PurchaseDate = dtpPurchaseDate.Value.Date,
                VehicleId = (int)cmbVehicle.SelectedValue,
                VendorId = (int)cmbVendor.SelectedValue,
                MaterialId = (int)cmbMaterial.SelectedValue,
                Quantity = decimal.Parse(txtQuantity.Text),
                Rate = decimal.Parse(txtRate.Text),
                Amount = decimal.Parse(txtAmount.Text),
                VendorSite = string.IsNullOrWhiteSpace(txtVendorSite.Text) ? null : txtVendorSite.Text.Trim()
            };
            
            if (_editPurchaseId.HasValue)
            {
                purchase.PurchaseId = _editPurchaseId.Value;
                PurchaseRepository.Update(purchase);
                MessageBox.Show("Purchase updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                PurchaseRepository.Insert(purchase);
                MessageBox.Show("Purchase saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            Close();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save purchase");
            MessageBox.Show("Failed to save purchase: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnSaveAndNew_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;
        
        try
        {
            var purchase = new Purchase
            {
                PurchaseDate = dtpPurchaseDate.Value.Date,
                VehicleId = (int)cmbVehicle.SelectedValue,
                VendorId = (int)cmbVendor.SelectedValue,
                MaterialId = (int)cmbMaterial.SelectedValue,
                Quantity = decimal.Parse(txtQuantity.Text),
                Rate = decimal.Parse(txtRate.Text),
                Amount = decimal.Parse(txtAmount.Text),
                VendorSite = string.IsNullOrWhiteSpace(txtVendorSite.Text) ? null : txtVendorSite.Text.Trim()
            };
            
            PurchaseRepository.Insert(purchase);
            
            MessageBox.Show("Purchase saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Clear form for next entry
            ClearForm();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save purchase");
            MessageBox.Show("Failed to save purchase: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        
        if (cmbVendor.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a vendor", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbVendor.Focus();
            return false;
        }
        
        if (cmbMaterial.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a material", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbMaterial.Focus();
            return false;
        }
        
        if (!decimal.TryParse(txtQuantity.Text, out var quantity) || quantity <= 0)
        {
            MessageBox.Show("Please enter a valid quantity (greater than 0)", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtQuantity.Focus();
            return false;
        }
        
        if (!decimal.TryParse(txtRate.Text, out var rate) || rate <= 0)
        {
            MessageBox.Show("Please enter a valid rate (greater than 0)", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtRate.Focus();
            return false;
        }
        
        return true;
    }
    
    private void ClearForm()
    {
        dtpPurchaseDate.Value = DateTime.Today;
        cmbVehicle.SelectedIndex = -1;
        cmbVendor.SelectedIndex = -1;
        cmbMaterial.SelectedIndex = -1;
        txtVendorSite.Text = "";
        txtQuantity.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "0.00";
        cmbVehicle.Focus();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}
