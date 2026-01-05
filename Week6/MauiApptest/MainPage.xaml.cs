using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MauiApptest
{
    public partial class MainPage : ContentPage
    {
        private RPNCalculator calculator = new RPNCalculator();

        public MainPage()
        {
            InitializeComponent();
        }

        private void OperatorsButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ShowResult("", true);
                foreach (var line in calculator.OperationsHelpText)
                {
                    ResultLabel.Text += line + "\n";
                }
            }
        }

        private void ShowHelp_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ShowResult("", true);
                ResultLabel.Text += "Enter expressions using RPN notation, for instance to calculate:\n";
                ResultLabel.Text += " 2 + 3 * 4\n";
                ResultLabel.Text += " enter '2 3 4 * +' \n";
                ResultLabel.Text += "enter (o)ps to see available operations";
            }
        }

        private void NumberButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string number = button.Text;
                DisplayEntry.Text += number;
                HideResult();
            }
        }

        private void DecimalButton_Clicked(object sender, EventArgs e)
        {
            DisplayEntry.Text += ",";
            HideResult();
        }

        private void OperatorButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string op = button.Text;
                DisplayEntry.Text += " " + op + " ";
                HideResult();
            }
        }

        private void FunctionButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string func = button.Text;
                DisplayEntry.Text += " " + func + " ";
                HideResult();
            }
        }

        private void SpaceButton_Clicked(object sender, EventArgs e)
        {
            DisplayEntry.Text += " ";
            HideResult();
        }

        private void BackspaceButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DisplayEntry.Text))
            {
                DisplayEntry.Text = DisplayEntry.Text.Substring(0, DisplayEntry.Text.Length - 1);
                HideResult();
            }
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            DisplayEntry.Text = "";
            HideResult();
        }

        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string expression = DisplayEntry.Text.Trim();
                if (string.IsNullOrEmpty(expression))
                {
                    ShowResult("Enter an expression first", false);
                    return;
                }

                var tokens = ParseExpression(expression);
                double result = calculator.Calculate(tokens);

                ShowResult($"Result: {result}", true);
                DisplayEntry.Text = $"{result}";
            }
            catch (FormatException ex)
            {
                ShowResult($"Error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                ShowResult($"Error: {ex.Message}", false);
            }
        }

        private List<Token> ParseExpression(string expression)
        {
            var tokens = new List<Token>();
            var parts = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (double.TryParse(part, out double number))
                {
                    tokens.Add(new Token(part, TokenType.Number));
                }
                else if (calculator.SupportedOperators.Contains(part))
                {
                    tokens.Add(new Token(part, TokenType.Operator));
                }
                else
                {
                    throw new FormatException($"Invalid token: '{part}'");
                }
            }

            return tokens;
        }

        private void ShowResult(string message, bool isSuccess)
        {
            ResultLabel.Text = message;
            ResultLabel.TextColor = isSuccess ? Colors.Green : Colors.Red;
            ResultLabel.IsVisible = true;
        }

        private void HideResult()
        {
            ResultLabel.IsVisible = false;
        }
    }
}