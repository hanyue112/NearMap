using System;
using System.Collections.Generic;
using System.Threading;

namespace NMTest.DataSource
{
    public class DataSource : IDataSource
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        private readonly DistributedCacheStore _dcs;
        private readonly object Sync_Lock = new object();

        public DataSource(DistributedCacheStore dcs)
        {
            _dcs = dcs ?? throw new NullReferenceException("DistributedCacheStore cannot be NULL.");
        }

        public object GetValue(string key)
        {
            if (_values.TryGetValue(key, out object v))
            {
                return v;
            }
            else
            {
                v = _dcs.GetValue(key);

                if (v == null)
                {
                    return null;
                }

                ThreadPool.QueueUserWorkItem(Copy_mem, new object[] { key, v });
                return v;
            }
        }

        private void Copy_mem(object o)
        {
            try
            {
                object[] p = o as object[];

                if (p[1] as object == null || string.IsNullOrWhiteSpace(p[0] as string))
                {
                    return;
                }

                lock (Sync_Lock)
                {
                    if (!_values.TryGetValue(p[0] as string, out object v))
                    {
                        _values.Add(p[0] as string, p[1] as object);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Copy_mem " + ex.Message + " @ " + Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}

