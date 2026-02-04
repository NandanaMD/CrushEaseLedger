using CrushEase.Utils;

namespace CrushEase.Forms;

/// <summary>
/// Global search dialog that appears on Ctrl+F
/// </summary>
public partial class GlobalSearchDialog : Form
{
    private string _searchText = "";
    
    public string SearchText => _searchText;
    
    public GlobalSearchDialog()
    {
        InitializeComponent();
    }
    
    private void GlobalSearchDialog_Load(object sender, EventArgs e)
    {
        // Center on parent form
        if (this.Owner != null)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                this.Owner.Location.X + (this.Owner.Width - this.Width) / 2,
                this.Owner.Location.Y + 100
            );
        }
        
        // Focus the search textbox
        txtSearch.Focus();
        
        // Apply modern theme
        ModernTheme.ApplyToForm(this);
    }
    
    private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            PerformSearch();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
    
    private void BtnSearch_Click(object sender, EventArgs e)
    {
        PerformSearch();
    }
    
    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
    
    private void PerformSearch()
    {
        _searchText = txtSearch.Text.Trim();
        
        if (string.IsNullOrEmpty(_searchText))
        {
            MessageBox.Show("Please enter search text.", "Search", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
            return;
        }
        
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}
