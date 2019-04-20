using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using de.fearvel.net.Manastone;

namespace de.fearvel.openMPS.UserInterface.Windows
{
    /// <summary>
    /// Interaktionslogik für LicenseDialog.xaml
    /// </summary>
    public partial class LicenseDialog : Window
    {
        public bool Result { get; private set; }

        public LicenseDialog()
        {
            Result = false;
            InitializeComponent();
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void ButtonActivate_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckLength())
            {
                if (ManastoneClient.GetInstance().Activate(SpliceSegments()))
                {
                    Result = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Fehlerhafe Seriennummer");
                }
            }
            else
            {
                MessageBox.Show("Bitte geben Sie eine gültige Seriennummer ein!");
            }
        }

        private string SpliceSegments()
        {
            return TextBoxSegmentOne.Text + "-" + TextBoxSegmentTwo.Text + "-" + TextBoxSegmentThree.Text + "-" +
                   TextBoxSegmentFour.Text + "-" + TextBoxSegmentFive.Text;
        }

        private bool CheckLength()
        {
            return TextBoxSegmentOne.Text.Trim().Length == 8 & TextBoxSegmentTwo.Text.Trim().Length == 4 &
                   TextBoxSegmentThree.Text.Trim().Length == 4 & TextBoxSegmentFour.Text.Trim().Length == 4 &
                   TextBoxSegmentFive.Text.Trim().Length == 12;
        }


        #region AutomaticTextboxSwitch

        private void TextBoxSegmentOne_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox) sender).Text.Length == 8 && this.IsLoaded)
            {
                TextBoxSegmentTwo.Focus();
                TextBoxSegmentTwo.SelectAll();
            }
        }

        private void TextBoxSegmentTwo_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox) sender).Text.Length == 4 && this.IsLoaded)
            {
                TextBoxSegmentThree.Focus();
                TextBoxSegmentThree.SelectAll();
            }
        }

        private void TextBoxSegmentThree_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox) sender).Text.Length == 4 && this.IsLoaded)
            {
                TextBoxSegmentFour.Focus();
                TextBoxSegmentFour.SelectAll();
            }
        }

        private void TextBoxSegmentFour_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox) sender).Text.Length == 4 && this.IsLoaded)
            {
                TextBoxSegmentFive.Focus();
                TextBoxSegmentFive.SelectAll();
            }
        }

        #endregion
    }
}