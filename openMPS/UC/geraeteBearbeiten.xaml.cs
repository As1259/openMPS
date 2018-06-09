#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.SNMP;
using de.fearvel.openMPS.Tools;

namespace de.fearvel.openMPS.UC
{
    /// <summary>
    ///     Interaktionslogik für geraeteBearbeiten.xaml
    /// </summary>
    public partial class geraeteBearbeiten : UserControl
    {
        /// <summary>
        ///     The DataRowView
        /// </summary>
        private DataRowView drv;

        /// <summary>
        ///     The DataTable
        /// </summary>
        private DataTable dt;

        /// <summary>
        ///     bool for selected on grid
        /// </summary>
        private bool selected;

        /// <summary>
        ///     Initializes a new instance of the <see cref="geraeteBearbeiten" /> class.
        /// </summary>
        public geraeteBearbeiten()
        {
            InitializeComponent();
            Loaded += geraeteSuchen_Load;
        }

        /// <summary>
        ///     Handles the Load event of the geraeteSuchen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void geraeteSuchen_Load(object sender, RoutedEventArgs e)
        {
            loadGridData();
        }

        /// <summary>
        ///     Loads the grid data.
        /// </summary>
        public void loadGridData()
        {
            dt = Config.GetInstance().Query("Select * from devices");
            geraeteGrid.ItemsSource = dt.DefaultView;
            //   geraeteGrid.IsReadOnly = true;
            geraeteGrid.Columns[5].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[0].IsReadOnly = true;
            geraeteGrid.Columns[1].IsReadOnly = true;
            geraeteGrid.Columns[2].IsReadOnly = true;
            geraeteGrid.Columns[3].IsReadOnly = true;
            geraeteGrid.Columns[4].IsReadOnly = true;

            //geraeteGrid.Columns[1].Width = 120;
            //geraeteGrid.Columns[4].Width = 300;
        }

        /// <summary>
        ///     Handles the SelectedCellsChanged event of the geraeteGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Controls.SelectedCellsChangedEventArgs" /> instance containing the event
        ///     data.
        /// </param>
        private void geraeteGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                drv = (DataRowView)geraeteGrid.SelectedItem;
                var ipAddress = ScanIP.ConvertStringToAddress(drv["IP"].ToString());
                unlockElements();
                bt_del.IsEnabled = true;
                tb_ip_block1.Text = ipAddress[0].ToString();
                tb_ip_block2.Text = ipAddress[1].ToString();
                tb_ip_block3.Text = ipAddress[2].ToString();
                tb_ip_block4.Text = ipAddress[3].ToString();
                tb_SerialNumber.Text = drv["Seriennummer"].ToString();
                //tb_assetnumber.Text = drv["Assetnumber"].ToString();
                if (drv["Aktiv"].ToString().Contains("True"))
                    cb_aktiv.IsChecked = true;
                else
                    cb_aktiv.IsChecked = false;

                selected = true;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///     Locks the elements.
        /// </summary>
        private void lockElements()
        {
            bt_save.IsEnabled = false;
            tb_ip_block1.IsEnabled = false;
            tb_ip_block2.IsEnabled = false;
            tb_ip_block3.IsEnabled = false;
            tb_ip_block4.IsEnabled = false;
            cb_aktiv.IsEnabled = false;
            bt_del.IsEnabled = false;
        }

        /// <summary>
        ///     Unlocks the elements.
        /// </summary>
        private void unlockElements()
        {
            bt_save.IsEnabled = true;
            tb_ip_block1.IsEnabled = true;
            tb_ip_block2.IsEnabled = true;
            tb_ip_block3.IsEnabled = true;
            tb_ip_block4.IsEnabled = true;
            cb_aktiv.IsEnabled = true;
        }

        /// <summary>
        ///     Handles the Click event of the bt_save control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            var ipAddress =
                tb_ip_block1.Text + "." +
                tb_ip_block2.Text + "." +
                tb_ip_block3.Text + "." +
                tb_ip_block4.Text;
            var aktiv = "1";
            try
            {
                ScanIP.ConvertStringToAddress(ipAddress);
                if (!(bool)cb_aktiv.IsChecked) aktiv = "0";
                if (selected)
                {
                    if (drv["IP"].ToString().CompareTo(ipAddress) == 0)
                    {
                        Config.GetInstance().NonQuery("update Devices set Aktiv='" + aktiv + "' where IP='" + ipAddress +
                                            "';");
                        loadGridData();
                        lockElements();
                    }
                    else
                    {
                        var thread = new Thread(UpdateDevicesViaThread);
                        thread.Start(new object[] { ipAddress, aktiv, drv["IP"].ToString() });
                        lockElements();
                    }
                }
                else
                {
                    if (Config.GetInstance().Query(
                            "Select * from Devices where ip='" + ipAddress + "'").Rows.Count > 0)
                    {
                        MessageBox.Show(
                            "Diese IP wird derzeit schon verwendet\nZum Ändern wählen sie das gerät in der tabelle\nund tragen den neuen Wert ein");
                    }
                    else
                    {
                        lockElements();
                        var thread = new Thread(InsertInDevicesViaThread);
                        thread.Start(new object[] { ipAddress, aktiv });
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fehlerhafte Eingabe der IP", "IPEingabeError", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///     Updates the bekannte geraete via thread.
        /// </summary>
        /// <param name="param">The parameter.</param>
        private void UpdateDevicesViaThread(object param)
        {
            var obj = (object[])param;
            var ipAddress = (string)obj[0];
            var aktiv = (string)obj[1];
            var altIP = (string)obj[2];
            var ident = DeviceTools.identDevice(ipAddress);
            var modell = "";
            var serial = "";
            var asset = "";

            var ip = ScanIP.ConvertStringToAddress(ipAddress);
            progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));

            if (ident.Length > 0)
            {
                var dt = OID.GetInstance().GetOidRowByPrivateId(ident);
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                modell = SNMPget.GetOidValue(ipAddress, dt.Rows[0].Field<string>("Model"));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                serial = SNMPget.GetOidValue(ipAddress, dt.Rows[0].Field<string>("SerialNumber"));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                asset = SNMPget.GetOidValue(ipAddress, dt.Rows[0].Field<string>("AssetNumber"));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
            }
            else
            {
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
            }

            var ipAlt = ScanIP.ConvertStringToAddress(altIP);
            Config.GetInstance().UpdateDeviceTable(
                aktiv,
                ip,
                modell,
                serial,
                asset,
                ipAlt
            );

            geraeteGrid.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(loadGridData));
        }

