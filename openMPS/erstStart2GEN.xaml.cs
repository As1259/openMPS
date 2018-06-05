// Copyright (c) 2018, Andreas Schreiner

using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using de.fearvel.manastone.serialManagement;

namespace de.fearvel.openMPS
{
    /// <summary>
    ///     Interaktionslogik für erstStart.xaml
    /// </summary>
    public partial class erstStart2GEN : Window
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="erstStart" /> class.
        /// </summary>
        public erstStart2GEN()
        {
            InitializeComponent();
            Loaded += erstStart_Load;
        }

        /// <summary>
        ///     Handles the Load event of the erstStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        public void erstStart_Load(object sender, RoutedEventArgs e)
        {
            var erfasstvon = WindowsIdentity.GetCurrent().Name;
            if (erfasstvon.Contains("\\"))
                erfasstvon = erfasstvon.Substring(erfasstvon.IndexOf("\\") + 1,
                    erfasstvon.Length - erfasstvon.IndexOf("\\") - 1);
        }

        /// <summary>
        ///     Checks the database product key.
        /// </summary>
        //private void handleProductKey()
        //{
        //    try
        //    {
        //        if (CounterConfig.shellDT("Select * from manastone").Rows.Count > 0)
        //        {
        //            CounterConfig.shell("Delete from manastone");
        //        }
        //        if (tb_productkey1.Text.Length == 7 && tb_productkey2.Text.Length == 7 &&
        //            tb_productkey3.Text.Length == 7 && tb_productkey4.Text.Length == 7 &&
        //            tb_company.Text.Length > 0)
        //        {
        //            string serial = tb_productkey1.Text + tb_productkey2.Text +
        //                tb_productkey3.Text + tb_productkey4.Text;
        //            string i = SerialManagement.getCustomerNumber(serial);
        //            if (SerialManagement.checkValidity(serial) && SerialManagement.getCustomerNumber(serial).CompareTo(tb_kdnr.Text) == 0)
        //            {
        //                if (SerialManagement.checkDateOfExpiry(serial))
        //                {
        //                    CounterConfig.shell("insert into MANASTONE (Name, Serial) values" +
        //                    "('" + tb_company.Text + "','" + tb_productkey1.Text + tb_productkey2.Text +
        //                tb_productkey3.Text + tb_productkey4.Text + "');");
        //                    this.Close();
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Ihre Seriennummer ist abgelaufen\n" +
        //                "Sollten Sie Rückfragen haben, dann wenden Sie sich gerne an unsere Hotline unter: 0681-96719-140.",
        //                "ERR LIC", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        //                }

        //            }
        //            else
        //            {
        //                errormessage("Fehlerhafte Seriennummer oder Kundenummer");

        //            }

        //        }
        //        else
        //        {
        //            errormessage("Fehlerhafte Seriennummer");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        errormessage("Ein Fehler ist aufgetreten");
        //    }

        //}
        private void handleI01ProductKey()
        {
            if (SerialManager.ApplySerial(tb_I01Serial.Text))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                errormessage("Fehlerhafte Seriennummer");
            }
        }

        private void errormessage(string s)
        {
            MessageBox.Show(s, "Es ist ein Fehler aufgetreten", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        ///     Checks the input.
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     Handles the Click event of the bt_register control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_register_Click(object sender, RoutedEventArgs e)
        {
            handleI01ProductKey();
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_productkey1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_productkey1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_productkey1.Text.Contains("-")) tb_productkey1.Text = tb_productkey1.Text.Replace("-", "");
            if (tb_productkey1.Text.Length == 7)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox) sender).MoveFocus(request);
            }

            tb_productkey1.Text = tb_productkey1.Text.ToUpper();
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_productkey2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_productkey2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_productkey2.Text.Contains("-")) tb_productkey2.Text = tb_productkey2.Text.Replace("-", "");
            if (tb_productkey2.Text.Length == 7)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox) sender).MoveFocus(request);
            }

            tb_productkey2.Text = tb_productkey2.Text.ToUpper();
        }

        /// <summary>
        ///     Handles the TextChanged event of the tb_productkey3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs" /> instance containing the event data.</param>
        private void tb_productkey3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_productkey3.Text.Contains("-")) tb_productkey3.Text = tb_productkey3.Text.Replace("-", "");
            if (tb_productkey2.Text.Length == 7)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                };
                ((TextBox) sender).MoveFocus(request);
            }

            tb_productkey3.Text = tb_productkey3.Text.ToUpper();
        }

        private void tb_productkey4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_productkey4.Text.Contains("-")) tb_productkey4.Text = tb_productkey4.Text.Replace("-", "");
            tb_productkey4.Text = tb_productkey4.Text.ToUpper();
        }
    }
}