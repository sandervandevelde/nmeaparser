using System.Linq;

namespace svelde.nmea.parser
{
    public class GngsaMessage : GsaMessage
    {
        public override string GetIdentifier()
        {
            return "$GNGSA";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }
}

