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
    /// Interaction logic for PedidoDetailsEntregador.xaml
    /// </summary>
    public partial class PedidoDetailsEntregador : Window
    {

        public int EntregadorSelecionado { get; set; }
        public event EventHandler Selecionado;

        public PedidoDetailsEntregador()
        {
            InitializeComponent();
        }

        private async Task LoadEntregadores()
        {
            List<Entregador> entregadores = await (new Entregador()).FindAll();
            datagridEntregadores.ItemsSource = null;
            datagridEntregadores.ItemsSource = entregadores.Where(e => e.Ativo == 1);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadEntregadores();
        }

        private void Entregador_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            EntregadorSelecionado = (int)senderAsButton.Tag;
            Selecionado?.Invoke(this, new EventArgs());
            Close();
        }
    }
}
