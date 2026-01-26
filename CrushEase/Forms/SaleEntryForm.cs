using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class SaleEntryForm : Form
{
    private List<Vehicle> _vehicles;
    private List<Buyer> _buyers;
    private List<Material> _materials;
    private int? _editSaleId;
    
    public SaleEntryForm()
    {
        InitializeComponent();
        _vehicles = new List<Vehicle>();
        _buyers = new List<Buyer>();
        _materials = new List<Material>();
        _editSaleId = null;
    }
    
    public SaleEntryForm(int saleId) : this()
    {
        _editSaleId = saleId;
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
                if (!_editSaleId.HasValue)
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
            dtpSaleDate,
            cmbVehicle,
            cmbBuyer,
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
    
    private void SaleEntryForm_Load(object sender, EventArgs e)
    {
        dtpSaleDate.Value = DateTime.Today;
        LoadMasterData();
        
        if (_editSaleId.HasValue)
        {
            this.Text = "Edit Sale";
            btnSaveAndNew.Visible = false;
            LoadSaleForEdit();
        }
    }
    
    private void LoadSaleForEdit()
    {
        if (!_editSaleId.HasValue) return;
        
        var sale = SaleRepository.GetById(_editSaleId.Value);
        if (sale != null)
        {
            dtpSaleDate.Value = sale.SaleDate;
            cmbVehicle.SelectedValue = sale.VehicleId;
            cmbBuyer.SelectedValue = sale.BuyerId;
            cmbMaterial.SelectedValue = sale.MaterialId;
            txtQuantity.Text = sale.Quantity.ToString("N2");
            txtRate.Text = sale.Rate.ToString("N2");
            txtAmount.Text = sale.Amount.ToString("N2");
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
            
            // Load buyers
            _buyers = BuyerRepository.GetAll(activeOnly: true);
            cmbBuyer.DataSource = _buyers;
            cmbBuyer.DisplayMember = "BuyerName";
            cmbBuyer.ValueMember = "BuyerId";
            cmbBuyer.SelectedIndex = -1;
            
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
    
    private void BtnAddBuyer_Click(object sender, EventArgs e)
    {
        using var buyerForm = new BuyerMasterForm();
        buyerForm.ShowDialog();
        
        // Reload buyers
        var selectedBuyerId = cmbBuyer.SelectedValue;
        _buyers = BuyerRepository.GetAll(activeOnly: true);
        cmbBuyer.DataSource = _buyers;
        
        // Try to restore selection
        if (selectedBuyerId != null)
        {
            cmbBuyer.SelectedValue = selectedBuyerId;
        }
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;
        
        try
        {
            var sale = new Sale
            {
                SaleDate = dtpSaleDate.Value.Date,
                VehicleId = (int)cmbVehicle.SelectedValue,
                BuyerId = (int)cmbBuyer.SelectedValue,
                MaterialId = (int)cmbMaterial.SelectedValue,
                Quantity = decimal.Parse(txtQuantity.Text),
                Rate = decimal.Parse(txtRate.Text),
                Amount = decimal.Parse(txtAmount.Text)
            };
            
            if (_editSaleId.HasValue)
            {
                sale.SaleId = _editSaleId.Value;
                SaleRepository.Update(sale);
                MessageBox.Show("Sale updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SaleRepository.Insert(sale);
                MessageBox.Show("Sale saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            Close();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save sale");
            MessageBox.Show("Failed to save sale: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnSaveAndNew_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;
        
        try
        {
            var sale = new Sale
            {
                SaleDate = dtpSaleDate.Value.Date,
                VehicleId = (int)cmbVehicle.SelectedValue,
                BuyerId = (int)cmbBuyer.SelectedValue,
                MaterialId = (int)cmbMaterial.SelectedValue,
                Quantity = decimal.Parse(txtQuantity.Text),
                Rate = decimal.Parse(txtRate.Text),
                Amount = decimal.Parse(txtAmount.Text)
            };
            
            SaleRepository.Insert(sale);
            
            MessageBox.Show("Sale saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Clear form for next entry
            ClearForm();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save sale");
            MessageBox.Show("Failed to save sale: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        
        if (cmbBuyer.SelectedIndex == -1)
        {
            MessageBox.Show("Please select a buyer", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbBuyer.Focus();
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
        dtpSaleDate.Value = DateTime.Today;
        cmbVehicle.SelectedIndex = -1;
        cmbBuyer.SelectedIndex = -1;
        cmbMaterial.SelectedIndex = -1;
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
