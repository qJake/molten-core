using System;
using Molten.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Molten.Core.Tests.Molten.Core.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void AppendA()
        {
            var h = "Hello, ".Append("World");
            Assert.AreEqual("Hello, World", h);
        }

        [TestMethod]
        public void AppendB()
        {
            var h = "Test".Append("");
            Assert.AreEqual("Test", h);
        }

        [TestMethod]
        public void AppendC()
        {
            var h = "".Append("Test");
            Assert.AreEqual("Test", h);
        }

        [TestMethod]
        public void PrependA()
        {
            var h = "World".Prepend("Hello, ");
            Assert.AreEqual("Hello, World", h);
        }

        [TestMethod]
        public void PrependB()
        {
            var h = "Test".Prepend("");
            Assert.AreEqual("Test", h);
        }

        [TestMethod]
        public void PrependC()
        {
            var h = "".Prepend("Test");
            Assert.AreEqual("Test", h);
        }

        [TestMethod]
        public void AppendPrependA()
        {
            var h = "World".Prepend("Hello, ").Append("!");
            Assert.AreEqual("Hello, World!", h);
        }

        [TestMethod]
        public void AppendPrependB()
        {
            var h = "name".Prepend("my ").Append(" is").Prepend("Hello, ").Append(" C#!");
            Assert.AreEqual("Hello, my name is C#!", h);
        }
    }
}
