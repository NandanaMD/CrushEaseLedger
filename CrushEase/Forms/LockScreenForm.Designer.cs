namespace CrushEase.Forms
{
    partial class LockScreenForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblInstruction = new Label();
            this.txtPin = new TextBox();
            this.btnUnlock = new Button();
            this.lnkForgotPin = new LinkLabel();
            this.lblError = new Label();
            this.lblAppName = new Label();
            this.btnExit = new Button();
            
            this.SuspendLayout();
            
            // Form
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ClientSize = new Size(450, 550);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LockScreenForm";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "CrushEase Lock";
            this.Opacity = 0.98;
            this.Load += new EventHandler(this.LockScreenForm_Load);
            
            // lblAppName
            this.lblAppName.AutoSize = false;
            this.lblAppName.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblAppName.ForeColor = Color.FromArgb(8, 145, 178);
            this.lblAppName.Location = new Point(0, 60);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new Size(450, 50);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "CrushEase";
            this.lblAppName.TextAlign = ContentAlignment.MiddleCenter;
            
            // Lock Icon (using text)
            var lblLockIcon = new Label();
            lblLockIcon.AutoSize = false;
            lblLockIcon.Font = new Font("Segoe UI", 48F, FontStyle.Bold);
            lblLockIcon.ForeColor = Color.FromArgb(8, 145, 178);
            lblLockIcon.Location = new Point(0, 120);
            lblLockIcon.Name = "lblLockIcon";
            lblLockIcon.Size = new Size(450, 80);
            lblLockIcon.TabIndex = 1;
            lblLockIcon.Text = "🔒";
            lblLockIcon.TextAlign = ContentAlignment.MiddleCenter;
            
            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblTitle.Location = new Point(0, 210);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(450, 40);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Enter PIN";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            
            // lblInstruction
            this.lblInstruction.AutoSize = false;
            this.lblInstruction.Font = new Font("Segoe UI", 10F);
            this.lblInstruction.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblInstruction.Location = new Point(50, 255);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new Size(350, 30);
            this.lblInstruction.TabIndex = 3;
            this.lblInstruction.Text = "Enter your 4-digit PIN to unlock";
            this.lblInstruction.TextAlign = ContentAlignment.MiddleCenter;
            
            // txtPin
            this.txtPin.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.txtPin.Location = new Point(125, 300);
            this.txtPin.MaxLength = 4;
            this.txtPin.Name = "txtPin";
            this.txtPin.PasswordChar = '●';
            this.txtPin.Size = new Size(200, 50);
            this.txtPin.TabIndex = 4;
            this.txtPin.TextAlign = HorizontalAlignment.Center;
            this.txtPin.TextChanged += new EventHandler(this.TxtPin_TextChanged);
            this.txtPin.KeyPress += new KeyPressEventHandler(this.TxtPin_KeyPress);
            
            // lblError
            this.lblError.AutoSize = false;
            this.lblError.Font = new Font("Segoe UI", 9F);
            this.lblError.ForeColor = Color.FromArgb(220, 38, 38);
            this.lblError.Location = new Point(50, 360);
            this.lblError.Name = "lblError";
            this.lblError.Size = new Size(350, 40);
            this.lblError.TabIndex = 5;
            this.lblError.Text = "";
            this.lblError.TextAlign = ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            
            // btnUnlock
            this.btnUnlock.BackColor = Color.FromArgb(8, 145, 178);
            this.btnUnlock.FlatAppearance.BorderSize = 0;
            this.btnUnlock.FlatStyle = FlatStyle.Flat;
            this.btnUnlock.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnUnlock.ForeColor = Color.White;
            this.btnUnlock.Location = new Point(125, 410);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new Size(200, 45);
            this.btnUnlock.TabIndex = 6;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = false;
            this.btnUnlock.Cursor = Cursors.Hand;
            this.btnUnlock.Click += new EventHandler(this.BtnUnlock_Click);
            
            // lnkForgotPin
            this.lnkForgotPin.AutoSize = false;
            this.lnkForgotPin.Font = new Font("Segoe UI", 9F);
            this.lnkForgotPin.LinkColor = Color.FromArgb(8, 145, 178);
            this.lnkForgotPin.Location = new Point(0, 470);
            this.lnkForgotPin.Name = "lnkForgotPin";
            this.lnkForgotPin.Size = new Size(450, 20);
            this.lnkForgotPin.TabIndex = 7;
            this.lnkForgotPin.TabStop = true;
            this.lnkForgotPin.Text = "Forgot PIN? Use Master Key";
            this.lnkForgotPin.TextAlign = ContentAlignment.MiddleCenter;
            this.lnkForgotPin.LinkClicked += new LinkLabelLinkClickedEventHandler(this.LnkForgotPin_LinkClicked);
            
            // btnExit
            this.btnExit.BackColor = Color.FromArgb(229, 231, 235);
            this.btnExit.FlatAppearance.BorderSize = 1;
            this.btnExit.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
            this.btnExit.FlatStyle = FlatStyle.Flat;
            this.btnExit.Font = new Font("Segoe UI", 10F);
            this.btnExit.ForeColor = Color.FromArgb(55, 65, 81);
            this.btnExit.Location = new Point(125, 500);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(200, 38);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Exit Application";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Cursor = Cursors.Hand;
            this.btnExit.Click += new EventHandler(this.BtnExit_Click);
            
            // Add controls
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lnkForgotPin);
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtPin);
            this.Controls.Add(this.lblInstruction);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(lblLockIcon);
            this.Controls.Add(this.lblAppName);
            
            this.ResumeLayout(false);
        }

        private Label lblTitle;
        private Label lblInstruction;
        private TextBox txtPin;
        private Button btnUnlock;
        private LinkLabel lnkForgotPin;
        private Label lblError;
        private Label lblAppName;
        private Button btnExit;
    }
}
