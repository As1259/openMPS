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
using de.fearvel.net;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes.Exceptions;
using de.fearvel.openMPS.Net;

namespace de.fearvel.openMPS.UserInterface.UserControls
{
    /// <summary>
    ///     Interaktionslogik für geraeteSuchen.xaml
    /// </summary>
    public partial class SearchForDevices : UserControl
    {
        /// <summary>
        ///     The DataTable
        /// </summary>

        public IPAddress StartIpAddress { get; private set; }
        public IPAddress EndIpAddress { get; private set; }

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:de.fearvel.openMPS.UserInterface.geraeteSuchen" /> class.
        /// </summary>
        public SearchForDevices()
        {
            InitializeComponent();
            Loaded += SearchForDevices_Load;
        }

        /// <summary>
        ///     Loads the grid data.
        /// </summary>
        public void LoadGridData()
        {
            Config.GetInstance().UpdateDevices();
            geraeteGrid.ItemsSource = Config.GetInstance().Devices.DefaultView;
            geraeteGrid.Columns[5].Visibility = Visibility.Hidden;

            geraeteGrid.IsReadOnly = true;
        }

        /// <summary>
        ///     Handles the Load event of the geraeteSuchen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void SearchForDevices_Load(object sender, RoutedEventArgs e)
        {
            geraeteGrid.ItemsSource = Config.GetInstance().Devices.DefaultView;
            geraeteGrid.Columns[5].Visibility = Visibility.Hidden;

            var ip = ScanIp.GetIpMask();
            var ipaddr = ScanIp.FindIpRange(ScanIp.GetIpMask());
            var start = ipaddr[0].GetAddressBytes();
            var stop = ipaddr[1].GetAddressBytes();

            TextBoxIpRangeStart1.Text = Convert.ToString(start[0]);
            TextBoxIpRangeStart2.Text = Convert.ToString(start[1]);
            TextBoxIpRangeStart3.Text = Convert.ToString(start[2]);
            TextBoxIpRangeStart4.Text = Convert.ToString(start[3]);
            TextBoxIpRangeStop1.Text = Convert.ToString(stop[0]);
            TextBoxIpRangeStop2.Text = Convert.ToString(stop[1]);
            TextBoxIpRangeStop3.Text = Convert.ToString(stop[2]);
            TextBoxIpRangeStop4.Text = Convert.ToString(stop[3]);
        }

        /// <summary>
        ///     Handles the Click event of the Button_suchen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Suchen_Click(object sender, RoutedEventArgs e)
        {
            ButtonSuchen.Visibility = Visibility.Hidden;
            progress.Value = 0;

            StartIpAddress = new IPAddress(ScanIp.ConvertStringToAddress(TextBoxIpRangeStart1.Text + "." + TextBoxIpRangeStart2.Text +
                                                        "." + TextBoxIpRangeStart3.Text + "." +
                                                        TextBoxIpRangeStart4.Text));
            EndIpAddress = new IPAddress(ScanIp.ConvertStringToAddress(TextBoxIpRangeStop1.Text + "." + TextBoxIpRangeStop2.Text + "." +
                                                            TextBoxIpRangeStop3.Text + "." + TextBoxIpRangeStop4.Text));

            ThreadPool.QueueUserWorkItem(SearchForPrinter);
            ThreadPool.QueueUserWorkItem(AdaptProgressLoad);
        }

        /// <summary>
        ///     Adapts the progress load.
        /// </summary>
        /// <param name="state">The state.</param>
        private void AdaptProgressLoad(object state)
        {
            for (var i = 0; i < 99; i++)
            {
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(AdaptProgress));
                Thread.Sleep(5000);
            }
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
        ///     Adapts the progress.
        /// </summary>
        private void AdaptProgress()
        {
            if (progress.Value < 99) progress.Value += 1;
        }

