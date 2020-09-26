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
    public partial class EstoqueView : Window
    {
        public List<Item> Items { get; set; }
        public Item ItemSelecionado { get; set; }
        public List<Estoque> Entradas { get; set; }
        public List<Estoque> Vendas { get; set; }
        public List<Estoque> Saidas { get; set; }

        public EstoqueView()
        {
            InitializeComponent();
        }

        private async void datagridItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            ItemSelecionado = (Item)dataGrid.SelectedItem;
            ItemSelecionado = await ItemSelecionado.ReloadInstance(new Dictionary<string, string> { { "estoque", "true" } });

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
            Items = await (new Item()).FindAll(new Dictionary<string, string> {
                { "estoqueatual","true" },
                { "estoqueonly","true"} }
            );
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = Items;
        }

        public async Task LoadEntradas(Item item)
        {
            Entradas = item.ItemHasEstoque.Select(e => e.IdestoqueNavigation).Where(e => e.Saida == 0).ToList();
            datagridEntradas.ItemsSource = null;
            datagridEntradas.ItemsSource = Entradas;
        }

        public async Task LoadVendas(Item item)
        {
            Vendas = item.ItemHasEstoque.Select(e => e.IdestoqueNavigation).Where(e => e.OrigemVenda == 1).ToList();
            datagridVendas.ItemsSource = null;
            datagridVendas.ItemsSource = Vendas;
        }

        public async Task LoadSaidas(Item item)
        {
            Saidas = item.ItemHasEstoque.Select(e => e.IdestoqueNavigation).Where(e => e.Saida == 1 & e.OrigemVenda == 0).ToList();
            datagridSaidas.ItemsSource = null;
            datagridSaidas.ItemsSource = Saidas;
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
