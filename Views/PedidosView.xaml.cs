using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for PedidosView.xaml
    /// </summary>
    public partial class PedidosView : Page
    {
        public PedidosView()
        {
            InitializeComponent();

            comboboxFiltrarPor.ItemsSource = new List<string> { "Data de Entrada", "Prazo de Entrega" };
            comboboxFiltrarPor.SelectedIndex = 0;

            comboboxSituacao.ItemsSource = new List<string> { "Todos", "Aberto", "Fechado", "Cancelado" };
            comboboxFiltroEntrega.ItemsSource = new List<string> { "Todos", "Iniciado", "Aguardando Entrega", "Saiu para Entrega", "Entregue", "Cancelado" };

            DateTime dataInicial = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                0,0,0);

            DateTime dataFinal = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                23,59,59);

            textboxDataInicial.Text = dataInicial.ToString("dd/MM/yy");
            textboxHoraInicial.Text = dataInicial.ToString("hh:mm:ss");
            textboxDataFinal.Text = dataFinal.ToString("dd/MM/yy");
            textboxHoraFinal.Text = dataFinal.ToString("hh:mm:ss");

        }

        private void ButtonNovo(object sender, RoutedEventArgs e)
        {
            PedidoDetails pedidoDetails = new PedidoDetails();
            pedidoDetails.Closed += PedidoDetails_Closed;
            pedidoDetails.ShowDialog();
        }

        private async void PedidoDetails_Closed(object sender, EventArgs e)
        {
            await LoadListPedidos();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadListPedidos();
        }

        public async Task LoadListPedidos()
        {
            List<Pedido> pedidos = await (new Pedido()).FindAll();
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = pedidos;
        }

        public async Task AlterarPedido()
        {
            int Idvenda = ((Pedido)datagridItems.SelectedItem).Idvenda;
            PedidoDetails pedidoDetails = new PedidoDetails(Idvenda);
            pedidoDetails.Closed += PedidoDetails_Closed;
            pedidoDetails.ShowDialog();
        }

        private async void datagridItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await AlterarPedido();
        }

        private async void ButtonAlterar(object sender, RoutedEventArgs e)
        {
            await AlterarPedido();
        }

        private async void ButtonFechar(object sender, RoutedEventArgs e)
        {
            Caixa caixaAberto = await (new Caixa()).GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
            if (caixaAberto != null)
            {
                var venda = ((Pedido)datagridItems.SelectedItem).IdvendaNavigation;
                VendaPagamentos pagamentosVendaView = new VendaPagamentos();
                await pagamentosVendaView.LoadVenda(venda.Idvenda);
                pagamentosVendaView.PagamentoRealizado += PagamentosVendaView_PagamentoRealizado;
                pagamentosVendaView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nenhum caixa aberto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

        }

        private async void PagamentosVendaView_PagamentoRealizado(object sender, EventArgs e)
        {
            await LoadListPedidos();
        }

        private void ButtonEntregar(object sender, RoutedEventArgs e)
        {
            var pedido = (Pedido)datagridItems.SelectedItem;
            PedidosEntrega pedidosEntrega = new PedidosEntrega(pedido);
            pedidosEntrega.Closed += PedidosEntrega_Closed;
            pedidosEntrega.ShowDialog();
        }

        private async void PedidosEntrega_Closed(object sender, EventArgs e)
        {
            await LoadListPedidos();
        }
    }
}
