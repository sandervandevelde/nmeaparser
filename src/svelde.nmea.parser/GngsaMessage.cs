namespace svelde.nmea.parser
{
    /// <summary>
    ///$GNGSA,A,3,01,18,32,08,11,,,,,,,,6.16,1.86,5.88*16
    ///$GNGSA,A,3,,,,,,,,,,,,,6.16,1.86,5.88*17
    ///        GSA - GPS DOP and active satellites
    ///        GSA, A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*39
    ///           A Auto selection of 2D or 3D fix(M = manual)
    ///           3            3D fix
    ///           04,05...     PRNs of satellites used for fix(space for 12)
    ///           2.5          PDOP(dilution of precision)
    ///           1.3          Horizontal dilution of precision(HDOP)
    ///           2.1          Vertical dilution of precision(VDOP)
    ///             DOP is an indication of the effect of satellite geometry on
    ///             the accuracy of the fix.
    /// </summary>
    public class GngsaMessage : NmeaMessage
    {
        public override string GetIdentifier() => "$GNGSA";

        public string AutoSelection{ get; set; }
        public string Fix3D { get; set; }
        public string PrnsOfSatellitesUsedForFix { get; set; }
        public string PDop { get; set; }
        public string HorizontalPod { get; set; }
        public string VerticalPod { get; set; }

        public void Parse(string nmeaLine)
        {
            if (string.IsNullOrWhiteSpace(nmeaLine) 
                    || !nmeaLine.StartsWith(GetIdentifier()))
            {
                throw new NmeaParseMismatchException();
            }

            ParseChecksum(nmeaLine);

            if(MandatoryChecksum != ExtractChecksum(nmeaLine))
            {
                throw new NmeaParseChecksumException();
            }

            // remove identifier plus first comma
            var sentence = nmeaLine.Remove(0, GetIdentifier().Length+1);

            // remove checksum and star
            sentence = sentence.Remove(sentence.IndexOf('*'));

            var items = sentence.Split(',');

            // TODO: check existance of indexbefore inserting

            AutoSelection = items[0];
            Fix3D = items[1];
            PrnsOfSatellitesUsedForFix = items[2] +","+ items[3] + "," + items[4] + "," + items[5] + "," + items[6] + "," + items[7] + "," + items[8] + "," + items[9] + "," + items[10] + "," + items[11] + "," + items[12] + "," + items[13];
            PDop = items[14];
            HorizontalPod= items[15];
            VerticalPod  = items[16];
        }
    }

}

