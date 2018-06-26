#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Data.SQLite;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public class Config : SqLiteConnect
    {
        private static Config _instance;
        private DataTable _devices;

        public DataTable Devices
        {
            get => _devices ?? (_devices = Query("Select * from Devices"));
            set => _devices = value;
        }

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
            using (var command = new SQLiteCommand(
                "Insert into Devices"
                + " (Aktiv,IP,Modell,Seriennummer,AssetNumber)"
                + " Values (@Aktiv,@IPAddress,@Modell,@Seriennummer,@AssetNumber);"))
            {
                command.Parameters.AddWithValue("@Aktiv", aktiv);
                command.Parameters.AddWithValue("@IPAddress",
                    ipAddress[0] + "." + ipAddress[1] + "." + ipAddress[2] + "." + ipAddress[3]);
                command.Parameters.AddWithValue("@Modell", modell);
                command.Parameters.AddWithValue("@Seriennummer", serial);
                command.Parameters.AddWithValue("@AssetNumber", assetNumber);
                command.Prepare();
                NonQuery(command);
            }
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
            using (var command = new SQLiteCommand(
                "Update Devices set Aktiv=@Aktiv, ip=@IPAddress, Modell=@Modell, Seriennummer=@Seriennummer, AssetNumber=@AssetNumber" +
                " where ip=@AltIPAddress;"))
            {
                command.Parameters.AddWithValue("@Aktiv", aktiv);
                command.Parameters.AddWithValue("@IPAddress",
                    ipAddress[0] + "." + ipAddress[1] + "." + ipAddress[2] + "." + ipAddress[3]);
                command.Parameters.AddWithValue("@Modell", modell);
                command.Parameters.AddWithValue("@Seriennummer", serial);
                command.Parameters.AddWithValue("@AssetNumber", assetNumber);
                command.Parameters.AddWithValue("@AltIPAddress",
                    altIp[0] + "." + altIp[1] + "." + altIp[2] + "." + altIp[3]);
                command.Prepare();
                NonQuery(command);
            }
        }

      
        public void UpdateDevices()
        {
            _devices = Query("Select * from Devices");
        }

        private void ReadInitialisationFile()
        {
            if (File.Exists(@"init.db"))
            {

            }
        }

        private void InsertIntoDirectory(string key, string value)
        {

        }

    }

}