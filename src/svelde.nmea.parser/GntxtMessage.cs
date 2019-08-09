using Newtonsoft.Json;

namespace svelde.nmea.parser
{
    /// <summary>
    ///$GNTXT,01,01,02,u-blox AG - www.u-blox.com*4E
    ///$GNTXT,01,01,02,HW UBX-M8030 00080000*60
    ///$GNTXT,01,01,02,ROM CORE 3.01 (107888)*2B
    ///$GNTXT,01,01,02,FWVER=SPG 3.01*46
    ///$GNTXT,01,01,02,PROTVER=18.00*11
    ///$GNTXT,01,01,02,GPS;GLO;GAL;BDS*77
    ///$GNTXT,01,01,02,SBAS;IMES;QZSS*49
    ///$GNTXT,01,01,02,GNSS OTP = GPS; GLO*37
    ///$GNTXT,01,01,02,LLC=FFFFFFFF-FFFFFFFF-FFFFFFFF-FFFFFFFF-FFFFFFFD*2F
    ///$GNTXT,01,01,02,ANTSUPERV=AC SD PDoS SR*3E
    ///$GNTXT,01,01,02,ANTSTATUS=OK*25
    ///$GNTXT,01,01,02,PF=3FF*4B
    /// </summary>
    public class GntxtMessage : NmeaMessage
    {
        public override string GetIdentifier() => "$GNTXT";

        [JsonProperty(PropertyName = "text")]
        public string Text { get; private set; }

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

            Text = items[3];

            OnNmeaMessageParsed(this);
        }

        protected override void OnNmeaMessageParsed(NmeaMessage e)
        {
            base.OnNmeaMessageParsed(e);
        }

        public override string ToString()
        {
            var result = $"{GetIdentifier()} Text:{Text} ";

            return result;
        }
    }
}

