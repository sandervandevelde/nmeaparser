using svelde.nmea.parser;
using System;
using System.Collections.Generic;

namespace svelde.nmea.app
{
    class Program
    {
        private static NmeaParser _parser;

        static void Main(string[] args)
        {
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
                { typeof(GnggaMessage), () => { Console.WriteLine("gngga"); } },
                { typeof(GngllMessage), () => { Console.WriteLine("gngll"); } },
                { typeof(GngsaMessage), () => { Console.WriteLine("gngsa"); } },
                { typeof(GnrmcMessage), () => { Console.WriteLine("gnrmc"); } },
                { typeof(GntxtMessage), () => { Console.WriteLine("gntxt"); } },
                { typeof(GnvtgMessage), () => { Console.WriteLine("gnvtg"); } },
                { typeof(GpgsvMessage), () => { Console.WriteLine("gpgsv"); } },
                { typeof(GlgsvMessage), () => { Console.WriteLine("glgsv"); } },
                { typeof(GbgsvMessage), () => { Console.WriteLine("gbgsv"); } },
            };

            @switch[e.GetType()]();
        }

        private static void NmeaSentenceReceived(object sender, NmeaSentence e)
        {
            _parser.Parse(e.Sentence);
        }
    }
}
