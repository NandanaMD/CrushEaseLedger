namespace CrushEase.Forms
{
    partial class TransactionViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl;
        private TabPage tabSales;
        private TabPage tabPurchases;
        private TabPage tabMaintenance;
        
        // Sales tab controls
        private DataGridView dgvSales;
        private DateTimePicker dtpSalesFrom;
        private DateTimePicker dtpSalesTo;
        private ComboBox cmbSalesVehicle;
        private Button btnApplySalesFilter;
        private Button btnEditSale;
        private Button btnDeleteSale;
        private Label lblSalesFrom;
        private Label lblSalesTo;
        private Label lblSalesVehicle;
        private Label lblSalesCount;
        
        // Purchases tab controls
        private DataGridView dgvPurchases;
        private DateTimePicker dtpPurchasesFrom;
        private DateTimePicker dtpPurchasesTo;
        private ComboBox cmbPurchasesVehicle;
        private Button btnApplyPurchasesFilter;
        private Button btnEditPurchase;
        private Button btnDeletePurchase;
        private Label lblPurchasesFrom;
        private Label lblPurchasesTo;
        private Label lblPurchasesVehicle;
        private Label lblPurchasesCount;
        
        // Maintenance tab controls
        private DataGridView dgvMaintenance;
        private DateTimePicker dtpMaintenanceFrom;
        private DateTimePicker dtpMaintenanceTo;
        private ComboBox cmbMaintenanceVehicle;
        private Button btnApplyMaintenanceFilter;
        private Button btnEditMaintenance;
        private Button btnDeleteMaintenance;
        private Label lblMaintenanceFrom;
        private Label lblMaintenanceTo;
        private Label lblMaintenanceVehicle;
        private Label lblMaintenanceCount;
        
        // Common controls
        private Button btnRefresh;
        private Button btnClose;

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
            this.tabControl = new TabControl();
            this.tabSales = new TabPage();
            this.tabPurchases = new TabPage();
            this.tabMaintenance = new TabPage();
            
            // Sales
            this.dgvSales = new DataGridView();
            this.dtpSalesFrom = new DateTimePicker();
            this.dtpSalesTo = new DateTimePicker();
            this.cmbSalesVehicle = new ComboBox();
            this.btnApplySalesFilter = new Button();
            this.btnEditSale = new Button();
            this.btnDeleteSale = new Button();
            this.lblSalesFrom = new Label();
            this.lblSalesTo = new Label();
            this.lblSalesVehicle = new Label();
            this.lblSalesCount = new Label();
            
            // Purchases
            this.dgvPurchases = new DataGridView();
            this.dtpPurchasesFrom = new DateTimePicker();
            this.dtpPurchasesTo = new DateTimePicker();
            this.cmbPurchasesVehicle = new ComboBox();
            this.btnApplyPurchasesFilter = new Button();
            this.btnEditPurchase = new Button();
            this.btnDeletePurchase = new Button();
            this.lblPurchasesFrom = new Label();
            this.lblPurchasesTo = new Label();
            this.lblPurchasesVehicle = new Label();
            this.lblPurchasesCount = new Label();
            
            // Maintenance
            this.dgvMaintenance = new DataGridView();
            this.dtpMaintenanceFrom = new DateTimePicker();
            this.dtpMaintenanceTo = new DateTimePicker();
            this.cmbMaintenanceVehicle = new ComboBox();
            this.btnApplyMaintenanceFilter = new Button();
            this.btnEditMaintenance = new Button();
            this.btnDeleteMaintenance = new Button();
            this.lblMaintenanceFrom = new Label();
            this.lblMaintenanceTo = new Label();
            this.lblMaintenanceVehicle = new Label();
            this.lblMaintenanceCount = new Label();
            
            // Common
            this.btnRefresh = new Button();
            this.btnClose = new Button();
            
            this.tabControl.SuspendLayout();
            this.tabSales.SuspendLayout();
            this.tabPurchases.SuspendLayout();
            this.tabMaintenance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaintenance)).BeginInit();
            this.SuspendLayout();
            
            // tabControl
            this.tabControl.Controls.Add(this.tabSales);
            this.tabControl.Controls.Add(this.tabPurchases);
            this.tabControl.Controls.Add(this.tabMaintenance);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(1200, 650);
            this.tabControl.TabIndex = 0;
            
            // tabSales
            this.tabSales.Controls.Add(this.dgvSales);
            this.tabSales.Controls.Add(this.dtpSalesFrom);
            this.tabSales.Controls.Add(this.dtpSalesTo);
            this.tabSales.Controls.Add(this.cmbSalesVehicle);
            this.tabSales.Controls.Add(this.btnApplySalesFilter);
            this.tabSales.Controls.Add(this.btnEditSale);
            this.tabSales.Controls.Add(this.btnDeleteSale);
            this.tabSales.Controls.Add(this.lblSalesFrom);
            this.tabSales.Controls.Add(this.lblSalesTo);
            this.tabSales.Controls.Add(this.lblSalesVehicle);
            this.tabSales.Controls.Add(this.lblSalesCount);
            this.tabSales.Location = new Point(4, 24);
            this.tabSales.Name = "tabSales";
            this.tabSales.Padding = new Padding(10);
            this.tabSales.Size = new Size(1192, 622);
            this.tabSales.TabIndex = 0;
            this.tabSales.Text = "Sales";
            this.tabSales.UseVisualStyleBackColor = true;
            
            // lblSalesFrom
            this.lblSalesFrom.AutoSize = true;
            this.lblSalesFrom.Location = new Point(10, 15);
            this.lblSalesFrom.Name = "lblSalesFrom";
            this.lblSalesFrom.Size = new Size(38, 15);
            this.lblSalesFrom.TabIndex = 0;
            this.lblSalesFrom.Text = "From:";
            
            // dtpSalesFrom
            this.dtpSalesFrom.Format = DateTimePickerFormat.Short;
            this.dtpSalesFrom.Location = new Point(60, 12);
            this.dtpSalesFrom.Name = "dtpSalesFrom";
            this.dtpSalesFrom.Size = new Size(120, 23);
            this.dtpSalesFrom.TabIndex = 1;
            
            // lblSalesTo
            this.lblSalesTo.AutoSize = true;
            this.lblSalesTo.Location = new Point(200, 15);
            this.lblSalesTo.Name = "lblSalesTo";
            this.lblSalesTo.Size = new Size(22, 15);
            this.lblSalesTo.TabIndex = 2;
            this.lblSalesTo.Text = "To:";
            
            // dtpSalesTo
            this.dtpSalesTo.Format = DateTimePickerFormat.Short;
            this.dtpSalesTo.Location = new Point(230, 12);
            this.dtpSalesTo.Name = "dtpSalesTo";
            this.dtpSalesTo.Size = new Size(120, 23);
            this.dtpSalesTo.TabIndex = 3;
            
            // lblSalesVehicle
            this.lblSalesVehicle.AutoSize = true;
            this.lblSalesVehicle.Location = new Point(370, 15);
            this.lblSalesVehicle.Name = "lblSalesVehicle";
            this.lblSalesVehicle.Size = new Size(47, 15);
            this.lblSalesVehicle.TabIndex = 4;
            this.lblSalesVehicle.Text = "Vehicle:";
            
            // cmbSalesVehicle
            this.cmbSalesVehicle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSalesVehicle.FormattingEnabled = true;
            this.cmbSalesVehicle.Location = new Point(430, 12);
            this.cmbSalesVehicle.Name = "cmbSalesVehicle";
            this.cmbSalesVehicle.Size = new Size(150, 23);
            this.cmbSalesVehicle.TabIndex = 5;
            
            // btnApplySalesFilter
            this.btnApplySalesFilter.Location = new Point(600, 11);
            this.btnApplySalesFilter.Name = "btnApplySalesFilter";
            this.btnApplySalesFilter.Size = new Size(90, 25);
            this.btnApplySalesFilter.TabIndex = 6;
            this.btnApplySalesFilter.Text = "Apply Filter";
            this.btnApplySalesFilter.UseVisualStyleBackColor = true;
            this.btnApplySalesFilter.Click += new EventHandler(this.BtnApplySalesFilter_Click);
            
            // lblSalesCount
            this.lblSalesCount.AutoSize = true;
            this.lblSalesCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblSalesCount.Location = new Point(710, 15);
            this.lblSalesCount.Name = "lblSalesCount";
            this.lblSalesCount.Size = new Size(50, 15);
            this.lblSalesCount.TabIndex = 7;
            this.lblSalesCount.Text = "Total: 0";
            
            // dgvSales
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            this.dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSales.Location = new Point(10, 45);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.ReadOnly = true;
            this.dgvSales.RowHeadersVisible = false;
            this.dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvSales.Size = new Size(1172, 527);
            this.dgvSales.TabIndex = 8;
            this.dgvSales.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvSales_CellDoubleClick);
            
            // btnEditSale
            this.btnEditSale.Location = new Point(10, 582);
            this.btnEditSale.Name = "btnEditSale";
            this.btnEditSale.Size = new Size(90, 30);
            this.btnEditSale.TabIndex = 9;
            this.btnEditSale.Text = "Edit";
            this.btnEditSale.UseVisualStyleBackColor = true;
            this.btnEditSale.Click += new EventHandler(this.BtnEditSale_Click);
            
            // btnDeleteSale
            this.btnDeleteSale.Location = new Point(110, 582);
            this.btnDeleteSale.Name = "btnDeleteSale";
            this.btnDeleteSale.Size = new Size(90, 30);
            this.btnDeleteSale.TabIndex = 10;
            this.btnDeleteSale.Text = "Delete";
            this.btnDeleteSale.UseVisualStyleBackColor = true;
            this.btnDeleteSale.Click += new EventHandler(this.BtnDeleteSale_Click);
            
            // tabPurchases
            this.tabPurchases.Controls.Add(this.dgvPurchases);
            this.tabPurchases.Controls.Add(this.dtpPurchasesFrom);
            this.tabPurchases.Controls.Add(this.dtpPurchasesTo);
            this.tabPurchases.Controls.Add(this.cmbPurchasesVehicle);
            this.tabPurchases.Controls.Add(this.btnApplyPurchasesFilter);
            this.tabPurchases.Controls.Add(this.btnEditPurchase);
            this.tabPurchases.Controls.Add(this.btnDeletePurchase);
            this.tabPurchases.Controls.Add(this.lblPurchasesFrom);
            this.tabPurchases.Controls.Add(this.lblPurchasesTo);
            this.tabPurchases.Controls.Add(this.lblPurchasesVehicle);
            this.tabPurchases.Controls.Add(this.lblPurchasesCount);
            this.tabPurchases.Location = new Point(4, 24);
            this.tabPurchases.Name = "tabPurchases";
            this.tabPurchases.Padding = new Padding(10);
            this.tabPurchases.Size = new Size(1192, 622);
            this.tabPurchases.TabIndex = 1;
            this.tabPurchases.Text = "Purchases";
            this.tabPurchases.UseVisualStyleBackColor = true;
            
            // lblPurchasesFrom
            this.lblPurchasesFrom.AutoSize = true;
            this.lblPurchasesFrom.Location = new Point(10, 15);
            this.lblPurchasesFrom.Name = "lblPurchasesFrom";
            this.lblPurchasesFrom.Size = new Size(38, 15);
            this.lblPurchasesFrom.TabIndex = 0;
            this.lblPurchasesFrom.Text = "From:";
            
            // dtpPurchasesFrom
            this.dtpPurchasesFrom.Format = DateTimePickerFormat.Short;
            this.dtpPurchasesFrom.Location = new Point(60, 12);
            this.dtpPurchasesFrom.Name = "dtpPurchasesFrom";
            this.dtpPurchasesFrom.Size = new Size(120, 23);
            this.dtpPurchasesFrom.TabIndex = 1;
            
            // lblPurchasesTo
            this.lblPurchasesTo.AutoSize = true;
            this.lblPurchasesTo.Location = new Point(200, 15);
            this.lblPurchasesTo.Name = "lblPurchasesTo";
            this.lblPurchasesTo.Size = new Size(22, 15);
            this.lblPurchasesTo.TabIndex = 2;
            this.lblPurchasesTo.Text = "To:";
            
            // dtpPurchasesTo
            this.dtpPurchasesTo.Format = DateTimePickerFormat.Short;
            this.dtpPurchasesTo.Location = new Point(230, 12);
            this.dtpPurchasesTo.Name = "dtpPurchasesTo";
            this.dtpPurchasesTo.Size = new Size(120, 23);
            this.dtpPurchasesTo.TabIndex = 3;
            
            // lblPurchasesVehicle
            this.lblPurchasesVehicle.AutoSize = true;
            this.lblPurchasesVehicle.Location = new Point(370, 15);
            this.lblPurchasesVehicle.Name = "lblPurchasesVehicle";
            this.lblPurchasesVehicle.Size = new Size(47, 15);
            this.lblPurchasesVehicle.TabIndex = 4;
            this.lblPurchasesVehicle.Text = "Vehicle:";
            
            // cmbPurchasesVehicle
            this.cmbPurchasesVehicle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPurchasesVehicle.FormattingEnabled = true;
            this.cmbPurchasesVehicle.Location = new Point(430, 12);
            this.cmbPurchasesVehicle.Name = "cmbPurchasesVehicle";
            this.cmbPurchasesVehicle.Size = new Size(150, 23);
            this.cmbPurchasesVehicle.TabIndex = 5;
            
            // btnApplyPurchasesFilter
            this.btnApplyPurchasesFilter.Location = new Point(600, 11);
            this.btnApplyPurchasesFilter.Name = "btnApplyPurchasesFilter";
            this.btnApplyPurchasesFilter.Size = new Size(90, 25);
            this.btnApplyPurchasesFilter.TabIndex = 6;
            this.btnApplyPurchasesFilter.Text = "Apply Filter";
            this.btnApplyPurchasesFilter.UseVisualStyleBackColor = true;
            this.btnApplyPurchasesFilter.Click += new EventHandler(this.BtnApplyPurchasesFilter_Click);
            
            // lblPurchasesCount
            this.lblPurchasesCount.AutoSize = true;
            this.lblPurchasesCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblPurchasesCount.Location = new Point(710, 15);
            this.lblPurchasesCount.Name = "lblPurchasesCount";
            this.lblPurchasesCount.Size = new Size(50, 15);
            this.lblPurchasesCount.TabIndex = 7;
            this.lblPurchasesCount.Text = "Total: 0";
            
            // dgvPurchases
            this.dgvPurchases.AllowUserToAddRows = false;
            this.dgvPurchases.AllowUserToDeleteRows = false;
            this.dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPurchases.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchases.Location = new Point(10, 45);
            this.dgvPurchases.Name = "dgvPurchases";
            this.dgvPurchases.ReadOnly = true;
            this.dgvPurchases.RowHeadersVisible = false;
            this.dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPurchases.Size = new Size(1172, 527);
            this.dgvPurchases.TabIndex = 8;
            this.dgvPurchases.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvPurchases_CellDoubleClick);
            
            // btnEditPurchase
            this.btnEditPurchase.Location = new Point(10, 582);
            this.btnEditPurchase.Name = "btnEditPurchase";
            this.btnEditPurchase.Size = new Size(90, 30);
            this.btnEditPurchase.TabIndex = 9;
            this.btnEditPurchase.Text = "Edit";
            this.btnEditPurchase.UseVisualStyleBackColor = true;
            this.btnEditPurchase.Click += new EventHandler(this.BtnEditPurchase_Click);
            
            // btnDeletePurchase
            this.btnDeletePurchase.Location = new Point(110, 582);
            this.btnDeletePurchase.Name = "btnDeletePurchase";
            this.btnDeletePurchase.Size = new Size(90, 30);
            this.btnDeletePurchase.TabIndex = 10;
            this.btnDeletePurchase.Text = "Delete";
            this.btnDeletePurchase.UseVisualStyleBackColor = true;
            this.btnDeletePurchase.Click += new EventHandler(this.BtnDeletePurchase_Click);
            
            // tabMaintenance
            this.tabMaintenance.Controls.Add(this.dgvMaintenance);
            this.tabMaintenance.Controls.Add(this.dtpMaintenanceFrom);
            this.tabMaintenance.Controls.Add(this.dtpMaintenanceTo);
            this.tabMaintenance.Controls.Add(this.cmbMaintenanceVehicle);
            this.tabMaintenance.Controls.Add(this.btnApplyMaintenanceFilter);
            this.tabMaintenance.Controls.Add(this.btnEditMaintenance);
            this.tabMaintenance.Controls.Add(this.btnDeleteMaintenance);
            this.tabMaintenance.Controls.Add(this.lblMaintenanceFrom);
            this.tabMaintenance.Controls.Add(this.lblMaintenanceTo);
            this.tabMaintenance.Controls.Add(this.lblMaintenanceVehicle);
            this.tabMaintenance.Controls.Add(this.lblMaintenanceCount);
            this.tabMaintenance.Location = new Point(4, 24);
            this.tabMaintenance.Name = "tabMaintenance";
            this.tabMaintenance.Padding = new Padding(10);
            this.tabMaintenance.Size = new Size(1192, 622);
            this.tabMaintenance.TabIndex = 2;
            this.tabMaintenance.Text = "Maintenance";
            this.tabMaintenance.UseVisualStyleBackColor = true;
            
            // lblMaintenanceFrom
            this.lblMaintenanceFrom.AutoSize = true;
            this.lblMaintenanceFrom.Location = new Point(10, 15);
            this.lblMaintenanceFrom.Name = "lblMaintenanceFrom";
            this.lblMaintenanceFrom.Size = new Size(38, 15);
            this.lblMaintenanceFrom.TabIndex = 0;
            this.lblMaintenanceFrom.Text = "From:";
            
            // dtpMaintenanceFrom
            this.dtpMaintenanceFrom.Format = DateTimePickerFormat.Short;
            this.dtpMaintenanceFrom.Location = new Point(60, 12);
            this.dtpMaintenanceFrom.Name = "dtpMaintenanceFrom";
            this.dtpMaintenanceFrom.Size = new Size(120, 23);
            this.dtpMaintenanceFrom.TabIndex = 1;
            
            // lblMaintenanceTo
            this.lblMaintenanceTo.AutoSize = true;
            this.lblMaintenanceTo.Location = new Point(200, 15);
            this.lblMaintenanceTo.Name = "lblMaintenanceTo";
            this.lblMaintenanceTo.Size = new Size(22, 15);
            this.lblMaintenanceTo.TabIndex = 2;
            this.lblMaintenanceTo.Text = "To:";
            
            // dtpMaintenanceTo
            this.dtpMaintenanceTo.Format = DateTimePickerFormat.Short;
            this.dtpMaintenanceTo.Location = new Point(230, 12);
            this.dtpMaintenanceTo.Name = "dtpMaintenanceTo";
            this.dtpMaintenanceTo.Size = new Size(120, 23);
            this.dtpMaintenanceTo.TabIndex = 3;
            
            // lblMaintenanceVehicle
            this.lblMaintenanceVehicle.AutoSize = true;
            this.lblMaintenanceVehicle.Location = new Point(370, 15);
            this.lblMaintenanceVehicle.Name = "lblMaintenanceVehicle";
            this.lblMaintenanceVehicle.Size = new Size(47, 15);
            this.lblMaintenanceVehicle.TabIndex = 4;
            this.lblMaintenanceVehicle.Text = "Vehicle:";
            
            // cmbMaintenanceVehicle
            this.cmbMaintenanceVehicle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMaintenanceVehicle.FormattingEnabled = true;
            this.cmbMaintenanceVehicle.Location = new Point(430, 12);
            this.cmbMaintenanceVehicle.Name = "cmbMaintenanceVehicle";
            this.cmbMaintenanceVehicle.Size = new Size(150, 23);
            this.cmbMaintenanceVehicle.TabIndex = 5;
            
            // btnApplyMaintenanceFilter
            this.btnApplyMaintenanceFilter.Location = new Point(600, 11);
            this.btnApplyMaintenanceFilter.Name = "btnApplyMaintenanceFilter";
            this.btnApplyMaintenanceFilter.Size = new Size(90, 25);
            this.btnApplyMaintenanceFilter.TabIndex = 6;
            this.btnApplyMaintenanceFilter.Text = "Apply Filter";
            this.btnApplyMaintenanceFilter.UseVisualStyleBackColor = true;
            this.btnApplyMaintenanceFilter.Click += new EventHandler(this.BtnApplyMaintenanceFilter_Click);
            
            // lblMaintenanceCount
            this.lblMaintenanceCount.AutoSize = true;
            this.lblMaintenanceCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblMaintenanceCount.Location = new Point(710, 15);
            this.lblMaintenanceCount.Name = "lblMaintenanceCount";
            this.lblMaintenanceCount.Size = new Size(50, 15);
            this.lblMaintenanceCount.TabIndex = 7;
            this.lblMaintenanceCount.Text = "Total: 0";
            
            // dgvMaintenance
            this.dgvMaintenance.AllowUserToAddRows = false;
            this.dgvMaintenance.AllowUserToDeleteRows = false;
            this.dgvMaintenance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMaintenance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaintenance.Location = new Point(10, 45);
            this.dgvMaintenance.Name = "dgvMaintenance";
            this.dgvMaintenance.ReadOnly = true;
            this.dgvMaintenance.RowHeadersVisible = false;
            this.dgvMaintenance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMaintenance.Size = new Size(1172, 527);
            this.dgvMaintenance.TabIndex = 8;
            this.dgvMaintenance.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvMaintenance_CellDoubleClick);
            
            // btnEditMaintenance
            this.btnEditMaintenance.Location = new Point(10, 582);
            this.btnEditMaintenance.Name = "btnEditMaintenance";
            this.btnEditMaintenance.Size = new Size(90, 30);
            this.btnEditMaintenance.TabIndex = 9;
            this.btnEditMaintenance.Text = "Edit";
            this.btnEditMaintenance.UseVisualStyleBackColor = true;
            this.btnEditMaintenance.Click += new EventHandler(this.BtnEditMaintenance_Click);
            
            // btnDeleteMaintenance
            this.btnDeleteMaintenance.Location = new Point(110, 582);
            this.btnDeleteMaintenance.Name = "btnDeleteMaintenance";
            this.btnDeleteMaintenance.Size = new Size(90, 30);
            this.btnDeleteMaintenance.TabIndex = 10;
            this.btnDeleteMaintenance.Text = "Delete";
            this.btnDeleteMaintenance.UseVisualStyleBackColor = true;
            this.btnDeleteMaintenance.Click += new EventHandler(this.BtnDeleteMaintenance_Click);
            
            // btnRefresh
            this.btnRefresh.Location = new Point(1006, 665);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(90, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(this.BtnRefresh_Click);
            
            // btnClose
            this.btnClose.Location = new Point(1104, 665);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(90, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            
            // TransactionViewerForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.tabControl);
            this.Name = "TransactionViewerForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Transaction Viewer";
            this.Load += new EventHandler(this.TransactionViewerForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabSales.ResumeLayout(false);
            this.tabSales.PerformLayout();
            this.tabPurchases.ResumeLayout(false);
            this.tabPurchases.PerformLayout();
            this.tabMaintenance.ResumeLayout(false);
            this.tabMaintenance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaintenance)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
