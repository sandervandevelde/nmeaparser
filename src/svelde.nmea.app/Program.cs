using System;


namespace svelde.nmea.app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Read serial port");

            var s = new SerialReader();

            s.NmeaSentenceReceived += NmeaSentenceReceived;

            s.Open();

            Console.WriteLine("Initialized...");

            Console.ReadKey();
        }

        private static void NmeaSentenceReceived(object sender, NmeaSentence e)
        {
          //  throw new NotImplementedException();
        }
    }
}
