#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using de.fearvel.openMPS.SQLiteConnectionTools;
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
        private static readonly string[] abgefragteOIDS =
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
        public static void readDeviceOIDs(string ip, string ident)
        {
            string[] OIDValues = null;
            DataTable dt;
            dt =Config.GetInstance().Query("Select * from OID where OIDPrivateID='" + ident + "'");
            var s = new string[abgefragteOIDS.Length];

            for (var i = 0; i < abgefragteOIDS.Length; i++) s[i] = dt.Rows[0].Field<string>(abgefragteOIDS[i]);
            try
            {
                OIDValues = getOIDValues(ip, s);
            }
            catch (SnmpException)
            {
            }

            writeToTable(OIDValues, ip);
        }

        /// <summary>
        ///     Finds the alive printer.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        public static bool findAlivePrinter(string ip)
        {
            try
            {
                if (getOIDValue(ip, "1.3.6.1.2.1.43.5.1.1.2.1").Length > 0) return true;
            }
            catch (SnmpException)
            {
            }

            return false;
        }

        /// <summary>
        ///     Gets the specific values of an OID
        ///     ///
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="OID">The oid.</param>
        /// <returns></returns>
        public static string getOIDValue(string ip, string OID)
        {
            var OIDValue = "";

            try
            {
                if (OID.Length > 0)
                {
                    var community = new OctetString("public");
                    var param = new AgentParameters(community)
                    {
                        Version = SnmpVersion.Ver1
                    };
                    var agent = new IpAddress(ip);
                    var target = new UdpTarget((IPAddress) agent, 161, 2000, 1);
                    var pdu = new Pdu(PduType.Get);
                    pdu.VbList.Add(OID);
                    var result = (SnmpV1Packet) target.Request(pdu, param);
                    if (result != null)
                        if (result.Pdu.ErrorStatus == 0)
                            OIDValue = result.Pdu.VbList[0].Value.ToString();
                    target.Close();
                }
            }
            catch (Exception)
            {
            }

            return OIDValue;
        }

        /// <summary>
        ///     Gets the specific values of an OID Array.
        ///     string[0,n] == OID
        ///     string[1,n] == Values
        /// </summary>
        /// <param name="ip">The ip string.</param>
        /// <param name="OID">The oid string[].</param>
        /// <returns>string[2,n]</returns>
        public static string[] getOIDValues(string ip, string[] OID)
        {
            var OIDValues = new string[abgefragteOIDS.Length];
            try
            {
                for (var i = 0; i < OIDValues.Length; i++)
                {
                    OIDValues[i] = "";
                    OIDValues[i] = getOIDValue(ip, OID[i]);
                }
            }
            catch (Exception)
            {
            }

            return OIDValues;
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
        public static void writeToTable(string[] s, string ip)
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
                + prepareNumber(s[21]) + "," //TotalPages 
                + prepareNumber(s[22]) + "," //TotalPagesMono 
                + prepareNumber(s[23]) + "," //TotalPagesColor 
                + prepareNumber(s[24]) + "," //TotalPagesFullColor 
                + prepareNumber(s[25]) + "," //TotalPagesTwoColor 
                + prepareNumber(s[26]) + "," //TotalPagesSingleColor 
                + prepareNumber(s[27]) + "," //TotalPagesDuplex 
                + prepareNumber(s[28]) + "," //UsagePages 
                + prepareNumber(s[29]) + "," //UsagePagesMono 
                + prepareNumber(s[30]) + "," //UsagePagesColor 
                + prepareNumber(s[31]) + "," //UsagePagesFullColor 
                + prepareNumber(s[32]) + "," //UsagePagesTwoColor 
                + prepareNumber(s[33]) + "," //UsagePagesSingleColor 
                + prepareNumber(s[34]) + "," //PrinterPages 
                + prepareNumber(s[35]) + "," //PrinterPagesMono 
                + prepareNumber(s[36]) + "," //PrinterPagesColor 
                + prepareNumber(s[37]) + "," //PrinterPagesFullColor 
                + prepareNumber(s[38]) + "," //PrinterPagesTwoColor 
                + prepareNumber(s[39]) + "," //PrinterPagesSingleColor 
                + prepareNumber(s[40]) + "," //CopyPages 
                + prepareNumber(s[41]) + "," //CopyPagesMono 
                + prepareNumber(s[42]) + "," //CopyPagesColor 
                + prepareNumber(s[43]) + "," //CopyPagesFullColor 
                + prepareNumber(s[44]) + "," //CopyPagesTwoColor 
                + prepareNumber(s[45]) + "," //CopyPagesSingleColor 
                + prepareNumber(s[46]) + "," //FaxPages 
                + prepareNumber(s[47]) + "," //FaxPagesMono 
                + prepareNumber(s[48]) + "," //FaxPagesColor 
                + prepareNumber(s[49]) + "," //OtherPagesOther 
                + prepareNumber(s[50]) + "," //PagesMonoOther 
                + prepareNumber(s[51]) + "," //PagesColorOther 
                + prepareNumber(s[52]) + "," //PagesFullColor 
                + prepareNumber(s[53]) + "," //OtherPagesTwoColor 
                + prepareNumber(s[54]) + "," //OtherPagesSingleColor 
                + prepareNumber(s[55]) + "," //FaxesSentFaxesReceived 
                + prepareNumber(s[56]) + "," //ScansTotalScansTotalMono 
                + prepareNumber(s[57]) + "," //ScansTotalColor 
                + prepareNumber(s[58]) + "," //ScansUsageScansUsageMono 
                + prepareNumber(s[59]) + "," //ScansUsageColor 
                + prepareNumber(s[60]) + "," //ScansCopy 
                + prepareNumber(s[61]) + "," //ScansCopyMono 
                + prepareNumber(s[62]) + "," //ScansCopyColor 
                + prepareNumber(s[63]) + "," //ScansFax 
                + prepareNumber(s[64]) + "," //ScansFaxMono 
                + prepareNumber(s[65]) + "," //ScansFaxColor 
                + prepareNumber(s[66]) + "," //Scansemail 
                + prepareNumber(s[67]) + "," //ScansemailMono 
                + prepareNumber(s[68]) + "," //ScansemailColor 
                + prepareNumber(s[69]) + "," //ScansNet 
                + prepareNumber(s[70]) + "," //ScansNetMono 
                + prepareNumber(s[71]) + "," //ScansNetColor 
                + prepareNumber(s[72]) + "," //ListPages 
                + prepareNumber(s[73]) + "," //LargePages 
                + prepareNumber(s[74]) + "," //LargePagesMono 
                + prepareNumber(s[75]) + "," //LargePagesColor 
                + prepareNumber(s[76]) + "," //LargePagesFullColor 
                + prepareNumber(s[77]) + "," //LargePagesTwoColor 
                + prepareNumber(s[78]) + "," //LargePagesSingleColor 
                + prepareNumber(s[79]) + "," //TotalLargeSheets 
                + prepareNumber(s[80]) + "," //SquareFeetSquareMeters 
                + prepareNumber(s[81]) + "," //LinearFeetStapledSets 
                + prepareNumber(s[82]) + "," //Level1Pages 
                + prepareNumber(s[83]) + "," //Level2Pages 
                + prepareNumber(s[84]) + "," //Level3Pages 
                + prepareNumber(s[85]) + "," //ColorUsageOffice 
                + prepareNumber(s[86]) + "," //ColorUsageOfficeAccent 
                + prepareNumber(s[87]) + "," //ColorUsageProfessional 
                + prepareNumber(s[88]) + "," //ColorUsageProfessionalAccent
                + prepareNumber(s[89]) + "," //DoubleClickTotal 
                + prepareNumber(s[90]) + "," //DoubleClickMono 
                + prepareNumber(s[91]) + "," //DoubleClickColor 
                + prepareNumber(s[92]) + "," //DoubleClickFullColor 
                + prepareNumber(s[93]) + "," //DoubleClickTwoColor 
                + prepareNumber(s[94]) + "," //DoubleClickSingleColor 
                + prepareNumber(s[95]) + "," //DoubleClickDuplex 
                + prepareNumber(s[96]) + "," //DevelopmentTotal 
                + prepareNumber(s[97]) + "," //DevelopmentMono 
                + prepareNumber(s[98]) + "," //DevelopmentColor 
                + prepareNumber(s[99]) + "," //CoverageAverageBlack 
                + prepareNumber(s[100]) + "," //CoverageAverageCyan 
                + prepareNumber(s[101]) + "," //CoverageAverageMagenta 
                + prepareNumber(s[102]) + "," //CoverageAverageYellow 
                + prepareNumber(s[103]) + "," //CoverageSumBlack 
                + prepareNumber(s[104]) + "," //CoverageSumCyan 
                + prepareNumber(s[105]) + "," //CoverageSumMagenta 
                + prepareNumber(s[106]) + "," //CoverageSumYellow 
                + prepareNumber(s[107]) + "," //CoverageSum2Black 
                + prepareNumber(s[108]) + "," //CoverageSum2Cyan 
                + prepareNumber(s[109]) + "," //CoverageSum2Magenta 
                + prepareNumber(s[110]) + "," //CoverageSum2Yellow 
                + "'" + s[111] + "'," //MeterGroup1 
                + "'" + s[112] + "'," //MeterGroup2
                + prepareNumber(s[113]) + ","
                + prepareNumber(s[114]) + ","
                + prepareNumber(s[115]) + ","
                + prepareNumber(s[116]) + ","
                + prepareNumber(s[117]) + ","
                + prepareNumber(s[118]) + ","
                + prepareNumber(s[119]) + ","
                + prepareNumber(s[120])
                + ");"
            );
        }

        private static string prepareNumber(string num)
        {
            if (num.Length == 0) return "0";
            return num;
        }
    }
}