﻿namespace svelde.nmea.parser
{
    public class GpgsaMessage : GngsaMessage
    {
        public override string GetIdentifier()
        {
            return "$GPGSA";
        }

        public override void Parse(string nmeaLine)
        {
            base.Parse(nmeaLine);
        }
    }
}
