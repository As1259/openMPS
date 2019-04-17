#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.SNMP;

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
            var dt = Config.GetInstance().Query("Select * from OID");
            var profile = SNMPget.GetOidValue(ip, "1.3.6.1.2.1.1.2.0");
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                Debug.Write("\n\n " + SNMPget.GetOidValue(ip, "1.3.6.1.2.1.1.2.0"));
                Debug.Write("\n\n " + dt.Rows[i].Field<string>("ProfileName"));
                if (profile.Contains(dt.Rows[i].Field<string>("ProfileName")))
                    return dt.Rows[i].Field<string>("OidPrivateId");
            }

            return "Generic";
        }

    }
}