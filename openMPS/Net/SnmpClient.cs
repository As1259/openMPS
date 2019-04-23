#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using de.fearvel.net.FnLog;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes;
using SnmpSharpNet;

namespace de.fearvel.openMPS.Net
{

    /// <summary>
    ///     Aquireing data
    /// </summary>
    public static class SnmpClient
    {
        /// <summary>
        ///     The abgefragte oids
        /// </summary>
        private static Dictionary<string, Type> AbgefragteOids = new Dictionary<string, Type>()
        {
            {
                "OidPrivateId", typeof(string)
            },
            {
                "VendorName", typeof(string)
            },
            {
                "Model", typeof(string)
            },
            {
                "SerialNumber", typeof(string)
            },
            {
                "MacAddress", typeof(string)
            },
            {
                "IpAddress", typeof(string)
            },
            {
                "HostName", typeof(string)
            },
            {
                "DescriptionLocation", typeof(string)
            },
            {
                "AssetNumber", typeof(string)
            },
            {
                "FirmwareVersion", typeof(string)
            },
            {
                "PowerSleep1", typeof(string)
            },
            {
                "PowerSleep2", typeof(string)
            },
            {
                "ProfileName", typeof(string)
            },
            {
                "DeviceName", typeof(string)
            },
            {
                "DeviceType", typeof(string)
            },
            {
                "Manufacturer", typeof(string)
            },
            {
                "TotalPages", typeof(long)
            },
            {
                "TotalPagesMono", typeof(long)
            },
            {
                "TotalPagesColor", typeof(long)
            },
            {
                "TotalPagesDuplex", typeof(long)
            },
            {
                "PrinterPages", typeof(long)
            },
            {
                "PrinterPagesMono", typeof(long)
            },
            {
                "PrinterPagesColor", typeof(long)
            },
            {
                "PrinterPagesFullColor", typeof(long)
            },
            {
                "PrinterPagesTwoColor", typeof(long)
            },
            {
                "CopyPagesMono", typeof(long)
            },
            {
                "CopyPagesColor", typeof(long)
            },
            {
                "CopyPagesFullColor", typeof(long)
            },
            {
                "CopyPagesTwoColor", typeof(long)
            },
            {
                "CopyPagesSingleColor", typeof(long)
            },
            {
                "FaxesSentFaxesReceived", typeof(long)
            },
            {
                "ScansTotalScansTotalMono", typeof(long)
            },
            {
                "ScansTotalColor", typeof(long)
            },
            {
                "ScansCopyMono", typeof(long)
            },
            {
                "ScansCopyColor", typeof(long)
            },
            {
                "ScansEmail", typeof(long)
            },
            {
                "ScansEmailMono", typeof(long)
            },
            {
                "ScansNet", typeof(long)
            },
            {
                "ScansNetMono", typeof(long)
            },
            {
                "ScansNetColor", typeof(long)
            },
            {
                "LargePagesMono", typeof(long)
            },
            {
                "LargePagesFullColor", typeof(long)
            },
            {
                "CoverageAverageBlack", typeof(long)
            },
            {
                "CoverageAverageCyan", typeof(long)
            },
            {
                "CoverageAverageMagenta", typeof(long)
            },
            {
                "CoverageAverageYellow", typeof(long)
            },
            {
                "BlackLevelMax", typeof(long)
            },
            {
                "CyanLevelMax", typeof(long)
            },
            {
                "MagentaLevelMax", typeof(long)
            },
            {
                "YellowLevelMax", typeof(long)
            },
            {
                "BlackLevel", typeof(long)
            },
            {
                "CyanLevel", typeof(long)
            },
            {
                "MagentaLevel", typeof(long)
            },
            {
                "YellowLevel", typeof(long)
            }
        };

        public static bool ReadDeviceOiDs(string ip, string ident, out OidData oidData)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SnmpClient", "ReadDeviceOiDs");
            var dt = Config.GetInstance().SelectFromOidTable(ident);
            try
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SnmpClient",
                    "ReadDeviceOiDs - IP " + ip + " OIDDATA " + GetOidValues(ip, dt));

