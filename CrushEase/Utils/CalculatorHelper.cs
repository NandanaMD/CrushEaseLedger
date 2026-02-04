namespace CrushEase.Utils;

/// <summary>
/// Floating calculator helper for quick calculations during data entry
/// </summary>
public partial class CalculatorHelper : Form
{
    private TextBox _displayTextBox;
    private double _currentValue = 0;
    private double _storedValue = 0;
    private string _currentOperation = "";
    private bool _isNewEntry = true;
    
    public CalculatorHelper()
    {
        InitializeComponent();
        SetupCalculator();
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        this.StartPosition = FormStartPosition.Manual;
        this.TopMost = true;
        this.ShowInTaskbar = false;
        this.Size = new Size(250, 320);
        this.Text = "Calculator";
        this.BackColor = Color.FromArgb(245, 247, 250);
        this.KeyPreview = true;
        this.KeyPress += CalculatorHelper_KeyPress;
        this.KeyDown += CalculatorHelper_KeyDown;
        
        // Position at top-right of screen
        var screen = Screen.PrimaryScreen.WorkingArea;
        this.Location = new Point(screen.Right - this.Width - 20, 100);
    }
    
    private void SetupCalculator()
    {
        // Display
        _displayTextBox = new TextBox
        {
            Location = new Point(10, 10),
            Size = new Size(210, 30),
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = HorizontalAlignment.Right,
            ReadOnly = true,
            Text = "0"
        };
        this.Controls.Add(_displayTextBox);
        
        // Number and operation buttons
        string[] buttonTexts = {
            "7", "8", "9", "÷",
            "4", "5", "6", "×",
            "1", "2", "3", "-",
            "0", ".", "=", "+",
            "C", "±", "%", "Use"
        };
        
        int row = 0, col = 0;
        foreach (var text in buttonTexts)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(10 + col * 55, 50 + row * 50),
                Size = new Size(50, 45),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = IsOperation(text) ? Color.FromArgb(8, 145, 178) : Color.White,
                ForeColor = IsOperation(text) ? Color.White : Color.FromArgb(55, 65, 81),
                Cursor = Cursors.Hand
            };
            
            btn.FlatAppearance.BorderColor = Color.FromArgb(229, 231, 235);
            btn.Click += ButtonClick;
            this.Controls.Add(btn);
            
