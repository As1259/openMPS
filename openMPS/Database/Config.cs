// Copyright (c) 2018 / 2019, Andreas Schreiner

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Data.SQLite;
using de.fearvel.net.FnLog;
using de.fearvel.openMPS.DataTypes;
using de.fearvel.openMPS.DataTypes.Exceptions;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     Manages the Connection to the Local config DB
    /// </summary>
    public class Config : SqliteConnect
    {
        /// <summary>
        /// the Instance of this Singleton
        /// </summary>
        private static Config _instance;

        /// <summary>
        /// the Device Table
        /// </summary>
        private DataTable _devices;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, bool> _flags;

        /// <summary>
        /// filename of the db
        /// </summary>
        protected override string FileName => "config.db";

        /// <summary>
        /// returns the Device DataTable
        /// </summary>
        public DataTable Devices
        {
            get => _devices ?? (_devices = Query("Select * from Devices"));
            set => _devices = value;
        }

        #region "FLAGS"

        /// <summary>
        /// Flag dictionary for use in the futur
        /// </summary>
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

        /// <summary>
        /// reads the data of the Flags table and fills the _flags DataTable
        /// </summary>
        private void ReadFromFlags()
        {
            try
            {
                foreach (DataRow ds in Query("select * from Flags;").Rows)
                {
                    _flags.Add(ds.Field<string>("DKey"), ds.Field<bool>("DVal"));
                }
            }
            catch (Exception)
            {
                throw new MPSSQLiteException();
            }
        }

        #endregion


        /// <summary>
        /// Gets the instance of this singleton class
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Config GetInstance()
        {
            return _instance ?? (_instance = new Config());
        }

        #region "INSERT / UPDATE"

        /// <summary>
        /// Inserts a device into the Devices table
        /// </summary>
        /// <param name="aktiv"></param>
        /// <param name="ipAddress"></param>
        /// <param name="model"></param>
        /// <param name="serial"></param>
        /// <param name="assetNumber"></param>
        public void InsertInDeviceTable(string aktiv, byte[] ipAddress, string model, string serial,
            string assetNumber)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "InsertInDeviceTable");
            using (var command = new SQLiteCommand(
                "Insert into `Devices`" +
                " (`Active`, `Ip`, `Model`, `SerialNumber`, `AssetNumber`)" +
                " Values (@Active,@IPAddress,@Model,@SerialNumber,@AssetNumber);"))
            {
                command.Parameters.AddWithValue("@Active", aktiv);
                command.Parameters.AddWithValue("@IPAddress",
                    ipAddress[0] + "." + ipAddress[1] + "." + ipAddress[2] + "." + ipAddress[3]);
                command.Parameters.AddWithValue("@Model", model);
                command.Parameters.AddWithValue("@SerialNumber", serial);
                command.Parameters.AddWithValue("@AssetNumber", assetNumber);
                command.Prepare();
                NonQuery(command);
            }
        }

        /// <summary>
        /// Updates an entry of the Devices table
        /// </summary>
        /// <param name="aktiv"></param>
        /// <param name="ipAddress"></param>
        /// <param name="model"></param>
        /// <param name="serial"></param>
        /// <param name="assetNumber"></param>
        /// <param name="altIp"></param>
        public void UpdateDeviceTable(string aktiv, byte[] ipAddress, string model, string serial,
            string assetNumber, byte[] altIp)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "UpdateDeviceTable");

            using (var command = new SQLiteCommand(
                "Update Devices set `Active`=@Active, `Ip`=@IPAddress, `Model`=@Model, `SerialNumber`=@SerialNumber, `AssetNumber`=@AssetNumber" +
                " where `Ip`=@AltIPAddress;"))
            {
                command.Parameters.AddWithValue("@Active", aktiv);
                command.Parameters.AddWithValue("@IPAddress",
                    ipAddress[0] + "." + ipAddress[1] + "." + ipAddress[2] + "." + ipAddress[3]);
                command.Parameters.AddWithValue("@Model", model);
                command.Parameters.AddWithValue("@SerialNumber", serial);
                command.Parameters.AddWithValue("@AssetNumber", assetNumber);
                command.Parameters.AddWithValue("@AltIPAddress",
                    altIp[0] + "." + altIp[1] + "." + altIp[2] + "." + altIp[3]);
                command.Prepare();
                NonQuery(command);
            }
        }

        /// <summary>
        /// _devices the Devices DataTable
        /// </summary>
        public void UpdateDevices()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "UpdateDevices");
            _devices = Query("Select * from `Devices`");
        }

        /// <summary>
        /// Inserts a key value(bool) pair into the Flags table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void InsertIntoFlags(string key, bool value)
        {
            using (var command = new SQLiteCommand(
                "Insert Into `Flags`" +
                " (`DKey`, `DVal`)" +
                " Values (@DKey,@DVal);"))
            {
                command.Parameters.AddWithValue("@DKey", key);
                command.Parameters.AddWithValue("@DVal", value);
                command.Prepare();
                NonQuery(command);
            }
        }

        /// <summary>
        /// clears and refills the oid table
        /// </summary>
        /// <param name="ver"></param>
        /// <param name="oids"></param>
        public void UpdateOids(string ver, List<Oid> oids)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "UpdateOids");
            if (oids.Count > 0)
            {
                DeleteFromOidTable();
                InsertOidListToTable(oids);
                UpdateOidVersion(ver);
            }
        }

        /// <summary>
        /// Clears the oid table
        /// resets the increment
        /// </summary>
        private void DeleteFromOidTable()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "DeleteFromOidTable");
            NonQuery("Delete from `Oid`");
            NonQuery("delete from `sqlite_sequence` where `name` = 'Oid';");
        }

        /// <summary>
        /// Inserts the OidList into the oid table via InsertOidToTable
        /// </summary>
        /// <param name="oids"></param>
        private void InsertOidListToTable(List<Oid> oids)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "InsertOidListToTable");

            foreach (var oid in oids)
            {
                InsertOidToTable(oid);
            }
        }

        /// <summary>
        /// Inserts OidToTable
        /// </summary>
        /// <param name="oid"></param>
        private void InsertOidToTable(Oid oid)
        {
            using (var command = new SQLiteCommand(
                "Insert into `Oid` (" +
                " `VendorName` ," +
                " `OidPrivateId` ," +
                " `ProfileName` ," +
                " `DeviceName` ," +
                " `DeviceType` ," +
                " `Manufacturer` ," +
                " `Model` ," +
                " `SerialNumber` ," +
                " `MacAddress` ," +
                " `IpAddress` ," +
                " `HostName` ," +
                " `DescriptionLocation` ," +
                " `AssetNumber` ," +
                " `FirmwareVersion` ," +
                " `PowerSleep1` ," +
                " `PowerSleep2` ," +
                " `TotalPages` ," +
                " `TotalPagesMono` ," +
                " `TotalPagesColor` ," +
                " `TotalPagesDuplex` ," +
                " `PrinterPages` ," +
                " `PrinterPagesMono` ," +
                " `PrinterPagesColor` ," +
                " `PrinterPagesFullColor` ," +
                " `PrinterPagesTwoColor` ," +
                " `CopyPagesMono` ," +
                " `CopyPagesColor` ," +
                " `CopyPagesFullColor` ," +
                " `CopyPagesTwoColor` ," +
                " `CopyPagesSingleColor` ," +
                " `FaxesSentFaxesReceived` ," +
                " `ScansTotalScansTotalMono` ," +
                " `ScansTotalColor` ," +
                " `ScansCopyMono` ," +
                " `ScansCopyColor` ," +
                " `ScansEmail` ," +
                " `ScansEmailMono` ," +
                " `ScansNet` ," +
                " `ScansNetMono` ," +
                " `ScansNetColor` ," +
                " `LargePagesMono` ," +
                " `LargePagesFullColor` ," +
                " `CoverageAverageBlack` ," +
                " `CoverageAverageCyan` ," +
                " `CoverageAverageMagenta` ," +
                " `CoverageAverageYellow` ," +
                " `BlackLevelMax` ," +
                " `CyanLevelMax` ," +
                " `MagentaLevelMax` ," +
                " `YellowLevelMax` ," +
                " `BlackLevel` ," +
                " `CyanLevel` ," +
                " `MagentaLevel` ," +
                " `YellowLevel`" +
                ") Values (" +
                " @VendorName," +
                " @OidPrivateId," +
                " @ProfileName," +
                " @DeviceName," +
                " @DeviceType," +
                " @Manufacturer," +
                " @Model," +
                " @SerialNumber," +
                " @MacAddress," +
                " @IpAddress," +
                " @HostName," +
                " @DescriptionLocation," +
                " @AssetNumber," +
                " @FirmwareVersion," +
                " @PowerSleep1," +
                " @PowerSleep2," +
                " @TotalPages," +
                " @TotalPagesMono," +
                " @TotalPagesColor," +
                " @TotalPagesDuplex," +
                " @PrinterPages," +
                " @PrinterPagesMono," +
                " @PrinterPagesColor," +
                " @PrinterPagesFullColor," +
                " @PrinterPagesTwoColor," +
                " @CopyPagesMono," +
                " @CopyPagesColor," +
                " @CopyPagesFullColor," +
                " @CopyPagesTwoColor," +
                " @CopyPagesSingleColor," +
                " @FaxesSentFaxesReceived," +
                " @ScansTotalScansTotalMono," +
                " @ScansTotalColor," +
                " @ScansCopyMono," +
                " @ScansCopyColor," +
                " @ScansEmail," +
                " @ScansEmailMono," +
                " @ScansNet," +
                " @ScansNetMono," +
                " @ScansNetColor," +
                " @LargePagesMono," +
                " @LargePagesFullColor," +
                " @CoverageAverageBlack," +
                " @CoverageAverageCyan," +
                " @CoverageAverageMagenta," +
                " @CoverageAverageYellow," +
                " @BlackLevelMax," +
                " @CyanLevelMax," +
                " @MagentaLevelMax," +
                " @YellowLevelMax," +
                " @BlackLevel," +
                " @CyanLevel," +
                " @MagentaLevel," +
                " @YellowLevel" +
                ");"))
            {
                command.Parameters.AddWithValue("@VendorName", oid.VendorName);
                command.Parameters.AddWithValue("@OidPrivateId", oid.OidPrivateId);
                command.Parameters.AddWithValue("@ProfileName", oid.ProfileName);
                command.Parameters.AddWithValue("@DeviceName", oid.DeviceName);
                command.Parameters.AddWithValue("@DeviceType", oid.DeviceType);
                command.Parameters.AddWithValue("@Manufacturer", oid.Manufacturer);
                command.Parameters.AddWithValue("@Model", oid.Model);
                command.Parameters.AddWithValue("@SerialNumber", oid.SerialNumber);
                command.Parameters.AddWithValue("@MacAddress", oid.MacAddress);
                command.Parameters.AddWithValue("@IpAddress", oid.IpAddress);
                command.Parameters.AddWithValue("@HostName", oid.HostName);
                command.Parameters.AddWithValue("@DescriptionLocation", oid.DescriptionLocation);
                command.Parameters.AddWithValue("@AssetNumber", oid.AssetNumber);
                command.Parameters.AddWithValue("@FirmwareVersion", oid.FirmwareVersion);
                command.Parameters.AddWithValue("@PowerSleep1", oid.PowerSleep1);
                command.Parameters.AddWithValue("@PowerSleep2", oid.PowerSleep2);
                command.Parameters.AddWithValue("@TotalPages", oid.TotalPages);
                command.Parameters.AddWithValue("@TotalPagesMono", oid.TotalPagesMono);
                command.Parameters.AddWithValue("@TotalPagesColor", oid.TotalPagesColor);
                command.Parameters.AddWithValue("@TotalPagesDuplex", oid.TotalPagesDuplex);
                command.Parameters.AddWithValue("@PrinterPages", oid.PrinterPages);
                command.Parameters.AddWithValue("@PrinterPagesMono", oid.PrinterPagesMono);
                command.Parameters.AddWithValue("@PrinterPagesColor", oid.PrinterPagesColor);
                command.Parameters.AddWithValue("@PrinterPagesFullColor", oid.PrinterPagesFullColor);
                command.Parameters.AddWithValue("@PrinterPagesTwoColor", oid.PrinterPagesTwoColor);
                command.Parameters.AddWithValue("@CopyPagesMono", oid.CopyPagesMono);
                command.Parameters.AddWithValue("@CopyPagesColor", oid.CopyPagesColor);
                command.Parameters.AddWithValue("@CopyPagesFullColor", oid.CopyPagesFullColor);
                command.Parameters.AddWithValue("@CopyPagesTwoColor", oid.CopyPagesTwoColor);
                command.Parameters.AddWithValue("@CopyPagesSingleColor", oid.CopyPagesSingleColor);
                command.Parameters.AddWithValue("@FaxesSentFaxesReceived", oid.FaxesSentFaxesReceived);
                command.Parameters.AddWithValue("@ScansTotalScansTotalMono", oid.ScansTotalScansTotalMono);
                command.Parameters.AddWithValue("@ScansTotalColor", oid.ScansTotalColor);
                command.Parameters.AddWithValue("@ScansCopyMono", oid.ScansCopyMono);
                command.Parameters.AddWithValue("@ScansCopyColor", oid.ScansCopyColor);
                command.Parameters.AddWithValue("@ScansEmail", oid.ScansEmail);
                command.Parameters.AddWithValue("@ScansEmailMono", oid.ScansEmailMono);
                command.Parameters.AddWithValue("@ScansNet", oid.ScansNet);
                command.Parameters.AddWithValue("@ScansNetMono", oid.ScansNetMono);
                command.Parameters.AddWithValue("@ScansNetColor", oid.ScansNetColor);
                command.Parameters.AddWithValue("@LargePagesMono", oid.LargePagesMono);
                command.Parameters.AddWithValue("@LargePagesFullColor", oid.LargePagesFullColor);
                command.Parameters.AddWithValue("@CoverageAverageBlack", oid.CoverageAverageBlack);
                command.Parameters.AddWithValue("@CoverageAverageCyan", oid.CoverageAverageCyan);
                command.Parameters.AddWithValue("@CoverageAverageMagenta", oid.CoverageAverageMagenta);
                command.Parameters.AddWithValue("@CoverageAverageYellow", oid.CoverageAverageYellow);
                command.Parameters.AddWithValue("@BlackLevelMax", oid.BlackLevelMax);
                command.Parameters.AddWithValue("@CyanLevelMax", oid.CyanLevelMax);
                command.Parameters.AddWithValue("@MagentaLevelMax", oid.MagentaLevelMax);
                command.Parameters.AddWithValue("@YellowLevelMax", oid.YellowLevelMax);
                command.Parameters.AddWithValue("@BlackLevel", oid.BlackLevel);
                command.Parameters.AddWithValue("@CyanLevel", oid.CyanLevel);
                command.Parameters.AddWithValue("@MagentaLevel", oid.MagentaLevel);
                command.Parameters.AddWithValue("@YellowLevel", oid.YellowLevel);
                command.Prepare();
                NonQuery(command);
            }

            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "InsertOidToTable done");
        }

        /// <summary>
        /// Updates the OidVersion
        /// </summary>
        /// <param name="ver"></param>
        private void UpdateOidVersion(string ver)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "UpdateOidVersion");

            using (var command = new SQLiteCommand("Update `Directory` set `DVal` = @ver where `DKey` = 'OidVersion'"))
            {
                command.Parameters.AddWithValue("@ver", ver);
                NonQuery(command);
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "UpdateOidVersion done");
            }
        }

        /// <summary>
        /// returns a DataTable with all oid values
        /// </summary>
        /// <returns></returns>
        public DataTable SelectFromOidTable()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "SelectFromOidTable");

            return Query("Select * from `Oid`;");
        }

        /// <summary>
        /// returns a DataTable with a specific oid value
        /// </summary>
        /// <returns></returns>
        public DataTable SelectFromOidTable(string ident)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "Config", "SelectFromOidTable " + ident);

            return Query("Select * from `Oid` where `OidPrivateId`='" + ident + "'");
        }

        #endregion

        #region "TABLE CREATION"

        /// <summary>
        /// Calls the generate table functions
        /// </summary>
        public override void GenerateTables()
        {
            CreateDirectory();
            CreateDevicesTable();
            CreateFlagTable();
            CreateOidTable();
        }

        /// <summary>
        /// creates the Directory table
        /// </summary>
        public void CreateDirectory()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS Directory ( " +
                     " `DKey` varchar(200)," +
                     " `DVal` Text," +
                     " CONSTRAINT uq_Version_Identifier UNIQUE (`DKey`));");
            if (Query("SELECT * FROM Directory").Rows.Count != 0) return;
            NonQuery("INSERT INTO `Directory` (`DKey`,`DVal`) VALUES ('MPSVersion'," +
                     "'" + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)
                         .ProductVersion + "');");
            NonQuery("INSERT INTO `Directory` (`DKey`,`DVal`) VALUES ('OidVersion','0.0.0.0');");
            NonQuery("INSERT INTO `Directory` (`DKey`,`DVal`) VALUES ('UUID','" + Guid.NewGuid().ToString() + "');");
        }

        /// <summary>
        /// returns a DataTable with a specific oid value
        /// </summary>
        /// <returns></returns>
        public DataTable GetOidRowByPrivateId(string ident)
        {
            using (var command = new SQLiteCommand(
                "SELECT * FROM `Oid` Where `OidPrivateId` = @OidPrivateId;"))
            {
                command.Parameters.AddWithValue("@OidPrivateId", ident);
                command.Prepare();
                return Query(command);
            }
        }

        /// <summary>
        /// creates the OidTable
        /// </summary>
        public void CreateOidTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS `Oid` (" +
                     " `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                     " `VendorName` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `OidPrivateId` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `ProfileName` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `DeviceName` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `DeviceType` VARCHAR ( 100 ) NOT NULL DEFAULT ''," +
                     " `Manufacturer` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `Model` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `SerialNumber` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `MacAddress` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
                     " `IpAddress` VARCHAR ( 500 ) NOT NULL DEFAULT ''," +
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

        /// <summary>
        /// creates the Flags table
        /// </summary>
        private void CreateFlagTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS `Flags` (" +
                     " `DKey` varchar(200)," +
                     " `DVal` bool," +
                     " CONSTRAINT uq_Version_Identifier UNIQUE (`DKey`));");
        }

        /// <summary>
        /// creates the Devices table
        /// </summary>
        public void CreateDevicesTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS `Devices` (" +
                     " `Active` BOOL NOT NULL DEFAULT 'true'," +
                     " `Ip` varchar(39) NOT NULL DEFAULT ''," +
                     " `Model` varchar(250)," +
                     " `SerialNumber` varchar(250)," +
                     " `AssetNumber` varchar(250) NOT NULL DEFAULT ''," +
                     " `Id` INTEGER NOT NULL CONSTRAINT pk_DEVICES_id PRIMARY KEY AUTOINCREMENT);");
        }

        #endregion
    }
}