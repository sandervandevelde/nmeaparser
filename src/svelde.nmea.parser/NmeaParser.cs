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
            _parsers.Add(gga.GetIdentifier(), gga);

            var gll = new GngllMessage();
            _parsers.Add(gll.GetIdentifier(), gll);

            var gsa = new GngsaMessage();
            _parsers.Add(gsa.GetIdentifier(), gsa);

            var gsv = new GpgsvMessage();
            _parsers.Add(gsv.GetIdentifier(), gsv);

            var glgsv = new GlgsvMessage();
            _parsers.Add(glgsv.GetIdentifier(), glgsv);

            var rmc = new GnrmcMessage();
            _parsers.Add(rmc.GetIdentifier(), rmc);

            var txt = new GntxtMessage();
            _parsers.Add(txt.GetIdentifier(), txt);

            var vtg = new GnvtgMessage();
            _parsers.Add(vtg.GetIdentifier(), vtg);
        }

        public void Parse(string nmeaLine)
        {

            if (_parsers.ContainsKey(nmeaLine.Substring(0, 6)))
            {
                var p = _parsers.First(x => x.Key == nmeaLine.Substring(0, 6)).Value;

                p.Parse(nmeaLine);

                if (NmeaMessageParsed != null)
                {
                    NmeaMessageParsed(this, p);
                }
            }
        }

        public event EventHandler<NmeaMessage> NmeaMessageParsed;
    }
}
