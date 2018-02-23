#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using de.as1259.openMPS.SNMP;
using de.as1259.openMPS.SQLiteConnectionTools;
using de.as1259.openMPS.Tools;

namespace de.as1259.openMPS.UC
{
    /// <summary>
    ///     Interaktionslogik für geraeteSuchen.xaml
    /// </summary>
    public partial class geraeteSuchen : UserControl
    {
        /// <summary>
        ///     The DataTable
        /// </summary>
        private DataTable dt;

        /// <summary>
        ///     The ip
        /// </summary>
        private IPAddress[] ip;

        /// <summary>
        ///     Initializes a new instance of the <see cref="geraeteSuchen" /> class.
        /// </summary>
        public geraeteSuchen()
        {
            InitializeComponent();
            Loaded += geraeteSuchen_Load;
        }

        /// <summary>
        ///     Loads the grid data.
        /// </summary>
        public void loadGridData()
        {
            dt = CounterConfig.shellDT("Select Aktiv,IP, Modell,Seriennummer,AssetNumber from Devices");
            geraeteGrid.ItemsSource = dt.DefaultView;
            geraeteGrid.IsReadOnly = true;
        }

        /// <summary>
        ///     Handles the Load event of the geraeteSuchen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void geraeteSuchen_Load(object sender, RoutedEventArgs e)
        {
            loadGridData();
            var ip = ScanIP.getIPMask();
            var ipaddr = ScanIP.findIPRange(ScanIP.getIPMask());
            var start = ipaddr[0].GetAddressBytes();
            var stop = ipaddr[1].GetAddressBytes();

            tb_ip_rangeStart1.Text = Convert.ToString(start[0]);
            tb_ip_rangeStart2.Text = Convert.ToString(start[1]);
            tb_ip_rangeStart3.Text = Convert.ToString(start[2]);
            tb_ip_rangeStart4.Text = Convert.ToString(start[3]);
            tb_ip_rangeStop1.Text = Convert.ToString(stop[0]);
            tb_ip_rangeStop2.Text = Convert.ToString(stop[1]);
            tb_ip_rangeStop3.Text = Convert.ToString(stop[2]);
            tb_ip_rangeStop4.Text = Convert.ToString(stop[3]);
        }

        /// <summary>
        ///     Handles the Click event of the bt_suchen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_suchen_Click(object sender, RoutedEventArgs e)
        {
            bt_suchen.Visibility = Visibility.Hidden;
            progress.Value = 0;
            ip = new IPAddress[2]
            {
                new IPAddress(ScanIP.convertStringToAddress(tb_ip_rangeStart1.Text + "." + tb_ip_rangeStart2.Text +
                                                            "." + tb_ip_rangeStart3.Text + "." +
                                                            tb_ip_rangeStart4.Text)),
                new IPAddress(ScanIP.convertStringToAddress(tb_ip_rangeStop1.Text + "." + tb_ip_rangeStop2.Text + "." +
                                                            tb_ip_rangeStop3.Text + "." + tb_ip_rangeStop4.Text))
            };
            ThreadPool.QueueUserWorkItem(searchForPrinter);
            ThreadPool.QueueUserWorkItem(adaptProgressLoad);
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
        ///     Handles the ValueChanged event of the progress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="System.Windows.RoutedPropertyChangedEventArgs{System.Double}" /> instance containing the
        ///     event data.
        /// </param>
        private void progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            progressPercent.Content = progress.Value + " %";
        }

        /// <summary>
        ///     Adapts the progress.
        /// </summary>
        private void adaptProgress()
        {
            if (progress.Value < 99) progress.Value += 1;
        }

        /// <summary>
        ///     Searches for printer.
        /// </summary>
        /// <param name="state">The state.</param>
        private void searchForPrinter(object state)
        {
            var dt = ScanIP.getAliveNetworkHosts(ip);

            if (dt.Rows.Count > 0)
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var ipAddress = dt.Rows[i].Field<string>("IP");
                    var ident = DeviceTools.identDevice(ipAddress);
                    var modell = "";
                    var serial = "";
                    var asset = "";
                    if (ident.Length > 0)
                        if (CounterConfig.shellDT(
                                "Select * from Devices where IP='" + dt.Rows[i].Field<string>("IP") + "';").Rows.Count >
                            0)
                        {
                            var dts = CounterConfig.shellDT(
                                "select * from OID where OIDPrivateID='" + ident + "'");
                            modell = SNMPget.getOIDValue(ipAddress, dts.Rows[0].Field<string>("Model"));
                            serial = SNMPget.getOIDValue(ipAddress, dts.Rows[0].Field<string>("SerialNumber"));
                            asset = SNMPget.getOIDValue(ipAddress, dts.Rows[0].Field<string>("AssetNumber"));
                            DeviceTools.updateDevices(
                                "1",
                                ScanIP.convertStringToAddress(ipAddress),
                                modell,
                                serial,
                                asset,
                                ScanIP.convertStringToAddress(ipAddress)
                            );
                        }
                        else
                        {
                            var dts = CounterConfig.shellDT(
                                "select * from OID where OIDPrivateID='" + ident + "'");
                            modell = SNMPget.getOIDValue(ipAddress, dts.Rows[0].Field<string>("Model"));
                            serial = SNMPget.getOIDValue(ipAddress, dts.Rows[0].Field<string>("SerialNumber"));
                            asset = SNMPget.getOIDValue(ipAddress, dts.Rows[0].Field<string>("AssetNumber"));
                            CounterConfig.shellDT("Delete from Devices where IP = '" +
                                                  dt.Rows[i].Field<string>("IP") + "'; ");
                            DeviceTools.insertInDevices(
                                "1",
                                ScanIP.convertStringToAddress(ipAddress),
                                modell,
                                serial,
                                asset
                            );
                        }
                }

