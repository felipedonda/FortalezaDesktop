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

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for ConfiguracoesModulos.xaml
    /// </summary>
    public partial class ConfiguracoesModulos : Window
    {
        public ConfiguracoesModulos()
        {
            InitializeComponent();
        }

        private void ButtoSalvar_Click(object sender, RoutedEventArgs e)
        {
            UserPreferences.Preferences.ModuloDelivery = CheckboxDelivery.IsChecked ?? false;
            UserPreferences.Preferences.ModuloPedido = CheckboxPedidos.IsChecked ?? false;
            UserPreferences.Preferences.ModuloVenda = CheckboxVenda.IsChecked ?? false;
            UserPreferences.Preferences.ModuloTroca = CheckboxTroca.IsChecked ?? false;
            UserPreferences.Save();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfig();
        }

        public void LoadConfig()
        {
            CheckboxDelivery.IsChecked = UserPreferences.Preferences.ModuloDelivery;
            CheckboxPedidos.IsChecked = UserPreferences.Preferences.ModuloPedido;
            CheckboxVenda.IsChecked = UserPreferences.Preferences.ModuloVenda;
            CheckboxTroca.IsChecked = UserPreferences.Preferences.ModuloTroca;
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
