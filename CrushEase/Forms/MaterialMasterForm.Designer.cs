namespace CrushEase.Forms
{
    partial class MaterialMasterForm
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
            this.groupInput = new GroupBox();
            this.txtNotes = new TextBox();
            this.lblNotes = new Label();
            this.txtUnit = new TextBox();
            this.lblUnit = new Label();
            this.txtMaterialName = new TextBox();
            this.lblMaterialName = new Label();
            this.btnAdd = new Button();
            this.btnSave = new Button();
            
            this.groupGrid = new GroupBox();
            this.dgvMaterials = new DataGridView();
            this.txtSearch = new TextBox();
            this.lblSearch = new Label();
            this.chkShowInactive = new CheckBox();
            
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnClose = new Button();
            
            this.statusStrip = new StatusStrip();
            this.lblStatus = new ToolStripStatusLabel();
            
            this.groupInput.SuspendLayout();
            this.groupGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaterials)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            
            // groupInput
            this.groupInput.Controls.Add(this.btnSave);
            this.groupInput.Controls.Add(this.btnAdd);
            this.groupInput.Controls.Add(this.txtNotes);
            this.groupInput.Controls.Add(this.lblNotes);
            this.groupInput.Controls.Add(this.txtUnit);
            this.groupInput.Controls.Add(this.lblUnit);
            this.groupInput.Controls.Add(this.txtMaterialName);
            this.groupInput.Controls.Add(this.lblMaterialName);
            this.groupInput.Location = new Point(12, 12);
            this.groupInput.Name = "groupInput";
            this.groupInput.Size = new Size(760, 120);
            this.groupInput.TabIndex = 0;
            this.groupInput.TabStop = false;
            this.groupInput.Text = "Add New Material";
            
            // lblMaterialName
            this.lblMaterialName.AutoSize = true;
            this.lblMaterialName.Location = new Point(15, 30);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new Size(90, 15);
            this.lblMaterialName.TabIndex = 0;
            this.lblMaterialName.Text = "Material Name:";
            
            // txtMaterialName
            this.txtMaterialName.Location = new Point(120, 27);
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Size = new Size(250, 23);
            this.txtMaterialName.TabIndex = 1;
            
            // lblUnit
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new Point(15, 60);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new Size(32, 15);
            this.lblUnit.TabIndex = 2;
            this.lblUnit.Text = "Unit:";
            
            // txtUnit
            this.txtUnit.Location = new Point(120, 57);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new Size(150, 23);
            this.txtUnit.TabIndex = 3;
            
            // lblNotes
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new Point(400, 30);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(41, 15);
            this.lblNotes.TabIndex = 4;
            this.lblNotes.Text = "Notes:";
            
            // txtNotes
            this.txtNotes.Location = new Point(450, 27);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(290, 53);
            this.txtNotes.TabIndex = 5;
            
            // btnAdd
            this.btnAdd.Location = new Point(560, 86);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(80, 25);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Clear";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);
            
            // btnSave
            this.btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSave.Location = new Point(660, 86);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(80, 25);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);
            
            // groupGrid
            this.groupGrid.Controls.Add(this.chkShowInactive);
            this.groupGrid.Controls.Add(this.lblSearch);
            this.groupGrid.Controls.Add(this.txtSearch);
            this.groupGrid.Controls.Add(this.dgvMaterials);
            this.groupGrid.Location = new Point(12, 138);
            this.groupGrid.Name = "groupGrid";
            this.groupGrid.Size = new Size(760, 340);
            this.groupGrid.TabIndex = 1;
            this.groupGrid.TabStop = false;
            this.groupGrid.Text = "Materials List";
            
            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new Point(15, 25);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(45, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search:";
            
            // txtSearch
            this.txtSearch.Location = new Point(70, 22);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(250, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);
            
            // chkShowInactive
            this.chkShowInactive.AutoSize = true;
            this.chkShowInactive.Location = new Point(340, 24);
            this.chkShowInactive.Name = "chkShowInactive";
            this.chkShowInactive.Size = new Size(100, 19);
            this.chkShowInactive.TabIndex = 2;
            this.chkShowInactive.Text = "Show Inactive";
            this.chkShowInactive.UseVisualStyleBackColor = true;
            this.chkShowInactive.CheckedChanged += new EventHandler(this.ChkShowInactive_CheckedChanged);
            
            // dgvMaterials
            this.dgvMaterials.AllowUserToAddRows = false;
            this.dgvMaterials.AllowUserToDeleteRows = false;
            this.dgvMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMaterials.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaterials.Location = new Point(15, 55);
            this.dgvMaterials.Name = "dgvMaterials";
            this.dgvMaterials.ReadOnly = true;
            this.dgvMaterials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMaterials.Size = new Size(730, 270);
            this.dgvMaterials.TabIndex = 3;
            
            // btnEdit
            this.btnEdit.Location = new Point(532, 490);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(75, 30);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new EventHandler(this.BtnEdit_Click);
            
            // btnDelete
            this.btnDelete.Location = new Point(618, 490);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(75, 30);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.BtnDelete_Click);
            
            // btnClose
            this.btnClose.Location = new Point(704, 490);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(75, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            
            // statusStrip
            this.statusStrip.Items.AddRange(new ToolStripItem[] { this.lblStatus });
            this.statusStrip.Location = new Point(0, 535);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(784, 22);
            this.statusStrip.TabIndex = 5;
            
            // lblStatus
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(39, 17);
            this.lblStatus.Text = "Ready";
            
            // MaterialMasterForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(784, 557);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.groupGrid);
            this.Controls.Add(this.groupInput);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialMasterForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Material Master";
            this.Load += new EventHandler(this.MaterialMasterForm_Load);
            
            this.groupInput.ResumeLayout(false);
            this.groupInput.PerformLayout();
            this.groupGrid.ResumeLayout(false);
            this.groupGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaterials)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private GroupBox groupInput;
        private Label lblMaterialName;
        private TextBox txtMaterialName;
        private Label lblUnit;
        private TextBox txtUnit;
        private Label lblNotes;
        private TextBox txtNotes;
        private Button btnAdd;
        private Button btnSave;
        
        private GroupBox groupGrid;
        private Label lblSearch;
        private TextBox txtSearch;
        private CheckBox chkShowInactive;
        private DataGridView dgvMaterials;
        
        private Button btnEdit;
        private Button btnDelete;
        private Button btnClose;
        
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
    }
}
