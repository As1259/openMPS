#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using de.fearvel.manastone.serialManagement;
using de.fearvel.net.SQL.Connector;
using de.fearvel.openMPS.Database;

namespace de.fearvel.openMPS.MYSQLConnectionTools
{
    /// <summary>
    ///     MYSQL Namespace
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    ///     Static MYSQL Connector
    ///     This contains the Connection
    /// </summary>
    [Obsolete("broken and about to replaced", true)]
    public static class CollectionToMysql
    {
        /// <summary>
        ///     The connection
        /// </summary>
        private static MysqlConnector connection;

        /// <summary>
        ///     boolean that contains the information of the status of the MYSQL connection
        /// </summary>
        private static bool opened;

        /// <summary>
        ///     Opens a Connection to a specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void open(string server, string database, string username, string password)
        {
            try
            {
                connection = new MysqlConnector(server, 3306, database, username, password, true);
                opened = true;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        ///     Opens a Connection to a specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        public static void open(string server, string database, string username, string password, int port)
        {
            try
            {
                connection = new MysqlConnector(server, port, database, username, password, true);
                opened = true;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }


        /// <summary>
        ///     Closes this connection.
        /// </summary>
        public static void close()
        {
            if (opened)
            {
                connection.Close();
                opened = false;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        ///     executes a SQL quarry
        /// </summary>
        /// <param name="cmd">The command.</param>
        public static void shell(string cmd)
        {
            if (opened)
                connection.NonQuery(cmd);
            else
                throw new ArgumentException();
        }

        /// <summary>
        ///     executes a SQL quarry
        ///     delivers a DataTable
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        public static DataTable shellDT(string cmd)
        {
            if (!opened)
                throw new ArgumentException();

            connection.Query(cmd, out DataTable dt);
            return dt;
        }



        public static void updateOID(SqliteConnector sqlCon)
        {
         //   sqlCon.Query("Select * from INFO", out DataTable fileOIDDt);
         //   var fileOID = fileOIDDt.Rows[0].Field<long>("OIDVersion");
         //   ;
         //   var version = Config.GetInstance().Query("Select OIDVersion from INFO").Rows[0].Field<long>("OIDVersion");
         //   if (version < fileOID)
         //
         //   {
         //        sqlCon.Query("Select * from OID", out DataTable dt);
         //
         //       if (dt.Rows.Count > 0)
         //       {
         //           Config.GetInstance().NonQuery("Delete from OID");
         //           Config.GetInstance().NonQuery("delete from sqlite_sequence where name = 'OID';");
         //
         //
         //           for (var i = 0; i < dt.Rows.Count; i++)
         //               Config.GetInstance().NonQuery(
         //                   "insert into OID"
         //                   + "("
         //                   + "HerstellerName,"
         //                   + "OIDPrivateID,"
         //                   + "ProfileName,"
         //                   + "DeviceName,"
         //                   + "DeviceType,"
         //                   + "Manufacturer,"
         //                   + "Model,"
         //                   + "SerialNumber,"
         //                   + "MACAddress,"
         //                   + "IPAddresse,"
         //                   + "HostName,"
         //                   + "LocalID,"
         //                   + "DescriptionLocation,"
         //                   + "AssetNumber,"
         //                   + "InstalledMemory,"
         //                   + "FirmwareVersion,"
         //                   + "FirmwareVersion2,"
         //                   + "FirmwareVersion3,"
         //                   + "FirmwareVersion4,"
         //                   + "InstallationDate,"
         //                   + "ServiceContactIsColor,"
         //                   + "IsDuplex,"
         //                   + "PowerActive,"
         //                   + "PowerIdle,"
         //                   + "PowerSleep1,"
         //                   + "PowerSleep2,"
         //                   + "TotalPages,"
         //                   + "TotalPagesMono,"
         //                   + "TotalPagesColor,"
         //                   + "TotalPagesFullColor,"
         //                   + "TotalPagesTwoColor,"
         //                   + "TotalPagesSingleColor,"
         //                   + "TotalPagesDuplex,"
         //                   + "UsagePages,"
         //                   + "UsagePagesMono,"
         //                   + "UsagePagesColor,"
         //                   + "UsagePagesFullColor,"
         //                   + "UsagePagesTwoColor,"
         //                   + "UsagePagesSingleColor,"
         //                   + "PrinterPages,"
         //                   + "PrinterPagesMono,"
         //                   + "PrinterPagesColor,"
         //                   + "PrinterPagesFullColor,"
         //                   + "PrinterPagesTwoColor,"
         //                   + "PrinterPagesSingleColor,"
         //                   + "CopyPages,"
         //                   + "CopyPagesMono,"
         //                   + "CopyPagesColor,"
         //                   + "CopyPagesFullColor,"
         //                   + "CopyPagesTwoColor,"
         //                   + "CopyPagesSingleColor,"
         //                   + "FaxPages,"
         //                   + "FaxPagesMono,"
         //                   + "FaxPagesColor,"
         //                   + "OtherPagesOther,"
         //                   + "PagesMonoOther,"
         //                   + "PagesColorOther,"
         //                   + "PagesFullColor,"
         //                   + "OtherPagesTwoColor,"
         //                   + "OtherPagesSingleColor,"
         //                   + "FaxesSentFaxesReceived,"
         //                   + "ScansTotalScansTotalMono,"
         //                   + "ScansTotalColor,"
         //                   + "ScansUsageScansUsageMono,"
         //                   + "ScansUsageColor,"
         //                   + "ScansCopy,"
         //                   + "ScansCopyMono,"
         //                   + "ScansCopyColor,"
         //                   + "ScansFax,"
         //                   + "ScansFaxMono,"
         //                   + "ScansFaxColor,"
         //                   + "ScansEmail,"
         //                   + "ScansEmailMono,"
         //                   + "ScansEmailColor,"
         //                   + "ScansNet,"
         //                   + "ScansNetMono,"
         //                   + "ScansNetColor,"
         //                   + "ListPages,"
         //                   + "LargePages,"
         //                   + "LargePagesMono,"
         //                   + "LargePagesColor,"
         //                   + "LargePagesFullColor,"
         //                   + "LargePagesTwoColor,"
         //                   + "LargePagesSingleColor,"
         //                   + "TotalLargeSheets,"
         //                   + "SquareFeetSquareMeters,"
         //                   + "LinearFeetStapledSets,"
         //                   + "Level1Pages,"
         //                   + "Level2Pages,"
         //                   + "Level3Pages,"
         //                   + "ColorUsageOffice,"
         //                   + "ColorUsageOfficeAccent,"
         //                   + "ColorUsageProfessional,"
         //                   + "ColorUsageProfessionalAccent,"
         //                   + "DoubleClickTotal,"
         //                   + "DoubleClickMono,"
         //                   + "DoubleClickColor,"
         //                   + "DoubleClickFullColor,"
         //                   + "DoubleClickTwoColor,"
         //                   + "DoubleClickSingleColor,"
         //                   + "DoubleClickDuplex,"
         //                   + "DevelopmentTotal,"
         //                   + "DevelopmentMono,"
         //                   + "DevelopmentColor,"
         //                   + "CoverageAverageBlack,"
         //                   + "CoverageAverageCyan,"
         //                   + "CoverageAverageMagenta,"
         //                   + "CoverageAverageYellow,"
         //                   + "CoverageSumBlack,"
         //                   + "CoverageSumCyan,"
         //                   + "CoverageSumMagenta,"
         //                   + "CoverageSumYellow,"
         //                   + "CoverageSum2Black,"
         //                   + "CoverageSum2Cyan,"
         //                   + "CoverageSum2Magenta,"
         //                   + "CoverageSum2Yellow,"
         //                   + "MeterGroup1,"
         //                   + "MeterGroup2,"
         //                   + "BlackLevelMax,"
         //                   + "CyanLevelMax,"
         //                   + "MagentaLevelMax,"
         //                   + "YellowLevelMax,"
         //                   + "BlackLevel,"
         //                   + "CyanLevel,"
         //                   + "MagentaLevel,"
         //                   + "YellowLevel"
         //                   + ")"
         //                   + " Values"
         //                   + " ("
         //                   + "'" + dt.Rows[i].Field<string>("HerstellerName") + "'," //HerstellerName 
         //                   + "'" + dt.Rows[i].Field<string>("OIDPrivateID") + "'," //OIDPrivateID 
         //                   + "'" + dt.Rows[i].Field<string>("ProfileName") + "'," //ProfileName 
         //                   + "'" + dt.Rows[i].Field<string>("DeviceName") + "'," //DeviceName 
         //                   + "'" + dt.Rows[i].Field<string>("DeviceType") + "'," //DeviceType 
         //                   + "'" + dt.Rows[i].Field<string>("Manufacturer") + "'," //Manufacturer 
         //                   + "'" + dt.Rows[i].Field<string>("Model") + "'," //Model 
         //                   + "'" + dt.Rows[i].Field<string>("SerialNumber") + "'," //SerialNumber 
         //                   + "'" + dt.Rows[i].Field<string>("MACAddress") + "'," //MACAddress 
         //                   + "'" + dt.Rows[i].Field<string>("IPAddresse") + "'," //IPAddresse 
         //                   + "'" + dt.Rows[i].Field<string>("HostName") + "'," //HostName 
         //                   + "'" + dt.Rows[i].Field<string>("LocalID") + "'," //LocalID 
         //                   + "'" + dt.Rows[i].Field<string>("DescriptionLocation") + "'," //DescriptionLocation 
         //                   + "'" + dt.Rows[i].Field<string>("AssetNumber") + "'," //AssetNumber 
         //                   + "'" + dt.Rows[i].Field<string>("InstalledMemory") + "'," //InstalledMemory 
         //                   + "'" + dt.Rows[i].Field<string>("FirmwareVersion") + "'," //FirmwareVersion 
         //                   + "'" + dt.Rows[i].Field<string>("FirmwareVersion2") + "'," //FirmwareVersion2 
         //                   + "'" + dt.Rows[i].Field<string>("FirmwareVersion3") + "'," //FirmwareVersion3 
         //                   + "'" + dt.Rows[i].Field<string>("FirmwareVersion4") + "'," //FirmwareVersion4 
         //                   + "'" + dt.Rows[i].Field<string>("InstallationDate") + "'," //InstallationDate 
         //                   + "'" + dt.Rows[i].Field<string>("ServiceContactIsColor") + "'," //ServiceContactIsColor 
         //                   + "'" + dt.Rows[i].Field<string>("IsDuplex") + "'," //IsDuplex 
         //                   + "'" + dt.Rows[i].Field<string>("PowerActive") + "'," //PowerActive 
         //                   + "'" + dt.Rows[i].Field<string>("PowerIdle") + "'," //PowerIdle 
         //                   + "'" + dt.Rows[i].Field<string>("PowerSleep1") + "'," //PowerSleep1 
         //                   + "'" + dt.Rows[i].Field<string>("PowerSleep2") + "'," //PowerSleep2 
         //                   + "'" + dt.Rows[i].Field<string>("TotalPages") + "'," //TotalPages 
         //                   + "'" + dt.Rows[i].Field<string>("TotalPagesMono") + "'," //TotalPagesMono 
         //                   + "'" + dt.Rows[i].Field<string>("TotalPagesColor") + "'," //TotalPagesColor 
         //                   + "'" + dt.Rows[i].Field<string>("TotalPagesFullColor") + "'," //TotalPagesFullColor 
         //                   + "'" + dt.Rows[i].Field<string>("TotalPagesTwoColor") + "'," //TotalPagesTwoColor 
         //                   + "'" + dt.Rows[i].Field<string>("TotalPagesSingleColor") + "'," //TotalPagesSingleColor 
         //                   + "'" + dt.Rows[i].Field<string>("TotalPagesDuplex") + "'," //TotalPagesDuplex 
         //                   + "'" + dt.Rows[i].Field<string>("UsagePages") + "'," //UsagePages 
         //                   + "'" + dt.Rows[i].Field<string>("UsagePagesMono") + "'," //UsagePagesMono 
         //                   + "'" + dt.Rows[i].Field<string>("UsagePagesColor") + "'," //UsagePagesColor 
         //                   + "'" + dt.Rows[i].Field<string>("UsagePagesFullColor") + "'," //UsagePagesFullColor 
         //                   + "'" + dt.Rows[i].Field<string>("UsagePagesTwoColor") + "'," //UsagePagesTwoColor 
         //                   + "'" + dt.Rows[i].Field<string>("UsagePagesSingleColor") + "'," //UsagePagesSingleColor 
         //                   + "'" + dt.Rows[i].Field<string>("PrinterPages") + "'," //PrinterPages 
         //                   + "'" + dt.Rows[i].Field<string>("PrinterPagesMono") + "'," //PrinterPagesMono 
         //                   + "'" + dt.Rows[i].Field<string>("PrinterPagesColor") + "'," //PrinterPagesColor 
         //                   + "'" + dt.Rows[i].Field<string>("PrinterPagesFullColor") + "'," //PrinterPagesFullColor 
         //                   + "'" + dt.Rows[i].Field<string>("PrinterPagesTwoColor") + "'," //PrinterPagesTwoColor 
         //                   + "'" + dt.Rows[i].Field<string>("PrinterPagesSingleColor") +
         //                   "'," //PrinterPagesSingleColor 
         //                   + "'" + dt.Rows[i].Field<string>("CopyPages") + "'," //CopyPages 
         //                   + "'" + dt.Rows[i].Field<string>("CopyPagesMono") + "'," //CopyPagesMono 
         //                   + "'" + dt.Rows[i].Field<string>("CopyPagesColor") + "'," //CopyPagesColor 
         //                   + "'" + dt.Rows[i].Field<string>("CopyPagesFullColor") + "'," //CopyPagesFullColor 
         //                   + "'" + dt.Rows[i].Field<string>("CopyPagesTwoColor") + "'," //CopyPagesTwoColor 
         //                   + "'" + dt.Rows[i].Field<string>("CopyPagesSingleColor") + "'," //CopyPagesSingleColor 
         //                   + "'" + dt.Rows[i].Field<string>("FaxPages") + "'," //FaxPages 
         //                   + "'" + dt.Rows[i].Field<string>("FaxPagesMono") + "'," //FaxPagesMono 
         //                   + "'" + dt.Rows[i].Field<string>("FaxPagesColor") + "'," //FaxPagesColor 
         //                   + "'" + dt.Rows[i].Field<string>("OtherPagesOther") + "'," //OtherPagesOther 
         //                   + "'" + dt.Rows[i].Field<string>("PagesMonoOther") + "'," //PagesMonoOther 
         //                   + "'" + dt.Rows[i].Field<string>("PagesColorOther") + "'," //PagesColorOther 
         //                   + "'" + dt.Rows[i].Field<string>("PagesFullColor") + "'," //PagesFullColor 
         //                   + "'" + dt.Rows[i].Field<string>("OtherPagesTwoColor") + "'," //OtherPagesTwoColor 
         //                   + "'" + dt.Rows[i].Field<string>("OtherPagesSingleColor") + "'," //OtherPagesSingleColor 
         //                   + "'" + dt.Rows[i].Field<string>("FaxesSentFaxesReceived") + "'," //FaxesSentFaxesReceived 
         //                   + "'" + dt.Rows[i].Field<string>("ScansTotalScansTotalMono") +
         //                   "'," //ScansTotalScansTotalMono 
         //                   + "'" + dt.Rows[i].Field<string>("ScansTotalColor") + "'," //ScansTotalColor 
         //                   + "'" + dt.Rows[i].Field<string>("ScansUsageScansUsageMono") +
         //                   "'," //ScansUsageScansUsageMono 
         //                   + "'" + dt.Rows[i].Field<string>("ScansUsageColor") + "'," //ScansUsageColor 
         //                   + "'" + dt.Rows[i].Field<string>("ScansCopy") + "'," //ScansCopy 
         //                   + "'" + dt.Rows[i].Field<string>("ScansCopyMono") + "'," //ScansCopyMono 
         //                   + "'" + dt.Rows[i].Field<string>("ScansCopyColor") + "'," //ScansCopyColor 
         //                   + "'" + dt.Rows[i].Field<string>("ScansFax") + "'," //ScansFax 
         //                   + "'" + dt.Rows[i].Field<string>("ScansFaxMono") + "'," //ScansFaxMono 
         //                   + "'" + dt.Rows[i].Field<string>("ScansFaxColor") + "'," //ScansFaxColor 
         //                   + "'" + dt.Rows[i].Field<string>("Scansemail") + "'," //Scansemail 
         //                   + "'" + dt.Rows[i].Field<string>("ScansemailMono") + "'," //ScansemailMono 
         //                   + "'" + dt.Rows[i].Field<string>("ScansemailColor") + "'," //ScansemailColor 
         //                   + "'" + dt.Rows[i].Field<string>("ScansNet") + "'," //ScansNet 
         //                   + "'" + dt.Rows[i].Field<string>("ScansNetMono") + "'," //ScansNetMono 
         //                   + "'" + dt.Rows[i].Field<string>("ScansNetColor") + "'," //ScansNetColor 
         //                   + "'" + dt.Rows[i].Field<string>("ListPages") + "'," //ListPages 
         //                   + "'" + dt.Rows[i].Field<string>("LargePages") + "'," //LargePages 
         //                   + "'" + dt.Rows[i].Field<string>("LargePagesMono") + "'," //LargePagesMono 
         //                   + "'" + dt.Rows[i].Field<string>("LargePagesColor") + "'," //LargePagesColor 
         //                   + "'" + dt.Rows[i].Field<string>("LargePagesFullColor") + "'," //LargePagesFullColor 
         //                   + "'" + dt.Rows[i].Field<string>("LargePagesTwoColor") + "'," //LargePagesTwoColor 
         //                   + "'" + dt.Rows[i].Field<string>("LargePagesSingleColor") + "'," //LargePagesSingleColor 
         //                   + "'" + dt.Rows[i].Field<string>("TotalLargeSheets") + "'," //TotalLargeSheets 
         //                   + "'" + dt.Rows[i].Field<string>("SquareFeetSquareMeters") + "'," //SquareFeetSquareMeters 
         //                   + "'" + dt.Rows[i].Field<string>("LinearFeetStapledSets") + "'," //LinearFeetStapledSets 
         //                   + "'" + dt.Rows[i].Field<string>("Level1Pages") + "'," //Level1Pages 
         //                   + "'" + dt.Rows[i].Field<string>("Level2Pages") + "'," //Level2Pages 
         //                   + "'" + dt.Rows[i].Field<string>("Level3Pages") + "'," //Level3Pages 
         //                   + "'" + dt.Rows[i].Field<string>("ColorUsageOffice") + "'," //ColorUsageOffice 
         //                   + "'" + dt.Rows[i].Field<string>("ColorUsageOfficeAccent") + "'," //ColorUsageOfficeAccent 
         //                   + "'" + dt.Rows[i].Field<string>("ColorUsageProfessional") + "'," //ColorUsageProfessional 
         //                   + "'" + dt.Rows[i].Field<string>("ColorUsageProfessionalAccent") +
         //                   "'," //ColorUsageProfessionalAccent
         //                   + "'" + dt.Rows[i].Field<string>("DoubleClickTotal") + "'," //DoubleClickTotal 
         //                   + "'" + dt.Rows[i].Field<string>("DoubleClickMono") + "'," //DoubleClickMono 
         //                   + "'" + dt.Rows[i].Field<string>("DoubleClickColor") + "'," //DoubleClickColor 
         //                   + "'" + dt.Rows[i].Field<string>("DoubleClickFullColor") + "'," //DoubleClickFullColor 
         //                   + "'" + dt.Rows[i].Field<string>("DoubleClickTwoColor") + "'," //DoubleClickTwoColor 
         //                   + "'" + dt.Rows[i].Field<string>("DoubleClickSingleColor") + "'," //DoubleClickSingleColor 
         //                   + "'" + dt.Rows[i].Field<string>("DoubleClickDuplex") + "'," //DoubleClickDuplex 
         //                   + "'" + dt.Rows[i].Field<string>("DevelopmentTotal") + "'," //DevelopmentTotal 
         //                   + "'" + dt.Rows[i].Field<string>("DevelopmentMono") + "'," //DevelopmentMono 
         //                   + "'" + dt.Rows[i].Field<string>("DevelopmentColor") + "'," //DevelopmentColor 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageAverageBlack") + "'," //CoverageAverageBlack 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageAverageCyan") + "'," //CoverageAverageCyan 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageAverageMagenta") + "'," //CoverageAverageMagenta 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageAverageYellow") + "'," //CoverageAverageYellow 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageSumBlack") + "'," //CoverageSumBlack 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageSumCyan") + "'," //CoverageSumCyan 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageSumMagenta") + "'," //CoverageSumMagenta 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageSumYellow") + "'," //CoverageSumYellow 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageSum2Black") + "'," //CoverageSum2Black 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageSum2Cyan") + "'," //CoverageSum2Cyan 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageSum2Magenta") + "'," //CoverageSum2Magenta 
         //                   + "'" + dt.Rows[i].Field<string>("CoverageSum2Yellow") + "'," //CoverageSum2Yellow 
         //                   + "'" + dt.Rows[i].Field<string>("MeterGroup1") + "'," //MeterGroup1 
         //                   + "'" + dt.Rows[i].Field<string>("MeterGroup2") + "'," //MeterGroup2
         //                   + "'" + dt.Rows[i].Field<string>("BlackLevelMax") + "',"
         //                   + "'" + dt.Rows[i].Field<string>("CyanLevelMax") + "',"
         //                   + "'" + dt.Rows[i].Field<string>("MagentaLevelMax") + "',"
         //                   + "'" + dt.Rows[i].Field<string>("YellowLevelMax") + "',"
         //                   + "'" + dt.Rows[i].Field<string>("BlackLevel") + "',"
         //                   + "'" + dt.Rows[i].Field<string>("CyanLevel") + "',"
         //                   + "'" + dt.Rows[i].Field<string>("MagentaLevel") + "',"
         //                   + "'" + dt.Rows[i].Field<string>("YellowLevel") + "'"
         //                   + ");"
         //               );
         //           Config.GetInstance().NonQuery("Update INFO   set OIDVersion = " + fileOID + ";");
         //
         //           MessageBox.Show("Update erfolgreich", "Infomation", MessageBoxButton.OK,
         //               MessageBoxImage.Information);
         //       }
         //   }
         //   else
         //   {
         //       MessageBox.Show("Sie verwenden bereits die neuste OID-Version", "Infomation", MessageBoxButton.OK,
         //           MessageBoxImage.Information);
         //   }
        }

        /// <summary>
        ///     Writes to table.
        ///     Arraybelegung
        ///     [0]   Manufacturer
        ///     [1]   Model
        ///     [2]   SerialNumber
        ///     [3]   MACAddress
        ///     [4]   IPAddresse
        ///     [5]   HostName
        ///     [6]   LocalID
        ///     [7]   DescriptionLocation
        ///     [8]   AssetNumber
        ///     [9]   InstalledMemory
        ///     [10]  FirmwareVersion
        ///     [11]  FirmwareVersion2
        ///     [12]  FirmwareVersion3
        ///     [13]  FirmwareVersion4
        ///     [14]  InstallationDate
        ///     [15]  ServiceContactIsColor
        ///     [16]  IsDuplex
        ///     [17]  PowerActive
        ///     [18]  PowerIdle
        ///     [19]  PowerSleep1
        ///     [20]  PowerSleep2
        ///     [21]  TotalPages
        ///     [22]  TotalPagesMono
        ///     [23]  TotalPagesColor
        ///     [24]  TotalPagesFullColor
        ///     [25]  TotalPagesTwoColor
        ///     [26]  TotalPagesSingleColor
        ///     [27]  TotalPagesDuplex
        ///     [28]  UsagePages
        ///     [29]  UsagePagesMono
        ///     [30]  UsagePagesColor
        ///     [31]  UsagePagesFullColor
        ///     [32]  UsagePagesTwoColor
        ///     [33]  UsagePagesSingleColor
        ///     [34]  PrinterPages
        ///     [35]  PrinterPagesMono
        ///     [36]  PrinterPagesColor
        ///     [37]  PrinterPagesFullColor
        ///     [38]  PrinterPagesTwoColor
        ///     [39]  PrinterPagesSingleColor
        ///     [40]  CopyPages
        ///     [41]  CopyPagesMono
        ///     [42]  CopyPagesColor
        ///     [43]  CopyPagesFullColor
        ///     [44]  CopyPagesTwoColor
        ///     [45]  CopyPagesSingleColor
        ///     [46]  FaxPages
        ///     [47]  FaxPagesMono
        ///     [48]  FaxPagesColor
        ///     [49]  OtherPagesOther
        ///     [50]  PagesMonoOther
        ///     [51]  PagesColorOther
        ///     [52]  PagesFullColor
        ///     [53]  OtherPagesTwoColor
        ///     [54]  OtherPagesSingleColor
        ///     [55]  FaxesSentFaxesReceived
        ///     [56]  ScansTotalScansTotalMono
        ///     [57]  ScansTotalColor
        ///     [58]  ScansUsageScansUsageMono
        ///     [59]  ScansUsageColor
        ///     [60]  ScansCopy
        ///     [61]  ScansCopyMono
        ///     [62]  ScansCopyColor
        ///     [63]  ScansFax
        ///     [64]  ScansFaxMono
        ///     [65]  ScansFaxColor
        ///     [66]  Scansemail
        ///     [67]  ScansemailMono
        ///     [68]  ScansemailColor
        ///     [69]  ScansNet
        ///     [70]  ScansNetMono
        ///     [71]  ScansNetColor
        ///     [72]  ListPages
        ///     [73]  LargePages
        ///     [74]  LargePagesMono
        ///     [75]  LargePagesColor
        ///     [76]  LargePagesFullColor
        ///     [77]  LargePagesTwoColor
        ///     [78]  LargePagesSingleColor
        ///     [79]  TotalLargeSheets
        ///     [80]  SquareFeetSquareMeters
        ///     [81]  LinearFeetStapledSets
        ///     [82]  Level1Pages
        ///     [83]  Level2Pages
        ///     [84]  Level3Pages
        ///     [85]  ColorUsageOffice
        ///     [86]  ColorUsageOfficeAccent
        ///     [87]  ColorUsageProfessional
        ///     [88]  ColorUsageProfessionalAccent
        ///     [89]  DoubleClickTotal
        ///     [90]  DoubleClickMono
        ///     [91]  DoubleClickColor
        ///     [92]  DoubleClickFullColor
        ///     [93]  DoubleClickTwoColor
        ///     [94]  DoubleClickSingleColor
        ///     [95]  DoubleClickDuplex
        ///     [96]  DevelopmentTotal
        ///     [97]  DevelopmentMono
        ///     [98]  DevelopmentColor
        ///     [99]  CoverageAverageBlack
        ///     [100] CoverageAverageCyan
        ///     [101] CoverageAverageMagenta
        ///     [102] CoverageAverageYellow
        ///     [103] CoverageSumBlack
        ///     [104] CoverageSumCyan
        ///     [105] CoverageSumMagenta
        ///     [106] CoverageSumYellow
        ///     [107] CoverageSum2Black
        ///     [108] CoverageSum2Cyan
        ///     [109] CoverageSum2Magenta
        ///     [110] CoverageSum2Yellow
        ///     [111] MeterGroup1
        ///     [112] MeterGroup2
        ///     [113] Erfassungsdatum
        ///     Kundennummer
        /// </summary>
        /// <param name="s">The String Array</param>
        public static void writeToExternMYSQL()
        {
            var dt = Collector.shellDT("Select * from Collector");
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var cmd =
                    "insert into snmp.snmpGEN2 "
                    + "("
                    + "Manufacturer,"
                    + "Model,"
                    + "SerialNumber,"
                    + "MACAddress,"
                    + "IPAddresse,"
                    + "HostName,"
                    + "LocalID,"
                    + "DescriptionLocation,"
                    + "AssetNumber,"
                    + "InstalledMemory,"
                    + "FirmwareVersion,"
                    + "FirmwareVersion2,"
                    + "FirmwareVersion3,"
                    + "FirmwareVersion4,"
                    + "InstallationDate,"
                    + "ServiceContactIsColor,"
                    + "IsDuplex,"
                    + "PowerActive,"
                    + "PowerIdle,"
                    + "PowerSleep1,"
                    + "PowerSleep2,"
                    + "TotalPages,"
                    + "TotalPagesMono,"
                    + "TotalPagesColor,"
                    + "TotalPagesFullColor,"
                    + "TotalPagesTwoColor,"
                    + "TotalPagesSingleColor,"
                    + "TotalPagesDuplex,"
                    + "UsagePages,"
                    + "UsagePagesMono,"
                    + "UsagePagesColor,"
                    + "UsagePagesFullColor,"
                    + "UsagePagesTwoColor,"
                    + "UsagePagesSingleColor,"
                    + "PrinterPages,"
                    + "PrinterPagesMono,"
                    + "PrinterPagesColor,"
                    + "PrinterPagesFullColor,"
                    + "PrinterPagesTwoColor,"
                    + "PrinterPagesSingleColor,"
                    + "CopyPages,"
                    + "CopyPagesMono,"
                    + "CopyPagesColor,"
                    + "CopyPagesFullColor,"
                    + "CopyPagesTwoColor,"
                    + "CopyPagesSingleColor,"
                    + "FaxPages,"
                    + "FaxPagesMono,"
                    + "FaxPagesColor,"
                    + "OtherPagesOther,"
                    + "PagesMonoOther,"
                    + "PagesColorOther,"
                    + "PagesFullColor,"
                    + "OtherPagesTwoColor,"
                    + "OtherPagesSingleColor,"
                    + "FaxesSentFaxesReceived,"
                    + "ScansTotalScansTotalMono,"
                    + "ScansTotalColor,"
                    + "ScansUsageScansUsageMono,"
                    + "ScansUsageColor,"
                    + "ScansCopy,"
                    + "ScansCopyMono,"
                    + "ScansCopyColor,"
                    + "ScansFax,"
                    + "ScansFaxMono,"
                    + "ScansFaxColor,"
                    + "ScansEmail,"
                    + "ScansEmailMono,"
                    + "ScansEmailColor,"
                    + "ScansNet,"
                    + "ScansNetMono,"
                    + "ScansNetColor,"
                    + "ListPages,"
                    + "LargePages,"
                    + "LargePagesMono,"
                    + "LargePagesColor,"
                    + "LargePagesFullColor,"
                    + "LargePagesTwoColor,"
                    + "LargePagesSingleColor,"
                    + "TotalLargeSheets,"
                    + "SquareFeetSquareMeters,"
                    + "LinearFeetStapledSets,"
                    + "Level1Pages,"
                    + "Level2Pages,"
                    + "Level3Pages,"
                    + "ColorUsageOffice,"
                    + "ColorUsageOfficeAccent,"
                    + "ColorUsageProfessional,"
                    + "ColorUsageProfessionalAccent,"
                    + "DoubleClickTotal,"
                    + "DoubleClickMono,"
                    + "DoubleClickColor,"
                    + "DoubleClickFullColor,"
                    + "DoubleClickTwoColor,"
                    + "DoubleClickSingleColor,"
                    + "DoubleClickDuplex,"
                    + "DevelopmentTotal,"
                    + "DevelopmentMono,"
                    + "DevelopmentColor,"
                    + "CoverageAverageBlack,"
                    + "CoverageAverageCyan,"
                    + "CoverageAverageMagenta,"
                    + "CoverageAverageYellow,"
                    + "CoverageSumBlack,"
                    + "CoverageSumCyan,"
                    + "CoverageSumMagenta,"
                    + "CoverageSumYellow,"
                    + "CoverageSum2Black,"
                    + "CoverageSum2Cyan,"
                    + "CoverageSum2Magenta,"
                    + "CoverageSum2Yellow,"
                    + "MeterGroup1,"
                    + "MeterGroup2,"
                    + "ErfassungsDatum,"
                    + "Kundennummer,"
                    + "mpsversion,"
                    + "oidversion"
                    //+ "BlackLevelMax,"
                    //+ "CyanLevelMax,"
                    // + "MagentaLevelMax,"
                    //+ "YellowLevelMax,"
                    //+ "BlackLevel,"
                    //+ "CyanLevel,"
                    //+ "MagentaLevel,"
                    //+ "YellowLevel"
                    + ")"
                    + " Values"
                    + " ("
                    + "'" + dt.Rows[i].Field<string>("Manufacturer") + "'," //Manufacturer 
                    + "'" + dt.Rows[i].Field<string>("Model") + "'," //Model 
                    + "'" + dt.Rows[i].Field<string>("SerialNumber") + "'," //SerialNumber 
                    + "'" + dt.Rows[i].Field<string>("MACAddress") + "'," //MACAddress 
                    + "'" + dt.Rows[i].Field<string>("IPAddresse") + "'," //IPAddresse 
                    + "'" + dt.Rows[i].Field<string>("HostName") + "'," //HostName 
                    + "'" + dt.Rows[i].Field<string>("LocalID") + "'," //LocalID 
                    + "'" + dt.Rows[i].Field<string>("DescriptionLocation") + "'," //DescriptionLocation 
                    + "'" + dt.Rows[i].Field<string>("AssetNumber") + "'," //AssetNumber 
                    + "'" + dt.Rows[i].Field<string>("InstalledMemory") + "'," //InstalledMemory 
                    + "'" + dt.Rows[i].Field<string>("FirmwareVersion") + "'," //FirmwareVersion 
                    + "'" + dt.Rows[i].Field<string>("FirmwareVersion2") + "'," //FirmwareVersion2 
                    + "'" + dt.Rows[i].Field<string>("FirmwareVersion3") + "'," //FirmwareVersion3 
                    + "'" + dt.Rows[i].Field<string>("FirmwareVersion4") + "'," //FirmwareVersion4 
                    + "'" + dt.Rows[i].Field<string>("InstallationDate") + "'," //InstallationDate 
                    + "'" + dt.Rows[i].Field<string>("ServiceContactIsColor") + "'," //ServiceContactIsColor 
                    + "'" + dt.Rows[i].Field<string>("IsDuplex") + "'," //IsDuplex 
                    + "'" + dt.Rows[i].Field<string>("PowerActive") + "'," //PowerActive 
                    + "'" + dt.Rows[i].Field<string>("PowerIdle") + "'," //PowerIdle 
                    + "'" + dt.Rows[i].Field<string>("PowerSleep1") + "'," //PowerSleep1 
                    + "'" + dt.Rows[i].Field<string>("PowerSleep2") + "'," //PowerSleep2 
                    + +dt.Rows[i].Field<long>("TotalPages") + "," //TotalPages 
                    + +dt.Rows[i].Field<long>("TotalPagesMono") + "," //TotalPagesMono 
                    + +dt.Rows[i].Field<long>("TotalPagesColor") + "," //TotalPagesColor 
                    + +dt.Rows[i].Field<long>("TotalPagesFullColor") + "," //TotalPagesFullColor 
                    + +dt.Rows[i].Field<long>("TotalPagesTwoColor") + "," //TotalPagesTwoColor 
                    + +dt.Rows[i].Field<long>("TotalPagesSingleColor") + "," //TotalPagesSingleColor 
                    + +dt.Rows[i].Field<long>("TotalPagesDuplex") + "," //TotalPagesDuplex 
                    + +dt.Rows[i].Field<long>("UsagePages") + "," //UsagePages 
                    + +dt.Rows[i].Field<long>("UsagePagesMono") + "," //UsagePagesMono 
                    + +dt.Rows[i].Field<long>("UsagePagesColor") + "," //UsagePagesColor 
                    + +dt.Rows[i].Field<long>("UsagePagesFullColor") + "," //UsagePagesFullColor 
                    + +dt.Rows[i].Field<long>("UsagePagesTwoColor") + "," //UsagePagesTwoColor 
                    + +dt.Rows[i].Field<long>("UsagePagesSingleColor") + "," //UsagePagesSingleColor 
                    + +dt.Rows[i].Field<long>("PrinterPages") + "," //PrinterPages 
                    + +dt.Rows[i].Field<long>("PrinterPagesMono") + "," //PrinterPagesMono 
                    + +dt.Rows[i].Field<long>("PrinterPagesColor") + "," //PrinterPagesColor 
                    + +dt.Rows[i].Field<long>("PrinterPagesFullColor") + "," //PrinterPagesFullColor 
                    + +dt.Rows[i].Field<long>("PrinterPagesTwoColor") + "," //PrinterPagesTwoColor 
                    + +dt.Rows[i].Field<long>("PrinterPagesSingleColor") + "," //PrinterPagesSingleColor 
                    + +dt.Rows[i].Field<long>("CopyPages") + "," //CopyPages 
                    + +dt.Rows[i].Field<long>("CopyPagesMono") + "," //CopyPagesMono 
                    + +dt.Rows[i].Field<long>("CopyPagesColor") + "," //CopyPagesColor 
                    + +dt.Rows[i].Field<long>("CopyPagesFullColor") + "," //CopyPagesFullColor 
                    + +dt.Rows[i].Field<long>("CopyPagesTwoColor") + "," //CopyPagesTwoColor 
                    + +dt.Rows[i].Field<long>("CopyPagesSingleColor") + "," //CopyPagesSingleColor 
                    + +dt.Rows[i].Field<long>("FaxPages") + "," //FaxPages 
                    + +dt.Rows[i].Field<long>("FaxPagesMono") + "," //FaxPagesMono 
                    + +dt.Rows[i].Field<long>("FaxPagesColor") + "," //FaxPagesColor 
                    + +dt.Rows[i].Field<long>("OtherPagesOther") + "," //OtherPagesOther 
                    + +dt.Rows[i].Field<long>("PagesMonoOther") + "," //PagesMonoOther 
                    + +dt.Rows[i].Field<long>("PagesColorOther") + "," //PagesColorOther 
                    + +dt.Rows[i].Field<long>("PagesFullColor") + "," //PagesFullColor 
                    + +dt.Rows[i].Field<long>("OtherPagesTwoColor") + "," //OtherPagesTwoColor 
                    + +dt.Rows[i].Field<long>("OtherPagesSingleColor") + "," //OtherPagesSingleColor 
                    + +dt.Rows[i].Field<long>("FaxesSentFaxesReceived") + "," //FaxesSentFaxesReceived 
                    + +dt.Rows[i].Field<long>("ScansTotalScansTotalMono") + "," //ScansTotalScansTotalMono 
                    + +dt.Rows[i].Field<long>("ScansTotalColor") + "," //ScansTotalColor 
                    + +dt.Rows[i].Field<long>("ScansUsageScansUsageMono") + "," //ScansUsageScansUsageMono 
                    + +dt.Rows[i].Field<long>("ScansUsageColor") + "," //ScansUsageColor 
                    + +dt.Rows[i].Field<long>("ScansCopy") + "," //ScansCopy 
                    + +dt.Rows[i].Field<long>("ScansCopyMono") + "," //ScansCopyMono 
                    + +dt.Rows[i].Field<long>("ScansCopyColor") + "," //ScansCopyColor 
                    + +dt.Rows[i].Field<long>("ScansFax") + "," //ScansFax 
                    + +dt.Rows[i].Field<long>("ScansFaxMono") + "," //ScansFaxMono 
                    + +dt.Rows[i].Field<long>("ScansFaxColor") + "," //ScansFaxColor 
                    + +dt.Rows[i].Field<long>("Scansemail") + "," //Scansemail 
                    + +dt.Rows[i].Field<long>("ScansemailMono") + "," //ScansemailMono 
                    + +dt.Rows[i].Field<long>("ScansemailColor") + "," //ScansemailColor 
                    + +dt.Rows[i].Field<long>("ScansNet") + "," //ScansNet 
                    + +dt.Rows[i].Field<long>("ScansNetMono") + "," //ScansNetMono 
                    + +dt.Rows[i].Field<long>("ScansNetColor") + "," //ScansNetColor 
                    + +dt.Rows[i].Field<long>("ListPages") + "," //ListPages 
                    + +dt.Rows[i].Field<long>("LargePages") + "," //LargePages 
                    + +dt.Rows[i].Field<long>("LargePagesMono") + "," //LargePagesMono 
                    + +dt.Rows[i].Field<long>("LargePagesColor") + "," //LargePagesColor 
                    + +dt.Rows[i].Field<long>("LargePagesFullColor") + "," //LargePagesFullColor 
                    + +dt.Rows[i].Field<long>("LargePagesTwoColor") + "," //LargePagesTwoColor 
                    + +dt.Rows[i].Field<long>("LargePagesSingleColor") + "," //LargePagesSingleColor 
                    + +dt.Rows[i].Field<long>("TotalLargeSheets") + "," //TotalLargeSheets 
                    + +dt.Rows[i].Field<long>("SquareFeetSquareMeters") + "," //SquareFeetSquareMeters 
                    + +dt.Rows[i].Field<long>("LinearFeetStapledSets") + "," //LinearFeetStapledSets 
                    + +dt.Rows[i].Field<long>("Level1Pages") + "," //Level1Pages 
                    + +dt.Rows[i].Field<long>("Level2Pages") + "," //Level2Pages 
                    + +dt.Rows[i].Field<long>("Level3Pages") + "," //Level3Pages 
                    + +dt.Rows[i].Field<long>("ColorUsageOffice") + "," //ColorUsageOffice 
                    + +dt.Rows[i].Field<long>("ColorUsageOfficeAccent") + "," //ColorUsageOfficeAccent 
                    + +dt.Rows[i].Field<long>("ColorUsageProfessional") + "," //ColorUsageProfessional 
                    + +dt.Rows[i].Field<long>("ColorUsageProfessionalAccent") + "," //ColorUsageProfessionalAccent
                    + +dt.Rows[i].Field<long>("DoubleClickTotal") + "," //DoubleClickTotal 
                    + +dt.Rows[i].Field<long>("DoubleClickMono") + "," //DoubleClickMono 
                    + +dt.Rows[i].Field<long>("DoubleClickColor") + "," //DoubleClickColor 
                    + +dt.Rows[i].Field<long>("DoubleClickFullColor") + "," //DoubleClickFullColor 
                    + +dt.Rows[i].Field<long>("DoubleClickTwoColor") + "," //DoubleClickTwoColor 
                    + +dt.Rows[i].Field<long>("DoubleClickSingleColor") + "," //DoubleClickSingleColor 
                    + +dt.Rows[i].Field<long>("DoubleClickDuplex") + "," //DoubleClickDuplex 
                    + +dt.Rows[i].Field<long>("DevelopmentTotal") + "," //DevelopmentTotal 
                    + +dt.Rows[i].Field<long>("DevelopmentMono") + "," //DevelopmentMono 
                    + +dt.Rows[i].Field<long>("DevelopmentColor") + "," //DevelopmentColor 
                    + +dt.Rows[i].Field<long>("CoverageAverageBlack") + "," //CoverageAverageBlack 
                    + +dt.Rows[i].Field<long>("CoverageAverageCyan") + "," //CoverageAverageCyan 
                    + +dt.Rows[i].Field<long>("CoverageAverageMagenta") + "," //CoverageAverageMagenta 
                    + +dt.Rows[i].Field<long>("CoverageAverageYellow") + "," //CoverageAverageYellow 
                    + +dt.Rows[i].Field<long>("CoverageSumBlack") + "," //CoverageSumBlack 
                    + +dt.Rows[i].Field<long>("CoverageSumCyan") + "," //CoverageSumCyan 
                    + +dt.Rows[i].Field<long>("CoverageSumMagenta") + "," //CoverageSumMagenta 
                    + +dt.Rows[i].Field<long>("CoverageSumYellow") + "," //CoverageSumYellow 
                    + +dt.Rows[i].Field<long>("CoverageSum2Black") + "," //CoverageSum2Black 
                    + +dt.Rows[i].Field<long>("CoverageSum2Cyan") + "," //CoverageSum2Cyan 
                    + +dt.Rows[i].Field<long>("CoverageSum2Magenta") + "," //CoverageSum2Magenta 
                    + +dt.Rows[i].Field<long>("CoverageSum2Yellow") + "," //CoverageSum2Yellow 
                    + "'" + dt.Rows[i].Field<string>("MeterGroup1") + "'," //MeterGroup1 
                    + "'" + dt.Rows[i].Field<string>("MeterGroup2") + "'," //MeterGroup2
                    + "'" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "',"
                    + "'" + SerialManager.GetSerialContainer(UserInterface.MainWindow.Programid).CustomerIdentificationNumber +
                    "'," //Kundennummer
                    +""// Config.GetInstance().Query("Select version from INFO").Rows[0].Field<long>("version") + ","
                    +""// Config.GetInstance().Query("Select OIDVersion from INFO").Rows[0].Field<long>("OIDVersion") + ""

                    //  + "'" + dt.Rows[i].Field<int>("BlackLevelMax") + "',"
                    //  + "'" + dt.Rows[i].Field<int>("CyanLevelMax") + "',"
                    //                                                                               + "'" + dt.Rows[i].Field<int>("MagentaLevelMax") + "',"
                    //                                                                               + "'" + dt.Rows[i].Field<int>("YellowLevelMax") + "',"
                    //                                                                               + "'" + dt.Rows[i].Field<int>("BlackLevel") + "',"
                    //                                                                               + "'" + dt.Rows[i].Field<int>("CyanLevel") + "',"
                    //                                                                               //+ "'" + dt.Rows[i].Field<int>("MagentaLevel") + "',"
                    //+ "'" + dt.Rows[i].Field<int>("YellowLevel") + "',"
                    + ");";

                shellDT(cmd);
            }
        }
    }
}