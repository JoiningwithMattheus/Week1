public class Parser : IParser
{
    public IList<string> SupportedOperators { get; set; }

    public Parser()
    {
        SupportedOperators = new List<string>();
    }

    public Parser(IList<string> supportedOperators)
    {
        SupportedOperators = supportedOperators ?? new List<string>();
    }

    public override IList<string> Tokenize(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return new List<string>();
        }
        
        return expression.split(' ').toList();
    }

    public override IList<Token> Lex(IList<string> expression)
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
                result.Add(new Token(TokenType.Operator, token));
            }
            else if (double.TryParse(token, out double numericValue))
            {
                result.Add(new Token(TokenType.Number, token, numericValue));
            }
            else
            {
                throw new ArgumentException($"Invalid token: '{token}'");
            }
        }

        return result;
    }
}