using Microsoft.Azure.Devices.Client;
using svelde.nmea.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace svelde.nmea.app
{
    class Program
    {
        private static NmeaParser _parser;

        private static StreamWriter _streamWriter;

        private static DeviceClient _deviceClient = null; 

        static void Main(string[] args)
        {
            var utc = DateTime.UtcNow;
            var fileName = $"{utc.Year}-{utc.Month}-{utc.Day}={utc.Hour}-{utc.Minute}-{utc.Second}.log";
            _streamWriter = File.AppendText(fileName);

            try
            {
                _deviceClient = DeviceClient.CreateFromConnectionString("");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during IoT connection {ex}");
            }

            Console.WriteLine("Read serial port");

            _parser = new NmeaParser();

            _parser.NmeaMessageParsed += NmeaMessageParsed;

            var s = new SerialReader();

            s.NmeaSentenceReceived += NmeaSentenceReceived;

            s.Open();

            Console.WriteLine("Initialized...");

            Console.ReadKey();
        }

        private static void NmeaMessageParsed(object sender, NmeaMessage e)
        {
            var @switch = new Dictionary<Type, Action> {
                { typeof(GnggaMessage), () => { Console.WriteLine($"{e}"); } },
                { typeof(GngllMessage), () => 
                {
                    Console.WriteLine($"{e}");

                    if (_deviceClient == null)
                    {
                        return;
                    }

                    try
                    {
                        var telemetry = new Telemetry
                        {
                            Latitude = (e as GngllMessage).Latitude,
                            Longitude = (e as GngllMessage).Longitude,
                            FixTaken = (e as GngllMessage).FixTaken,
                            ModeIndicator = (e as GngllMessage).ModeIndicator,
                        };

                        var message = new Message(Encoding.ASCII.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(telemetry)));

                        _deviceClient.SendEventAsync(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception during IoT communication {ex}");
                    }
                } },
                { typeof(GngsaMessage), () => { Console.WriteLine($"{e}"); } },
                { typeof(GnrmcMessage), () => { Console.WriteLine($"{e}"); } },
                { typeof(GntxtMessage), () => { Console.WriteLine($"{e}"); } },
                { typeof(GnvtgMessage), () => { Console.WriteLine($"{e}"); } },
                { typeof(GpgsvMessage), () => { Console.WriteLine($"{e}(GPS)"); } },
                { typeof(GlgsvMessage), () => { Console.WriteLine($"{e}(Glosnass)"); } },
                { typeof(GbgsvMessage), () => { Console.WriteLine($"{e}(Baidoo)"); } },
            };

            @switch[e.GetType()]();
        }

        private static void NmeaSentenceReceived(object sender, NmeaSentence e)
        {
            _streamWriter.WriteLine(e.Sentence);

            _parser.Parse(e.Sentence);
        }
    }

    public class Telemetry
    {
        public Location Latitude { get; set; }
        public Location Longitude { get; set; }
        public ModeIndicator ModeIndicator { get; set; }
        public string FixTaken { get; set; }
    }
}
