using System;
using System.Data;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using de.fearvel.net.FnLog;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes.Exceptions;
using de.fearvel.openMPS.Interfaces;
using de.fearvel.openMPS.Net;

namespace de.fearvel.openMPS.UserInterface.UserControls
{
    /// <summary>
    /// Interaktionslogik für geraeteBearbeiten.xaml
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public partial class EditDevices : UserControl, IRibbonAdvisoryText
    {
        /// <summary>
        /// The DataRowView
        /// </summary>
        private DataRowView drv;

        /// <summary>
        /// The DataTable
        /// </summary>
        /// <summary>
        /// bool for selected on grid
        /// </summary>
        private bool selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditDevices" /> class.
        /// </summary>
        public EditDevices()
        {
            InitializeComponent();
            Loaded += geraeteSuchen_Load;
        }

        /// <summary>
        /// Handles the Load event of the geraeteSuchen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void geraeteSuchen_Load(object sender, RoutedEventArgs e)
        {
            DataGridDevices.ItemsSource = Config.GetInstance().Devices.DefaultView;
            if (DataGridDevices.Columns.Count >= 7)
            {
                DataGridDevices.Columns[6].Visibility = Visibility.Hidden;
                DataGridDevices.Columns[0].IsReadOnly = true;
                DataGridDevices.Columns[1].IsReadOnly = true;
                DataGridDevices.Columns[2].IsReadOnly = true;
                DataGridDevices.Columns[3].IsReadOnly = true;
                DataGridDevices.Columns[4].IsReadOnly = true;
                DataGridDevices.Columns[5].IsReadOnly = true;
            }
        }

        /// <summary>
        /// Loads the grid data.
        /// </summary>
        public void LoadGridData()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices", "LoadGridData");

            Config.GetInstance().UpdateDevices();
            DataGridDevices.ItemsSource = Config.GetInstance().Devices.DefaultView;
            if (DataGridDevices.Columns.Count >= 7)
            {
                DataGridDevices.Columns[6].Visibility = Visibility.Hidden;
                DataGridDevices.Columns[0].IsReadOnly = true;
                DataGridDevices.Columns[1].IsReadOnly = true;
                DataGridDevices.Columns[2].IsReadOnly = true;
                DataGridDevices.Columns[3].IsReadOnly = true;
                DataGridDevices.Columns[4].IsReadOnly = true;
                DataGridDevices.Columns[5].IsReadOnly = true;
            }

            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices", "LoadGridData Complete");
        }

        /// <summary>
        /// Handles the SelectedCellsChanged event of the DataGridDevices control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Controls.SelectedCellsChangedEventArgs" /> instance containing the event
        /// data.
        /// </param>
        private void geraeteGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                drv = (DataRowView) DataGridDevices.SelectedItem;
                var ipAddress = IPAddress.Parse(drv["IP"].ToString()).GetAddressBytes();
                unlockElements();
                ButtonDeleteEntry.IsEnabled = true;
                TextBoxIpSegmentOne.Text = ipAddress[0].ToString();
                TextBoxIpSegmentTwo.Text = ipAddress[1].ToString();
                TextBoxIpSegmentThree.Text = ipAddress[2].ToString();
                TextBoxIpSegmentFour.Text = ipAddress[3].ToString();
                TextBoxHostName.Text = drv["HostName"].ToString();
                //tb_assetnumber.Text = drv["Assetnumber"].ToString();
                if (drv["Active"].ToString().Contains("True"))
                    CheckBoxActive.IsChecked = true;
                else
                    CheckBoxActive.IsChecked = false;

                selected = true;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Locks the elements.
        /// </summary>
        private void lockElements()
        {
            ButtonSaveEntry.IsEnabled = false;
            TextBoxIpSegmentOne.IsEnabled = false;
            TextBoxIpSegmentTwo.IsEnabled = false;
            TextBoxIpSegmentThree.IsEnabled = false;
            TextBoxIpSegmentFour.IsEnabled = false;
            TextBoxHostName.IsEnabled = false;
            CheckBoxActive.IsEnabled = false;
            ButtonDeleteEntry.IsEnabled = false;
        }

        /// <summary>
        /// Unlocks the elements.
        /// </summary>
        private void unlockElements()
        {
            ButtonSaveEntry.IsEnabled = true;
            TextBoxIpSegmentOne.IsEnabled = true;
            TextBoxIpSegmentTwo.IsEnabled = true;
            TextBoxIpSegmentThree.IsEnabled = true;
            TextBoxIpSegmentFour.IsEnabled = true;
            CheckBoxActive.IsEnabled = true;
        }

