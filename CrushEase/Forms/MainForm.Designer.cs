namespace CrushEase.Forms
{
    partial class MainForm
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
            this.menuStrip = new MenuStrip();
            this.menuMasters = new ToolStripMenuItem();
            this.menuVehicles = new ToolStripMenuItem();
            this.menuVendors = new ToolStripMenuItem();
            this.menuBuyers = new ToolStripMenuItem();
            this.menuMaterials = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.menuViewMasterData = new ToolStripMenuItem();
            this.menuTransactions = new ToolStripMenuItem();
            this.menuAddSale = new ToolStripMenuItem();
            this.menuAddPurchase = new ToolStripMenuItem();
            this.menuAddMaintenance = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.menuViewTransactions = new ToolStripMenuItem();
            this.menuReports = new ToolStripMenuItem();
            this.menuSalesReport = new ToolStripMenuItem();
            this.menuPurchaseReport = new ToolStripMenuItem();
            this.menuMaintenanceReport = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.menuVehicleProfitReport = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.menuExportAllData = new ToolStripMenuItem();
            this.menuFile = new ToolStripMenuItem();
            this.menuBackup = new ToolStripMenuItem();
            this.menuRestore = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.menuExit = new ToolStripMenuItem();
            
            this.groupToday = new GroupBox();
            this.lblTodaySales = new Label();
            this.lblTodayPurchases = new Label();
            this.lblTodayMaintenance = new Label();
            
            this.groupMonth = new GroupBox();
            this.lblMonthSales = new Label();
            this.lblMonthPurchases = new Label();
            this.lblMonthMaintenance = new Label();
            this.lblMonthProfit = new Label();
            
            this.groupQuickActions = new GroupBox();
            this.btnAddSale = new Button();
            this.btnAddPurchase = new Button();
            this.btnAddMaintenance = new Button();
            this.btnShortcuts = new Button();
            
            this.groupRecent = new GroupBox();
            this.dgvRecent = new DataGridView();
            
            this.statusStrip = new StatusStrip();
            this.lblStatus = new ToolStripStatusLabel();
            
            this.menuStrip.SuspendLayout();
            this.groupToday.SuspendLayout();
            this.groupMonth.SuspendLayout();
            this.groupQuickActions.SuspendLayout();
            this.groupRecent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            
            // menuStrip
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
                this.menuFile,
                this.menuMasters,
                this.menuTransactions,
                this.menuReports
            });
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(1000, 24);
            this.menuStrip.TabIndex = 0;
            
            // File Menu
            this.menuFile.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuBackup,
                this.menuRestore,
                this.toolStripSeparator2,
                this.menuExit
            });
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new Size(37, 20);
            this.menuFile.Text = "&File";
            
            this.menuBackup.Name = "menuBackup";
            this.menuBackup.Size = new Size(180, 22);
            this.menuBackup.Text = "&Backup Now...";
            this.menuBackup.ShortcutKeys = Keys.Control | Keys.B;
            this.menuBackup.Click += new EventHandler(this.MenuBackup_Click);
            
            this.menuRestore.Name = "menuRestore";
            this.menuRestore.Size = new Size(180, 22);
            this.menuRestore.Text = "&Restore from Backup...";
            this.menuRestore.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
            this.menuRestore.Click += new EventHandler(this.MenuRestore_Click);
            
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(177, 6);
            
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new Size(180, 22);
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new EventHandler(this.MenuExit_Click);
            
            // Masters Menu
            this.menuMasters.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuVehicles,
                this.menuVendors,
                this.menuBuyers,
                this.menuMaterials,
                this.toolStripSeparator3,
                this.menuViewMasterData
            });
            this.menuMasters.Name = "menuMasters";
            this.menuMasters.Size = new Size(60, 20);
            this.menuMasters.Text = "&Masters";
            
            this.menuVehicles.Name = "menuVehicles";
            this.menuVehicles.Size = new Size(180, 22);
            this.menuVehicles.Text = "&Vehicles";
            this.menuVehicles.ShortcutKeys = Keys.Control | Keys.Shift | Keys.V;
            this.menuVehicles.Click += new EventHandler(this.MenuVehicles_Click);
            
            this.menuVendors.Name = "menuVendors";
            this.menuVendors.Size = new Size(180, 22);
            this.menuVendors.Text = "V&endors";
            this.menuVendors.ShortcutKeys = Keys.Control | Keys.Shift | Keys.D;
            this.menuVendors.Click += new EventHandler(this.MenuVendors_Click);
            
            this.menuBuyers.Name = "menuBuyers";
            this.menuBuyers.Size = new Size(180, 22);
            this.menuBuyers.Text = "&Buyers";
            this.menuBuyers.ShortcutKeys = Keys.Control | Keys.Shift | Keys.B;
            this.menuBuyers.Click += new EventHandler(this.MenuBuyers_Click);
            
            this.menuMaterials.Name = "menuMaterials";
            this.menuMaterials.Size = new Size(180, 22);
            this.menuMaterials.Text = "&Materials";
            this.menuMaterials.ShortcutKeys = Keys.Control | Keys.Shift | Keys.M;
            this.menuMaterials.Click += new EventHandler(this.MenuMaterials_Click);
            
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(177, 6);
            
            this.menuViewMasterData.Name = "menuViewMasterData";
            this.menuViewMasterData.Size = new Size(180, 22);
            this.menuViewMasterData.Text = "View All &Data...";
            this.menuViewMasterData.ShortcutKeys = Keys.Control | Keys.D;
            this.menuViewMasterData.Click += new EventHandler(this.MenuViewMasterData_Click);
            
            // Transactions Menu
            this.menuTransactions.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuAddSale,
                this.menuAddPurchase,
                this.menuAddMaintenance,
                this.toolStripSeparator4,
                this.menuViewTransactions
            });
            this.menuTransactions.Name = "menuTransactions";
            this.menuTransactions.Size = new Size(84, 20);
            this.menuTransactions.Text = "&Transactions";
            
            this.menuAddSale.Name = "menuAddSale";
            this.menuAddSale.Size = new Size(180, 22);
            this.menuAddSale.Text = "Add &Sale";
            this.menuAddSale.ShortcutKeys = Keys.F2;
            this.menuAddSale.Click += new EventHandler(this.MenuAddSale_Click);
            
            this.menuAddPurchase.Name = "menuAddPurchase";
            this.menuAddPurchase.Size = new Size(180, 22);
            this.menuAddPurchase.Text = "Add &Purchase";
            this.menuAddPurchase.ShortcutKeys = Keys.F3;
            this.menuAddPurchase.Click += new EventHandler(this.MenuAddPurchase_Click);
            
            this.menuAddMaintenance.Name = "menuAddMaintenance";
            this.menuAddMaintenance.Size = new Size(180, 22);
            this.menuAddMaintenance.Text = "Add &Maintenance";
            this.menuAddMaintenance.ShortcutKeys = Keys.F4;
            this.menuAddMaintenance.Click += new EventHandler(this.MenuAddMaintenance_Click);
            
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(177, 6);
            
            this.menuViewTransactions.Name = "menuViewTransactions";
            this.menuViewTransactions.Size = new Size(180, 22);
            this.menuViewTransactions.Text = "View All &Transactions...";
            this.menuViewTransactions.ShortcutKeys = Keys.Control | Keys.T;
            this.menuViewTransactions.Click += new EventHandler(this.MenuViewTransactions_Click);
            
            // Reports Menu
            this.menuReports.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuSalesReport,
                this.menuPurchaseReport,
                this.menuMaintenanceReport,
                this.toolStripSeparator1,
                this.menuVehicleProfitReport,
                this.toolStripSeparator5,
                this.menuExportAllData
            });
            this.menuReports.Name = "menuReports";
            this.menuReports.Size = new Size(59, 20);
            this.menuReports.Text = "&Reports";
            
            this.menuSalesReport.Name = "menuSalesReport";
            this.menuSalesReport.Size = new Size(200, 22);
            this.menuSalesReport.Text = "&Sales Report";
            this.menuSalesReport.ShortcutKeys = Keys.Control | Keys.R;
            this.menuSalesReport.Click += new EventHandler(this.MenuSalesReport_Click);
            
            this.menuPurchaseReport.Name = "menuPurchaseReport";
            this.menuPurchaseReport.Size = new Size(200, 22);
            this.menuPurchaseReport.Text = "&Purchase Report";
            this.menuPurchaseReport.ShortcutKeys = Keys.Control | Keys.Shift | Keys.R;
            this.menuPurchaseReport.Click += new EventHandler(this.MenuPurchaseReport_Click);
            
            this.menuMaintenanceReport.Name = "menuMaintenanceReport";
            this.menuMaintenanceReport.Size = new Size(200, 22);
            this.menuMaintenanceReport.Text = "&Maintenance Report";
            this.menuMaintenanceReport.ShortcutKeys = Keys.Control | Keys.M;
            this.menuMaintenanceReport.Click += new EventHandler(this.MenuMaintenanceReport_Click);
            
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(197, 6);
            
            this.menuVehicleProfitReport.Name = "menuVehicleProfitReport";
            this.menuVehicleProfitReport.Size = new Size(200, 22);
            this.menuVehicleProfitReport.Text = "&Vehicle Profit Summary";
            this.menuVehicleProfitReport.ShortcutKeys = Keys.Control | Keys.P;
            this.menuVehicleProfitReport.Click += new EventHandler(this.MenuVehicleProfitReport_Click);
            
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(197, 6);
            
            this.menuExportAllData.Name = "menuExportAllData";
            this.menuExportAllData.Size = new Size(200, 22);
            this.menuExportAllData.Text = "E&xport All Data to Excel...";
            this.menuExportAllData.ShortcutKeys = Keys.Control | Keys.E;
            this.menuExportAllData.Click += new EventHandler(this.MenuExportAllData_Click);
            
            // groupToday
            this.groupToday.Controls.Add(this.lblTodaySales);
            this.groupToday.Controls.Add(this.lblTodayPurchases);
            this.groupToday.Controls.Add(this.lblTodayMaintenance);
            this.groupToday.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.groupToday.Location = new Point(12, 35);
            this.groupToday.Name = "groupToday";
            this.groupToday.Size = new Size(300, 120);
            this.groupToday.TabIndex = 1;
            this.groupToday.TabStop = false;
            this.groupToday.Text = "Today's Summary";
            
            this.lblTodaySales.AutoSize = true;
            this.lblTodaySales.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTodaySales.Location = new Point(15, 30);
            this.lblTodaySales.Name = "lblTodaySales";
            this.lblTodaySales.Size = new Size(120, 19);
            this.lblTodaySales.Text = "Sales: ₹0.00";
            
            this.lblTodayPurchases.AutoSize = true;
            this.lblTodayPurchases.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTodayPurchases.Location = new Point(15, 60);
            this.lblTodayPurchases.Name = "lblTodayPurchases";
            this.lblTodayPurchases.Size = new Size(150, 19);
            this.lblTodayPurchases.Text = "Purchases: ₹0.00";
            
            this.lblTodayMaintenance.AutoSize = true;
            this.lblTodayMaintenance.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTodayMaintenance.Location = new Point(15, 90);
            this.lblTodayMaintenance.Name = "lblTodayMaintenance";
            this.lblTodayMaintenance.Size = new Size(170, 19);
            this.lblTodayMaintenance.Text = "Maintenance: ₹0.00";
            
            // groupMonth
            this.groupMonth.Controls.Add(this.lblMonthSales);
            this.groupMonth.Controls.Add(this.lblMonthPurchases);
            this.groupMonth.Controls.Add(this.lblMonthMaintenance);
            this.groupMonth.Controls.Add(this.lblMonthProfit);
            this.groupMonth.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.groupMonth.Location = new Point(325, 35);
            this.groupMonth.Name = "groupMonth";
            this.groupMonth.Size = new Size(300, 150);
            this.groupMonth.TabIndex = 2;
            this.groupMonth.TabStop = false;
            this.groupMonth.Text = "This Month";
            
            this.lblMonthSales.AutoSize = true;
            this.lblMonthSales.Font = new Font("Segoe UI", 10F);
            this.lblMonthSales.Location = new Point(15, 30);
            this.lblMonthSales.Name = "lblMonthSales";
            this.lblMonthSales.Size = new Size(120, 19);
            this.lblMonthSales.Text = "Sales: ₹0.00";
            
            this.lblMonthPurchases.AutoSize = true;
            this.lblMonthPurchases.Font = new Font("Segoe UI", 10F);
            this.lblMonthPurchases.Location = new Point(15, 55);
            this.lblMonthPurchases.Name = "lblMonthPurchases";
            this.lblMonthPurchases.Size = new Size(150, 19);
            this.lblMonthPurchases.Text = "Purchases: ₹0.00";
            
            this.lblMonthMaintenance.AutoSize = true;
            this.lblMonthMaintenance.Font = new Font("Segoe UI", 10F);
            this.lblMonthMaintenance.Location = new Point(15, 80);
            this.lblMonthMaintenance.Name = "lblMonthMaintenance";
            this.lblMonthMaintenance.Size = new Size(170, 19);
            this.lblMonthMaintenance.Text = "Maintenance: ₹0.00";
            
            this.lblMonthProfit.AutoSize = true;
            this.lblMonthProfit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblMonthProfit.ForeColor = Color.Green;
            this.lblMonthProfit.Location = new Point(15, 115);
            this.lblMonthProfit.Name = "lblMonthProfit";
            this.lblMonthProfit.Size = new Size(180, 21);
            this.lblMonthProfit.Text = "Net Profit: ₹0.00";
            
            // groupQuickActions
            this.groupQuickActions.Controls.Add(this.btnAddSale);
            this.groupQuickActions.Controls.Add(this.btnAddPurchase);
            this.groupQuickActions.Controls.Add(this.btnAddMaintenance);
            this.groupQuickActions.Controls.Add(this.btnShortcuts);
            this.groupQuickActions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.groupQuickActions.Location = new Point(640, 35);
            this.groupQuickActions.Name = "groupQuickActions";
            this.groupQuickActions.Size = new Size(345, 150);
            this.groupQuickActions.TabIndex = 3;
            this.groupQuickActions.TabStop = false;
            this.groupQuickActions.Text = "Quick Actions";
            
            this.btnAddSale.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnAddSale.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnAddSale.Location = new Point(15, 30);
            this.btnAddSale.Name = "btnAddSale";
            this.btnAddSale.Size = new Size(315, 30);
            this.btnAddSale.TabIndex = 0;
            this.btnAddSale.Text = "Add Sale";
            this.btnAddSale.UseVisualStyleBackColor = true;
            this.btnAddSale.Click += new EventHandler(this.MenuAddSale_Click);
            
            this.btnAddPurchase.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnAddPurchase.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnAddPurchase.Location = new Point(15, 70);
            this.btnAddPurchase.Name = "btnAddPurchase";
            this.btnAddPurchase.Size = new Size(315, 30);
            this.btnAddPurchase.TabIndex = 1;
            this.btnAddPurchase.Text = "Add Purchase";
            this.btnAddPurchase.UseVisualStyleBackColor = true;
            this.btnAddPurchase.Click += new EventHandler(this.MenuAddPurchase_Click);
            
            this.btnAddMaintenance.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnAddMaintenance.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnAddMaintenance.Location = new Point(15, 110);
            this.btnAddMaintenance.Name = "btnAddMaintenance";
            this.btnAddMaintenance.Size = new Size(240, 30);
            this.btnAddMaintenance.TabIndex = 2;
            this.btnAddMaintenance.Text = "Add Maintenance";
            this.btnAddMaintenance.UseVisualStyleBackColor = true;
            this.btnAddMaintenance.Click += new EventHandler(this.MenuAddMaintenance_Click);
            
            this.btnShortcuts.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnShortcuts.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.btnShortcuts.Location = new Point(260, 110);
            this.btnShortcuts.Name = "btnShortcuts";
            this.btnShortcuts.Size = new Size(70, 30);
            this.btnShortcuts.TabIndex = 3;
            this.btnShortcuts.Text = "⌨ Keys";
            this.btnShortcuts.UseVisualStyleBackColor = true;
            this.btnShortcuts.Click += new EventHandler(this.BtnShortcuts_Click);
            
            // groupRecent
            this.groupRecent.Controls.Add(this.dgvRecent);
            this.groupRecent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.groupRecent.Location = new Point(12, 195);
            this.groupRecent.Name = "groupRecent";
            this.groupRecent.Size = new Size(973, 350);
            this.groupRecent.TabIndex = 4;
            this.groupRecent.TabStop = false;
            this.groupRecent.Text = "Recent Transactions";
            
            // dgvRecent
            this.dgvRecent.AllowUserToAddRows = false;
            this.dgvRecent.AllowUserToDeleteRows = false;
            this.dgvRecent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecent.Dock = DockStyle.Fill;
            this.dgvRecent.Location = new Point(3, 19);
            this.dgvRecent.Name = "dgvRecent";
            this.dgvRecent.ReadOnly = true;
            this.dgvRecent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecent.Size = new Size(967, 328);
            this.dgvRecent.TabIndex = 0;
            this.dgvRecent.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvRecent_CellDoubleClick);
            this.dgvRecent.MouseClick += new MouseEventHandler(this.DgvRecent_MouseClick);
            
            // statusStrip
            this.statusStrip.Items.AddRange(new ToolStripItem[] { this.lblStatus });
            this.statusStrip.Location = new Point(0, 555);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(1000, 22);
            this.statusStrip.TabIndex = 5;
            
            // lblStatus
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(39, 17);
            this.lblStatus.Text = "Ready";
            
            // MainForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 577);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupRecent);
            this.Controls.Add(this.groupQuickActions);
            this.Controls.Add(this.groupMonth);
            this.Controls.Add(this.groupToday);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CrushEase Ledger";
            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "mainlogo.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
            }
            catch { }
            this.Load += new EventHandler(this.MainForm_Load);
            
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupToday.ResumeLayout(false);
            this.groupToday.PerformLayout();
            this.groupMonth.ResumeLayout(false);
            this.groupMonth.PerformLayout();
            this.groupQuickActions.ResumeLayout(false);
            this.groupRecent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuBackup;
        private ToolStripMenuItem menuRestore;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem menuExit;
        private ToolStripMenuItem menuMasters;
        private ToolStripMenuItem menuVehicles;
        private ToolStripMenuItem menuVendors;
        private ToolStripMenuItem menuBuyers;
        private ToolStripMenuItem menuMaterials;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem menuViewMasterData;
        private ToolStripMenuItem menuTransactions;
        private ToolStripMenuItem menuAddSale;
        private ToolStripMenuItem menuAddPurchase;
        private ToolStripMenuItem menuAddMaintenance;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem menuViewTransactions;
        private ToolStripMenuItem menuReports;
        private ToolStripMenuItem menuSalesReport;
        private ToolStripMenuItem menuPurchaseReport;
        private ToolStripMenuItem menuMaintenanceReport;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem menuVehicleProfitReport;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem menuExportAllData;
        private GroupBox groupToday;
        private Label lblTodaySales;
        private Label lblTodayPurchases;
        private Label lblTodayMaintenance;
        private GroupBox groupMonth;
        private Label lblMonthSales;
        private Label lblMonthPurchases;
        private Label lblMonthMaintenance;
        private Label lblMonthProfit;
        private GroupBox groupQuickActions;
        private Button btnAddSale;
        private Button btnAddPurchase;
        private Button btnAddMaintenance;
        private Button btnShortcuts;
        private GroupBox groupRecent;
        private DataGridView dgvRecent;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
    }
}
