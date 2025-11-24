public abstract class IMenu
{
    public abstract IList<string> OperationsHelp { get; set; }

    public abstract void ShowMenu();
    public abstract void ShowOperations();
    public abstract void ShowHelp();
}