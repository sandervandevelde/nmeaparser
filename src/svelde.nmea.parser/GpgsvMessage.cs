namespace svelde.nmea.parser
{
    public class GpgsvMessage : GsvMessage
    {
        public override string GetIdentifier()
        {
            return "$GPGSV";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }
}

