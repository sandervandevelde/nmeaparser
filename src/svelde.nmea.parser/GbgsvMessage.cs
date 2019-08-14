namespace svelde.nmea.parser
{
    /// <summary>
    /// Satellite information about elevation, azimuth and CNR, 
    /// $GPGSV is used for GPS satellites, 
    /// $BDGSV is used for Beidou satellites,
    /// $GLGSV is used for GLOSNASS satellites - https://www.cypress.bc.ca/documents/Report_Messages/CTM200/msg_127_GLGSV.html
    /// </summary>
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

