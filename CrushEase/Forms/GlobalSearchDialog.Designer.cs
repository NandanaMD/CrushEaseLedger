namespace CrushEase.Forms
{
    partial class GlobalSearchDialog
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnCancel;
        private Label lblHint;

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
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            this.btnCancel = new Button();
            this.lblHint = new Label();
            
            this.SuspendLayout();
            
            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblSearch.Location = new Point(20, 20);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(150, 19);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search Transactions";
            
            // txtSearch
            this.txtSearch.Font = new Font("Segoe UI", 11F);
            this.txtSearch.Location = new Point(20, 50);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Enter search text...";
            this.txtSearch.Size = new Size(360, 27);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyDown += new KeyEventHandler(this.TxtSearch_KeyDown);
            
            // lblHint
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new Font("Segoe UI", 8F);
            this.lblHint.ForeColor = SystemColors.GrayText;
            this.lblHint.Location = new Point(20, 85);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new Size(250, 13);
            this.lblHint.TabIndex = 2;
            this.lblHint.Text = "Press Enter to search, Esc to cancel";
            
            // btnSearch
            this.btnSearch.Font = new Font("Segoe UI", 9F);
            this.btnSearch.Location = new Point(160, 115);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(100, 32);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);
            
            // btnCancel
            this.btnCancel.Font = new Font("Segoe UI", 9F);
            this.btnCancel.Location = new Point(270, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(100, 32);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);
            
            // GlobalSearchDialog
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 165);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GlobalSearchDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Global Search";
            this.Load += new EventHandler(this.GlobalSearchDialog_Load);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
