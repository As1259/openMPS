﻿#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using de.fearvel.net.FnLog;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes;
using de.fearvel.openMPS.Net;

namespace de.fearvel.openMPS.UserInterface.UserControls
{

    /// <summary>
    ///     Interaktionslogik für abraegeStarten.xaml
    /// </summary>
    public partial class RetrieveDeviceInformation : UserControl
    {

        /// <summary>
        /// Saves the Information of the SNMP data Retrieve
        /// </summary>
        public List<OidData> _oidData { get; private set; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="RetrieveDeviceInformation" /> class.
        /// </summary>
        public RetrieveDeviceInformation()
        {
            InitializeComponent();
            Loaded += AbfrageStarten_Load;
        }

        /// <summary>
        ///     Handles the Load event of the abfrageStarten control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void AbfrageStarten_Load(object sender, RoutedEventArgs e)
        {
            ButtonSend.IsEnabled = false;
        }

        /// <summary>
        ///     Handles the Click event of the bt_start control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonRetrieveData_Click(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "RetrieveDeviceInformation", "Button ButtonRetrieveData Clicked");

            ProgressBarRetrieveData.Value = 0;
            ProgressBarRetrieveData.Visibility = Visibility.Visible;
            ThreadPool.QueueUserWorkItem(UpdateDataGrid);
            ThreadPool.QueueUserWorkItem(AdaptProgressLoad);
        }

        /// <summary>
        ///     Updates the grid.
        /// </summary>
        private void UpdateGrid()
        {
            DataGridItemViewer.ItemsSource = OidData.ToDataTable( _oidData).DefaultView;
            ButtonRetrieveData.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(GetNormalView));
            ButtonSend.IsEnabled = true;
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "RetrieveDeviceInformation", "Button ButtonRetrieveData Complete");
        }

        /// <summary>
        ///     Adapts the ProgressBarSearchProgress load.
        /// </summary>
        /// <param name="state">The state.</param>
        private void AdaptProgressLoad(object state)
        {
            for (var i = 0; i < 99; i++)
            {
                ProgressBarRetrieveData.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(AdaptProgress));
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        ///     Adapts the ProgressBarSearchProgress.
        /// </summary>
        private void AdaptProgress()
        {
            if (ProgressBarRetrieveData.Value < 99) ProgressBarRetrieveData.Value += 1;
        }

        public List<OidData> GainData()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "RetrieveDeviceInformation", "GainData");

            var dt = Config.GetInstance().Query("select * from Devices where active='1' or active='True'");
           // DataTable resultTable = null;
            var data = new List<OidData>();
            for (var i = 0; i < dt.Rows.Count; i++)
                if (DeviceTools.IdentDevice(dt.Rows[i].Field<string>("Ip")).Length > 0)
                    if (ScanIp.PingIp(new IPAddress(ScanIp.ConvertStringToAddress(dt.Rows[i].Field<string>("Ip")))))
                    {
                        if( SnmpClient.ReadDeviceOiDs(dt.Rows[i].Field<string>("Ip"),
                            DeviceTools.IdentDevice(dt.Rows[i].Field<string>("Ip")),out OidData oidData))
                        {

                            if (oidData!= null)
                            {
                                data.Add(oidData);

                            }
                        }
                    }
            return data;
        }


        /// <summary>
        ///     Updates the data grid.
        /// </summary>
        /// <param name="state">The state.</param>
        private void UpdateDataGrid(object state)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "RetrieveDeviceInformation", "UpdateDataGrid");

            var oidData = GainData();

               _oidData = oidData;
            //     dt = Collector.shellDT("Select * from Collector");
            // this.dt = dt;
            DataGridItemViewer.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(UpdateGrid));
                              



            }

        /// <summary>
        ///     Handles the Click event of the button_send control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "RetrieveDeviceInformation", "Button Send Clicked");
                OpenMPSClient.GetInstance().SendOidData(_oidData);
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "RetrieveDeviceInformation", "Button Send SUCCESS");

                MessageBox.Show("Daten wurden versandt");


            }
            catch (Exception)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "RetrieveDeviceInformation", "Button Send FAILED");

                MessageBox.Show("Fehler beim Senden");
            }
            ButtonSend.IsEnabled = false;

        }



        /// <summary>
        ///     Handles the ValueChanged event of the ProgressBarSearchProgress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="double" /> instance containing the
        ///     event data.
        /// </param>
        private void ProgressBarRetrieveData_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelPercent.Content = ProgressBarRetrieveData.Value + " %";
        }

        /// <summary>
        ///     Gets the normal view.
        /// </summary>
        private void GetNormalView()
        {
            ProgressBarRetrieveData.Value = 100;
            ProgressBarRetrieveData.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Handles the IsVisibleChanged event of the ProgressBarSearchProgress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event
        ///     data.
        /// </param>
        private void ProgressBarRetrieveData_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LabelPercent.Visibility = ProgressBarRetrieveData.Visibility;
        }

        #region "enable function for buttons for threading"

        /// <summary>
        ///     Bts the senden enable.
        /// </summary>
        private void bt_sendenEnable()
        {
            ButtonSend.IsEnabled = true;
        }


        /// <summary>
        ///     Bts the senden disable.
        /// </summary>
        private void bt_sendenDisable()
        {
            ButtonSend.IsEnabled = false;
        }


        #endregion
    }
}