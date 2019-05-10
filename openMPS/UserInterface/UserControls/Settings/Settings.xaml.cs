using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using de.fearvel.net.DataTypes.Interfaces;
using de.fearvel.net.FnLog;
using de.fearvel.net.Gui.wpf;
using de.fearvel.openMPS.Database;

namespace de.fearvel.openMPS.UserInterface.UserControls.Settings
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public partial class Settings : UserControl
    {
        /// <summary>
        /// Dictionary containing the loaded user controls
        /// </summary>
        private Dictionary<string, UserControl> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings" /> class.
        /// </summary>
        public Settings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds the default items
        /// </summary>
        private void AddDefaultItems()
        {
            _options.Clear();
            AddItem("BasicInformation", new BasicInformation());
            RestrictableTableEditor.SetInstance(Config.GetInstance().GetConnector());
            // AddItem("FnLog DEV", new DisplayFnLog());
            // AddItem("ManastoneFnLog DEV", new DisplayManastoneFnLog());
        }

        /// <summary>
        /// adds an item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="uc"></param>
        public void AddItem(string key, UserControl uc)
        {
            _options.Add(key, uc);
        }

        /// <summary>
        /// loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _options = new Dictionary<string, UserControl>();
            AddDefaultItems();
            GridSetting.Children.Clear();
            ListBoxSettings.ItemsSource = _options.Keys;
            ListBoxSettings.SelectedIndex = 0;
            LoadSettingsUserControl();
        }

        /// <summary>
        /// Loads the remaining part of the Settings UserControl
        /// </summary>
        private void LoadSettingsUserControl()
        {
            try
            {
                FnLog.GetInstance().ProcessLogList();

                GridSetting.Children.Clear();
                var item = _options[ListBoxSettings.SelectedItem.ToString()];
                GridSetting.Children.Add(item);

                ((IReloadable) item).Reload(); //unclean but works
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// ListBoxSettings SelectionChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSettingsUserControl();
        }
    }
}