        /// <summary>
        ///     Inserts the in bekannte geraete via thread.
        /// </summary>
        /// <param name="param">The parameter.</param>
        private void InsertInDevicesViaThread(object param)
        {
            var obj = (object[])param;
            var ipAddress = (string)obj[0];
            var aktiv = (string)obj[1];
            var ident = DeviceTools.identDevice(ipAddress);
            var modell = "";
            var serial = "";
            var asset = "";
            var ip = ScanIP.ConvertStringToAddress(ipAddress);
            progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));

            if (ident.Length > 0)
            {
                var dt = OID.GetInstance().GetOidRowByPrivateId(ident);
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                modell = SNMPget.GetOidValue(ipAddress, dt.Rows[0].Field<string>("Model"));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                serial = SNMPget.GetOidValue(ipAddress, dt.Rows[0].Field<string>("SerialNumber"));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                asset = SNMPget.GetOidValue(ipAddress, dt.Rows[0].Field<string>("AssetNumber"));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
            }
            else
            {
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));
            }

            Config.GetInstance().InsertInDeviceTable(
                 aktiv,
                 ip,
                 modell,
                 serial,
                 asset
             );
            geraeteGrid.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(loadGridData));
        }

        /// <summary>
        ///     Adjusts the progress.
        /// </summary>
        private void adjustProgress()
        {
            if (progress.Value >= 80)
            {
                progress.Visibility = Visibility.Hidden;
            }
            else
            {
                progress.Visibility = Visibility.Visible;
                progress.Value += 20;
            }
        }

        /// <summary>
        ///     Handles the Click event of the bt_anlegen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_anlegen_Click(object sender, RoutedEventArgs e)
        {
            selected = false;
            bt_save.IsEnabled = true;
            unlockElements();
            geraeteGrid.UnselectAll();

            tb_ip_block1.Clear();
            tb_ip_block2.Clear();
            tb_ip_block3.Clear();
            tb_ip_block4.Clear();
            tb_SerialNumber.Clear();
            // tb_assetnumber.Clear();
            cb_aktiv.IsChecked = true;
        }

        /// <summary>
        ///     Handles the switch event of the tb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_switch(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_block1.Text.Length == 3 || (tb_ip_block1.Text.Contains(".") || tb_ip_block1.Text.Contains(" ")) &&
                tb_ip_block1.Text.Length > 1)
            {
                tb_ip_block1.Text = tb_ip_block1.Text.Replace(".", "");
                tb_ip_block1.Text = tb_ip_block1.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the switch2 event of the tb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_switch2(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_block2.Text.Length == 3 || (tb_ip_block2.Text.Contains(".") || tb_ip_block2.Text.Contains(" ")) &&
                tb_ip_block2.Text.Length > 1)
            {
                tb_ip_block2.Text = tb_ip_block2.Text.Replace(".", "");
                tb_ip_block2.Text = tb_ip_block2.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the switch3 event of the tb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_switch3(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_block3.Text.Length == 3 || (tb_ip_block3.Text.Contains(".") || tb_ip_block3.Text.Contains(" ")) &&
                tb_ip_block3.Text.Length > 1)
            {
                tb_ip_block3.Text = tb_ip_block3.Text.Replace(".", "");
                tb_ip_block3.Text = tb_ip_block3.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the Click event of the bt_del control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_del_Click(object sender, RoutedEventArgs e)
        {
            bt_del.IsEnabled = false;
            bt_save.IsEnabled = false;
            var ipAddress =
                tb_ip_block1.Text + "." +
                tb_ip_block2.Text + "." +
                tb_ip_block3.Text + "." +
                tb_ip_block4.Text;
            if (selected)
                try
                {
                    Config.GetInstance().NonQuery("delete from Devices where IP='" + ipAddress + "';");
                    loadGridData();
                }
                catch (Exception)
                {
                }
        }

        /// <summary>
        ///     Handles the Click event of the bt_anleitung control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_anleitung_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gerät neu anlegen"
                            + "Klicken Sie auf den Button „Gerät neu anlegen“ und erfassen anschließend"
                            + "die IPv4 - Adresse in den vorgesehenen Feldern"
                , "Anleitung", MessageBoxButton.OK, MessageBoxImage.Information);
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
            progresslabel.Visibility = progress.Visibility;
        }

        /// <summary>
        ///     Handles the ValueChanged event of the progress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="System.Windows.RoutedPropertyChangedEventArgs{System.Double}" /> instance containing the
        ///     event data.
        /// </param>
        private void progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            progresslabel.Content = progress.Value + " %";
        }
    }
}