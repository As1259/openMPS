#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using de.as1259.openMPS.SQLiteConnectionTools;
using de.as1259.openMPS.UC;
using de.as1259.openMPS.UC.Einstellungen;

namespace de.as1259.openMPS
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        ///     The configname
        /// </summary>
        public const string CONFIGNAME = @"core\oMPS.oConfig";

        /// <summary>
        ///     The erfassungname
        /// </summary>
        public const string ERFASSUNGNAME = @"core\oMPS.oData";

        public static int PROGRAMID = 10001;

        /// <summary>
        ///     The ucontrol
        /// </summary>
        private readonly UserControl[] ucontrol = new UserControl[3];

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
                ucontrol[0] = new geraeteSuchen();
                ucontrol[1] = new geraeteBearbeiten();
                ucontrol[2] = new abraegeStarten();
                openSuchen();
                grid_help.Children.Add(new start());

                grid_settings.Children.Add(new EinstellungenMain());

                CounterConfig.openENC(CONFIGNAME);
                Collector.openENC(ERFASSUNGNAME);
                var a = new openRegistration();
                a.ShowDialog();
            }
            catch (SQLiteZaehlerConfigException)
            {
                MessageBox.Show("Fehler!!\nKonfigurationsdatei nicht gefunden!!\nFehlercode. 55534552\n",
                    "!!!Kritischer Fehler!!!\n"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        /// <summary>
        ///     Handles the backstageExit event of the bt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_backstageExit(object sender, RoutedEventArgs e)
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
            openSuchen();
        }

        private void openSuchen()
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(ucontrol[0]);
            tbl_info.Text = "Die automatische Suche nach Druckern ist sehr komplex und nimmt einige Zeit in Anspruch." +
                            " Im Schnitt ca. 6 Sekunden pro IP-Adresse, die abgeprüft wird. Wenn Sie nur eine kleine Anzahl an Geräten" +
                            " haben ist es für Sie möglicherweise effektiver, die Geräte über den Bereich ‚Geräte bearbeiten‘ manuell anzulegen.";
        }

        private void bt_geraetBearbeiten(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(ucontrol[1]);
            tbl_info.Text =
                "Hier können Sie neue Geräte hinzufügen, oder die IP-Adressen bereits erfasster Geräte anpassen." +
                " Über die Kennzeichnung „Aktiv“ können Sie entscheiden, ob zu einem Gerät Werte abgefragt und übermittelt " +
                "werden, oder nicht.";
        }

        /// <summary>
        ///     Handles the abfrageStarten event of the bt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_abfrageStarten(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(ucontrol[2]);
            tbl_info.Text = "";
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

            MainGrid.Children.Add(new abraegeStarten());
        }


        /// <summary>
        ///     Handles the Click event of the bt_einstellungen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_einstellungen_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(ucontrol[3]);
            tbl_info.Text = "";
        }


        private void bt_verbrauchsmaterialinfo(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new geraeteInfo());
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