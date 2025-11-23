class ICalculator
{
    public IList<string> SupportedOperators { get; }
    public IList<string> OperationsHelpText { get; }

    public double Calculate(IList<Token> express);
}