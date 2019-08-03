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

            Assert.AreEqual("40.61382", d.ToString("0.00000"));
        }

        [TestMethod]
        public void TestMethodLocationSouth()
        {
            //// ARRANGE

            var l = new Location("4036.82924S");

            //// ACT

            var d = l.ToDecimalDegrees();

            ///ASSERT

            Assert.AreEqual("-40.61382", d.ToString("0.00000"));
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

            Assert.AreEqual("24.61291", d.ToString("0.00000"));
        }

        [TestMethod]
        public void TestMethodLocationWest()
        {
            //// ARRANGE

            var l = new Location("02436.77459W");

            //// ACT

            var d = l.ToDecimalDegrees();

            ///ASSERT

            Assert.AreEqual("-24.61291", d.ToString("0.00000"));
        }
    }
}
