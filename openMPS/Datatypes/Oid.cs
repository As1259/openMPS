using de.fearvel.net.DataTypes.AbstractDataTypes;

namespace de.fearvel.openMPS.DataTypes
{
    /// <summary>
    /// Container Class for the received Oid 
    /// </summary>
    public class Oid : JsonSerializable<Oid>
    {
        public string VendorName;
        public string OidPrivateId;
        public string ProfileName;
        public string DeviceName;
        public string DeviceType;
        public string Manufacturer;
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
        public string TotalPages;
        public string TotalPagesMono;
        public string TotalPagesColor;
        public string TotalPagesDuplex;
        public string PrinterPages;
        public string PrinterPagesMono;
        public string PrinterPagesColor;
        public string PrinterPagesFullColor;
        public string PrinterPagesTwoColor;
        public string CopyPagesMono;
        public string CopyPagesColor;
        public string CopyPagesFullColor;
        public string CopyPagesTwoColor;
        public string CopyPagesSingleColor;
        public string FaxesSentFaxesReceived;
        public string ScansTotalScansTotalMono;
        public string ScansTotalColor;
        public string ScansCopyMono;
        public string ScansCopyColor;
        public string ScansEmail;
        public string ScansEmailMono;
        public string ScansNet;
        public string ScansNetMono;
        public string ScansNetColor;
        public string LargePagesMono;
        public string LargePagesFullColor;
        public string CoverageAverageBlack;
        public string CoverageAverageCyan;
        public string CoverageAverageMagenta;
        public string CoverageAverageYellow;
        public string BlackLevelMax;
        public string CyanLevelMax;
        public string MagentaLevelMax;
        public string YellowLevelMax;
        public string BlackLevel;
        public string CyanLevel;
        public string MagentaLevel;
        public string YellowLevel;
    }
}