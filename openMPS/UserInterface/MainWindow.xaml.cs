#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using de.fearvel.net.DataTypes;
using de.fearvel.net.DataTypes.Exceptions;
using de.fearvel.net.DataTypes.Exceptions.Manastone;
using de.fearvel.net.FnLog;
using de.fearvel.net.Manastone;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes.Exceptions;
using de.fearvel.openMPS.Net;
using de.fearvel.openMPS.UserInterface.UserControls.Settings;
using de.fearvel.openMPS.UserInterface.UserControls;
using de.fearvel.openMPS.UserInterface.Windows;
using Fluent;

namespace de.fearvel.openMPS.UserInterface
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IRibbonWindow
    {
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
            Init();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "INIT COMPLETE");
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "InitializeComponent Started");

            InitializeComponent();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "InitializeComponent COMPLETE");
            MetroWindowMain.Title +=
                " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "Title updated");
            Loaded += RibbonWindow_Load;
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "RibbonWindow_Load complete");

            //FnLog.SetInstance(new FnLogInitPackage(
            //    "https://log.fearvel.de:9024",
            //    System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
            //    Version.Parse(GetFileVersion()),
            //    FnLog.TelemetryType.LogLocalSendAll,
            //    "fnlog.db", "")
            //);

            //   RetrieveDeviceInformation v = new RetrieveDeviceInformation();
            //   var a = v.GainData();
            //    OpenMPSClient.SendOidData("https://localhost:9051", a);


            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "Program Started");
            FnLog.GetInstance().ProcessLogList();
            OpenMPSClient.GetInstance().UpdateOidTable();

        }

        private void Init()
        {
            try
            {
                Config.GetInstance().Open();
                FnLog.SetInstance(new FnLog.FnLogInitPackage(
                        "https://app.fearvel.de:9020/",
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        Version.Parse(GetFileVersion()),
                        FnLog.TelemetryType.LogLocalSendAll,
                        "", ""), Config.GetInstance().GetConnector()
                );
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "FnLog INIT Complete");
                OpenMPSClient.SetInstance("https://app.fearvel.de:9040/");
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "OpenMPSClient set");
                ManastoneClient.SetInstance("https://app.fearvel.de:9060/", "5d1ae2a2-6ef3-4abd-86b8-905686dc6567");
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "Manastone set");
                OpenMPSClient.GetInstance().CheckForCompatibleVersion();
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "Compatible VersionChecked");
                if (!ManastoneClient.GetInstance().CheckActivation())
                {
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "Activation process Started");
                    ActivateProgram();
                }
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "Activation Update OID Starting");
            }
            catch (Exception e)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupError, "Error on Init of openMPS", e.Message);
                FnLog.GetInstance().ProcessLogList();
                if (e is ManastoneException || e is ResultNullOrNotReceivedException)
                {
                    MessageBox.Show(
                        "Failed to connect to the Server. Check your internet connection and try again Later!");
                    Environment.Exit(0);
                }
                else if (e is SQLiteException)
                {
                    MessageBox.Show(
                        "SQLite Error\n Falls das Produkt noch nicht Aktiviert wurde löschen sie bitte alle *.db dateien\n" +
                        "Falls es bereits aktiviert wurde wenden Sie sich bitte an den Entwickeler");
                    Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("Ein unbekannter Fehler ist aufgetreten!!");
                    Environment.Exit(0);
                }
            }
        }

        private void ActivateProgram()
        {


            var licCheck = new LicenseDialog();
            licCheck.ShowDialog();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "ActivateProgram Dialog Opened");
            if (!licCheck.Result)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "ActivateProgram Result false: " + licCheck.Result);
                FnLog.GetInstance().ProcessLogList();
                Environment.Exit(1);
            }
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "ActivateProgram Dialog finished successfully");
        }


        private void LoadUserControls()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "LoadUserControls");

            _userControls = new Dictionary<Type, UserControl>();
            Dispatcher.Invoke(new ThreadStart(LoadDelayableUserControl));
            Dispatcher.Invoke(new ThreadStart(LoadBackstageUserControls));

        }

        public void LoadBackstageUserControls()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "LoadBackstageUserControls");

            _userControls.Add(typeof(InfoPage), new InfoPage());
            _userControls.Add(typeof(Settings), new Settings());
            GridHelp.Children.Add(_userControls[typeof(InfoPage)]);
            GridSettings.Children.Add(_userControls[typeof(Settings)]);
        }

        private void LoadDelayableUserControl()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "LoadDelayableUserControl");

            _userControls.Add(typeof(SearchForDevices), new SearchForDevices());
            _userControls.Add(typeof(DeviceManagement), new DeviceManagement());

            _userControls.Add(typeof(EditDevices), new EditDevices());
            _userControls.Add(typeof(RetrieveDeviceInformation), new RetrieveDeviceInformation());
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "LoadDelayableUserControl done");

        }


        private void DisplayUserControl(UserControl uc, string text = "")
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "DisplayUserControl");

            MainGrid.Children.Clear();
            MainGrid.Children.Add(uc);
            TextBlockInfo.Text = text;
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "DisplayUserControl done");
            FnLog.GetInstance().ProcessLogList();

        }

        private void OpenSuchen()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening SearchForDevices");
            DisplayUserControl(_userControls[typeof(SearchForDevices)],
                "Die automatische Suche nach Druckern kann einige Minuten in Anspruch nehmen");
        }

        private void OpenBearbeiten()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening EditDevices");
            DisplayUserControl(_userControls[typeof(EditDevices)],
                "Hier können Sie neue Geräte hinzufügen, oder die IP-Adressen bereits erfasster Geräte anpassen." +
                " Über die Kennzeichnung „Aktiv“ können Sie entscheiden, ob zu einem Gerät Werte abgefragt und übermittelt " +
                "werden, oder nicht.");
        }

        private void OpenDeviceManagement()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening DeviceManagement");
            DisplayUserControl(_userControls[typeof(DeviceManagement)],
                "Hier können Sie neue Geräte suchen, bearbeiten, oder die IP-Adressen neuer Geräte manuell hinzufügen." +
                " Über die Kennzeichnung „Aktiv“ können Sie entscheiden, ob zu einem Gerät Werte abgefragt und übermittelt " +
                "werden, oder nicht.");
        }

        private void OpenAbfrage()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening RetrieveDeviceInformation");
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
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "RibbonWindow_Load Start");

                LoadUserControls();
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "RibbonWindow_Load LoadUserControls done");

                OpenSuchen();
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "RibbonWindow_Load OpenSuchen done");

                //var a = new openRegistration();
                //a.ShowDialog();
            }
            catch (MPSSQLiteException)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupError, "openMPS Main Window INIT", "RibbonWindow_Load Config File Error");

                MessageBox.Show("Fehler!!\nKonfigurationsdatei Fehlerhaft",
                    "!!!Kritischer Fehler!!!\n"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void RibbonControl_IsMinimizedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (RibbonControl.IsMinimized)
            {
                GridAdditionalThings.Visibility = Visibility.Hidden;
                Height = Height - 95;
            }
            else
            {
                GridAdditionalThings.Visibility = Visibility.Visible;
                Height = Height + 95;
            }
        }

        /// <summary>
        ///     Handles the geraetSuchen event of the bt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void FluentButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            OpenSuchen();
        }

        private void FluentButtonDeviceManagement_Click(object sender, RoutedEventArgs e)
        {
            OpenBearbeiten();
        }

        /// <summary>
        ///     Handles the abfrageStarten event of the bt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void FluentButtonRetrieveData_Click(object sender, RoutedEventArgs e)
        {
            OpenAbfrage();
        }

        private void BackstageTabItemExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "openMPS Main Window BS", "Exit Click");

            Environment.Exit(0);
        }

        public RibbonTitleBar TitleBar { get; }

        private void ButtonDeviceManagement_Click(object sender, RoutedEventArgs e)
        {
            OpenDeviceManagement();
        }

        private string GetFileVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        private void MetroWindowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FnLog.GetInstance().ProcessLogList();
        }
    }
}