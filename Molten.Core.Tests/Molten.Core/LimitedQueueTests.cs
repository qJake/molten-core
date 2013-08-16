using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Molten.Core.Tests.Molten.Core
{
    [TestClass]
    public class LimitedQueueTests
    {
        [TestMethod]
        public void LimitedQueueLimitA()
        {
            LimitedQueue<int> q = new LimitedQueue<int>(3);

            q.Enqueue(1);
            q.Enqueue(2);
            q.Enqueue(3);
            q.Enqueue(4);

            Assert.AreEqual(2, q.Dequeue());
        }

        [TestMethod]
        public void LimitedQueueLimitB()
        {
            LimitedQueue<int> q = new LimitedQueue<int>(10);

            q.Enqueue(1);
            q.Enqueue(2);
            q.Enqueue(3);
            q.Enqueue(4);
            q.Enqueue(5);
            q.Enqueue(6);
            q.Enqueue(7);

            q.Limit = 2;

            Assert.AreEqual(6, q.Dequeue());
        }

        [TestMethod]
        public void LimitedQueueLimitC()
        {
            LimitedQueue<string> q = new LimitedQueue<string>(10);

            Assert.AreEqual(10, q.Limit);
        }
    }
}
