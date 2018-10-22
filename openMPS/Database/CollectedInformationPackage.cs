using System.Runtime.CompilerServices;

namespace de.fearvel.openMPS.Database
{
    /// <inheritdoc />
    public class CollectedInformationPackage : SqliteConnect
    {
        private static CollectedInformationPackage _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static CollectedInformationPackage GetInstance()
        {
            return _instance ?? (_instance = new CollectedInformationPackage());
        }

        private CollectedInformationPackage() { }

        protected override string FileName => "Data.db";
        protected string EncKey => "aWHXzuLJxUWZ9UMSCpx4Y49Ubzh2h3QhQq7eHJP5vCVupybQMzJCtJnv9vmgp3r4";


        public override void GenerateTables()
        {
            GenerateCollectionTable();
        }

        public override void OpenEncrypted(string name)
        {
            OpenWithCustomEncrypted(name, EncKey);
        }

        public override void Open(string name)
        {
            OpenEncrypted(name);
        }

        public void GenerateCollectionTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS Collection" +
                     "(id INTEGER NOT NULL CONSTRAINT pk_Collector_id PRIMARY KEY AUTOINCREMENT," +
                     "  Manufacturer VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  Model VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  SerialNumber VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  MACAddress VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  IPAddresse VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  HostName VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  LocalID VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  DescriptionLocation VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  AssetNumber VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  InstalledMemory VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  FirmwareVersion VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  FirmwareVersion2 VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  FirmwareVersion3 VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  FirmwareVersion4 VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  InstallationDate VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  ServiceContactIsColor VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  IsDuplex VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  PowerActive VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  PowerIdle VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  PowerSleep1 VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  PowerSleep2 VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  TotalPages bigint NOT NULL DEFAULT '0'," +
                     "  TotalPagesMono bigint NOT NULL DEFAULT '0'," +
                     "  TotalPagesColor bigint NOT NULL DEFAULT '0'," +
                     "  TotalPagesFullColor bigint NOT NULL DEFAULT '0'," +
                     "  TotalPagesTwoColor bigint NOT NULL DEFAULT '0'," +
                     "  TotalPagesSingleColor bigint NOT NULL DEFAULT '0'," +
                     "  TotalPagesDuplex bigint NOT NULL DEFAULT '0'," +
                     "  UsagePages bigint NOT NULL DEFAULT '0'," +
                     "  UsagePagesMono bigint NOT NULL DEFAULT '0'," +
                     "  UsagePagesColor bigint NOT NULL DEFAULT '0'," +
                     "  UsagePagesFullColor bigint NOT NULL DEFAULT '0'," +
                     "  UsagePagesTwoColor bigint NOT NULL DEFAULT '0'," +
                     "  UsagePagesSingleColor bigint NOT NULL DEFAULT '0'," +
                     "  PrinterPages bigint NOT NULL DEFAULT '0'," +
                     "  PrinterPagesMono bigint NOT NULL DEFAULT '0'," +
                     "  PrinterPagesColor bigint NOT NULL DEFAULT '0'," +
                     "  PrinterPagesFullColor bigint NOT NULL DEFAULT '0'," +
                     "  PrinterPagesTwoColor bigint NOT NULL DEFAULT '0'," +
                     "  PrinterPagesSingleColor bigint NOT NULL DEFAULT '0'," +
                     "  CopyPages bigint NOT NULL DEFAULT '0'," +
                     "  CopyPagesMono bigint NOT NULL DEFAULT '0'," +
                     "  CopyPagesColor bigint NOT NULL DEFAULT '0'," +
                     "  CopyPagesFullColor bigint NOT NULL DEFAULT '0'," +
                     "  CopyPagesTwoColor bigint NOT NULL DEFAULT '0'," +
                     "  CopyPagesSingleColor bigint NOT NULL DEFAULT '0'," +
                     "  FaxPages bigint NOT NULL DEFAULT '0'," +
                     "  FaxPagesMono bigint NOT NULL DEFAULT '0'," +
                     "  FaxPagesColor bigint NOT NULL DEFAULT '0'," +
                     "  OtherPagesOther bigint NOT NULL DEFAULT '0'," +
                     "  PagesMonoOther bigint NOT NULL DEFAULT '0'," +
                     "  PagesColorOther bigint NOT NULL DEFAULT '0'," +
                     "  PagesFullColor bigint NOT NULL DEFAULT '0'," +
                     "  OtherPagesTwoColor bigint NOT NULL DEFAULT '0'," +
                     "  OtherPagesSingleColor bigint NOT NULL DEFAULT '0'," +
                     "  FaxesSentFaxesReceived bigint NOT NULL DEFAULT '0'," +
                     "  ScansTotalScansTotalMono bigint NOT NULL DEFAULT '0'," +
                     "  ScansTotalColor bigint NOT NULL DEFAULT '0'," +
                     "  ScansUsageScansUsageMono bigint NOT NULL DEFAULT '0'," +
                     "  ScansUsageColor bigint NOT NULL DEFAULT '0'," +
                     "  ScansCopy bigint NOT NULL DEFAULT '0'," +
                     "  ScansCopyMono bigint NOT NULL DEFAULT '0'," +
                     "  ScansCopyColor bigint NOT NULL DEFAULT '0'," +
                     "  ScansFax bigint NOT NULL DEFAULT '0'," +
                     "  ScansFaxMono bigint NOT NULL DEFAULT '0'," +
                     "  ScansFaxColor bigint NOT NULL DEFAULT '0'," +
                     "  Scansemail bigint NOT NULL DEFAULT '0'," +
                     "  ScansemailMono bigint NOT NULL DEFAULT '0'," +
                     "  ScansemailColor bigint NOT NULL DEFAULT '0'," +
                     "  ScansNet bigint NOT NULL DEFAULT '0'," +
                     "  ScansNetMono bigint NOT NULL DEFAULT '0'," +
                     "  ScansNetColor bigint NOT NULL DEFAULT '0'," +
                     "  ListPages bigint NOT NULL DEFAULT '0'," +
                     "  LargePages bigint NOT NULL DEFAULT '0'," +
                     "  LargePagesMono bigint NOT NULL DEFAULT '0'," +
                     "  LargePagesColor bigint NOT NULL DEFAULT '0'," +
                     "  LargePagesFullColor bigint NOT NULL DEFAULT '0'," +
                     "  LargePagesTwoColor bigint NOT NULL DEFAULT '0'," +
                     "  LargePagesSingleColor bigint NOT NULL DEFAULT '0'," +
                     "  TotalLargeSheets bigint NOT NULL DEFAULT '0'," +
                     "  SquareFeetSquareMeters bigint NOT NULL DEFAULT '0'," +
                     "  LinearFeetStapledSets bigint NOT NULL DEFAULT '0'," +
                     "  Level1Pages bigint NOT NULL DEFAULT '0'," +
                     "  Level2Pages bigint NOT NULL DEFAULT '0'," +
                     "  Level3Pages bigint NOT NULL DEFAULT '0'," +
                     "  ColorUsageOffice bigint NOT NULL DEFAULT '0'," +
                     "  ColorUsageOfficeAccent bigint NOT NULL DEFAULT '0'," +
                     "  ColorUsageProfessional bigint NOT NULL DEFAULT '0'," +
                     "  ColorUsageProfessionalAccent bigint NOT NULL DEFAULT '0'," +
                     "  DoubleClickTotal bigint NOT NULL DEFAULT '0'," +
                     "  DoubleClickMono bigint NOT NULL DEFAULT '0'," +
                     "  DoubleClickColor bigint NOT NULL DEFAULT '0'," +
                     "  DoubleClickFullColor bigint NOT NULL DEFAULT '0'," +
                     "  DoubleClickTwoColor bigint NOT NULL DEFAULT '0'," +
                     "  DoubleClickSingleColor bigint NOT NULL DEFAULT '0'," +
                     "  DoubleClickDuplex bigint NOT NULL DEFAULT '0'," +
                     "  DevelopmentTotal bigint NOT NULL DEFAULT '0'," +
                     "  DevelopmentMono bigint NOT NULL DEFAULT '0'," +
                     "  DevelopmentColor bigint NOT NULL DEFAULT '0'," +
                     "  CoverageAverageBlack bigint NOT NULL DEFAULT '0'," +
                     "  CoverageAverageCyan bigint NOT NULL DEFAULT '0'," +
                     "  CoverageAverageMagenta bigint NOT NULL DEFAULT '0'," +
                     "  CoverageAverageYellow bigint NOT NULL DEFAULT '0'," +
                     "  CoverageSumBlack bigint NOT NULL DEFAULT '0'," +
                     "  CoverageSumCyan bigint NOT NULL DEFAULT '0'," +
                     "  CoverageSumMagenta bigint NOT NULL DEFAULT '0'," +
                     "  CoverageSumYellow bigint NOT NULL DEFAULT '0'," +
                     "  CoverageSum2Black bigint NOT NULL DEFAULT '0'," +
                     "  CoverageSum2Cyan bigint NOT NULL DEFAULT '0'," +
                     "  CoverageSum2Magenta bigint NOT NULL DEFAULT '0'," +
                     "  CoverageSum2Yellow bigint NOT NULL DEFAULT '0'," +
                     "  MeterGroup1 VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  MeterGroup2 VARCHAR(500) NOT NULL DEFAULT ''," +
                     "  BlackLevelMax INTEGER DEFAULT 0," +
                     "  CyanLevelMax INTEGER DEFAULT 0," +
                     "  MagentaLevelMax INTEGER DEFAULT 0," +
                     "  YellowLevelMax INTEGER DEFAULT 0," +
                     "  BlackLevel INTEGER DEFAULT 0," +
                     "  CyanLevel INTEGER DEFAULT 0," +
                     "  MagentaLevel INTEGER DEFAULT 0," +
                     "  YellowLevel INTEGER DEFAULT 0);");

        }

    }
}