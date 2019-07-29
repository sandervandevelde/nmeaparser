using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestGnvtg

    {
        [TestMethod]
        public void TestMethodParse()
        {
            // ARRANGE

            var m = "$GNVTG,,T,,M,0.050,N,0.092,K,A*33";

            // ACT

            var n = new GnvtgMessage();

            n.Parse(m);

            // ASSERT

            Assert.AreEqual(n.TrueTrackMadeGood, "T");
            Assert.AreEqual(n.MagneticTrackMadeGood, "M");
            Assert.AreEqual(n.GroundSpeedKnots, "0.050N");
            Assert.AreEqual(n.GroundSpeedKilometersPerHour, "0.092K");
            Assert.AreEqual(n.ModeIndicator, "A");
        }
    }

}
