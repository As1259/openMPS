#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using de.fearvel.net.DataTypes;
using de.fearvel.net.FnLog;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.Database.Exceptions;
using de.fearvel.openMPS.UserInterface.UserControls.Settings;
using de.fearvel.openMPS.UserInterface.UserControls;
using Fluent;
using Oid = de.fearvel.openMPS.Database.Oid;

namespace de.fearvel.openMPS.UserInterface
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IRibbonWindow
    {
        private Thread _dbConnectionLoader;

        public static int Programid = 10001;

        /// <summary>
        ///     The ucontrol
        /// </summary>

        private Dictionary<Type, UserControl> _userControls;
        /// <summary>
        ///     Initializes a new instance of the <see cref="RibbonWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            _dbConnectionLoader = new Thread(new ThreadStart(LoadDatabases));
            _dbConnectionLoader.Start();
            InitializeComponent();
            Loaded += RibbonWindow_Load;
            //FnLog.SetInstance(new FnLogInitPackage(
            //    "https://log.fearvel.de:9024",
            //    System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
            //    Version.Parse(GetFileVersion()),
            //    FnLog.TelemetryType.LogLocalSendAll,
            //    "fnlog.db", "")
            //);
            FnLog.SetInstance(new FnLog.FnLogInitPackage(
                "https://localhost:9024",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                Version.Parse(GetFileVersion()),
                FnLog.TelemetryType.LogLocalSendAll,
                "fnlog.db", "")
            );
            FnLog.GetInstance().Log(FnLog.LogType.RuntimeInfo, "ProgramInfo", "Program Started");
        }

        private void LoadUserControls()
        {
            _userControls = new Dictionary<Type, UserControl>();
            Dispatcher.Invoke(new ThreadStart(LoadDelayableUserControl));
            Dispatcher.Invoke(new ThreadStart(LoadBackstageUserControls));
        }

        public void LoadBackstageUserControls()
        {
            _userControls.Add(typeof(InfoPage), new InfoPage());
            _userControls.Add(typeof(SettingsV2), new SettingsV2());
            grid_help.Children.Add(_userControls[typeof(InfoPage)]);
            grid_settings.Children.Add(_userControls[typeof(SettingsV2)]);
        }
        private void LoadDelayableUserControl()
        {
            _userControls.Add(typeof(SearchForDevices), new SearchForDevices());
            _userControls.Add(typeof(DeviceManagement), new DeviceManagement());

            _userControls.Add(typeof(EditDevices), new EditDevices());
            _userControls.Add(typeof(RetrieveDeviceInformation), new RetrieveDeviceInformation());

        }

        private void LoadDatabases()
        {
            Config.GetInstance().Open();
            Oid.GetInstance().Open();
        }

        private void DisplayUserControl(UserControl uc, string text = "")
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(uc);
            tbl_info.Text = text;
        }

        private void OpenSuchen()
        {
            FnLog.GetInstance().Log(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening SearchForDevices");
            DisplayUserControl(_userControls[typeof(SearchForDevices)],
                "Die automatische Suche nach Druckern wird im Schnitt ca. 0.03 Sekunden pro IP-Adresse benötigen,");
        }

        private void OpenBearbeiten()
        {
            FnLog.GetInstance().Log(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening EditDevices");
            DisplayUserControl(_userControls[typeof(EditDevices)],
                "Hier können Sie neue Geräte hinzufügen, oder die IP-Adressen bereits erfasster Geräte anpassen." +
                " Über die Kennzeichnung „Aktiv“ können Sie entscheiden, ob zu einem Gerät Werte abgefragt und übermittelt " +
                "werden, oder nicht.");
        }
        private void OpenDeviceManagement()
        {
            FnLog.GetInstance().Log(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening DeviceManagement");
            DisplayUserControl(_userControls[typeof(DeviceManagement)],
                "Hier können Sie neue Geräte suchen, bearbeiten, oder die IP-Adressen neuer Geräte manuell hinzufügen." +
                " Über die Kennzeichnung „Aktiv“ können Sie entscheiden, ob zu einem Gerät Werte abgefragt und übermittelt " +
                "werden, oder nicht.");
        }

        private void OpenAbfrage()
        {
            FnLog.GetInstance().Log(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening RetrieveDeviceInformation");
            DisplayUserControl(_userControls[typeof(RetrieveDeviceInformation)]);
        }

        /// <summary>
        ///     Handles the Load event of the RibbonWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void RibbonWindow_Load(object sender, RoutedEventArgs e)
        {
            //zurueck.IsEnabled = false;
            try
            {
                while (_dbConnectionLoader.IsAlive) { }           // Überprüfung auf vollständiges Laden/ Erzeugen der Internen Datenbank  
                LoadUserControls();
                OpenSuchen();
                //var a = new openRegistration();
                //a.ShowDialog();
            }
            catch (MPSSQLiteException)
            {
                MessageBox.Show("Fehler!!\nKonfigurationsdatei Fehlerhaft",
                    "!!!Kritischer Fehler!!!\n"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void Ribbon_control_IsMinimizedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Ribbon_control.IsMinimized)
            {
                grid_additionalThings.Visibility = Visibility.Hidden;
                Height = Height - 95;
            }
            else
            {
                grid_additionalThings.Visibility = Visibility.Visible;
                Height = Height + 95;
            }
        }

        /// <summary>
        ///     Handles the geraetSuchen event of the bt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_geraetSuchen(object sender, RoutedEventArgs e)
        {
            OpenSuchen();
        }

        private void bt_geraetBearbeiten(object sender, RoutedEventArgs e)
        {
            OpenBearbeiten();
        }
        /// <summary>
        ///     Handles the abfrageStarten event of the bt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_abfrageStarten(object sender, RoutedEventArgs e)
        {
            OpenAbfrage();
        }

        private void bsi_close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        public RibbonTitleBar TitleBar { get; }

        private void ButtonDeviceManagement_OnClick(object sender, RoutedEventArgs e)
        {
            OpenDeviceManagement();

        }
        private string GetFileVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}