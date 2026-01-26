using CrushEase.Data;
using CrushEase.Services;
using CrushEase.Utils;

namespace CrushEase.Forms;

public partial class MainForm : Form
{
    private System.Windows.Forms.Timer? _backupTimer;
    private System.Windows.Forms.Timer? _idleTimer;
    private DateTime _lastActivity;
    private const int IDLE_TIMEOUT_MINUTES = 6;
    
    public MainForm()
    {
        InitializeComponent();
        InitializeBackupTimer();
        InitializeIdleTimer();
        
        // Track user activity
        this.MouseMove += ResetIdleTimer;
        this.KeyPress += (s, e) => ResetIdleTimer(s, EventArgs.Empty);
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // Handle global keyboard shortcuts
        switch (keyData)
        {
            case Keys.F1:
                ShowShortcutsHelp();
                return true;
            case Keys.F2:
                MenuAddSale_Click(this, EventArgs.Empty);
                return true;
            case Keys.F3:
                MenuAddPurchase_Click(this, EventArgs.Empty);
                return true;
            case Keys.F4:
                MenuAddMaintenance_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.Shift | Keys.V:
                MenuVehicles_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.Shift | Keys.D:
                MenuVendors_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.Shift | Keys.B:
                MenuBuyers_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.Shift | Keys.M:
                MenuMaterials_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.T:
                MenuViewTransactions_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.D:
                MenuViewMasterData_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.R:
                MenuSalesReport_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.Shift | Keys.R:
                MenuPurchaseReport_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.M:
                MenuMaintenanceReport_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.E:
                MenuExportAllData_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.B:
                MenuBackup_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.Shift | Keys.O:
                MenuRestore_Click(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.L:
                LockApplication();
                return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
    
    private void ShowShortcutsHelp()
    {
        using var form = new ShortcutsHelpForm();
        form.ShowDialog(this);
    }
    
    private void MainForm_Load(object sender, EventArgs e)
    {
        // Check for PIN setup or lock on startup
        if (!Services.SecurityService.IsPinConfigured())
        {
            // First time - setup PIN
            using var setupForm = new LockScreenForm(setupMode: true, parent: this);
            if (setupForm.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }
        }
        else
        {
            // Lock on startup
            if (!ShowLockScreen())
            {
                Application.Exit();
                return;
            }
        }
        
        LoadDashboard();
        Utils.ModernTheme.ApplyToForm(this);
        ResetIdleTimer(this, EventArgs.Empty);
    }
    
    private void InitializeIdleTimer()
    {
        _lastActivity = DateTime.Now;
        _idleTimer = new System.Windows.Forms.Timer();
        _idleTimer.Interval = 30000; // Check every 30 seconds
        _idleTimer.Tick += (s, e) =>
        {
            TimeSpan idleTime = DateTime.Now - _lastActivity;
            if (idleTime.TotalMinutes >= IDLE_TIMEOUT_MINUTES)
            {
                LockApplication();
            }
        };
        _idleTimer.Start();
    }
    
    private void ResetIdleTimer(object? sender, EventArgs e)
    {
        _lastActivity = DateTime.Now;
    }
    
    private void LockApplication()
    {
        _idleTimer?.Stop();
        
        if (!ShowLockScreen())
        {
            Application.Exit();
        }
        else
        {
            ResetIdleTimer(this, EventArgs.Empty);
            _idleTimer?.Start();
        }
    }
    
    private bool ShowLockScreen()
    {
        using var lockForm = new LockScreenForm(setupMode: false, parent: this);
        return lockForm.ShowDialog(this) == DialogResult.OK;
    }
    
    private void InitializeBackupTimer()
    {
        // Set up automatic backup every 5 minutes
        _backupTimer = new System.Windows.Forms.Timer();
        _backupTimer.Interval = 5 * 60 * 1000; // 5 minutes in milliseconds
        _backupTimer.Tick += (s, e) => 
        {
            Services.BackupService.AutoBackup();
            Logger.LogInfo("Periodic auto backup completed (5 min interval)");
        };
        _backupTimer.Start();
    }
    
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        
        // Backup on exit for extra data safety
        try
        {
            Services.BackupService.AutoBackup();
            Logger.LogInfo("Exit backup completed");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Exit backup failed");
        }
        
        // Clean up timer
        _backupTimer?.Stop();
        _backupTimer?.Dispose();
        _idleTimer?.Stop();
        _idleTimer?.Dispose();
    }
    
    private void LoadDashboard()
    {
        try
        {
            var summary = DashboardRepository.GetSummary();
            
            // Today's summary
            lblTodaySales.Text = $"Sales: ₹{summary.TodaySales:N2}";
            lblTodayPurchases.Text = $"Purchases: ₹{summary.TodayPurchases:N2}";
            lblTodayMaintenance.Text = $"Maintenance: ₹{summary.TodayMaintenance:N2}";
            
            // Month summary
            lblMonthSales.Text = $"Sales: ₹{summary.MonthSales:N2}";
            lblMonthPurchases.Text = $"Purchases: ₹{summary.MonthPurchases:N2}";
            lblMonthMaintenance.Text = $"Maintenance: ₹{summary.MonthMaintenance:N2}";
            lblMonthProfit.Text = $"Net Profit: ₹{summary.MonthNetProfit:N2}";
            lblMonthProfit.ForeColor = summary.MonthNetProfit >= 0 ? Color.Green : Color.Red;
            
            // Load recent transactions (combined from all three types)
            LoadRecentTransactions();
            
            lblStatus.Text = $"Dashboard loaded - {DateTime.Now:dd-MMM-yyyy HH:mm}";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load dashboard");
            MessageBox.Show("Failed to load dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void LoadRecentTransactions()
    {
        try
        {
            var recentTransactions = new List<RecentTransaction>();
            
            // Get recent sales
            var sales = SaleRepository.GetRecent(5);
            foreach (var sale in sales)
            {
                recentTransactions.Add(new RecentTransaction
                {
                    TransactionId = sale.SaleId,
                    Date = sale.SaleDate,
                    Type = "Sale",
                    Vehicle = sale.VehicleNo ?? "",
                    Party = sale.BuyerName ?? "",
                    Material = sale.MaterialName ?? "",
                    Amount = sale.Amount
                });
            }
            
            // Get recent purchases
            var purchases = PurchaseRepository.GetRecent(5);
            foreach (var purchase in purchases)
            {
                recentTransactions.Add(new RecentTransaction
                {
                    TransactionId = purchase.PurchaseId,
                    Date = purchase.PurchaseDate,
                    Type = "Purchase",
                    Vehicle = purchase.VehicleNo ?? "",
                    Party = purchase.VendorName ?? "",
                    Material = purchase.MaterialName ?? "",
                    Amount = purchase.Amount
                });
            }
            
            // Get recent maintenance
            var maintenance = MaintenanceRepository.GetRecent(5);
            foreach (var maint in maintenance)
            {
                recentTransactions.Add(new RecentTransaction
                {
                    TransactionId = maint.MaintenanceId,
                    Date = maint.MaintenanceDate,
                    Type = "Maintenance",
                    Vehicle = maint.VehicleNo ?? "",
                    Party = "-",
                    Material = maint.Description,
                    Amount = maint.Amount
                });
            }
            
            // Sort by date descending and take top 15
            var sortedTransactions = recentTransactions
                .OrderByDescending(t => t.Date)
                .Take(15)
                .ToList();
            
            dgvRecent.DataSource = sortedTransactions;
            
            // Hide TransactionId column
            if (dgvRecent.Columns["TransactionId"] != null)
            {
                dgvRecent.Columns["TransactionId"].Visible = false;
            }
            
            // Format amount column
            if (dgvRecent.Columns["Amount"] != null)
            {
                dgvRecent.Columns["Amount"].DefaultCellStyle.Format = "N2";
                dgvRecent.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Format date column
            if (dgvRecent.Columns["Date"] != null)
            {
                dgvRecent.Columns["Date"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load recent transactions");
            // Don't show error to user, just log it
        }
    }
    
    // Menu handlers
    private void MenuVehicles_Click(object sender, EventArgs e)
    {
        var form = new VehicleMasterForm();
        form.ShowDialog();
        LoadDashboard(); // Refresh
    }
    
    private void MenuVendors_Click(object sender, EventArgs e)
    {
        var form = new VendorMasterForm();
        form.ShowDialog();
        LoadDashboard();
    }
    
    private void MenuBuyers_Click(object sender, EventArgs e)
    {
        var form = new BuyerMasterForm();
        form.ShowDialog();
        LoadDashboard();
    }
    
    private void MenuMaterials_Click(object sender, EventArgs e)
    {
        var form = new MaterialMasterForm();
        form.ShowDialog();
        LoadDashboard();
    }
    
    private void MenuViewMasterData_Click(object sender, EventArgs e)
    {
        var form = new MasterDataViewerForm();
        form.ShowDialog();
        LoadDashboard();
    }
    
    private void MenuAddSale_Click(object sender, EventArgs e)
    {
        var form = new SaleEntryForm();
        form.ShowDialog();
        LoadDashboard(); // Refresh
    }
    
    private void MenuAddPurchase_Click(object sender, EventArgs e)
    {
        var form = new PurchaseEntryForm();
        form.ShowDialog();
        LoadDashboard();
    }
    
    private void MenuAddMaintenance_Click(object sender, EventArgs e)
    {
        var form = new MaintenanceEntryForm();
        form.ShowDialog();
        LoadDashboard();
    }
    
    private void MenuViewTransactions_Click(object sender, EventArgs e)
    {
        var form = new TransactionViewerForm();
        form.ShowDialog();
        LoadDashboard();
    }
    
    private void MenuSalesReport_Click(object sender, EventArgs e)
    {
        var form = new SalesReportForm();
        form.ShowDialog();
    }
    
    private void MenuPurchaseReport_Click(object sender, EventArgs e)
    {
        var form = new PurchaseReportForm();
        form.ShowDialog();
    }
    
    private void MenuMaintenanceReport_Click(object sender, EventArgs e)
    {
        var form = new MaintenanceReportForm();
        form.ShowDialog();
    }
    
    private void MenuLock_Click(object sender, EventArgs e)
    {
        LockApplication();
    }
    
    private void MenuExportAllData_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog();
        dialog.Filter = "Excel Files|*.xlsx";
        dialog.FileName = $"CrushEase_CompleteData_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        dialog.Title = "Export All Data to Excel";
        
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                lblStatus.Text = "Generating complete data export...";
                Application.DoEvents();
                
                Services.ExcelReportGenerator.GenerateCompleteDataExport(dialog.FileName);
                
                lblStatus.Text = "Export completed successfully";
                
                if (MessageBox.Show(
                    $"Complete data export created successfully!\n\n{dialog.FileName}\n\nWould you like to open the file?", 
                    "Success", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = dialog.FileName,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to generate complete data export");
                MessageBox.Show($"Failed to generate export: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Export failed";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
    
    private void MenuBackup_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog();
        dialog.Filter = "Database Files|*.db";
        dialog.FileName = $"CrushEase_Backup_{DateTime.Now:yyyyMMdd}.db";
        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            if (Services.BackupService.ManualBackup(dialog.FileName))
            {
                MessageBox.Show($"Backup created successfully:\n{dialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Backup failed. Check logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void MenuRestore_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Restoring will replace current database.\nMake sure you have a recent backup!\n\nContinue?",
            "Warning",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result != DialogResult.Yes)
            return;
        
        using var dialog = new OpenFileDialog();
        dialog.Filter = "Database Files|*.db";
        dialog.InitialDirectory = Config.BackupFolder;
        
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            this.Hide(); // Hide main form immediately
            
            if (Services.BackupService.RestoreBackup(dialog.FileName))
            {
                MessageBox.Show("Database restored successfully.\nApplication will now restart.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Start new instance before closing this one
                System.Diagnostics.Process.Start(Application.ExecutablePath);
                Environment.Exit(0);
            }
            else
            {
                this.Show(); // Show form again if restore failed
                MessageBox.Show("Restore failed. Check logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void MenuExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
    
    private void MenuCheckForUpdates_Click(object sender, EventArgs e)
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/NandanaMD/CrushEaseLedger/releases/latest",
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
        catch
        {
            // Silent fail - no internet or browser unavailable
        }
    }
    
    private void BtnShortcuts_Click(object sender, EventArgs e)
    {
        ShowShortcutsHelp();
    }
    
    private void DgvRecent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgvRecent.Rows[e.RowIndex].DataBoundItem == null) return;
        
        var transaction = dgvRecent.Rows[e.RowIndex].DataBoundItem as RecentTransaction;
        if (transaction != null)
        {
            EditTransaction(transaction);
        }
    }
    
    private void DgvRecent_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            var hitTest = dgvRecent.HitTest(e.X, e.Y);
            if (hitTest.RowIndex >= 0 && hitTest.RowIndex < dgvRecent.Rows.Count)
            {
                if (dgvRecent.Rows[hitTest.RowIndex].DataBoundItem == null) return;
                
                dgvRecent.ClearSelection();
                dgvRecent.Rows[hitTest.RowIndex].Selected = true;
                
                var transaction = dgvRecent.Rows[hitTest.RowIndex].DataBoundItem as RecentTransaction;
                if (transaction == null) return;
                
                var contextMenu = new ContextMenuStrip();
                var editItem = new ToolStripMenuItem("Edit", null, (s, ev) =>
                {
                    EditTransaction(transaction);
                });
                var deleteItem = new ToolStripMenuItem("Delete", null, (s, ev) =>
                {
                    DeleteTransaction(transaction);
                });
                
                contextMenu.Items.Add(editItem);
                contextMenu.Items.Add(deleteItem);
                contextMenu.Show(dgvRecent, e.Location);
            }
        }
    }
    
    private void EditTransaction(RecentTransaction transaction)
    {
        Form? form = transaction.Type switch
        {
            "Sale" => new SaleEntryForm(transaction.TransactionId),
            "Purchase" => new PurchaseEntryForm(transaction.TransactionId),
            "Maintenance" => new MaintenanceEntryForm(transaction.TransactionId),
            _ => null
        };
        
        if (form != null)
        {
            form.ShowDialog();
            LoadDashboard();
        }
    }
    
    private void DeleteTransaction(RecentTransaction transaction)
    {
        var result = MessageBox.Show(
            $"Are you sure you want to delete this {transaction.Type}?\n\n" +
            $"Date: {transaction.Date:dd-MMM-yyyy}\n" +
            $"Vehicle: {transaction.Vehicle}\n" +
            $"Amount: ₹{transaction.Amount:N2}",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result == DialogResult.Yes)
        {
            try
            {
                switch (transaction.Type)
                {
                    case "Sale":
                        SaleRepository.Delete(transaction.TransactionId);
                        break;
                    case "Purchase":
                        PurchaseRepository.Delete(transaction.TransactionId);
                        break;
                    case "Maintenance":
                        MaintenanceRepository.Delete(transaction.TransactionId);
                        break;
                }
                
                MessageBox.Show($"{transaction.Type} deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDashboard();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to delete {transaction.Type}");
                MessageBox.Show($"Failed to delete {transaction.Type}: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    // Helper class for recent transactions display
    private class RecentTransaction
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";
        public string Vehicle { get; set; } = "";
        public string Party { get; set; } = "";
        public string Material { get; set; } = "";
        public decimal Amount { get; set; }
    }
}
