﻿#pragma checksum "..\..\..\..\Views\ClienteDetails.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B39036ACA7BDACDBD6987C3AB9B464470E842A2B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FortalezaDesktop.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace FortalezaDesktop.Views {
    
    
    /// <summary>
    /// ClienteDetails
    /// </summary>
    public partial class ClienteDetails : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\Views\ClienteDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridCliente;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\Views\ClienteDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textboxCep;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\Views\ClienteDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonCriar;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\..\Views\ClienteDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonAlterar;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\..\Views\ClienteDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonRemover;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\..\Views\ClienteDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonCancelar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FortalezaDesktop;V1.0.0.0;component/views/clientedetails.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\ClienteDetails.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.gridCliente = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            
            #line 52 "..\..\..\..\Views\ClienteDetails.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonConsultarCEP);
            
            #line default
            #line hidden
            return;
            case 3:
            this.textboxCep = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.buttonCriar = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\..\..\Views\ClienteDetails.xaml"
            this.buttonCriar.Click += new System.Windows.RoutedEventHandler(this.buttonCriar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.buttonAlterar = ((System.Windows.Controls.Button)(target));
            
            #line 91 "..\..\..\..\Views\ClienteDetails.xaml"
            this.buttonAlterar.Click += new System.Windows.RoutedEventHandler(this.buttonAlterar_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.buttonRemover = ((System.Windows.Controls.Button)(target));
            
            #line 92 "..\..\..\..\Views\ClienteDetails.xaml"
            this.buttonRemover.Click += new System.Windows.RoutedEventHandler(this.buttonRemover_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.buttonCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 93 "..\..\..\..\Views\ClienteDetails.xaml"
            this.buttonCancelar.Click += new System.Windows.RoutedEventHandler(this.buttonCancelar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

