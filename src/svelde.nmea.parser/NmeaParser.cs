using System;

namespace svelde.nmea.parser
{
    using System.Collections.Generic;
    using System.Linq;

    public class NmeaParser
    {
        private Dictionary<string, NmeaMessage> _parsers;

        public NmeaParser()
        {
            _parsers = new Dictionary<string, NmeaMessage>();

            var gngga = new GnggaMessage();
            gngga.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gngga.Type}", gngga);

            var gpgga = new GpggaMessage();
            gpgga.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gpgga.Type}", gpgga);

            var gll = new GngllMessage();
            gll.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gll.Type}", gll);

            var gngsa = new GngsaMessage();
            gngsa.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gngsa.Type}", gngsa);

            var gpgsa = new GpgsaMessage();
            gpgsa.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gpgsa.Type}", gpgsa);

            var gsv = new GpgsvMessage();
            gsv.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gsv.Type}", gsv);

            var glgsv = new GlgsvMessage();
            glgsv.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${glgsv.Type}", glgsv);

            var gbgsv = new GbgsvMessage();
            gbgsv.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gbgsv.Type}", gbgsv);

            var gnrmc = new GnrmcMessage();
            gnrmc.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gnrmc.Type}", gnrmc);

            var gprmc = new GprmcMessage();
            gprmc.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gprmc.Type}", gprmc);

            var txt = new GntxtMessage();
            txt.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${txt.Type}", txt);

            var gnvtg = new GnvtgMessage();
            gnvtg.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gnvtg.Type}", gnvtg);

            var gpvtg = new GpvtgMessage();
            gpvtg.NmeaMessageParsed += messageParsed;
            _parsers.Add($"${gpvtg.Type}", gpvtg);
        }

        private void messageParsed(object sender, NmeaMessage e)
        {
            if (NmeaMessageParsed != null)
            {
                NmeaMessageParsed(this, e);
            }
        }

        public void Parse(string nmeaLine)
        {
            this.Parse(nmeaLine, "nmea", DateTime.UtcNow);
        }

        public void Parse(string nmeaLine, string port, DateTime timestampUtc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nmeaLine))
                {
                    throw new NmeaParseUnknownException();
                }

                if (_parsers.ContainsKey(nmeaLine.Substring(0, 6)))
                {
                    var p = _parsers.First(x => x.Key == nmeaLine.Substring(0, 6)).Value;

                    p.Port = port;
                    p.TimestampUtc = timestampUtc;

                    p.Parse(nmeaLine);
                }
                else
                {
                    Console.WriteLine($"No parser available for {nmeaLine.Substring(0, 6)}");
                }
            }
            catch (NmeaParseChecksumException)
            {
                Console.WriteLine($"PARSE EXCEPTION FOR '{nmeaLine}'");
            }
        }

        public event EventHandler<NmeaMessage> NmeaMessageParsed;
    }
}
