using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestGnrmc

    {
        [TestMethod]
        public void TestMethodParse()
        {
            // ARRANGE

            var m = "$GNRMC,143718.00,A,4513.13793,N,01859.19704,E,0.050,,290719,,,A*65";

            // ACT

            var n = new GnrmcMessage();

            n.Parse(m);

            // ASSERT

            Assert.AreEqual(n.TimeOfFix, "143718.00");
            Assert.AreEqual(n.NavigationReceiverWarning, "A");
            Assert.AreEqual(n.Latitude, "4513.13793N");
            Assert.AreEqual(n.Longitude, "01859.19704E");
            Assert.AreEqual(n.SpeedOverGround, "0.050");
            Assert.AreEqual(n.CourseMadeGood, "");
            Assert.AreEqual(n.DateOfFix, "290719");
            Assert.AreEqual(n.MagneticVariation, "");
            Assert.AreEqual(n.ModeIndicator, "A");
        }
    }

}
