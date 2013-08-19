using System;
using Molten.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Molten.Core.Tests.Molten.Core
{
    [TestClass]
    public class TimeSpanExtensionTests
    {
        [TestMethod]
        public void TimeSpanAbbreviatedStringA()
        {
            var t = new TimeSpan(1, 2, 3);

            Assert.AreEqual("1h 2m 3s", t.ToAbbreviatedString());
        }

        [TestMethod]
        public void TimeSpanAbbreviatedStringB()
        {
            var t = new TimeSpan(4, 5, 6, 7);

            Assert.AreEqual("4d", t.ToAbbreviatedString(1));
        }

        [TestMethod]
        public void TimeSpanAbbreviatedStringC()
        {
            var t = new TimeSpan(4, 5, 6, 7);

            Assert.AreEqual("4d 5h", t.ToAbbreviatedString(2));
        }

        [TestMethod]
        public void TimeSpanAbbreviatedStringD()
        {
            var t = new TimeSpan(4, 5, 6, 7);

            Assert.AreEqual("4d 5h 6m", t.ToAbbreviatedString(3));
        }

        [TestMethod]
        public void TimeSpanAbbreviatedStringE()
        {
            var t = new TimeSpan(4, 5, 6, 7);

            Assert.AreEqual("4d 5h 6m 7s", t.ToAbbreviatedString(4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TimeSpanAbbreviatedStringFailA()
        {
            var t = new TimeSpan(4, 5, 6, 7);
            t.ToAbbreviatedString(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TimeSpanAbbreviatedStringFailB()
        {
            var t = new TimeSpan(4, 5, 6, 7);
            t.ToAbbreviatedString(5);
        }
    }
}
