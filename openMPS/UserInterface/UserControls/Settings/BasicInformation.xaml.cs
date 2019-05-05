// Copyright (c) 2018 / 2019, Andreas Schreiner

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using de.fearvel.net.DataTypes.Interfaces;
using de.fearvel.net.FnLog;
using de.fearvel.net.Manastone;
using de.fearvel.openMPS.Database;

namespace de.fearvel.openMPS.UserInterface.UserControls.Settings
{
    /// <summary>
    /// Interaktionslogik für BasicInformation.xaml
    /// </summary>
    public partial class BasicInformation : UserControl, IReloadable
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicInformation" /> class.
        /// </summary>
        public BasicInformation()
        {
            InitializeComponent();
            Loaded += Information_Load;
        }

        /// <summary>
        /// Handles the Load event of the BasicInformation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void Information_Load(object sender, RoutedEventArgs e)
        {
            LoadFields();
        }

        /// <summary>
        /// Fills the labels with content
        /// </summary>
        private void LoadFields()
        {
            LabelCustomerReference.Content = ManastoneClient.GetInstance().CustomerReference;
            LabelConfigVersion.Content = $"{Config.GetInstance().Directory["MPSVersion"]}";
            LabelProgramVersion.Content = FileVersionInfo
                .GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
            LabelOidVersion.Content = $"{Config.GetInstance().Directory["OidVersion"]}";
            LabelUuid.Content = $"{Config.GetInstance().Directory["UUID"]}";
            LabelFnLogVersion.Content = FnLog.FnLogClientVersion;
            LabelManastoneVersion.Content = ManastoneClient.ClientVersion;

        }

        /// <summary>
        /// reloads the displayed values 
        /// </summary>
        public void Reload()
        {
            Config.GetInstance().ReloadDirectory();
            LoadFields();
        }
    }
}