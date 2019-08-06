using System;
using System.Collections.Generic;

namespace svelde.nmea.parser
{

    /// <summary>
    ///$GPGSV,3,1,10,01,50,304,26,03,24,245,16,08,56,204,28,10,21,059,20*77
    ///$GPGSV,3,2,10,11,61,296,29,18,76,325,19,22,46,251,20,27,25,169,19*7E
    ///$GPGSV,3,3,10,28,16,307,20,32,49,078,30*73
    ///        GSV - Satellites in view
    ///        GSV,2,1,08,01,40,083,46,02,17,308,41,12,07,344,39,14,22,228,45*75
    ///           2            Number of sentences for full data
    ///           1            sentence 1 of 2
    ///           08           Number of satellites in view
    ///           01           Satellite PRN number
    ///           40           Elevation, degrees
    ///           083          Azimuth, degrees
    ///           46           Signal strength - higher is better
    ///           <repeat for up to 4 satellites per sentence>
    ///                There my be up to three GSV sentences in a data packet
    /// </summary>
    public class GpgsvMessage : NmeaMessage
    {
        public override string GetIdentifier() => "$GPGSV";
        public string NumberOfSentences { get; set; }
        public string SentenceNr { get; set; }
        public string NumberOfSatelitesInView { get; set; }
        public List<Satelite> Satelites { get; private set; }
        public override void Parse(string nmeaLine)
        {
            if (string.IsNullOrWhiteSpace(nmeaLine)
                    || !nmeaLine.StartsWith(GetIdentifier()))
            {
                throw new NmeaParseMismatchException();
            }

            ParseChecksum(nmeaLine);

            if (MandatoryChecksum != ExtractChecksum(nmeaLine))
            {
                throw new NmeaParseChecksumException();
            }

            // remove identifier plus first comma
            var sentence = nmeaLine.Remove(0, GetIdentifier().Length + 1);

            // remove checksum and star
            sentence = sentence.Remove(sentence.IndexOf('*'));

            var items = sentence.Split(',');

            // TODO: check existance of indexbefore inserting

            NumberOfSentences = items[0];
            SentenceNr = items[1];
            NumberOfSatelitesInView = items[2];

            //            var sateliteCount = (items.Length - 3)/4;

            var sateliteCount = GetSateliteCount(
                Convert.ToInt32(NumberOfSatelitesInView),
                Convert.ToInt32(NumberOfSentences),
                Convert.ToInt32(SentenceNr));

            Satelites = new List<Satelite>();

            for (int i = 0; i < sateliteCount; i++)
            {
                Satelites.Add(
                    new Satelite
                    {
                        SatelitePrnNumber = items[3 + (i * 4) + 0],
                        ElevationDegrees = items[3 + (i * 4) + 1],
                        AzimuthDegrees = items[3 + (i * 4) + 2],
                        SignalStrength = items[3 + (i * 4) + 3],
                    });
            }

            OnNmeaMessageParsed(this);
        }

        protected override void OnNmeaMessageParsed(NmeaMessage e)
        {
            base.OnNmeaMessageParsed(e);
        }

        private int GetSateliteCount(int numberOfSatelitesInView, int numberOfSentences, int sentenceNr)
        {
            if (numberOfSentences != sentenceNr)
            {
                return 4;
            }
            else
            {
                return numberOfSatelitesInView - ((sentenceNr - 1) * 4);
            }
        }

        public override string ToString()
        {
            var result = $"{GetIdentifier()} InView:{NumberOfSatelitesInView} ";

            foreach(var s in Satelites)
            {
                result += $"{s.SatelitePrnNumber}: Azi={s.AzimuthDegrees}° Ele={s.ElevationDegrees}° Str={s.SignalStrength}; ";
            }

            return result; 
        }
    }
}

