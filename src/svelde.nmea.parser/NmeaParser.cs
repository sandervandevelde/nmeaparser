using System;

namespace svelde.nmea.parser
{
    using System.Collections.Generic;
    using System.Linq;
    using Unity;
    using Unity.Injection;

    public class NmeaParser
    {
        private Dictionary<string, NmeaMessage> _parsers;

        public NmeaParser()
        {
            _parsers = new Dictionary<string, NmeaMessage>();

            var gga = new GnggaMessage();
            gga.NmeaMessageParsed += messageParsed;
            _parsers.Add(gga.GetIdentifier(), gga);

            var gll = new GngllMessage();
            gll.NmeaMessageParsed += messageParsed;
            _parsers.Add(gll.GetIdentifier(), gll);

            var gsa = new GngsaMessage();
            gsa.NmeaMessageParsed += messageParsed;
            _parsers.Add(gsa.GetIdentifier(), gsa);

            var gsv = new GpgsvMessage();
            gsv.NmeaMessageParsed += messageParsed;
            _parsers.Add(gsv.GetIdentifier(), gsv);

            var glgsv = new GlgsvMessage();
            glgsv.NmeaMessageParsed += messageParsed;
            _parsers.Add(glgsv.GetIdentifier(), glgsv);

            var gbgsv = new GbgsvMessage();
            gbgsv.NmeaMessageParsed += messageParsed;
            _parsers.Add(gbgsv.GetIdentifier(), gbgsv);

            var rmc = new GnrmcMessage();
            rmc.NmeaMessageParsed += messageParsed;
            _parsers.Add(rmc.GetIdentifier(), rmc);

            var txt = new GntxtMessage();
            txt.NmeaMessageParsed += messageParsed;
            _parsers.Add(txt.GetIdentifier(), txt);

            var vtg = new GnvtgMessage();
            vtg.NmeaMessageParsed += messageParsed;
            _parsers.Add(vtg.GetIdentifier(), vtg);
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

            if (_parsers.ContainsKey(nmeaLine.Substring(0, 6)))
            {
                var p = _parsers.First(x => x.Key == nmeaLine.Substring(0, 6)).Value;

                p.Parse(nmeaLine);
            }
        }

        public event EventHandler<NmeaMessage> NmeaMessageParsed;
    }
}
