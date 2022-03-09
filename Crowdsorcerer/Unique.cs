using System;
using System.Collections.Generic;
using System.Linq;

namespace Crowdsorcerer
{
    public static class Unique
    {
        static object portLock = new();
        static Dictionary<int, DateTime> portsReturnedAt = new();
        static int nextPort = 12235;
        const int PORT_TIMEOUT_SECONDS = 10;

        public static string Name() => Guid.NewGuid().ToString();
        public static string FileName(string extension) => Name() + "." + extension;

        public static int Port()
        {
            lock (portLock)
            {
                if (portsReturnedAt.Count > 0)
                {
                    var freePortKv = portsReturnedAt.FirstOrDefault(p =>
                        (DateTime.Now - p.Value).TotalSeconds > PORT_TIMEOUT_SECONDS);

                    if (!freePortKv.Equals(default))
                    {
                        portsReturnedAt.Remove(freePortKv.Key);
                        return freePortKv.Key;
                    }
                }

                return nextPort++;
            }
        }
        public static void ReturnPort(int port)
        {
            lock (portLock)
            {
                portsReturnedAt.Add(port, DateTime.Now);
            }
        }
    }
}