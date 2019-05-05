// Copyright (c) 2018 / 2019, Andreas Schreiner

using System.Collections.Generic;
using System.Data;
using de.fearvel.net.DataTypes.AbstractDataTypes;
using de.fearvel.net.Manastone;

namespace de.fearvel.openMPS.DataTypes
{
    /// <summary>
    /// Container Class for the received Oid 
    /// </summary>
    public class OidData : JsonSerializable<OidData>
    {
        public string CustomerReference = ManastoneClient.GetInstance().CustomerReference;
        public string VendorName;
        public string Model;
        public string SerialNumber;
        public string MacAddress;
        public string IpAddress;
        public string HostName;
        public string DescriptionLocation;
        public string AssetNumber;
        public string FirmwareVersion;
        public string PowerSleep1;
        public string PowerSleep2;
        public string ProfileName;
        public string DeviceName;
        public string DeviceType;
        public string Manufacturer;
        public long TotalPages;
        public long TotalPagesMono;
        public long TotalPagesColor;
        public long TotalPagesDuplex;
        public long PrinterPages;
        public long PrinterPagesMono;
        public long PrinterPagesColor;
        public long PrinterPagesFullColor;
        public long PrinterPagesTwoColor;
        public long CopyPagesMono;
        public long CopyPagesColor;
        public long CopyPagesFullColor;
        public long CopyPagesTwoColor;
        public long CopyPagesSingleColor;
        public long FaxesSentFaxesReceived;
        public long ScansTotalScansTotalMono;
        public long ScansTotalColor;
        public long ScansCopyMono;
        public long ScansCopyColor;
        public long ScansEmail;
        public long ScansEmailMono;
        public long ScansNet;
        public long ScansNetMono;
        public long ScansNetColor;
        public long LargePagesMono;
        public long LargePagesFullColor;
        public long CoverageAverageBlack;
        public long CoverageAverageCyan;
        public long CoverageAverageMagenta;
        public long CoverageAverageYellow;
        public long BlackLevelMax;
        public long CyanLevelMax;
        public long MagentaLevelMax;
        public long YellowLevelMax;
        public long BlackLevel;
        public long CyanLevel;
        public long MagentaLevel;
        public long YellowLevel;

        public OidData(string vendorName, string model, string serialNumber, string macAddress, string ipAddress,
            string hostName, string descriptionLocation, string assetNumber, string firmwareVersion, string powerSleep1,
            string powerSleep2, string profileName, string deviceName, string deviceType, string manufacturer,
            long totalPages,
            long totalPagesMono, long totalPagesColor, long totalPagesDuplex, long printerPages, long printerPagesMono,
            long printerPagesColor, long printerPagesFullColor, long printerPagesTwoColor, long copyPagesMono,
            long copyPagesColor, long copyPagesFullColor, long copyPagesTwoColor, long copyPagesSingleColor,
            long faxesSentFaxesReceived, long scansTotalScansTotalMono, long scansTotalColor, long scansCopyMono,
            long scansCopyColor, long scansEmail, long scansEmailMono, long scansNet, long scansNetMono,
            long scansNetColor, long largePagesMono, long largePagesFullColor, long coverageAverageBlack,
            long coverageAverageCyan, long coverageAverageMagenta, long coverageAverageYellow, long blackLevelMax,
            long cyanLevelMax, long magentaLevelMax, long yellowLevelMax, long blackLevel, long cyanLevel,
            long magentaLevel, long yellowLevel)
        {
            VendorName = vendorName;
            Model = model;
            SerialNumber = serialNumber;
            MacAddress = macAddress;
            IpAddress = ipAddress;
            HostName = hostName;
            DescriptionLocation = descriptionLocation;
            AssetNumber = assetNumber;
            FirmwareVersion = firmwareVersion;
            PowerSleep1 = powerSleep1;
            PowerSleep2 = powerSleep2;
            ProfileName = profileName;
            DeviceName = deviceName;
            DeviceType = deviceType;
            Manufacturer = manufacturer;
            TotalPages = totalPages;
            TotalPagesMono = totalPagesMono;
            TotalPagesColor = totalPagesColor;
            TotalPagesDuplex = totalPagesDuplex;
            PrinterPages = printerPages;
            PrinterPagesMono = printerPagesMono;
            PrinterPagesColor = printerPagesColor;
            PrinterPagesFullColor = printerPagesFullColor;
            PrinterPagesTwoColor = printerPagesTwoColor;
            CopyPagesMono = copyPagesMono;
            CopyPagesColor = copyPagesColor;
            CopyPagesFullColor = copyPagesFullColor;
            CopyPagesTwoColor = copyPagesTwoColor;
            CopyPagesSingleColor = copyPagesSingleColor;
            FaxesSentFaxesReceived = faxesSentFaxesReceived;
            ScansTotalScansTotalMono = scansTotalScansTotalMono;
            ScansTotalColor = scansTotalColor;
            ScansCopyMono = scansCopyMono;
            ScansCopyColor = scansCopyColor;
            ScansEmail = scansEmail;
            ScansEmailMono = scansEmailMono;
            ScansNet = scansNet;
            ScansNetMono = scansNetMono;
            ScansNetColor = scansNetColor;
            LargePagesMono = largePagesMono;
            LargePagesFullColor = largePagesFullColor;
            CoverageAverageBlack = coverageAverageBlack;
            CoverageAverageCyan = coverageAverageCyan;
            CoverageAverageMagenta = coverageAverageMagenta;
            CoverageAverageYellow = coverageAverageYellow;
            BlackLevelMax = blackLevelMax;
            CyanLevelMax = cyanLevelMax;
            MagentaLevelMax = magentaLevelMax;
            YellowLevelMax = yellowLevelMax;
            BlackLevel = blackLevel;
            CyanLevel = cyanLevel;
            MagentaLevel = magentaLevel;
            YellowLevel = yellowLevel;
        }


