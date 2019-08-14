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

            Assert.AreEqual("143718.00", n.FixTaken);
            Assert.AreEqual("45.21896550", n.Latitude.ToString());
            Assert.AreEqual("18.98661733", n.Longitude.ToString());
            Assert.AreEqual("GPS fix", n.FixQuality);
            Assert.AreEqual(5, n.NumberOfSatellites);
            Assert.AreEqual(1.86m, n.HorizontalPod);
            Assert.AreEqual("108.1M", n.AltitudeMetres);
            Assert.AreEqual("38.1M", n.HeightOfGeoid);
            Assert.AreEqual(string.Empty, n.SecondsSinceLastUpdateDGPS);
            Assert.AreEqual(string.Empty, n.StationIdNumberDGPS);
        }
    }

}
