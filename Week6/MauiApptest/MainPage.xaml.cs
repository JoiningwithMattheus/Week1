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

    // Calculator classes (maybe make these seperate files later)

    public abstract class ICalculator
    {
        public virtual IList<string> SupportedOperators { get; } = new List<string>();
        public virtual IList<string> OperationsHelpText { get; } = new List<string>();

        public virtual double Calculate(IList<Token> express)
        {
            throw new NotImplementedException();
        }
    }

    public class RPNCalculator : ICalculator
    {
        public override IList<string> OperationsHelpText { get; }
        public override IList<string> SupportedOperators { get; }
        
        public RPNCalculator()
        {
            OperationsHelpText = new List<string> {
                "+ - (Addition) adds two numbers",
                "- - (Subtraction) subtracts two numbers",
                "* - (Multiplication) multiplies two numbers",
                "/ - (Division) calculates the fraction of two numbers",
                "^ - (Power) calculates the power of two numbers",
                "sqrt - (SquareRoot) calculates the square root of a number",
                "exp - (Exponentiation) calculates the exponent with the natural base e",
                "ln - (Logarithm) calculates the natural logarithm of a number"
            };
            
            SupportedOperators = new List<string> {
                "+", "-", "*", "/", "^", "sqrt", "exp", "ln"
            };
        }
        
        public override double Calculate(IList<Token> express)
        {
            var stack = new Stack<double>();
            
            foreach (var token in express)
            {
                if (token.Type == TokenType.Number)
                {
                    stack.Push(token.NumericValue);
                    continue;
                }
                
                string op = token.Value;

                if (op == "+" || op == "-" || op == "*" || op == "/" || op == "^")
                {
                    if (stack.Count < 2)
                    {
                        throw new FormatException($"Operator '{op}' needs at least two operands!");
                    }
                    
                    double b = stack.Pop();
                    double a = stack.Pop();
                    
                    switch (op)
                    {
                        case "+": stack.Push(a + b); break;
                        case "-": stack.Push(a - b); break;
                        case "*": stack.Push(a * b); break;
                        case "/": stack.Push(a / b); break;
                        case "^": stack.Push(Math.Pow(a, b)); break;
                    }
                    continue;
                }
                else
                {
                    if (stack.Count < 1)
                    {
                        throw new FormatException($"Operator '{op}' needs at least one operand!");
                    }
                    
                    double a = stack.Pop();
                    
                    switch (op)
                    {
                        case "sqrt": stack.Push(Math.Sqrt(a)); break;
                        case "exp": stack.Push(Math.Exp(a)); break;
                        case "ln": stack.Push(Math.Log(a)); break;
                    }
                    continue;
                }
                
                throw new FormatException($"Unsupported Operator '{op}'");
            }
            
            if (stack.Count != 1)
            {
                throw new FormatException("Invalid expression! Too many or too few numbers.");
            }
            
            return stack.Pop();
        }
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public double NumericValue { get; }
        
        public Token(string s, TokenType type)
        {
            Type = type;
            Value = s;
            
            if (Type == TokenType.Number)
            {
                NumericValue = double.Parse(s);
            }
            else
            {
                NumericValue = double.NaN;
            }
        }
    }

    public enum TokenType
    {
        Number,
        Operator
    }
}