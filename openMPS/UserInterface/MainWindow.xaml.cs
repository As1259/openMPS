// Copyright (c) 2018 / 2019, Andreas Schreiner

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
using de.fearvel.openMPS.Interfaces;
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
            InitClientConnection();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "INIT COMPLETE");
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "InitializeComponent Started");

            InitializeComponent();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "InitializeComponent COMPLETE");
            MetroWindowMain.Title +=
                " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "Title updated");
            Loaded += RibbonWindow_Load;
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "RibbonWindow_Load complete");

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

        /// <summary>
        /// Connects to FnLogServer, OpenMPSServer, ManastoneServer
        /// The license check and the retrieval of the OID Table will happen here
        /// Exits if connection is not possible
        /// </summary>
        private void InitClientConnection()
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
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                    "FnLog INIT Complete");
                OpenMPSClient.SetInstance("https://app.fearvel.de:9040/");
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                    "OpenMPSClient set");
                ManastoneClient.SetInstance("https://app.fearvel.de:9060/", "5d1ae2a2-6ef3-4abd-86b8-905686dc6567");
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "Manastone set");
                OpenMPSClient.GetInstance().CheckForCompatibleVersion();
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                    "Compatible VersionChecked");
                if (!ManastoneClient.GetInstance().CheckActivation())
                {
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                        "Activation process Started");
                    ActivateProgram();
                }

                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                    "Activation Update OID Starting");
            }
            catch (Exception e)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupError, "Error on InitClientConnection of openMPS",
                    e.Message);
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

        /// <summary>
        /// Opens the Activation window
        /// Program will exit if an activation is failed or the activation window is closed
        /// </summary>
        private void ActivateProgram()
        {
            var licCheck = new LicenseDialog();
            licCheck.ShowDialog();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "ActivateProgram Dialog Opened");
            if (!licCheck.Result)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                    "ActivateProgram Result false: " + licCheck.Result);
                FnLog.GetInstance().ProcessLogList();
                Environment.Exit(1);
            }

            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "ActivateProgram Dialog finished successfully");
        }

        /// <summary>
        /// Loads the usercontrols that are not needed to start the program threaded to speed up the program start
        /// </summary>
        private void LoadUserControls()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "LoadUserControls");

            _userControls = new Dictionary<Type, UserControl>();
            Dispatcher.Invoke(new ThreadStart(LoadDelayableUserControl));
            Dispatcher.Invoke(new ThreadStart(LoadBackstageUserControls));
        }

        /// <summary>
        /// Loads the usercontrols for the backstage threaded
        /// </summary>
        public void LoadBackstageUserControls()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "LoadBackstageUserControls");
            _userControls.Add(typeof(InfoPage), new InfoPage());
            _userControls.Add(typeof(Settings), new Settings());
            GridHelp.Children.Add(_userControls[typeof(InfoPage)]);
            GridSettings.Children.Add(_userControls[typeof(Settings)]);
        }

        /// <summary>
        /// Loads the usercontrols for the ribbon buttons threaded
        /// </summary>
        private void LoadDelayableUserControl()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "LoadDelayableUserControl");

            _userControls.Add(typeof(SearchForDevices), new SearchForDevices());
            _userControls.Add(typeof(DeviceManagement), new DeviceManagement());

            _userControls.Add(typeof(EditDevices), new EditDevices());
            _userControls.Add(typeof(RetrieveDeviceInformation), new RetrieveDeviceInformation());
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "LoadDelayableUserControl done");
        }

        /// <summary>
        /// Displays the 
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="text"></param>
        private void DisplayUserControl(UserControl uc, string text = "")
        {
            FnLog.GetInstance()
                .AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT", "DisplayUserControl");

            MainGrid.Children.Clear();
            MainGrid.Children.Add(uc);
            TextBlockInfo.Text = uc is IRibbonAdvisoryText ? ((IRibbonAdvisoryText) uc).AdvisoryText : text;
            FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                "DisplayUserControl done");
            FnLog.GetInstance().ProcessLogList();
        }

        /// <summary>
        /// Oppens the Search UC, which is the standard uc what will be displayed on startup
        /// </summary>
        private void OpenSearchUserControl()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening SearchForDevices");
            DisplayUserControl(_userControls[typeof(SearchForDevices)]);
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
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                    "RibbonWindow_Load Start");

                LoadUserControls();
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                    "RibbonWindow_Load LoadUserControls done");

                OpenSearchUserControl();
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupLog, "openMPS Main Window INIT",
                    "RibbonWindow_Load OpenSearchUserControl done");

                //var a = new openRegistration();
                //a.ShowDialog();
            }
            catch (MPSSQLiteException)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.StartupError, "openMPS Main Window INIT",
                    "RibbonWindow_Load Config File Error");

                MessageBox.Show("Fehler!!\nKonfigurationsdatei Fehlerhaft",
                    "!!!Kritischer Fehler!!!\n"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }


        /// <summary>
        /// Event for IsMinimizedChanged
        /// adjusts the Height
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Button event for ButtonSearch
        /// calls OpenSearchUserControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FluentButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            OpenSearchUserControl();
        }

        /// <summary>
        /// Button event for DeviceManagement
        /// opens EditDevices uc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FluentButtonDeviceManagement_Click(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening EditDevices");
            DisplayUserControl(_userControls[typeof(EditDevices)]);
        }

        /// <summary>
        /// Button event for RetrieveData
        /// opens RetrieveDeviceInformation uc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FluentButtonRetrieveData_Click(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance()
                .AddToLogList(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening RetrieveDeviceInformation");
            DisplayUserControl(_userControls[typeof(RetrieveDeviceInformation)]);
        }

        /// <summary>
        /// Button event for the Exit button which is located in the backstage
        /// exits the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackstageTabItemExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "openMPS Main Window BS", "Exit Click");

            Environment.Exit(0);
        }

        /// <summary>
        /// Adjusts the ribbon title bar
        /// unused but nesecarry (interface implementation)
        /// </summary>
        public RibbonTitleBar TitleBar { get; }


        /// <summary>
        /// Button event for ButtonDeviceManagement
        /// opens DeviceManagement uc
        /// name inconsistent i know.
        /// button is for testing purpose only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeviceManagement_Click(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "ProgramInfo", "opening DeviceManagement");
            DisplayUserControl(_userControls[typeof(DeviceManagement)]);
        }

        /// <summary>
        /// Returns the FileVersion of the assembly
        /// </summary>
        /// <returns></returns>
        private string GetFileVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        /// <summary>
        /// on closing event
        /// processes all queued logs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FnLog.GetInstance().ProcessLogList();
        }
    }
}