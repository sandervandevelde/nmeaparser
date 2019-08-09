using Newtonsoft.Json;
using System;

namespace svelde.nmea.parser
{
    /// <summary>
    ///$GNGGA,143718.00,4513.13793,N,01859.19704,E,1,05,1.86,108.1,M,38.1,M,,*40
    ///        GGA - Global Positioning System Fix Data
    ///        GGA,123519,4807.038,N,01131.324,E,1,08,0.9,545.4,M,46.9,M, , *42
    ///           123519       Fix taken at 12:35:19 UTC
    ///           4807.038,N Latitude 48 deg 07.038' N
    ///           01131.324,E Longitude 11 deg 31.324' E
    ///           1            Fix quality: 0 = invalid
    ///                                     1 = GPS fix
    ///                                     2 = DGPS fix
    ///           08           Number of satellites being tracked
    ///           0.9          Horizontal dilution of position
    ///           545.4,M Altitude, Metres, above mean sea level
    ///           46.9,M Height of geoid(mean sea level) above WGS84
    ///                        ellipsoid
    ///           (empty field) time in seconds since last DGPS update
    ///           (empty field) DGPS station ID number
    /// </summary>
    public class GnggaMessage : NmeaMessage
    {
        [JsonProperty(PropertyName = "fixTaken")]
        public string FixTaken { get; private set; }

        [JsonProperty(PropertyName = "latitude")]
        public Location Latitude { get; private set; }

        [JsonProperty(PropertyName = "longitude")]
        public Location Longitude { get; private set; }

        [JsonProperty(PropertyName = "fixQuality")]
        public string FixQuality { get; private set; }

        [JsonProperty(PropertyName = "numberOfSatellites")]
        public int NumberOfSatellites { get; private set; }

        [JsonProperty(PropertyName = "horizontalPod")]
        public decimal HorizontalPod { get; private set; }

        [JsonProperty(PropertyName = "altitudeMetres")]
        public string AltitudeMetres { get; private set; }

        [JsonProperty(PropertyName = "heightOfGeoid")]
        public string HeightOfGeoid { get; private set; }

        [JsonProperty(PropertyName = "secondsSinceLastUpdateDGPS")]
        public string SecondsSinceLastUpdateDGPS { get; private set; }

        [JsonProperty(PropertyName = "stationIdNumberDGPS")]
        public string StationIdNumberDGPS { get; private set; }

        public override string GetIdentifier() => "$GNGGA";

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

            FixTaken = items[0];
            Latitude = new Location(items[1] + items[2]);
            Longitude = new Location(items[3] + items[4]);

            var fixQuality = "Invalid"; // 0 or other values

            switch(items[5])
            {
                case "1":
                    fixQuality = "GPS fix";
                    break;
                case "2":
                    fixQuality = "DGPS fix";
                    break;
            }

            FixQuality = fixQuality;

            NumberOfSatellites  = Convert.ToInt32(items[6]);
            HorizontalPod  = Convert.ToDecimal(items[7]);
            AltitudeMetres = items[8] + items[9];
            HeightOfGeoid = items[10] + items[11];
            SecondsSinceLastUpdateDGPS = items[12];
            StationIdNumberDGPS = items[13];

            OnNmeaMessageParsed(this);
        }

        protected override void OnNmeaMessageParsed(NmeaMessage e)
        {
            base.OnNmeaMessageParsed(e);
        }

        public override string ToString()
        {
            var result = $"{GetIdentifier()} Fix:{FixTaken} Latitude:{Latitude} Longitude:{Longitude} Quality:{FixQuality} SatCount:{NumberOfSatellites} HDop:{HorizontalPod:N1} Altitude:{AltitudeMetres} Geoid:{HeightOfGeoid} LastUpdate:{SecondsSinceLastUpdateDGPS} DGPS:{StationIdNumberDGPS} ";

            return result;
        }
    }
}

