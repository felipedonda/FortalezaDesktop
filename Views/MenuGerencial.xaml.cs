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
using FortalezaDesktop.Views;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for MenuGerencial.xaml
    /// </summary>
    public partial class MenuGerencial : Window
    {
        public event EventHandler UpdateParent;
        public MenuGerencial()
        {
            InitializeComponent();
        }
        
        public void buttonProdutos_Click(object sender, RoutedEventArgs e)
        {
            Produtos produtoView = new Produtos();
            produtoView.Closed += ConfiguracoesView_UpdateParent;
            produtoView.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonConfiguracoes_Click(object sender, RoutedEventArgs e)
        {
            Configuracoes configuracoesView = new Configuracoes();
            configuracoesView.UpdateParent += ConfiguracoesView_UpdateParent;
            configuracoesView.Show();
        }

        private void ConfiguracoesView_UpdateParent(object sender, EventArgs e)
        {
            UpdateParent?.Invoke(this,new EventArgs());
        }

        private void buttonEstoque_Click(object sender, RoutedEventArgs e)
        {
            Estoque estoqueView = new Estoque();
            estoqueView.Show();
            Close();
        }

        private void buttonProdutosVendidos_Click(object sender, RoutedEventArgs e)
        {
            RelatoriosProdutosVendidos relatoriosProdutosVendidos = new RelatoriosProdutosVendidos();
            relatoriosProdutosVendidos.Show();
            Close();
        }

        private void buttonClientes_Click(object sender, RoutedEventArgs e)
        {
            Clientes clientes = new Clientes();
            clientes.Show();
            Close();
        }
    }
}