            geraeteGrid.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(loadGridData));
            bt_suchen.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(showStartButton));
        }

        /// <summary>
        ///     Shows the start button.
        /// </summary>
        private void showStartButton()
        {
            bt_suchen.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Idents the device.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <returns></returns>
        private string identDevice(string ip)
        {
            var dt = CounterConfig.shellDT("Select * from OID");
            for (var i = 0; i < dt.Rows.Count; i++)
                if (SNMPget.getOIDValue(ip, dt.Rows[i].Field<string>("TotalPages")).Length > 0)
                    return dt.Rows[i].Field<string>("OIDPrivateID");
            return "Generic";
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStart1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_ip_rangeStart1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_rangeStart1.Text.Length == 3 ||
                (tb_ip_rangeStart1.Text.Contains(".") || tb_ip_rangeStart1.Text.Contains(" ")) &&
                tb_ip_rangeStart1.Text.Length > 1)
            {
                tb_ip_rangeStart1.Text = tb_ip_rangeStart1.Text.Replace(".", "");
                tb_ip_rangeStart1.Text = tb_ip_rangeStart1.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStart2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_ip_rangeStart2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_rangeStart2.Text.Length == 3 ||
                (tb_ip_rangeStart2.Text.Contains(".") || tb_ip_rangeStart2.Text.Contains(" ")) &&
                tb_ip_rangeStart2.Text.Length > 1)
            {
                tb_ip_rangeStart2.Text = tb_ip_rangeStart2.Text.Replace(".", "");
                tb_ip_rangeStart2.Text = tb_ip_rangeStart2.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStart3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_ip_rangeStart3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_rangeStart3.Text.Length == 3 ||
                (tb_ip_rangeStart3.Text.Contains(".") || tb_ip_rangeStart3.Text.Contains(" ")) &&
                tb_ip_rangeStart3.Text.Length > 1)
            {
                tb_ip_rangeStart3.Text = tb_ip_rangeStart3.Text.Replace(".", "");
                tb_ip_rangeStart3.Text = tb_ip_rangeStart3.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_ip_rangeStop1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_rangeStop1.Text.Length == 3 ||
                (tb_ip_rangeStop1.Text.Contains(".") || tb_ip_rangeStop1.Text.Contains(" ")) &&
                tb_ip_rangeStop1.Text.Length > 1)
            {
                tb_ip_rangeStop1.Text = tb_ip_rangeStop1.Text.Replace(".", "");
                tb_ip_rangeStop1.Text = tb_ip_rangeStop1.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_ip_rangeStop2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_rangeStop2.Text.Length == 3 ||
                (tb_ip_rangeStop2.Text.Contains(".") || tb_ip_rangeStop2.Text.Contains(" ")) &&
                tb_ip_rangeStop2.Text.Length > 1)
            {
                tb_ip_rangeStop2.Text = tb_ip_rangeStop2.Text.Replace(".", "");
                tb_ip_rangeStop2.Text = tb_ip_rangeStop2.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_ip_rangeStop3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_ip_rangeStop3.Text.Length == 3 ||
                (tb_ip_rangeStop3.Text.Contains(".") || tb_ip_rangeStop3.Text.Contains(" ")) &&
                tb_ip_rangeStop3.Text.Length > 1)
            {
                tb_ip_rangeStop3.Text = tb_ip_rangeStop3.Text.Replace(".", "");
                tb_ip_rangeStop3.Text = tb_ip_rangeStop3.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_ip_rangeStop4_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_ip_rangeStop3.Text = tb_ip_rangeStop3.Text.Replace(".", "");
            tb_ip_rangeStop3.Text = tb_ip_rangeStop3.Text.Replace(" ", "");
            var request = new TraversalRequest(0);
            request.Wrapped = true;
        }
    }
}