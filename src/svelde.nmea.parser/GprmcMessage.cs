namespace svelde.nmea.parser
{
    public class GprmcMessage : RmcMessage
    {
        public override string GetIdentifier()
        {
            return "$GPRMC";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }
}

