﻿#pragma checksum "..\..\..\..\Views\EntradaEstoqueProdutos.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9D806A22E6CA8E56AA1BEBD4CB16BCF4FE0FAB70"
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
    /// EntradaEstoqueProdutos
    /// </summary>
    public partial class EntradaEstoqueProdutos : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 54 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridTituloCaixaAberto;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textboxBuscaCodigo;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textboxBuscaDescricao;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboboxBuscaGrupo;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
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
            System.Uri resourceLocater = new System.Uri("/FortalezaDesktop;component/views/entradaestoqueprodutos.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
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
            
            #line 9 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
            ((FortalezaDesktop.Views.EntradaEstoqueProdutos)(target)).Loaded += new System.Windows.RoutedEventHandler(this.OnLoad);
            
            #line default
            #line hidden
            return;
            case 2:
            this.gridTituloCaixaAberto = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.textboxBuscaCodigo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.textboxBuscaDescricao = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.comboboxBuscaGrupo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.datagridItems = ((System.Windows.Controls.DataGrid)(target));
            
            #line 73 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
            this.datagridItems.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.datagridItems_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 86 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonAdicionar);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 87 "..\..\..\..\Views\EntradaEstoqueProdutos.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonCancelar);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

