using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using de.fearvel.net.DataTypes.Interfaces;
using de.fearvel.net.FnLog;

namespace de.fearvel.openMPS.UserInterface.UserControls.Settings
{
    /// <summary>
    /// Interaktionslogik für DisplayFnLog.xaml
    /// </summary>
    public partial class DisplayFnLog : UserControl, IReloadable
    {

        private DataTable _logs;


        public DisplayFnLog()
        {
            InitializeComponent();
        }

        private void LoadLogs()
        {
            _logs = FnLog.GetInstance().GetLog();
        }

        private void DisplayLogs()
        {
            DataGridLogs.ItemsSource = _logs.DefaultView;
        }
        

        public void Reload()
        {
            LoadLogs();
            DisplayLogs();
        }
    }
}
