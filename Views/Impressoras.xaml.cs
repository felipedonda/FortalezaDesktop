using System;
using System.Collections.Generic;
using System.Printing;
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
    /// Interaction logic for Impressoras.xaml
    /// </summary>
    public partial class Impressoras : Window
    {
        public Impressoras()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImpressoras();
            LoadConfig();
        }

        public void LoadImpressoras()
        {
            LocalPrintServer printServer = new LocalPrintServer();
            var printQueues = printServer.GetPrintQueues();
            List<string> printers = new List<string>
            {
                ""
            };
            foreach (var queue in printQueues)
            {
                printers.Add(queue.Name);
            }
            ComboBoxCozinha.ItemsSource = printers;
            ComboBoxCupom.ItemsSource = printers;
            ComboBoxRelatorio.ItemsSource = printers;
        }

        public void LoadConfig()
        {
            ComboBoxCozinha.SelectedItem = UserPreferences.Preferences.ImpressoraCozinha.ImpressoraPadrao;
            CheckboxHabilitadaCozinha.IsChecked = UserPreferences.Preferences.ImpressoraCozinha.Habilitada;
            CheckboxSempreImprimirCozinha.IsChecked = UserPreferences.Preferences.ImpressoraCozinha.SempreImprimir;
            CheckboxVisualizarCozinha.IsChecked = UserPreferences.Preferences.ImpressoraCozinha.Visualizar;

            ComboBoxCupom.SelectedItem = UserPreferences.Preferences.ImpressoraCupom.ImpressoraPadrao;
            CheckboxHabilitadaCupom.IsChecked = UserPreferences.Preferences.ImpressoraCupom.Habilitada;
            CheckboxSempreImprimirCupom.IsChecked = UserPreferences.Preferences.ImpressoraCupom.SempreImprimir;
            CheckboxVisualizarCupom.IsChecked = UserPreferences.Preferences.ImpressoraCupom.Visualizar;
            if (UserPreferences.Preferences.ImpressoraCupom.Tamanho == ConfigImpressora.TamanhoImpressao.Tamanho80mm)
            {
                RadioButton80.IsChecked = true;
            }
            else
            {
                RadioButton50.IsChecked = true;
            }

            ComboBoxRelatorio.SelectedItem = UserPreferences.Preferences.ImpressoraRelatorio.ImpressoraPadrao;
            CheckboxHabilitadaRelatorio.IsChecked = UserPreferences.Preferences.ImpressoraRelatorio.Habilitada;
            CheckboxHabilitadaRelatorio.IsChecked = UserPreferences.Preferences.ImpressoraRelatorio.Habilitada;
            CheckboxSempreImprimirRelatorio.IsChecked = UserPreferences.Preferences.ImpressoraRelatorio.SempreImprimir;
            CheckboxVisualizarRelatorio.IsChecked = UserPreferences.Preferences.ImpressoraRelatorio.Visualizar;

            CheckCupomHabilitada();
            CheckCozinhaHabilitada();
            CheckRelatorioHabilitada();
        }

        private void SaveConfig()
        {
            UserPreferences.Preferences.ImpressoraCozinha.ImpressoraPadrao = ComboBoxCozinha.SelectedItem.ToString();
            UserPreferences.Preferences.ImpressoraCozinha.SempreImprimir = CheckboxSempreImprimirCozinha.IsChecked ?? false;
            UserPreferences.Preferences.ImpressoraCozinha.Visualizar = CheckboxVisualizarCozinha.IsChecked ?? false;
            UserPreferences.Preferences.ImpressoraCozinha.Habilitada = CheckboxHabilitadaCozinha.IsChecked ?? false;

            UserPreferences.Preferences.ImpressoraCupom.ImpressoraPadrao = ComboBoxCupom.SelectedItem.ToString();
            UserPreferences.Preferences.ImpressoraCupom.SempreImprimir = CheckboxSempreImprimirCupom.IsChecked ?? false;
            UserPreferences.Preferences.ImpressoraCupom.Visualizar = CheckboxVisualizarCupom.IsChecked ?? false;
            UserPreferences.Preferences.ImpressoraCupom.Habilitada = CheckboxHabilitadaCupom.IsChecked ?? false;

            if (RadioButton80.IsChecked ?? false)
            {
                UserPreferences.Preferences.ImpressoraCupom.Tamanho = ConfigImpressora.TamanhoImpressao.Tamanho80mm;
            }
            else
            {
                UserPreferences.Preferences.ImpressoraCupom.Tamanho = ConfigImpressora.TamanhoImpressao.Tamanho50mm;
            }

            UserPreferences.Preferences.ImpressoraRelatorio.ImpressoraPadrao = ComboBoxRelatorio.SelectedItem.ToString();
            UserPreferences.Preferences.ImpressoraRelatorio.SempreImprimir = CheckboxSempreImprimirRelatorio.IsChecked ?? false;
            UserPreferences.Preferences.ImpressoraRelatorio.Visualizar = CheckboxVisualizarRelatorio.IsChecked ?? false;
            UserPreferences.Preferences.ImpressoraRelatorio.Habilitada = CheckboxHabilitadaRelatorio.IsChecked ?? false;

            UserPreferences.Save();
        }

        private void ButtoSalvar_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
            Close();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckboxHabilitadaCupom_Checked(object sender, RoutedEventArgs e)
        {
            CheckCupomHabilitada();
        }

        private void CheckboxHabilitadaCozinha_Checked(object sender, RoutedEventArgs e)
        {
            CheckCozinhaHabilitada();
        }

        private void CheckboxHabilitadaRelatorio_Checked(object sender, RoutedEventArgs e)
        {
            CheckRelatorioHabilitada();
        }

        private void CheckCupomHabilitada()
        {
            ComboBoxCupom.IsEnabled = CheckboxHabilitadaCupom.IsChecked ?? false;
            CheckboxSempreImprimirCupom.IsEnabled = CheckboxHabilitadaCupom.IsChecked ?? false;
            CheckboxVisualizarCupom.IsEnabled = CheckboxHabilitadaCupom.IsChecked ?? false;
            RadioButton50.IsEnabled = CheckboxHabilitadaCupom.IsChecked ?? false;
            RadioButton80.IsEnabled = CheckboxHabilitadaCupom.IsChecked ?? false;
        }

        private void CheckCozinhaHabilitada()
        {
            ComboBoxCozinha.IsEnabled = CheckboxHabilitadaCupom.IsChecked ?? false;
            CheckboxSempreImprimirCozinha.IsEnabled = CheckboxHabilitadaCozinha.IsChecked ?? false;
            CheckboxVisualizarCozinha.IsEnabled = CheckboxHabilitadaCozinha.IsChecked ?? false;
        }

        private void CheckRelatorioHabilitada()
        {
            ComboBoxCozinha.IsEnabled = CheckboxHabilitadaCupom.IsChecked ?? false;
            CheckboxSempreImprimirRelatorio.IsEnabled = CheckboxHabilitadaRelatorio.IsChecked ?? false;
            CheckboxVisualizarRelatorio.IsEnabled = CheckboxHabilitadaRelatorio.IsChecked ?? false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
