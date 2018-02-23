#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using de.as1259.manastone.serialManagement;
using de.as1259.openMPS.MYSQLConnectionTools;
using de.as1259.openMPS.SQLiteConnectionTools;
using de.as1259.openMPS.SQLiteConnectionTools.Connector;
using Microsoft.Win32;

namespace de.as1259.openMPS.UC.Einstellungen
{
    /// <summary>
    ///     Interaktionslogik für Informationen.xaml
    /// </summary>
    public partial class Informationen : UserControl
    {
        private const string ENCKEY = "Ag8K50Fc05I4PW4e8dN257amZzo227Pprs11s2Vv4VPDk7Zney1U648055Ud";

        /// <summary>
        ///     Initializes a new instance of the <see cref="Informationen" /> class.
        /// </summary>
        public Informationen()
        {
            InitializeComponent();
            Loaded += Informationen_Load;
        }

        /// <summary>
        ///     Handles the Load event of the Informationen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void Informationen_Load(object sender, RoutedEventArgs e)
        {
            loadFields();
        }

        public void loadFields()
        {
            var dtInfo = CounterConfig.shellDT("Select * from Info");
            var sc = SerialManager.GetSerialContainer(MainWindow.PROGRAMID);
            var serial = sc.SerialNumber;

            lbl_val_kundennummer.Content = FillNumberStrings(sc.CustomerIdentificationNumber.ToString(), 5);
            lbl_val_PV.Content = dtInfo.Rows[0].Field<long>("version").ToString();
            lbl_val_OIDV.Content = dtInfo.Rows[0].Field<long>("OIDVersion").ToString();
        }

        private string FillNumberStrings(string s, int l)
        {
            while (s.Length < l) s = "0" + s;
            return s;
        }

        private void bt_import_bg_Click(object sender, RoutedEventArgs e)
        {
            var dia = new OpenFileDialog();
            dia.Filter = "oMPSDD (*.oMPSDD)|*.oMPSDD";
            if ((bool) dia.ShowDialog())
                if (dia.FileName.Contains("oMPSDD"))
                    try
                    {
                        var sqlCon = new SQLiteConnector(dia.FileName, ENCKEY);
                        var dt = sqlCon.sqlShellDT("Select * from DEVICES");
                        CounterConfig.shell("Delete from Devices");


                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            var aktiv = 0;
                            if (dt.Rows[i].Field<bool>("Aktiv")) aktiv = 1;
                            var sqlCMD = "insert into Devices (AKTIV,IP,Modell,Seriennummer,AssetNumber) values ("
                                         + "'" + aktiv + "',"
                                         + "'" + dt.Rows[i].Field<string>("IP") + "',"
                                         + "'" + dt.Rows[i].Field<string>("Modell") + "',"
                                         + "'" + dt.Rows[i].Field<string>("Seriennummer") + "',"
                                         + "'" + dt.Rows[i].Field<string>("AssetNumber") + "');";

                            CounterConfig.shell(sqlCMD);
                        }
                    }
                    catch (Exception)
                    {
                    }
                else if (dia.FileName.Contains("conf"))
                    try
                    {
                        var sqlCon = new SQLiteConnector(dia.FileName);
                        var dt = sqlCon.sqlShellDT("Select * from bekanntegeraete");
                        CounterConfig.shell("Delete from Devices");


                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            var aktiv = 0;
                            if (dt.Rows[i].Field<bool>("Aktiv")) aktiv = 1;
                            var sqlCMD = "insert into Devices (AKTIV,IP,Modell,Seriennummer,AssetNumber) values ("
                                         + "'" + aktiv + "',"
                                         + "'" + dt.Rows[i].Field<string>("IP") + "',"
                                         + "'" + dt.Rows[i].Field<string>("Modell") + "',"
                                         + "'" + dt.Rows[i].Field<string>("Seriennummer") + "',"
                                         + "'" + dt.Rows[i].Field<string>("AssetNumber") + "');";

                            CounterConfig.shell(sqlCMD);
                        }
                    }
                    catch (Exception)
                    {
                    }

