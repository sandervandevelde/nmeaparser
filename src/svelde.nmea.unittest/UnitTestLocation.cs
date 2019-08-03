using Microsoft.VisualStudio.TestTools.UnitTesting;
using svelde.nmea.parser;

namespace svelde.nmea.unittest
{
    [TestClass]
    public class UnitTestLocation
    {
        [TestMethod]
        public void TestMethodLocationNone()
        {
            //// ARRANGE

            var l = new Location("");

            //// ACT

            var d = l.ToDecimalDegrees();

            ///ASSERT

            Assert.AreEqual<double>(-1, d);
        }

        /// <summary>
        /// Latitude = "4036.82924N"
        /// </summary>
        [TestMethod]
        public void TestMethodLocationNorth()
        {
            //// ARRANGE

            var l = new Location("4036.82924N");

            //// ACT

            var d = l.ToDecimalDegrees();

            ///ASSERT

            Assert.AreEqual<double>(40.613820666666669, d);
        }

        [TestMethod]
        public void TestMethodLocationSouth()
        {
            //// ARRANGE

            var l = new Location("4036.82924S");

            //// ACT

            var d = l.ToDecimalDegrees();

            ///ASSERT

            Assert.AreEqual<double>(-40.613820666666669, d);
        }

        /// <summary>
        /// Longitude = "02436.77459E"
        /// </summary>
        [TestMethod]
        public void TestMethodLocationEast()
        {
            //// ARRANGE

            var l = new Location("02436.77459E");

            //// ACT

            var d = l.ToDecimalDegrees();

            ///ASSERT

            Assert.AreEqual<double>(24.612909833333333, d);
        }

        [TestMethod]
        public void TestMethodLocationWest()
        {
            //// ARRANGE

            var l = new Location("02436.77459W");

            //// ACT

            var d = l.ToDecimalDegrees();

            ///ASSERT

            Assert.AreEqual<double>(-24.612909833333333, d);
        }
    }
}
