public abstract class ICalculator
{
    public IList<string> SupportedOperators { get; }
    public IList<string> OperationsHelpText { get; }

    public abstract double Calculate(IList<Token> express);
}