public abstract class IParser
{
    public abstract IList<string> SupportedOperators { get; set; }

    public abstract IList<string> Tokenize(string expression);
    public abstract IList<Token> Lex(IList<string> expression);
}