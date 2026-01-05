public class RPNCalculator : ICalculator
{
    public override IList<string> OperationsHelpText { get; }
    public override IList<string> SupportedOperators { get; }

    public RPNCalculator()
    {
        OperationsHelpText = new List<string> {
                 "+    - (Addition)            adds two numbers",
            "-    - (Subtraction)         subtracts two numbers",
            "*    - (Multiplication)      multiplies two numbers",
            "/    - (Division)            divides two numbers (a / b)",
            "^    - (Power)               calculates a^b",
            "pow  - alias for ^ (Power)",
            "mod  - (Modulo)              a mod b (remainder)",
            "sqrt - (SquareRoot)          sqrt(a) - requires a >= 0",
            "exp  - (Exponential)         e^a",
            "ln   - (Natural Logarithm)   ln(a) - requires a > 0",
            "sin  - (Sine)                sine(a) (a in radians)",
            "cos  - (Cosine)              cosine(a) (a in radians)",
            "tan  - (Tangent)             tangent(a) (a in radians)",
            "asin - (ArcSine)             asin(a) - requires -1 <= a <= 1",
            "acos - (ArcCosine)           acos(a) - requires -1 <= a <= 1",
            "atan - (ArcTangent)          atan(a)",
            "e    - (Constant)            pushes e (Euler's number)"
            };

        SupportedOperators = new List<string> {
                "+","-","*","/","^","pow","mod",
            "sqrt","cbrt","exp","ln",
            "sin","cos","tan","asin","acos","atan","round","e"
            };
    }

    public override double Calculate(IList<Token> express)
    {
        if (express == null) throw new ArgumentNullException(nameof(express));

        var stack = new Stack<double>();

        foreach (var token in express)
        {
            if (token == null) continue;

            if (token.Type == TokenType.Number)
            {
                stack.Push(token.NumericValue);
                continue;
            }

            // Normalize operator token
            string op = (token.Value ?? string.Empty).Trim().ToLowerInvariant();

            if (op == "e")
            {
                stack.Push(Math.E);
                continue;
            }

            // Binary operators
            if (op == "+" || op == "-" || op == "*" || op == "/" || op == "^" ||
                op == "pow" || op == "mod")
            {
                if (stack.Count < 2)
                    throw new FormatException($"Operator '{op}' needs at least two operands.");

                double b = stack.Pop();
                double a = stack.Pop();

                switch (op)
                {
                    case "+": stack.Push(a + b); break;
                    case "-": stack.Push(a - b); break;
                    case "*": stack.Push(a * b); break;
                    case "/":
                        if (b == 0.0) throw new DivideByZeroException("Division by zero.");
                        stack.Push(a / b);
                        break;
                    case "^":
                    case "pow":
                        stack.Push(Math.Pow(a, b));
                        break;
                    case "mod":
                        if (b == 0.0) throw new DivideByZeroException("Modulo by zero.");
                        stack.Push(a % b);
                        break;
                }
                continue;
            }

            // Unary operators
            if (op == "sqrt" || op == "exp" || op == "ln" || op == "round" ||
                op == "sin" || op == "cos" || op == "tan" || op == "asin" || op == "acos" || op == "atan")
            {
                if (stack.Count < 1)
                    throw new FormatException($"Operator '{op}' needs at least one operand.");

                double a = stack.Pop();

                switch (op)
                {
                    case "sqrt":
                        if (a < 0) throw new FormatException("Square root of negative number is not supported.");
                        stack.Push(Math.Sqrt(a));
                        break;
                    case "exp":
                        stack.Push(Math.Exp(a));
                        break;
                    case "ln":
                        if (a <= 0) throw new FormatException("Natural logarithm requires a positive operand.");
                        stack.Push(Math.Log(a));
                        break;
                    case "sin":
                        stack.Push(Math.Sin(a));
                        break;
                    case "cos":
                        stack.Push(Math.Cos(a));
                        break;
                    case "tan":
                        stack.Push(Math.Tan(a));
                        break;
                    case "asin":
                        if (a < -1.0 || a > 1.0) throw new FormatException("asin requires operand in [-1, 1].");
                        stack.Push(Math.Asin(a));
                        break;
                    case "acos":
                        if (a < -1.0 || a > 1.0) throw new FormatException("acos requires operand in [-1, 1].");
                        stack.Push(Math.Acos(a));
                        break;
                    case "atan":
                        stack.Push(Math.Atan(a));
                        break;
                    case "round":
                        stack.Push(Math.Round(a));
                        break;
                }
                continue;
            }

            // If reached here, operator is unsupported
            throw new FormatException($"Unsupported operator '{op}'.");
        }

        if (stack.Count != 1)
        {
            throw new FormatException("Invalid expression: the stack did not end with a single result.");
        }

        return stack.Pop();
    }
}