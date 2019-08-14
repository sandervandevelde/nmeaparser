using Newtonsoft.Json;

namespace svelde.nmea.parser
{
    public abstract class RmcMessage : NmeaMessage
    {
        [JsonProperty(PropertyName = "timeOfFix")]
        public string TimeOfFix { get; private set; }

        [JsonProperty(PropertyName = "navigationReceiverWarning")]
        public string NavigationReceiverWarning { get; private set; }

        [JsonProperty(PropertyName = "latitude")]
        public Location Latitude { get; private set; }

        [JsonProperty(PropertyName = "longitude")]
        public Location Longitude { get; private set; }

        [JsonProperty(PropertyName = "speedOverGround")]
        public string SpeedOverGround { get; private set; }

        [JsonProperty(PropertyName = "courseMadeGood")]
        public string CourseMadeGood { get; private set; }

        [JsonProperty(PropertyName = "dateOfFix")]
        public string DateOfFix { get; private set; }

        [JsonProperty(PropertyName = "magneticVariation")]
        public string MagneticVariation { get; private set; }

        [JsonProperty(PropertyName = "modeIndicator")]
        public ModeIndicator ModeIndicator { get; private set; }

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

            TimeOfFix = items[0];

            //A = OK, V = warning
            switch (items[1])
            {
                case "A":
                    NavigationReceiverWarning = "OK";
                    break;

                case "V":
                    NavigationReceiverWarning = "Warning";
                    break;
            }


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

