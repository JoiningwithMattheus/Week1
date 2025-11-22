public class IParser
{
    public virtual IList<string> SupportedOperators { get; set; }

    public virtual IList<string> Tokenize(string expression);
    public virtual IList<Token> Lex(IList<string> expression);
}