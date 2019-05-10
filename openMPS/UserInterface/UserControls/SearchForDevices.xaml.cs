using System;
using System.Data;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using de.fearvel.net;
using de.fearvel.net.FnLog;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes.Exceptions;
using de.fearvel.openMPS.Net;
using de.fearvel.openMPS.Interfaces;

namespace de.fearvel.openMPS.UserInterface.UserControls
{
    /// <summary>
    /// Interaktionslogik für geraeteSuchen.xaml
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public partial class SearchForDevices : UserControl, IRibbonAdvisoryText
    {
        /// <summary>
        /// Start IpAddress for the Mps Device scan
        /// </summary>
        public IPAddress StartIpAddress { get; private set; }

        /// <summary>
        /// End IpAddress for the Mps Device scan
        /// </summary>
        public IPAddress EndIpAddress { get; private set; }

        /// <summary>
        /// constructor
        /// </summary>
        public SearchForDevices()
        {
            InitializeComponent();
            Loaded += SearchForDevices_Load;
        }

        /// <summary>
        /// Loads the grid data.
        /// </summary>
        public void LoadGridData()
        {
            Config.GetInstance().UpdateDevices();
            var dt = Config.GetInstance().Devices;
            DataGridDevices.ItemsSource = dt.DefaultView;
            DataGridDevices.Columns[5].Visibility = Visibility.Hidden;

            DataGridDevices.IsReadOnly = true;
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SearchForDevices",
                "Search Complete Count: " + dt.Rows.Count);
        }

        /// <summary>
        /// Handles the Load event of the geraeteSuchen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void SearchForDevices_Load(object sender, RoutedEventArgs e)
        {
            DataGridDevices.ItemsSource = Config.GetInstance().Devices.DefaultView;
            DataGridDevices.Columns[5].Visibility = Visibility.Hidden;

            var ip = ScanIp.GetIpMask();
            var ipaddr = ScanIp.FindIpRange(ScanIp.GetIpMask());
            var start = ipaddr[0].GetAddressBytes();
            var stop = ipaddr[1].GetAddressBytes();

            TextBoxIpFirstSegmentOne.Text = Convert.ToString(start[0]);
            TextBoxIpFirstSegmentTwo.Text = Convert.ToString(start[1]);
            TextBoxIpFirstSegmentThree.Text = Convert.ToString(start[2]);
            TextBoxIpFirstSegmentFour.Text = Convert.ToString(start[3]);
            TextBoxIpSecondSegmentOne.Text = Convert.ToString(stop[0]);
            TextBoxIpSecondSegmentTwo.Text = Convert.ToString(stop[1]);
            TextBoxIpSecondSegmentThree.Text = Convert.ToString(stop[2]);
            TextBoxIpSecondSegmentFour.Text = Convert.ToString(stop[3]);
        }

        /// <summary>
        /// Handles the Click event of the Button_suchen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance()
                .AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SearchForDevices", "Search Button pressed");

            ButtonSearch.Visibility = Visibility.Hidden;
            ProgressBarSearchProgress.Value = 0;

            StartIpAddress =  IPAddress.Parse(
                TextBoxIpFirstSegmentOne.Text + "." + TextBoxIpFirstSegmentTwo.Text +
                "." + TextBoxIpFirstSegmentThree.Text + "." +
                TextBoxIpFirstSegmentFour.Text);
            EndIpAddress = IPAddress.Parse(TextBoxIpSecondSegmentOne.Text + "." +
                                                                       TextBoxIpSecondSegmentTwo.Text + "." +
                                                                       TextBoxIpSecondSegmentThree.Text + "." +
                                                                       TextBoxIpSecondSegmentFour.Text);

