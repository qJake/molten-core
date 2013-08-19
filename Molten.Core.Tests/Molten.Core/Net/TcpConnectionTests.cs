using System;
using System.Threading;
using Molten.Core.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Molten.Core.Tests.Molten.Core.Net
{
    [TestClass]
    public class TcpConnectionTests
    {
        [TestMethod]
        public void TestHttpConnectionA()
        {
            TcpConnection conn = new TcpConnection("google.com", 80);

            AutoResetEvent are = new AutoResetEvent(false);

            string data = null;

            conn.DataReceived += d =>
            {
                data = d;
                are.Set();
            };

            conn.Send("GET / HTTP/1.1\nHost: www.google.com\nUser-Agent: Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko\n\n");
            are.WaitOne();

            conn.Disconnect();

            if (data == null)
            {
                Assert.Fail("Request timed out, or no data was returned.");
            }
            else
            {
                Assert.IsTrue(data.ToUpper().Contains("200 OK"));
            }
        }
    }
}
