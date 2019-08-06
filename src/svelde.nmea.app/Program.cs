using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using svelde.nmea.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace svelde.nmea.app
{
    class Program
    {
        private static DateTime _LastSent;

        private static NmeaParser _parser;

        private static StreamWriter _streamWriter;

        private static DeviceClient _deviceClient = null; 

        static void Main(string[] args)
        {
            _LastSent = DateTime.Now;

            var utc = DateTime.UtcNow;
            var fileName = $"{utc.Year}-{utc.Month}-{utc.Day}={utc.Hour}-{utc.Minute}-{utc.Second}.log";
            _streamWriter = File.AppendText(fileName);

            try
            {
                var connectionString = "";

                _deviceClient = DeviceClient.CreateFromConnectionString(connectionString);

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

                    if  (_LastSent >= DateTime.Now.AddSeconds(-10))
                    {
                        return;
                    }

                    try
                    {
                        _LastSent = DateTime.Now;

                        var telemetry = new Telemetry
                        {
                            Location = new TelemetryLocation
                                        {
                                           Latitude = (e as GngllMessage).Latitude.ToDecimalDegrees(),
                                           Longitude = (e as GngllMessage).Longitude.ToDecimalDegrees()
                                        },
                            FixTaken = (e as GngllMessage).FixTaken,
                            ModeIndicator = (e as GngllMessage).ModeIndicator,
                        };

                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(telemetry);

                        var message = new Message(Encoding.ASCII.GetBytes(json));

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
        [JsonProperty(PropertyName = "location")]
        public TelemetryLocation Location { get; set; }
        [JsonProperty(PropertyName = "modeIndicator")]
        public ModeIndicator ModeIndicator { get; set; }
        [JsonProperty(PropertyName = "fixTaken")]
        public string FixTaken { get; set; }
    }

    public class TelemetryLocation
    {
        [JsonProperty(PropertyName = "lat")]
        public decimal Latitude { get; set; }
        [JsonProperty(PropertyName = "lon")]
        public decimal Longitude { get; set; }
        [JsonProperty(PropertyName = "alt")]
        public decimal? Altitude { get; set; }
    }
}
