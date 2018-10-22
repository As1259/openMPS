#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using de.fearvel.openMPS.Database;

namespace de.fearvel.openMPS.UserInterface.UserControls.Settings
{
    /// <summary>
    ///     Interaktionslogik für EinstellungenOID.xaml
    /// </summary>
    public partial class EinstellungenOid : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EinstellungenOid" /> class.
        /// </summary>
        public EinstellungenOid()
        {
            InitializeComponent();
            Loaded += einstellungenOID_Load;
        }

        /// <summary>
        ///     Handles the Load event of the einstellungenOID control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void einstellungenOID_Load(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(loadUIElements);
        }

        /// <summary>
        ///     Loads the UI elements.
        /// </summary>
        /// <param name="state">The state.</param>
        private void loadUIElements(object state)
        {
            dg_data.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(loadDataGridData));
            OIDV.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(loadOIDV));
        }

        /// <summary>
        ///     Loads the data grid source.
        /// </summary>
        private void loadDataGridData()
        {
            dg_data.ItemsSource =Oid.GetInstance().Query("select * from OID").DefaultView;
        }

        /// <summary>
        ///     Loads the oidv.
        /// </summary>
        private void loadOIDV()
        {
            OIDV.Content = $"{Oid.GetInstance().Directory["OID-Version"]:N2}";
        }
    }
}