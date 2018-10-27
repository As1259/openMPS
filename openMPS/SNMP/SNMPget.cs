#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using de.fearvel.openMPS.Database;
using SnmpSharpNet;

namespace de.fearvel.openMPS.SNMP
{
    /// <summary>
    ///     SNMP Namespace
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    ///     Aquireing data
    /// </summary>
    public static class SNMPget
    {
        /// <summary>
        ///     The abgefragte oids
        /// </summary>
        private static readonly string[] AbgefragteOids =
        {
            "Manufacturer",
            "Model",
            "SerialNumber",
            "MACAddress",
            "IPAddresse",
            "HostName",
            "LocalID",
            "DescriptionLocation",
            "AssetNumber",
            "InstalledMemory",
            "FirmwareVersion",
            "FirmwareVersion2",
            "FirmwareVersion3",
            "FirmwareVersion4",
            "InstallationDate",
            "ServiceContactIsColor",
            "IsDuplex",
            "PowerActive",
            "PowerIdle",
            "PowerSleep1",
            "PowerSleep2",
            "TotalPages",
            "TotalPagesMono",
            "TotalPagesColor",
            "TotalPagesFullColor",
            "TotalPagesTwoColor",
            "TotalPagesSingleColor",
            "TotalPagesDuplex",
            "UsagePages",
            "UsagePagesMono",
            "UsagePagesColor",
            "UsagePagesFullColor",
            "UsagePagesTwoColor",
            "UsagePagesSingleColor",
            "PrinterPages",
            "PrinterPagesMono",
            "PrinterPagesColor",
            "PrinterPagesFullColor",
            "PrinterPagesTwoColor",
            "PrinterPagesSingleColor",
            "CopyPages",
            "CopyPagesMono",
            "CopyPagesColor",
            "CopyPagesFullColor",
            "CopyPagesTwoColor",
            "CopyPagesSingleColor",
            "FaxPages",
            "FaxPagesMono",
            "FaxPagesColor",
            "OtherPagesOther",
            "PagesMonoOther",
            "PagesColorOther",
            "PagesFullColor",
            "OtherPagesTwoColor",
            "OtherPagesSingleColor",
            "FaxesSentFaxesReceived",
            "ScansTotalScansTotalMono",
            "ScansTotalColor",
            "ScansUsageScansUsageMono",
            "ScansUsageColor",
            "ScansCopy",
            "ScansCopyMono",
            "ScansCopyColor",
            "ScansFax",
            "ScansFaxMono",
            "ScansFaxColor",
            "Scansemail",
            "ScansemailMono",
            "ScansemailColor",
            "ScansNet",
            "ScansNetMono",
            "ScansNetColor",
            "ListPages",
            "LargePages",
            "LargePagesMono",
            "LargePagesColor",
            "LargePagesFullColor",
            "LargePagesTwoColor",
            "LargePagesSingleColor",
            "TotalLargeSheets",
            "SquareFeetSquareMeters",
            "LinearFeetStapledSets",
            "Level1Pages",
            "Level2Pages",
            "Level3Pages",
            "ColorUsageOffice",
            "ColorUsageOfficeAccent",
            "ColorUsageProfessional",
            "ColorUsageProfessionalAccent",
            "DoubleClickTotal",
            "DoubleClickMono",
            "DoubleClickColor",
            "DoubleClickFullColor",
            "DoubleClickTwoColor",
            "DoubleClickSingleColor",
            "DoubleClickDuplex",
            "DevelopmentTotal",
            "DevelopmentMono",
            "DevelopmentColor",
            "CoverageAverageBlack",
            "CoverageAverageCyan",
            "CoverageAverageMagenta",
            "CoverageAverageYellow",
            "CoverageSumBlack",
            "CoverageSumCyan",
            "CoverageSumMagenta",
            "CoverageSumYellow",
            "CoverageSum2Black",
            "CoverageSum2Cyan",
            "CoverageSum2Magenta",
            "CoverageSum2Yellow",
            "MeterGroup1",
            "MeterGroup2",
            "BlackLevelMax",
            "CyanLevelMax",
            "MagentaLevelMax",
            "YellowLevelMax",
            "BlackLevel",
            "CyanLevel",
            "MagentaLevel",
            "YellowLevel"
        };

