using FortalezaDesktop.Models;
using FortalezaDesktop.Views.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for RelatoriosProdutosVendidos.xaml
    /// </summary>
    public partial class RelatoriosProdutosVendidos : Window
    {
        public RelatoriosProdutosVendidos()
        {
            InitializeComponent();
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonOk(object sender, RoutedEventArgs e)
        {
            TemplateRelatorioProdutosVendidos relatorio = new TemplateRelatorioProdutosVendidos();
            relatorio.textSubtitulo.Text = "Período de: 00/00/00 até: 00/00/00.";
            List<ItemVenda> itemsVendidos = await (new ItemVenda()).FindAll();
            relatorio.mainGrid.ItemsSource = itemsVendidos;

            RelatoriosVizualizador vizualizador = new RelatoriosVizualizador();
            vizualizador.LoadChildPage(relatorio);
            vizualizador.Show();
        }
    }
}