                oidData = GetOidValues(ip, dt);
                return true;
            }
            catch (SnmpException)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SnmpClient", "ReadDeviceOiDs - SnmpException");
                oidData = null;
                return false;
            }
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
                    var target = new UdpTarget((IPAddress) agent, 161, 5000, 1);
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

    

        public static OidData GetOidValues(string ip, DataTable oid)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SnmpClient", "GetOidValues ip " + ip);

            var data = new OidData();
            var strDict = new Dictionary<string, string>();
            var longDict = new Dictionary<string, long>();

            try
            {
                foreach (var pair in AbgefragteOids)
                {
                    if (pair.Value == typeof(string))
                    {
                        strDict.Add(pair.Key, GetOidValue(ip, oid.Rows[0].Field<string>(pair.Key)));
                    }
                    else
                    {
                        var dataStr = GetOidValue(ip, oid.Rows[0].Field<string>(pair.Key));
                        if (dataStr.Length > 0)
                        {
                            longDict.Add(pair.Key, long.Parse(dataStr));

                        }
                        else
                        {
                            longDict.Add(pair.Key, 0);

                        }

                    }
                }
                data.VendorName = strDict["VendorName"];
                data.Model = strDict["Model"];
                data.SerialNumber = strDict["SerialNumber"];
                data.MacAddress = strDict["MacAddress"];
                data.IpAddress = strDict["IpAddress"];
                data.HostName = strDict["HostName"];
                data.DescriptionLocation = strDict["DescriptionLocation"];
                data.AssetNumber = strDict["AssetNumber"];
                data.FirmwareVersion = strDict["FirmwareVersion"];
                data.PowerSleep1 = strDict["PowerSleep1"];
                data.PowerSleep2 = strDict["PowerSleep2"];
                data.ProfileName = strDict["ProfileName"];
                data.DeviceName = strDict["DeviceName"];
                data.DeviceType = strDict["DeviceType"];
                data.Manufacturer = strDict["Manufacturer"];
                data.TotalPages = longDict["TotalPages"];
                data.TotalPagesMono = longDict["TotalPagesMono"];
                data.TotalPagesColor = longDict["TotalPagesColor"];
                data.TotalPagesDuplex = longDict["TotalPagesDuplex"];
                data.PrinterPages = longDict["PrinterPages"];
                data.PrinterPagesMono = longDict["PrinterPagesMono"];
                data.PrinterPagesColor = longDict["PrinterPagesColor"];
                data.PrinterPagesFullColor = longDict["PrinterPagesFullColor"];
                data.PrinterPagesTwoColor = longDict["PrinterPagesTwoColor"];
                data.CopyPagesMono = longDict["CopyPagesMono"];
                data.CopyPagesColor = longDict["CopyPagesColor"];
                data.CopyPagesFullColor = longDict["CopyPagesFullColor"];
                data.CopyPagesTwoColor = longDict["CopyPagesTwoColor"];
                data.CopyPagesSingleColor = longDict["CopyPagesSingleColor"];
                data.FaxesSentFaxesReceived = longDict["FaxesSentFaxesReceived"];
                data.ScansTotalScansTotalMono = longDict["ScansTotalScansTotalMono"];
                data.ScansTotalColor = longDict["ScansTotalColor"];
                data.ScansCopyMono = longDict["ScansCopyMono"];
                data.ScansCopyColor = longDict["ScansCopyColor"];
                data.ScansEmail = longDict["ScansEmail"];
                data.ScansEmailMono = longDict["ScansEmailMono"];
                data.ScansNet = longDict["ScansNet"];
                data.ScansNetMono = longDict["ScansNetMono"];
                data.ScansNetColor = longDict["ScansNetColor"];
                data.LargePagesMono = longDict["LargePagesMono"];
                data.LargePagesFullColor = longDict["LargePagesFullColor"];
                data.CoverageAverageBlack = longDict["CoverageAverageBlack"];
                data.CoverageAverageCyan = longDict["CoverageAverageCyan"];
                data.CoverageAverageMagenta = longDict["CoverageAverageMagenta"];
                data.CoverageAverageYellow = longDict["CoverageAverageYellow"];
                data.BlackLevelMax = longDict["BlackLevelMax"];
                data.CyanLevelMax = longDict["CyanLevelMax"];
                data.MagentaLevelMax = longDict["MagentaLevelMax"];
                data.YellowLevelMax = longDict["YellowLevelMax"];
                data.BlackLevel = longDict["BlackLevel"];
                data.CyanLevel = longDict["CyanLevel"];
                data.MagentaLevel = longDict["MagentaLevel"];
                data.YellowLevel = longDict["YellowLevel"];
            }
            catch (Exception)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.Error, "SnmpClient", "GetOidValues ERROR ip " + ip);

                // ignored
            }

            FnLog.GetInstance().AddToLogList(FnLog.LogType.Error, "SnmpClient", "GetOidValues COMPLETE ip " + ip);
            return data;
        }
    }
}