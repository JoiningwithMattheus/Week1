using System;
using System.Collections.Generic;

public class TextMenu : IMenu
{
    public override IList<string> OperationsHelp { get; set; }

    public TextMenu(IList<string> operationsHelp)
    {
        OperationsHelp = operationsHelp ?? new List<string>();
    }

    public override void ShowMenu()
    {
        Console.WriteLine("Enter an RPN expression to evaluate.");
        Console.WriteLine("Enter '(h)elp' for help.");
        Console.WriteLine("Enter '(o)ps' for available operations.");
        Console.WriteLine("Enter '(q)uit' to exit.");
    }

    public override void ShowOperations()
    {
        Console.WriteLine("Calculator operations:");
        if (OperationsHelp != null && OperationsHelp.Count > 0)
        {
            foreach (var operation in OperationsHelp)
            {
                Console.WriteLine($"{operation}");
            }
        }
        else
        {
            Console.WriteLine("No operations available.");
        }
    }

    public override void ShowHelp()
    {
        Console.WriteLine("Enter expressions using RPN notation, for instance to calculate:");
        Console.WriteLine(" 2 + 3 * 4");
        Console.WriteLine(" enter '2 3 4 * +'");
        Console.WriteLine("enter (o)ps to see available operations");
    }
}