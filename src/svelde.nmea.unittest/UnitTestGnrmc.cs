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

            Assert.AreEqual("143718.00", n.TimeOfFix);
            Assert.AreEqual("OK", n.NavigationReceiverWarning);
            Assert.AreEqual("45.21896550", n.Latitude.ToString());
            Assert.AreEqual("18.98661733", n.Longitude.ToString());
            Assert.AreEqual("0.050", n.SpeedOverGround);
            Assert.AreEqual(string.Empty, n.CourseMadeGood);
            Assert.AreEqual("290719", n.DateOfFix);
            Assert.AreEqual(string.Empty, n.MagneticVariation);
            Assert.AreEqual("Autonomous", n.ModeIndicator.Mode);
        }
    }
}
