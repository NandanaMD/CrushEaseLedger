using CrushEase.Data;
using CrushEase.Models;
using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class VehicleProfitReportForm : Form
{
    private List<VehicleProfitSummary> _currentData;
    
    public VehicleProfitReportForm()
    {
        InitializeComponent();
        _currentData = new List<VehicleProfitSummary>();
    }
    
    private void VehicleProfitReportForm_Load(object sender, EventArgs e)
    {
        // Set default date range to current month
        var today = DateTime.Today;
        dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
        dtpToDate.Value = today;
        
        LoadReport();
    }
    
    private void BtnGenerate_Click(object sender, EventArgs e)
    {
        LoadReport();
    }
    
    private void LoadReport()
    {
        try
        {
            var fromDate = dtpFromDate.Value.Date;
            var toDate = dtpToDate.Value.Date;
            
            if (fromDate > toDate)
            {
                MessageBox.Show("From Date cannot be greater than To Date", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get vehicle profit summary
            _currentData = DashboardRepository.GetVehicleProfitSummary(fromDate, toDate);
            
            // Bind to grid
            dgvReport.DataSource = null;
            dgvReport.DataSource = _currentData;
            
            // Configure columns
            if (dgvReport.Columns.Count > 0)
            {
                dgvReport.Columns["VehicleNo"].HeaderText = "Vehicle";
                dgvReport.Columns["TotalSales"].HeaderText = "Total Sales";
                dgvReport.Columns["TotalPurchases"].HeaderText = "Total Purchases";
                dgvReport.Columns["TotalMaintenance"].HeaderText = "Total Maintenance";
                dgvReport.Columns["NetProfit"].HeaderText = "Net Profit";
                
                dgvReport.Columns["TotalSales"].DefaultCellStyle.Format = "N2";
                dgvReport.Columns["TotalPurchases"].DefaultCellStyle.Format = "N2";
                dgvReport.Columns["TotalMaintenance"].DefaultCellStyle.Format = "N2";
                dgvReport.Columns["NetProfit"].DefaultCellStyle.Format = "N2";
                
                dgvReport.Columns["TotalSales"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvReport.Columns["TotalPurchases"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvReport.Columns["TotalMaintenance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvReport.Columns["NetProfit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Apply color coding
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.DataBoundItem is VehicleProfitSummary summary)
                {
                    if (summary.NetProfit >= 0)
                    {
                        row.Cells["NetProfit"].Style.ForeColor = Color.Green;
                        row.Cells["NetProfit"].Style.Font = new Font(dgvReport.Font, FontStyle.Bold);
                    }
                    else
                    {
                        row.Cells["NetProfit"].Style.ForeColor = Color.Red;
                        row.Cells["NetProfit"].Style.Font = new Font(dgvReport.Font, FontStyle.Bold);
                    }
                }
            }
            
            // Update totals
            var totalSales = _currentData.Sum(v => v.TotalSales);
            var totalPurchases = _currentData.Sum(v => v.TotalPurchases);
            var totalMaintenance = _currentData.Sum(v => v.TotalMaintenance);
            var grandNetProfit = _currentData.Sum(v => v.NetProfit);
            
            lblTotalSales.Text = $"Total Sales: ₹{totalSales:N2}";
            lblTotalPurchases.Text = $"Total Purchases: ₹{totalPurchases:N2}";
            lblTotalMaintenance.Text = $"Total Maintenance: ₹{totalMaintenance:N2}";
            lblGrandProfit.Text = $"Grand Net Profit: ₹{grandNetProfit:N2}";
            
            // Color code grand profit
            if (grandNetProfit >= 0)
            {
                lblGrandProfit.ForeColor = Color.Green;
            }
            else
            {
                lblGrandProfit.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load vehicle profit report");
            MessageBox.Show("Failed to load report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (_currentData == null || _currentData.Count == 0)
            {
                MessageBox.Show("No data to export", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            using var sfd = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Export Vehicle Profit Report",
                FileName = $"VehicleProfitReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExcelReportGenerator.GenerateVehicleProfitReport(
                    _currentData,
                    dtpFromDate.Value.Date,
                    dtpToDate.Value.Date,
                    sfd.FileName);
                
                MessageBox.Show("Report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Ask if user wants to open the file
                if (MessageBox.Show("Do you want to open the file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to export vehicle profit report");
            MessageBox.Show("Failed to export report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}
