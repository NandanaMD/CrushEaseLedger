using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class MaterialMasterForm : Form
{
    private List<Material> _materials = new();
    
    public MaterialMasterForm()
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
        
        // For Material, only one input field, so arrow keys don't navigate
        // Just return false to allow default behavior
        return false;
    }
    
    private void MaterialMasterForm_Load(object sender, EventArgs e)
    {
        LoadMaterials();
        Utils.ModernTheme.ApplyToForm(this);
    }
    
    private void LoadMaterials()
    {
        try
        {
            _materials = MaterialRepository.GetAll(chkShowInactive.Checked == false);
            dgvMaterials.DataSource = _materials;
            
            // Hide ID column
            if (dgvMaterials.Columns["MaterialId"] != null)
                dgvMaterials.Columns["MaterialId"].Visible = false;
            
            // Format column headers
            if (dgvMaterials.Columns["MaterialName"] != null)
                dgvMaterials.Columns["MaterialName"].HeaderText = "Material Name";
            
            if (dgvMaterials.Columns["Unit"] != null)
                dgvMaterials.Columns["Unit"].HeaderText = "Unit";
            
            if (dgvMaterials.Columns["ConversionFactor_MT_to_CFT"] != null)
            {
                dgvMaterials.Columns["ConversionFactor_MT_to_CFT"].HeaderText = "Density (MT/CFT)";
                dgvMaterials.Columns["ConversionFactor_MT_to_CFT"].DefaultCellStyle.Format = "N4";
                dgvMaterials.Columns["ConversionFactor_MT_to_CFT"].Width = 110;
            }
            
            if (dgvMaterials.Columns["Notes"] != null)
                dgvMaterials.Columns["Notes"].HeaderText = "Notes";
            
            if (dgvMaterials.Columns["IsActive"] != null)
                dgvMaterials.Columns["IsActive"].HeaderText = "Active";
            
            if (dgvMaterials.Columns["CreatedAt"] != null)
            {
                dgvMaterials.Columns["CreatedAt"].HeaderText = "Created Date";
                dgvMaterials.Columns["CreatedAt"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            }
            
            lblStatus.Text = $"{_materials.Count} material(s)";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load materials");
            MessageBox.Show("Failed to load materials: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnAdd_Click(object sender, EventArgs e)
    {
        txtMaterialName.Text = "";
        txtUnit.Text = "";
        txtNotes.Text = "";
        txtMaterialName.Focus();
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaterialName.Text))
        {
            MessageBox.Show("Material name is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtMaterialName.Focus();
            return;
        }
        
        if (string.IsNullOrWhiteSpace(txtUnit.Text))
        {
            MessageBox.Show("Unit is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUnit.Focus();
            return;
        }
        
        try
        {
            // Check for duplicates
            if (MaterialRepository.Exists(txtMaterialName.Text.Trim()))
            {
                MessageBox.Show("Material name already exists", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaterialName.Focus();
                return;
            }
            
            var material = new Material
            {
                MaterialName = txtMaterialName.Text.Trim(),
                Unit = txtUnit.Text.Trim(),
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                IsActive = true
            };
            
            MaterialRepository.Insert(material);
            
            txtMaterialName.Text = "";
            txtUnit.Text = "";
            txtNotes.Text = "";
            LoadMaterials();
            
            MessageBox.Show("Material added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save material");
            MessageBox.Show("Failed to save: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dgvMaterials.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a material to edit", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var material = (Material)dgvMaterials.SelectedRows[0].DataBoundItem;
        
        // Show edit dialog with all fields
        using var editForm = new MaterialEditDialog(material);
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Check for duplicates (excluding current)
                if (MaterialRepository.Exists(editForm.MaterialName.Trim(), material.MaterialId))
                {
                    MessageBox.Show("Material name already exists", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                material.MaterialName = editForm.MaterialName.Trim();
                material.Unit = editForm.Unit.Trim();
                material.Notes = string.IsNullOrWhiteSpace(editForm.Notes) ? null : editForm.Notes.Trim();
                material.ConversionFactor_MT_to_CFT = editForm.ConversionFactor;
                
                MaterialRepository.Update(material);
                
                LoadMaterials();
                MessageBox.Show("Material updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update material");
                MessageBox.Show("Failed to update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnDelete_Click(object sender, EventArgs e)
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
            Logger.LogError(ex, "Failed to delete material");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void TxtSearch_TextChanged(object sender, EventArgs e)
    {
        var searchText = txtSearch.Text.ToLower();
        
        if (string.IsNullOrWhiteSpace(searchText))
        {
            dgvMaterials.DataSource = _materials;
        }
        else
        {
            var filtered = _materials.Where(m => 
                m.MaterialName.ToLower().Contains(searchText) ||
                m.Unit.ToLower().Contains(searchText)
            ).ToList();
            dgvMaterials.DataSource = filtered;
        }
        
        lblStatus.Text = $"{dgvMaterials.Rows.Count} material(s)";
    }
    
    private void ChkShowInactive_CheckedChanged(object sender, EventArgs e)
    {
        LoadMaterials();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}

/// <summary>
/// Dialog for editing material details
/// </summary>
internal class MaterialEditDialog : Form
{
    private TextBox txtMaterialName;
    private TextBox txtUnit;
    private NumericUpDown numConversionFactor;
    private TextBox txtNotes;
    
    public string MaterialName => txtMaterialName.Text;
    public string Unit => txtUnit.Text;
    public decimal ConversionFactor => numConversionFactor.Value;
    public string Notes => txtNotes.Text;
    
    public MaterialEditDialog(Material material)
    {
        InitializeComponent();
        txtMaterialName!.Text = material.MaterialName;
        txtUnit!.Text = material.Unit;
        numConversionFactor!.Value = material.ConversionFactor_MT_to_CFT;
        txtNotes!.Text = material.Notes ?? "";
    }
    
    private void InitializeComponent()
    {
        this.Width = 450;
        this.Height = 300;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Text = "Edit Material";
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        var lblMaterialName = new Label { Left = 20, Top = 20, Width = 100, Text = "Material Name:" };
        txtMaterialName = new TextBox { Left = 130, Top = 17, Width = 280 };
        
        var lblUnit = new Label { Left = 20, Top = 55, Width = 100, Text = "Unit:" };
        txtUnit = new TextBox { Left = 130, Top = 52, Width = 280 };
        
        var lblConversionFactor = new Label { Left = 20, Top = 90, Width = 100, Text = "Density (MT/CFT):" };
        numConversionFactor = new NumericUpDown { Left = 130, Top = 87, Width = 150, Minimum = 0.0001m, Maximum = 10m, DecimalPlaces = 4, Increment = 0.01m };
        
        var lblNotes = new Label { Left = 20, Top = 125, Width = 100, Text = "Notes:" };
        txtNotes = new TextBox { Left = 130, Top = 122, Width = 280, Height = 60, Multiline = true };
        
        var btnOK = new Button { Text = "OK", Left = 230, Width = 80, Top = 200, DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Cancel", Left = 320, Width = 80, Top = 200, DialogResult = DialogResult.Cancel };
        
        btnOK.Click += (s, e) => { 
            if (string.IsNullOrWhiteSpace(txtMaterialName.Text)) 
            { 
                MessageBox.Show("Material name is required"); 
            } 
            else if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                MessageBox.Show("Unit is required");
            }
            else if (numConversionFactor.Value <= 0)
            {
                MessageBox.Show("Conversion factor must be greater than zero");
            }
            else 
            { 
                this.Close(); 
            } 
        };
        btnCancel.Click += (s, e) => { this.Close(); };
        
        this.Controls.Add(lblMaterialName);
        this.Controls.Add(txtMaterialName);
        this.Controls.Add(lblUnit);
        this.Controls.Add(txtUnit);
        this.Controls.Add(lblConversionFactor);
        this.Controls.Add(numConversionFactor);
        this.Controls.Add(lblNotes);
        this.Controls.Add(txtNotes);
        this.Controls.Add(btnOK);
        this.Controls.Add(btnCancel);
        
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
        
        txtMaterialName.SelectAll();
        txtMaterialName.Focus();
    }
}
