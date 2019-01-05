#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Data.SQLite;
using de.fearvel.openMPS.Database.Exceptions;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public class Config : SqliteConnect
    {
        private static Config _instance;
        private DataTable _devices;
        private Dictionary<string, bool> _flags;

        public Dictionary<string, bool> Flags
        {
            get
            {
                if (_flags.Count == 0)
                {
                    ReadFromFlags();
                }

                return _flags;

            }
            private set => _flags = value;
        }

        private void ReadFromFlags()
        {
            try
            {
                foreach (DataRow ds in Query("select * from Flags;").Rows)
                {
                    _flags.Add(ds.Field<string>("Identifier"), ds.Field<bool>("val"));
                }
            }
            catch (Exception)
            {
                throw new MPSSQLiteException();
            }
        }


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
            CreateInformationTable();
            CreateDevicesTable();
            CreateFlagTable();
            GenerateOidTable();
        }

        public void CreateInformationTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS Directory" +
                     " (Identifier varchar(200),val Text," +
                     " CONSTRAINT uq_Version_Identifier UNIQUE (Identifier));");
            if (Query("SELECT * FROM Directory").Rows.Count != 0) return;
            NonQuery("INSERT INTO Directory (Identifier,val) VALUES ('MPS-Version'," +
                     "'" + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion + "');");
            NonQuery("INSERT INTO Directory (Identifier,val) VALUES ('OID-Version','0.0.0.0');");
            NonQuery("INSERT INTO Directory (Identifier,val) VALUES ('UUID','" + Guid.NewGuid().ToString() + "');");
        }

        public DataTable GetOidRowByPrivateId(string ident)
        {
            using (var command = new SQLiteCommand(
                "SELECT * FROM Oid Where OidPrivateId = @OidPrivateId;"))
            {
                command.Parameters.AddWithValue("@OidPrivateId", ident);
                command.Prepare();
                return Query(command);
            }
        }

        public void GenerateOidTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS `OID` (" +
                     " `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                     " `HerstellerName` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `OidPrivateId` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `ProfileName` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `DeviceName` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `DeviceType` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `Manufacturer` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `Model` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `SerialNumber` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `MACAddress` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `IPAddresse` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `HostName` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `DescriptionLocation` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `AssetNumber` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `FirmwareVersion` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `PowerSleep1` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `PowerSleep2` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `TotalPages` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `TotalPagesMono` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `TotalPagesColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `TotalPagesDuplex` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `PrinterPages` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `PrinterPagesMono` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `PrinterPagesColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `PrinterPagesFullColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `PrinterPagesTwoColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CopyPagesMono` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CopyPagesColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CopyPagesFullColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CopyPagesTwoColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CopyPagesSingleColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `OtherPagesSingleColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `FaxesSentFaxesReceived` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansTotalScansTotalMono` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansTotalColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansCopyMono` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansCopyColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansEmail` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansEmailMono` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansNet` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansNetMono` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `ScansNetColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `LargePagesMono` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `LargePagesFullColor` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CoverageAverageBlack` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CoverageAverageCyan` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CoverageAverageMagenta` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `CoverageAverageYellow` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `BlackLevelMax` varchar ( 50 ) DEFAULT ''," +
                     " `CyanLevelMax` varchar ( 50 ) DEFAULT ''," +
                     " `MagentaLevelMax` varchar ( 50 ) DEFAULT ''," +
                     " `YellowLevelMax` varchar ( 50 ) DEFAULT ''," +
                     " `BlackLevel` varchar ( 50 ) DEFAULT ''," +
                     " `CyanLevel` varchar ( 50 ) DEFAULT ''," +
                     " `MagentaLevel` varchar ( 50 ) DEFAULT ''," +
                     " `YellowLevel` varchar ( 50 ) DEFAULT ''" +
                     ");");
        }

        private void CreateFlagTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS Flags" +
                     " (Identifier varchar(200),val bool," +
                     " CONSTRAINT uq_Version_Identifier UNIQUE (Identifier));");
            }
        public void CreateDevicesTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS DEVICES" +
                     " (Aktiv BOOL NOT NULL DEFAULT 'true'," +
                     " IP varchar(39) NOT NULL DEFAULT ''," +
                     " Modell varchar(250)," +
                     " Seriennummer varchar(250)," +
                     " AssetNumber varchar(250) NOT NULL DEFAULT ''," +
                     " id INTEGER NOT NULL CONSTRAINT pk_DEVICES_id PRIMARY KEY AUTOINCREMENT);");
        }


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

        private void InsertIntoFlags(string key, bool value)
        {
            using (var command = new SQLiteCommand(
                "Insert Into Flags"
                + " (Identifier,val)"
                + " Values (@Identifier,@val);"))
            {
                command.Parameters.AddWithValue("@Identifier", key);
                command.Parameters.AddWithValue("@val", value);
                command.Prepare();
                NonQuery(command);
            }
        }

    }

}