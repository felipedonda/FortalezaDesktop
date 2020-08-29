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
    /// Interaction logic for EntradaEstoqueView.xaml
    /// </summary>
    public partial class EstoqueEntrada : Window
    {
        public bool Saida { get; set; }
        public List<Item> Items { get; set; }

        public EstoqueEntrada(bool saida)
        {
            Items = new List<Item>();
            InitializeComponent();
        }

        private void buttonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            EstoqueEntradaProdutos entradaEstoqueProdutos = new EstoqueEntradaProdutos();
            entradaEstoqueProdutos.Selecionado += EntradaEstoqueProdutos_Selecionado;
            entradaEstoqueProdutos.Show();

        }

        private async void EntradaEstoqueProdutos_Selecionado(object sender, EventArgs e)
        {
            Items.Add(((EstoqueEntradaProdutos)sender).ItemSelecionado);
            await UpdateListaItems();
        }

        public async Task UpdateListaItems()
        {
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = Items;
        }

        private async void ButtonEntrada(object sender, RoutedEventArgs e)
        {
            foreach(Item item in Items)
            {
                if(Saida)
                {
                    await item.CreateEstoque(- item.EstoqueAtual.QuantidadeDisponivel);
                }
                else
                {
                    await item.CreateEstoque(item.EstoqueAtual.QuantidadeDisponivel, item.EstoqueAtual.Custo);
                }
            }
            Close();
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void buttonRemover_Click(object sender, RoutedEventArgs e)
        {
            Item item = (Item)datagridItems.SelectedItem;
            for(int i = 0; i < Items.Count; i++)
            {
                if(Items[i].Iditem == item.Iditem)
                {
                    Items.Remove(Items[i]);
                }
            }
            await UpdateListaItems();
        }
    }
}
