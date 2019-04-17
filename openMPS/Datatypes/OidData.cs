using System;
using de.fearvel.net.DataTypes.AbstractDataTypes;

namespace de.fearvel.openMPS.DataTypes
{
    /// <summary>
    /// Container Class for the received Oid 
    /// </summary>
    public class OidData : JsonSerializable<OidData>
    {
        public string CustomerRef;
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
            string powerSleep2, string profileName, string deviceName, string deviceType, string manufacturer, long totalPages,
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

        public OidData()
        {
        }

        public override string ToString()//TESTING
        {
            return DeviceType;
        }
    }
}