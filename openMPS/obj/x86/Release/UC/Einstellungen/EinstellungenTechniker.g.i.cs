﻿#pragma checksum "..\..\..\..\..\UC\Einstellungen\EinstellungenTechniker.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "71C8AFFB024D0B62EC4FDEA1F81F69EC"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using btg.Zaehlerstand.UC.Einstellungen;


namespace btg.Zaehlerstand.UC.Einstellungen {
    
    
    /// <summary>
    /// EinstellungenTechniker
    /// </summary>
    public partial class EinstellungenTechniker : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\..\UC\Einstellungen\EinstellungenTechniker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bt_SQLite_dec;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\..\UC\Einstellungen\EinstellungenTechniker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bt_SQLite_enc;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Zählerstandsermittlung;component/uc/einstellungen/einstellungentechniker.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\UC\Einstellungen\EinstellungenTechniker.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.bt_SQLite_dec = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\..\..\UC\Einstellungen\EinstellungenTechniker.xaml"
            this.bt_SQLite_dec.Click += new System.Windows.RoutedEventHandler(this.bt_SQLite_dec_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.bt_SQLite_enc = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\..\..\UC\Einstellungen\EinstellungenTechniker.xaml"
            this.bt_SQLite_enc.Click += new System.Windows.RoutedEventHandler(this.bt_SQLite_enc_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
