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
    /// Interaction logic for ProdutoDetailsPacoteAdd.xaml
    /// </summary>
    public partial class ProdutoDetailsPacoteAdd : Window
    {
        public Item ItemSelecionado { get; set; }
        public List<Item> Items { get; set; }
        public event EventHandler Selecionado;

        public ProdutoDetailsPacoteAdd()
        {
            InitializeComponent();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadItems();
        }

        public async Task LoadItems()
        {
            Items = (await Item.GetItems()).Where(e => e.Tipo == "Produto").ToList();
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = Items;
        }

        public async Task SelecionarItem()
        {
            ItemSelecionado = (Item)datagridItems.SelectedItem;
            Close();
        }

        private async void ButtonAdicionar(object sender, RoutedEventArgs e)
        {
            await SelecionarItem();
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void datagridItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await SelecionarItem();
        }
    }
}
