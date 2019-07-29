using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestGngll

    {
        [TestMethod]
        public void TestMethodParse()
        {
            // ARRANGE

            var m = "$GNGLL,4513.13795,N,01859.19702,E,143717.00,A,A*72";

            // ACT

            var n = new GngllMessage();

            n.Parse(m);

            // ASSERT

            Assert.AreEqual(n.Latitude, "4513.13795N");
            Assert.AreEqual(n.Longitude, "01859.19702E");
            Assert.AreEqual(n.FixTaken, "143717.00");
            Assert.AreEqual(n.DataValid, "A");
            Assert.AreEqual(n.ModeIndicator, "A");
        }
    }
}
