namespace CrushEase.Forms
{
    partial class PurchaseReportForm
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
            this.groupFilters = new GroupBox();
            this.lblFromDate = new Label();
            this.dtpFromDate = new DateTimePicker();
            this.lblToDate = new Label();
            this.dtpToDate = new DateTimePicker();
            this.lblVehicle = new Label();
            this.cmbVehicle = new ComboBox();
            this.lblMaterial = new Label();
            this.cmbMaterial = new ComboBox();
            this.lblVendor = new Label();
            this.cmbVendor = new ComboBox();
            this.btnGenerate = new Button();
            this.dgvReport = new DataGridView();
            this.lblTotal = new Label();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnExport = new Button();
            this.btnClose = new Button();
            
            this.groupFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvReport).BeginInit();
            this.SuspendLayout();
            
            // groupFilters
            this.groupFilters.Controls.Add(this.btnGenerate);
            this.groupFilters.Controls.Add(this.cmbVendor);
            this.groupFilters.Controls.Add(this.lblVendor);
            this.groupFilters.Controls.Add(this.cmbMaterial);
            this.groupFilters.Controls.Add(this.lblMaterial);
            this.groupFilters.Controls.Add(this.cmbVehicle);
            this.groupFilters.Controls.Add(this.lblVehicle);
            this.groupFilters.Controls.Add(this.dtpToDate);
            this.groupFilters.Controls.Add(this.lblToDate);
            this.groupFilters.Controls.Add(this.dtpFromDate);
            this.groupFilters.Controls.Add(this.lblFromDate);
            this.groupFilters.Location = new Point(12, 12);
            this.groupFilters.Name = "groupFilters";
            this.groupFilters.Size = new Size(976, 100);
            this.groupFilters.TabIndex = 0;
            this.groupFilters.TabStop = false;
            this.groupFilters.Text = "Filters";
            
            // lblFromDate
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new Point(20, 35);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new Size(67, 15);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From Date:";
            
            // dtpFromDate
            this.dtpFromDate.Format = DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new Point(100, 32);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new Size(150, 23);
            this.dtpFromDate.TabIndex = 1;
            
            // lblToDate
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new Point(270, 35);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new Size(53, 15);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To Date:";
            
            // dtpToDate
            this.dtpToDate.Format = DateTimePickerFormat.Short;
            this.dtpToDate.Location = new Point(340, 32);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new Size(150, 23);
            this.dtpToDate.TabIndex = 3;
            
            // lblVehicle
            this.lblVehicle.AutoSize = true;
            this.lblVehicle.Location = new Point(520, 35);
            this.lblVehicle.Name = "lblVehicle";
            this.lblVehicle.Size = new Size(50, 15);
            this.lblVehicle.TabIndex = 4;
            this.lblVehicle.Text = "Vehicle:";
            
            // cmbVehicle
            this.cmbVehicle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbVehicle.FormattingEnabled = true;
            this.cmbVehicle.Location = new Point(580, 32);
            this.cmbVehicle.Name = "cmbVehicle";
            this.cmbVehicle.Size = new Size(180, 23);
            this.cmbVehicle.TabIndex = 5;
            
            // lblMaterial
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.Location = new Point(20, 70);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new Size(55, 15);
            this.lblMaterial.TabIndex = 6;
            this.lblMaterial.Text = "Material:";
            
            // cmbMaterial
            this.cmbMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMaterial.FormattingEnabled = true;
            this.cmbMaterial.Location = new Point(100, 67);
            this.cmbMaterial.Name = "cmbMaterial";
            this.cmbMaterial.Size = new Size(180, 23);
            this.cmbMaterial.TabIndex = 7;
            
            // lblVendor
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new Point(300, 70);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new Size(49, 15);
            this.lblVendor.TabIndex = 8;
            this.lblVendor.Text = "Vendor:";
            
            // cmbVendor
            this.cmbVendor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbVendor.FormattingEnabled = true;
            this.cmbVendor.Location = new Point(360, 67);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new Size(200, 23);
            this.cmbVendor.TabIndex = 9;
            
            // btnGenerate
            this.btnGenerate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnGenerate.Location = new Point(820, 32);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new Size(140, 58);
            this.btnGenerate.TabIndex = 10;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new EventHandler(this.BtnGenerate_Click);
            
            // dgvReport
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Location = new Point(12, 120);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new Size(976, 400);
            this.dgvReport.TabIndex = 1;
            
            // lblTotal
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTotal.Location = new Point(12, 535);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new Size(100, 19);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Total: ₹0.00";
            
            // btnEdit
            this.btnEdit.Font = new Font("Segoe UI", 9F);
            this.btnEdit.Location = new Point(358, 530);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(150, 35);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit Selected";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new EventHandler(this.BtnEdit_Click);
            
            // btnDelete
            this.btnDelete.Font = new Font("Segoe UI", 9F);
            this.btnDelete.Location = new Point(518, 530);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(150, 35);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete Selected";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.BtnDelete_Click);
            
            // btnExport
            this.btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExport.Location = new Point(678, 530);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(150, 35);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new EventHandler(this.BtnExport_Click);
            
            // btnClose
            this.btnClose.Location = new Point(838, 530);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(150, 35);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            
            // PurchaseReportForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 577);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.groupFilters);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PurchaseReportForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Purchase Report";
            this.Load += new EventHandler(this.PurchaseReportForm_Load);
            this.groupFilters.ResumeLayout(false);
            this.groupFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvReport).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private GroupBox groupFilters;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Label lblVehicle;
        private ComboBox cmbVehicle;
        private Label lblMaterial;
        private ComboBox cmbMaterial;
        private Label lblVendor;
        private ComboBox cmbVendor;
        private Button btnGenerate;
        private DataGridView dgvReport;
        private Label lblTotal;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnExport;
        private Button btnClose;
    }
}
