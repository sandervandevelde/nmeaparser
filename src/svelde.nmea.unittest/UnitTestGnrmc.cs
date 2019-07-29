using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestGnrmc

    {
        [TestMethod]
        public void TestMethod1()
        {
            // ARRANGE

            var m = "$GNRMC,143718.00,A,4513.13793,N,01859.19704,E,0.050,,290719,,,A*65";

            // ACT

            var n = new GnrmcMessage();

            n.Parse(m);

            // ASSERT
        }
    }
}
