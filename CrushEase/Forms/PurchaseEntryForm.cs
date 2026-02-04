using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class PurchaseEntryForm : Form
{
    private List<Vehicle> _vehicles;
    private List<Vendor> _vendors;
    private List<Material> _materials;
    private int? _editPurchaseId;
    private Material? _selectedMaterial;
    private int? _lastSavedPurchaseId;
    
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
            cmbUnit,
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
        
        // Initialize Unit ComboBox
        cmbUnit.Items.Clear();
        cmbUnit.Items.Add("CFT");
        cmbUnit.Items.Add("MT");
        cmbUnit.SelectedIndex = 0; // Default to CFT
        
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
            
            // Load unit and quantity based on InputUnit
            cmbUnit.SelectedItem = purchase.InputUnit;
            txtQuantity.Text = purchase.InputQuantity.ToString("N2");
            
            if (purchase.InputUnit == "MT")
            {
                txtCalculatedCFT.Text = purchase.CalculatedCFT.ToString("N2");
                txtCalculatedCFT.Visible = true;
                lblCalculatedCFT.Visible = true;
            }
            
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
            
            // Hide calculated CFT by default
            lblCalculatedCFT.Visible = false;
            txtCalculatedCFT.Visible = false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load master data");
            MessageBox.Show("Failed to load data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void CmbMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbMaterial.SelectedIndex >= 0)
        {
            _selectedMaterial = (Material)cmbMaterial.SelectedItem;
            UpdateCalculatedCFT();
        }
    }
    
    private void CmbUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool isMT = cmbUnit.SelectedItem?.ToString() == "MT";
        lblCalculatedCFT.Visible = isMT;
        txtCalculatedCFT.Visible = isMT;
        
        if (isMT)
        {
            lblQuantity.Text = "Quantity (MT):";
        }
        else
        {
            lblQuantity.Text = "Quantity (CFT):";
        }
        
        UpdateCalculatedCFT();
        CalculateAmount();
    }
    
    private void TxtQuantity_TextChanged(object sender, EventArgs e)
    {
        UpdateCalculatedCFT();
        CalculateAmount();
    }
    
    private void UpdateCalculatedCFT()
    {
        if (cmbUnit.SelectedItem?.ToString() == "MT" && 
            _selectedMaterial != null &&
            decimal.TryParse(txtQuantity.Text, out var quantityMT))
        {
            // ConversionFactor represents MT per CFT (density), so divide to get CFT
            var calculatedCFT = quantityMT / _selectedMaterial.ConversionFactor_MT_to_CFT;
            txtCalculatedCFT.Text = calculatedCFT.ToString("N2");
        }
        else if (cmbUnit.SelectedItem?.ToString() == "MT")
        {
            txtCalculatedCFT.Text = "0.00";
        }
    }
    
    private void TxtRate_TextChanged(object sender, EventArgs e)
    {
        CalculateAmount();
    }
    
    private void CalculateAmount()
    {
        // Determine the effective CFT quantity for amount calculation
        decimal effectiveCFT = 0;
        
        if (cmbUnit.SelectedItem?.ToString() == "MT")
        {
            // For MT, use calculated CFT
            if (decimal.TryParse(txtCalculatedCFT.Text, out var cft))
            {
                effectiveCFT = cft;
            }
        }
        else
        {
            // For CFT, use entered quantity directly
            if (decimal.TryParse(txtQuantity.Text, out var quantity))
            {
                effectiveCFT = quantity;
            }
        }
        
        if (effectiveCFT > 0 && decimal.TryParse(txtRate.Text, out var rate))
        {
            txtAmount.Text = (effectiveCFT * rate).ToString("N2");
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
            var inputUnit = cmbUnit.SelectedItem?.ToString() ?? "CFT";
            var inputQuantity = decimal.Parse(txtQuantity.Text);
            decimal calculatedCFT;
            
            if (inputUnit == "MT")
            {
                calculatedCFT = decimal.Parse(txtCalculatedCFT.Text);
            }
            else
            {
                calculatedCFT = inputQuantity;
            }
            
            var purchase = new Purchase
            {
                PurchaseDate = dtpPurchaseDate.Value.Date,
                VehicleId = (int)cmbVehicle.SelectedValue,
                VendorId = (int)cmbVendor.SelectedValue,
                MaterialId = (int)cmbMaterial.SelectedValue,
                Quantity = calculatedCFT, // Store CFT in Quantity for backward compatibility
                Rate = decimal.Parse(txtRate.Text),
                Amount = decimal.Parse(txtAmount.Text),
                VendorSite = string.IsNullOrWhiteSpace(txtVendorSite.Text) ? null : txtVendorSite.Text.Trim(),
                InputUnit = inputUnit,
                InputQuantity = inputQuantity,
                CalculatedCFT = calculatedCFT
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
            var inputUnit = cmbUnit.SelectedItem?.ToString() ?? "CFT";
            var inputQuantity = decimal.Parse(txtQuantity.Text);
            decimal calculatedCFT;
            
            if (inputUnit == "MT")
            {
                calculatedCFT = decimal.Parse(txtCalculatedCFT.Text);
            }
            else
            {
                calculatedCFT = inputQuantity;
            }
            
            var purchase = new Purchase
            {
                PurchaseDate = dtpPurchaseDate.Value.Date,
                VehicleId = (int)cmbVehicle.SelectedValue,
                VendorId = (int)cmbVendor.SelectedValue,
                MaterialId = (int)cmbMaterial.SelectedValue,
                Quantity = calculatedCFT, // Store CFT in Quantity for backward compatibility
                Rate = decimal.Parse(txtRate.Text),
                Amount = decimal.Parse(txtAmount.Text),
                VendorSite = string.IsNullOrWhiteSpace(txtVendorSite.Text) ? null : txtVendorSite.Text.Trim(),
                InputUnit = inputUnit,
                InputQuantity = inputQuantity,
                CalculatedCFT = calculatedCFT
            };
            
            PurchaseRepository.Insert(purchase);
            
            // Store the last saved purchase ID for receipt printing
            _lastSavedPurchaseId = purchase.PurchaseId;
            btnPrintReceipt.Enabled = true;
            
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
        cmbUnit.SelectedIndex = 0; // Reset to CFT
        txtVendorSite.Text = "";
        txtQuantity.Text = "";
        txtCalculatedCFT.Text = "0.00";
        txtRate.Text = "";
        txtAmount.Text = "0.00";
        lblCalculatedCFT.Visible = false;
        txtCalculatedCFT.Visible = false;
        _lastSavedPurchaseId = null;
        btnPrintReceipt.Enabled = false;
        cmbVehicle.Focus();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
    
    private void ContextMenuEntry_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Enable/disable the context menu item based on whether we have a saved purchase
        menuItemPrintReceipt.Enabled = _lastSavedPurchaseId.HasValue;
    }
    
    private void BtnPrintReceipt_Click(object sender, EventArgs e)
    {
        if (!_lastSavedPurchaseId.HasValue)
        {
            MessageBox.Show("No purchase to print receipt for.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        try
        {
            var purchase = PurchaseRepository.GetById(_lastSavedPurchaseId.Value);
            if (purchase == null)
            {                MessageBox.Show("Purchase not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var result = InvoiceGenerator.GeneratePurchaseReceipt(purchase);
            
            if (result.Success)
            {
                var dialogResult = MessageBox.Show(
                    $"Receipt generated successfully!\n\nReceipt No: {result.InvoiceNumber}\nLocation: {result.FilePath}\n\nWould you like to open the receipt?",
                    "Success",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );
                
                if (dialogResult == DialogResult.Yes && !string.IsNullOrEmpty(result.FilePath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = result.FilePath,
                        UseShellExecute = true
                    });
                }
            }
            else
            {
                MessageBox.Show($"Failed to generate receipt:\n{result.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to print receipt");
            MessageBox.Show($"Failed to print receipt: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
