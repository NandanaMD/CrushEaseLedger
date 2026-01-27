namespace CrushEase.Forms
{
    partial class SaleEntryForm
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
            this.lblSaleDate = new Label();
            this.dtpSaleDate = new DateTimePicker();
            this.lblVehicle = new Label();
            this.cmbVehicle = new ComboBox();
            this.lblBuyer = new Label();
            this.cmbBuyer = new ComboBox();
            this.btnAddBuyer = new Button();
            this.lblMaterial = new Label();
            this.cmbMaterial = new ComboBox();
            this.lblUnit = new Label();
            this.cmbUnit = new ComboBox();
            this.lblQuantity = new Label();
            this.txtQuantity = new TextBox();
            this.lblCalculatedCFT = new Label();
            this.txtCalculatedCFT = new TextBox();
            this.lblRate = new Label();
            this.txtRate = new TextBox();
            this.lblAmount = new Label();
            this.txtAmount = new TextBox();
            this.btnSave = new Button();
            this.btnSaveAndNew = new Button();
            this.btnClose = new Button();
            this.groupBox = new GroupBox();
            
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            
            // groupBox
            this.groupBox.Controls.Add(this.txtAmount);
            this.groupBox.Controls.Add(this.lblAmount);
            this.groupBox.Controls.Add(this.txtRate);
            this.groupBox.Controls.Add(this.lblRate);
            this.groupBox.Controls.Add(this.txtCalculatedCFT);
            this.groupBox.Controls.Add(this.lblCalculatedCFT);
            this.groupBox.Controls.Add(this.txtQuantity);
            this.groupBox.Controls.Add(this.lblQuantity);
            this.groupBox.Controls.Add(this.cmbUnit);
            this.groupBox.Controls.Add(this.lblUnit);
            this.groupBox.Controls.Add(this.cmbMaterial);
            this.groupBox.Controls.Add(this.lblMaterial);
            this.groupBox.Controls.Add(this.btnAddBuyer);
            this.groupBox.Controls.Add(this.cmbBuyer);
            this.groupBox.Controls.Add(this.lblBuyer);
            this.groupBox.Controls.Add(this.cmbVehicle);
            this.groupBox.Controls.Add(this.lblVehicle);
            this.groupBox.Controls.Add(this.dtpSaleDate);
            this.groupBox.Controls.Add(this.lblSaleDate);
            this.groupBox.Location = new Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new Size(560, 340);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Sale Details";
            
            // lblSaleDate
            this.lblSaleDate.AutoSize = true;
            this.lblSaleDate.Location = new Point(20, 35);
            this.lblSaleDate.Name = "lblSaleDate";
            this.lblSaleDate.Size = new Size(60, 15);
            this.lblSaleDate.TabIndex = 0;
            this.lblSaleDate.Text = "Sale Date:";
            
            // dtpSaleDate
            this.dtpSaleDate.Format = DateTimePickerFormat.Short;
            this.dtpSaleDate.Location = new Point(130, 32);
            this.dtpSaleDate.Name = "dtpSaleDate";
            this.dtpSaleDate.Size = new Size(200, 23);
            this.dtpSaleDate.TabIndex = 1;
            
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
            this.cmbVehicle.Location = new Point(130, 67);
            this.cmbVehicle.Name = "cmbVehicle";
            this.cmbVehicle.Size = new Size(200, 23);
            this.cmbVehicle.TabIndex = 3;
            
            // lblBuyer
            this.lblBuyer.AutoSize = true;
            this.lblBuyer.Location = new Point(20, 105);
            this.lblBuyer.Name = "lblBuyer";
            this.lblBuyer.Size = new Size(40, 15);
            this.lblBuyer.TabIndex = 4;
            this.lblBuyer.Text = "Buyer:";
            
            // cmbBuyer
            this.cmbBuyer.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbBuyer.FormattingEnabled = true;
            this.cmbBuyer.Location = new Point(130, 102);
            this.cmbBuyer.Name = "cmbBuyer";
            this.cmbBuyer.Size = new Size(200, 23);
            this.cmbBuyer.TabIndex = 5;
            
            // btnAddBuyer
            this.btnAddBuyer.Location = new Point(340, 101);
            this.btnAddBuyer.Name = "btnAddBuyer";
            this.btnAddBuyer.Size = new Size(80, 25);
            this.btnAddBuyer.TabIndex = 6;
            this.btnAddBuyer.Text = "Add New";
            this.btnAddBuyer.UseVisualStyleBackColor = true;
            this.btnAddBuyer.Click += new EventHandler(this.BtnAddBuyer_Click);
            
            // lblMaterial
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.Location = new Point(20, 140);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new Size(55, 15);
            this.lblMaterial.TabIndex = 7;
            this.lblMaterial.Text = "Material:";
            
            // cmbMaterial
            this.cmbMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMaterial.FormattingEnabled = true;
            this.cmbMaterial.Location = new Point(130, 137);
            this.cmbMaterial.Name = "cmbMaterial";
            this.cmbMaterial.Size = new Size(200, 23);
            this.cmbMaterial.TabIndex = 8;
            this.cmbMaterial.SelectedIndexChanged += new EventHandler(this.CmbMaterial_SelectedIndexChanged);
            
            // lblUnit
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new Point(20, 175);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new Size(32, 15);
            this.lblUnit.TabIndex = 9;
            this.lblUnit.Text = "Unit:";
            
            // cmbUnit
            this.cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbUnit.FormattingEnabled = true;
            this.cmbUnit.Location = new Point(130, 172);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Size = new Size(100, 23);
            this.cmbUnit.TabIndex = 10;
            this.cmbUnit.SelectedIndexChanged += new EventHandler(this.CmbUnit_SelectedIndexChanged);
            
            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new Point(20, 210);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new Size(85, 15);
            this.lblQuantity.TabIndex = 11;
            this.lblQuantity.Text = "Quantity (CFT):";
            
            // txtQuantity
            this.txtQuantity.Location = new Point(130, 207);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new Size(150, 23);
            this.txtQuantity.TabIndex = 12;
            this.txtQuantity.TextAlign = HorizontalAlignment.Right;
            this.txtQuantity.TextChanged += new EventHandler(this.TxtQuantity_TextChanged);
            
            // lblCalculatedCFT
            this.lblCalculatedCFT.AutoSize = true;
            this.lblCalculatedCFT.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.lblCalculatedCFT.Location = new Point(295, 210);
            this.lblCalculatedCFT.Name = "lblCalculatedCFT";
            this.lblCalculatedCFT.Size = new Size(65, 15);
            this.lblCalculatedCFT.TabIndex = 13;
            this.lblCalculatedCFT.Text = "= CFT:";
            this.lblCalculatedCFT.Visible = false;
            
            // txtCalculatedCFT
            this.txtCalculatedCFT.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.txtCalculatedCFT.Location = new Point(370, 207);
            this.txtCalculatedCFT.Name = "txtCalculatedCFT";
            this.txtCalculatedCFT.ReadOnly = true;
            this.txtCalculatedCFT.Size = new Size(120, 23);
            this.txtCalculatedCFT.TabIndex = 14;
            this.txtCalculatedCFT.TabStop = false;
            this.txtCalculatedCFT.Text = "0.00";
            this.txtCalculatedCFT.TextAlign = HorizontalAlignment.Right;
            this.txtCalculatedCFT.Visible = false;
            
            // lblRate
            this.lblRate.AutoSize = true;
            this.lblRate.Location = new Point(20, 245);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new Size(33, 15);
            this.lblRate.TabIndex = 15;
            this.lblRate.Text = "Rate:";
            
            // txtRate
            this.txtRate.Location = new Point(130, 242);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new Size(150, 23);
            this.txtRate.TabIndex = 16;
            this.txtRate.TextAlign = HorizontalAlignment.Right;
            this.txtRate.TextChanged += new EventHandler(this.TxtRate_TextChanged);
            
            // lblAmount
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblAmount.Location = new Point(20, 280);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Size(57, 15);
            this.lblAmount.TabIndex = 17;
            this.lblAmount.Text = "Amount:";
            
            // txtAmount
            this.txtAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.txtAmount.Location = new Point(130, 277);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.Size = new Size(150, 23);
            this.txtAmount.TabIndex = 18;
            this.txtAmount.TabStop = false;
            this.txtAmount.Text = "0.00";
            this.txtAmount.TextAlign = HorizontalAlignment.Right;
            
            // btnSave
            this.btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSave.Location = new Point(252, 365);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(100, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);
            
            // btnSaveAndNew
            this.btnSaveAndNew.Location = new Point(362, 365);
            this.btnSaveAndNew.Name = "btnSaveAndNew";
            this.btnSaveAndNew.Size = new Size(110, 35);
            this.btnSaveAndNew.TabIndex = 2;
            this.btnSaveAndNew.Text = "Save && Add New";
            this.btnSaveAndNew.UseVisualStyleBackColor = true;
            this.btnSaveAndNew.Click += new EventHandler(this.BtnSaveAndNew_Click);
            
            // btnClose
            this.btnClose.Location = new Point(482, 365);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(90, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            
            // SaleEntryForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(584, 412);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveAndNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaleEntryForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Add Sale";
            this.Load += new EventHandler(this.SaleEntryForm_Load);
            
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        private GroupBox groupBox;
        private Label lblSaleDate;
        private DateTimePicker dtpSaleDate;
        private Label lblVehicle;
        private ComboBox cmbVehicle;
        private Label lblBuyer;
        private ComboBox cmbBuyer;
        private Button btnAddBuyer;
        private Label lblMaterial;
        private ComboBox cmbMaterial;
        private Label lblUnit;
        private ComboBox cmbUnit;
        private Label lblQuantity;
        private TextBox txtQuantity;
        private Label lblCalculatedCFT;
        private TextBox txtCalculatedCFT;
        private Label lblRate;
        private TextBox txtRate;
        private Label lblAmount;
        private TextBox txtAmount;
        private Button btnSave;
        private Button btnSaveAndNew;
        private Button btnClose;
    }
}
