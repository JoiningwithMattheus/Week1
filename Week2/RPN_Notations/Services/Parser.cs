public class Parser : IParser
{
    public override IList<string> SupportedOperators {get; set;}

    public Parser()
    {
        SupportedOperators = new List<string>();
    }

    public Parser(RPNCalculator calculator)
    {
        SupportedOperators = calculator.SupportedOperators ?? new List<string>();
    }

    public override IList<string> Tokenize(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return new List<string>();
        }
        
        return expression.Split(' ').ToList();
    }

    public override IList<Token> Lex(IList<string> tokens)
    {
        if (tokens == null)
        {
            throw new ArgumentNullException(nameof(tokens));
        }

        var result = new List<Token>();

        foreach (var token in tokens)
        {
            if (SupportedOperators.Contains(token))
            {
                result.Add(new Token(token, TokenType.Operator));
            }
            else if (double.TryParse(token, out double numericValue))
            {
                result.Add(new Token(token, TokenType.Number));
            }
            else
            {
                throw new FormatException($"Invalid token: '{token}'");
            }
        }

        return result;
    }
}