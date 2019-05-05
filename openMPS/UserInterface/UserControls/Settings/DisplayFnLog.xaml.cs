// Copyright (c) 2018 / 2019, Andreas Schreiner

using System.Data;
using System.Windows.Controls;
using de.fearvel.net.DataTypes.Interfaces;
using de.fearvel.net.FnLog;

namespace de.fearvel.openMPS.UserInterface.UserControls.Settings
{
    /// <summary>
    /// Interaktionslogik für DisplayFnLog.xaml
    /// </summary>
    public partial class DisplayFnLog : UserControl, IReloadable
    {
        /// <summary>
        /// contains the fnLogs
        /// </summary>
        private DataTable _logs;

        /// <summary>
        /// constructor
        /// </summary>
        public DisplayFnLog()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  retrieves the logs and fills _logs
        /// </summary>
        private void LoadLogs()
        {
            _logs = FnLog.GetInstance().GetLog();
        }

        /// <summary>
        /// displays _logs in the datagrid
        /// </summary>
        private void DisplayLogs()
        {
            DataGridLogs.ItemsSource = _logs.DefaultView;
        }
        
        /// <summary>
        /// reload _logs values
        /// </summary>
        public void Reload()
        {
            LoadLogs();
            DisplayLogs();
        }
    }
}
