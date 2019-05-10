using System;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using de.fearvel.net.FnLog;

namespace de.fearvel.openMPS.Net
{
    /// <summary>
    /// IP Scan Tools
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    internal static class ScanIp
    {

        /// <summary>
        /// Finds the ip range.
        /// </summary>
        /// <param name="ipMask">The ip mask.</param>
        /// <returns></returns>
        public static IPAddress[] FindIpRange(string[] ipMask)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "FindIpRange");

            var ip = IPAddress.Parse(ipMask[0]);
            var bits = NetmaskToBit(ipMask[1]);
            var mask = ~(uint.MaxValue >> bits);
            // Convert the IP address to bytes.
            var ipBytes = ip.GetAddressBytes();
            // BitConverter gives bytes in opposite order to GetAddressBytes().
            var maskBytes = BitConverter.GetBytes(mask).Reverse().ToArray();
            var startIpBytes = new byte[ipBytes.Length];
            var endIpBytes = new byte[ipBytes.Length];
            // Calculate the bytes of the start and end IP addresses.
            for (var i = 0; i < ipBytes.Length; i++)
            {
                startIpBytes[i] = (byte) (ipBytes[i] & maskBytes[i]);
                endIpBytes[i] = (byte) (ipBytes[i] | ~maskBytes[i]);
            }
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo,
                "ScanIp", "FindIpRange NETMASK FROM " +
                          new IPAddress(startIpBytes).ToString() +
                          " TO " + new IPAddress(endIpBytes).ToString());

            // Convert the bytes to IP addresses.
            return new[] {new IPAddress(startIpBytes), new IPAddress(endIpBytes)};
        }

        /// <summary>
        /// Resolves a HostName to a IPAddress using the DNS Server
        /// this will only return a IPV4 Address,
        /// because SnmpSharpNet can only handle IPV4
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static IPAddress ResolveHostName(string hostName)
        {
            return Dns.GetHostAddresses(hostName).FirstOrDefault(
                ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        /// <summary>
        /// Resolves an IPAddress to a IPHostEntry
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static IPHostEntry ResolveIPAddress(IPAddress ip)
        {
            return Dns.GetHostEntry(ip);
        }

        /// <summary>
        /// Netmask to bit.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns></returns>
        public static int NetmaskToBit(string mask)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "NetmaskToBit");
            var totalBits = 0;
            foreach (var octet in mask.Split('.'))
            {
                var octetByte = byte.Parse(octet);
                while (octetByte != 0)
                {
                    totalBits += octetByte & 1; // logical AND on the LSB
                    octetByte >>= 1; // do a bitwise shift to the right to create a new LSB
                }
            }

            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "NetmaskToBit - " + totalBits);
            return totalBits;
        }

        /// <summary>
        ///     Gets the ip mask.
        /// </summary>
        /// <returns></returns>
        public static string[] GetIpMask()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "GetIpMask");
            string[] address = null;
            string[] subnetMask = null;
            var networkInfo =
                new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            var moc = networkInfo.Get();
            foreach (var mo in moc)
            {
                address = (string[]) mo["IPAddress"];
                subnetMask = (string[]) mo["IPSubnet"];
            }
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp",
                "GetIpMask - " + address[0] + " - " + subnetMask[0]);

            return new[] {address[0], subnetMask[0]};
        }
        }
}