using System;
using System.IO.Ports;
using System.Threading;

namespace MyAnalogDeck.WorkerService
{
    public class MyAnalogDeckService
    {
        public string? DataFromDeck { get; private set; }

        private static SerialPort? _serialPort;

        private readonly ILogger<Worker> _logger;

        public MyAnalogDeckService(ILogger<Worker> logger)
        {
            _logger = logger;
            
            _serialPort = new SerialPort("COM3", 9600);
            
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(GetDataFromDeck);
        }

        private void GetDataFromDeck(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            DataFromDeck = sp.ReadExisting();

            Console.WriteLine(DataFromDeck);

            //_logger.LogInformation("My Analog Deck está na porta {0}", myAnalogDeck.DataFromDeck);
        }

        public string GetDeckPort()
        {
            var portNames = SerialPort.GetPortNames();

            foreach (var port in portNames)
            {
                _serialPort = new SerialPort(port, 9600);
                _serialPort.Open();
                _serialPort.WriteLine("Who are you?");

                //var response = _serialPort.ReadExisting();

                if (DataFromDeck.Contains("MyAnalogDeck"))
                {
                    Console.WriteLine(DataFromDeck);
                    //_serialPort.Close();
                    //return port;
                }
            }
            //_serialPort.Close();
            return "0";

        }
    }
}
