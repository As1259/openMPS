#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System.Runtime.CompilerServices;
using System.Data;
namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public class OID : SqLiteConnect
    {
        private static OID _instance;
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static OID GetInstance()
        {
            return _instance ?? (_instance = new OID());
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
            return Query("SELECT * FROM OID Where OIDPrivateID='" + ident + "'");
        }

        public void GenerateOidTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS OID" +
                    " (idUnterstuetzteGeraete INTEGER NOT NULL CONSTRAINT pk_OID_idUnterstuetzteGeraete PRIMARY KEY AUTOINCREMENT," +
                    " HerstellerName varchar(100) NOT NULL DEFAULT ''," +
                    " OIDPrivateID varchar(100) NOT NULL DEFAULT ''," +
                    " ProfileName varchar(100) NOT NULL DEFAULT ''," +
                    " DeviceName varchar(100) NOT NULL DEFAULT ''," +
                    " DeviceType varchar(100) NOT NULL DEFAULT ''," +
                    " Manufacturer varchar(500) NOT NULL DEFAULT ''," +
                    " Model varchar(500) NOT NULL DEFAULT ''," +
                    " SerialNumber varchar(500) NOT NULL DEFAULT ''," +
                    " MACAddress varchar(500) NOT NULL DEFAULT ''," +
                    " IPAddresse varchar(500) NOT NULL DEFAULT ''," +
                    " HostName varchar(500) NOT NULL DEFAULT ''," +
                    " LocalID varchar(500) NOT NULL DEFAULT ''," +
                    " DescriptionLocation varchar(500) NOT NULL DEFAULT ''," +
                    " AssetNumber varchar(500) NOT NULL DEFAULT ''," +
                    " InstalledMemory varchar(500) NOT NULL DEFAULT ''," +
                    " FirmwareVersion varchar(500) NOT NULL DEFAULT ''," +
                    " FirmwareVersion2 varchar(500) NOT NULL DEFAULT ''," +
                    " FirmwareVersion3 varchar(500) NOT NULL DEFAULT ''," +
                    " FirmwareVersion4 varchar(500) NOT NULL DEFAULT ''," +
                    " InstallationDate varchar(500) NOT NULL DEFAULT ''," +
                    " ServiceContactIsColor varchar(500) NOT NULL DEFAULT ''," +
                    " IsDuplex varchar(500) NOT NULL DEFAULT ''," +
                    " PowerActive varchar(500) NOT NULL DEFAULT ''," +
                    " PowerIdle varchar(500) NOT NULL DEFAULT ''," +
                    " PowerSleep1 varchar(500) NOT NULL DEFAULT ''," +
                    " PowerSleep2 varchar(500) NOT NULL DEFAULT ''," +
                    " TotalPages varchar(500) NOT NULL DEFAULT ''," +
                    " TotalPagesMono varchar(500) NOT NULL DEFAULT ''," +
                    " TotalPagesColor varchar(500) NOT NULL DEFAULT ''," +
                    " TotalPagesFullColor varchar(500) NOT NULL DEFAULT ''," +
                    " TotalPagesTwoColor varchar(500) NOT NULL DEFAULT ''," +
                    " TotalPagesSingleColor varchar(500) NOT NULL DEFAULT ''," +
                    " TotalPagesDuplex varchar(500) NOT NULL DEFAULT ''," +
                    " UsagePages varchar(500) NOT NULL DEFAULT ''," +
                    " UsagePagesMono varchar(500) NOT NULL DEFAULT ''," +
                    " UsagePagesColor varchar(500) NOT NULL DEFAULT ''," +
                    " UsagePagesFullColor varchar(500) NOT NULL DEFAULT ''," +
                    " UsagePagesTwoColor varchar(500) NOT NULL DEFAULT ''," +
                    " UsagePagesSingleColor varchar(500) NOT NULL DEFAULT ''," +
                    " PrinterPages varchar(500) NOT NULL DEFAULT ''," +
                    " PrinterPagesMono varchar(500) NOT NULL DEFAULT ''," +
                    " PrinterPagesColor varchar(500) NOT NULL DEFAULT ''," +
                    " PrinterPagesFullColor varchar(500) NOT NULL DEFAULT ''," +
                    " PrinterPagesTwoColor varchar(500) NOT NULL DEFAULT ''," +
                    " PrinterPagesSingleColor varchar(500) NOT NULL DEFAULT ''," +
                    " CopyPages varchar(500) NOT NULL DEFAULT ''," +
                    " CopyPagesMono varchar(500) NOT NULL DEFAULT ''," +
                    " CopyPagesColor varchar(500) NOT NULL DEFAULT ''," +
                    " CopyPagesFullColor varchar(500) NOT NULL DEFAULT ''," +
                    " CopyPagesTwoColor varchar(500) NOT NULL DEFAULT ''," +
                    " CopyPagesSingleColor varchar(500) NOT NULL DEFAULT ''," +
                    " FaxPages varchar(500) NOT NULL DEFAULT ''," +
                    " FaxPagesMono varchar(500) NOT NULL DEFAULT ''," +
                    " FaxPagesColor varchar(500) NOT NULL DEFAULT ''," +
                    " OtherPagesOther varchar(500) NOT NULL DEFAULT ''," +
                    " PagesMonoOther varchar(500) NOT NULL DEFAULT ''," +
                    " PagesColorOther varchar(500) NOT NULL DEFAULT ''," +
                    " PagesFullColor varchar(500) NOT NULL DEFAULT ''," +
                    " OtherPagesTwoColor varchar(500) NOT NULL DEFAULT ''," +
                    " OtherPagesSingleColor varchar(500) NOT NULL DEFAULT ''," +
                    " FaxesSentFaxesReceived varchar(500) NOT NULL DEFAULT ''," +
                    " ScansTotalScansTotalMono varchar(500) NOT NULL DEFAULT ''," +
                    " ScansTotalColor varchar(500) NOT NULL DEFAULT ''," +
                    " ScansUsageScansUsageMono varchar(500) NOT NULL DEFAULT ''," +
                    " ScansUsageColor varchar(500) NOT NULL DEFAULT ''," +
                    " ScansCopy varchar(500) NOT NULL DEFAULT ''," +
                    " ScansCopyMono varchar(500) NOT NULL DEFAULT ''," +
                    " ScansCopyColor varchar(500) NOT NULL DEFAULT ''," +
                    " ScansFax varchar(500) NOT NULL DEFAULT ''," +
                    " ScansFaxMono varchar(500) NOT NULL DEFAULT ''," +
                    " ScansFaxColor varchar(500) NOT NULL DEFAULT ''," +
                    " ScansEmail varchar(500) NOT NULL DEFAULT ''," +
                    " ScansEmailMono varchar(500) NOT NULL DEFAULT ''," +
                    " ScansEmailColor varchar(500) NOT NULL DEFAULT ''," +
                    " ScansNet varchar(500) NOT NULL DEFAULT ''," +
                    " ScansNetMono varchar(500) NOT NULL DEFAULT ''," +
                    " ScansNetColor varchar(500) NOT NULL DEFAULT ''," +
                    " ListPages varchar(500) NOT NULL DEFAULT ''," +
                    " LargePages varchar(500) NOT NULL DEFAULT ''," +
                    " LargePagesMono varchar(500) NOT NULL DEFAULT ''," +
                    " LargePagesColor varchar(500) NOT NULL DEFAULT ''," +
                    " LargePagesFullColor varchar(500) NOT NULL DEFAULT ''," +
                    " LargePagesTwoColor varchar(500) NOT NULL DEFAULT ''," +
                    " LargePagesSingleColor varchar(500) NOT NULL DEFAULT ''," +
                    " TotalLargeSheets varchar(500) NOT NULL DEFAULT ''," +
                    " SquareFeetSquareMeters varchar(500) NOT NULL DEFAULT ''," +
                    " LinearFeetStapledSets varchar(500) NOT NULL DEFAULT ''," +
                    " Level1Pages varchar(500) NOT NULL DEFAULT ''," +
                    " Level2Pages varchar(500) NOT NULL DEFAULT ''," +
                    " Level3Pages varchar(500) NOT NULL DEFAULT ''," +
                    " ColorUsageOffice varchar(500) NOT NULL DEFAULT ''," +
                    " ColorUsageOfficeAccent varchar(500) NOT NULL DEFAULT ''," +
                    " ColorUsageProfessional varchar(500) NOT NULL DEFAULT ''," +
                    " ColorUsageProfessionalAccent varchar(500) NOT NULL DEFAULT ''," +
                    " DoubleClickTotal varchar(500) NOT NULL DEFAULT ''," +
                    " DoubleClickMono varchar(500) NOT NULL DEFAULT ''," +
                    " DoubleClickColor varchar(500) NOT NULL DEFAULT ''," +
                    " DoubleClickFullColor varchar(500) NOT NULL DEFAULT ''," +
                    " DoubleClickTwoColor varchar(500) NOT NULL DEFAULT ''," +
                    " DoubleClickSingleColor varchar(500) NOT NULL DEFAULT ''," +
                    " DoubleClickDuplex varchar(500) NOT NULL DEFAULT ''," +
                    " DevelopmentTotal varchar(500) NOT NULL DEFAULT ''," +
                    " DevelopmentMono varchar(500) NOT NULL DEFAULT ''," +
                    " DevelopmentColor varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageAverageBlack varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageAverageCyan varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageAverageMagenta varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageAverageYellow varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageSumBlack varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageSumCyan varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageSumMagenta varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageSumYellow varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageSum2Black varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageSum2Cyan varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageSum2Magenta varchar(500) NOT NULL DEFAULT ''," +
                    " CoverageSum2Yellow varchar(500) NOT NULL DEFAULT ''," +
                    " MeterGroup1 varchar(500) NOT NULL DEFAULT ''," +
                    " MeterGroup2 varchar(500) NOT NULL DEFAULT ''," +
                    " BlackLevelMax varchar(50) DEFAULT ''," +
                    " CyanLevelMax varchar(50) DEFAULT ''," +
                    " MagentaLevelMax varchar(50) DEFAULT ''," +
                    " YellowLevelMax varchar(50) DEFAULT ''," +
                    " BlackLevel varchar(50) DEFAULT ''," +
                    " CyanLevel varchar(50) DEFAULT ''," +
                    " MagentaLevel varchar(50) DEFAULT ''," +
                    " YellowLevel varchar(50) DEFAULT '');");
        }

    }

}