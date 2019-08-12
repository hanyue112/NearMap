using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMTest.DataSource;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DistributedCacheStore _dcs = new DistributedCacheStore();
            NMTest.DataSource.DataSource _ds = new NMTest.DataSource.DataSource(_dcs); // Attach

            for (int k = 0; k < 10; k++)
            {
                _dcs.StoreValue("key" + k, "value" + k);
            }

            for (int k = 0; k < 10; k++)
            {
                Assert.AreEqual(_ds.GetValue("key" + k), "value" + k);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            string testKey = "KEY10";
            string testValue = "VALUE10";
            DistributedCacheStore _dcs = new DistributedCacheStore();
            NMTest.DataSource.DataSource _ds = new NMTest.DataSource.DataSource(_dcs); // Attach

            _dcs.StoreValue(testKey, testValue);

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            Assert.AreEqual(_ds.GetValue(testKey), testValue);
            stopWatch.Stop();
            Assert.IsTrue(stopWatch.ElapsedMilliseconds >= 100);

            System.Threading.Thread.Sleep(1000);

            stopWatch.Reset();
            stopWatch.Start();
            Assert.AreEqual(_ds.GetValue(testKey), testValue);
            stopWatch.Stop();
            Assert.IsTrue(stopWatch.ElapsedMilliseconds < 100);
        }
    }
}
