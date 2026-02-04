using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrushEase.Forms
{
    public partial class ShortcutsHelpForm : Form
    {
        public ShortcutsHelpForm()
        {
            InitializeComponent();
            PopulateShortcuts();
        }

        private void PopulateShortcuts()
        {
            // Add shortcuts to the list view
            AddShortcutCategory("Primary Transaction Entries");
            AddShortcut("F2", "New Sale Entry");
            AddShortcut("F3", "New Purchase Entry");
            AddShortcut("F4", "New Maintenance Entry");
            
            AddShortcutCategory("Master Data Management");
            AddShortcut("Ctrl+Shift+V", "Manage Vehicles");
            AddShortcut("Ctrl+Shift+D", "Manage Vendors");
            AddShortcut("Ctrl+Shift+B", "Manage Buyers");
            AddShortcut("Ctrl+Shift+M", "Manage Materials");
            
            AddShortcutCategory("Quick Views");
            AddShortcut("Ctrl+F", "Global Search");
            AddShortcut("Ctrl+T", "View All Transactions");
            AddShortcut("Ctrl+D", "View Master Data");
            
            AddShortcutCategory("Reports");
            AddShortcut("Ctrl+R", "Sales Report");
            AddShortcut("Ctrl+Shift+R", "Purchase Report");
            AddShortcut("Ctrl+M", "Maintenance Report");
            AddShortcut("Ctrl+P", "Vehicle Profit Summary");
            AddShortcut("Ctrl+E", "Export All Data to Excel");
            
            AddShortcutCategory("File Operations");
            AddShortcut("Ctrl+B", "Backup Now");
            AddShortcut("Ctrl+Shift+O", "Restore from Backup");
            AddShortcut("Alt+F4", "Exit Application");
            
            AddShortcutCategory("Tools");
            AddShortcut("Ctrl+K", "Calculator");
            AddShortcut("Ctrl+Shift+T", "Trash / Deleted Items");
            AddShortcut("Ctrl+L", "Lock Application");
            
            AddShortcutCategory("Form Operations");
            AddShortcut("Ctrl+S", "Save/Submit Entry");
            AddShortcut("Ctrl+N", "New Entry (after save)");
            AddShortcut("F5", "Refresh/Reset Form");
            AddShortcut("Escape", "Cancel/Close Form");
            
            AddShortcutCategory("Navigation");
            AddShortcut("↑/↓", "Move Up/Down between fields");
            AddShortcut("Tab", "Move to next field");
            AddShortcut("Shift+Tab", "Move to previous field");
            
            AddShortcutCategory("Help");
            AddShortcut("F1", "Show this help dialog");
            
            // Auto-resize columns
            lvShortcuts.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvShortcuts.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void AddShortcutCategory(string category)
        {
            var item = new ListViewItem(new[] { "", "" })
            {
                Font = new Font(lvShortcuts.Font, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                BackColor = Color.LightGray
            };
            item.SubItems[0].Text = category;
            lvShortcuts.Items.Add(item);
        }

        private void AddShortcut(string keys, string description)
        {
            var item = new ListViewItem(new[] { keys, description });
            lvShortcuts.Items.Add(item);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Allow Escape to close the dialog
            if (keyData == Keys.Escape || keyData == Keys.F1)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
