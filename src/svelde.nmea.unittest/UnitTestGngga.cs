using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestGngga
    {
        [TestMethod]
        public void TestMethodParse()
        {
            // ARRANGE

            var m = "$GNGGA,143718.00,4513.13793,N,01859.19704,E,1,05,1.86,108.1,M,38.1,M,,*40";

            // ACT

            var n = new GnggaMessage();

            n.Parse(m);

            // ASSERT

            Assert.AreEqual(n.FixTaken, "143718.00");
            Assert.AreEqual(n.Latitude, "4513.13793N");
            Assert.AreEqual(n.Longitude, "01859.19704E");
            Assert.AreEqual(n.FixQuality, "1");
            Assert.AreEqual(n.NumberOfSatellites, "05");
            Assert.AreEqual(n.HorizontalPod, "1.86");
            Assert.AreEqual(n.AltitudeMetres, "108.1M");
            Assert.AreEqual(n.HeightOfGeoid, "38.1M");
            Assert.AreEqual(n.SecondsSinceLastUpdateDGPS, "");
            Assert.AreEqual(n.StationIdNumberDGPS, "");
        }
    }
}
