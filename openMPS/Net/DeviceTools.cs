#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System.Data;
using System.Diagnostics;
using de.fearvel.openMPS.Database;

namespace de.fearvel.openMPS.Net
{
    
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
        public static string IdentDevice(string ip)
        {
            var dt = Config.GetInstance().Query("Select * from OID");
            var profile = SnmpClient.GetOidValue(ip, "1.3.6.1.2.1.1.2.0");
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                Debug.Write("\n\n " + SnmpClient.GetOidValue(ip, "1.3.6.1.2.1.1.2.0"));
                Debug.Write("\n\n " + dt.Rows[i].Field<string>("ProfileName"));
                if (profile.Contains(dt.Rows[i].Field<string>("ProfileName")))
                    return dt.Rows[i].Field<string>("OidPrivateId");
            }            
            return "Generic";
        }

    }
}