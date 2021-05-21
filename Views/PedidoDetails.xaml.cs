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
using System.Windows.Shapes;
using FortalezaDesktop.Utils;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for PedidoDetails.xaml
    /// </summary>
    public partial class PedidoDetails : Window
    {
        public Pedido Pedido { get; set; }

        public PedidoDetails()
        {
            InitializeComponent();
            NewPedido();
        }

        public PedidoDetails(int Idpedido)
        {
            InitializeComponent();
            LoadPedido(Idpedido);
        }

        public async void LoadPedido(int Idpedido)
        {
            Pedido = await (new Pedido()).FindById(Idpedido, new Dictionary<string, string> {
                {"itemVenda","true"}
            });
            if(Pedido != null)
            {
                Title = "Pedido Nº " + Pedido.NumeroPedido;
                ReloadPedido();
            }
            else
            {
                MessageBox.Show("Erro ao carregar pedido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Log("Erro ao tentar carregar pedido de ID " + Idpedido + ".", Logger.LogType.Error);
            }
        }

        public async void NewPedido()
        {
            Caixa caixaAberto = await new Caixa().GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
            Pedido = new Pedido
            {
                Delivery = 0,
                Entregue = 0,
                Status = 0,
                DataEntregue = DateTime.Now,
                DataPrazo = DateTime.Now,
                IdvendaNavigation = new Venda
                {
                    Tipo = 1,
                    Aberta = 1,
                    Idresponsavel = UserController.UsuarioLogado.Idusuario,
                    Paga = 0,
                    HoraEntrada = DateTime.Now,
                    Idcaixa = caixaAberto.Idcaixa,
                    Idpdv = UserPreferences.Preferences.Idpdv
                }
            };
            await Pedido.SaveInstance();
            Pedido = await Pedido.ReloadInstance(new Dictionary<string, string> {
                {"itemVenda","true"}
            });
            ReloadPedido();
        }

        public void ReloadPedido()
        {
            gridPedidoDetails.DataContext = null;
            gridPedidoDetails.DataContext = Pedido;
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = Pedido.IdvendaNavigation.ItemVenda;
        }

        private void ButtonAdicionarClick(object sender, RoutedEventArgs e)
        {
            PedidoDetailsProdutos pedidoDetailsProdutos = new PedidoDetailsProdutos();
            pedidoDetailsProdutos.ItemSelecionado += PedidoDetailsProdutos_Selecionado; ;
            pedidoDetailsProdutos.ShowDialog();
        }

        private async void PedidoDetailsProdutos_Selecionado(object sender, PedidoDetailsProdutos.ItemSelecionadoArgs e)
        {
            Item item = e.Item;
            decimal quantidade = e.Quantidade;
            try
            {
                quantidade = decimal.Parse(textboxQuantidade.Text);
            }
            catch
            {
                MessageBox.Show("Quantidade inválida.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ItemVenda itemVenda = new ItemVenda
            {
                Iditem = item.Iditem,
                Quantidade = quantidade,
                Valor = item.Valor ?? default,
            };

            await Pedido.IdvendaNavigation.SaveItemVenda(itemVenda);
            ReloadPedido();
        }

        private void ButtonSelecionarCliente_Click(object sender, RoutedEventArgs e)
        {
            PedidoDetailsCliente pedidoDetailsCliente = new PedidoDetailsCliente();
            pedidoDetailsCliente.Selecionado += PedidoDetailsCliente_Selecionado;
            pedidoDetailsCliente.ExigirEndereco = true;
            pedidoDetailsCliente.ShowDialog();
        }

        private async void PedidoDetailsCliente_Selecionado(object sender, PedidoDetailsCliente.ClienteSelecionadoEventArgs e)
        {
            Cliente cliente = ((PedidoDetailsCliente)sender).ClienteSelecionado;
            Pedido.IdvendaNavigation.Idcliente = cliente.Idcliente;
            await Pedido.IdvendaNavigation.UpdateInstance();
            LoadPedido(Pedido.Idvenda);
        }

        private void ButtonSelecionarEntregador_Click(object sender, RoutedEventArgs e)
        {
            PedidoDetailsEntregador produtoDetailsEntregador = new PedidoDetailsEntregador();
            produtoDetailsEntregador.Selecionado += ProdutoDetailsEntregador_Selecionado;
            produtoDetailsEntregador.ShowDialog();
        }

        private async void ProdutoDetailsEntregador_Selecionado(object sender, EventArgs e)
        {
            int identregador = ((PedidoDetailsEntregador)sender).EntregadorSelecionado;
            Pedido.Identregador = identregador;
            await Pedido.UpdateInstance();
            LoadPedido(Pedido.Idvenda);
        }

        private void ButtonConfirmar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAdiantamento_Click(object sender, RoutedEventArgs e)
        {
            if(Pedido.IdvendaNavigation.Idcliente != null)
            {
                PedidoDetailsPagamentos detailsPagamentos = new PedidoDetailsPagamentos(Pedido.IdvendaNavigation);
                detailsPagamentos.Closed += DetailsPagamentos_Closed;
                detailsPagamentos.ShowDialog();
            }
        }

        private void DetailsPagamentos_Closed(object sender, EventArgs e)
        {
            LoadPedido(Pedido.Idvenda);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
