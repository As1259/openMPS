﻿#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using de.fearvel.manastone.serialManagement;
using de.fearvel.net.SQL.Connector;
using de.fearvel.openMPS.Database;
using Microsoft.Win32;

namespace de.fearvel.openMPS.UserInterface.UserControls.Settings
{
    /// <summary>
    ///     Interaktionslogik für BasicInformation.xaml
    /// </summary>
    public partial class BasicInformation : UserControl
    {
        private const string ENCKEY = "Ag8K50Fc05I4PW4e8dN257amZzo227Pprs11s2Vv4VPDk7Zney1U648055Ud";

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasicInformation" /> class.
        /// </summary>
        public BasicInformation()
        {
            InitializeComponent();
            Loaded += Informationen_Load;
        }

        /// <summary>
        ///     Handles the Load event of the BasicInformation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void Informationen_Load(object sender, RoutedEventArgs e)
        {
            loadFields();
        }

        public void loadFields()
        {
            // lbl_val_kundennummer.Content = FillNumberStrings(sc.CustomerIdentificationNumber.ToString(), 5);
            lbl_val_DBV.Content = $"{Config.GetInstance().Directory["MPS-Version"]}";
            lbl_val_PV.Content = FileVersionInfo
                .GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
            lbl_val_OIDV.Content = $"{Oid.GetInstance().Directory["OID-Version"]}";
            lbl_val_UUID.Content = $"{Config.GetInstance().Directory["UUID"]}";

        }

        private string FillNumberStrings(string s, int l)
        {
            while (s.Length < l) s = "0" + s;
            return s;
        }

        private void bt_import_bg_Click(object sender, RoutedEventArgs e)
        {
            var dia = new OpenFileDialog
            {
                Filter = "oMPSDD (*.oMPSDD)|*.oMPSDD"
            };
            if ((bool)dia.ShowDialog())
                if (dia.FileName.Contains("oMPSDD"))
                    try
                    {
                        var sqlCon = new SqliteConnector(dia.FileName, ENCKEY);
                        sqlCon.Query("Select * from DEVICES", out DataTable dt);
                        Config.GetInstance().NonQuery("Delete from Devices");


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

                            Config.GetInstance().NonQuery(sqlCMD);
                        }
                    }
                    catch (Exception)
                    {
                    }
                else if (dia.FileName.Contains("conf"))
                    try
                    {
                        var sqlCon = new SqliteConnector(dia.FileName);
                        sqlCon.Query("Select * from bekanntegeraete", out DataTable dt);
                        Config.GetInstance().NonQuery("Delete from Devices");


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

                            Config.GetInstance().NonQuery(sqlCMD);
                        }
                    }
                    catch (Exception)
                    {
                    }

                else
                    MessageBox.Show("Es werden nur oMPSDD und conf Dateien unterstützt", "Auswahl fehler",
                        MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ImportoMpsdd(string filename)
        {
            var sqlCON = new SqliteConnector(filename);
            sqlCON.Query("Select * from bekanntegeraete", out DataTable dt);
            for (var i = 0; i < dt.Rows.Count; i++)
            {
            }
        }

        private void bt_export_bg_Click(object sender, RoutedEventArgs e)
        {
            var dia = new SaveFileDialog
            {
                Filter = "oMPSDD (*.oMPSDD)|*.oMPSDD"
            };
            if (!(bool)dia.ShowDialog())
            {
                return;
            }

            try
            {
                var sqlCon = createPreparedFile(dia.FileName);
                var dt = Config.GetInstance().Query("Select * from DEVICES");
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

                    sqlCon.NonQuery(sqlCMD);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private SqliteConnector createPreparedFile(string filename)
        {
            var sqlCon = new SqliteConnector(filename);
            sqlCon.SetPassword(ENCKEY);

            sqlCon.NonQuery(" CREATE TABLE `DEVICES` ("
                            + "`Aktiv` BOOL NOT NULL DEFAULT 'true', "
                            + "`IP` VARCHAR(39) NOT NULL DEFAULT '', "
                            + "`Modell` varchar(250), "
                            + "`Seriennummer` varchar(250), "
                            + "`AssetNumber` varchar(250) NOT NULL DEFAULT '', "
                            + "`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT); ");
            return sqlCon;
        }

        private void bt_updoid_online_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    CollectionToMysql.open(
            //       oMPSConfig.GetInstance().Query("select * from CREDENTIALS where Name='conn_mysql_oid_server'").Rows[0]
            //            .Field<string>("Value"),
            //       oMPSConfig.GetInstance().Query("select * from CREDENTIALS where Name='conn_mysql_oid_startdb'").Rows[0]
            //            .Field<string>("Value"),
            //       oMPSConfig.GetInstance().Query("select * from CREDENTIALS where Name='conn_mysql_oid_user'").Rows[0]
            //            .Field<string>("Value"),
            //       oMPSConfig.GetInstance().Query("select * from CREDENTIALS where Name='conn_mysql_oid_password'").Rows[0]
            //            .Field<string>("Value"),
            //        Convert.ToInt32(oMPSConfig.GetInstance().Query("select * from CREDENTIALS where Name='conn_mysql_oid_port'")
            //            .Rows[0].Field<string>(
            //                "Value"))); //port            MYSQLConnectionTools.CollectionToMysql.writeToExternMYSQL();
            //    CollectionToMysql.updateOID();
            //    CollectionToMysql.close();
            //    loadFields();
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Es konte keine Verbindung zum Server hergestellt werden", "ERROR", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //}
        }


        private void Bt_deactivate_OnClick(object sender, RoutedEventArgs e)
        {
            SerialManager.DeleteSerialFromStorage(SerialManager.GetSerialContainer(MainWindow.Programid).SerialNumber);
            Environment.Exit(0);
        }

        private void bt_delUserdata_Click(object sender, RoutedEventArgs e)
        {
            Config.GetInstance().NonQuery("Delete from DEVICES");
            Collector.Shell("Delete from Collector");
            Collector.Shell("update INFO set ErfassungsDatum =  null, Kundennummer = '', Uebertragungsweg = ''");
        }

        private void bt_updoid_file_Click(object sender, RoutedEventArgs e)
        {
            var dia = new OpenFileDialog
            {
                Filter = "db (*.db)|*.db"
            };

            try
            {
                if ((bool)dia.ShowDialog())
                {
                    OidUpdateFileHandler.GetInstance().Update(dia.FileName);
                    loadFields();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Datei konnte nicht eingelesen werden", "ERROR", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            loadFields();

        }
    }
}