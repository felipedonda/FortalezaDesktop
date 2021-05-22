using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for ConfigFiscal.xaml
    /// </summary>
    public partial class ConfigFiscal : Window
    {

        public ConfigFiscal()
        {
            InitializeComponent();
        }

        private void ButtoSalvar_Click(object sender, RoutedEventArgs e)
        {
            UserPreferences.Save();
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ComboboxCfop.ItemsSource = Fiscal.ListaCfop;
            ComboboxCfop.SelectedValuePath = "Value";
            ComboboxCfop.DisplayMemberPath = "Display";

            ComboboxOrigem.ItemsSource = Fiscal.ListaOrigem;
            ComboboxOrigem.SelectedValuePath = "Value";
            ComboboxOrigem.DisplayMemberPath = "Display";

            ComboboxCstIcms.ItemsSource = Fiscal.ListaCstIcms;
            ComboboxCstIcms.SelectedValuePath = "Value";
            ComboboxCstIcms.DisplayMemberPath = "Display";

            GridPrincipal.DataContext = UserPreferences.Preferences.FiscalPadrao;
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
