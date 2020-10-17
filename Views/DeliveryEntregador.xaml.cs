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
    /// Interaction logic for DeliveryEntregador.xaml
    /// </summary>
    public partial class DeliveryEntregador : Window
    {
        public Pedido Pedido { get; set; }
        public class SalvarEventArgs : EventArgs
        {
            public Pedido Pedido;

            public SalvarEventArgs(Pedido pedido)
            {
                Pedido = pedido;
            }
        }
        public event EventHandler<SalvarEventArgs> Salvar;

        public DeliveryEntregador()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }


        public async Task LoadData()
        {
            await LoadTipoEntregador();
            await LoadEntregador();
            await LoadMetodos();
        }

            private async Task LoadTipoEntregador()
        {
            List<TipoEntregador> tipoEntregadores = await new TipoEntregador().FindAll();
            comboboxTipo.ItemsSource = null;
            comboboxTipo.ItemsSource = tipoEntregadores;
            comboboxTipo.DisplayMemberPath = "Nome";
            comboboxTipo.SelectedValuePath = "IdtipoEntregador";

        }

        private async Task LoadEntregador()
        {
            List<Entregador> tipoEntregadores = await new Entregador().FindAll();
            tipoEntregadores.Insert(0, new Entregador {
                Nome = "",
                Identregador = -1
            });
            comboboxEntregador.ItemsSource = null;
            comboboxEntregador.ItemsSource = tipoEntregadores;
            comboboxEntregador.DisplayMemberPath = "Nome";
            comboboxEntregador.SelectedValuePath = "Identregador";

        }

        private async Task LoadMetodos()
        {
            List<Metodo> metodos = await new Metodo().FindAll();
            comboboxMetodoEntrega.ItemsSource = null;
            comboboxMetodoEntrega.ItemsSource = metodos;
            comboboxMetodoEntrega.DisplayMemberPath = "Nome";
            comboboxMetodoEntrega.SelectedValuePath = "Idmetodo";

        }

        public async Task LoadPedido(int idvenda)
        {
            Pedido = await new Pedido().FindById(idvenda, new Dictionary<string, string> {
                    {"itemvenda","true" }
                });
            gridPrimario.DataContext = null;
            gridPrimario.DataContext = Pedido;
      
            if(Pedido.Identregador == null)
                comboboxEntregador.SelectedIndex = 0;

            if (Pedido.Idmetodo == null)
                comboboxMetodoEntrega.SelectedIndex = 0;

            if (Pedido.IdtipoEntregador == null)
                comboboxTipo.SelectedIndex = 0;

            if (Pedido.TaxaEntrega == null)
            {
                if (Pedido.IdvendaNavigation.IdclienteNavigation != null)
                {
                    await Pedido.LoadTaxaAplicavel();
                    await LoadPedido(idvenda);
                }
            }
            
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            if(Pedido.Identregador == -1)
            {
                Pedido.Identregador = null;
            }
            Salvar?.Invoke(this, new SalvarEventArgs(Pedido));
            Close();
        }
    }
}
