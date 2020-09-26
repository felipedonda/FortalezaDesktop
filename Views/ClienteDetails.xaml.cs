using FortalezaDesktop.Models;
using FortalezaDesktop.Utils;
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
    /// Interaction logic for ClienteDetails.xaml
    /// </summary>
    public partial class ClienteDetails : Window
    {
        public Cliente Cliente { get; set; }
        public ClienteDetails()
        {
            InitializeComponent();
            textblockErroCpf.Visibility = Visibility.Hidden;

            buttonAlterar.Visibility = Visibility.Collapsed;
            buttonRemover.Visibility = Visibility.Collapsed;
            Cliente cliente = new Cliente
            {
                IdenderecoNavigation = new Endereco()
            };
            LoadCliente(cliente);
        }

        public ClienteDetails(int id)
        {
            InitializeComponent();
            textblockErroCpf.Visibility = Visibility.Hidden;

            buttonCriar.Visibility = Visibility.Collapsed;
            LoadCliente(id);
        }

        public async void LoadCliente(int idcliente)
        {
            LoadCliente(await new Cliente().FindById(idcliente));
        }

        public async void LoadCliente(Cliente cliente)
        {
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
                Cliente.IdenderecoNavigation = await ServicoCEP.ConsultarCEP(textboxCep.Text, Cliente.IdenderecoNavigation);
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
            bool validated = true;

            textblockErroCpf.Visibility = Visibility.Hidden;
            
            if(Cliente.Cpf.Length != 11 & Cliente.Cpf.Length != 14)
            {
                validated = false;
                textblockErroCpf.Text = "CPF ou CNPJ inválido.";
                textblockErroCpf.Visibility = Visibility.Visible;
            }

            if(validated)
            {
                await Cliente.SaveInstance();
                Close();
            }
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
