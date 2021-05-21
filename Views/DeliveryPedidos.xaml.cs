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
    /// Interaction logic for DeliveryPedidos.xaml
    /// </summary>
    public partial class DeliveryPedidos : Page
    {
        public event EventHandler<PedidoSelectedEventArgs> PedidoSelected;

        public event EventHandler<PedidoSelectedEventArgs> PedidoHighlighted;

        public class PedidoSelectedEventArgs : EventArgs
        {
            public int Idvenda { get; set; }
            public PedidoSelectedEventArgs(int idvenda)
            {
                Idvenda = idvenda;
            }
        }

        public DeliveryPedidos()
        {
            InitializeComponent();

            DateTime dataInicial = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                0, 0, 0);

            DateTime dataFinal = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                23, 59, 59);

            textboxDataInicial.Text = dataInicial.ToString("dd/MM/yy");
            textboxHoraInicial.Text = dataInicial.ToString("hh:mm:ss");
            textboxDataFinal.Text = dataFinal.ToString("dd/MM/yy");
            textboxHoraFinal.Text = dataFinal.ToString("hh:mm:ss");
        }

        private void datagridPedidos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Pedido pedido = (Pedido)datagridPedidos.SelectedItem;
            PedidoSelected?.Invoke(this, new PedidoSelectedEventArgs(pedido.Idvenda));
        }

        private void datagridPedidos_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Pedido pedido = (Pedido)datagridPedidos.SelectedItem;
            if(pedido != null)
            {
                PedidoHighlighted?.Invoke(this, new PedidoSelectedEventArgs(pedido.Idvenda));
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPedidos();
        }

        public async Task LoadPedidos()
        {
            var pedidos = await new Pedido().FindAll(new Dictionary<string, string> {
                {"delivery","true"}
            });
            datagridPedidos.ItemsSource = null;
            datagridPedidos.ItemsSource = pedidos;
        }
    }
}
