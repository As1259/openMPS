#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using de.fearvel.openMPS.SNMP;
using de.fearvel.openMPS.SQLiteConnectionTools;
using de.fearvel.openMPS.Tools;

namespace de.fearvel.openMPS.UC
{
    /// <summary>
    ///     Interaktionslogik für geraeteInfo.xaml
    /// </summary>
    public partial class geraeteInfo : UserControl
    {
        public geraeteInfo()
        {
            InitializeComponent();
            Loaded += geraeteInfo_Load;
        }

        public void geraeteInfo_Load(object sender, RoutedEventArgs e)
        {
            var dt =Config.GetInstance().Query("Select * from Devices");

            for (var i = 0; i < dt.Rows.Count; i++) lv_geraete.Items.Add(dt.Rows[i].Field<string>("IP"));
        }

        private void adjustCounter(object param)
        {
            try
            {
                var o = (object[]) param;
                var ip = (string) o[0];
                var percentage = new double[4];

                if (ScanIP.PingIp(new IPAddress(ScanIP.ConvertStringToAddress(ip))))
                {
                    var cmd = "Select * from OID where OIDPrivateID ='" + DeviceTools.identDevice(ip) + "';";
                    var OIDVAL =Config.GetInstance().Query(cmd);
                    percentage[0] =
                        Convert.ToDouble(SNMPget.GetOidValue(ip, OIDVAL.Rows[0].Field<string>("BlackLevel"))) /
                        Convert.ToDouble(SNMPget.GetOidValue(ip, OIDVAL.Rows[0].Field<string>("BlackLevelMax"))) * 100;
                    percentage[1] =
                        Convert.ToDouble(SNMPget.GetOidValue(ip, OIDVAL.Rows[0].Field<string>("CyanLevel"))) /
                        Convert.ToDouble(SNMPget.GetOidValue(ip, OIDVAL.Rows[0].Field<string>("CyanLevelMax"))) * 100;
                    percentage[2] =
                        Convert.ToDouble(SNMPget.GetOidValue(ip, OIDVAL.Rows[0].Field<string>("MagentaLevel"))) /
                        Convert.ToDouble(SNMPget.GetOidValue(ip, OIDVAL.Rows[0].Field<string>("MagentaLevelMax"))) *
                        100;
                    percentage[3] =
                        Convert.ToDouble(SNMPget.GetOidValue(ip, OIDVAL.Rows[0].Field<string>("YellowLevel"))) /
                        Convert.ToDouble(SNMPget.GetOidValue(ip, OIDVAL.Rows[0].Field<string>("YellowLevelMax"))) * 100;
                }

                lb_percentageBlack.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(resetPercentages));
                for (var i = 0; i < 100; i++)
                {
                    if (i < percentage[0])
                        lb_percentageBlack.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(raisePercentageBlack));
                    if (i < percentage[1])
                        lb_percentageCyan.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(raisePercentageCyan));
                    if (i < percentage[2])
                        lb_percentageMagenta.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(raisePercentageMagenta));
                    if (i < percentage[3])
                        lb_percentageYellow.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(raisePercentageYellow));
                }
            }
            catch (Exception)
            {
            }
        }

        private void raisePercentageBlack()
        {
            progressBlack.Percentage += 1;
            lb_percentageBlack.Content = (int) progressBlack.Percentage + " %";
        }

        private void raisePercentageCyan()
        {
            progressCyan.Percentage += 1;
            lb_percentageCyan.Content = (int) progressCyan.Percentage + " %";
        }

        private void raisePercentageMagenta()
        {
            progressMagenta.Percentage += 1;
            lb_percentageMagenta.Content = (int) progressMagenta.Percentage + " %";
        }

        private void raisePercentageYellow()
        {
            progressYellow.Percentage += 1;
            lb_percentageYellow.Content = (int) progressYellow.Percentage + " %";
        }

        private void resetPercentages()
        {
            lb_percentageBlack.Content = "0 %";
            progressBlack.Percentage = 0;
            lb_percentageCyan.Content = "0 %";
            progressCyan.Percentage = 0;
            lb_percentageMagenta.Content = "0 %";
            progressMagenta.Percentage = 0;
            lb_percentageYellow.Content = "0 %";
            progressYellow.Percentage = 0;
        }

        private void lv_geraete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var thread = new Thread(adjustCounter);
                thread.Start(new object[] {lv_geraete.SelectedValue.ToString()});
            }
            catch (Exception)
            {
            }

            // ThreadPool.QueueUserWorkItem(new WaitCallback(adjustCounter));
        }

        //private void lv_geraete_Selected(object sender, RoutedEventArgs e)
        //{
        //    Thread thread = new Thread(adjustCounter);
        //    thread.Start(new object[] { lv_geraete.SelectedValue.ToString() });
        //    ThreadPool.QueueUserWorkItem(new WaitCallback(adjustCounter));
        //}
    }
}