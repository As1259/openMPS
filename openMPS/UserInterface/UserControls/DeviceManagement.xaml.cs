﻿// Copyright (c) 2018 / 2019, Andreas Schreiner

using System;
using System.Data;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.Interfaces;
using de.fearvel.openMPS.UserInterface.UserControls.CustomControls;

namespace de.fearvel.openMPS.UserInterface.UserControls
{
    /// <summary>
    /// Interaktionslogik für DeviceManagement.xaml
    /// EXPERIMENTAL
    /// </summary>
    public partial class DeviceManagement : UserControl, IRibbonAdvisoryText
    {
        public DeviceManagement()
        {
            InitializeComponent();
        }

        private void progress_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStop3_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStop2_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStop1_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStart3_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStart2_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStart1_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_Suchen_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            throw new NotImplementedException();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            FillListView();
        }

        private void FillListView()
        {
            foreach (DataRow row in Config.GetInstance().Devices.Rows)
            {
                ListViewDevices.Items.Add(
                    new DeviceProps(row.Field<long>("id"),
                        row.Field<bool>("Active"),
                        IPAddress.Parse( row.Field<string>("Ip")),
                        row.Field<string>("Model"),
                        row.Field<string>("SerialNumber"),
                        row.Field<string>("AssetNumber")));
            }
        }

        /// <summary>
        /// AdvisoryText displayed in the Ribbon bar
        /// </summary>
        public string AdvisoryText =>
            "Hier können Sie neue Geräte suchen, bearbeiten, oder die IP-Adressen neuer Geräte manuell hinzufügen." +
            " Über die Kennzeichnung „Aktiv“ können Sie entscheiden, ob zu einem Gerät Werte abgefragt und übermittelt " +
            "werden, oder nicht.";
    }
}
