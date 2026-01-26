using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class LockScreenForm : Form
{
    private bool _isSetupMode;
    private string _confirmPin = "";
    private int _failedAttempts = 0;
    private Form? _parentForm;
    
    public LockScreenForm(bool setupMode = false, Form? parent = null)
    {
        InitializeComponent();
        _isSetupMode = setupMode;
        _parentForm = parent;
        
        if (_isSetupMode)
        {
            lblTitle.Text = "Setup PIN";
            lblInstruction.Text = "Enter a 4-digit PIN to secure CrushEase";
        }
        else
        {
            lblTitle.Text = "CrushEase Locked";
            lblInstruction.Text = "Enter your 4-digit PIN to unlock";
        }
    }
    
    private void LockScreenForm_Load(object sender, EventArgs e)
    {
        // Cover the entire parent form if provided
        if (_parentForm != null)
        {
            this.Size = _parentForm.Size;
            this.Location = _parentForm.Location;
            
            // Center all controls
            CenterControls();
        }
        else if (this.WindowState == FormWindowState.Maximized)
        {
            // If maximized (setup mode from master key), center controls
            CenterControls();
        }
        else
        {
            // Center on screen
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        
        txtPin.Focus();
    }
    
    private void CenterControls()
    {
        // Calculate center position
        int centerX = this.Width / 2;
        int centerY = this.Height / 2;
        
        // Reposition all controls to center
        lblAppName.Left = (this.Width - lblAppName.Width) / 2;
        lblTitle.Left = (this.Width - lblTitle.Width) / 2;
        lblInstruction.Left = (this.Width - lblInstruction.Width) / 2;
        txtPin.Left = (this.Width - txtPin.Width) / 2;
        lblError.Left = (this.Width - lblError.Width) / 2;
        btnUnlock.Left = (this.Width - btnUnlock.Width) / 2;
        lnkForgotPin.Left = (this.Width - lnkForgotPin.Width) / 2;
        btnExit.Left = (this.Width - btnExit.Width) / 2;
        
        // Find lock icon label
        foreach (Control ctrl in this.Controls)
        {
            if (ctrl is Label lbl && lbl.Text == "🔒")
            {
                lbl.Left = (this.Width - lbl.Width) / 2;
                break;
            }
        }
    }
    
    private void TxtPin_TextChanged(object sender, EventArgs e)
    {
        // Auto-submit when 4 digits entered
        if (txtPin.Text.Length == 4)
        {
            BtnUnlock_Click(sender, e);
        }
    }
    
    private void TxtPin_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Only allow digits and backspace
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
        {
            e.Handled = true;
        }
    }
    
    private void BtnUnlock_Click(object sender, EventArgs e)
    {
        string pin = txtPin.Text.Trim();
        
        if (pin.Length != 4)
        {
            ShowError("PIN must be 4 digits");
            return;
        }
        
        if (_isSetupMode)
        {
            SetupPin(pin);
        }
        else
        {
            VerifyPin(pin);
        }
    }
    
    private void SetupPin(string pin)
    {
        if (string.IsNullOrEmpty(_confirmPin))
        {
            // First entry - ask for confirmation
            _confirmPin = pin;
            lblInstruction.Text = "Confirm your 4-digit PIN";
            txtPin.Clear();
            txtPin.Focus();
        }
        else
        {
            // Second entry - verify match
            if (pin == _confirmPin)
            {
                SecurityService.SetPin(pin);
                MessageBox.Show("PIN setup successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                ShowError("PINs do not match. Please try again.");
                _confirmPin = "";
                lblInstruction.Text = "Enter a 4-digit PIN to secure CrushEase";
                txtPin.Clear();
            }
        }
    }
    
    private void VerifyPin(string pin)
    {
        if (SecurityService.VerifyPin(pin))
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            _failedAttempts++;
            
            if (_failedAttempts >= 3)
            {
                ShowError($"Failed attempts: {_failedAttempts}\\n\\nForgot PIN? Contact support for master key.");
            }
            else
            {
                ShowError("Incorrect PIN. Please try again.");
            }
            
            txtPin.Clear();
            txtPin.Focus();
        }
    }
    
    private void LnkForgotPin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        using var masterKeyForm = new MasterKeyForm();
        if (masterKeyForm.ShowDialog() == DialogResult.OK)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
    
    private void BtnExit_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to exit CrushEase?",
            "Exit Application",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            Application.Exit();
        }
    }
    
    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
        
        // Shake animation
        var originalLocation = this.Location;
        for (int i = 0; i < 3; i++)
        {
            this.Location = new Point(originalLocation.X - 10, originalLocation.Y);
            Application.DoEvents();
            Thread.Sleep(50);
            this.Location = new Point(originalLocation.X + 10, originalLocation.Y);
            Application.DoEvents();
            Thread.Sleep(50);
        }
        this.Location = originalLocation;
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // Only prevent Escape key (Alt+F4 allowed for other apps)
        if (keyData == Keys.Escape)
        {
            return true;
        }
        
        if (keyData == Keys.Enter)
        {
            BtnUnlock_Click(this, EventArgs.Empty);
            return true;
        }
        
        return base.ProcessCmdKey(ref msg, keyData);
    }
}