        /// <summary>
        ///     Reads the device oids.
        ///     and triggers writetotable
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="ident">The ident.</param>
        public static void ReadDeviceOiDs(string ip, string ident)
        {
            string[] oidValues = null;
            var dt = Config.GetInstance().Query("Select * from OID where OIDPrivateID='" + ident + "'");
            var s = new string[AbgefragteOids.Length];

            for (var i = 0; i < AbgefragteOids.Length; i++) s[i] = dt.Rows[0].Field<string>(AbgefragteOids[i]);
            try
            {
                oidValues = GetOidValues(ip, s);
            }
            catch (SnmpException)
            {
            }

            WriteToTable(oidValues, ip);
        }

        public static bool ReadDeviceOiDs(string ip, string ident, out DataRow dr)
        {
            var dt = Config.GetInstance().Query("Select * from OID where OIDPrivateID='" + ident + "'");
            var s = new string[AbgefragteOids.Length];

            try
            {
                dr = GetOidValues(ip, dt);
                return true;
            }
            catch (SnmpException)
            {
                dr = null;
                return false;
            }
        }
        /// <summary>
        ///     Finds the alive printer.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static bool FindAlivePrinter(string ip)
        {
            try
            {
                if (GetOidValue(ip, "1.3.6.1.2.1.43.5.1.1.2.1").Length > 0) return true;
            }
            catch (SnmpException)
            {
                // ignored
            }

            return false;
        }

