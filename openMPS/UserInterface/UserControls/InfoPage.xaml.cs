// Copyright (c) 2018 / 2019, Andreas Schreiner

using System.Windows.Controls;
using de.fearvel.net.FnLog;

namespace de.fearvel.openMPS.UserInterface.UserControls
{
    /// <summary>
    ///     Interaktionslogik für start.xaml
    /// </summary>
    public partial class InfoPage : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="start" /> class.
        /// </summary>
        public InfoPage()
        {
            InitializeComponent();
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MinorRuntimeInfo, "InfoPage", "Opened");

        }
    }
}