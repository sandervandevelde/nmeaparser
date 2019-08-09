namespace svelde.nmea.parser
{
    public class GpvtgMessage : GnvtgMessage
    {
        public override string GetIdentifier()
        {
            return "$GPVTG";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }

    public class GprmcMessage : GnrmcMessage
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

