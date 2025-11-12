namespace ConsoleAppSerialConnection;

using System;
using System.IO.Ports;

class Program
{
    const int BAUDRATE = 11500;
    const int MAX = 50;
    static void Main(string[] args)
    {
        var Program = new Program();
        var portNames = Program.PortNames;
        Console.WriteLine("Available Serial Outputs: ");
        foreach(var port in portNames)
        {
            Console.WriteLine()
        }
        Console.WriteLine("Hello, World!");
    }
}
