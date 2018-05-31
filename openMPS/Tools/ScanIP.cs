#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using de.as1259.openMPS.SNMP;

namespace de.as1259.openMPS.Tools
{
    /// <summary>
    ///     IP Scan Tools
    /// </summary>
    internal static class ScanIP
    {
        /// <summary>
        ///     Finds the ip range of local machine.
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] findIPRangeOfLocalMachine()
        {
            return findIPRange(getIPMask());
        }

        /// <summary>
        ///     Finds the ip range.
        /// </summary>
        /// <param name="ipMask">The ip mask.</param>
        /// <returns></returns>
        public static IPAddress[] findIPRange(string[] ipMask)
        {
            var ip = new IPAddress(convertStringToAddress(ipMask[0]));
            var bits = netmaskToBit(ipMask[1]);
            var mask = ~(uint.MaxValue >> bits);
            // Convert the IP address to bytes.
            var ipBytes = ip.GetAddressBytes();
            // BitConverter gives bytes in opposite order to GetAddressBytes().
            var maskBytes = BitConverter.GetBytes(mask).Reverse().ToArray();
            var startIPBytes = new byte[ipBytes.Length];
            var endIPBytes = new byte[ipBytes.Length];
            // Calculate the bytes of the start and end IP addresses.
            for (var i = 0; i < ipBytes.Length; i++)
            {
                startIPBytes[i] = (byte) (ipBytes[i] & maskBytes[i]);
                endIPBytes[i] = (byte) (ipBytes[i] | ~maskBytes[i]);
            }

            // Convert the bytes to IP addresses.
            return new[] {new IPAddress(startIPBytes), new IPAddress(endIPBytes)};
        }

        /// <summary>
        ///     Netmasks to bit.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns></returns>
        public static int netmaskToBit(string mask)
        {
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

            return totalBits;
        }

        /// <summary>
        ///     Converts the string to address.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static byte[] convertStringToAddress(string s)
        {
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
        public static string[] getIPMask()
        {
            string[] Adressen = null;
            string[] SubnetMasken = null;
            var NetworkInfo =
                new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            var MOC = NetworkInfo.Get();
            foreach (ManagementObject mo in MOC)
            {
                Adressen = (string[]) mo["IPAddress"];
                SubnetMasken = (string[]) mo["IPSubnet"];
            }

            return new[] {Adressen[0], SubnetMasken[0]};
        }      

        /// <summary>
        ///     Pings the ip.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static bool pingIP(IPAddress ip)
        {
            var pingSender = new Ping();
            var options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;
            var data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            var buffer = Encoding.ASCII.GetBytes(data);
            var timeout = 4;
            var reply = pingSender.Send(ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success) return true;
            return false;
        }
    }
}