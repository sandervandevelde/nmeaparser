using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestGntxt
    {
        [TestMethod]
        public void TestMethodParse()
        {
            // ARRANGE

            var m = "$GNTXT,01,01,02,u-blox AG - www.u-blox.com*4E";

            // ACT

            var n = new GntxtMessage();

            n.Parse(m);

            // ASSERT

            Assert.AreEqual(n.Text, "u-blox AG - www.u-blox.com");
        }
    }

}
