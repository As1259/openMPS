using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.UC.CustomControls;

namespace de.fearvel.openMPS.UC
{
    /// <summary>
    /// Interaktionslogik für DeviceManagement.xaml
    /// </summary>
    public partial class DeviceManagement : UserControl
    {
        public DeviceManagement()
        {
            InitializeComponent();
        }

        private void progress_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStop3_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStop2_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStop1_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStart3_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStart2_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_ip_rangeStart1_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_Suchen_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            throw new NotImplementedException();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            FillListView();
        }

        private void FillListView()
        {
            foreach (DataRow row in Config.GetInstance().Devices.Rows)
            {
                ListViewDevices.Items.Add(
                    new DeviceProps(row.Field<long>("id"),
                        row.Field<bool>("Aktiv"),
                        IPAddress.Parse( row.Field<string>("IP")),
                        row.Field<string>("Modell"),
                        row.Field<string>("Seriennummer"),
                        row.Field<string>("AssetNumber")));
            }
        }
    }
}
