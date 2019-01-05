#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System.Runtime.CompilerServices;
using System.Data;
using System.Data.SQLite;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public class Oid_DEPRECATED : SqliteConnect
    {
        private static Oid_DEPRECATED _instance;
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Oid_DEPRECATED GetInstance()
        {
            return _instance ?? (_instance = new Oid_DEPRECATED());
        }

        protected override string FileName => "OID.db";

        public override void GenerateTables()
        {
            GenerateInformationTable();
            GenerateOidTable();
        }

        public void GenerateInformationTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS Directory" +
                     " (Identifier varchar(200),val Text," +
                     " CONSTRAINT uq_Version_Identifier UNIQUE (Identifier));");
            if (Query("SELECT * FROM Directory").Rows.Count == 0)
            {
                NonQuery("INSERT INTO Directory (Identifier,val) VALUES ('OID-Version','0.0.0.0');");
            }
        }

        public DataTable GetOidRowByPrivateId(string ident)
        {
            using (var command = new SQLiteCommand(
                "SELECT * FROM OID Where OidPrivateId = @OidPrivateId;"))
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

        public void UpdateOid()
        {
        }

    }
}