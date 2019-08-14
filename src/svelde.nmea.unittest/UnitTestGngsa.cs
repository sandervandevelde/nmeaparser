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

            Assert.AreEqual("A", n.AutoSelection);
            Assert.AreEqual("3", n.Fix3D);
            Assert.AreEqual(5, n.PrnsOfSatellitesUsedForFix.Count); //"01,18,32,08,11,,,,,,,");
            Assert.AreEqual(6.16m, n.PercentDop);
            Assert.AreEqual(1.86m, n.HorizontalDop);
            Assert.AreEqual(5.88m, n.VerticalDop);
        }
    }


}
