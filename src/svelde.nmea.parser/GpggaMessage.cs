namespace svelde.nmea.parser
{
    public class GpggaMessage : GnggaMessage
    {
        public override string GetIdentifier()
        {
            return "$GPGGA";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }
}

