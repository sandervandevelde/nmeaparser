using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestGngsa

    {
        [TestMethod]
        public void TestMethodParse()
        {
            // ARRANGE

            var m = "$GNGSA,A,3,01,18,32,08,11,,,,,,,,6.16,1.86,5.88*16";

            // ACT

            var n = new GngsaMessage();

            n.Parse(m);

            // ASSERT

            Assert.AreEqual(n.AutoSelection, "A");
            Assert.AreEqual(n.Fix3D, "3");
            Assert.AreEqual(n.PrnsOfSatellitesUsedForFix, "01,18,32,08,11,,,,,,,");
            Assert.AreEqual(n.PercentDop, "6.16");
            Assert.AreEqual(n.HorizontalDop, "1.86");
            Assert.AreEqual(n.VerticalDop, "5.88");
        }
    }


}