            ThreadPool.QueueUserWorkItem(SearchForPrinter);
            ThreadPool.QueueUserWorkItem(AdaptProgressLoad);
        }

        /// <summary>
        /// Adapts the ProgressBarSearchProgress load.
        /// </summary>
        /// <param name="state">The state.</param>
        private void AdaptProgressLoad(object state)
        {
            for (var i = 0; i < 99; i++)
            {
                ProgressBarSearchProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(AdaptProgress));
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the ProgressBarSearchProgress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="double" /> instance containing the
        /// event data.
        /// </param>
        private void progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelPercent.Content = ProgressBarSearchProgress.Value + " %";
        }

        /// <summary>
        /// Adapts the ProgressBarSearchProgress.
        /// </summary>
        private void AdaptProgress()
        {
            if (ProgressBarSearchProgress.Value < 99) ProgressBarSearchProgress.Value += 1;
        }

        /// <summary>
        /// Searches for printer.
        /// </summary>
        /// <param name="state">The state.</param>
        private void SearchForPrinter(object state)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SearchForDevices", "SearchForPrinter");

            var fp = new FnPing(StartIpAddress, EndIpAddress);

            var pingResultsIps = fp.RangePing();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SearchForDevices",
                "SearchForPrinter - pingResultsIps Success Count:" + pingResultsIps.SuccessIpAddresses.Count);

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
                            serial = SnmpClient.GetOidValue(ipAddress.ToString(),
                                dts.Rows[0].Field<string>("SerialNumber"));
                            asset = SnmpClient.GetOidValue(ipAddress.ToString(),
                                dts.Rows[0].Field<string>("AssetNumber"));
                            Config.GetInstance().UpdateDeviceTable(
                                "1",
                                ipAddress.GetAddressBytes(),
                                modell,
                                serial,
                                asset,
                                ipAddress.GetAddressBytes()
                            );
                            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "SearchForDevices",
                                "SearchForPrinter Found: " + ipAddress.ToString() + " Type: " + ident);
                        }
                        else
                        {
                            var dts = Config.GetInstance().GetOidRowByPrivateId(ident);

                            if (dts.Rows.Count > 0)
                            {
                                modell = SnmpClient.GetOidValue(ipAddress.ToString(),
                                    dts.Rows[0].Field<string>("Model"));
                                serial = SnmpClient.GetOidValue(ipAddress.ToString(),
                                    dts.Rows[0].Field<string>("SerialNumber"));
                                asset = SnmpClient.GetOidValue(ipAddress.ToString(),
                                    dts.Rows[0].Field<string>("AssetNumber"));
                                Config.GetInstance().Query("Delete from Devices where IP = '" +
                                                           ipAddress.ToString() + "'; ");
                                Config.GetInstance().InsertInDeviceTable(
                                    "1",
                                    ipAddress.GetAddressBytes(),
                                    modell,
                                    serial,
                                    asset
                                );
                            }
                        }

                    DataGridDevices.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(LoadGridData));

                    ButtonSearch.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ShowStartButton));
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
            ButtonSearch.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Handles the TextChanged event of the TextBox_ip_rangeStart1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpFirstSegmentOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpFirstSegmentOne.Text.Length != 3 &&
                ((!TextBoxIpFirstSegmentOne.Text.Contains(".") && !TextBoxIpFirstSegmentOne.Text.Contains(" ")) ||
                 TextBoxIpFirstSegmentOne.Text.Length <= 1)) return;
            TextBoxIpFirstSegmentOne.Text = TextBoxIpFirstSegmentOne.Text.Replace(".", "");
            TextBoxIpFirstSegmentOne.Text = TextBoxIpFirstSegmentOne.Text.Replace(" ", "");
            var request = new TraversalRequest(FocusNavigationDirection.Next)
            {
                Wrapped = true
            };
            ((TextBox) sender).MoveFocus(request);
        }

        /// <summary>
        ///     Handles the TextChanged event of the TextBox_ip_rangeStart2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpFirstSegmentTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpFirstSegmentTwo.Text.Length != 3 &&
                ((!TextBoxIpFirstSegmentTwo.Text.Contains(".") && !TextBoxIpFirstSegmentTwo.Text.Contains(" ")) ||
                 TextBoxIpFirstSegmentTwo.Text.Length <= 1)) return;
            TextBoxIpFirstSegmentTwo.Text = TextBoxIpFirstSegmentTwo.Text.Replace(".", "");
            TextBoxIpFirstSegmentTwo.Text = TextBoxIpFirstSegmentTwo.Text.Replace(" ", "");
            var request = new TraversalRequest(FocusNavigationDirection.Next)
            {
                Wrapped = true
            };
            ((TextBox) sender).MoveFocus(request);
        }

        /// <summary>
        ///     Handles the TextChanged event of the TextBox_ip_rangeStart3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpFirstSegmentThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpFirstSegmentThree.Text.Length != 3 &&
                ((!TextBoxIpFirstSegmentThree.Text.Contains(".") && !TextBoxIpFirstSegmentThree.Text.Contains(" ")) ||
                 TextBoxIpFirstSegmentThree.Text.Length <= 1)) return;
            TextBoxIpFirstSegmentThree.Text = TextBoxIpFirstSegmentThree.Text.Replace(".", "");
            TextBoxIpFirstSegmentThree.Text = TextBoxIpFirstSegmentThree.Text.Replace(" ", "");
            var request = new TraversalRequest(FocusNavigationDirection.Next)
            {
                Wrapped = true
            };
            ((TextBox) sender).MoveFocus(request);
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpSecondSegmentOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpSecondSegmentOne.Text.Length != 3 &&
                ((!TextBoxIpSecondSegmentOne.Text.Contains(".") && !TextBoxIpSecondSegmentOne.Text.Contains(" ")) ||
                 TextBoxIpSecondSegmentOne.Text.Length <= 1)) return;
            TextBoxIpSecondSegmentOne.Text = TextBoxIpSecondSegmentOne.Text.Replace(".", "");
            TextBoxIpSecondSegmentOne.Text = TextBoxIpSecondSegmentOne.Text.Replace(" ", "");
            var request = new TraversalRequest(FocusNavigationDirection.Next)
            {
                Wrapped = true
            };
            ((TextBox) sender).MoveFocus(request);
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpSecondSegmentTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpSecondSegmentTwo.Text.Length != 3 &&
                ((!TextBoxIpSecondSegmentTwo.Text.Contains(".") && !TextBoxIpSecondSegmentTwo.Text.Contains(" ")) ||
                 TextBoxIpSecondSegmentTwo.Text.Length <= 1)) return;
            TextBoxIpSecondSegmentTwo.Text = TextBoxIpSecondSegmentTwo.Text.Replace(".", "");
            TextBoxIpSecondSegmentTwo.Text = TextBoxIpSecondSegmentTwo.Text.Replace(" ", "");
            var request = new TraversalRequest(FocusNavigationDirection.Next)
            {
                Wrapped = true
            };
            ((TextBox) sender).MoveFocus(request);
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_ip_rangeStop3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpSecondSegmentThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpSecondSegmentThree.Text.Length != 3 &&
                ((!TextBoxIpSecondSegmentThree.Text.Contains(".") && !TextBoxIpSecondSegmentThree.Text.Contains(" ")) ||
                 TextBoxIpSecondSegmentThree.Text.Length <= 1)) return;
            TextBoxIpSecondSegmentThree.Text = TextBoxIpSecondSegmentThree.Text.Replace(".", "");
            TextBoxIpSecondSegmentThree.Text = TextBoxIpSecondSegmentThree.Text.Replace(" ", "");
            var request = new TraversalRequest(FocusNavigationDirection.Next)
            {
                Wrapped = true
            };
            ((TextBox) sender).MoveFocus(request);
        }

        public string AdvisoryText => "Die automatische Suche nach Druckern kann einige Minuten in Anspruch nehmen";
    }
}