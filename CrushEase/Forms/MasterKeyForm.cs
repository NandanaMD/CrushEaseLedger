using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class MasterKeyForm : Form
{
    public MasterKeyForm()
    {
        InitializeComponent();
    }
    
    private void MasterKeyForm_Load(object sender, EventArgs e)
    {
        txtMasterKey.Focus();
    }
    
    private void TxtMasterKey_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Allow letters, digits, and backspace only
        if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
        {
            e.Handled = true;
        }
        
        // Enter key submits
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            BtnUnlock_Click(sender, EventArgs.Empty);
        }
    }
    
    private void BtnUnlock_Click(object sender, EventArgs e)
    {
        string masterKey = txtMasterKey.Text.Trim();
        
        if (string.IsNullOrEmpty(masterKey))
        {
            ShowError("Please enter the master key");
            return;
        }
        
        if (SecurityService.VerifyPin(masterKey))
        {
            Logger.LogInfo("Application unlocked using master key - redirecting to PIN setup");
            
            // Hide this form first
            this.Hide();
            
            // Show setup mode to create new PIN (no parent, so it centers on screen)
            using var setupForm = new LockScreenForm(setupMode: true, parent: null);
            setupForm.WindowState = FormWindowState.Maximized;
            setupForm.FormBorderStyle = FormBorderStyle.None;
            var result = setupForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                // User cancelled PIN setup - show this form again
                this.Show();
                txtMasterKey.Clear();
                txtMasterKey.Focus();
                return;
            }
            
            this.Close();
        }
        else
        {
            ShowError("Invalid master key");
            txtMasterKey.Clear();
            txtMasterKey.Focus();
        }
    }
    
    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
    
    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
        
        // Auto-hide error after 3 seconds
        var timer = new System.Windows.Forms.Timer();
        timer.Interval = 3000;
        timer.Tick += (s, e) =>
        {
            lblError.Visible = false;
            timer.Stop();
            timer.Dispose();
        };
        timer.Start();
    }
}
