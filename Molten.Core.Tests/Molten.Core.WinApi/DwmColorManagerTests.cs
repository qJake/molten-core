using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Molten.Core.WinApi;

namespace Molten.Core.Tests.Molten.Core.WinApi
{
    [TestClass]
    public class DwmColorManagerTests
    {
        // This should remain commented unless you are running the unit tests locally on a PC that has Windows Aero enabled (Vista/7/8/8.1).
        // Running this test on automated build servers and other automation environments will cause this test to fail.

        //[TestMethod]
        //public void DwmGetColorTest()
        //{
        //    var color = DwmColorManager.GetColor();

        //    Assert.IsTrue(color.R > 0 || color.G > 0 || color.B > 0 || color.A > 0);
        //}
    }
}
