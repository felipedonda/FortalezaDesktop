using FortalezaDesktop.Models;
using FortalezaDesktop.Utils;
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
    /// Interaction logic for ClienteDetails.xaml
    /// </summary>
    public partial class ClienteDetails : Window
    {
        public Cliente Cliente { get; set; }
        public ClienteDetails()
        {
            InitializeComponent();
            buttonAlterar.Visibility = Visibility.Collapsed;
            buttonRemover.Visibility = Visibility.Collapsed;
            Cliente cliente = new Cliente();
            LoadCliente(cliente);
        }

        public ClienteDetails(Cliente cliente)
        {
            InitializeComponent();
            buttonCriar.Visibility = Visibility.Collapsed;
            LoadCliente(cliente);
        }

        public async void LoadCliente(Cliente cliente)
        {
            if (cliente.Endereco == null)
            {
                await cliente.LoadEndereco();
            }

            gridCliente.DataContext = null;
            gridCliente.DataContext = cliente;
        }

        public async Task GetClientFromForm()
        {
            Cliente = (Cliente)gridCliente.DataContext;
        }

        private async void buttonConsultarCEP(object sender, RoutedEventArgs e)
        {
            await GetClientFromForm();
            try
            {
                Endereco endereco = await ServicoCEP.ConsultarCEP(textboxCep.Text);
                Cliente.Endereco = endereco;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            LoadCliente(Cliente);
        }

        private async void buttonCriar_Click(object sender, RoutedEventArgs e)
        {
            await GetClientFromForm();
            await Cliente.SaveInstance();
            Close();
        }

        private async void buttonAlterar_Click(object sender, RoutedEventArgs e)
        {
            await GetClientFromForm();
            await Cliente.UpdateInstance();
            Close();
        }

        private void buttonRemover_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
