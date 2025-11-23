namespace ConsoleAppRPN;

class Program
{
    static void Main(string[] args)
    {
        // Setup dependencies
        var supportedOperators = new List<string> { "+", "-", "*", "/", "sqrt" };
        var parser = new Parser(supportedOperators);
        var calculator = new RPNCalculator();
        var menu = new TextMenu(calculator);
        
        // Create and run the controller
        var controller = new Controller(calculator, parser, menu);
        controller.Run();
    }
}
