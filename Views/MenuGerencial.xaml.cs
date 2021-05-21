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
        public MenuGerencial()
        {
            InitializeComponent();
        }

        public void buttonProdutos_Click(object sender, RoutedEventArgs e)
        {
            Produtos produtoView = new Produtos();
            produtoView.ShowDialog();
            Close();
        }

        private void buttonConfiguracoes_Click(object sender, RoutedEventArgs e)
        {
            Configuracoes configuracoesView = new Configuracoes();
            configuracoesView.ShowDialog();
            Close();
        }

        private void buttonEstoque_Click(object sender, RoutedEventArgs e)
        {
            EstoqueView estoqueView = new EstoqueView();
            estoqueView.ShowDialog();
            Close();
        }

        private void buttonProdutosVendidos_Click(object sender, RoutedEventArgs e)
        {
            RelatoriosProdutosVendidos relatoriosProdutosVendidos = new RelatoriosProdutosVendidos();
            relatoriosProdutosVendidos.ShowDialog();
            Close();
        }

        private void buttonClientes_Click(object sender, RoutedEventArgs e)
        {
            Clientes clientes = new Clientes();
            clientes.ShowDialog();
            Close();
        }

        private void buttonGrupos_Click(object sender, RoutedEventArgs e)
        {
            GruposView gruposView = new GruposView();
            gruposView.ShowDialog();
            Close();
        }

        private void buttonDadosLoja_Click(object sender, RoutedEventArgs e)
        {
            InformacoesEmpresaDetails informacoesEmpresaDetails = new InformacoesEmpresaDetails();
            informacoesEmpresaDetails.ShowDialog();
            Close();
        }

        private void ButtonPontoVenda_Click(object sender, RoutedEventArgs e)
        {
            PontoVendaView pontoVendaView = new PontoVendaView();
            pontoVendaView.ShowDialog();
            Close();
        }

        private void ButtonPagamentos_Click(object sender, RoutedEventArgs e)
        {
            FormaPagamentosView formaPagamentosView = new FormaPagamentosView();
            formaPagamentosView.ShowDialog();
            Close();
        }

        private void ButtonImpressoras_Click(object sender, RoutedEventArgs e)
        {
            Impressoras impressoras = new Impressoras();
            impressoras.ShowDialog();
            Close();
        }

        private void ButtonModulos_Click(object sender, RoutedEventArgs e)
        {
            ConfiguracoesModulos configuracoesModulos = new ConfiguracoesModulos();
            configuracoesModulos.ShowDialog();
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void ButtonSat_Click(object sender, RoutedEventArgs e)
        {
            ConfiguracaoSat configuracaoSat = new ConfiguracaoSat();
            configuracaoSat.ShowDialog();
            Close();
        }
    }
}
