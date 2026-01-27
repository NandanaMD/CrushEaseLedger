using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class CompanySettingsForm : Form
{
    private byte[]? _logoImageBytes;
    
    public CompanySettingsForm()
    {
        InitializeComponent();
    }
    
    private void CompanySettingsForm_Load(object sender, EventArgs e)
    {
        LoadSettings();
        ModernTheme.ApplyToForm(this);
    }
    
    private void LoadSettings()
    {
        var settings = CompanySettingsRepository.Get();
        
        if (settings != null)
        {
            txtCompanyName.Text = settings.CompanyName;
            txtAddress.Text = settings.Address;
            txtPhone.Text = settings.Phone;
            txtEmail.Text = settings.Email;
            txtGSTNumber.Text = settings.GSTNumber ?? string.Empty;
            txtWebsite.Text = settings.Website ?? string.Empty;
            txtInvoicePrefix.Text = settings.InvoicePrefix;
            txtPaymentTerms.Text = settings.PaymentTerms;
            txtTermsAndConditions.Text = settings.TermsAndConditions ?? string.Empty;
            
            if (settings.LogoImage != null && settings.LogoImage.Length > 0)
            {
                _logoImageBytes = settings.LogoImage;
                using var ms = new MemoryStream(settings.LogoImage);
                picLogo.Image = Image.FromStream(ms);
            }
        }
        else
        {
            // Set defaults for new setup
            txtInvoicePrefix.Text = "INV";
            txtPaymentTerms.Text = "Payment Due on Receipt";
        }
    }
    
    private void BtnBrowseLogo_Click(object sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog
        {
            Title = "Select Company Logo",
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*",
            FilterIndex = 1
        };
        
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Load and display image
                picLogo.Image = Image.FromFile(openFileDialog.FileName);
                
                // Convert to byte array for storage
                using var ms = new MemoryStream();
                using var img = Image.FromFile(openFileDialog.FileName);
                
                // Resize if too large (max 200x200)
                var resized = ResizeImage(img, 200, 200);
                resized.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                _logoImageBytes = ms.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading logo: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnClearLogo_Click(object sender, EventArgs e)
    {
        picLogo.Image = null;
        _logoImageBytes = null;
    }
    
    private Image ResizeImage(Image image, int maxWidth, int maxHeight)
    {
        // Calculate aspect ratio
        double ratioX = (double)maxWidth / image.Width;
        double ratioY = (double)maxHeight / image.Height;
        double ratio = Math.Min(ratioX, ratioY);
        
        int newWidth = (int)(image.Width * ratio);
        int newHeight = (int)(image.Height * ratio);
        
        var newImage = new Bitmap(newWidth, newHeight);
        using (var graphics = Graphics.FromImage(newImage))
        {
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, 0, 0, newWidth, newHeight);
        }
        
        return newImage;
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
        {
            MessageBox.Show("Please enter company name.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCompanyName.Focus();
            return;
        }
        
        if (string.IsNullOrWhiteSpace(txtInvoicePrefix.Text))
        {
            MessageBox.Show("Please enter invoice prefix.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtInvoicePrefix.Focus();
            return;
        }
        
        // Validate invoice prefix (alphanumeric only)
        if (!System.Text.RegularExpressions.Regex.IsMatch(txtInvoicePrefix.Text, @"^[A-Z0-9]+$"))
        {
            MessageBox.Show("Invoice prefix must contain only uppercase letters and numbers.", 
                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtInvoicePrefix.Focus();
            return;
        }
        
        try
        {
            var settings = new CompanySettings
            {
                CompanyName = txtCompanyName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                GSTNumber = string.IsNullOrWhiteSpace(txtGSTNumber.Text) ? null : txtGSTNumber.Text.Trim(),
                Website = string.IsNullOrWhiteSpace(txtWebsite.Text) ? null : txtWebsite.Text.Trim(),
                LogoImage = _logoImageBytes,
                InvoicePrefix = txtInvoicePrefix.Text.Trim().ToUpper(),
                PaymentTerms = txtPaymentTerms.Text.Trim(),
                TermsAndConditions = string.IsNullOrWhiteSpace(txtTermsAndConditions.Text) 
                    ? null : txtTermsAndConditions.Text.Trim(),
                UpdatedAt = DateTime.Now
            };
            
            CompanySettingsRepository.SaveOrUpdate(settings);
            
            MessageBox.Show("Company settings saved successfully!", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save company settings");
            MessageBox.Show($"Error saving settings: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
