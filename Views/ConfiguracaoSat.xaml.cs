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
using FortalezaDesktop.Controllers;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for ConfiguracaoSat.xaml
    /// </summary>
    public partial class ConfiguracaoSat : Window
    {
        public ConfiguracaoSat()
        {
            InitializeComponent();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtoTestar_Click(object sender, RoutedEventArgs e)
        {
            bool result = await SatController.RunTest();
            if(result)
            {
                MessageBox.Show("Teste de conexão do SAT foi concluído com sucesso.", "Teste Conexão SAT", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Teste de conexão do SAT falhou.", "Teste Conexão SAT", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CheckboxHomologacao_Checked(object sender, RoutedEventArgs e)
        {
            UserPreferences.Preferences.SatHomologacao = CheckboxHomologacao.IsChecked ?? false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckboxHomologacao.IsChecked = UserPreferences.Preferences.SatHomologacao;
            TextboxSenha.Text = UserPreferences.Preferences.SenhaSat;
        }

        private void ButtoSalvar_Click(object sender, RoutedEventArgs e)
        {
            UserPreferences.Preferences.SenhaSat = TextboxSenha.Text;
            UserPreferences.Save();
            Close();
        }
    }
}
