public abstract class IMenu
{
    public virtual IList<string> OperationsHelp { get; set; }

    public abstract void ShowMenu();
    public abstract void ShowOperations();
    public abstract void ShowHelp();
}