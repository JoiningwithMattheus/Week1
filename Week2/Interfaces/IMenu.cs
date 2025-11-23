public class IMenu
{
    public virtual IList<string> OperationsHelp { get; set; }

    public virtual void ShowMenu();
    public virtual void ShowOperations();
    public virtual void ShowHelp();
}