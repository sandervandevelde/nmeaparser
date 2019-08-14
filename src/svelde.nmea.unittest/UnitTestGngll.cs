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

            Assert.AreEqual("45.21896583", n.Latitude.ToString());
            Assert.AreEqual("18.98661700", n.Longitude.ToString());
            Assert.AreEqual("143717.00", n.FixTaken);
            Assert.AreEqual("A", n.DataValid);
            Assert.AreEqual("Autonomous", n.ModeIndicator.Mode);
        }
    }
}
