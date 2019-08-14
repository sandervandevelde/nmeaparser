namespace svelde.nmea.parser
{
    public class GlgsvMessage : GsvMessage
    {
        public override string GetIdentifier()
        {
            return "$GLGSV";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }
}

