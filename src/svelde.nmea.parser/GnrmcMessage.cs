namespace svelde.nmea.parser
{
    /// <summary>
    /// RMC - Recommended minimum specific GPS/Transit data
    ///    RMC,225446,A,4916.45,N,12311.12,W,000.5,054.7,191194,020.3,E*68
    ///       225446       Time of fix 22:54:46 UTC
    ///       A            Navigation receiver warning A = OK, V = warning
    ///       4916.45,N Latitude 49 deg. 16.45 min North
    ///       12311.12,W Longitude 123 deg. 11.12 min West
    ///       000.5        Speed over ground, Knots
    ///       054.7        Course Made Good, True
    ///       191194       Date of fix  19 November 1994
    ///       020.3,E Magnetic variation 20.3 deg East
    ///       *68          mandatory checksum
    ///       
    /// or with extra mode indicator
    ///  $GNRMC,143718.00,A,4513.13793,N,01859.19704,E,0.050,,290719,,,A
    /// http://navspark.mybigcommerce.com/content/NMEA_Format_v0.1.pdf
    /// Mode indicator
    ///‘N’ = Data not valid
    ///‘A’ = Autonomous mode
    ///‘D’ = Differential mode
    ///‘E’ = Estimated(dead reckoning) mode
    /// </summary>
    public class GnrmcMessage : NmeaMessage
    {
        public override string GetIdentifier() => "$GNRMC";

        public string TimeOfFix { get; set; }
        public string NavigationReceiverWarning { get; set; }
        public Location Latitude { get; set;}
        public Location Longitude { get; set; }
        public string SpeedOverGround { get; set; }
        public string CourseMadeGood { get; set; }
        public string DateOfFix { get; set; }
        public string MagneticVariation { get; set; }
        public ModeIndicator ModeIndicator { get; set; }

        public override void Parse(string nmeaLine)
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

            TimeOfFix = items[0];
            NavigationReceiverWarning = items[1];
            Latitude = new Location (items[2]+ items[3]);
            Longitude = new Location(items[4]+ items[5]);
            SpeedOverGround = items[6];
            CourseMadeGood = items[7];
            DateOfFix = items[8];
            MagneticVariation = items[9]+ items[10];

            ModeIndicator = items.Length > 11
                ? new ModeIndicator(items[11])
                : new ModeIndicator("");

            OnNmeaMessageParsed(this);
        }

        protected override void OnNmeaMessageParsed(NmeaMessage e)
        {
            base.OnNmeaMessageParsed(e);
        }

        public override string ToString()
        {
            var result = $"{GetIdentifier()} Time:{TimeOfFix} Warning:{NavigationReceiverWarning} Latitude:{Latitude} Longitude:{Longitude} Speed:{SpeedOverGround} Course:{CourseMadeGood} Date:{DateOfFix} Variation:{MagneticVariation} Mode:{ModeIndicator} ";

            return result;
        }
    }
}

