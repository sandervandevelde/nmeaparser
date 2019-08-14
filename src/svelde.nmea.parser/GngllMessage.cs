using Newtonsoft.Json;

namespace svelde.nmea.parser
{
    public class GngllMessage : NmeaMessage
    {
        public override string GetIdentifier() => "$GNGLL";

        [JsonProperty(PropertyName = "latitude")]
        public Location Latitude { get; private set; }

        [JsonProperty(PropertyName = "longitude")]
        public Location Longitude { get; private set; }

        [JsonProperty(PropertyName = "fixTaken")]
        public string FixTaken { get; private set; }

        [JsonProperty(PropertyName = "dataValid")]
        public string DataValid { get; private set; }

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

            Latitude = new Location(items[0]+ items[1]);
            Longitude = new Location(items[2]+ items[3]);
            FixTaken = items[4];
            DataValid = items[5];

            ModeIndicator = items.Length > 6
                ? new ModeIndicator(items[6])
                : new ModeIndicator("");

            OnNmeaMessageParsed(this);
        }

        protected override void OnNmeaMessageParsed(NmeaMessage e)
        {
            base.OnNmeaMessageParsed(e);
        }

        public override string ToString()
        {
            var result = $"{GetIdentifier()} Latitude:{Latitude} Longitude:{Longitude} Fix:{FixTaken} Valid:{DataValid} Mode:{ModeIndicator}";

            return result;
        }
    }
}

