namespace svelde.nmea.parser
{
    public class GnrmcMessage : RmcMessage
    {
        public override string GetIdentifier()
        {
            return "$GNRMC";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }
}

