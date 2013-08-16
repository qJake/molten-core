using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Molten.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace Molten.Core.Tests.Molten.Core.Data
{
    [TestClass]
    public class ValidationHelperTests
    {
        [TestMethod]
        public void ValidateBadDataObject()
        {
            MockData d = new MockData()
            {
                Name = "Elizabeth",
                Email = "not.a.real.email",
                Age = -1,
                Website = null
            };

            Assert.IsTrue(ValidationHelper.ValidateDataObject(d).Count > 0);
        }

        [TestMethod]
        public void ValidateGoodDataObject()
        {
            MockData d = new MockData()
            {
                Name = "Bob",
                Email = "valid@email.com",
                Age = 40,
                Website = "http://net.com/"
            };

            Assert.IsNull(ValidationHelper.ValidateDataObject(d));
        }

        [TestMethod]
        public void ValidateGoodPropertyWithOtherBadData()
        {
            MockData d = new MockData()
            {
                Name = "Elizabeth",
                Email = "not.a.real.email",
                Age = 45,
                Website = null
            };

            Assert.IsNull(ValidationHelper.ValidateProperty(d, "Age"));
        }

        [TestMethod]
        public void ValidateBadPropertyWithOtherBadData()
        {
            MockData d = new MockData()
            {
                Name = "Elizabeth",
                Email = "not.a.real.email",
                Age = 45,
                Website = null
            };

            Assert.IsTrue(ValidationHelper.ValidateProperty(d, "Name").Count > 0);
        }
    }
}
