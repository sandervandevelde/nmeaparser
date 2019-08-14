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

            Assert.AreEqual("T", n.TrueTrackMadeGood);
            Assert.AreEqual("M", n.MagneticTrackMadeGood);
            Assert.AreEqual("0.050N", n.GroundSpeedKnots);
            Assert.AreEqual("0.092K", n.GroundSpeedKilometersPerHour);
            Assert.AreEqual("Autonomous", n.ModeIndicator.Mode);
        }
    }

}
