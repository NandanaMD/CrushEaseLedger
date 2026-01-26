namespace CrushEase.Forms
{
    partial class MasterKeyForm
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
            this.txtMasterKey = new TextBox();
            this.btnUnlock = new Button();
            this.btnCancel = new Button();
            this.lblError = new Label();
            this.SuspendLayout();
            
            // Form
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 250);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblInstruction);
            this.Controls.Add(this.txtMasterKey);
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblError);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MasterKeyForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Master Key Recovery";
            this.Load += new EventHandler(this.MasterKeyForm_Load);
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.Location = new Point(100, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(200, 25);
            this.lblTitle.Text = "Enter Master Key";
            
            // lblInstruction
            this.lblInstruction.Location = new Point(50, 70);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new Size(300, 40);
            this.lblInstruction.Text = "Enter the master key to unlock the application.\nContact your administrator if you don't have it.";
            this.lblInstruction.TextAlign = ContentAlignment.MiddleCenter;
            
            // txtMasterKey
            this.txtMasterKey.CharacterCasing = CharacterCasing.Upper;
            this.txtMasterKey.Font = new Font("Segoe UI", 12F);
            this.txtMasterKey.Location = new Point(100, 120);
            this.txtMasterKey.MaxLength = 20;
            this.txtMasterKey.Name = "txtMasterKey";
            this.txtMasterKey.Size = new Size(200, 29);
            this.txtMasterKey.TabIndex = 0;
            this.txtMasterKey.TextAlign = HorizontalAlignment.Center;
            this.txtMasterKey.KeyPress += new KeyPressEventHandler(this.TxtMasterKey_KeyPress);
            
            // lblError
            this.lblError.ForeColor = Color.Red;
            this.lblError.Location = new Point(50, 155);
            this.lblError.Name = "lblError";
            this.lblError.Size = new Size(300, 20);
            this.lblError.TextAlign = ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            
            // btnUnlock
            this.btnUnlock.BackColor = Color.FromArgb(0, 120, 215);
            this.btnUnlock.FlatStyle = FlatStyle.Flat;
            this.btnUnlock.ForeColor = Color.White;
            this.btnUnlock.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnUnlock.Location = new Point(100, 185);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new Size(90, 35);
            this.btnUnlock.TabIndex = 1;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = false;
            this.btnUnlock.Click += new EventHandler(this.BtnUnlock_Click);
            
            // btnCancel
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F);
            this.btnCancel.Location = new Point(210, 185);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(90, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitle;
        private Label lblInstruction;
        private TextBox txtMasterKey;
        private Button btnUnlock;
        private Button btnCancel;
        private Label lblError;
    }
}
