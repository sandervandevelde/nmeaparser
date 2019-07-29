namespace svelde.nmea.parser
{
    /// <summary>
    ////GLL - Geographic position, Latitude and Longitude
    ////GLL,4916.45,N,12311.12,W,225444,A
    ////   4916.46,N Latitude 49 deg. 16.45 min.North
    ////   12311.12,W Longitude 123 deg. 11.12 min.West
    ////   225444       Fix taken at 22:54:44 UTC
    ////   A            Data valid
    ////     (Garmin 65 does not include time and status)
    ///
    /// $GNGLL,4513.13795,N,01859.19702,E,143717.00,A,A*72
    /// Has extra A?
    /// </summary>
    public class GngllMessage : NmeaMessage
    {
        const string IDENTIFIER= "$GNGLL";

        public string Latitude { get; set;}
        public string Longitude { get; set; }
        public string FixTaken { get; set; }
        public string DataValid { get; set; }
        public string ModeIndicator { get; set; }

        public void Parse(string nmeaLine)
        {
            if (string.IsNullOrWhiteSpace(nmeaLine) 
                    || !nmeaLine.StartsWith(IDENTIFIER))
            {
                throw new NmeaParseMismatchException();
            }

            ParseChecksum(nmeaLine);

            if(MandatoryChecksum != ExtractChecksum(nmeaLine))
            {
                throw new NmeaParseChecksumException();
            }

            // remove identifier plus first comma
            var sentence = nmeaLine.Remove(0, IDENTIFIER.Length+1);

            // remove checksum and star
            sentence = sentence.Remove(sentence.IndexOf('*'));

            var items = sentence.Split(',');

            // TODO: check existance of indexbefore inserting

            Latitude = items[0]+ items[1];
            Longitude = items[2]+ items[3];
            FixTaken = items[4];
            DataValid = items[5];
            ModeIndicator = items[6];
        }
    }
}

