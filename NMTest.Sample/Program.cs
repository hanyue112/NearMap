using NMTest.DataSource;
using System;
using System.Diagnostics;
using System.Threading;

namespace NMTest.Sample
{
    public static class Program
    {
        public static void Main(string[] parameters)
        {
            // your code goes here
            DatabaseStore _dbs = new DatabaseStore();
            DistributedCacheStore _dcs = new DistributedCacheStore();
            DataSource.DataSource _ds = new DataSource.DataSource(_dcs); // Attach

            for (int k = 0; k < 10; k++)
            {
                _dbs.StoreValue("key" + k, "value" + k); //Populate DatabaseStore with data
                _dcs.StoreValue("key" + k, "value" + k); //Distribute Data to DistributedCacheStore for current default "working node"
            }

            for (var i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    // your code goes here   
                    Random rnd = new Random();
                    Stopwatch stopWatch = new Stopwatch();
                    string key= string.Empty;
                    object d;

                    for (int t = 0; t < 50; t++)
                    {
                        key = "key" + rnd.Next(0, 9);
                        stopWatch.Reset();
                        stopWatch.Start();
                        d = _ds.GetValue(key);
                        stopWatch.Stop();

                        Console.WriteLine("[{0}] Request '{1}', response '{2}', time: {3} ms", Thread.CurrentThread.ManagedThreadId, key, d as string, stopWatch.ElapsedMilliseconds.ToString("0.00"));
                    }
                }).Start();
            }

            Console.ReadKey();
        }
    }
}