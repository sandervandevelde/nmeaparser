using System;

namespace svelde.nmea.parser
{
    using System.Collections.Generic;
    using Unity;
    using Unity.Injection;

    public class NmeaParser
    {
        private Dictionary<string, NmeaMessage> _parser;

        public NmeaParser()
        {
            _parser = new Dictionary<string, NmeaMessage>();

            var gga = new GnggaMessage();
            _parser.Add(gga.GetIdentifier(), gga);

            var gll = new GngllMessage();
            _parser.Add(gll.GetIdentifier(), gll);

            var gsa = new GngsaMessage();
            _parser.Add(gsa.GetIdentifier(), gsa);

            var gsv = new GpgsvMessage();
            _parser.Add(gsv.GetIdentifier(), gsv);

            var rmc = new GnrmcMessage();
            _parser.Add(rmc.GetIdentifier(), rmc);

            var txt = new GntxtMessage();
            _parser.Add(txt.GetIdentifier(), txt);

            var vtg = new GnvtgMessage();
            _parser.Add(vtg.GetIdentifier(), vtg);
        }

        public NmeaMessage Parse(string nmeaLine)
        {
            return null;
        }
    }
}
