using FortalezaDesktop.Models;
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
    /// Interaction logic for EntradaEstoqueProdutoQuantidade.xaml
    /// </summary>
    public partial class EstoqueEntradaProdutoQuantidade : Window
    {
        public decimal Quantidade { get; set; }
        public decimal Custo { get; set; }

        public EstoqueEntradaProdutoQuantidade(Item item)
        {
            Quantidade = 1;
            Custo = 0;
            InitializeComponent();
            textboxQuantidade.Text = Quantidade.ToString();
            textboxCusto.Text = Custo.ToString("C2");
            textboxDescricao.Text = item.Descricao;
        }

        private void ButtonAdicionar(object sender, RoutedEventArgs e)
        {
            Quantidade = decimal.Parse(textboxQuantidade.Text);
            Custo = decimal.Parse(textboxCusto.Text, System.Globalization.NumberStyles.Currency);
            Close();
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Quantidade = -1;
            Close();
        }
    }
}
