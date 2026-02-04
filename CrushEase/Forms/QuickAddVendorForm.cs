using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

/// <summary>
/// Quick add vendor form (compact, for inline use)
/// </summary>
public partial class QuickAddVendorForm : Form
{
    private TextBox _txtVendorName;
    private TextBox _txtContact;
    private Button _btnSave;
    private Button _btnCancel;
    
    public int? NewVendorId { get; private set; }
    
    public QuickAddVendorForm()
    {
        InitializeComponent();
        SetupUI();
        ModernTheme.ApplyToForm(this);
    }
    
    private void SetupUI()
    {
        this.Text = "Quick Add Vendor";
        this.Size = new Size(400, 200);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        // Vendor Name
        var lblName = new Label
        {
            Text = "Vendor Name: *",
            Location = new Point(20, 20),
            Size = new Size(100, 25)
        };
        this.Controls.Add(lblName);
        
        _txtVendorName = new TextBox
        {
            Location = new Point(130, 18),
            Size = new Size(230, 25),
            Font = new Font("Segoe UI", 10)
        };
        this.Controls.Add(_txtVendorName);
        
        // Contact
        var lblContact = new Label
        {
            Text = "Contact:",
            Location = new Point(20, 60),
            Size = new Size(100, 25)
        };
        this.Controls.Add(lblContact);
        
        _txtContact = new TextBox
        {
            Location = new Point(130, 58),
            Size = new Size(230, 25),
            Font = new Font("Segoe UI", 10)
        };
        this.Controls.Add(_txtContact);
        
        // Buttons
        _btnSave = new Button
        {
            Text = "Add Vendor",
            Location = new Point(130, 110),
            Size = new Size(110, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };
        _btnSave.Click += BtnSave_Click;
        this.Controls.Add(_btnSave);
        
        _btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(250, 110),
            Size = new Size(110, 35),
            Font = new Font("Segoe UI", 10)
        };
        _btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; Close(); };
        this.Controls.Add(_btnCancel);
        
        this.AcceptButton = _btnSave;
        this.CancelButton = _btnCancel;
    }
    
    private void BtnSave_Click(object? sender, EventArgs e)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(_txtVendorName.Text))
        {
            ToastNotification.ShowWarning("Please enter vendor name");
            _txtVendorName.Focus();
            return;
        }
        
        try
        {
            var vendor = new Vendor
            {
                VendorName = _txtVendorName.Text.Trim(),
                Contact = _txtContact.Text.Trim(),
                IsActive = true
            };
            
            NewVendorId = VendorRepository.Insert(vendor);
            
            ToastNotification.ShowSuccess($"Vendor '{vendor.VendorName}' added successfully!");
            this.DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to add vendor");
            ToastNotification.ShowError($"Failed to add vendor: {ex.Message}");
        }
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.ClientSize = new Size(400, 180);
        this.Name = "QuickAddVendorForm";
        this.Text = "Quick Add Vendor";
        this.ResumeLayout(false);
    }
    
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _txtVendorName.Focus();
    }
}
