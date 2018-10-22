#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using de.fearvel.manastone.serialManagement;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.SNMP;
using de.fearvel.openMPS.Tools;

namespace de.fearvel.openMPS.UserInterface.UserControls
{
    /// <summary>
    ///     Interaktionslogik für abraegeStarten.xaml
    /// </summary>
    public partial class RetrieveDeviceInformation : UserControl
    {
        /// <summary>
        ///     The DataTable
        /// </summary>
        private DataTable dt;

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
            bt_client.IsEnabled = false;
        }

        /// <summary>
        ///     Handles the Click event of the bt_start control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void Bt_start_Click(object sender, RoutedEventArgs e)
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
            geraeteGrid.ItemsSource = dt.DefaultView;
            geraeteGrid.Columns[0].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[7].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[8].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[10].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[12].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[13].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[14].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[15].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[16].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[17].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[18].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[19].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[25].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[26].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[27].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[28].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[29].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[30].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[31].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[32].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[33].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[34].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[35].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[39].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[40].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[41].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[46].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[47].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[48].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[49].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[50].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[51].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[52].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[53].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[54].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[55].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[58].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[59].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[60].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[61].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[62].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[63].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[64].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[65].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[66].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[69].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[72].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[73].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[74].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[78].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[79].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[80].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[81].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[82].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[83].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[84].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[85].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[86].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[87].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[88].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[89].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[90].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[91].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[92].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[93].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[94].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[95].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[96].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[97].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[98].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[99].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[104].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[105].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[106].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[107].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[108].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[109].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[110].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[111].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[112].Visibility = Visibility.Hidden;
            geraeteGrid.Columns[113].Visibility = Visibility.Hidden;
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

        /// <summary>
        ///     Gains the data.
        /// </summary>
        /// <returns></returns>
        private DataTable gainData()
        {
            DataTable dt;
            Collector.shell("Delete from Collector;");
            dt =Config.GetInstance().Query("select *  from Devices where aktiv='1' or aktiv='True'");

            for (var i = 0; i < dt.Rows.Count; i++)
                if (DeviceTools.identDevice(dt.Rows[i].Field<string>("ip")).Length > 0)
                    if (ScanIP.PingIp(new IPAddress(ScanIP.ConvertStringToAddress(dt.Rows[i].Field<string>("ip")))))
                        SNMPget.ReadDeviceOiDs(dt.Rows[i].Field<string>("ip"),
                            DeviceTools.identDevice(dt.Rows[i].Field<string>("ip")));
            return dt;
        }

        /// <summary>
        ///     Updates the data grid.
        /// </summary>
        /// <param name="state">The state.</param>
        private void UpdateDataGrid(object state)
        {
            if (SerialManager.CheckLicence(MainWindow.Programid))
            {
                var dt = gainData();
                dt = Collector.shellDT("Select * from Collector");
                this.dt = dt;
                geraeteGrid.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(updateGrid));
                Collector.shellDT("update INFO set Erfassungsdatum='" + DateTime.Now.ToString("yyyy-MM-dd") + "';");
                Collector.shellDT("update INFO set Kundennummer='" +
                                  SerialManager.GetSerialContainer(MainWindow.Programid).CustomerIdentificationNumber +
                                  "';");

                bt_client.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(bt_clientEnable));
                bt_senden.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(bt_sendenEnable));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(GetNormalView));
            }
            else
            {
                MessageBox.Show("Seriennummer Abgelaufen!!", "ERROR LICD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///     Handles the Click event of the bt_senden control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_senden_Click(object sender, RoutedEventArgs e)
        {
            progress.Value = 0;
            progress.Visibility = Visibility.Visible;
            ThreadPool.QueueUserWorkItem(smtpSend);
            ThreadPool.QueueUserWorkItem(adaptProgressLoad);
        }

        /// <summary>
        ///     SMTPs the send.
        /// </summary>
        /// <param name="state">The state.</param>
        private void smtpSend(object state)
        {
            try
            {
                Collector.shellDT("update INFO set Uebertragungsweg='smtpSend';");
                var filename = prepareOpenMPSFile();
                var mail = new MailMessage();
                var SmtpServer = new SmtpClient(Config.GetInstance().Directory["cred_connection_smtp_server"]);
                mail.From = new MailAddress(Config.GetInstance().Directory["cred_connection_smtp_sendaddress"]);
                mail.To.Add(Config.GetInstance().Directory["cred_connection_smtp_receiveaddress"]);
                mail.Subject = "MPS2018 Gerätedaten";
                mail.Body = "MPS2018 Gerätedaten";
                mail.Attachments.Add(new Attachment(filename));

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential(
                    Config.GetInstance().Directory["cred_connection_smtp_user"],
                    Config.GetInstance().Directory["cred_connection_smtp_password"]);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show("E-Mail wurde versand", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                bt_client.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(bt_clientDisable));
                bt_senden.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(bt_sendenDisable));
                progress.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(GetNormalView));
                var fInfo = new FileInfo(filename)
                {
                    IsReadOnly = false
                };
            }
            catch (Exception)
            {
                MessageBox.Show("Fehler bei Übertragung, bitte verwenden sie eine andere Übertragungsart", "ERROR",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string prepareOpenMPSFile()
        {
            Collector.shellDT("update INFO set version=" +Config.GetInstance().Query(
                                  "Select version from INFO").Rows[0].Field<long>("version") + ";");
            Collector.shellDT("update INFO set OIDversion=" +Config.GetInstance().Query(
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
        ///     Handles the Click event of the bt_client control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_client_Click(object sender, RoutedEventArgs e)
        {
            Collector.shellDT("update INFO set Uebertragungsweg='customerEmail';");
            var filename = prepareOpenMPSFile();
            var emailHead =
                "MPS2018 Gerätedaten";
            var emailProsaBody =
                "Sehr geehrter Kunde,\n\nim Rahmen der Zählerstandsmeldung wurden die Daten in einer oMPS-Datei „Gerätedaten.oMPS“ erfasst und" +
                " dieser Mail angefügt.\nSenden Sie diese Mail bitte an die vorgegebene Adresse, damit wir die Daten weiter verarbeiten können.\n\nFalls Ihr Mailclient" +
                " das automatische Anhängen der oMPS-Datei nicht zulässt, bzw. verhindert hat, fügen Sie die Datei bitte manuell aus folgenden Ordner in diese Mail ein:\n"
                + Directory.GetCurrentDirectory() + "\\" + filename;
            var mail = new MAPI();
            mail.AddAttachment(filename);
            mail.AddRecipientTo(Config.GetInstance().Directory["cred_connection_smtp_sendaddress"]);
            try
            {
                mail.SendMailPopup(emailHead, emailProsaBody);
            }
            catch (Exception)
            {
                MessageBox.Show("Ihr E-Mail client wird nicht unterstützt\nBitte wählen sie eine andere Option",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            var fInfo = new FileInfo(filename)
            {
                IsReadOnly = false
            };
            bt_senden.IsEnabled = false;
            bt_client.IsEnabled = false;
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
        ///     Bts the client enable.
        /// </summary>
        private void bt_clientEnable()
        {
            bt_client.IsEnabled = true;
        }


        /// <summary>
        ///     Bts the senden disable.
        /// </summary>
        private void bt_sendenDisable()
        {
            bt_senden.IsEnabled = false;
        }

        /// <summary>
        ///     Bts the client disable.
        /// </summary>
        private void bt_clientDisable()
        {
            bt_client.IsEnabled = false;
        }

        #endregion
    }
}