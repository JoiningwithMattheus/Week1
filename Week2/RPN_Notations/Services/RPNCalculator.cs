public class RPNCalculator : ICalculator
{
    public override IList<string> OperationsHelpText { get; } =
       new List<string> {"+ - (Addition) adds two numbers",
        "- - (Subtraction) subtracts two numbers",
        "* - (Multiplication) multiplies two numbers",
        "/ - (Division) calculates the fraction of two numbers",
        "^ - (Power) calculates the power of two numbers",
        "sqrt - (SquareRoot) calculates the square root of a number",
        "exp - (Exponentiation) calculates the exponent with the natural base e",
        "ln - (Logarithm) calculates the natural logarithm of a number"};
    public override IList<string> SupportedOperators { get; } = new List<string> {
        "+", "-", "*", "/", "^", "sqrt", "exp", "ln"};
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
                    throw new FormatException($"Operator '{op}' needs at least two operands!!!");
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
                    throw new FormatException($"Operator '{op}' needs at least one operand!!!");
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
            throw new FormatException("Invalid Expresion!!!");
        }
        return stack.Pop();
    }
}