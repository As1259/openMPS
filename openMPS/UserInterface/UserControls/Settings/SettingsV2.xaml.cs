#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace de.fearvel.openMPS.UserInterface.UserControls.Settings
{
    /// <summary>
    ///     Interaktionslogik für SettingsV2.xaml
    /// </summary>
    public partial class SettingsV2 : UserControl
    {

        private Dictionary<string, UserControl> Options;
        /// <summary>
        ///     Initializes a new instance of the <see cref="SettingsV2" /> class.
        /// </summary>
        public SettingsV2()
        {
            InitializeComponent();
        }

        private void AddDefaultItems()
        {
            Options.Clear();
            AddItem("BasicInformation", new BasicInformation());
            AddItem("OID Übersicht", new EinstellungenOid());
            //AddItem("Verschlüsslung", new EinstellungenTechniker());
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
            grid_setting.Children.Clear();
            grid_setting.Children.Add(Options[ListBoxEinstellungen.SelectedItem.ToString()]);
        }
        private void ListBoxEinstellungen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSettingsUserControl();
        }
    }
}