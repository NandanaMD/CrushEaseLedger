using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class VendorMasterForm : Form
{
    private List<Vendor> _vendors = new();
    
    public VendorMasterForm()
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
        
        List<Control> navOrder = new List<Control>
        {
            txtVendorName
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
    
    private void VendorMasterForm_Load(object sender, EventArgs e)
    {
        LoadVendors();
    }
    
    private void LoadVendors()
    {
        try
        {
            _vendors = VendorRepository.GetAll(chkShowInactive.Checked == false);
            dgvVendors.DataSource = _vendors;
            
            // Hide ID column
            if (dgvVendors.Columns["VendorId"] != null)
                dgvVendors.Columns["VendorId"].Visible = false;
            
            // Format column headers
            if (dgvVendors.Columns["VendorName"] != null)
                dgvVendors.Columns["VendorName"].HeaderText = "Vendor Name";
            
            if (dgvVendors.Columns["Contact"] != null)
                dgvVendors.Columns["Contact"].HeaderText = "Contact";
            
            if (dgvVendors.Columns["Notes"] != null)
                dgvVendors.Columns["Notes"].HeaderText = "Notes";
            
            if (dgvVendors.Columns["IsActive"] != null)
                dgvVendors.Columns["IsActive"].HeaderText = "Active";
            
            if (dgvVendors.Columns["CreatedAt"] != null)
            {
                dgvVendors.Columns["CreatedAt"].HeaderText = "Created Date";
                dgvVendors.Columns["CreatedAt"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            }
            
            lblStatus.Text = $"{_vendors.Count} vendor(s)";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load vendors");
            MessageBox.Show("Failed to load vendors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnAdd_Click(object sender, EventArgs e)
    {
        txtVendorName.Text = "";
        txtContact.Text = "";
        txtNotes.Text = "";
        txtVendorName.Focus();
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtVendorName.Text))
        {
            MessageBox.Show("Vendor name is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtVendorName.Focus();
            return;
        }
        
        try
        {
            // Check for duplicates
            if (VendorRepository.Exists(txtVendorName.Text.Trim()))
            {
                MessageBox.Show("Vendor name already exists", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtVendorName.Focus();
                return;
            }
            
            var vendor = new Vendor
            {
                VendorName = txtVendorName.Text.Trim(),
                Contact = string.IsNullOrWhiteSpace(txtContact.Text) ? null : txtContact.Text.Trim(),
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                IsActive = true
            };
            
            VendorRepository.Insert(vendor);
            
            txtVendorName.Text = "";
            txtContact.Text = "";
            txtNotes.Text = "";
            LoadVendors();
            
            MessageBox.Show("Vendor added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save vendor");
            MessageBox.Show("Failed to save: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dgvVendors.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a vendor to edit", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var vendor = (Vendor)dgvVendors.SelectedRows[0].DataBoundItem;
        
        // Show edit dialog with all fields
        using var editForm = new VendorEditDialog(vendor);
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Check for duplicates (excluding current)
                if (VendorRepository.Exists(editForm.VendorName.Trim(), vendor.VendorId))
                {
                    MessageBox.Show("Vendor name already exists", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                vendor.VendorName = editForm.VendorName.Trim();
                vendor.Contact = string.IsNullOrWhiteSpace(editForm.Contact) ? null : editForm.Contact.Trim();
                vendor.Notes = string.IsNullOrWhiteSpace(editForm.Notes) ? null : editForm.Notes.Trim();
                
                VendorRepository.Update(vendor);
                
                LoadVendors();
                MessageBox.Show("Vendor updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update vendor");
                MessageBox.Show("Failed to update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnDelete_Click(object sender, EventArgs e)
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
            Logger.LogError(ex, "Failed to delete vendor");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void TxtSearch_TextChanged(object sender, EventArgs e)
    {
        var searchText = txtSearch.Text.ToLower();
        
        if (string.IsNullOrWhiteSpace(searchText))
        {
            dgvVendors.DataSource = _vendors;
        }
        else
        {
            var filtered = _vendors.Where(v => 
                v.VendorName.ToLower().Contains(searchText) ||
                (v.Contact != null && v.Contact.ToLower().Contains(searchText))
            ).ToList();
            dgvVendors.DataSource = filtered;
        }
        
        lblStatus.Text = $"{dgvVendors.Rows.Count} vendor(s)";
    }
    
    private void ChkShowInactive_CheckedChanged(object sender, EventArgs e)
    {
        LoadVendors();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}

/// <summary>
/// Dialog for editing vendor details
/// </summary>
internal class VendorEditDialog : Form
{
    private TextBox txtVendorName;
    private TextBox txtContact;
    private TextBox txtNotes;
    
    public string VendorName => txtVendorName.Text;
    public string Contact => txtContact.Text;
    public string Notes => txtNotes.Text;
    
    public VendorEditDialog(Vendor vendor)
    {
        InitializeComponent();
        txtVendorName!.Text = vendor.VendorName;
        txtContact!.Text = vendor.Contact ?? "";
        txtNotes!.Text = vendor.Notes ?? "";
    }
    
    private void InitializeComponent()
    {
        this.Width = 450;
        this.Height = 250;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Text = "Edit Vendor";
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        var lblVendorName = new Label { Left = 20, Top = 20, Width = 100, Text = "Vendor Name:" };
        txtVendorName = new TextBox { Left = 130, Top = 17, Width = 280 };
        
        var lblContact = new Label { Left = 20, Top = 55, Width = 100, Text = "Contact:" };
        txtContact = new TextBox { Left = 130, Top = 52, Width = 280 };
        
        var lblNotes = new Label { Left = 20, Top = 90, Width = 100, Text = "Notes:" };
        txtNotes = new TextBox { Left = 130, Top = 87, Width = 280, Height = 60, Multiline = true };
        
        var btnOK = new Button { Text = "OK", Left = 230, Width = 80, Top = 165, DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Cancel", Left = 320, Width = 80, Top = 165, DialogResult = DialogResult.Cancel };
        
        btnOK.Click += (s, e) => { if (string.IsNullOrWhiteSpace(txtVendorName.Text)) { MessageBox.Show("Vendor name is required"); } else { this.Close(); } };
        btnCancel.Click += (s, e) => { this.Close(); };
        
        this.Controls.Add(lblVendorName);
        this.Controls.Add(txtVendorName);
        this.Controls.Add(lblContact);
        this.Controls.Add(txtContact);
        this.Controls.Add(lblNotes);
        this.Controls.Add(txtNotes);
        this.Controls.Add(btnOK);
        this.Controls.Add(btnCancel);
        
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
        
        txtVendorName.SelectAll();
        txtVendorName.Focus();
    }
}
