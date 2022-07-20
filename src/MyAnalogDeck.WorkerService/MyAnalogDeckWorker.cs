using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using System.IO.Ports;

namespace MyAnalogDeck.WorkerService;
public class MyAnalogDeckWorker : BackgroundService
{
    private readonly ILogger<MyAnalogDeckWorker> _logger;
    private readonly Dictionary<string, string> _buttonsSettings;
    private string MyAnalogDeckPortName;
    private string portNameChecked;
    private bool portNameFound = false;


    public MyAnalogDeckWorker(ILogger<MyAnalogDeckWorker> logger, IConfiguration _configuration)
    {
        _logger = logger;

        _buttonsSettings = _configuration.GetSection("MyAnalogDeck").GetSection("ButtonsSettings")
            .GetChildren()
            .ToDictionary(x => x.Key, x => x.Value);

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(GetMyAnalogDeckPort);

        _logger.LogInformation("Running...");

        await Task.Delay(1000, stoppingToken);
    }

    public void GetMyAnalogDeckPort()
    {
        SerialPort _myAnalogDeckSerialPort = new SerialPort();

        while (true)
        {
            portNameFound = false;
            var portNames = SerialPort.GetPortNames();

            foreach (var port in portNames)
            {
                _myAnalogDeckSerialPort.PortName = port;
                _myAnalogDeckSerialPort.BaudRate = 9600;
                _myAnalogDeckSerialPort.Parity = Parity.None;
                _myAnalogDeckSerialPort.StopBits = StopBits.One;
                _myAnalogDeckSerialPort.DataBits = 8;
                _myAnalogDeckSerialPort.Handshake = Handshake.None;
                _myAnalogDeckSerialPort.RtsEnable = true;

                _myAnalogDeckSerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                try
                {
                    _myAnalogDeckSerialPort.Open();
                    _myAnalogDeckSerialPort.Write("Who are you?");  //Asking the device                    
                }
                catch (Exception e)
                {
                    break;
                }

                portNameChecked = _myAnalogDeckSerialPort.PortName;
                Thread.Sleep(5000);

                try
                {
                    _myAnalogDeckSerialPort.Close();
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    break;
                }
            }
            ShowMyAnalogDeckStatus();
        }
    }

    public void ShowMyAnalogDeckStatus()
    {
        if (!portNameFound)
        {
            // Prevent the message from repeating
            if (MyAnalogDeckPortName != "COM0")
            {
                _logger.LogInformation("{0} - MyAnalogDeck not found", DateTime.Now);
                MyAnalogDeckPortName = "COM0";
            }
        }
        else
        {
            // Prevent the message from repeating
            if (MyAnalogDeckPortName != portNameChecked)
            {
                _logger.LogInformation("{0} - MyAnalogDeck found at {1}", DateTime.Now, portNameChecked);
                MyAnalogDeckPortName = portNameChecked;
            }
        }
    }

    private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        var simulator = new InputSimulator();

        if (sp.IsOpen)
        {
            string dataReceived = sp.ReadExisting().Replace("\r\n", "");

            if (dataReceived != "MyAnalogDeck" && dataReceived != "")
            {
                _logger.LogInformation("{0} - {1}", DateTime.Now, dataReceived);
            }

            if (_buttonsSettings.ContainsKey(dataReceived))
            {
                if(_buttonsSettings[dataReceived].Split("+").Length > 1)
                {
                    simulator.Keyboard.ModifiedKeyStroke((VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode),_buttonsSettings[dataReceived].Split("+")[0]),
                       (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), _buttonsSettings[dataReceived].Split("+")[1]));
                }
                else
                {
                    simulator.Keyboard.KeyPress((VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), _buttonsSettings[dataReceived]));
                }
            }
        }
    }
}