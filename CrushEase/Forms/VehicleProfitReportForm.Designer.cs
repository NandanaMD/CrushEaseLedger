namespace CrushEase.Forms
{
    partial class VehicleProfitReportForm
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
            this.btnGenerate = new Button();
            this.dgvReport = new DataGridView();
            this.panelTotals = new Panel();
            this.lblTotalSales = new Label();
            this.lblTotalPurchases = new Label();
            this.lblTotalMaintenance = new Label();
            this.lblGrandProfit = new Label();
            this.btnExport = new Button();
            this.btnClose = new Button();
            
            this.groupFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvReport).BeginInit();
            this.panelTotals.SuspendLayout();
            this.SuspendLayout();
            
            // groupFilters
            this.groupFilters.Controls.Add(this.btnGenerate);
            this.groupFilters.Controls.Add(this.dtpToDate);
            this.groupFilters.Controls.Add(this.lblToDate);
            this.groupFilters.Controls.Add(this.dtpFromDate);
            this.groupFilters.Controls.Add(this.lblFromDate);
            this.groupFilters.Location = new Point(12, 12);
            this.groupFilters.Name = "groupFilters";
            this.groupFilters.Size = new Size(976, 80);
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
            
            // btnGenerate
            this.btnGenerate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnGenerate.Location = new Point(520, 23);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new Size(140, 40);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new EventHandler(this.BtnGenerate_Click);
            
            // dgvReport
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Location = new Point(12, 100);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new Size(976, 380);
            this.dgvReport.TabIndex = 1;
            
            // panelTotals
            this.panelTotals.BorderStyle = BorderStyle.FixedSingle;
            this.panelTotals.Controls.Add(this.lblGrandProfit);
            this.panelTotals.Controls.Add(this.lblTotalMaintenance);
            this.panelTotals.Controls.Add(this.lblTotalPurchases);
            this.panelTotals.Controls.Add(this.lblTotalSales);
            this.panelTotals.Location = new Point(12, 490);
            this.panelTotals.Name = "panelTotals";
            this.panelTotals.Size = new Size(976, 45);
            this.panelTotals.TabIndex = 2;
            
            // lblTotalSales
            this.lblTotalSales.AutoSize = true;
            this.lblTotalSales.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblTotalSales.Location = new Point(10, 12);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = new Size(110, 15);
            this.lblTotalSales.TabIndex = 0;
            this.lblTotalSales.Text = "Total Sales: ₹0.00";
            
            // lblTotalPurchases
            this.lblTotalPurchases.AutoSize = true;
            this.lblTotalPurchases.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblTotalPurchases.Location = new Point(200, 12);
            this.lblTotalPurchases.Name = "lblTotalPurchases";
            this.lblTotalPurchases.Size = new Size(145, 15);
            this.lblTotalPurchases.TabIndex = 1;
            this.lblTotalPurchases.Text = "Total Purchases: ₹0.00";
            
            // lblTotalMaintenance
            this.lblTotalMaintenance.AutoSize = true;
            this.lblTotalMaintenance.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblTotalMaintenance.Location = new Point(420, 12);
            this.lblTotalMaintenance.Name = "lblTotalMaintenance";
            this.lblTotalMaintenance.Size = new Size(165, 15);
            this.lblTotalMaintenance.TabIndex = 2;
            this.lblTotalMaintenance.Text = "Total Maintenance: ₹0.00";
            
            // lblGrandProfit
            this.lblGrandProfit.AutoSize = true;
            this.lblGrandProfit.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblGrandProfit.Location = new Point(680, 10);
            this.lblGrandProfit.Name = "lblGrandProfit";
            this.lblGrandProfit.Size = new Size(180, 20);
            this.lblGrandProfit.TabIndex = 3;
            this.lblGrandProfit.Text = "Grand Net Profit: ₹0.00";
            
            // btnExport
            this.btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExport.Location = new Point(678, 545);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(150, 35);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new EventHandler(this.BtnExport_Click);
            
            // btnClose
            this.btnClose.Location = new Point(838, 545);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(150, 35);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            
            // VehicleProfitReportForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 592);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.panelTotals);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.groupFilters);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "VehicleProfitReportForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Vehicle Profit Report";
            this.Load += new EventHandler(this.VehicleProfitReportForm_Load);
            this.groupFilters.ResumeLayout(false);
            this.groupFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvReport).EndInit();
            this.panelTotals.ResumeLayout(false);
            this.panelTotals.PerformLayout();
            this.ResumeLayout(false);
        }

        private GroupBox groupFilters;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Button btnGenerate;
        private DataGridView dgvReport;
        private Panel panelTotals;
        private Label lblTotalSales;
        private Label lblTotalPurchases;
        private Label lblTotalMaintenance;
        private Label lblGrandProfit;
        private Button btnExport;
        private Button btnClose;
    }
}
