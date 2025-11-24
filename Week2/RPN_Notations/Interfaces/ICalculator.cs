public abstract class ICalculator
{
    public virtual IList<string> SupportedOperators { get; } = new List<string>();
    public virtual IList<string> OperationsHelpText { get; } = new List<string>();

    public virtual double Calculate(IList<Token> express)
    {
        throw new NotImplementedException();
    }
}