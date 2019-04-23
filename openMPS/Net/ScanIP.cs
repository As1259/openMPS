#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using de.fearvel.net.FnLog;

namespace de.fearvel.openMPS.Net
{
    /// <summary>
    ///     IP Scan Tools
    /// </summary>
    internal static class ScanIp
    {


        /// <summary>
        ///     Finds the ip range.
        /// </summary>
        /// <param name="ipMask">The ip mask.</param>
        /// <returns></returns>
        public static IPAddress[] FindIpRange(string[] ipMask)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "FindIpRange");

            var ip = new IPAddress(ConvertStringToAddress(ipMask[0]));
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
        ///     Netmasks to bit.
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
        ///     Converts the string to address.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static byte[] ConvertStringToAddress(string s)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "ConvertStringToAddress");
            var ba = new byte[4];
            ba[0] = Convert.ToByte(s.Substring(0, s.IndexOf(".")));
            s = s.Substring(s.IndexOf(".") + 1, s.Length - s.IndexOf(".") - 1);
            ba[1] = Convert.ToByte(s.Substring(0, s.IndexOf(".")));
            s = s.Substring(s.IndexOf(".") + 1, s.Length - s.IndexOf(".") - 1);
            ba[2] = Convert.ToByte(s.Substring(0, s.IndexOf(".")));
            s = s.Substring(s.IndexOf(".") + 1, s.Length - s.IndexOf(".") - 1);
            ba[3] = Convert.ToByte(s);
            return ba;
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
            var NetworkInfo =
                new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            var moc = NetworkInfo.Get();
            foreach (var mo in moc)
            {
                address = (string[]) mo["IPAddress"];
                subnetMask = (string[]) mo["IPSubnet"];
            }
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", 
                "GetIpMask - " + address[0] + " - "+ subnetMask[0]);

            return new[] {address[0], subnetMask[0]};
        }      

        /// <summary>
        ///     Pings the ip.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static bool PingIp(IPAddress ip)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "PingIp");
            var pingSender = new Ping();
            var options = new PingOptions
            {

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                DontFragment = true
            };
            var data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            var buffer = Encoding.ASCII.GetBytes(data);
            var timeout = 4;
            var reply = pingSender.Send(ip, timeout, buffer, options);
            switch (reply.Status)
            {
                case IPStatus.Success:
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "PingIp - SUCCESS - " + ip.ToString());
                    return true;
                default:
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "ScanIp", "PingIp - FAILED - " + ip.ToString());
                    return false;
            }
        }
    }
}