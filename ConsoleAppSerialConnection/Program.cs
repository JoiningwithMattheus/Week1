using System;
using System.IO.Ports;

class Program
{
    const int BAUDRATE = 115200;
    private SerialPort _serialPort;

    static void Main(string[] args)
    {
        //
        // Setup
        //
        var program = new Program();

        // List available serial ports.
        var portNames = program.PortNames;
        Console.WriteLine("Available Serial Ports:");
        foreach (var port in portNames)
        {
            Console.WriteLine(port);
        }

        if (portNames.Length > 0)
        {
            string portToUse = "COM3";

            Console.WriteLine($"Opening port \"{portToUse}\"");

            try
            {
                program.OpenConnection(portToUse, BAUDRATE);

                bool stop = false;

                while (!stop)
                {
                    stop = !program.Run();

                    if (!stop && Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        char key = keyInfo.KeyChar;

                        if (key == 'o')
                        {
                            program._serialPort.WriteLine("ledOn");
                        }
                        else if (key == 'f')
                        {
                            program._serialPort.WriteLine("ledOff");
                        }

                        else if (key == 'h')
                        {
                            Console.WriteLine("Commands: o=LED on, f=LED off, h=help, q=quit");
                        }

                        // Quit program
                        stop = (key == 'q' || key == 'Q' || keyInfo.Key == ConsoleKey.Escape);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("No serial ports found.");
        }
    }


    Program()
    {
        _serialPort = new SerialPort();
    }

    ~Program()
    {
        CloseConnection();
    }

    public string[] PortNames
    {
        get { return SerialPort.GetPortNames(); }
    }

    public void OpenConnection(string portName, int baudRate)
    {
        CloseConnection();

        _serialPort.PortName = portName;
        _serialPort.BaudRate = baudRate;
        _serialPort.DtrEnable = true;
        _serialPort.RtsEnable = true;

        _serialPort.NewLine = "\n";   // IMPORTANT for Pico
        _serialPort.Open();

        if (_serialPort.BytesToRead > 0)
        {
            _serialPort.DiscardInBuffer();
        }
    }

    public void CloseConnection()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }

    public bool Run()
    {
        if (_serialPort.BytesToRead > 0)
        {
            string message = _serialPort.ReadLine().Trim();

            int number;
            if (int.TryParse(message, out number))
            {
                Console.WriteLine($"Message = {message}, number = {number}");
                return true;
            }
            return false;
        }
        return true;
    }
}
