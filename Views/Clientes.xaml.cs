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
            Cliente cliente = new Cliente();
            List<Cliente> clientes = await cliente.FindAll();
            datagridClientes.ItemsSource = null;
            datagridClientes.ItemsSource = clientes;
        }

        private async void buttonEditCliente_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            ClienteDetails clienteDetails = new ClienteDetails((int)senderAsButton.Tag);
            clienteDetails.Closed += ClienteDetails_Closed;
            clienteDetails.Show();
        }

        private async void ClienteDetails_Closed(object sender, EventArgs e)
        {
            await LoadClientes();
        }

        private async void buttonRemoverCliente_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            Button senderAsButton = (Button)sender;
            cliente = await cliente.FindById((int)senderAsButton.Tag);
            var result = MessageBox.Show(
                "Deseja realmente remover o cliente " + cliente.Nome + "?",
                "Remover Cliente",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await cliente.DeleteInstance();
                await LoadClientes();
            }
        }

        private void buttonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            ClienteDetails clienteDetails = new ClienteDetails();
            clienteDetails.Closed += ClienteDetails_Closed;
            clienteDetails.Show();
        }
    }
}
