#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

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
    ///     Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {

        private Dictionary<string, UserControl> Options;
        /// <summary>
        ///     Initializes a new instance of the <see cref="Settings" /> class.
        /// </summary>
        public Settings()
        {
            InitializeComponent();
        }

        private void AddDefaultItems()
        {
            Options.Clear();
            AddItem("BasicInformation", new BasicInformation());
            RestrictableTableEditor.SetInstance(Config.GetInstance().GetConnector());
          //  AddItem("RTE", new UserControl(){Content = RestrictableTableEditor.GetInstance().TableEditor});
          //  AddItem("RTE-Manager", new UserControl(){Content =  new RestrictableTableEditorManager(Config.GetInstance().GetConnector()) });//FEHLER der null exception bei children.clear auslöst existent

        //    AddItem("LogViewer", new UserControl() { Content = FnLog.GetInstance().GetViewer().FnLogTable });

        AddItem("FnLog DEV", new DisplayFnLog());
        AddItem("ManastoneFnLog DEV", new DisplayManastoneFnLog());

        }

        public void AddItem(string key, UserControl uc)
        {
            Options.Add(key,uc);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Options = new Dictionary<string, UserControl>();
            AddDefaultItems();
            grid_setting.Children.Clear();
            ListBoxEinstellungen.ItemsSource = Options.Keys;
            ListBoxEinstellungen.SelectedIndex = 0;
            LoadSettingsUserControl();
        }

        private void LoadSettingsUserControl()
        {
            try
            {
                grid_setting.Children.Clear();
                var item = Options[ListBoxEinstellungen.SelectedItem.ToString()];
                grid_setting.Children.Add(item);

                    ((IReloadable) item).Reload(); //unclean but works
                
            }
            catch (Exception)
            {

            }
            

        }
        private void ListBoxEinstellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSettingsUserControl();
        }
    }
}