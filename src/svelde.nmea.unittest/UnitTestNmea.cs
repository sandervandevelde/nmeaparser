using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    public class TestMessage : NmeaMessage
    {
        public TestMessage()
        {
            Type = "Test";
        }

        public override void Parse(string nmeaLine)
        {
            throw new System.NotImplementedException();
        }
    }
    
    [TestClass]
    public class UnitTestNmea
    {
        [TestMethod]
        public void TestMethodParseChecksum()
        {
            // ARRANGE

            var n = new TestMessage();

            var m = "$GNRMC,143718.00,A,4513.13793,N,01859.19704,E,0.050,,290719,,,A*65";

            // ACT

            n.ParseChecksum(m);

            // ASSERT

            Assert.AreEqual("65", n.MandatoryChecksum);
        }

        [TestMethod]
        public void TestMethodExtractChecksum()
        {
            // ARRANGE

            var n = new TestMessage();

            var m = "$GNRMC,143718.00,A,4513.13793,N,01859.19704,E,0.050,,290719,,,A*65";

            // ACT

            var c = n.ExtractChecksum(m);

            // ASSERT

            Assert.AreEqual("65", c);
        }

        [TestMethod]
        public void TestMethodExtractChecksum_NoStar()
        {
            // ARRANGE

            var n = new TestMessage();

            var m = "$GNRMC,143718.00,A,4513.13793,N,01859.19704,E,0.050,,290719,,,A";

            // ACT

            var c = n.ExtractChecksum(m);

            // ASSERT

            Assert.AreEqual(string.Empty, c);
        }

    }
}
