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
    /// Interaction logic for PedidoDetailsProdutos.xaml
    /// </summary>
    public partial class PedidoDetailsProdutos : Window
    {
        public Item ItemSelecionado { get; set; }
        public event EventHandler Selecionado;

        public PedidoDetailsProdutos()
        {
            InitializeComponent();
        }

        private async void ButtonAdicionar(object sender, RoutedEventArgs e)
        {
            await SelecionarItem();
        }

        private async Task LoadItems()
        {
            List<Item> items = await (new Item()).FindAll(new Dictionary<string, string> {
                {"estoqueAtual","true" }
            });
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = items;
        }

        private async Task SelecionarItem()
        {
            ItemSelecionado = (Item)datagridItems.SelectedItem;
            if(ItemSelecionado != null)
            {
                Selecionado?.Invoke(this, new EventArgs());
                Close();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadItems();
        }

        private async void datagridItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await SelecionarItem();
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
