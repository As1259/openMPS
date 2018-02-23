#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System.Windows;
using System.Windows.Input;

namespace de.as1259.openMPS.UC.Einstellungen
{
    /// <summary>
    ///     Interaktionslogik für TechnikLogin.xaml
    /// </summary>
    public partial class TechnikLogin : Window
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TechnikLogin" /> class.
        /// </summary>
        public TechnikLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Handles the MouseLeftButtonDown event of the password control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the event data.</param>
        private void password_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        ///     Handles the Click event of the bt_login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void bt_login_Click(object sender, RoutedEventArgs e)
        {
            // if (pass.Password.CompareTo(
            //         "^mf]eN61N(>j=461OPf5h4@A/iLoK!56A%5zX],a9)P@76O8oh0161]8.<EE4_JJm^oXy85P2j7pQk@G6z5tvM£k#Sxy") ==
            //     0)
            // {
            DialogResult = true;
            Close();
            // }
        }
    }
}