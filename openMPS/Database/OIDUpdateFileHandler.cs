#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Runtime.CompilerServices;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using de.fearvel.net.SQL.Connector;
using de.fearvel.openMPS.Database.Exceptions;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public class OidUpdateFileHandler
    {
        private SqliteConnector _connection;
        private static OidUpdateFileHandler _instance;
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static OidUpdateFileHandler GetInstance()
        {
            return _instance ?? (_instance = new OidUpdateFileHandler());
        }
        private OidUpdateFileHandler() { }

        public void Update(string fileName)
        {
            Open(fileName);
            UpdateOid();
            _connection.Close();
        }
        private void Open(string fileName)
        {
            try
            {
                _connection = new SqliteConnector(fileName, GetKey());
            }
            catch (Exception e)
            {
                throw new OidUpdateFileException(e.ToString());
            }
        }

        private string GetKey()
        {
            var sha256 = new SHA256CryptoServiceProvider();
            return Convert.ToBase64String(
                sha256.ComputeHash(Encoding.ASCII.GetBytes(Config.GetInstance().Directory["UUID"])));
        }

        private void UpdateOid()
        {
            _connection.Query("Select val from Directory Where Identifier = 'OID-Version'", out DataTable fileOidDt);
            var fileOid = fileOidDt.Rows[0].Field<string>("val");
            ;
            var version = Oid.GetInstance().Query("Select val from Directory Where Identifier = 'OID-Version'").Rows[0].Field<string>("val");
            if (version.CompareTo(fileOid) < 0)

            {
                _connection.Query("Select * from OID", out DataTable dt);

                if (dt.Rows.Count > 0)
                {
                    Oid.GetInstance().NonQuery("Delete from OID");
                    Oid.GetInstance().NonQuery("delete from sqlite_sequence where name = 'OID';");


                    for (var i = 0; i < dt.Rows.Count; i++)
                        Oid.GetInstance().NonQuery(
                            "insert into OID"
                            + "("
                            + "HerstellerName,"
                            + "OIDPrivateID,"
                            + "ProfileName,"
                            + "DeviceName,"
                            + "DeviceType,"
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
                            + "BlackLevelMax,"
                            + "CyanLevelMax,"
                            + "MagentaLevelMax,"
                            + "YellowLevelMax,"
                            + "BlackLevel,"
                            + "CyanLevel,"
                            + "MagentaLevel,"
                            + "YellowLevel"
                            + ")"
                            + " Values"
                            + " ("
                            + "'" + dt.Rows[i].Field<string>("HerstellerName") + "'," //HerstellerName 
                            + "'" + dt.Rows[i].Field<string>("OIDPrivateID") + "'," //OIDPrivateID 
                            + "'" + dt.Rows[i].Field<string>("ProfileName") + "'," //ProfileName 
                            + "'" + dt.Rows[i].Field<string>("DeviceName") + "'," //DeviceName 
                            + "'" + dt.Rows[i].Field<string>("DeviceType") + "'," //DeviceType 
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
                            + "'" + dt.Rows[i].Field<string>("TotalPages") + "'," //TotalPages 
                            + "'" + dt.Rows[i].Field<string>("TotalPagesMono") + "'," //TotalPagesMono 
                            + "'" + dt.Rows[i].Field<string>("TotalPagesColor") + "'," //TotalPagesColor 
                            + "'" + dt.Rows[i].Field<string>("TotalPagesFullColor") + "'," //TotalPagesFullColor 
                            + "'" + dt.Rows[i].Field<string>("TotalPagesTwoColor") + "'," //TotalPagesTwoColor 
                            + "'" + dt.Rows[i].Field<string>("TotalPagesSingleColor") + "'," //TotalPagesSingleColor 
                            + "'" + dt.Rows[i].Field<string>("TotalPagesDuplex") + "'," //TotalPagesDuplex 
                            + "'" + dt.Rows[i].Field<string>("UsagePages") + "'," //UsagePages 
                            + "'" + dt.Rows[i].Field<string>("UsagePagesMono") + "'," //UsagePagesMono 
                            + "'" + dt.Rows[i].Field<string>("UsagePagesColor") + "'," //UsagePagesColor 
                            + "'" + dt.Rows[i].Field<string>("UsagePagesFullColor") + "'," //UsagePagesFullColor 
                            + "'" + dt.Rows[i].Field<string>("UsagePagesTwoColor") + "'," //UsagePagesTwoColor 
                            + "'" + dt.Rows[i].Field<string>("UsagePagesSingleColor") + "'," //UsagePagesSingleColor 
                            + "'" + dt.Rows[i].Field<string>("PrinterPages") + "'," //PrinterPages 
                            + "'" + dt.Rows[i].Field<string>("PrinterPagesMono") + "'," //PrinterPagesMono 
                            + "'" + dt.Rows[i].Field<string>("PrinterPagesColor") + "'," //PrinterPagesColor 
                            + "'" + dt.Rows[i].Field<string>("PrinterPagesFullColor") + "'," //PrinterPagesFullColor 
                            + "'" + dt.Rows[i].Field<string>("PrinterPagesTwoColor") + "'," //PrinterPagesTwoColor 
                            + "'" + dt.Rows[i].Field<string>("PrinterPagesSingleColor") +
                            "'," //PrinterPagesSingleColor 
                            + "'" + dt.Rows[i].Field<string>("CopyPages") + "'," //CopyPages 
                            + "'" + dt.Rows[i].Field<string>("CopyPagesMono") + "'," //CopyPagesMono 
                            + "'" + dt.Rows[i].Field<string>("CopyPagesColor") + "'," //CopyPagesColor 
                            + "'" + dt.Rows[i].Field<string>("CopyPagesFullColor") + "'," //CopyPagesFullColor 
                            + "'" + dt.Rows[i].Field<string>("CopyPagesTwoColor") + "'," //CopyPagesTwoColor 
                            + "'" + dt.Rows[i].Field<string>("CopyPagesSingleColor") + "'," //CopyPagesSingleColor 
                            + "'" + dt.Rows[i].Field<string>("FaxPages") + "'," //FaxPages 
                            + "'" + dt.Rows[i].Field<string>("FaxPagesMono") + "'," //FaxPagesMono 
                            + "'" + dt.Rows[i].Field<string>("FaxPagesColor") + "'," //FaxPagesColor 
                            + "'" + dt.Rows[i].Field<string>("OtherPagesOther") + "'," //OtherPagesOther 
                            + "'" + dt.Rows[i].Field<string>("PagesMonoOther") + "'," //PagesMonoOther 
                            + "'" + dt.Rows[i].Field<string>("PagesColorOther") + "'," //PagesColorOther 
                            + "'" + dt.Rows[i].Field<string>("PagesFullColor") + "'," //PagesFullColor 
                            + "'" + dt.Rows[i].Field<string>("OtherPagesTwoColor") + "'," //OtherPagesTwoColor 
                            + "'" + dt.Rows[i].Field<string>("OtherPagesSingleColor") + "'," //OtherPagesSingleColor 
                            + "'" + dt.Rows[i].Field<string>("FaxesSentFaxesReceived") + "'," //FaxesSentFaxesReceived 
                            + "'" + dt.Rows[i].Field<string>("ScansTotalScansTotalMono") +
                            "'," //ScansTotalScansTotalMono 
                            + "'" + dt.Rows[i].Field<string>("ScansTotalColor") + "'," //ScansTotalColor 
                            + "'" + dt.Rows[i].Field<string>("ScansUsageScansUsageMono") +
                            "'," //ScansUsageScansUsageMono 
                            + "'" + dt.Rows[i].Field<string>("ScansUsageColor") + "'," //ScansUsageColor 
                            + "'" + dt.Rows[i].Field<string>("ScansCopy") + "'," //ScansCopy 
                            + "'" + dt.Rows[i].Field<string>("ScansCopyMono") + "'," //ScansCopyMono 
                            + "'" + dt.Rows[i].Field<string>("ScansCopyColor") + "'," //ScansCopyColor 
                            + "'" + dt.Rows[i].Field<string>("ScansFax") + "'," //ScansFax 
                            + "'" + dt.Rows[i].Field<string>("ScansFaxMono") + "'," //ScansFaxMono 
                            + "'" + dt.Rows[i].Field<string>("ScansFaxColor") + "'," //ScansFaxColor 
                            + "'" + dt.Rows[i].Field<string>("Scansemail") + "'," //Scansemail 
                            + "'" + dt.Rows[i].Field<string>("ScansemailMono") + "'," //ScansemailMono 
                            + "'" + dt.Rows[i].Field<string>("ScansemailColor") + "'," //ScansemailColor 
                            + "'" + dt.Rows[i].Field<string>("ScansNet") + "'," //ScansNet 
                            + "'" + dt.Rows[i].Field<string>("ScansNetMono") + "'," //ScansNetMono 
                            + "'" + dt.Rows[i].Field<string>("ScansNetColor") + "'," //ScansNetColor 
                            + "'" + dt.Rows[i].Field<string>("ListPages") + "'," //ListPages 
                            + "'" + dt.Rows[i].Field<string>("LargePages") + "'," //LargePages 
                            + "'" + dt.Rows[i].Field<string>("LargePagesMono") + "'," //LargePagesMono 
                            + "'" + dt.Rows[i].Field<string>("LargePagesColor") + "'," //LargePagesColor 
                            + "'" + dt.Rows[i].Field<string>("LargePagesFullColor") + "'," //LargePagesFullColor 
                            + "'" + dt.Rows[i].Field<string>("LargePagesTwoColor") + "'," //LargePagesTwoColor 
                            + "'" + dt.Rows[i].Field<string>("LargePagesSingleColor") + "'," //LargePagesSingleColor 
                            + "'" + dt.Rows[i].Field<string>("TotalLargeSheets") + "'," //TotalLargeSheets 
                            + "'" + dt.Rows[i].Field<string>("SquareFeetSquareMeters") + "'," //SquareFeetSquareMeters 
                            + "'" + dt.Rows[i].Field<string>("LinearFeetStapledSets") + "'," //LinearFeetStapledSets 
                            + "'" + dt.Rows[i].Field<string>("Level1Pages") + "'," //Level1Pages 
                            + "'" + dt.Rows[i].Field<string>("Level2Pages") + "'," //Level2Pages 
                            + "'" + dt.Rows[i].Field<string>("Level3Pages") + "'," //Level3Pages 
                            + "'" + dt.Rows[i].Field<string>("ColorUsageOffice") + "'," //ColorUsageOffice 
                            + "'" + dt.Rows[i].Field<string>("ColorUsageOfficeAccent") + "'," //ColorUsageOfficeAccent 
                            + "'" + dt.Rows[i].Field<string>("ColorUsageProfessional") + "'," //ColorUsageProfessional 
                            + "'" + dt.Rows[i].Field<string>("ColorUsageProfessionalAccent") +
                            "'," //ColorUsageProfessionalAccent
                            + "'" + dt.Rows[i].Field<string>("DoubleClickTotal") + "'," //DoubleClickTotal 
                            + "'" + dt.Rows[i].Field<string>("DoubleClickMono") + "'," //DoubleClickMono 
                            + "'" + dt.Rows[i].Field<string>("DoubleClickColor") + "'," //DoubleClickColor 
                            + "'" + dt.Rows[i].Field<string>("DoubleClickFullColor") + "'," //DoubleClickFullColor 
                            + "'" + dt.Rows[i].Field<string>("DoubleClickTwoColor") + "'," //DoubleClickTwoColor 
                            + "'" + dt.Rows[i].Field<string>("DoubleClickSingleColor") + "'," //DoubleClickSingleColor 
                            + "'" + dt.Rows[i].Field<string>("DoubleClickDuplex") + "'," //DoubleClickDuplex 
                            + "'" + dt.Rows[i].Field<string>("DevelopmentTotal") + "'," //DevelopmentTotal 
                            + "'" + dt.Rows[i].Field<string>("DevelopmentMono") + "'," //DevelopmentMono 
                            + "'" + dt.Rows[i].Field<string>("DevelopmentColor") + "'," //DevelopmentColor 
                            + "'" + dt.Rows[i].Field<string>("CoverageAverageBlack") + "'," //CoverageAverageBlack 
                            + "'" + dt.Rows[i].Field<string>("CoverageAverageCyan") + "'," //CoverageAverageCyan 
                            + "'" + dt.Rows[i].Field<string>("CoverageAverageMagenta") + "'," //CoverageAverageMagenta 
                            + "'" + dt.Rows[i].Field<string>("CoverageAverageYellow") + "'," //CoverageAverageYellow 
                            + "'" + dt.Rows[i].Field<string>("CoverageSumBlack") + "'," //CoverageSumBlack 
                            + "'" + dt.Rows[i].Field<string>("CoverageSumCyan") + "'," //CoverageSumCyan 
                            + "'" + dt.Rows[i].Field<string>("CoverageSumMagenta") + "'," //CoverageSumMagenta 
                            + "'" + dt.Rows[i].Field<string>("CoverageSumYellow") + "'," //CoverageSumYellow 
                            + "'" + dt.Rows[i].Field<string>("CoverageSum2Black") + "'," //CoverageSum2Black 
                            + "'" + dt.Rows[i].Field<string>("CoverageSum2Cyan") + "'," //CoverageSum2Cyan 
                            + "'" + dt.Rows[i].Field<string>("CoverageSum2Magenta") + "'," //CoverageSum2Magenta 
                            + "'" + dt.Rows[i].Field<string>("CoverageSum2Yellow") + "'," //CoverageSum2Yellow 
                            + "'" + dt.Rows[i].Field<string>("MeterGroup1") + "'," //MeterGroup1 
                            + "'" + dt.Rows[i].Field<string>("MeterGroup2") + "'," //MeterGroup2
                            + "'" + dt.Rows[i].Field<string>("BlackLevelMax") + "',"
                            + "'" + dt.Rows[i].Field<string>("CyanLevelMax") + "',"
                            + "'" + dt.Rows[i].Field<string>("MagentaLevelMax") + "',"
                            + "'" + dt.Rows[i].Field<string>("YellowLevelMax") + "',"
                            + "'" + dt.Rows[i].Field<string>("BlackLevel") + "',"
                            + "'" + dt.Rows[i].Field<string>("CyanLevel") + "',"
                            + "'" + dt.Rows[i].Field<string>("MagentaLevel") + "',"
                            + "'" + dt.Rows[i].Field<string>("YellowLevel") + "'"
                            + ");"
                        );
                    Oid.GetInstance().NonQuery("Update Directory set val = '" + fileOid + "' Where Identifier = 'OID-Version';");
                    Oid.GetInstance().UpdateDirectory();
                    MessageBox.Show("OID's erfolgreich Eingelesen\nSie haben nun die OID-Version: " + fileOid, "Infomation", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Sie verwenden bereits die neuste OID-Version", "Infomation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }






    }

}