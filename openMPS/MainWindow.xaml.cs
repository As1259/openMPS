#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.Database.Exceptions;
using de.fearvel.openMPS.UC;
using de.fearvel.openMPS.UC.Einstellungen;
using Fluent;

namespace de.fearvel.openMPS
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        ///     The configname
        /// </summary>
        public const string Configname = @"core\oMPS.oConfig";

        /// <summary>
        ///     The erfassungname
        /// </summary>
        public const string Erfassungname = @"core\oMPS.oData";

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
            InitializeComponent();
            Loaded += RibbonWindow_Load;
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
                LoadUserControls();
                OpenSuchen();
                LoadDatabases();
                var a = new openRegistration();
                a.ShowDialog();
            }
            catch (MPSSQLiteException)
            {
                MessageBox.Show("Fehler!!\nKonfigurationsdatei nicht gefunden!!\nFehlercode. 55534552\n",
                    "!!!Kritischer Fehler!!!\n"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void LoadUserControls()
        {
            _userControls = new Dictionary<Type, UserControl>();
            Dispatcher.Invoke(new ThreadStart(LoadDelayableUserControl));
            Dispatcher.Invoke(new ThreadStart(LoadBackstageUserControls));

        }

        public void LoadBackstageUserControls()
        {
            _userControls.Add(typeof(start), new start());
            _userControls.Add(typeof(EinstellungenMainV2), new EinstellungenMainV2());
            grid_help.Children.Add(_userControls[typeof(start)]);
            grid_settings.Children.Add(_userControls[typeof(EinstellungenMainV2)]);
        }
        private void LoadDelayableUserControl()
        {
            _userControls.Add(typeof(GeraeteSuchen), new GeraeteSuchen());

            _userControls.Add(typeof(geraeteBearbeiten), new geraeteBearbeiten());
            _userControls.Add(typeof(AbraegeStarten), new AbraegeStarten());

        }

        private void LoadDatabases()
        {
            Config.GetInstance().Open();
            OID.GetInstance().Open();

        }

        /// <summary>
        ///     Handles the backstageExit event of the bt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void Bt_backstageExit(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void DisplayUserControl(UserControl uc, string text = "")
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(uc);
            tbl_info.Text = text;
        }
        private void OpenSuchen()
        {
            DisplayUserControl(_userControls[typeof(GeraeteSuchen)],
                "Die automatische Suche nach Druckern wird im Schnitt ca. 0.03 Sekunden pro IP-Adresse benötigen,");

        }
        private void OpenBearbeiten()
        {
            DisplayUserControl(_userControls[typeof(geraeteBearbeiten)],
                "Hier können Sie neue Geräte hinzufügen, oder die IP-Adressen bereits erfasster Geräte anpassen." +
                " Über die Kennzeichnung „Aktiv“ können Sie entscheiden, ob zu einem Gerät Werte abgefragt und übermittelt " +
                "werden, oder nicht.");
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

        private void OpenAbfrage()
        {
            DisplayUserControl(_userControls[typeof(AbraegeStarten)]);
        }
        /// <summary>
        ///     Loads the abfrage starten.
        /// </summary>
        private void loadAbfrageStarten()
        {
            if (!MainGrid.CheckAccess())
            {
                MainGrid.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(loadAbfrageStarten));
                return;
            }

            MainGrid.Children.Add(new AbraegeStarten());
        }




        private void bsi_close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
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


    }
}