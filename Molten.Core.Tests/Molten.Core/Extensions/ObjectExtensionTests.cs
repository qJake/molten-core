using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Molten.Core.Extensions;

namespace Molten.Core.Tests.Molten.Core
{
    [TestClass]
    public class ObjectExtensionTests
    {
        [TestMethod]
        public void DeepCopyA()
        {
            var dorig = new MockData();

            var d = dorig.Clone();

            dorig = null;

            Assert.AreEqual(default(string), d.Name);
            Assert.AreEqual(default(int), d.Age);
            Assert.AreEqual(default(string), d.Email);
            Assert.AreEqual(default(string), d.Website);
        }

        [TestMethod]
        public void DeepCopyB()
        {
            var dorig =  new MockData()
            {
                Name = "Bob",
                Age = 25,
                Email = "email@email.com",
                Website = "http://doubleyouww.com"
            };

            var d = dorig.Clone();

            dorig = null;

            Assert.AreEqual("Bob", d.Name);
            Assert.AreEqual(25, d.Age);
            Assert.AreEqual("email@email.com", d.Email);
            Assert.AreEqual("http://doubleyouww.com", d.Website);
        }

        [TestMethod]
        public void DeepCopyC()
        {
            MockData nullObj = null;

            var d = ObjectExtensions.Clone(nullObj);

            Assert.IsNull(d);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeepCopyFailA()
        {
            var ns = new NonSerializableMockData();

            ns.Clone();
        }
    }
}
