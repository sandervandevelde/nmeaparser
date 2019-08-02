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
        public string FixTaken { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string FixQuality { get; set; }
        public string NumberOfSatellites { get; set; }
        public string HorizontalPod { get; set; }
        public string AltitudeMetres { get; set; }
        public string HeightOfGeoid { get; set; }
        public string SecondsSinceLastUpdateDGPS { get; set; }
        public string StationIdNumberDGPS { get; set; }

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
            Latitude = items[1] + items[2];
            Longitude = items[3] + items[4];
            FixQuality = items[5];
            NumberOfSatellites  = items[6];
            HorizontalPod  = items[7];
            AltitudeMetres = items[8] + items[9];
            HeightOfGeoid = items[10] + items[11];
            SecondsSinceLastUpdateDGPS = items[12];
            StationIdNumberDGPS = items[13];
        }

        public override string ToString()
        {
            var result = $"{GetIdentifier()} Fix:{FixTaken} Latitude:{Latitude} Longitude:{Longitude} Quality:{FixQuality} SatCount:{NumberOfSatellites} HDop:{HorizontalPod} Altitude:{AltitudeMetres}mtr Geoid:{HeightOfGeoid} LastUpdate:{SecondsSinceLastUpdateDGPS} DGPS:{StationIdNumberDGPS} ";

            return result;
        }
    }
}

