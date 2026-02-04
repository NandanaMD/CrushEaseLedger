using CrushEase.Data;
using CrushEase.Utils;
using System.Data.SQLite;

namespace CrushEase.Forms;

/// <summary>
/// Form to view and restore soft-deleted items
/// </summary>
public partial class TrashViewerForm : Form
{
    private ComboBox _cmbItemType;
    private DataGridView _dgvTrash;
    private Button _btnRestore;
    private Button _btnPermanentDelete;
    private Button _btnClose;
    private Label _lblCount;
    
    public TrashViewerForm()
    {
        InitializeComponent();
        SetupUI();
        ModernTheme.ApplyToForm(this);
        LoadTrashItems();
    }
    
    private void SetupUI()
    {
        this.Text = "Trash Bin - Deleted Items";
        this.Size = new Size(1000, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        
        // Header label
        var lblHeader = new Label
        {
            Text = "View and restore deleted items",
            Location = new Point(20, 20),
            Size = new Size(400, 25),
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };
        this.Controls.Add(lblHeader);
        
        // Item type selector
        var lblType = new Label
        {
            Text = "Item Type:",
            Location = new Point(20, 60),
            Size = new Size(80, 25)
        };
        this.Controls.Add(lblType);
        
        _cmbItemType = new ComboBox
        {
            Location = new Point(110, 58),
            Size = new Size(200, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _cmbItemType.Items.AddRange(new object[] { 
            "Sales", "Purchases", "Maintenance", 
            "Vehicles", "Buyers", "Vendors", "Materials" 
        });
        _cmbItemType.SelectedIndex = 0;
        _cmbItemType.SelectedIndexChanged += (s, e) => LoadTrashItems();
        this.Controls.Add(_cmbItemType);
        
        // Count label
        _lblCount = new Label
        {
            Location = new Point(330, 60),
            Size = new Size(300, 25),
            Text = "0 deleted items"
        };
        this.Controls.Add(_lblCount);
        
        // DataGridView
        _dgvTrash = new DataGridView
        {
            Location = new Point(20, 100),
            Size = new Size(940, 390),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false
        };
        ModernTheme.StyleDataGridView(_dgvTrash);
        this.Controls.Add(_dgvTrash);
        
        // Buttons
        _btnRestore = new Button
        {
            Text = "Restore Selected",
            Location = new Point(20, 510),
            Size = new Size(150, 35),
            Font = new Font("Segoe UI", 10)
        };
        _btnRestore.Click += BtnRestore_Click;
        this.Controls.Add(_btnRestore);
        
        _btnPermanentDelete = new Button
        {
            Text = "Delete Permanently",
            Location = new Point(190, 510),
            Size = new Size(150, 35),
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(239, 68, 68)
        };
        _btnPermanentDelete.Click += BtnPermanentDelete_Click;
        this.Controls.Add(_btnPermanentDelete);
        
        _btnClose = new Button
        {
            Text = "Close",
            Location = new Point(810, 510),
            Size = new Size(150, 35),
            Font = new Font("Segoe UI", 10)
        };
        _btnClose.Click += (s, e) => Close();
        this.Controls.Add(_btnClose);
    }
    
    private void LoadTrashItems()
    {
        string itemType = _cmbItemType.SelectedItem?.ToString() ?? "Sales";
        
        using var connection = DatabaseManager.GetConnection();
        connection.Open();
        
        string sql = "";
        string tableName = "";
        
        switch (itemType)
        {
            case "Sales":
                sql = @"SELECT s.sale_id AS 'ID', s.sale_date AS 'Date', v.vehicle_no AS 'Vehicle', 
                        b.buyer_name AS 'Buyer', m.material_name AS 'Material', 
                        s.quantity AS 'Quantity', s.amount AS 'Amount', s.deleted_at AS 'Deleted At'
                        FROM sales s
                        INNER JOIN vehicles v ON s.vehicle_id = v.vehicle_id
                        INNER JOIN buyers b ON s.buyer_id = b.buyer_id
                        INNER JOIN materials m ON s.material_id = m.material_id
                        WHERE s.is_deleted = 1
                        ORDER BY s.deleted_at DESC";
                tableName = "sales";
                break;
                
            case "Purchases":
                sql = @"SELECT p.purchase_id AS 'ID', p.purchase_date AS 'Date', v.vehicle_no AS 'Vehicle', 
                        vd.vendor_name AS 'Vendor', m.material_name AS 'Material', 
                        p.quantity AS 'Quantity', p.amount AS 'Amount', p.deleted_at AS 'Deleted At'
                        FROM purchases p
                        INNER JOIN vehicles v ON p.vehicle_id = v.vehicle_id
                        INNER JOIN vendors vd ON p.vendor_id = vd.vendor_id
                        INNER JOIN materials m ON p.material_id = m.material_id
                        WHERE p.is_deleted = 1
                        ORDER BY p.deleted_at DESC";
                tableName = "purchases";
                break;
                
            case "Maintenance":
                sql = @"SELECT m.maintenance_id AS 'ID', m.maintenance_date AS 'Date', v.vehicle_no AS 'Vehicle', 
                        m.description AS 'Description', m.amount AS 'Amount', m.deleted_at AS 'Deleted At'
                        FROM maintenance m
                        INNER JOIN vehicles v ON m.vehicle_id = v.vehicle_id
                        WHERE m.is_deleted = 1
                        ORDER BY m.deleted_at DESC";
                tableName = "maintenance";
                break;
                
            case "Vehicles":
                sql = @"SELECT vehicle_id AS 'ID', vehicle_no AS 'Vehicle No', 
                        CASE WHEN is_active = 1 THEN 'Yes' ELSE 'No' END AS 'Was Active',
                        deleted_at AS 'Deleted At'
                        FROM vehicles
                        WHERE is_deleted = 1
                        ORDER BY deleted_at DESC";
                tableName = "vehicles";
                break;
                
            case "Buyers":
                sql = @"SELECT buyer_id AS 'ID', buyer_name AS 'Buyer Name', contact AS 'Contact',
                        CASE WHEN is_active = 1 THEN 'Yes' ELSE 'No' END AS 'Was Active',
                        deleted_at AS 'Deleted At'
                        FROM buyers
                        WHERE is_deleted = 1
                        ORDER BY deleted_at DESC";
                tableName = "buyers";
                break;
                
            case "Vendors":
                sql = @"SELECT vendor_id AS 'ID', vendor_name AS 'Vendor Name', contact AS 'Contact',
                        CASE WHEN is_active = 1 THEN 'Yes' ELSE 'No' END AS 'Was Active',
                        deleted_at AS 'Deleted At'
                        FROM vendors
                        WHERE is_deleted = 1
                        ORDER BY deleted_at DESC";
                tableName = "vendors";
                break;
                
            case "Materials":
                sql = @"SELECT material_id AS 'ID', material_name AS 'Material Name', unit AS 'Unit',
                        CASE WHEN is_active = 1 THEN 'Yes' ELSE 'No' END AS 'Was Active',
                        deleted_at AS 'Deleted At'
                        FROM materials
                        WHERE is_deleted = 1
                        ORDER BY deleted_at DESC";
                tableName = "materials";
                break;
        }
        
        using var cmd = new SQLiteCommand(sql, connection);
        using var adapter = new SQLiteDataAdapter(cmd);
        var dataTable = new System.Data.DataTable();
        adapter.Fill(dataTable);
        
        _dgvTrash.DataSource = dataTable;
        
        // Hide ID column
        if (_dgvTrash.Columns.Contains("ID"))
        {
            _dgvTrash.Columns["ID"].Visible = false;
        }
        
        // Format amount columns
        if (_dgvTrash.Columns.Contains("Amount"))
        {
            _dgvTrash.Columns["Amount"].DefaultCellStyle.Format = "N2";
            _dgvTrash.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        
        _lblCount.Text = $"{dataTable.Rows.Count} deleted {itemType.ToLower()}";
        
        // Store table name for restore/delete operations
        _dgvTrash.Tag = tableName;
    }
    
    private void BtnRestore_Click(object? sender, EventArgs e)
    {
        if (_dgvTrash.SelectedRows.Count == 0)
        {
            ToastNotification.ShowWarning("Please select an item to restore");
            return;
        }
        
        var result = MessageBox.Show(
            "Are you sure you want to restore this item?\n\nIt will be available again in the system.",
            "Confirm Restore",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            var row = _dgvTrash.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string tableName = _dgvTrash.Tag?.ToString() ?? "";
            string idColumn = GetIdColumnName(tableName);
            
            // Restore: set is_deleted = 0, deleted_at = NULL
            string sql = $"UPDATE {tableName} SET is_deleted = 0, deleted_at = NULL WHERE {idColumn} = @id";
            DatabaseManager.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
            
            ToastNotification.ShowSuccess($"Item restored successfully!");
            LoadTrashItems();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to restore item");
            ToastNotification.ShowError($"Failed to restore item: {ex.Message}");
        }
    }
    
    private void BtnPermanentDelete_Click(object? sender, EventArgs e)
    {
        if (_dgvTrash.SelectedRows.Count == 0)
        {
            ToastNotification.ShowWarning("Please select an item to delete");
            return;
        }
        
        var result = MessageBox.Show(
            "⚠️ WARNING: This will PERMANENTLY delete the item!\n\n" +
            "This action CANNOT be undone. The data will be lost forever.\n\n" +
            "Are you absolutely sure you want to continue?",
            "Confirm Permanent Deletion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        
        if (result != DialogResult.Yes)
            return;
        
        try
        {
            var row = _dgvTrash.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string tableName = _dgvTrash.Tag?.ToString() ?? "";
            string idColumn = GetIdColumnName(tableName);
            
            // Permanent delete: actually remove from database
            string sql = $"DELETE FROM {tableName} WHERE {idColumn} = @id";
            DatabaseManager.ExecuteNonQuery(sql, new SQLiteParameter("@id", id));
            
            ToastNotification.ShowInfo("Item permanently deleted");
            LoadTrashItems();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to permanently delete item");
            ToastNotification.ShowError($"Failed to delete item: {ex.Message}");
        }
    }
    
    private string GetIdColumnName(string tableName)
    {
        return tableName switch
        {
            "sales" => "sale_id",
            "purchases" => "purchase_id",
            "maintenance" => "maintenance_id",
            "vehicles" => "vehicle_id",
            "buyers" => "buyer_id",
            "vendors" => "vendor_id",
            "materials" => "material_id",
            _ => "id"
        };
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.ClientSize = new Size(1000, 600);
        this.Name = "TrashViewerForm";
        this.Text = "Trash Bin";
        this.ResumeLayout(false);
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            Close();
            return true;
        }
        else if (keyData == Keys.F5)
        {
            LoadTrashItems();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}
