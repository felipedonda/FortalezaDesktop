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
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for DeliveryVoltaEntregar.xaml
    /// </summary>
    public partial class DeliveryVoltaEntregar : Window
    {
        public class VoltaConfirmadaEventArgs : EventArgs
        {
            public Pedido Pedido { get; set; }

            public VoltaConfirmadaEventArgs(Pedido pedido)
            {
                Pedido = pedido;
            }

        }
        public event EventHandler<VoltaConfirmadaEventArgs> VoltaConfirmada;
        public Pedido Pedido;

        public DeliveryVoltaEntregar()
        {
            InitializeComponent();
        }

        public async Task LoadPedido(int idvenda)
        {
            Pedido = await new Pedido().FindById(idvenda);
            Pedido.IdvendaNavigation = await Pedido.IdvendaNavigation.ReloadInstance(new Dictionary<string, string> {
                {"pagamentos","true" }
            });
            GridPrincipal.DataContext = null;
            GridPrincipal.DataContext = Pedido.IdvendaNavigation;
        }

        private async void ButtonAlterarPagamentos_Click(object sender, RoutedEventArgs e)
        {
            VendaPagamentos vendaPagamentos = new VendaPagamentos(false, true);
            await vendaPagamentos.LoadVenda(Pedido.Idvenda);
            vendaPagamentos.PagamentoRealizado += VendaPagamentos_PagamentoRealizado;
            vendaPagamentos.ShowDialog();
        }

        private async void VendaPagamentos_PagamentoRealizado(object sender, EventArgs e)
        {
            await LoadPedido(Pedido.Idvenda);
        }

        private async void ButtonConfirmar_Click(object sender, RoutedEventArgs e)
        {
            VoltaConfirmada?.Invoke(this, new VoltaConfirmadaEventArgs(Pedido));
            Close();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
