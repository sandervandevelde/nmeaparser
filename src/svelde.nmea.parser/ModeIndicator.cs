using System.Collections.Generic;

namespace svelde.nmea.parser
{
    public class ModeIndicator : Dictionary<char,string>
	{
        public ModeIndicator()
        {
            this.Add('A', "Autonomous");
            this.Add('D', "Differential");
            this.Add('E', "Estimated(dead reckoning) mode");
            this.Add('M', "Manual input");
            this.Add('N', "Data not valid");
        }
	}
}