                else
                    MessageBox.Show("Es werden nur oMPSDD und conf Dateien unterstützt", "Auswahl fehler",
                        MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void importoMPSDD(string filename)
        {
            var sqlCON = new SQLiteConnector(filename);
            var dt = sqlCON.sqlShellDT("Select * from bekanntegeraete");
            for (var i = 0; i < dt.Rows.Count; i++)
            {
            }
        }

        private void bt_export_bg_Click(object sender, RoutedEventArgs e)
        {
            var dia = new SaveFileDialog();
            dia.Filter = "oMPSDD (*.oMPSDD)|*.oMPSDD";
            if ((bool) dia.ShowDialog())
                try
                {
                    var sqlCon = createPreparedFile(dia.FileName);
                    var dt = CounterConfig.shellDT("Select * from DEVICES");
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var aktiv = 0;
                        if (dt.Rows[i].Field<bool>("Aktiv")) aktiv = 1;
                        var sqlCMD = "Insert Into DEVICES (Aktiv,IP,Modell,Seriennummer,AssetNumber) values ("
                                     + "'" + aktiv + "',"
                                     + "'" + dt.Rows[i].Field<string>("IP") + "',"
                                     + "'" + dt.Rows[i].Field<string>("Modell") + "',"
                                     + "'" + dt.Rows[i].Field<string>("Seriennummer") + "',"
                                     + "'" + dt.Rows[i].Field<string>("AssetNumber") + "');";

                        sqlCon.sqlShell(sqlCMD);
                    }
                }
                catch (Exception)
                {
                }
        }

        private SQLiteConnector createPreparedFile(string filename)
        {
            var sqlCon = new SQLiteConnector(filename);
            sqlCon.setPassword(ENCKEY);

            sqlCon.sqlShell(" CREATE TABLE `DEVICES` ("
                            + "`Aktiv`	BOOL NOT NULL DEFAULT 'true',                "
                            + "`IP`	VARCHAR(39) NOT NULL DEFAULT '',                 "
                            + "`Modell`	varchar(250),                                "
                            + "`Seriennummer`	varchar(250),                        "
                            + "`AssetNumber`	varchar(250) NOT NULL DEFAULT '',        "
                            + "`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT);      ");
            return sqlCon;
        }

        private void bt_updoid_online_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectionToMysql.open(
                    CounterConfig.shellDT("select * from CREDENTIALS where Name='conn_mysql_oid_server'").Rows[0]
                        .Field<string>("Value"),
                    CounterConfig.shellDT("select * from CREDENTIALS where Name='conn_mysql_oid_startdb'").Rows[0]
                        .Field<string>("Value"),
                    CounterConfig.shellDT("select * from CREDENTIALS where Name='conn_mysql_oid_user'").Rows[0]
                        .Field<string>("Value"),
                    CounterConfig.shellDT("select * from CREDENTIALS where Name='conn_mysql_oid_password'").Rows[0]
                        .Field<string>("Value"),
                    Convert.ToInt32(CounterConfig.shellDT("select * from CREDENTIALS where Name='conn_mysql_oid_port'")
                        .Rows[0].Field<string>(
                            "Value"))); //port            MYSQLConnectionTools.CollectionToMysql.writeToExternMYSQL();
                CollectionToMysql.updateOID();
                CollectionToMysql.close();
                loadFields();
            }
            catch (Exception)
            {
                MessageBox.Show("Es konte keine Verbindung zum Server hergestellt werden", "ERROR", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void bt_updoid_file_Click(object sender, RoutedEventArgs e)
        {
            var dia = new OpenFileDialog();
            dia.Filter = "oOID (*.oOID)|*.oOID";
            if ((bool) dia.ShowDialog())
            {
                try
                {
                    var sqlCon = new SQLiteConnector(dia.FileName, ENCKEY);
                    CollectionToMysql.updateOID(sqlCon);
                }
                catch (Exception)
                {
                    MessageBox.Show("Datei konnte nicht eingelesen werden", "ERROR", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                loadFields();
            }
        }

        private void Bt_deactivate_OnClick(object sender, RoutedEventArgs e)
        {
            SerialManager.DeleteSerialFromStorage(SerialManager.GetSerialContainer(MainWindow.PROGRAMID).SerialNumber);
            Environment.Exit(0);
        }

        private void bt_delUserdata_Click(object sender, RoutedEventArgs e)
        {
            CounterConfig.shell("Delete from DEVICES");
            Collector.shell("Delete from Collector");
            Collector.shell("update INFO set ErfassungsDatum =  null, Kundennummer = '', Uebertragungsweg = ''");
        }
    }
}