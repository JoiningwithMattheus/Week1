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

        private void HelperButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                DisplayLabel.Text = calculator.OperationsHelpText;
                HideResult();
            }
        }

        private void OperatorsButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                DisplayLabel.Text = calculator.SupportedOperators;
                HideResult();
            }
        }

        private void NumberButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string number = button.Text;
                DisplayLabel.Text += number;
                HideResult();
            }
        }

        private void DecimalButton_Clicked(object sender, EventArgs e)
        {
            DisplayLabel.Text += ",";
            HideResult();
        }

        private void OperatorButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string op = button.Text;
                DisplayLabel.Text += " " + op + " ";
                HideResult();
            }
        }

        private void FunctionButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string func = button.Text;
                DisplayLabel.Text += " " + func + " ";
                HideResult();
            }
        }

        private void SpaceButton_Clicked(object sender, EventArgs e)
        {
            DisplayLabel.Text += " ";
            HideResult();
        }

        private void BackspaceButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DisplayLabel.Text))
            {
                DisplayLabel.Text = DisplayLabel.Text.Substring(0, DisplayLabel.Text.Length - 1);
                HideResult();
            }
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            DisplayLabel.Text = "";
            HideResult();
        }

        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string expression = DisplayLabel.Text.Trim();
                if (string.IsNullOrEmpty(expression))
                {
                    ShowResult("Enter an expression first", false);
                    return;
                }

                var tokens = ParseExpression(expression);
                double result = calculator.Calculate(tokens);

                ShowResult($"Result: {result}", true);
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