        /// <summary>
        /// Transforms a List of OidData to a DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(List<OidData> list)
        {
            var dt = CrateEmptyTable();
            foreach (var data in list)
            {
                var dr = dt.NewRow();
                dr["CustomerReference"] = data.CustomerReference;
                dr["VendorName"] = data.VendorName;
                dr["Model"] = data.Model;
                dr["SerialNumber"] = data.SerialNumber;
                dr["MacAddress"] = data.MacAddress;
                dr["IpAddress"] = data.IpAddress;
                dr["HostName"] = data.HostName;
                dr["DescriptionLocation"] = data.DescriptionLocation;
                dr["AssetNumber"] = data.AssetNumber;
                dr["FirmwareVersion"] = data.FirmwareVersion;
                dr["PowerSleep1"] = data.PowerSleep1;
                dr["PowerSleep2"] = data.PowerSleep2;
                dr["ProfileName"] = data.ProfileName;
                dr["DeviceName"] = data.DeviceName;
                dr["DeviceType"] = data.DeviceType;
                dr["Manufacturer"] = data.Manufacturer;
                dr["TotalPages"] = data.TotalPages;
                dr["TotalPagesMono"] = data.TotalPagesMono;
                dr["TotalPagesColor"] = data.TotalPagesColor;
                dr["TotalPagesDuplex"] = data.TotalPagesDuplex;
                dr["PrinterPages"] = data.PrinterPages;
                dr["PrinterPagesMono"] = data.PrinterPagesMono;
                dr["PrinterPagesColor"] = data.PrinterPagesColor;
                dr["PrinterPagesFullColor"] = data.PrinterPagesFullColor;
                dr["PrinterPagesTwoColor"] = data.PrinterPagesTwoColor;
                dr["CopyPagesMono"] = data.CopyPagesMono;
                dr["CopyPagesColor"] = data.CopyPagesColor;
                dr["CopyPagesFullColor"] = data.CopyPagesFullColor;
                dr["CopyPagesTwoColor"] = data.CopyPagesTwoColor;
                dr["CopyPagesSingleColor"] = data.CopyPagesSingleColor;
                dr["FaxesSentFaxesReceived"] = data.FaxesSentFaxesReceived;
                dr["ScansTotalScansTotalMono"] = data.ScansTotalScansTotalMono;
                dr["ScansTotalColor"] = data.ScansTotalColor;
                dr["ScansCopyMono"] = data.ScansCopyMono;
                dr["ScansCopyColor"] = data.ScansCopyColor;
                dr["ScansEmail"] = data.ScansEmail;
                dr["ScansEmailMono"] = data.ScansEmailMono;
                dr["ScansNet"] = data.ScansNet;
                dr["ScansNetMono"] = data.ScansNetMono;
                dr["ScansNetColor"] = data.ScansNetColor;
                dr["LargePagesMono"] = data.LargePagesMono;
                dr["LargePagesFullColor"] = data.LargePagesFullColor;
                dr["CoverageAverageBlack"] = data.CoverageAverageBlack;
                dr["CoverageAverageCyan"] = data.CoverageAverageCyan;
                dr["CoverageAverageMagenta"] = data.CoverageAverageMagenta;
                dr["CoverageAverageYellow"] = data.CoverageAverageYellow;
                dr["BlackLevelMax"] = data.BlackLevelMax;
                dr["CyanLevelMax"] = data.CyanLevelMax;
                dr["MagentaLevelMax"] = data.MagentaLevelMax;
                dr["YellowLevelMax"] = data.YellowLevelMax;
                dr["BlackLevel"] = data.BlackLevel;
                dr["CyanLevel"] = data.CyanLevel;
                dr["MagentaLevel"] = data.MagentaLevel;
                dr["YellowLevel"] = data.YellowLevel;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// Creates an Empty Datatable and defines the Colums
        /// </summary>
        /// <returns></returns>
        private static DataTable CrateEmptyTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CustomerReference", typeof(string));
            dt.Columns.Add("VendorName", typeof(string));
            dt.Columns.Add("Model", typeof(string));
            dt.Columns.Add("SerialNumber", typeof(string));
            dt.Columns.Add("MacAddress", typeof(string));
            dt.Columns.Add("IpAddress", typeof(string));
            dt.Columns.Add("HostName", typeof(string));
            dt.Columns.Add("DescriptionLocation", typeof(string));
            dt.Columns.Add("AssetNumber", typeof(string));
            dt.Columns.Add("FirmwareVersion", typeof(string));
            dt.Columns.Add("PowerSleep1", typeof(string));
            dt.Columns.Add("PowerSleep2", typeof(string));
            dt.Columns.Add("ProfileName", typeof(string));
            dt.Columns.Add("DeviceName", typeof(string));
            dt.Columns.Add("DeviceType", typeof(string));
            dt.Columns.Add("Manufacturer", typeof(string));
            dt.Columns.Add("TotalPages", typeof(long));
            dt.Columns.Add("TotalPagesMono", typeof(long));
            dt.Columns.Add("TotalPagesColor", typeof(long));
            dt.Columns.Add("TotalPagesDuplex", typeof(long));
            dt.Columns.Add("PrinterPages", typeof(long));
            dt.Columns.Add("PrinterPagesMono", typeof(long));
            dt.Columns.Add("PrinterPagesColor", typeof(long));
            dt.Columns.Add("PrinterPagesFullColor", typeof(long));
            dt.Columns.Add("PrinterPagesTwoColor", typeof(long));
            dt.Columns.Add("CopyPagesMono", typeof(long));
            dt.Columns.Add("CopyPagesColor", typeof(long));
            dt.Columns.Add("CopyPagesFullColor", typeof(long));
            dt.Columns.Add("CopyPagesTwoColor", typeof(long));
            dt.Columns.Add("CopyPagesSingleColor", typeof(long));
            dt.Columns.Add("FaxesSentFaxesReceived", typeof(long));
            dt.Columns.Add("ScansTotalScansTotalMono", typeof(long));
            dt.Columns.Add("ScansTotalColor", typeof(long));
            dt.Columns.Add("ScansCopyMono", typeof(long));
            dt.Columns.Add("ScansCopyColor", typeof(long));
            dt.Columns.Add("ScansEmail", typeof(long));
            dt.Columns.Add("ScansEmailMono", typeof(long));
            dt.Columns.Add("ScansNet", typeof(long));
            dt.Columns.Add("ScansNetMono", typeof(long));
            dt.Columns.Add("ScansNetColor", typeof(long));
            dt.Columns.Add("LargePagesMono", typeof(long));
            dt.Columns.Add("LargePagesFullColor", typeof(long));
            dt.Columns.Add("CoverageAverageBlack", typeof(long));
            dt.Columns.Add("CoverageAverageCyan", typeof(long));
            dt.Columns.Add("CoverageAverageMagenta", typeof(long));
            dt.Columns.Add("CoverageAverageYellow", typeof(long));
            dt.Columns.Add("BlackLevelMax", typeof(long));
            dt.Columns.Add("CyanLevelMax", typeof(long));
            dt.Columns.Add("MagentaLevelMax", typeof(long));
            dt.Columns.Add("YellowLevelMax", typeof(long));
            dt.Columns.Add("BlackLevel", typeof(long));
            dt.Columns.Add("CyanLevel", typeof(long));
            dt.Columns.Add("MagentaLevel", typeof(long));
            dt.Columns.Add("YellowLevel", typeof(long));
            return dt;
        }

        public OidData()
        {
        }

        public override string ToString() //TESTING
        {
            return DeviceType;
        }
    }
}