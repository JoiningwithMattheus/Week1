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
            "sqrt","cbrt","exp","ln","log10",
            "sin","cos","tan","asin","acos","atan",
            "sinh","cosh","tanh",
            "abs","ceil","floor","round",
            "min","max","fact",
            "pi","e","deg","rad"
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

            // Zero-operand operators (constants)
            if (op == "pi")
            {
                stack.Push(Math.PI);
                continue;
            }
            if (op == "e")
            {
                stack.Push(Math.E);
                continue;
            }

            // Binary operators
            if (op == "+" || op == "-" || op == "*" || op == "/" || op == "^" ||
                op == "pow" || op == "mod" || op == "min" || op == "max")
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
                    case "min":
                        stack.Push(Math.Min(a, b));
                        break;
                    case "max":
                        stack.Push(Math.Max(a, b));
                        break;
                }
                continue;
            }

            // Unary operators
            if (op == "sqrt" || op == "cbrt" || op == "exp" || op == "ln" || op == "log10" ||
                op == "sin" || op == "cos" || op == "tan" || op == "asin" || op == "acos" || op == "atan" ||
                op == "sinh" || op == "cosh" || op == "tanh" ||
                op == "abs" || op == "ceil" || op == "floor" || op == "round" ||
                op == "fact" || op == "deg" || op == "rad")
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
                    case "cbrt":
                        // Proper cube-root for negative numbers
                        stack.Push(Math.Sign(a) * Math.Pow(Math.Abs(a), 1.0 / 3.0));
                        break;
                    case "exp":
                        stack.Push(Math.Exp(a));
                        break;
                    case "ln":
                        if (a <= 0) throw new FormatException("Natural logarithm requires a positive operand.");
                        stack.Push(Math.Log(a));
                        break;
                    case "log10":
                        if (a <= 0) throw new FormatException("Log base 10 requires a positive operand.");
                        stack.Push(Math.Log10(a));
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
                    case "sinh":
                        stack.Push(Math.Sinh(a));
                        break;
                    case "cosh":
                        stack.Push(Math.Cosh(a));
                        break;
                    case "tanh":
                        stack.Push(Math.Tanh(a));
                        break;
                    case "abs":
                        stack.Push(Math.Abs(a));
                        break;
                    case "ceil":
                        stack.Push(Math.Ceiling(a));
                        break;
                    case "floor":
                        stack.Push(Math.Floor(a));
                        break;
                    case "round":
                        stack.Push(Math.Round(a));
                        break;
                    case "fact":
                        {
                            // Factorial: only for non-negative integers
                            if (a < 0) throw new FormatException("Factorial requires a non-negative operand.");
                            double intPart = Math.Floor(a);
                            if (Math.Abs(a - intPart) > 1e-12) // not integer
                                throw new FormatException("Factorial requires an integer operand.");

                            // compute factorial iteratively as double (watch for overflow)
                            double result = 1.0;
                            for (long i = 2; i <= (long)intPart; i++)
                            {
                                result *= i;
                                // Avoid infinite loop on overflow; result will be Infinity if too large
                                if (double.IsInfinity(result)) break;
                            }
                            stack.Push(result);
                        }
                        break;
                    case "deg":
                        // convert degrees -> radians
                        stack.Push(a * Math.PI / 180.0);
                        break;
                    case "rad":
                        // convert radians -> degrees
                        stack.Push(a * 180.0 / Math.PI);
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