        /// <summary>
        /// Handles the Click event of the ButtonSaveEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonSaveEntry_Click(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices", "Button Save clicked");

            var ipAddress =
                TextBoxIpSegmentOne.Text + "." +
                TextBoxIpSegmentTwo.Text + "." +
                TextBoxIpSegmentThree.Text + "." +
                TextBoxIpSegmentFour.Text;
            var active = "1";
            try
            {
                if (ipAddress.Length == 3 && !selected)
                {
                    var ipFromHostName = ScanIp.ResolveHostName(TextBoxHostName.Text);
                    ipAddress = ipFromHostName.ToString();
                }
                else
                {
                    IPAddress.Parse(ipAddress);
                }

                if (CheckBoxActive.IsChecked != null && !(bool) CheckBoxActive.IsChecked) active = "0";
                if (selected)
                {
                    if (drv["IP"].ToString().CompareTo(ipAddress) == 0)
                    {
                        Config.GetInstance().NonQuery("update Devices set Active='" + active + "' where IP='" +
                                                      ipAddress +
                                                      "';");
                        LoadGridData();
                        lockElements();
                    }
                    else
                    {
                        var thread = new Thread(UpdateDevicesViaThread);
                        thread.Start(new object[] {ipAddress, active, drv["IP"].ToString()});
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
                        thread.Start(new object[] {ipAddress, active});
                    }
                }
            }
            catch (Exception ex)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices",
                    "Button Save Error: " + ex.Message);
                MessageBox.Show("Fehlerhafte Eingabe der IP", "IPEingabeError", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Updates the devices via thread.
        /// </summary>
        /// <param name="param">The parameter.</param>
        private void UpdateDevicesViaThread(object param)
        {
            var obj = (object[]) param;
            var ipAddress = (string) obj[0];
            var aktiv = (string) obj[1];
            var altIP = (string) obj[2];
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices",
                "UpdateDevicesViaThread " + altIP + " -> " + ipAddress);

            try
            {
                var ident = DeviceTools.IdentDevice(ipAddress);
                var modell = "";
                var serial = "";
                var asset = "";

                var ip = IPAddress.Parse(ipAddress);
                ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));

                if (ident.Length > 0)
                {
                    var dt = Config.GetInstance().GetOidRowByPrivateId(ident);
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    modell = SnmpClient.GetOidValue(ipAddress, dt.Rows[0].Field<string>("Model"));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    serial = SnmpClient.GetOidValue(ipAddress, dt.Rows[0].Field<string>("SerialNumber"));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    asset = SnmpClient.GetOidValue(ipAddress, dt.Rows[0].Field<string>("AssetNumber"));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                }
                else
                {
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                }

                var ipAlt = IPAddress.Parse(altIP);
                Config.GetInstance().UpdateDeviceTable(
                    aktiv,
                    ip.GetAddressBytes(),
                    modell,
                    serial,
                    asset,
                    ipAlt.GetAddressBytes()
                );
            }
            catch (SnmpIdentNotFoundException)
            {
            }

            DataGridDevices.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(LoadGridData));
        }

        /// <summary>
        /// Inserts the in devices via thread.
        /// </summary>
        /// <param name="param">The parameter.</param>
        private void InsertInDevicesViaThread(object param)
        {
            var obj = (object[]) param;
            var ipAddress = (string) obj[0];
            var aktiv = (string) obj[1];
            try
            {
                var ident = DeviceTools.IdentDevice(ipAddress);
                var modell = "";
                var serial = "";
                var asset = "";
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices",
                    "InsertDevicesViaThread " + ipAddress);

                var ip = IPAddress.Parse(ipAddress);
                ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(adjustProgress));

                if (ident.Length > 0)
                {
                    var dt = Config.GetInstance().GetOidRowByPrivateId(ident);
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    modell = SnmpClient.GetOidValue(ipAddress, dt.Rows[0].Field<string>("Model"));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    serial = SnmpClient.GetOidValue(ipAddress, dt.Rows[0].Field<string>("SerialNumber"));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    asset = SnmpClient.GetOidValue(ipAddress, dt.Rows[0].Field<string>("AssetNumber"));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                }
                else
                {
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                    ProgressBarProgress.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(adjustProgress));
                }

                Config.GetInstance().InsertInDeviceTable(
                    aktiv,
                    ip.GetAddressBytes(),
                    modell,
                    serial,
                    asset
                );
            }
            catch (SnmpIdentNotFoundException)
            {
            }

            DataGridDevices.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(LoadGridData));
        }

        /// <summary>
        /// Adjusts the ProgressBarSearchProgress.
        /// </summary>
        private void adjustProgress()
        {
            if (ProgressBarProgress.Value >= 80)
            {
                ProgressBarProgress.Visibility = Visibility.Hidden;
            }
            else
            {
                ProgressBarProgress.Visibility = Visibility.Visible;
                ProgressBarProgress.Value += 20;
            }
        }

        /// <summary>
        /// Handles the Click event of the ButtonCreateEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonCreateEntry_Click(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices", "Button Create click ");

            selected = false;
            ButtonSaveEntry.IsEnabled = true;
            unlockElements();
            DataGridDevices.UnselectAll();
            TextBoxHostName.IsEnabled = true;
            TextBoxIpSegmentOne.Clear();
            TextBoxIpSegmentTwo.Clear();
            TextBoxIpSegmentThree.Clear();
            TextBoxIpSegmentFour.Clear();
            TextBoxHostName.Clear();
            CheckBoxActive.IsChecked = true;
        }

        /// <summary>
        /// Handles the switch event of the tb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpSegmentOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpSegmentOne.Text.Length == 3 ||
                (TextBoxIpSegmentOne.Text.Contains(".") || TextBoxIpSegmentOne.Text.Contains(" ")) &&
                TextBoxIpSegmentOne.Text.Length > 1)
            {
                TextBoxIpSegmentOne.Text = TextBoxIpSegmentOne.Text.Replace(".", "");
                TextBoxIpSegmentOne.Text = TextBoxIpSegmentOne.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        /// Handles the switch2 event of the tb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpSegmentTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpSegmentTwo.Text.Length == 3 ||
                (TextBoxIpSegmentTwo.Text.Contains(".") || TextBoxIpSegmentTwo.Text.Contains(" ")) &&
                TextBoxIpSegmentTwo.Text.Length > 1)
            {
                TextBoxIpSegmentTwo.Text = TextBoxIpSegmentTwo.Text.Replace(".", "");
                TextBoxIpSegmentTwo.Text = TextBoxIpSegmentTwo.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        /// Handles the switch3 event of the tb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void TextBoxIpSegmentThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxIpSegmentThree.Text.Length == 3 ||
                (TextBoxIpSegmentThree.Text.Contains(".") || TextBoxIpSegmentThree.Text.Contains(" ")) &&
                TextBoxIpSegmentThree.Text.Length > 1)
            {
                TextBoxIpSegmentThree.Text = TextBoxIpSegmentThree.Text.Replace(".", "");
                TextBoxIpSegmentThree.Text = TextBoxIpSegmentThree.Text.Replace(" ", "");
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox) sender).MoveFocus(request);
            }
        }

        /// <summary>
        /// Handles the Click event of the ButtonDeleteEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonDeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            ButtonDeleteEntry.IsEnabled = false;
            ButtonSaveEntry.IsEnabled = false;
            var ipAddress =
                TextBoxIpSegmentOne.Text + "." +
                TextBoxIpSegmentTwo.Text + "." +
                TextBoxIpSegmentThree.Text + "." +
                TextBoxIpSegmentFour.Text;
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices",
                "Button Delete click " + ipAddress);

            if (selected)
                try
                {
                    Config.GetInstance().NonQuery("delete from Devices where IP='" + ipAddress + "';");
                    LoadGridData();
                }
                catch (Exception)
                {
                }
        }

        /// <summary>
        /// Handles the Click event of the ButtonHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "EditDevices", "Button help click");

            MessageBox.Show("Gerät neu anlegen:\n"
                            + "Klicken Sie auf den Button „Gerät neu anlegen“ und erfassen anschließend"
                            + "die IPv4-Adresse oder wahlweise den Hostname des Gerätes in den dafür vorgesehenen Feldern"
                , "Anleitung", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handles the IsVisibleChanged event of the ProgressBarSearchProgress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event
        /// data.
        /// </param>
        private void ProgressBarProgress_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LabelProgress.Visibility = ProgressBarProgress.Visibility;
        }

        /// <summary>
        /// Handles the ValueChanged event of the ProgressBarSearchProgress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedPropertyChangedEventArgs{System.Double}" /> instance containing the
        /// event data.
        /// </param>
        private void ProgressBarProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LabelProgress.Content = ProgressBarProgress.Value + " %";
        }

        /// <summary>
        /// AdvisoryText displayed in the Ribbon bar
        /// </summary>
        public string AdvisoryText =>
            "Hier können Sie neue Geräte hinzufügen, anpassen oder entfernen." +
            " Über die Kennzeichnung „Aktiv“ können Sie entscheiden, ob zu einem Gerät Werte abgefragt und übermittelt " +
            "werden, oder nicht.";
    }
}