            col++;
            if (col >= 4)
            {
                col = 0;
                row++;
            }
        }
    }
    
    private bool IsOperation(string text)
    {
        return text == "+" || text == "-" || text == "×" || text == "÷" || 
               text == "=" || text == "C" || text == "%" || text == "±" || text == "Use";
    }
    
    private void ButtonClick(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        
        string btnText = btn.Text;
        
        switch (btnText)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                HandleNumber(btnText);
                break;
            case ".":
                HandleDecimal();
                break;
            case "+":
            case "-":
            case "×":
            case "÷":
                HandleOperation(btnText);
                break;
            case "=":
                HandleEquals();
                break;
            case "C":
                HandleClear();
                break;
            case "±":
                HandlePlusMinus();
                break;
            case "%":
                HandlePercent();
                break;
            case "Use":
                HandleUseResult();
                break;
        }
    }
    
    private void HandleNumber(string number)
    {
        if (_isNewEntry)
        {
            _displayTextBox.Text = number;
            _isNewEntry = false;
        }
        else
        {
            _displayTextBox.Text += number;
        }
    }
    
    private void HandleDecimal()
    {
        if (!_displayTextBox.Text.Contains("."))
        {
            _displayTextBox.Text += ".";
            _isNewEntry = false;
        }
    }
    
    private void HandleOperation(string operation)
    {
        if (!string.IsNullOrEmpty(_currentOperation))
        {
            HandleEquals();
        }
        else
        {
            _storedValue = double.Parse(_displayTextBox.Text);
        }
        
        _currentOperation = operation;
        _isNewEntry = true;
    }
    
    private void HandleEquals()
    {
        if (string.IsNullOrEmpty(_currentOperation)) return;
        
        double displayValue = double.Parse(_displayTextBox.Text);
        double result = 0;
        
        switch (_currentOperation)
        {
            case "+":
                result = _storedValue + displayValue;
                break;
            case "-":
                result = _storedValue - displayValue;
                break;
            case "×":
                result = _storedValue * displayValue;
                break;
            case "÷":
                result = displayValue != 0 ? _storedValue / displayValue : 0;
                break;
        }
        
        _displayTextBox.Text = result.ToString("0.##########");
        _currentValue = result;
        _currentOperation = "";
        _isNewEntry = true;
    }
    
    private void HandleClear()
    {
        _displayTextBox.Text = "0";
        _currentValue = 0;
        _storedValue = 0;
        _currentOperation = "";
        _isNewEntry = true;
    }
    
    private void HandlePlusMinus()
    {
        double value = double.Parse(_displayTextBox.Text);
        _displayTextBox.Text = (-value).ToString();
    }
    
    private void HandlePercent()
    {
        double value = double.Parse(_displayTextBox.Text);
        _displayTextBox.Text = (value / 100).ToString();
    }
    
    private void CalculatorHelper_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // Handle number keys
        if (char.IsDigit(e.KeyChar))
        {
            HandleNumber(e.KeyChar.ToString());
            e.Handled = true;
        }
        else if (e.KeyChar == '.')
        {
            HandleDecimal();
            e.Handled = true;
        }
        else if (e.KeyChar == '+')
        {
            HandleOperation("+");
            e.Handled = true;
        }
        else if (e.KeyChar == '-')
        {
            HandleOperation("-");
            e.Handled = true;
        }
        else if (e.KeyChar == '*')
        {
            HandleOperation("×");
            e.Handled = true;
        }
        else if (e.KeyChar == '/')
        {
            HandleOperation("÷");
            e.Handled = true;
        }
        else if (e.KeyChar == '=' || e.KeyChar == '\r')
        {
            HandleEquals();
            e.Handled = true;
        }
        else if (e.KeyChar == '%')
        {
            HandlePercent();
            e.Handled = true;
        }
    }
    
    private void CalculatorHelper_KeyDown(object? sender, KeyEventArgs e)
    {
        // Handle special keys
        if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete)
        {
            HandleClear();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Back)
        {
            if (_displayTextBox.Text.Length > 1)
            {
                _displayTextBox.Text = _displayTextBox.Text.Substring(0, _displayTextBox.Text.Length - 1);
            }
            else
            {
                _displayTextBox.Text = "0";
            }
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Enter)
        {
            if (e.Control)
            {
                HandleUseResult();
            }
            else
            {
                HandleEquals();
            }
            e.Handled = true;
        }
    }
    
    private void HandleUseResult()
    {
        // Copy result to clipboard
        Clipboard.SetText(_displayTextBox.Text);
        
        // Try to paste into the active control of the owner form
        if (Owner != null)
        {
            var activeControl = GetActiveControl(Owner);
            if (activeControl is TextBox textBox)
            {
                textBox.Text = _displayTextBox.Text;
                textBox.Focus();
                textBox.SelectAll();
            }
        }
        
        ToastNotification.ShowSuccess($"Result copied: {_displayTextBox.Text}");
    }
    
    private Control? GetActiveControl(Form form)
    {
        var container = form as IContainerControl;
        while (container != null)
        {
            if (container.ActiveControl == null)
                break;
            
            if (container.ActiveControl is TextBox)
                return container.ActiveControl;
            
            container = container.ActiveControl as IContainerControl;
        }
        return null;
    }
    
    /// <summary>
    /// Show calculator or bring to front if already open
    /// </summary>
    public static void ShowCalculator(Form owner)
    {
        // Check if calculator is already open
        var existingCalc = Application.OpenForms.OfType<CalculatorHelper>().FirstOrDefault();
        if (existingCalc != null)
        {
            existingCalc.BringToFront();
            existingCalc.Focus();
            return;
        }
        
        var calc = new CalculatorHelper();
        calc.Owner = owner;
        calc.Show();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.ClientSize = new Size(250, 320);
        this.Name = "CalculatorHelper";
        this.ResumeLayout(false);
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // Handle keyboard shortcuts
        if (keyData == Keys.Escape)
        {
            this.Close();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}
