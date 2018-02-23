#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Threading;

namespace de.as1259.openMPS.Tools
{
    /// <summary>
    ///     Class containing Pingers
    /// </summary>
    internal static class RetrieveAlive
    {
        /// <summary>
        ///     Pings the specified ips.
        /// </summary>
        /// <param name="ips">The ips.</param>
        /// <returns></returns>
        public static DataTable ping(List<string> ips)
        {
            var ipCount = 0;
            var dt = new DataTable();
            dt.Columns.Add("IP");
            foreach (var ip in ips)
            {
                ipCount++;
                var loopIp = ip;
                WaitCallback func = delegate
                {
                    if (PingIP(loopIp)) dt.Rows.Add(loopIp);

                    ipCount--;
                };
                ThreadPool.QueueUserWorkItem(func);
            }

            do
            {
            } while (ipCount > 0);

            return dt;
        }

        /// <summary>
        ///     Pings the ip.
        /// </summary>
        /// <param name="IP">The ip.</param>
        /// <returns></returns>
        public static bool PingIP(string IP)
        {
            var result = false;
            try
            {
                var ping = new Ping();
                var pingReply = ping.Send(IP);

                if (pingReply.Status == IPStatus.Success)
                    result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}