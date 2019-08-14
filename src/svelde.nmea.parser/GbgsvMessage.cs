namespace svelde.nmea.parser
{
    public class GbgsvMessage : GsvMessage
    {
        public override string GetIdentifier()
        {
            return "$GBGSV";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }
}