        /// <summary>
        ///     Searches for printer.
        /// </summary>
        /// <param name="state">The state.</param>
        private void SearchForPrinter(object state)
        {
            var fp = new FnPing(StartIpAddress, EndIpAddress);

            var pingResultsIps = fp.RangePing();


            foreach (var ipAddress in pingResultsIps.SuccessIpAddresses)
            {
                try
                {
                    var ident = DeviceTools.IdentDevice(ipAddress.ToString());
                    var modell = "";
                    var serial = "";
                    var asset = "";
                    if (ident.Length > 0)
                        if (Config.GetInstance().Query(
                                "Select * from Devices where IP='" + ipAddress.ToString() + "';").Rows.Count >
                            0)
                        {
                            var dts = Config.GetInstance().GetOidRowByPrivateId(ident);
                            modell = SnmpClient.GetOidValue(ipAddress.ToString(), dts.Rows[0].Field<string>("Model"));
                            serial = SnmpClient.GetOidValue(ipAddress.ToString(), dts.Rows[0].Field<string>("SerialNumber"));
                            asset = SnmpClient.GetOidValue(ipAddress.ToString(), dts.Rows[0].Field<string>("AssetNumber"));
                            Config.GetInstance().UpdateDeviceTable(
                                "1",
                                ScanIp.ConvertStringToAddress(ipAddress.ToString()),
                                modell,
                                serial,
                                asset,
                                ScanIp.ConvertStringToAddress(ipAddress.ToString())
                            );
                        }
                        else
                        {
                            var dts = Config.GetInstance().GetOidRowByPrivateId(ident);

                            modell = SnmpClient.GetOidValue(ipAddress.ToString(), dts.Rows[0].Field<string>("Model"));
                            serial = SnmpClient.GetOidValue(ipAddress.ToString(), dts.Rows[0].Field<string>("SerialNumber"));
                            asset = SnmpClient.GetOidValue(ipAddress.ToString(), dts.Rows[0].Field<string>("AssetNumber"));
                            Config.GetInstance().Query("Delete from Devices where IP = '" +
                                                   ipAddress.ToString() + "'; ");
                            Config.GetInstance().InsertInDeviceTable(
                                "1",
                                ScanIp.ConvertStringToAddress(ipAddress.ToString()),
                                modell,
                                serial,
                                asset
                            );

                        }
                    geraeteGrid.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(LoadGridData));

                    ButtonSuchen.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ShowStartButton));
                }
                catch (SnmpIdentNotFoundException)
                {
                }

            }
        }


        /// <summary>
            ///     Shows the start button.
            /// </summary>
            private void ShowStartButton()
        {
            ButtonSuchen.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Handles the TextChanged event of the TextBox_ip_rangeStart1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBox_ip_rangeStart1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpRangeStart1.Text.Length == 3 ||
                (TextBoxIpRangeStart1.Text.Contains(".") || TextBoxIpRangeStart1.Text.Contains(" ")) &&
                TextBoxIpRangeStart1.Text.Length > 1)
            {
                TextBoxIpRangeStart1.Text = TextBoxIpRangeStart1.Text.Replace(".", "");
                TextBoxIpRangeStart1.Text = TextBoxIpRangeStart1.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the TextBox_ip_rangeStart2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBox_ip_rangeStart2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpRangeStart2.Text.Length == 3 ||
                (TextBoxIpRangeStart2.Text.Contains(".") || TextBoxIpRangeStart2.Text.Contains(" ")) &&
                TextBoxIpRangeStart2.Text.Length > 1)
            {
                TextBoxIpRangeStart2.Text = TextBoxIpRangeStart2.Text.Replace(".", "");
                TextBoxIpRangeStart2.Text = TextBoxIpRangeStart2.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the TextBox_ip_rangeStart3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBox_ip_rangeStart3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpRangeStart3.Text.Length == 3 ||
                (TextBoxIpRangeStart3.Text.Contains(".") || TextBoxIpRangeStart3.Text.Contains(" ")) &&
                TextBoxIpRangeStart3.Text.Length > 1)
            {
                TextBoxIpRangeStart3.Text = TextBoxIpRangeStart3.Text.Replace(".", "");
                TextBoxIpRangeStart3.Text = TextBoxIpRangeStart3.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBox_ip_rangeStop1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpRangeStop1.Text.Length == 3 ||
                (TextBoxIpRangeStop1.Text.Contains(".") || TextBoxIpRangeStop1.Text.Contains(" ")) &&
                TextBoxIpRangeStop1.Text.Length > 1)
            {
                TextBoxIpRangeStop1.Text = TextBoxIpRangeStop1.Text.Replace(".", "");
                TextBoxIpRangeStop1.Text = TextBoxIpRangeStop1.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBox_ip_rangeStop2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpRangeStop2.Text.Length == 3 ||
                (TextBoxIpRangeStop2.Text.Contains(".") || TextBoxIpRangeStop2.Text.Contains(" ")) &&
                TextBoxIpRangeStop2.Text.Length > 1)
            {
                TextBoxIpRangeStop2.Text = TextBoxIpRangeStop2.Text.Replace(".", "");
                TextBoxIpRangeStop2.Text = TextBoxIpRangeStop2.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBox_ip_rangeStop3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpRangeStop3.Text.Length == 3 ||
                (TextBoxIpRangeStop3.Text.Contains(".") || TextBoxIpRangeStop3.Text.Contains(" ")) &&
                TextBoxIpRangeStop3.Text.Length > 1)
            {
                TextBoxIpRangeStop3.Text = TextBoxIpRangeStop3.Text.Replace(".", "");
                TextBoxIpRangeStop3.Text = TextBoxIpRangeStop3.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox)sender).MoveFocus(request);
            }
        }
    }
}