using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace de.fearvel.openMPS.UC.CustomControls
{
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class DeviceProps : UserControl
    {
        public long Id { get; private set; }
        public bool Activ { get; private set; }
        public IPAddress IpAddress { get; private set; }
        public string Model { get; private set; }
        public string SerialNumber { get; private set; }
        public string AssetNumber { get; private set; }


        public DeviceProps(long id, bool activ, IPAddress ipAddress, string model, string serialNumber, string assetNumber)
        {
            Id = id;
            Activ = activ;
            IpAddress = ipAddress;
            Model = model;
            SerialNumber = serialNumber;
            AssetNumber = assetNumber;
            InitializeComponent();
            DisplayValues();
        }

        public void DisplayValues()
        {
            CheckBoxAktiv.IsChecked = Activ;
            LabelIp.Content = IpAddress;
            LabelModel.Content = Model;
            LabelSerialNumber.Content = SerialNumber;
            LabelAssetNumber.Content = AssetNumber;
        }

    }
}
