using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestGpgsv

    {
        [TestMethod]
        public void TestMethodParse()
        {
            // ARRANGE

            var m = "$GPGSV,3,1,10,01,50,304,26,03,24,245,16,08,56,204,28,10,21,059,20*77";

            // ACT

            var n = new GpgsvMessage();

            n.Parse(m);

            // ASSERT

            Assert.AreEqual(n.NumberOfSentences, "3");
            Assert.AreEqual(n.SentenceNr, "1");
            Assert.AreEqual(n.NumberOFSatelitesInView, "10");
            Assert.AreEqual(n.Satelites.Count, 4);
        }
    }
}
