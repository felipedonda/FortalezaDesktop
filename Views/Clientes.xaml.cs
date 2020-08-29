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

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for Clientes.xaml
    /// </summary>
    public partial class Clientes : Window
    {
        public Clientes()
        {
            InitializeComponent();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadClientes();
        }

        public async Task LoadClientes()
        {
            List<Cliente> clientes = await Cliente.GetClientes();
            datagridClientes.ItemsSource = null;
            datagridClientes.ItemsSource = clientes;
        }

        private async void buttonEditCliente_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            Cliente cliente = await Cliente.GetCliente((int)senderAsButton.Tag);
            ClienteDetails clienteDetails = new ClienteDetails(cliente);
            clienteDetails.Show();
        }

        private void buttonRemoverCliente_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            ClienteDetails clienteDetails = new ClienteDetails();
            clienteDetails.Show();
        }
    }
}
