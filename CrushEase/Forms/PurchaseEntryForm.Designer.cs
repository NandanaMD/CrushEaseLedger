namespace CrushEase.Forms
{
    partial class PurchaseEntryForm
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
            this.components = new System.ComponentModel.Container();
            this.lblPurchaseDate = new Label();
            this.dtpPurchaseDate = new DateTimePicker();
            this.lblVehicle = new Label();
            this.cmbVehicle = new ComboBox();
            this.lblVendor = new Label();
            this.cmbVendor = new ComboBox();
            this.btnAddVendor = new Button();
            this.lblMaterial = new Label();
            this.cmbMaterial = new ComboBox();
            this.lblVendorSite = new Label();
            this.txtVendorSite = new TextBox();
            this.lblUnit = new Label();
            this.cmbUnit = new ComboBox();
            this.contextMenuEntry = new ContextMenuStrip(this.components);
            this.menuItemPrintReceipt = new ToolStripMenuItem();
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
            this.btnPrintReceipt = new Button();
            this.btnClose = new Button();
            this.groupBox = new GroupBox();
            
            this.groupBox.SuspendLayout();
            this.contextMenuEntry.SuspendLayout();
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
            this.groupBox.Controls.Add(this.txtVendorSite);
            this.groupBox.Controls.Add(this.lblVendorSite);
            this.groupBox.Controls.Add(this.cmbMaterial);
            this.groupBox.Controls.Add(this.lblMaterial);
            this.groupBox.Controls.Add(this.btnAddVendor);
            this.groupBox.Controls.Add(this.cmbVendor);
            this.groupBox.Controls.Add(this.lblVendor);
            this.groupBox.Controls.Add(this.cmbVehicle);
            this.groupBox.Controls.Add(this.lblVehicle);
            this.groupBox.Controls.Add(this.dtpPurchaseDate);
            this.groupBox.Controls.Add(this.lblPurchaseDate);
            this.groupBox.Location = new Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new Size(560, 365);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Purchase Details";
            
            // lblPurchaseDate
            this.lblPurchaseDate.AutoSize = true;
            this.lblPurchaseDate.Location = new Point(20, 35);
            this.lblPurchaseDate.Name = "lblPurchaseDate";
            this.lblPurchaseDate.Size = new Size(88, 15);
            this.lblPurchaseDate.TabIndex = 0;
            this.lblPurchaseDate.Text = "Purchase Date:";
            
            // dtpPurchaseDate
            this.dtpPurchaseDate.Format = DateTimePickerFormat.Short;
            this.dtpPurchaseDate.Location = new Point(130, 32);
            this.dtpPurchaseDate.Name = "dtpPurchaseDate";
            this.dtpPurchaseDate.Size = new Size(200, 23);
            this.dtpPurchaseDate.TabIndex = 1;
            
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
            
            // lblVendor
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new Point(20, 105);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new Size(50, 15);
            this.lblVendor.TabIndex = 4;
            this.lblVendor.Text = "Vendor:";
            
            // cmbVendor
            this.cmbVendor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbVendor.FormattingEnabled = true;
            this.cmbVendor.Location = new Point(130, 102);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new Size(200, 23);
            this.cmbVendor.TabIndex = 5;
            
            // btnAddVendor
            this.btnAddVendor.Location = new Point(340, 101);
            this.btnAddVendor.Name = "btnAddVendor";
            this.btnAddVendor.Size = new Size(80, 25);
            this.btnAddVendor.TabIndex = 6;
            this.btnAddVendor.Text = "Add New";
            this.btnAddVendor.UseVisualStyleBackColor = true;
            this.btnAddVendor.Click += new EventHandler(this.BtnAddVendor_Click);
            
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
            
            // lblVendorSite
            this.lblVendorSite.AutoSize = true;
            this.lblVendorSite.Location = new Point(20, 210);
            this.lblVendorSite.Name = "lblVendorSite";
            this.lblVendorSite.Size = new Size(73, 15);
            this.lblVendorSite.TabIndex = 11;
            this.lblVendorSite.Text = "Vendor Site:";
            
            // txtVendorSite
            this.txtVendorSite.Location = new Point(130, 207);
            this.txtVendorSite.Name = "txtVendorSite";
            this.txtVendorSite.Size = new Size(290, 23);
            this.txtVendorSite.TabIndex = 12;
            
            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new Point(20, 245);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new Size(85, 15);
            this.lblQuantity.TabIndex = 13;
            this.lblQuantity.Text = "Quantity (CFT):";
            
            // txtQuantity
            this.txtQuantity.Location = new Point(130, 242);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new Size(150, 23);
            this.txtQuantity.TabIndex = 14;
            this.txtQuantity.TextAlign = HorizontalAlignment.Right;
            this.txtQuantity.TextChanged += new EventHandler(this.TxtQuantity_TextChanged);
            
            // lblCalculatedCFT
            this.lblCalculatedCFT.AutoSize = true;
            this.lblCalculatedCFT.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.lblCalculatedCFT.Location = new Point(295, 245);
            this.lblCalculatedCFT.Name = "lblCalculatedCFT";
            this.lblCalculatedCFT.Size = new Size(65, 15);
            this.lblCalculatedCFT.TabIndex = 15;
            this.lblCalculatedCFT.Text = "= CFT:";
            this.lblCalculatedCFT.Visible = false;
            
            // txtCalculatedCFT
            this.txtCalculatedCFT.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.txtCalculatedCFT.Location = new Point(370, 242);
            this.txtCalculatedCFT.Name = "txtCalculatedCFT";
            this.txtCalculatedCFT.ReadOnly = true;
            this.txtCalculatedCFT.Size = new Size(120, 23);
            this.txtCalculatedCFT.TabIndex = 16;
            this.txtCalculatedCFT.TabStop = false;
            this.txtCalculatedCFT.Text = "0.00";
            this.txtCalculatedCFT.TextAlign = HorizontalAlignment.Right;
            this.txtCalculatedCFT.Visible = false;
            
            // lblRate
            this.lblRate.AutoSize = true;
            this.lblRate.Location = new Point(20, 280);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new Size(33, 15);
            this.lblRate.TabIndex = 17;
            this.lblRate.Text = "Rate:";
            
            // txtRate
            this.txtRate.Location = new Point(130, 277);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new Size(150, 23);
            this.txtRate.TabIndex = 18;
            this.txtRate.TextAlign = HorizontalAlignment.Right;
            this.txtRate.TextChanged += new EventHandler(this.TxtRate_TextChanged);
            
            // lblAmount
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblAmount.Location = new Point(20, 315);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Size(57, 15);
            this.lblAmount.TabIndex = 19;
            this.lblAmount.Text = "Amount:";
            
            // txtAmount
            this.txtAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.txtAmount.Location = new Point(130, 312);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.Size = new Size(150, 23);
            this.txtAmount.TabIndex = 20;
            this.txtAmount.TabStop = false;
            this.txtAmount.Text = "0.00";
            this.txtAmount.TextAlign = HorizontalAlignment.Right;
            
            // btnSave
            this.btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSave.Location = new Point(252, 390);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(100, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);
            
            // btnSaveAndNew
            this.btnSaveAndNew.Location = new Point(362, 390);
            this.btnSaveAndNew.Name = "btnSaveAndNew";
            this.btnSaveAndNew.Size = new Size(110, 35);
            this.btnSaveAndNew.TabIndex = 2;
            this.btnSaveAndNew.Text = "Save && New";
            this.btnSaveAndNew.UseVisualStyleBackColor = true;
            this.btnSaveAndNew.Click += new EventHandler(this.BtnSaveAndNew_Click);
            
            // btnPrintReceipt
            this.btnPrintReceipt.Location = new Point(12, 390);
            this.btnPrintReceipt.Name = "btnPrintReceipt";
            this.btnPrintReceipt.Size = new Size(110, 35);
            this.btnPrintReceipt.TabIndex = 4;
            this.btnPrintReceipt.Text = "Print Receipt";
            this.btnPrintReceipt.UseVisualStyleBackColor = true;
            this.btnPrintReceipt.Enabled = false;
            this.btnPrintReceipt.Click += new EventHandler(this.BtnPrintReceipt_Click);
            
            // contextMenuEntry
            this.contextMenuEntry.Items.AddRange(new ToolStripItem[] {
                this.menuItemPrintReceipt
            });
            this.contextMenuEntry.Name = "contextMenuEntry";
            this.contextMenuEntry.Size = new Size(181, 26);
            this.contextMenuEntry.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuEntry_Opening);
            
            // menuItemPrintReceipt
            this.menuItemPrintReceipt.Name = "menuItemPrintReceipt";
            this.menuItemPrintReceipt.Size = new Size(180, 22);
            this.menuItemPrintReceipt.Text = "Print Receipt";
            this.menuItemPrintReceipt.Click += new EventHandler(this.BtnPrintReceipt_Click);
            
            // btnClose
            this.btnClose.Location = new Point(482, 390);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(90, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            
            // PurchaseEntryForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(584, 437);
            this.ContextMenuStrip = this.contextMenuEntry;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrintReceipt);
            this.Controls.Add(this.btnSaveAndNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PurchaseEntryForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Purchase Entry";
            this.Load += new EventHandler(this.PurchaseEntryForm_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.contextMenuEntry.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Label lblPurchaseDate;
        private DateTimePicker dtpPurchaseDate;
        private Label lblVehicle;
        private ComboBox cmbVehicle;
        private Label lblVendor;
        private ComboBox cmbVendor;
        private Button btnAddVendor;
        private Label lblMaterial;
        private ComboBox cmbMaterial;
        private Label lblVendorSite;
        private TextBox txtVendorSite;
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
        private Button btnPrintReceipt;
        private Button btnClose;
        private GroupBox groupBox;
        private ContextMenuStrip contextMenuEntry;
        private ToolStripMenuItem menuItemPrintReceipt;
    }
}
