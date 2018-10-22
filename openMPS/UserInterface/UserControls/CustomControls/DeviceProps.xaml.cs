﻿using System.Net;
using System.Windows.Controls;

namespace de.fearvel.openMPS.UserInterface.UserControls.CustomControls
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
