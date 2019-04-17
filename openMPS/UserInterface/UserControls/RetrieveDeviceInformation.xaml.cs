#region Copyright

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
using de.fearvel.manastone.serialManagement;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes;
using de.fearvel.openMPS.SNMP;
using de.fearvel.openMPS.SocketIo;
using de.fearvel.openMPS.Tools;

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
            bt_senden.IsEnabled = false;
        }

        /// <summary>
        ///     Handles the Click event of the bt_start control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_retrieveData_Click(object sender, RoutedEventArgs e)
        {
            progress.Value = 0;
            progress.Visibility = Visibility.Visible;
            ThreadPool.QueueUserWorkItem(UpdateDataGrid);
            ThreadPool.QueueUserWorkItem(adaptProgressLoad);
        }

        /// <summary>
        ///     Updates the grid.
        /// </summary>
        private void updateGrid()
        {
            ListViewData.ItemsSource = _oidData;
            progressPercent.Content = 100;
            progressPercent.Visibility = Visibility.Hidden;
            bt_retrieveData.Visibility = Visibility.Visible;
            bt_senden.IsEnabled = true;


        }

        /// <summary>
        ///     Adapts the progress load.
        /// </summary>
        /// <param name="state">The state.</param>
        private void adaptProgressLoad(object state)
        {
            for (var i = 0; i < 99; i++)
            {
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adaptProgress));
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        ///     Adapts the progress.
        /// </summary>
        private void adaptProgress()
        {
            if (progress.Value < 99) progress.Value += 1;
        }

        public List<OidData> GainData()
        {
            var dt = Config.GetInstance().Query("select * from Devices where aktiv='1' or aktiv='True'");
           // DataTable resultTable = null;
            var data = new List<OidData>();
            for (var i = 0; i < dt.Rows.Count; i++)
                if (DeviceTools.identDevice(dt.Rows[i].Field<string>("ip")).Length > 0)
                    if (ScanIP.PingIp(new IPAddress(ScanIP.ConvertStringToAddress(dt.Rows[i].Field<string>("ip")))))
                    {
                        if( SNMPget.ReadDeviceOiDs(dt.Rows[i].Field<string>("ip"),
                            DeviceTools.identDevice(dt.Rows[i].Field<string>("ip")),out OidData oidData))
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

                var oidData = GainData();

               _oidData = oidData;
            //     dt = Collector.shellDT("Select * from Collector");
            // this.dt = dt;
            ListViewData.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(updateGrid));
                              



            }

        private ObservableCollection<T>  ListToObservableCollection<T>(List<T> oid)
        {
            var oc = new ObservableCollection<T>();
            foreach (var item in oid)
            {
                oc.Add(item);
            }
            return oc;
        }


        /// <summary>
        ///     Handles the Click event of the bt_senden control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void button_send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenMPSClient.GetInstance().SendOidData(_oidData);
            }
            catch (Exception)
            {
                MessageBox.Show("Fehler beim Senden");
            }
        }

        private string prepareOpenMPSFile()
        {
            Collector.shellDT("update INFO set version=" + Config.GetInstance().Query(
                                  "Select version from INFO").Rows[0].Field<long>("version") + ";");
            Collector.shellDT("update INFO set OIDversion=" + Config.GetInstance().Query(
                                  "Select OIDversion from INFO").Rows[0].Field<long>("OIDversion") + ";");

            var filename =
                "DateiZumSenden-" + SerialManager.GetSerialContainer(MainWindow.Programid).CustomerIdentificationNumber
                                  + "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".oMPS";
            File.Copy("CollectionData.oData", filename);
            var fInfo = new FileInfo(filename)
            {
                IsReadOnly = true
            };
            return filename;
        }

        /// <summary>
        ///     Handles the ValueChanged event of the progress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="double" /> instance containing the
        ///     event data.
        /// </param>
        private void progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            progressPercent.Content = progress.Value + " %";
        }

        /// <summary>
        ///     Gets the normal view.
        /// </summary>
        private void GetNormalView()
        {
            progress.Value = 100;
            progress.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Handles the IsVisibleChanged event of the progress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event
        ///     data.
        /// </param>
        private void progress_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            progressPercent.Visibility = progress.Visibility;
        }

        #region "enable function for buttons for threading"

        /// <summary>
        ///     Bts the senden enable.
        /// </summary>
        private void bt_sendenEnable()
        {
            bt_senden.IsEnabled = true;
        }


        /// <summary>
        ///     Bts the senden disable.
        /// </summary>
        private void bt_sendenDisable()
        {
            bt_senden.IsEnabled = false;
        }


        #endregion
    }
}