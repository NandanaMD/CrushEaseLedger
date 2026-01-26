namespace CrushEase.Forms
{
    partial class MaintenanceEntryForm
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
            this.lblMaintenanceDate = new Label();
            this.dtpMaintenanceDate = new DateTimePicker();
            this.lblVehicle = new Label();
            this.cmbVehicle = new ComboBox();
            this.lblDescription = new Label();
            this.txtDescription = new TextBox();
            this.lblAmount = new Label();
            this.txtAmount = new TextBox();
            this.btnSave = new Button();
            this.btnClose = new Button();
            this.groupBox = new GroupBox();
            
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            
            // groupBox
            this.groupBox.Controls.Add(this.txtAmount);
            this.groupBox.Controls.Add(this.lblAmount);
            this.groupBox.Controls.Add(this.txtDescription);
            this.groupBox.Controls.Add(this.lblDescription);
            this.groupBox.Controls.Add(this.cmbVehicle);
            this.groupBox.Controls.Add(this.lblVehicle);
            this.groupBox.Controls.Add(this.dtpMaintenanceDate);
            this.groupBox.Controls.Add(this.lblMaintenanceDate);
            this.groupBox.Location = new Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new Size(560, 250);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Maintenance Details";
            
            // lblMaintenanceDate
            this.lblMaintenanceDate.AutoSize = true;
            this.lblMaintenanceDate.Location = new Point(20, 35);
            this.lblMaintenanceDate.Name = "lblMaintenanceDate";
            this.lblMaintenanceDate.Size = new Size(110, 15);
            this.lblMaintenanceDate.TabIndex = 0;
            this.lblMaintenanceDate.Text = "Maintenance Date:";
            
            // dtpMaintenanceDate
            this.dtpMaintenanceDate.Format = DateTimePickerFormat.Short;
            this.dtpMaintenanceDate.Location = new Point(150, 32);
            this.dtpMaintenanceDate.Name = "dtpMaintenanceDate";
            this.dtpMaintenanceDate.Size = new Size(200, 23);
            this.dtpMaintenanceDate.TabIndex = 1;
            
            // lblVehicle
            this.lblVehicle.AutoSize = true;
            this.lblVehicle.Location = new Point(20, 70);
            this.lblVehicle.Name = "lblVehicle";
            this.lblVehicle.Size = new Size(50, 15);
            this.lblVehicle.TabIndex = 2;
            this.lblVehicle.Text = "Vehicle:";
            
            // cmbVehicle
            this.cmbVehicle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbVehicle.FormattingEnabled = true;
            this.cmbVehicle.Location = new Point(150, 67);
            this.cmbVehicle.Name = "cmbVehicle";
            this.cmbVehicle.Size = new Size(200, 23);
            this.cmbVehicle.TabIndex = 3;
            
            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new Point(20, 105);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(70, 15);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            
            // txtDescription
            this.txtDescription.Location = new Point(150, 102);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(390, 80);
            this.txtDescription.TabIndex = 5;
            
            // lblAmount
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblAmount.Location = new Point(20, 200);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Size(57, 15);
            this.lblAmount.TabIndex = 6;
            this.lblAmount.Text = "Amount:";
            
            // txtAmount
            this.txtAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.txtAmount.Location = new Point(150, 197);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new Size(150, 23);
            this.txtAmount.TabIndex = 7;
            this.txtAmount.TextAlign = HorizontalAlignment.Right;
            
            // btnSave
            this.btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSave.Location = new Point(372, 275);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(100, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);
            
            // btnClose
            this.btnClose.Location = new Point(482, 275);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(90, 35);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            
            // MaintenanceEntryForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(584, 322);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaintenanceEntryForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Maintenance Entry";
            this.Load += new EventHandler(this.MaintenanceEntryForm_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        private Label lblMaintenanceDate;
        private DateTimePicker dtpMaintenanceDate;
        private Label lblVehicle;
        private ComboBox cmbVehicle;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblAmount;
        private TextBox txtAmount;
        private Button btnSave;
        private Button btnClose;
        private GroupBox groupBox;
    }
}
