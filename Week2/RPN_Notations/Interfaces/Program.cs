using System;
namespace RPN_Notations;

class Program
{
    static void Main(string[] args)
    {
        // Setup dependencies
        var calculator = new RPNCalculator();
        var menu = new TextMenu(calculator);
        var parser = new Parser(calculator);
        
        // Create and run the controller
        var controller = new Controller(calculator, parser, menu);
        controller.Run();
    }
}




