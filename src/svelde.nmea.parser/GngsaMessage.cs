using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
    ///           2.5          PDOP(Percent dilution of precision)
    ///           1.3          Horizontal dilution of precision(HDOP)
    ///           2.1          Vertical dilution of precision(VDOP)
    ///             DOP is an indication of the effect of satellite geometry on
    ///             the accuracy of the fix.
    ///             
    /// http://continuouswave.com/ubb/Forum6/HTML/003694.html
    /// NMEA ID 1 to 32: GPS
    /// NMEA ID 33 to 64: WAAS
    /// NMEA ID 65 to 96: GLONASS
    /// Combines multiple Satelite ranges (from GPS, Glosnass, etc.) into one sentence 
    /// If a new sentence of GPS arrives, dismiss old one and start collecting again
    /// </summary>
    public class GngsaMessage : NmeaMessage
    {
        public GngsaMessage()
        {
            PrnsOfSatellitesUsedForFix = new List<int>();
        }

        // TODO: Make the (gps) range variable as parameter

        public override string GetIdentifier() => "$GNGSA";

        [JsonProperty(PropertyName = "autoSelection")]
        public string AutoSelection{ get; private set; }

        [JsonProperty(PropertyName = "fix3D")]
        public string Fix3D { get; private set; }

        [JsonProperty(PropertyName = "percentDop")]
        public decimal PercentDop { get; private set; }

        [JsonProperty(PropertyName = "horizontalDop")]
        public decimal HorizontalDop { get; private set; }

        [JsonProperty(PropertyName = "verticalDop")]
        public decimal VerticalDop { get; private set; }

        [JsonProperty(PropertyName = "prnsOfSatellitesUsedForFix")]
        public List<int> PrnsOfSatellitesUsedForFix { get; private set; }

        public override void Parse(string nmeaLine)
        {
            if (PrnsOfSatellitesUsedForFix.Any(x => x <= 32))
            {
                PrnsOfSatellitesUsedForFix.Sort();

                OnNmeaMessageParsed(this);

                PrnsOfSatellitesUsedForFix.Clear();
            }

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

            AddPrn(items[2]);
            AddPrn(items[3]);
            AddPrn(items[4]);
            AddPrn(items[5]);
            AddPrn(items[6]);
            AddPrn(items[7]);
            AddPrn(items[8]);
            AddPrn(items[9]);
            AddPrn(items[10]);
            AddPrn(items[11]);
            AddPrn(items[12]);
            AddPrn(items[13]);

            PercentDop = Convert.ToDecimal(items[14]);
            HorizontalDop= Convert.ToDecimal(items[15]);
            VerticalDop  = Convert.ToDecimal(items[16]);
        }

        public void AddPrn(string prn)
        {
            if (!string.IsNullOrEmpty(prn))
            PrnsOfSatellitesUsedForFix.Add(Convert.ToInt32(prn));
        }

        public override string ToString()
        {
            var prnsOfSatellitesUsedForFix = string.Empty;

            foreach(var prn in PrnsOfSatellitesUsedForFix)
            {
                prnsOfSatellitesUsedForFix += $"{prn} ";
            }

            prnsOfSatellitesUsedForFix = prnsOfSatellitesUsedForFix.Trim();

            var result = $"{GetIdentifier()} AutoSelection:{AutoSelection} Fix3D:{Fix3D} Prns:{prnsOfSatellitesUsedForFix} PDop:{PercentDop:N1} HDop:{HorizontalDop:N1} VDop:{VerticalDop:N1} ";

            return result;
        }

        protected override void OnNmeaMessageParsed(NmeaMessage e)
        {
            base.OnNmeaMessageParsed(e);
        }
    }
}

