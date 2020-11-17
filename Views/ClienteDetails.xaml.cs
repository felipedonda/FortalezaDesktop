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
        public event EventHandler AlteracaoRealizada;

        public Cliente Cliente { get; set; }

        public bool ExigirEndereco { get; set; }

        public ClienteDetails()
        {
            InitializeComponent();
            
            textblockErroCpf.Visibility = Visibility.Hidden;
            TextblockLogradouroErro.Visibility = Visibility.Hidden;
            TextblockNumeroErro.Visibility = Visibility.Hidden;

            buttonAlterar.Visibility = Visibility.Collapsed;
            buttonRemover.Visibility = Visibility.Collapsed;


            ExigirEndereco = false;

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

        public async Task<bool> ValidateCliente()
        {
            textblockErroCpf.Visibility = Visibility.Hidden;
            bool validated = true;

            if (Cliente.Cpf != null)
            {
                if (Cliente.Cpf.Length != 11 & Cliente.Cpf.Length != 14)
                {
                    textblockErroCpf.Text = "CPF ou CNPJ inválido.";
                    textblockErroCpf.Visibility = Visibility.Visible;
                    validated = false;
                }
            }

            if (ExigirEndereco)
            {
                if(string.IsNullOrEmpty(TextboxLogradouro.Text))
                {
                    TextblockLogradouroErro.Text = "Logradouro vazio.";
                    TextblockLogradouroErro.Visibility = Visibility.Visible;
                    validated = false;
                }

                if (string.IsNullOrEmpty(TextboxNumero.Text))
                {
                    TextblockNumeroErro.Text = "Número vazio.";
                    TextblockNumeroErro.Visibility = Visibility.Visible;
                    validated = false;
                }
            }

            return validated;
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

            if(await ValidateCliente())
            {
                await Cliente.SaveInstance();
                AlteracaoRealizada?.Invoke(sender, new EventArgs());
                Close();
            }
        }

        private async void buttonAlterar_Click(object sender, RoutedEventArgs e)
        {
            await GetClientFromForm();
            if(await ValidateCliente())
            {
                if (await Cliente.UpdateInstance())
                {
                    AlteracaoRealizada?.Invoke(sender, new EventArgs());
                    Close();
                }
            }
        }

        private async void buttonRemover_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Deseja realmente remover o cliente " + Cliente.Nome + "?",
                "Remover Cliente",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await Cliente.DeleteInstance();
                Close();
            }
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
