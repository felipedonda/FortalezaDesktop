using FortalezaDesktop.Controllers;
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
    /// Interaction logic for DeliveryView.xaml
    /// </summary>
    public partial class DeliveryView : Page
    {
        public VendaItemsSelecionados ItemsSelecionados { get; set; }
        public Pedido Pedido { get; set; }
        public DeliveryDetails DeliveryDetails { get; set; }
        public DeliveryPedidos DeliveryPedidos { get; set; }
        public bool PedidoIsNew { get; set; }

        public DeliveryView()
        {
            InitializeComponent();
            ItemsSelecionados = new VendaItemsSelecionados(2);
            
            DeliveryDetails = new DeliveryDetails();
            ItemsSelecionados.ItemVendaSelected += ItemsSelecionados_ItemVendaSelected;
            ItemsSelecionados.ReceberClicked += ItemsSelecionados_ReceberClicked;
            ItemsSelecionados.CancelarSelected += ItemsSelecionados_CancelarSelected;

            DeliveryPedidos = new DeliveryPedidos();
            DeliveryPedidos.PedidoHighlighted += DeliveryPedidos_PedidoHighlighted;
            DeliveryPedidos.PedidoSelected += DeliveryPedidos_PedidoSelected;
        }

        private async void ItemsSelecionados_CancelarSelected(object sender, EventArgs e)
        {
            var msgResult = MessageBox.Show("Deseja cancelar o pedido?", "Cancelar Pedido", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgResult == MessageBoxResult.Yes)
            {
                if(PedidoIsNew)
                {
                    await Pedido.DeleteInstance();
                    ItemsSelecionados.LimparVenda();
                }
                else
                {
                    Pedido.Status = 5;
                    await Pedido.UpdateInstance();
                    ItemsSelecionados.LimparVenda();
                    await DeliveryDetails.LoadPedido(Pedido.Idvenda);
                }
                await LoadListaPedidos();
                await LoadDeliveryDetails();
            }
            else
            {
                return;
            }
        }

        private async void ItemsSelecionados_ReceberClicked(object sender, EventArgs e)
        {
            await ReceberPagamento();
        }

        private async Task ReceberPagamento(bool gerarCupom = false)
        {
            if (Pedido.IdvendaNavigation.Idcliente == null)
            {
                PedidoDetailsCliente pedidoDetailsCliente = new PedidoDetailsCliente();
                pedidoDetailsCliente.Selecionado += PedidoDetailsCliente_SelecionadoReceber;
                pedidoDetailsCliente.ExigirEndereco = true;
                pedidoDetailsCliente.ShowDialog();
                return;
            }

            if (Pedido.Idmetodo == null)
            {
                DeliveryEntregador deliveryEntregador = new DeliveryEntregador();
                await deliveryEntregador.LoadData();
                await deliveryEntregador.LoadPedido(Pedido.Idvenda);
                deliveryEntregador.Salvar += DeliveryEntregador_SalvarReceber;
                deliveryEntregador.ShowDialog();
                return;
            }

            VendaPagamentos vendaPagamentos = new VendaPagamentos();
            await vendaPagamentos.LoadVenda(Pedido.Idvenda);

            if (gerarCupom)
            {
                vendaPagamentos.PagamentoRealizado += VendaPagamentos_PagamentoRealizadoCupom;
            }
            else
            {
                vendaPagamentos.AllowPagamentoIncompleto = true;
                vendaPagamentos.PagamentoRealizado += VendaPagamentos_PagamentoRealizado;
            }
            vendaPagamentos.ShowDialog();
        }

        private async void PedidoDetailsCliente_SelecionadoReceber(object sender, PedidoDetailsCliente.ClienteSelecionadoEventArgs e)
        {
            Pedido.IdvendaNavigation.Idcliente = e.Cliente.Idcliente;
            textButtonCliente.Text = e.Cliente.Nome;
            await Pedido.IdvendaNavigation.UpdateInstance();

            Pedido = await Pedido.ReloadInstance(new Dictionary<string, string> {
                    {"itemvenda","true" }
                });

            await ReceberPagamento();
        }

        private async void DeliveryEntregador_SalvarReceber(object sender, DeliveryEntregador.SalvarEventArgs e)
        {
            Pedido = e.Pedido;
            await Pedido.UpdateInstance();
            await ReceberPagamento();
        }

        private async void VendaPagamentos_PagamentoRealizado(object sender, EventArgs e)
        {
            await LoadListaPedidos();
            await LoadDeliveryDetails();
            await DeliveryDetails.LoadPedido(Pedido.Idvenda);
            ItemsSelecionados.LimparVenda();
        }

        private async void VendaPagamentos_PagamentoRealizadoCupom(object sender, EventArgs e)
        {
            await GerarCupom();
        }

        private async void ItemsSelecionados_ItemVendaSelected(object sender, VendaItemsSelecionados.ItemVendaSelectedEventArgs e)
        {
            await ItemsSelecionados.AddProdutoVenda(e.Iditem);
            Pedido.IdvendaNavigation = ItemsSelecionados.Venda;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadListaPedidos();
            await LoadDeliveryDetails();
        }

        public async Task LoadItemsSelecionados()
        {
            frameDetails.NavigationService.RemoveBackEntry();
            frameDetails.Navigate(ItemsSelecionados);
        }


        public async Task LoadListaPedidos()
        {
            gridSubMenuCompra.Visibility = Visibility.Collapsed;
            gridMenuCompra.Visibility = Visibility.Collapsed;
            gridMenuListaPedidos.Visibility = Visibility.Visible;

            frameListaDeliveries.NavigationService.RemoveBackEntry();
            frameListaDeliveries.Navigate(DeliveryPedidos);
            await DeliveryPedidos.LoadPedidos();
        }

        private async void DeliveryPedidos_PedidoSelected(object sender, DeliveryPedidos.PedidoSelectedEventArgs e)
        {
            PedidoIsNew = false;
            await LoadDelivery(e.Idvenda);
        }

        private async void DeliveryPedidos_PedidoHighlighted(object sender, DeliveryPedidos.PedidoSelectedEventArgs e)
        {
            await DeliveryDetails.LoadPedido(e.Idvenda);

            switch (DeliveryDetails.Pedido.Status)
            {
                case 1:
                    //return "Novo Pedido";
                    buttonAlterar.IsEnabled = true;
                    buttonCancelar.IsEnabled = true;
                    buttonEnviarCozinha.IsEnabled = true;
                    buttonGerarCupom.IsEnabled = true;
                    buttonEnviarEntrega.IsEnabled = false;
                    buttonConcluir.IsEnabled = false;
                    break;
                case 2:
                    //return "Pedido Faturado";
                    buttonAlterar.IsEnabled = false;
                    buttonCancelar.IsEnabled = false;
                    buttonEnviarCozinha.IsEnabled = true;
                    buttonGerarCupom.IsEnabled = false;
                    buttonEnviarEntrega.IsEnabled = true;
                    buttonConcluir.IsEnabled = true;
                    break;
                case 3:
                    //return "Saiu para Entrega";
                    buttonAlterar.IsEnabled = false;
                    buttonCancelar.IsEnabled = false;
                    buttonEnviarCozinha.IsEnabled = false;
                    buttonGerarCupom.IsEnabled = false;
                    buttonEnviarEntrega.IsEnabled = false;
                    buttonConcluir.IsEnabled = true;
                    break;
                case 4:
                    //return "Pedido Concluído";
                    buttonAlterar.IsEnabled = false;
                    buttonCancelar.IsEnabled = false;
                    buttonEnviarCozinha.IsEnabled = false;
                    buttonGerarCupom.IsEnabled = false;
                    buttonEnviarEntrega.IsEnabled = false;
                    buttonConcluir.IsEnabled = false;
                    break;
                case 5:
                    //return "Cancelado";
                    buttonAlterar.IsEnabled = false;
                    buttonCancelar.IsEnabled = false;
                    buttonEnviarCozinha.IsEnabled = false;
                    buttonGerarCupom.IsEnabled = false;
                    buttonEnviarEntrega.IsEnabled = false;
                    buttonConcluir.IsEnabled = false;
                    break;
            }
        }

        public async Task LoadDeliveryDetails()
        {
            frameDetails.NavigationService.RemoveBackEntry();
            frameDetails.Navigate(DeliveryDetails);
        }

        public async Task LoadSelecaoProdutos()
        {
            gridSubMenuCompra.Visibility = Visibility.Visible;
            gridMenuCompra.Visibility = Visibility.Visible;
            gridMenuListaPedidos.Visibility = Visibility.Collapsed;

            VendaSelecaoProdutos selecaoProdutos = new VendaSelecaoProdutos();
            selecaoProdutos.ProdutoSelected += SelecaoProdutos_ProdutoSelected;
            frameListaDeliveries.NavigationService.RemoveBackEntry();
            frameListaDeliveries.Navigate(selecaoProdutos);
        }

        private async void SelecaoProdutos_ProdutoSelected(object sender, VendaSelecaoProdutos.ProdutoSelectedEventArgs e)
        {
            await ItemsSelecionados.AddProdutoVenda(e.Iditem);
            Pedido.IdvendaNavigation = ItemsSelecionados.Venda;
        }

        private void buttonSelecionaCliente_Click(object sender, RoutedEventArgs e)
        {
            PedidoDetailsCliente pedidoDetailsCliente = new PedidoDetailsCliente();
            pedidoDetailsCliente.Selecionado += PedidoDetailsCliente_Selecionado;
            pedidoDetailsCliente.ExigirEndereco = true;
            pedidoDetailsCliente.ShowDialog();
        }

        private async void PedidoDetailsCliente_Selecionado(object sender, PedidoDetailsCliente.ClienteSelecionadoEventArgs e)
        {
            ItemsSelecionados.Venda.Idcliente = e.Cliente.Idcliente;
            textButtonCliente.Text = e.Cliente.Nome;
            await ItemsSelecionados.Venda.UpdateInstance();
            Pedido.IdvendaNavigation = ItemsSelecionados.Venda;
        }

        private async void buttonNovoDelivery_Click(object sender, RoutedEventArgs e)
        {
            if(await ItemsSelecionados.ValidateVenda())
            {
                PedidoIsNew = true;
                Pedido = new Pedido
                {
                    Idvenda = ItemsSelecionados.Venda.Idvenda,
                    Status = 1,
                    Delivery = 1,
                    TempoCozinha = 20,
                    TempoEntrega = 20,
                    Entregue = 0
                };
                await Pedido.SaveInstance();
                Pedido.IdvendaNavigation = ItemsSelecionados.Venda;
                textButtonCliente.Text = "Selecionar Cliente";
                await LoadSelecaoProdutos();
                await LoadItemsSelecionados();
            }
        }

        private async void buttonVoltar_Click(object sender, RoutedEventArgs e)
        {
            if(PedidoIsNew && ItemsSelecionados.Venda.ItemVenda.Count == 0)
            {
                var msgResult = MessageBox.Show("Pedido não preenchido, deseja cancelar?","Cancelar Pedido", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(msgResult == MessageBoxResult.Yes)
                {
                    await Pedido.DeleteInstance();
                }
                else
                {
                    return;
                }
            }
            ItemsSelecionados.LimparVenda();
            await LoadListaPedidos();
            await LoadDeliveryDetails();
        }

        private async void buttonAlterarDelivery_Click(object sender, RoutedEventArgs e)
        {
            if(DeliveryPedidos.datagridPedidos.SelectedItem != null)
            {
                Pedido pedido = (Pedido)DeliveryPedidos.datagridPedidos.SelectedItem;
                await LoadDelivery(pedido.Idvenda);
            }
        }

        public async Task LoadDelivery(int idvenda)
        {
            await LoadItemsSelecionados();
            Pedido = await new Pedido().FindById(idvenda, new Dictionary<string, string> {
                    {"itemvenda","true" }
                });

            if (Pedido.IdvendaNavigation.Idcliente != null)
            {
                textButtonCliente.Text = Pedido.IdvendaNavigation.IdclienteNavigation.Nome;
            }
            else
            {
                textButtonCliente.Text = "Selecionar Cliente";
            }

            await ItemsSelecionados.LoadVenda(Pedido.Idvenda);
            await LoadSelecaoProdutos();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Unloading.");
        }

        private async void buttonSelecionarEntregador(object sender, RoutedEventArgs e)
        {
            DeliveryEntregador deliveryEntregador = new DeliveryEntregador();
            await deliveryEntregador.LoadData();
            await deliveryEntregador.LoadPedido(Pedido.Idvenda);
            deliveryEntregador.Salvar += DeliveryEntregador_Salvar;
            deliveryEntregador.ShowDialog();
        }

        private async void DeliveryEntregador_Salvar(object sender, DeliveryEntregador.SalvarEventArgs e)
        {
            Pedido = e.Pedido;
            await Pedido.UpdateInstance();
            await ItemsSelecionados.ReloadVenda();
        }

        private async void buttonCancelarDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (DeliveryPedidos.datagridPedidos.SelectedItem != null)
            {
                Pedido pedido = (Pedido)DeliveryPedidos.datagridPedidos.SelectedItem;
                var msgResult = MessageBox.Show("Deseja cancelar o pedido " + pedido.NumeroPedido + "?", "Cancelar Pedido", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (msgResult == MessageBoxResult.Yes)
                {
                    pedido.Status = 5;
                    await pedido.UpdateInstance();
                    await DeliveryPedidos.LoadPedidos();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void buttonGerarCupom_Click(object sender, RoutedEventArgs e)
        {
            if (DeliveryPedidos.datagridPedidos.SelectedItem != null)
            {
                Pedido pedido = (Pedido)DeliveryPedidos.datagridPedidos.SelectedItem;

                Pedido = await new Pedido().FindById(pedido.Idvenda, new Dictionary<string, string> {
                    {"itemvenda","true" }
                });

                await GerarCupom();
            }
        }

        private async Task GerarCupom()
        {
            if (Pedido.IdvendaNavigation.Aberta == 0)
            {
                return;
            }

            if(Pedido.IdvendaNavigation.Paga == 0)
            {
                await ReceberPagamento(true);
                return;
            }

            await Pedido.IdvendaNavigation.FecharVenda();
            Pedido.Status = 2;
            await Pedido.UpdateInstance();
            RelatoriosController.GerarCupomPedido(Pedido);
            await DeliveryPedidos.LoadPedidos();
            await DeliveryDetails.LoadPedido(Pedido.Idvenda);
        }

        private async void buttonImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (DeliveryPedidos.datagridPedidos.SelectedItem != null)
            {
                Pedido pedido = (Pedido)DeliveryPedidos.datagridPedidos.SelectedItem;

                Pedido = await new Pedido().FindById(pedido.Idvenda, new Dictionary<string, string> {
                    {"itemvenda","true" }
                });

                DeliveryImprimir deliveryImprimir = new DeliveryImprimir();
                deliveryImprimir.CupomSelected += DeliveryImprimir_CupomSelected;
                deliveryImprimir.ShowDialog();
            }
        }

        private void DeliveryImprimir_CupomSelected(object sender, EventArgs e)
        {
            RelatoriosController.GerarCupomPedido(Pedido);
        }

        private async void buttonEnviarEntrega_Click(object sender, RoutedEventArgs e)
        {
            await EnviarEntrega();
        }

        public async Task EnviarEntrega()
        {
            if (DeliveryPedidos.datagridPedidos.SelectedItem != null)
            {
                Pedido pedido = (Pedido)DeliveryPedidos.datagridPedidos.SelectedItem;

                Pedido = await new Pedido().FindById(pedido.Idvenda, new Dictionary<string, string> {
                    {"itemvenda","true" }
                });

                if (Pedido.Identregador == null)
                {
                    PedidoDetailsEntregador detailsEntregador = new PedidoDetailsEntregador();
                    detailsEntregador.Selecionado += DetailsEntregador_Selecionado;
                    detailsEntregador.ShowDialog();
                }
                else
                {
                    Pedido.Status = 3;
                    await Pedido.UpdateInstance();
                    await DeliveryPedidos.LoadPedidos();
                    await DeliveryDetails.LoadPedido(Pedido.Idvenda);
                }
            }
        }

        private async void DetailsEntregador_Selecionado(object sender, EventArgs e)
        {
            int identregador = ((PedidoDetailsEntregador)sender).EntregadorSelecionado;
            Pedido.Identregador = identregador;
            await Pedido.UpdateInstance();
            await EnviarEntrega();
        }

        private async void buttonConcluir_Click(object sender, RoutedEventArgs e)
        {
            if (DeliveryPedidos.datagridPedidos.SelectedItem != null)
            {
                Pedido pedido = (Pedido)DeliveryPedidos.datagridPedidos.SelectedItem;

                Pedido = await new Pedido().FindById(pedido.Idvenda, new Dictionary<string, string> {
                    {"itemvenda","true" }
                });

                DeliveryVoltaEntregar deliveryVolta = new DeliveryVoltaEntregar();
                await deliveryVolta.LoadPedido(pedido.Idvenda);
                deliveryVolta.VoltaConfirmada += DeliveryVolta_VoltaConfirmada;
                deliveryVolta.ShowDialog();
            }
        }

        private async void DeliveryVolta_VoltaConfirmada(object sender, DeliveryVoltaEntregar.VoltaConfirmadaEventArgs e)
        {
            e.Pedido.Status = 4;
            await e.Pedido.UpdateInstance();
            await DeliveryPedidos.LoadPedidos();
            await DeliveryDetails.LoadPedido(e.Pedido.Idvenda);
            ItemsSelecionados.LimparVenda();
        }
    }
}
