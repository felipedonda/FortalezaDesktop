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
        public bool ExigirEndereco { get; set; }

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
            ExigirEndereco = false;
        }

        private async void ButtonAdicionar(object sender, RoutedEventArgs e)
        {
            await SelecionarCliente();
        }

        private async Task LoadClientes()
        {
            List<Cliente> clientes = await (new Cliente()).FindAll();
            datagridClientes.ItemsSource = null;
            if(ExigirEndereco)
            {
                datagridClientes.ItemsSource = clientes.Where(e => e.IdenderecoNavigation != null);
            }
            else
            {
                datagridClientes.ItemsSource = clientes;
            }
        }

        private async Task LoadClientes(string query)
        {
            List<Cliente> clientes = await new Cliente().FindAll(new Dictionary<string, string> {
                {"query",query}
            });
            datagridClientes.ItemsSource = null;
            datagridClientes.ItemsSource = clientes;
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
            clienteDetails.ExigirEndereco = ExigirEndereco;
            clienteDetails.ShowDialog();
        }

        private async void ClienteDetails_AlteracaoRealizada(object sender, EventArgs e)
        {
            await LoadClientes();
        }

        private async void textboxBuscaDescricao_TextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadClientes(textboxBuscaDescricao.Text);
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape || e.Key == Key.F11)
            {
                Close();
            }

            if (e.Key == Key.Return || e.Key == Key.F12)
            {
                if (datagridClientes.SelectedItem == null)
                {
                    datagridClientes.SelectedIndex = 0;
                }
                await SelecionarCliente();
            }
        }
    }
}
