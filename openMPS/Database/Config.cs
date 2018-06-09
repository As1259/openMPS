#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public class Config : SqLiteConnect
    {
        private static Config _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Config GetInstance()
        {
            return _instance ?? (_instance = new Config());
        }

        protected override string FileName => "config.db";

        public override void GenerateTables()
        {
            GenerateInformationTable();
            GenerateDevicesTable();
        }
        
        public void GenerateInformationTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS Directory" +
                     " (Identifier varchar(200),val Text," +
                     " CONSTRAINT uq_Version_Identifier UNIQUE (Identifier));");
            if (Query("SELECT * FROM Directory").Rows.Count == 0)
            {
                NonQuery("INSERT INTO Directory (Identifier,val) VALUES ('MPS-Version'," +
                         "'" + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion + "');");
                NonQuery("INSERT INTO Directory (Identifier,val) VALUES ('UUID','" + Guid.NewGuid().ToString() + "');");
            }
        }

        public void GenerateDevicesTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS DEVICES" +
                     " (Aktiv BOOL NOT NULL DEFAULT 'true'," +
                     " IP varchar(39) NOT NULL DEFAULT ''," +
                     " Modell varchar(250)," +
                     " Seriennummer varchar(250)," +
                     " AssetNumber varchar(250) NOT NULL DEFAULT ''," +
                     " id INTEGER NOT NULL CONSTRAINT pk_DEVICES_id PRIMARY KEY AUTOINCREMENT);");
        }
        /// <summary>
        ///     Inserts the in Devices.
        /// </summary>
        /// <param name="aktiv">The aktiv.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="modell">The modell.</param>
        /// <param name="serial">The serial.</param>
        /// <param name="assetNumber">The asset number.</param>
        public void InsertInDeviceTable(string aktiv, byte[] ipAddress, string modell, string serial,
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
        /// <summary>
        ///     Updates the Devices.
        /// </summary>
        /// <param name="aktiv">The aktiv.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="modell">The modell.</param>
        /// <param name="serial">The serial.</param>
        /// <param name="assetNumber">The asset number.</param>
        /// <param name="altIp">The alt ip.</param>
        public void UpdateDeviceTable(string aktiv, byte[] ipAddress, string modell, string serial,
            string assetNumber, byte[] altIp)
        {
            var cmd =
                "Update Devices"
                + " set"
                + " Aktiv='" + aktiv + "',"
                + " ip='" + ipAddress[0] + "." + ipAddress[1] + "." + ipAddress[2] + "." + ipAddress[3] + "',"
                + " Modell='" + modell + "',"
                + " Seriennummer='" + serial + "',"
                + " AssetNumber='" + assetNumber + "'"
                + " where ip='" + altIp[0] + "." + altIp[1] + "." + altIp[2] + "." + altIp[3] + "';";
            Config.GetInstance().NonQuery(cmd);
        }

    }

}