// Copyright (c) 2018 / 2019, Andreas Schreiner

using System;
using System.Windows;
using System.Windows.Controls;
using de.fearvel.net.FnLog;
using de.fearvel.net.Manastone;

namespace de.fearvel.openMPS.UserInterface.Windows
{
    /// <summary>
    /// Interaktionslogik für LicenseDialog.xaml
    /// </summary>
    public partial class LicenseDialog : Window
    {
        /// <summary>
        /// Result of this dialog
        /// true if an activation happened
        /// </summary>
        public bool Result { get; private set; }

        /// <summary>
        /// constructor
        /// presets Result
        /// and puts the window in the center of the screen
        /// </summary>
        public LicenseDialog()
        {
            Result = false;
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Exit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "LicenseDialog", "Exit Click");
            FnLog.GetInstance().ProcessLogList();
            Environment.Exit(1);
        }

        /// <summary>
        /// Activate button click event
        /// triggers an activation via ManastoneClient
        /// prompts message-boxes if activation failed
        /// closes the dialog if activation is successful
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonActivate_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckLength())
            {
                if (ManastoneClient.GetInstance().Activate(SpliceSegments()))
                {
                    Result = true;
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.RuntimeInfo, "LicenseDialog", "true close");
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

        /// <summary>
        /// splices the serialnumber segments
        /// </summary>
        /// <returns></returns>
        private string SpliceSegments()
        {
            return TextBoxSegmentOne.Text + "-" + TextBoxSegmentTwo.Text + "-" + TextBoxSegmentThree.Text + "-" +
                   TextBoxSegmentFour.Text + "-" + TextBoxSegmentFive.Text;
        }

        /// <summary>
        /// checks the length of the serialNumber
        /// </summary>
        /// <returns></returns>
        private bool CheckLength()
        {
            return TextBoxSegmentOne.Text.Trim().Length == 8 & TextBoxSegmentTwo.Text.Trim().Length == 4 &
                   TextBoxSegmentThree.Text.Trim().Length == 4 & TextBoxSegmentFour.Text.Trim().Length == 4 &
                   TextBoxSegmentFive.Text.Trim().Length == 12;
        }


        #region AutomaticTextboxSwitch

        /// <summary>
        /// automatic textbox switching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxSegmentOne_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox) sender).Text.Length == 8 && this.IsLoaded)
            {
                TextBoxSegmentTwo.Focus();
                TextBoxSegmentTwo.SelectAll();
            }
        }

        /// <summary>
        /// automatic textbox switching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxSegmentTwo_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox) sender).Text.Length == 4 && this.IsLoaded)
            {
                TextBoxSegmentThree.Focus();
                TextBoxSegmentThree.SelectAll();
            }
        }

        /// <summary>
        /// automatic textbox switching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxSegmentThree_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox) sender).Text.Length == 4 && this.IsLoaded)
            {
                TextBoxSegmentFour.Focus();
                TextBoxSegmentFour.SelectAll();
            }
        }

        /// <summary>
        /// automatic textbox switching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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