using System.Drawing;
using System.Windows.Forms;

namespace CrushEase.Utils;

/// <summary>
/// Modern Neutral Theme - Consistent styling across all forms
/// </summary>
public static class ModernTheme
{
    // Color Palette
    public static readonly Color BackgroundColor = Color.FromArgb(245, 247, 250); // Soft gray
    public static readonly Color PanelBackgroundColor = Color.White;
    public static readonly Color TealAccent = Color.FromArgb(8, 145, 178);
    public static readonly Color TealAccentHover = Color.FromArgb(6, 120, 147); // Subtle darker on hover
    public static readonly Color LightGray = Color.FromArgb(243, 244, 246);
    public static readonly Color DarkGray = Color.FromArgb(55, 65, 81);
    public static readonly Color GridBorder = Color.FromArgb(229, 231, 235);
    public static readonly Color AlternateRow = Color.FromArgb(249, 250, 251);
    
    /// <summary>
    /// Apply modern theme to a form
    /// </summary>
    public static void ApplyToForm(Form form)
    {
        form.BackColor = BackgroundColor;
        
        // Apply to all controls recursively
        ApplyToControls(form.Controls);
    }
    
    private static void ApplyToControls(Control.ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            // Apply to GroupBoxes
            if (control is GroupBox groupBox)
            {
                groupBox.BackColor = PanelBackgroundColor;
                ApplyToControls(groupBox.Controls);
            }
            // Apply to Panels
            else if (control is Panel panel)
            {
                panel.BackColor = PanelBackgroundColor;
                ApplyToControls(panel.Controls);
            }
            // Apply to DataGridView
            else if (control is DataGridView dgv)
            {
                StyleDataGridView(dgv);
            }
            // Apply to Buttons
            else if (control is Button btn)
            {
                StyleButton(btn);
            }
            // Recursively apply to nested controls
            else if (control.HasChildren)
            {
                ApplyToControls(control.Controls);
            }
        }
    }
    
    /// <summary>
    /// Style a DataGridView with modern theme
    /// </summary>
    public static void StyleDataGridView(DataGridView dgv)
    {
        dgv.BackgroundColor = PanelBackgroundColor;
        dgv.DefaultCellStyle.BackColor = PanelBackgroundColor;
        dgv.AlternatingRowsDefaultCellStyle.BackColor = AlternateRow;
        dgv.GridColor = GridBorder;
        dgv.BorderStyle = BorderStyle.None;
        
        // Style headers
        dgv.ColumnHeadersDefaultCellStyle.BackColor = LightGray;
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = DarkGray;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.Font.FontFamily, dgv.Font.Size, FontStyle.Bold);
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = LightGray;
        dgv.EnableHeadersVisualStyles = false;
        
        // Style selection
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 254);
        dgv.DefaultCellStyle.SelectionForeColor = DarkGray;
    }
    
    /// <summary>
    /// Style a button with modern theme
    /// </summary>
    public static void StyleButton(Button btn)
    {
        // Check if button text suggests it's a primary action
        bool isPrimary = btn.Text.Contains("Add") || 
                        btn.Text.Contains("Save") || 
                        btn.Text.Contains("Generate") ||
                        btn.Text.Contains("Export") ||
                        btn.Text.Contains("Submit") ||
                        btn.Text.Contains("Create");
        
        if (isPrimary)
        {
            btn.BackColor = TealAccent;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            
            // Add hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = TealAccentHover;
            btn.MouseLeave += (s, e) => btn.BackColor = TealAccent;
        }
        else
        {
            btn.BackColor = Color.FromArgb(229, 231, 235); // Darker gray for better visibility
            btn.ForeColor = Color.FromArgb(31, 41, 55); // Darker text
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219); // Subtle border
            btn.Cursor = Cursors.Hand;
            
            // Add hover effect
            btn.MouseEnter += (s, e) => {
                btn.BackColor = Color.FromArgb(209, 213, 219);
                btn.FlatAppearance.BorderColor = Color.FromArgb(156, 163, 175);
            };
            btn.MouseLeave += (s, e) => {
                btn.BackColor = Color.FromArgb(229, 231, 235);
                btn.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
            };
        }
    }
    
    /// <summary>
    /// Style a primary button (teal)
    /// </summary>
    public static void StylePrimaryButton(Button btn)
    {
        btn.BackColor = TealAccent;
        btn.ForeColor = Color.White;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Cursor = Cursors.Hand;
        
        // Add hover effect
        btn.MouseEnter += (s, e) => btn.BackColor = TealAccentHover;
        btn.MouseLeave += (s, e) => btn.BackColor = TealAccent;
    }
    
    /// <summary>
    /// Style a secondary button (gray)
    /// </summary>
    public static void StyleSecondaryButton(Button btn)
    {
        btn.BackColor = Color.FromArgb(229, 231, 235); // Darker gray for better visibility
        btn.ForeColor = Color.FromArgb(31, 41, 55); // Darker text
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219); // Subtle border
        btn.Cursor = Cursors.Hand;
        
        // Add hover effect
        btn.MouseEnter += (s, e) => {
            btn.BackColor = Color.FromArgb(209, 213, 219);
            btn.FlatAppearance.BorderColor = Color.FromArgb(156, 163, 175);
        };
        btn.MouseLeave += (s, e) => {
            btn.BackColor = Color.FromArgb(229, 231, 235);
            btn.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
        };
    }
}