        /// <summary>
        ///     Gets the specific values of an OID
        ///     ///
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public static string GetOidValue(string ip, string oid)
        {
            var oidValue = "";

            try
            {
                if (oid.Length > 0)
                {
                    var community = new OctetString("public");
                    var param = new AgentParameters(community)
                    {
                        Version = SnmpVersion.Ver1
                    };
                    var agent = new IpAddress(ip);
                    var target = new UdpTarget((IPAddress) agent, 161, 2000, 1);
                    var pdu = new Pdu(PduType.Get);
                    pdu.VbList.Add(oid);
                    var result = (SnmpV1Packet) target.Request(pdu, param);
                    if (result?.Pdu.ErrorStatus == 0)
                        oidValue = result.Pdu.VbList[0].Value.ToString();
                    target.Close();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return oidValue;
        }

        /// <summary>
        ///     Gets the specific values of an OID Array.
        ///     string[0,n] == OID
        ///     string[1,n] == Values
        /// </summary>
        /// <param name="ip">The ip string.</param>
        /// <param name="oid">The oid string[].</param>
        /// <returns>string[2,n]</returns>
        public static string[] GetOidValues(string ip, string[] oid)
        {
            var oidValues = new string[AbgefragteOids.Length];
            try
            {
                for (var i = 0; i < oidValues.Length; i++)
                {
                    oidValues[i] = "";
                    oidValues[i] = GetOidValue(ip, oid[i]);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return oidValues;
        }

        public static DataRow GetOidValues(string ip, DataTable oid)
        {
            var oidValues = oid.NewRow();
            try
            {
                for (var i = 0; i < oid.Columns.Count; i++)
                {
                    oidValues[i] = GetOidValue(ip, (string)oid.Rows[0][i]);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return oidValues;
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
        /// </summary>
        /// <param name="s">The String Array</param>
        /// <param name="ip"></param>
        public static void WriteToTable(string[] s, string ip)
        {
            Collector.shell(
                "insert into Collector "
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
                + "Scansemail,"
                + "ScansemailMono,"
                + "ScansemailColor,"
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
                + "YellowLevel)"
                + " Values ("
                + "'" + s[0] + "'," //Manufacturer 
                + "'" + s[1] + "'," //Model 
                + "'" + s[2] + "'," //SerialNumber 
                + "'" + s[3] + "'," //MACAddress 
                + "'" + ip + "'," //IPAddresse 
                + "'" + s[5] + "'," //HostName 
                + "'" + s[6] + "'," //LocalID 
                + "'" + s[7] + "'," //DescriptionLocation 
                + "'" + s[8] + "'," //AssetNumber 
                + "'" + s[9] + "'," //InstalledMemory 
                + "'" + s[10] + "'," //FirmwareVersion 
                + "'" + s[11] + "'," //FirmwareVersion2 
                + "'" + s[12] + "'," //FirmwareVersion3 
                + "'" + s[13] + "'," //FirmwareVersion4 
                + "'" + s[14] + "'," //InstallationDate 
                + "'" + s[15] + "'," //ServiceContactIsColor 
                + "'" + s[16] + "'," //IsDuplex 
                + "'" + s[17] + "'," //PowerActive 
                + "'" + s[18] + "'," //PowerIdle 
                + "'" + s[19] + "'," //PowerSleep1 
                + "'" + s[20] + "'," //PowerSleep2 
                + PrepareNumber(s[21]) + "," //TotalPages 
                + PrepareNumber(s[22]) + "," //TotalPagesMono 
                + PrepareNumber(s[23]) + "," //TotalPagesColor 
                + PrepareNumber(s[24]) + "," //TotalPagesFullColor 
                + PrepareNumber(s[25]) + "," //TotalPagesTwoColor 
                + PrepareNumber(s[26]) + "," //TotalPagesSingleColor 
                + PrepareNumber(s[27]) + "," //TotalPagesDuplex 
                + PrepareNumber(s[28]) + "," //UsagePages 
                + PrepareNumber(s[29]) + "," //UsagePagesMono 
                + PrepareNumber(s[30]) + "," //UsagePagesColor 
                + PrepareNumber(s[31]) + "," //UsagePagesFullColor 
                + PrepareNumber(s[32]) + "," //UsagePagesTwoColor 
                + PrepareNumber(s[33]) + "," //UsagePagesSingleColor 
                + PrepareNumber(s[34]) + "," //PrinterPages 
                + PrepareNumber(s[35]) + "," //PrinterPagesMono 
                + PrepareNumber(s[36]) + "," //PrinterPagesColor 
                + PrepareNumber(s[37]) + "," //PrinterPagesFullColor 
                + PrepareNumber(s[38]) + "," //PrinterPagesTwoColor 
                + PrepareNumber(s[39]) + "," //PrinterPagesSingleColor 
                + PrepareNumber(s[40]) + "," //CopyPages 
                + PrepareNumber(s[41]) + "," //CopyPagesMono 
                + PrepareNumber(s[42]) + "," //CopyPagesColor 
                + PrepareNumber(s[43]) + "," //CopyPagesFullColor 
                + PrepareNumber(s[44]) + "," //CopyPagesTwoColor 
                + PrepareNumber(s[45]) + "," //CopyPagesSingleColor 
                + PrepareNumber(s[46]) + "," //FaxPages 
                + PrepareNumber(s[47]) + "," //FaxPagesMono 
                + PrepareNumber(s[48]) + "," //FaxPagesColor 
                + PrepareNumber(s[49]) + "," //OtherPagesOther 
                + PrepareNumber(s[50]) + "," //PagesMonoOther 
                + PrepareNumber(s[51]) + "," //PagesColorOther 
                + PrepareNumber(s[52]) + "," //PagesFullColor 
                + PrepareNumber(s[53]) + "," //OtherPagesTwoColor 
                + PrepareNumber(s[54]) + "," //OtherPagesSingleColor 
                + PrepareNumber(s[55]) + "," //FaxesSentFaxesReceived 
                + PrepareNumber(s[56]) + "," //ScansTotalScansTotalMono 
                + PrepareNumber(s[57]) + "," //ScansTotalColor 
                + PrepareNumber(s[58]) + "," //ScansUsageScansUsageMono 
                + PrepareNumber(s[59]) + "," //ScansUsageColor 
                + PrepareNumber(s[60]) + "," //ScansCopy 
                + PrepareNumber(s[61]) + "," //ScansCopyMono 
                + PrepareNumber(s[62]) + "," //ScansCopyColor 
                + PrepareNumber(s[63]) + "," //ScansFax 
                + PrepareNumber(s[64]) + "," //ScansFaxMono 
                + PrepareNumber(s[65]) + "," //ScansFaxColor 
                + PrepareNumber(s[66]) + "," //Scansemail 
                + PrepareNumber(s[67]) + "," //ScansemailMono 
                + PrepareNumber(s[68]) + "," //ScansemailColor 
                + PrepareNumber(s[69]) + "," //ScansNet 
                + PrepareNumber(s[70]) + "," //ScansNetMono 
                + PrepareNumber(s[71]) + "," //ScansNetColor 
                + PrepareNumber(s[72]) + "," //ListPages 
                + PrepareNumber(s[73]) + "," //LargePages 
                + PrepareNumber(s[74]) + "," //LargePagesMono 
                + PrepareNumber(s[75]) + "," //LargePagesColor 
                + PrepareNumber(s[76]) + "," //LargePagesFullColor 
                + PrepareNumber(s[77]) + "," //LargePagesTwoColor 
                + PrepareNumber(s[78]) + "," //LargePagesSingleColor 
                + PrepareNumber(s[79]) + "," //TotalLargeSheets 
                + PrepareNumber(s[80]) + "," //SquareFeetSquareMeters 
                + PrepareNumber(s[81]) + "," //LinearFeetStapledSets 
                + PrepareNumber(s[82]) + "," //Level1Pages 
                + PrepareNumber(s[83]) + "," //Level2Pages 
                + PrepareNumber(s[84]) + "," //Level3Pages 
                + PrepareNumber(s[85]) + "," //ColorUsageOffice 
                + PrepareNumber(s[86]) + "," //ColorUsageOfficeAccent 
                + PrepareNumber(s[87]) + "," //ColorUsageProfessional 
                + PrepareNumber(s[88]) + "," //ColorUsageProfessionalAccent
                + PrepareNumber(s[89]) + "," //DoubleClickTotal 
                + PrepareNumber(s[90]) + "," //DoubleClickMono 
                + PrepareNumber(s[91]) + "," //DoubleClickColor 
                + PrepareNumber(s[92]) + "," //DoubleClickFullColor 
                + PrepareNumber(s[93]) + "," //DoubleClickTwoColor 
                + PrepareNumber(s[94]) + "," //DoubleClickSingleColor 
                + PrepareNumber(s[95]) + "," //DoubleClickDuplex 
                + PrepareNumber(s[96]) + "," //DevelopmentTotal 
                + PrepareNumber(s[97]) + "," //DevelopmentMono 
                + PrepareNumber(s[98]) + "," //DevelopmentColor 
                + PrepareNumber(s[99]) + "," //CoverageAverageBlack 
                + PrepareNumber(s[100]) + "," //CoverageAverageCyan 
                + PrepareNumber(s[101]) + "," //CoverageAverageMagenta 
                + PrepareNumber(s[102]) + "," //CoverageAverageYellow 
                + PrepareNumber(s[103]) + "," //CoverageSumBlack 
                + PrepareNumber(s[104]) + "," //CoverageSumCyan 
                + PrepareNumber(s[105]) + "," //CoverageSumMagenta 
                + PrepareNumber(s[106]) + "," //CoverageSumYellow 
                + PrepareNumber(s[107]) + "," //CoverageSum2Black 
                + PrepareNumber(s[108]) + "," //CoverageSum2Cyan 
                + PrepareNumber(s[109]) + "," //CoverageSum2Magenta 
                + PrepareNumber(s[110]) + "," //CoverageSum2Yellow 
                + "'" + s[111] + "'," //MeterGroup1 
                + "'" + s[112] + "'," //MeterGroup2
                + PrepareNumber(s[113]) + ","
                + PrepareNumber(s[114]) + ","
                + PrepareNumber(s[115]) + ","
                + PrepareNumber(s[116]) + ","
                + PrepareNumber(s[117]) + ","
                + PrepareNumber(s[118]) + ","
                + PrepareNumber(s[119]) + ","
                + PrepareNumber(s[120])
                + ");"
            );
        }

        private static string PrepareNumber(string num)
        {
            return num.Length == 0 ? "0" : num;
        }
    }
}