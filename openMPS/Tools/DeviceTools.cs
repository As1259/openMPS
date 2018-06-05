#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using de.fearvel.openMPS.SNMP;
using de.fearvel.openMPS.SQLiteConnectionTools;

namespace de.fearvel.openMPS.Tools
{
    /// <summary>
    ///     Tool Namespace
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    ///     Tools for the devices
    /// </summary>
    public static class DeviceTools
    {
        /// <summary>
        ///     Identifyes the device.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static string identDevice(string ip)
        {
            var dt =Config.GetInstance().Query("Select * from OID");
            var profile = SNMPget.getOIDValue(ip, "1.3.6.1.2.1.1.2.0");
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                Debug.Write("\n\n " + SNMPget.getOIDValue(ip, "1.3.6.1.2.1.1.2.0"));
                Debug.Write("\n\n " + dt.Rows[i].Field<string>("ProfileName"));
                if (profile.Contains(dt.Rows[i].Field<string>("ProfileName")))
                    return dt.Rows[i].Field<string>("OIDPrivateID");
            }

            return "Generic";
        }

        /// <summary>
        ///     Updates the Devices.
        /// </summary>
        /// <param name="aktiv">The aktiv.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="modell">The modell.</param>
        /// <param name="serial">The serial.</param>
        /// <param name="assetNumber">The asset number.</param>
        /// <param name="altIP">The alt ip.</param>
        public static void updateDevices(string aktiv, byte[] ipAddress, string modell, string serial,
            string assetNumber, byte[] altIP)
        {
            var cmd =
                "Update Devices"
                + " set"
                + " Aktiv='" + aktiv + "',"
                + " ip='" + ipAddress[0] + "." + ipAddress[1] + "." + ipAddress[2] + "." + ipAddress[3] + "',"
                + " Modell='" + modell + "',"
                + " Seriennummer='" + serial + "',"
                + " AssetNumber='" + assetNumber + "'"
                + " where ip='" + altIP[0] + "." + altIP[1] + "." + altIP[2] + "." + altIP[3] + "';";
            Config.GetInstance().NonQuery(cmd);
        }

        /// <summary>
        ///     Inserts the in Devices.
        /// </summary>
        /// <param name="aktiv">The aktiv.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="modell">The modell.</param>
        /// <param name="serial">The serial.</param>
        /// <param name="assetNumber">The asset number.</param>
        public static void insertInDevices(string aktiv, byte[] ipAddress, string modell, string serial,
            string assetNumber)
        {
            var cmd =
                "Insert into Devices"
                + " (Aktiv,IP,Modell,Seriennummer,AssetNumber)"
                + " Values("
                + " '" + aktiv + "',"
                + " '" + ipAddress[0] + "." + ipAddress[1] + "." + ipAddress[2] + "." + ipAddress[3] + "',"
                + " '" + modell + "',"
                + " '" + serial + "',"
                + " '" + assetNumber + "'"
                + ");";
            Config.GetInstance().NonQuery(cmd);
        }
    }
}