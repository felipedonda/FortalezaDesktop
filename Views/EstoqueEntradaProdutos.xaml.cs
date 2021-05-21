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
    /// Interaction logic for EntradaEstoqueProdutos.xaml
    /// </summary>
    public partial class EstoqueEntradaProdutos : Window
    {
        public Item ItemSelecionado { get; set; }
        public List<Item> Items { get; set; }
        public event EventHandler Selecionado;
        public bool Saida;

        public EstoqueEntradaProdutos(bool saida)
        {
            Saida = saida;
            InitializeComponent();
            if(saida)
            {
                datagridItems.Columns[2].Visibility = Visibility.Collapsed;
            }
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadItems();
        }

        public async Task LoadItems()
        {
            Item _item = new Item();
            Items = (await _item.FindAll( new Dictionary<string, string> { { "estoqueatual", "true" } }))
                .Where(e => e.Estoque == 1)
                .ToList();
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = Items;
        }

        public async Task SelecionarItem()
        {
            ItemSelecionado = (Item)datagridItems.SelectedItem;
            if(ItemSelecionado != null)
            {
                EstoqueEntradaProdutoQuantidade produtoQuantidade = new EstoqueEntradaProdutoQuantidade(ItemSelecionado, Saida);
                produtoQuantidade.Closed += ProdutoQuantidade_Closed;
                produtoQuantidade.ShowDialog();
            }
        }

        private void ProdutoQuantidade_Closed(object sender, EventArgs e)
        {
            decimal quantidade = ((EstoqueEntradaProdutoQuantidade)sender).Quantidade;
            if (quantidade > 0)
            {
                decimal custo = ((EstoqueEntradaProdutoQuantidade)sender).Custo;
                ItemSelecionado.EstoqueAtual = new Models.Estoque { QuantidadeDisponivel = quantidade, Custo = custo };
                Selecionado?.Invoke(this, new EventArgs());
                Close();
            }
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
