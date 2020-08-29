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
    /// Interaction logic for EstoqueView.xaml
    /// </summary>
    public partial class Estoque : Window
    {
        public List<Item> Items { get; set; }
        public Item ItemSelecionado { get; set; }
        public List<Models.Estoque> Entradas { get; set; }
        public List<Models.Estoque> Vendas { get; set; }
        public List<Models.Estoque> Saidas { get; set; }

        public Estoque()
        {
            InitializeComponent();
        }

        private async void datagridItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            ItemSelecionado = (Item)dataGrid.SelectedItem;
            await ItemSelecionado.LoadEstoques();

            gridItemSelecionado.DataContext = null;
            gridItemSelecionado.DataContext = ItemSelecionado;
            textboxDataFinal.Text = DateTime.UtcNow.ToString("dd/MM/yyyy");
            
            await LoadEntradas(ItemSelecionado);
            await LoadSaidas(ItemSelecionado);
            await LoadVendas(ItemSelecionado);
            tabcontrol.SelectedIndex = 1;
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadItems();
        }

        public async Task LoadItems()
        {
            Items = (await Item.GetItems()).Where(e => e.Estoque).ToList();
            foreach(var Item in Items)
            {
                await Item.LoadEstoqueAtual();
            }
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = Items;
        }

        public async Task LoadEntradas(Item item)
        {
            Entradas = item.Estoques.Where(e => e.Saida == false).ToList();
            datagridEntradas.ItemsSource = null;
            datagridEntradas.ItemsSource = Entradas;
        }

        public async Task LoadVendas(Item item)
        {
            Vendas = item.Estoques.Where(e => e.OrigemVenda == true).ToList();
            datagridVendas.ItemsSource = null;
            datagridVendas.ItemsSource = Vendas;
        }

        public async Task LoadSaidas(Item item)
        {
            Saidas = item.Estoques.Where(e => e.Saida == true & e.OrigemVenda == false).ToList();
            datagridVendas.ItemsSource = null;
            datagridVendas.ItemsSource = Saidas;
        }

        private void ButtonEntrada(object sender, RoutedEventArgs e)
        {
            EstoqueEntrada entradaEstoqueView = new EstoqueEntrada(false);
            entradaEstoqueView.Closed += EntradaEstoqueView_Closed;
            entradaEstoqueView.Show();
        }

        private async void EntradaEstoqueView_Closed(object sender, EventArgs e)
        {
            await LoadItems();
        }

        private void ButtonSaida(object sender, RoutedEventArgs e)
        {
            EstoqueEntrada entradaEstoqueView = new EstoqueEntrada(true);
            entradaEstoqueView.Closed += EntradaEstoqueView_Closed;
            entradaEstoqueView.Show();
        }

        private void ButtonInventario(object sender, RoutedEventArgs e)
        {

        }
    }
}
