using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

/// <summary>
/// Quick add material form (compact, for inline use)
/// </summary>
public partial class QuickAddMaterialForm : Form
{
    private TextBox _txtMaterialName;
    private ComboBox _cmbUnit;
    private TextBox _txtConversionFactor;
    private Button _btnSave;
    private Button _btnCancel;
    
    public int? NewMaterialId { get; private set; }
    
    public QuickAddMaterialForm()
    {
        InitializeComponent();
        SetupUI();
        ModernTheme.ApplyToForm(this);
    }
    
    private void SetupUI()
    {
        this.Text = "Quick Add Material";
        this.Size = new Size(400, 250);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        
        // Material Name
        var lblName = new Label
        {
            Text = "Material Name: *",
            Location = new Point(20, 20),
            Size = new Size(120, 25)
        };
        this.Controls.Add(lblName);
        
        _txtMaterialName = new TextBox
        {
            Location = new Point(150, 18),
            Size = new Size(210, 25),
            Font = new Font("Segoe UI", 10)
        };
        this.Controls.Add(_txtMaterialName);
        
        // Unit
        var lblUnit = new Label
        {
            Text = "Default Unit:",
            Location = new Point(20, 60),
            Size = new Size(120, 25)
        };
        this.Controls.Add(lblUnit);
        
        _cmbUnit = new ComboBox
        {
            Location = new Point(150, 58),
            Size = new Size(210, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _cmbUnit.Items.AddRange(new object[] { "Ton", "CFT", "MT", "Kg", "Pcs" });
        _cmbUnit.SelectedIndex = 0;
        this.Controls.Add(_cmbUnit);
        
        // Conversion Factor
        var lblConversion = new Label
        {
            Text = "MT/CFT Factor:",
            Location = new Point(20, 100),
            Size = new Size(120, 25)
        };
        this.Controls.Add(lblConversion);
        
        _txtConversionFactor = new TextBox
        {
            Location = new Point(150, 98),
            Size = new Size(100, 25),
            Text = "0.04",
            Font = new Font("Segoe UI", 10)
        };
        this.Controls.Add(_txtConversionFactor);
        
        var lblHint = new Label
        {
            Text = "(e.g., 0.04 means 4 MT = 100 CFT)",
            Location = new Point(260, 100),
            Size = new Size(200, 25),
            ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8)
        };
        this.Controls.Add(lblHint);
        
        // Buttons
        _btnSave = new Button
        {
            Text = "Add Material",
            Location = new Point(150, 150),
            Size = new Size(110, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };
        _btnSave.Click += BtnSave_Click;
        this.Controls.Add(_btnSave);
        
        _btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(270, 150),
            Size = new Size(90, 35),
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
        if (string.IsNullOrWhiteSpace(_txtMaterialName.Text))
        {
            ToastNotification.ShowWarning("Please enter material name");
            _txtMaterialName.Focus();
            return;
        }
        
        if (!decimal.TryParse(_txtConversionFactor.Text, out decimal conversionFactor) || conversionFactor <= 0)
        {
            ToastNotification.ShowWarning("Please enter a valid conversion factor");
            _txtConversionFactor.Focus();
            return;
        }
        
        try
        {
            var material = new Material
            {
                MaterialName = _txtMaterialName.Text.Trim(),
                Unit = _cmbUnit.SelectedItem?.ToString() ?? "Ton",
                ConversionFactor_MT_to_CFT = conversionFactor,
                IsActive = true
            };
            
            NewMaterialId = MaterialRepository.Insert(material);
            
            ToastNotification.ShowSuccess($"Material '{material.MaterialName}' added successfully!");
            this.DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to add material");
            ToastNotification.ShowError($"Failed to add material: {ex.Message}");
        }
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.ClientSize = new Size(400, 230);
        this.Name = "QuickAddMaterialForm";
        this.Text = "Quick Add Material";
        this.ResumeLayout(false);
    }
    
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _txtMaterialName.Focus();
    }
}
