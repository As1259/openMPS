using System.Data;
using System.Diagnostics;
using de.fearvel.net.FnLog;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes.Exceptions;

namespace de.fearvel.openMPS.Net
{
    /// <summary>
    /// Tools for MPS Identification
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public static class DeviceTools
    {
        /// <summary>
        /// Identifies the device.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static string IdentDevice(string ip)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "DeviceTools", "IdentDevice");
            var dt = Config.GetInstance().SelectFromOidTable();
            var profile = SnmpClient.GetOidValue(ip, "1.3.6.1.2.1.1.2.0");
            if (profile.Length == 0)
            {
                FnLog.GetInstance()
                    .AddToLogList(FnLog.LogType.Error, "DeviceTools", "IdentDevice - profile.Length == 0");
                throw new SnmpIdentNotFoundException();
            }

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                Debug.Write("\n\n " + SnmpClient.GetOidValue(ip, "1.3.6.1.2.1.1.2.0"));
                Debug.Write("\n\n " + dt.Rows[i].Field<string>("ProfileName"));
                if (profile.Contains(dt.Rows[i].Field<string>("ProfileName")))
                    return dt.Rows[i].Field<string>("OidPrivateId");
                FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "DeviceTools",
                    "IdentDevice - " + dt.Rows[i].Field<string>("OidPrivateId"));
            }

            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "DeviceTools", "IdentDevice - GENERIC");
            return "Generic";
        }
    }
}