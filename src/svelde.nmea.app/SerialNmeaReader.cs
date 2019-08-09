using System;
using System.IO.Ports;
using System.Threading;
using System.Linq;

namespace svelde.nmea.app
{
    /// <summary>
    /// Read Nmea sentence from a serial conntection.
    /// </summary>
    public class SerialReader : IDisposable
    {
        private Timer _timer;

        private SerialPort _port = null;

        private string _textBuffer = string.Empty;

        public SerialReader()
        {
            var portNames = SerialPort.GetPortNames();

            var portNamesText = string.Empty;

            foreach (var portName in portNames)
            {
                portNamesText += $"{PortName} ";
            }

            Console.WriteLine($"Ports available: {portNamesText}");

            // Time is set to 15 to prevent this check from blocking the port while connecting
            _timer = new Timer(DetectDisconnectedSerialPort, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
        }

        public SerialReader(string portName) 
            : this()
        {
            PortName = portName;
        }

        public SerialReader(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : this(portName)
        {
            BaudRate = baudRate;
            Parity = parity;
            DataBits = dataBits;
            StopBits = StopBits;
        }

        /// <summary>
        /// Detect disconnected serial port.
        /// Reset serialport on disconnection.
        /// Workaround because the serial port does not generate an event when the serialport disappears.
        /// </summary>
        /// <param name="state"></param>
        private void DetectDisconnectedSerialPort(object state)
        {
            Console.WriteLine($"Comport {PortName} disconnect check");
            if (_port != null
                    && !_port.IsOpen)
            {
                Console.WriteLine($"Comport {PortName} not available anymore");
                Reset();
            }
        }

        public string PortName { get; set; } = "COM8";
        public int BaudRate { get; set; } = 115200;
        public Parity Parity { get; set; } = Parity.None;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        
        public void Open()
        {
            Console.WriteLine("Reset on Open");
            Reset();
        }

        /// <summary>
        /// Is reader connected to serial port.
        /// </summary>
        public bool IsPortConnected()
        {
            return _port.IsOpen;
        }

        public void Close()
        {
            Console.WriteLine("Reset on Close");
            Reset(false);
        }

        private void Reset(bool restart = true)
        {
            var connected = false;

            Console.WriteLine($"Reset state: {PortName}, baudrate: {BaudRate}, databits: {DataBits}, stopbits: {StopBits}, parity: {Parity}");

            while (!connected)
            {
                try
                {
                    if (_port != null)
                    {
                        _port.ErrorReceived -= ErrorReceived;
                    }

                    if (_port != null)
                    {
                        _port.PinChanged -= PinChanged;
                    }

                    if (_port != null)
                    {
                        _port.DataReceived -= DataReceived;
                    }

                    if (_port != null)
                    {
                        if (_port.IsOpen)
                        {
                            _port.Close();
                        }

                        _port.Dispose();

                        _port = null;
                    }

                    _textBuffer = string.Empty;

                    if (!restart)
                    {
                        System.Console.WriteLine("No restart, just close the connection");
                        return;
                    }

                    Thread.Sleep(2500);

                    _port = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);

                    _port.DataReceived += DataReceived;

                    _port.PinChanged += PinChanged;

                    _port.ErrorReceived += ErrorReceived;

                    try
                    {
                        _port.Open();

                        System.Console.WriteLine("Port opened.");

                        connected = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception when Opening: {ex.Message}");

                        connected = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception when Resetting: {ex.Message}");

                    connected = false;
                }
            }
        }

        private void ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine($"Error received: {e.EventType}");

            Reset();
        }

        private void PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            Console.WriteLine($"Pin changed: {e.EventType}");
            Reset();
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            UpdateBuffer();
        }

        private void UpdateBuffer()
        {
            // read current text gathered in serial port
            var str = _port.ReadExisting();

            // append text to buffer
            _textBuffer = _textBuffer + str;

            while (_textBuffer.IndexOf('$') != -1)
            {
                // buffer contains at least one message (start of next message is availble)
                var nextDollarIndex = _textBuffer.IndexOf('$');

                // current nmea message is available from start to next message. (clean it up)
                var nmeaSentence = "$" + _textBuffer.Substring(0, nextDollarIndex).Replace("\n", string.Empty).Replace("\r", string.Empty);

                // remove current message from buffer
                _textBuffer = _textBuffer.Substring(nextDollarIndex + 1);

                // test for availability checksum
                var starIndex = nmeaSentence.IndexOf('*');

                if (starIndex == -1)
                {
                    //No checksum available, ignore this message
                    continue;
                }

                // nmea message start with "$" and code, contains a star and ends with checksum

                // HOLD: Do not show the raw message = Console.WriteLine($"{nmeaSentence}");

                if (NmeaSentenceReceived != null)
                {
                    var sentence = new NmeaSentence
                    {
                        PortName = PortName,
                        Sentence = nmeaSentence,
                    };

                    NmeaSentenceReceived(this, sentence);
                }
            }
        }

        public void Dispose()
        {
            if (_port != null)
            {
                if (_port.IsOpen)
                {
                    _port.Close();
                }

                _port.Dispose();

                _port = null;
            }
        }

        public event EventHandler<NmeaSentence> NmeaSentenceReceived;
    }
}
