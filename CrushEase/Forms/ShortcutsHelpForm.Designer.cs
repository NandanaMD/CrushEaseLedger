using System.Drawing;
using System.Windows.Forms;

namespace CrushEase.Forms
{
    partial class ShortcutsHelpForm
    {
        private System.ComponentModel.IContainer components = null;
        private ListView lvShortcuts;
        private ColumnHeader colKey;
        private ColumnHeader colDescription;
        private Button btnClose;
        private Label lblTitle;

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
            this.lvShortcuts = new ListView();
            this.colKey = new ColumnHeader();
            this.colDescription = new ColumnHeader();
            this.btnClose = new Button();
            this.lblTitle = new Label();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.Location = new Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(560, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "⌨️ Keyboard Shortcuts";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            
            // lvShortcuts
            this.lvShortcuts.Columns.AddRange(new ColumnHeader[] {
                this.colKey,
                this.colDescription
            });
            this.lvShortcuts.FullRowSelect = true;
            this.lvShortcuts.GridLines = true;
            this.lvShortcuts.Location = new Point(12, 50);
            this.lvShortcuts.Name = "lvShortcuts";
            this.lvShortcuts.Size = new Size(560, 500);
            this.lvShortcuts.TabIndex = 1;
            this.lvShortcuts.UseCompatibleStateImageBehavior = false;
            this.lvShortcuts.View = View.Details;
            this.lvShortcuts.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            
            // colKey
            this.colKey.Text = "Shortcut Key";
            this.colKey.Width = 150;
            
            // colDescription
            this.colDescription.Text = "Action";
            this.colDescription.Width = 400;
            
            // btnClose
            this.btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnClose.Location = new Point(237, 565);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(110, 35);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close (Esc)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            
            // ShortcutsHelpForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(584, 612);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvShortcuts);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShortcutsHelpForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Keyboard Shortcuts - CrushEase Ledger";
            this.ShowIcon = false;
            this.ResumeLayout(false);
        }
    }
}
