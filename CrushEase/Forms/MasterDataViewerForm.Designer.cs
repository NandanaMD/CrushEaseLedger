namespace CrushEase.Forms
{
    partial class MasterDataViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl;
        private TabPage tabVehicles;
        private TabPage tabVendors;
        private TabPage tabBuyers;
        private TabPage tabMaterials;
        
        // Vehicles tab controls
        private DataGridView dgvVehicles;
        private TextBox txtVehicleSearch;
        private CheckBox chkVehiclesActiveOnly;
        private Label lblVehicleSearch;
        private Label lblVehicleCount;
        private Button btnDeleteVehicle;
        
        // Vendors tab controls
        private DataGridView dgvVendors;
        private TextBox txtVendorSearch;
        private CheckBox chkVendorsActiveOnly;
        private Label lblVendorSearch;
        private Label lblVendorCount;
        private Button btnDeleteVendor;
        
        // Buyers tab controls
        private DataGridView dgvBuyers;
        private TextBox txtBuyerSearch;
        private CheckBox chkBuyersActiveOnly;
        private Label lblBuyerSearch;
        private Label lblBuyerCount;
        private Button btnDeleteBuyer;
        
        // Materials tab controls
        private DataGridView dgvMaterials;
        private TextBox txtMaterialSearch;
        private CheckBox chkMaterialsActiveOnly;
        private Label lblMaterialSearch;
        private Label lblMaterialCount;
        private Button btnDeleteMaterial;
        
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
            this.tabVehicles = new TabPage();
            this.tabVendors = new TabPage();
            this.tabBuyers = new TabPage();
            this.tabMaterials = new TabPage();
            
            // Vehicles
            this.dgvVehicles = new DataGridView();
            this.txtVehicleSearch = new TextBox();
            this.chkVehiclesActiveOnly = new CheckBox();
            this.lblVehicleSearch = new Label();
            this.lblVehicleCount = new Label();
            this.btnDeleteVehicle = new Button();
            
            // Vendors
            this.dgvVendors = new DataGridView();
            this.txtVendorSearch = new TextBox();
            this.chkVendorsActiveOnly = new CheckBox();
            this.lblVendorSearch = new Label();
            this.lblVendorCount = new Label();
            this.btnDeleteVendor = new Button();
            
            // Buyers
            this.dgvBuyers = new DataGridView();
            this.txtBuyerSearch = new TextBox();
            this.chkBuyersActiveOnly = new CheckBox();
            this.lblBuyerSearch = new Label();
            this.lblBuyerCount = new Label();
            this.btnDeleteBuyer = new Button();
            
            // Materials
            this.dgvMaterials = new DataGridView();
            this.txtMaterialSearch = new TextBox();
            this.chkMaterialsActiveOnly = new CheckBox();
            this.lblMaterialSearch = new Label();
            this.lblMaterialCount = new Label();
            this.btnDeleteMaterial = new Button();
            
            // Common
            this.btnRefresh = new Button();
            this.btnClose = new Button();
            
            this.tabControl.SuspendLayout();
            this.tabVehicles.SuspendLayout();
            this.tabVendors.SuspendLayout();
            this.tabBuyers.SuspendLayout();
            this.tabMaterials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaterials)).BeginInit();
            this.SuspendLayout();
            
            // tabControl
            this.tabControl.Controls.Add(this.tabVehicles);
            this.tabControl.Controls.Add(this.tabVendors);
            this.tabControl.Controls.Add(this.tabBuyers);
            this.tabControl.Controls.Add(this.tabMaterials);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(1000, 600);
            this.tabControl.TabIndex = 0;
            
            // tabVehicles
            this.tabVehicles.Controls.Add(this.dgvVehicles);
            this.tabVehicles.Controls.Add(this.btnDeleteVehicle);
            this.tabVehicles.Controls.Add(this.txtVehicleSearch);
            this.tabVehicles.Controls.Add(this.chkVehiclesActiveOnly);
            this.tabVehicles.Controls.Add(this.lblVehicleSearch);
            this.tabVehicles.Controls.Add(this.lblVehicleCount);
            this.tabVehicles.Location = new Point(4, 24);
            this.tabVehicles.Name = "tabVehicles";
            this.tabVehicles.Padding = new Padding(10);
            this.tabVehicles.Size = new Size(992, 572);
            this.tabVehicles.TabIndex = 0;
            this.tabVehicles.Text = "Vehicles";
            this.tabVehicles.UseVisualStyleBackColor = true;
            
            // lblVehicleSearch
            this.lblVehicleSearch.AutoSize = true;
            this.lblVehicleSearch.Location = new Point(10, 15);
            this.lblVehicleSearch.Name = "lblVehicleSearch";
            this.lblVehicleSearch.Size = new Size(48, 15);
            this.lblVehicleSearch.TabIndex = 0;
            this.lblVehicleSearch.Text = "Search:";
            
            // txtVehicleSearch
            this.txtVehicleSearch.Location = new Point(70, 12);
            this.txtVehicleSearch.Name = "txtVehicleSearch";
            this.txtVehicleSearch.Size = new Size(300, 23);
            this.txtVehicleSearch.TabIndex = 1;
            this.txtVehicleSearch.TextChanged += new EventHandler(this.TxtVehicleSearch_TextChanged);
            
            // chkVehiclesActiveOnly
            this.chkVehiclesActiveOnly.AutoSize = true;
            this.chkVehiclesActiveOnly.Checked = true;
            this.chkVehiclesActiveOnly.CheckState = CheckState.Checked;
            this.chkVehiclesActiveOnly.Location = new Point(390, 14);
            this.chkVehiclesActiveOnly.Name = "chkVehiclesActiveOnly";
            this.chkVehiclesActiveOnly.Size = new Size(85, 19);
            this.chkVehiclesActiveOnly.TabIndex = 2;
            this.chkVehiclesActiveOnly.Text = "Active Only";
            this.chkVehiclesActiveOnly.UseVisualStyleBackColor = true;
            this.chkVehiclesActiveOnly.CheckedChanged += new EventHandler(this.ChkVehiclesActiveOnly_CheckedChanged);
            
            // lblVehicleCount
            this.lblVehicleCount.AutoSize = true;
            this.lblVehicleCount.Location = new Point(500, 15);
            this.lblVehicleCount.Name = "lblVehicleCount";
            this.lblVehicleCount.Size = new Size(40, 15);
            this.lblVehicleCount.TabIndex = 3;
            this.lblVehicleCount.Text = "Total: 0";
            
            // dgvVehicles
            this.dgvVehicles.AllowUserToAddRows = false;
            this.dgvVehicles.AllowUserToDeleteRows = false;
            this.dgvVehicles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVehicles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehicles.Location = new Point(10, 45);
            this.dgvVehicles.Name = "dgvVehicles";
            this.dgvVehicles.ReadOnly = true;
            this.dgvVehicles.RowHeadersVisible = false;
            this.dgvVehicles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvVehicles.Size = new Size(892, 517);
            this.dgvVehicles.TabIndex = 4;
            
            // btnDeleteVehicle
            this.btnDeleteVehicle.Location = new Point(908, 45);
            this.btnDeleteVehicle.Name = "btnDeleteVehicle";
            this.btnDeleteVehicle.Size = new Size(74, 30);
            this.btnDeleteVehicle.TabIndex = 5;
            this.btnDeleteVehicle.Text = "Delete";
            this.btnDeleteVehicle.UseVisualStyleBackColor = true;
            this.btnDeleteVehicle.Click += new EventHandler(this.BtnDeleteVehicle_Click);
            
            // tabVendors
            this.tabVendors.Controls.Add(this.dgvVendors);
            this.tabVendors.Controls.Add(this.btnDeleteVendor);
            this.tabVendors.Controls.Add(this.txtVendorSearch);
            this.tabVendors.Controls.Add(this.chkVendorsActiveOnly);
            this.tabVendors.Controls.Add(this.lblVendorSearch);
            this.tabVendors.Controls.Add(this.lblVendorCount);
            this.tabVendors.Location = new Point(4, 24);
            this.tabVendors.Name = "tabVendors";
            this.tabVendors.Padding = new Padding(10);
            this.tabVendors.Size = new Size(992, 572);
            this.tabVendors.TabIndex = 1;
            this.tabVendors.Text = "Vendors";
            this.tabVendors.UseVisualStyleBackColor = true;
            
            // lblVendorSearch
            this.lblVendorSearch.AutoSize = true;
            this.lblVendorSearch.Location = new Point(10, 15);
            this.lblVendorSearch.Name = "lblVendorSearch";
            this.lblVendorSearch.Size = new Size(48, 15);
            this.lblVendorSearch.TabIndex = 0;
            this.lblVendorSearch.Text = "Search:";
            
            // txtVendorSearch
            this.txtVendorSearch.Location = new Point(70, 12);
            this.txtVendorSearch.Name = "txtVendorSearch";
            this.txtVendorSearch.Size = new Size(300, 23);
            this.txtVendorSearch.TabIndex = 1;
            this.txtVendorSearch.TextChanged += new EventHandler(this.TxtVendorSearch_TextChanged);
            
            // chkVendorsActiveOnly
            this.chkVendorsActiveOnly.AutoSize = true;
            this.chkVendorsActiveOnly.Checked = true;
            this.chkVendorsActiveOnly.CheckState = CheckState.Checked;
            this.chkVendorsActiveOnly.Location = new Point(390, 14);
            this.chkVendorsActiveOnly.Name = "chkVendorsActiveOnly";
            this.chkVendorsActiveOnly.Size = new Size(85, 19);
            this.chkVendorsActiveOnly.TabIndex = 2;
            this.chkVendorsActiveOnly.Text = "Active Only";
            this.chkVendorsActiveOnly.UseVisualStyleBackColor = true;
            this.chkVendorsActiveOnly.CheckedChanged += new EventHandler(this.ChkVendorsActiveOnly_CheckedChanged);
            
            // lblVendorCount
            this.lblVendorCount.AutoSize = true;
            this.lblVendorCount.Location = new Point(500, 15);
            this.lblVendorCount.Name = "lblVendorCount";
            this.lblVendorCount.Size = new Size(40, 15);
            this.lblVendorCount.TabIndex = 3;
            this.lblVendorCount.Text = "Total: 0";
            
            // dgvVendors
            this.dgvVendors.AllowUserToAddRows = false;
            this.dgvVendors.AllowUserToDeleteRows = false;
            this.dgvVendors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVendors.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendors.Location = new Point(10, 45);
            this.dgvVendors.Name = "dgvVendors";
            this.dgvVendors.ReadOnly = true;
            this.dgvVendors.RowHeadersVisible = false;
            this.dgvVendors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendors.Size = new Size(892, 517);
            this.dgvVendors.TabIndex = 4;
            
            // btnDeleteVendor
            this.btnDeleteVendor.Location = new Point(908, 45);
            this.btnDeleteVendor.Name = "btnDeleteVendor";
            this.btnDeleteVendor.Size = new Size(74, 30);
            this.btnDeleteVendor.TabIndex = 5;
            this.btnDeleteVendor.Text = "Delete";
            this.btnDeleteVendor.UseVisualStyleBackColor = true;
            this.btnDeleteVendor.Click += new EventHandler(this.BtnDeleteVendor_Click);
            
            // tabBuyers
            this.tabBuyers.Controls.Add(this.dgvBuyers);
            this.tabBuyers.Controls.Add(this.btnDeleteBuyer);
            this.tabBuyers.Controls.Add(this.txtBuyerSearch);
            this.tabBuyers.Controls.Add(this.chkBuyersActiveOnly);
            this.tabBuyers.Controls.Add(this.lblBuyerSearch);
            this.tabBuyers.Controls.Add(this.lblBuyerCount);
            this.tabBuyers.Location = new Point(4, 24);
            this.tabBuyers.Name = "tabBuyers";
            this.tabBuyers.Padding = new Padding(10);
            this.tabBuyers.Size = new Size(992, 572);
            this.tabBuyers.TabIndex = 2;
            this.tabBuyers.Text = "Buyers";
            this.tabBuyers.UseVisualStyleBackColor = true;
            
            // lblBuyerSearch
            this.lblBuyerSearch.AutoSize = true;
            this.lblBuyerSearch.Location = new Point(10, 15);
            this.lblBuyerSearch.Name = "lblBuyerSearch";
            this.lblBuyerSearch.Size = new Size(48, 15);
            this.lblBuyerSearch.TabIndex = 0;
            this.lblBuyerSearch.Text = "Search:";
            
            // txtBuyerSearch
            this.txtBuyerSearch.Location = new Point(70, 12);
            this.txtBuyerSearch.Name = "txtBuyerSearch";
            this.txtBuyerSearch.Size = new Size(300, 23);
            this.txtBuyerSearch.TabIndex = 1;
            this.txtBuyerSearch.TextChanged += new EventHandler(this.TxtBuyerSearch_TextChanged);
            
            // chkBuyersActiveOnly
            this.chkBuyersActiveOnly.AutoSize = true;
            this.chkBuyersActiveOnly.Checked = true;
            this.chkBuyersActiveOnly.CheckState = CheckState.Checked;
            this.chkBuyersActiveOnly.Location = new Point(390, 14);
            this.chkBuyersActiveOnly.Name = "chkBuyersActiveOnly";
            this.chkBuyersActiveOnly.Size = new Size(85, 19);
            this.chkBuyersActiveOnly.TabIndex = 2;
            this.chkBuyersActiveOnly.Text = "Active Only";
            this.chkBuyersActiveOnly.UseVisualStyleBackColor = true;
            this.chkBuyersActiveOnly.CheckedChanged += new EventHandler(this.ChkBuyersActiveOnly_CheckedChanged);
            
            // lblBuyerCount
            this.lblBuyerCount.AutoSize = true;
            this.lblBuyerCount.Location = new Point(500, 15);
            this.lblBuyerCount.Name = "lblBuyerCount";
            this.lblBuyerCount.Size = new Size(40, 15);
            this.lblBuyerCount.TabIndex = 3;
            this.lblBuyerCount.Text = "Total: 0";
            
            // dgvBuyers
            this.dgvBuyers.AllowUserToAddRows = false;
            this.dgvBuyers.AllowUserToDeleteRows = false;
            this.dgvBuyers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBuyers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuyers.Location = new Point(10, 45);
            this.dgvBuyers.Name = "dgvBuyers";
            this.dgvBuyers.ReadOnly = true;
            this.dgvBuyers.RowHeadersVisible = false;
            this.dgvBuyers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvBuyers.Size = new Size(892, 517);
            this.dgvBuyers.TabIndex = 4;
            
            // btnDeleteBuyer
            this.btnDeleteBuyer.Location = new Point(908, 45);
            this.btnDeleteBuyer.Name = "btnDeleteBuyer";
            this.btnDeleteBuyer.Size = new Size(74, 30);
            this.btnDeleteBuyer.TabIndex = 5;
            this.btnDeleteBuyer.Text = "Delete";
            this.btnDeleteBuyer.UseVisualStyleBackColor = true;
            this.btnDeleteBuyer.Click += new EventHandler(this.BtnDeleteBuyer_Click);
            
            // tabMaterials
            this.tabMaterials.Controls.Add(this.dgvMaterials);
            this.tabMaterials.Controls.Add(this.btnDeleteMaterial);
            this.tabMaterials.Controls.Add(this.txtMaterialSearch);
            this.tabMaterials.Controls.Add(this.chkMaterialsActiveOnly);
            this.tabMaterials.Controls.Add(this.lblMaterialSearch);
            this.tabMaterials.Controls.Add(this.lblMaterialCount);
            this.tabMaterials.Location = new Point(4, 24);
            this.tabMaterials.Name = "tabMaterials";
            this.tabMaterials.Padding = new Padding(10);
            this.tabMaterials.Size = new Size(992, 572);
            this.tabMaterials.TabIndex = 3;
            this.tabMaterials.Text = "Materials";
            this.tabMaterials.UseVisualStyleBackColor = true;
            
            // lblMaterialSearch
            this.lblMaterialSearch.AutoSize = true;
            this.lblMaterialSearch.Location = new Point(10, 15);
            this.lblMaterialSearch.Name = "lblMaterialSearch";
            this.lblMaterialSearch.Size = new Size(48, 15);
            this.lblMaterialSearch.TabIndex = 0;
            this.lblMaterialSearch.Text = "Search:";
            
            // txtMaterialSearch
            this.txtMaterialSearch.Location = new Point(70, 12);
            this.txtMaterialSearch.Name = "txtMaterialSearch";
            this.txtMaterialSearch.Size = new Size(300, 23);
            this.txtMaterialSearch.TabIndex = 1;
            this.txtMaterialSearch.TextChanged += new EventHandler(this.TxtMaterialSearch_TextChanged);
            
            // chkMaterialsActiveOnly
            this.chkMaterialsActiveOnly.AutoSize = true;
            this.chkMaterialsActiveOnly.Checked = true;
            this.chkMaterialsActiveOnly.CheckState = CheckState.Checked;
            this.chkMaterialsActiveOnly.Location = new Point(390, 14);
            this.chkMaterialsActiveOnly.Name = "chkMaterialsActiveOnly";
            this.chkMaterialsActiveOnly.Size = new Size(85, 19);
            this.chkMaterialsActiveOnly.TabIndex = 2;
            this.chkMaterialsActiveOnly.Text = "Active Only";
            this.chkMaterialsActiveOnly.UseVisualStyleBackColor = true;
            this.chkMaterialsActiveOnly.CheckedChanged += new EventHandler(this.ChkMaterialsActiveOnly_CheckedChanged);
            
            // lblMaterialCount
            this.lblMaterialCount.AutoSize = true;
            this.lblMaterialCount.Location = new Point(500, 15);
            this.lblMaterialCount.Name = "lblMaterialCount";
            this.lblMaterialCount.Size = new Size(40, 15);
            this.lblMaterialCount.TabIndex = 3;
            this.lblMaterialCount.Text = "Total: 0";
            
            // dgvMaterials
            this.dgvMaterials.AllowUserToAddRows = false;
            this.dgvMaterials.AllowUserToDeleteRows = false;
            this.dgvMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMaterials.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaterials.Location = new Point(10, 45);
            this.dgvMaterials.Name = "dgvMaterials";
            this.dgvMaterials.ReadOnly = true;
            this.dgvMaterials.RowHeadersVisible = false;
            this.dgvMaterials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMaterials.Size = new Size(892, 517);
            this.dgvMaterials.TabIndex = 4;
            
            // btnDeleteMaterial
            this.btnDeleteMaterial.Location = new Point(908, 45);
            this.btnDeleteMaterial.Name = "btnDeleteMaterial";
            this.btnDeleteMaterial.Size = new Size(74, 30);
            this.btnDeleteMaterial.TabIndex = 5;
            this.btnDeleteMaterial.Text = "Delete";
            this.btnDeleteMaterial.UseVisualStyleBackColor = true;
            this.btnDeleteMaterial.Click += new EventHandler(this.BtnDeleteMaterial_Click);
            
            // btnRefresh
            this.btnRefresh.Location = new Point(806, 615);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(90, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(this.BtnRefresh_Click);
            
            // btnClose
            this.btnClose.Location = new Point(904, 615);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(90, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            
            // MasterDataViewerForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 650);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.tabControl);
            this.Name = "MasterDataViewerForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Master Data Viewer";
            this.Load += new EventHandler(this.MasterDataViewerForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabVehicles.ResumeLayout(false);
            this.tabVehicles.PerformLayout();
            this.tabVendors.ResumeLayout(false);
            this.tabVendors.PerformLayout();
            this.tabBuyers.ResumeLayout(false);
            this.tabBuyers.PerformLayout();
            this.tabMaterials.ResumeLayout(false);
            this.tabMaterials.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaterials)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
