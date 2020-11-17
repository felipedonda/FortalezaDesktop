using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for PedidoDetailsCliente.xaml
    /// </summary>
    public partial class PedidoDetailsCliente : Window
    {
        public Cliente ClienteSelecionado { get; set; }
        public event EventHandler<ClienteSelecionadoEventArgs> Selecionado;

        public class ClienteSelecionadoEventArgs : EventArgs
        {
            public Cliente Cliente {get;set;}
            public ClienteSelecionadoEventArgs(Cliente cliente)
            {
                Cliente = cliente;
            }
        }

        public PedidoDetailsCliente()
        {
            InitializeComponent();
        }

        private async void ButtonAdicionar(object sender, RoutedEventArgs e)
        {
            await SelecionarCliente();
        }

        private async Task LoadClientes()
        {
            List<Cliente> clientes = await (new Cliente()).FindAll();
            datagridClientes.ItemsSource = null;
            datagridClientes.ItemsSource = clientes.Where(e => e.IdenderecoNavigation != null);
        }

        private async Task SelecionarCliente()
        {
            ClienteSelecionado = (Cliente)datagridClientes.SelectedItem;
            if (ClienteSelecionado != null)
            {
                Selecionado?.Invoke(this, new ClienteSelecionadoEventArgs(ClienteSelecionado));
                Close();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadClientes();
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void datagridClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await SelecionarCliente();
        }

        private void ButtonNovoCliente_Click(object sender, RoutedEventArgs e)
        {
            ClienteDetails clienteDetails = new ClienteDetails();
            clienteDetails.AlteracaoRealizada += ClienteDetails_AlteracaoRealizada;
            clienteDetails.ExigirEndereco = true;
            clienteDetails.Show();
        }

        private async void ClienteDetails_AlteracaoRealizada(object sender, EventArgs e)
        {
            await LoadClientes();
        }
    }
}
