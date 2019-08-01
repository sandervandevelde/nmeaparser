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
                { typeof(GnggaMessage), () => { Console.WriteLine($"{e}"); } },
                { typeof(GngllMessage), () => { Console.WriteLine($"{e}"); } },
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
            _parser.Parse(e.Sentence);
        }
    }
}
