using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class BuyerMasterForm : Form
{
    private List<Buyer> _buyers = new();
    
    public BuyerMasterForm()
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
            txtBuyerName
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
    
    private void BuyerMasterForm_Load(object sender, EventArgs e)
    {
        LoadBuyers();
        Utils.ModernTheme.ApplyToForm(this);
    }
    
    private void LoadBuyers()
    {
        try
        {
            _buyers = BuyerRepository.GetAll(chkShowInactive.Checked == false);
            dgvBuyers.DataSource = _buyers;
            
            // Hide ID column
            if (dgvBuyers.Columns["BuyerId"] != null)
                dgvBuyers.Columns["BuyerId"].Visible = false;
            
            // Format column headers
            if (dgvBuyers.Columns["BuyerName"] != null)
                dgvBuyers.Columns["BuyerName"].HeaderText = "Buyer Name";
            
            if (dgvBuyers.Columns["Contact"] != null)
                dgvBuyers.Columns["Contact"].HeaderText = "Contact";
            
            if (dgvBuyers.Columns["Notes"] != null)
                dgvBuyers.Columns["Notes"].HeaderText = "Notes";
            
            if (dgvBuyers.Columns["IsActive"] != null)
                dgvBuyers.Columns["IsActive"].HeaderText = "Active";
            
            if (dgvBuyers.Columns["CreatedAt"] != null)
            {
                dgvBuyers.Columns["CreatedAt"].HeaderText = "Created Date";
                dgvBuyers.Columns["CreatedAt"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            }
            
            lblStatus.Text = $"{_buyers.Count} buyer(s)";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load buyers");
            MessageBox.Show("Failed to load buyers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnAdd_Click(object sender, EventArgs e)
    {
        txtBuyerName.Text = "";
        txtContact.Text = "";
        txtNotes.Text = "";
        txtBuyerName.Focus();
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtBuyerName.Text))
        {
            MessageBox.Show("Buyer name is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtBuyerName.Focus();
            return;
        }
        
        try
        {
            // Check for duplicates
            if (BuyerRepository.Exists(txtBuyerName.Text.Trim()))
            {
                MessageBox.Show("Buyer name already exists", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBuyerName.Focus();
                return;
            }
            
            var buyer = new Buyer
            {
                BuyerName = txtBuyerName.Text.Trim(),
                Contact = string.IsNullOrWhiteSpace(txtContact.Text) ? null : txtContact.Text.Trim(),
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                IsActive = true
            };
            
            BuyerRepository.Insert(buyer);
            
            txtBuyerName.Text = "";
            txtContact.Text = "";
            txtNotes.Text = "";
            LoadBuyers();
            
            MessageBox.Show("Buyer added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save buyer");
            MessageBox.Show("Failed to save: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dgvBuyers.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a buyer to edit", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        var buyer = (Buyer)dgvBuyers.SelectedRows[0].DataBoundItem;
        
        // Show edit dialog with all fields
        using var editForm = new BuyerEditDialog(buyer);
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Check for duplicates (excluding current)
                if (BuyerRepository.Exists(editForm.BuyerName.Trim(), buyer.BuyerId))
                {
                    MessageBox.Show("Buyer name already exists", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                buyer.BuyerName = editForm.BuyerName.Trim();
                buyer.Contact = string.IsNullOrWhiteSpace(editForm.Contact) ? null : editForm.Contact.Trim();
                buyer.Notes = string.IsNullOrWhiteSpace(editForm.Notes) ? null : editForm.Notes.Trim();
                
                BuyerRepository.Update(buyer);
                
                LoadBuyers();
                MessageBox.Show("Buyer updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update buyer");
                MessageBox.Show("Failed to update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnDelete_Click(object sender, EventArgs e)
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
            Logger.LogError(ex, "Failed to delete buyer");
            MessageBox.Show("Failed to delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void TxtSearch_TextChanged(object sender, EventArgs e)
    {
        var searchText = txtSearch.Text.ToLower();
        
        if (string.IsNullOrWhiteSpace(searchText))
        {
            dgvBuyers.DataSource = _buyers;
        }
        else
        {
            var filtered = _buyers.Where(b => 
                b.BuyerName.ToLower().Contains(searchText) ||
                (b.Contact != null && b.Contact.ToLower().Contains(searchText))
            ).ToList();
            dgvBuyers.DataSource = filtered;
        }
        
        lblStatus.Text = $"{dgvBuyers.Rows.Count} buyer(s)";
    }
    
    private void ChkShowInactive_CheckedChanged(object sender, EventArgs e)
    {
        LoadBuyers();
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}

/// <summary>
/// Dialog for editing buyer details
/// </summary>
internal class BuyerEditDialog : Form
{
    private TextBox txtBuyerName;
    private TextBox txtContact;
    private TextBox txtNotes;
    
    public string BuyerName => txtBuyerName.Text;
    public string Contact => txtContact.Text;
    public string Notes => txtNotes.Text;
    
    public BuyerEditDialog(Buyer buyer)
    {
        InitializeComponent();
        txtBuyerName!.Text = buyer.BuyerName;
        txtContact!.Text = buyer.Contact ?? "";
        txtNotes!.Text = buyer.Notes ?? "";
    }
    
    private void InitializeComponent()
    {
        this.Width = 450;
        this.Height = 250;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Text = "Edit Buyer";
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        var lblBuyerName = new Label { Left = 20, Top = 20, Width = 100, Text = "Buyer Name:" };
        txtBuyerName = new TextBox { Left = 130, Top = 17, Width = 280 };
        
        var lblContact = new Label { Left = 20, Top = 55, Width = 100, Text = "Contact:" };
        txtContact = new TextBox { Left = 130, Top = 52, Width = 280 };
        
        var lblNotes = new Label { Left = 20, Top = 90, Width = 100, Text = "Notes:" };
        txtNotes = new TextBox { Left = 130, Top = 87, Width = 280, Height = 60, Multiline = true };
        
        var btnOK = new Button { Text = "OK", Left = 230, Width = 80, Top = 165, DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Cancel", Left = 320, Width = 80, Top = 165, DialogResult = DialogResult.Cancel };
        
        btnOK.Click += (s, e) => { if (string.IsNullOrWhiteSpace(txtBuyerName.Text)) { MessageBox.Show("Buyer name is required"); } else { this.Close(); } };
        btnCancel.Click += (s, e) => { this.Close(); };
        
        this.Controls.Add(lblBuyerName);
        this.Controls.Add(txtBuyerName);
        this.Controls.Add(lblContact);
        this.Controls.Add(txtContact);
        this.Controls.Add(lblNotes);
        this.Controls.Add(txtNotes);
        this.Controls.Add(btnOK);
        this.Controls.Add(btnCancel);
        
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
        
        txtBuyerName.SelectAll();
        txtBuyerName.Focus();
    }
}
