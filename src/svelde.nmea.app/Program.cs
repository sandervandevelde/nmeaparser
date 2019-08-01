using svelde.nmea.parser;
using System;


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
            //
        }

        private static void NmeaSentenceReceived(object sender, NmeaSentence e)
        {
            _parser.Parse(e.Sentence);
        }
    }
}
