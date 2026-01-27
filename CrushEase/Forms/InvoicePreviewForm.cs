using CrushEase.Services;
using CrushEase.Utils;
using System.Diagnostics;

namespace CrushEase.Forms;

public partial class InvoicePreviewForm : Form
{
    private readonly InvoiceResult _invoiceResult;
    
    public InvoicePreviewForm(InvoiceResult invoiceResult)
    {
        InitializeComponent();
        _invoiceResult = invoiceResult;
    }
    
    private void InvoicePreviewForm_Load(object sender, EventArgs e)
    {
        lblInvoiceNumber.Text = $"Invoice Number: {_invoiceResult.InvoiceNumber}";
        txtFilePath.Text = _invoiceResult.FilePath;
        
        ModernTheme.ApplyToForm(this);
    }
    
    private void BtnOpenFile_Click(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists(_invoiceResult.FilePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _invoiceResult.FilePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("PDF file not found.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening PDF: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnOpenFolder_Click(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists(_invoiceResult.FilePath))
            {
                Process.Start("explorer.exe", $"/select,\"{_invoiceResult.FilePath}\"");
            }
            else
            {
                var directory = Path.GetDirectoryName(_invoiceResult.FilePath);
                if (Directory.Exists(directory))
                {
                    Process.Start("explorer.exe", directory);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening folder: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (!File.Exists(_invoiceResult.FilePath))
            {
                MessageBox.Show("PDF file not found.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Open with default PDF viewer which typically has print option
            // Or use verb "print" to directly invoke print dialog
            Process.Start(new ProcessStartInfo
            {
                FileName = _invoiceResult.FilePath,
                Verb = "print",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true
            });
            
            MessageBox.Show("Invoice sent to printer. Please check your print queue.", 
                "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to print invoice");
            
            // Fallback: open the file normally so user can print manually
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _invoiceResult.FilePath,
                    UseShellExecute = true
                });
                
                MessageBox.Show("Please use your PDF viewer's print function to print the invoice.", 
                    "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show($"Error printing invoice: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void BtnSaveAs_Click(object sender, EventArgs e)
    {
        try
        {
            using var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Invoice As",
                Filter = "PDF Files|*.pdf",
                FileName = Path.GetFileName(_invoiceResult.FilePath),
                DefaultExt = "pdf"
            };
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(_invoiceResult.FilePath))
                {
                    File.Copy(_invoiceResult.FilePath, saveFileDialog.FileName, true);
                    MessageBox.Show($"Invoice saved successfully to:\n{saveFileDialog.FileName}", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Source PDF file not found.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving invoice: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}
