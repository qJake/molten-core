using System;
using System.IO;
using Molten.Core.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Molten.Core.Tests.Molten.Core.Localization
{
    [TestClass]
    public class LocalizationTests
    {
        [TestInitialize]
        public void Init()
        {
            var s = new MemoryStream();
            var sw = new StreamWriter(s);

            sw.WriteLine("A=Hello");
            sw.WriteLine("B=1");
            sw.WriteLine("Dot.Test=Dots");
            sw.WriteLine("Spécial=Cháracters");
            sw.WriteLine("#Comment=Line");
            sw.WriteLine("%#=!()[]{}-_=+';\":<>,.~`|\\/");

            sw.Flush();

            Localizer.Initialize(s);
        }

        [TestMethod]
        public void TestLocalizationA()
        {
            Assert.AreEqual("Hello", Localizer.L("A"));
        }

        [TestMethod]
        public void TestLocalizationB()
        {
            Assert.AreEqual("Dots", Localizer.L("Dot.Test"));
        }

        [TestMethod]
        public void TestLocalizationC()
        {
            Assert.AreEqual("Cháracters", Localizer.L("Spécial"));
        }

        [TestMethod]
        public void TestLocalizationD()
        {
            Assert.AreNotEqual("Line", Localizer.L("#Comment"));
        }

        [TestMethod]
        public void TestLocalizationE()
        {
            Assert.AreEqual("!()[]{}-_=+';\":<>,.~`|\\/", Localizer.L("%#"));
        }

        [TestMethod]
        public void TestLocalizationF()
        {
            Assert.AreEqual("Fake.Entry", Localizer.L("Fake.Entry"));
        }
    }
}
