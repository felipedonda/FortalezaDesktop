﻿#pragma checksum "..\..\..\..\Views\EntradaEstoqueView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ABC9A737FDDA124AACE3715D42969D45204201B9"
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
    /// EntradaEstoqueView
    /// </summary>
    public partial class EntradaEstoqueView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 56 "..\..\..\..\Views\EntradaEstoqueView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridTituloCaixaAberto;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\Views\EntradaEstoqueView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textboxData;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\Views\EntradaEstoqueView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textboxHora;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\Views\EntradaEstoqueView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textboxObservacao;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\Views\EntradaEstoqueView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textboxNotaFiscal;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\Views\EntradaEstoqueView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox textboxFornecedor;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\..\Views\EntradaEstoqueView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid datagridItems;
        
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
            System.Uri resourceLocater = new System.Uri("/FortalezaDesktop;component/views/entradaestoqueview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\EntradaEstoqueView.xaml"
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
            this.gridTituloCaixaAberto = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.textboxData = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.textboxHora = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.textboxObservacao = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.textboxNotaFiscal = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.textboxFornecedor = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            
            #line 90 "..\..\..\..\Views\EntradaEstoqueView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonAdicionar_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 91 "..\..\..\..\Views\EntradaEstoqueView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonRemover_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.datagridItems = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            
            #line 106 "..\..\..\..\Views\EntradaEstoqueView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonEntrada);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 107 "..\..\..\..\Views\EntradaEstoqueView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonCancelar);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

