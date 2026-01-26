namespace CrushEase;

/// <summary>
/// Main application entry point
/// </summary>
static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        // Initialize logging
        Utils.Logger.Initialize();
        
        try
        {
            // Initialize database
            Data.DatabaseManager.Initialize();
            
            // Run main form
            Application.Run(new Forms.MainForm());
        }
        catch (Exception ex)
        {
            Utils.Logger.LogError(ex, "Fatal application error");
            MessageBox.Show(
                $"A critical error occurred:\n\n{ex.Message}\n\nPlease contact support.",
                "Application Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
