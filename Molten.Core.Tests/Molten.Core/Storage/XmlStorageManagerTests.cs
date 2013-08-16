using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Molten.Core.Storage;

namespace Molten.Core.Tests.Molten.Core.Storage
{
    [TestClass]
    public class XmlStorageManagerFailTests
    {
        [ClassInitialize]
        public static void Init(TestContext ctx)
        {
            XmlStorageManager.Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void XmlStorageLoadWithoutInit()
        {
            XmlStorageManager.Load<MockData>("A");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void XmlStorageSaveWithoutInit()
        {
            XmlStorageManager.Save("B", new MockData());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void XmlStorageDeleteWithoutInit()
        {
            XmlStorageManager.DeleteAll();
        }

        [TestMethod]
        [ExpectedException(typeof(StorageInitializationException))]
        public void XmlStorageInitializationFailureA()
        {
            XmlStorageManager.Initialize("@+_%$>*)+_%^$(^@<%#$!@/\\");
        }
        
    }

    [TestClass]
    public class XmlStorageManagerTests
    {
        [ClassInitialize]
        public static void Init(TestContext ctx)
        {
            XmlStorageManager.Initialize("StorageAreaUnitTest");
            XmlStorageManager.Reset();
            XmlStorageManager.Initialize();
        }

        [TestMethod]
        public void XmlStorageSaveA()
        {
            XmlStorageManager.Save("TestA", new MockData()
            {
                Name = "Bob",
                Age = 25,
                Email = "email@email.com",
                Website = "http://doubleyouww.com"
            });
        }

        [TestMethod]
        public void XmlStorageSaveB()
        {
            XmlStorageManager.Save("TestB", new MockData());
        }

        [TestMethod]
        public void XmlStorageLoadA()
        {
            var d = XmlStorageManager.Load<MockData>("Nonexistant-Name");

            Assert.IsNull(d);
        }

        [TestMethod]
        public void XmlStorageSaveAndLoadA()
        {
            XmlStorageManager.Save("TestC", new MockData()
            {
                Name = "Bob",
                Age = 25,
                Email = "email@email.com",
                Website = "http://doubleyouww.com"
            });

            var d = XmlStorageManager.Load<MockData>("TestC");

            Assert.AreEqual("Bob", d.Name);
            Assert.AreEqual(25, d.Age);
            Assert.AreEqual("email@email.com", d.Email);
            Assert.AreEqual("http://doubleyouww.com", d.Website);
        }

        [TestMethod]
        public void XmlStorageSaveAndLoadB()
        {
            XmlStorageManager.Save("TestD", new MockData());

            var d = XmlStorageManager.Load<MockData>("TestD");

            Assert.AreEqual(default(string), d.Name);
            Assert.AreEqual(default(int), d.Age);
            Assert.AreEqual(default(string), d.Email);
            Assert.AreEqual(default(string), d.Website);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void XmlStorageLoadFailureA()
        {
            XmlStorageManager.Load<MockData>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void XmlStorageLoadFailureB()
        {
            XmlStorageManager.Load<MockData>("<>*");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void XmlStorageSaveFailureA()
        {
            XmlStorageManager.Save(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void XmlStorageSaveFailureB()
        {
            XmlStorageManager.Save(" ", new MockData());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void XmlStorageSaveFailureC()
        {
            XmlStorageManager.Save("A", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void XmlStorageSaveFailureD()
        {
            XmlStorageManager.Save("@_)$%*#!$_&_*+!(&)$^%&*{}[]'\\//", new MockData());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void XmlStorageSaveFailureE()
        {
            XmlStorageManager.Save("A", new NonSerializableMockData());
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            XmlStorageManager.DeleteAll();
        }
    }

    [Serializable]
    public class MockData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Website { get; set; }
    }
}
