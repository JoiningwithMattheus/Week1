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
