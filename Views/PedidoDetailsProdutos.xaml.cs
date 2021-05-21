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
    /// Interaction logic for PedidoDetailsProdutos.xaml
    /// </summary>
    public partial class PedidoDetailsProdutos : Window
    {
        
        public event EventHandler<ItemSelecionadoArgs> ItemSelecionado;

        public class ItemSelecionadoArgs : EventArgs
        {
            public Item Item { get; set; }
            public decimal Quantidade { get; set; }
            public ItemSelecionadoArgs(Item item, decimal quantidade)
            {
                Item = item;
                Quantidade = quantidade;
            }
        }

        public bool ExigirQuantidade { get; set; }

        public PedidoDetailsProdutos()
        {
            InitializeComponent();
            ExigirQuantidade = false;
        }

        private void ButtonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarItem();
        }

        private async Task LoadGrupos()
        {
            List<Grupo> grupos = await new Grupo().FindAll();
            if (grupos != null)
            {
                grupos.Insert(0, new Grupo
                    {
                        Visivel = 1,
                        Nome = "Todos",
                        Idgrupo = -1
                    });
                comboboxBuscaGrupo.ItemsSource = grupos.Where(e => e.Visivel == 1);
                comboboxBuscaGrupo.DisplayMemberPath = "Nome";
                comboboxBuscaGrupo.SelectedValuePath = "Idgrupo";
                comboboxBuscaGrupo.SelectedIndex = 0;
            }
        }

        private async Task LoadItems()
        {
            List<Item> items = await new Item().FindAll();
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = items;
        }

        public async Task LoadItems(string query)
        {
            List<Item> items = await new Item().FindAll(new Dictionary<string, string> {
                {"query",query }
            });
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = items;
        }

        private void SelecionarItem()
        {
            if (ExigirQuantidade)
            {
                VendaCancelarItemInput input = new VendaCancelarItemInput(true, true);
                input.NumeroInserido += Input_NumeroInserido;
            }
            else
            {
                SelecionarItem(1);
            }
        }

        private void SelecionarItem(decimal quantidade)
        {
            Item item = (Item)datagridItems.SelectedItem;
            if (item != null)
            {
                ItemSelecionado?.Invoke(this, new ItemSelecionadoArgs(item, quantidade));
                Close();
            }
        }

        private void Input_NumeroInserido(object sender, VendaCancelarItemInput.NumeroInseridoEventArgs e)
        {
            SelecionarItem(e.Quantidade);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadGrupos();
            if(string.IsNullOrEmpty(textboxBuscaDescricao.Text))
            {
                await LoadItems();
            }
            else
            {
                await LoadItems(textboxBuscaDescricao.Text);
            }
            textboxBuscaDescricao.Focus();
            textboxBuscaDescricao.SelectAll();
        }

        private async void datagridItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarItem();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void textboxBuscaDescricao_TextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadItems(textboxBuscaDescricao.Text);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Escape || e.Key == Key.F11)
            {
                Close();
            }

            if (e.Key == Key.Return || e.Key == Key.F12)
            {
                if (datagridItems.SelectedItem == null)
                {
                    datagridItems.SelectedIndex = 0;
                }
                SelecionarItem();
            }

            if(e.Key == Key.Tab)
            {
                if(datagridItems.IsFocused)
                {
                    if (datagridItems.SelectedIndex < (datagridItems.Items.Count - 1))
                    {
                        datagridItems.SelectedIndex++;
                    }
                    else
                    {
                        textboxBuscaDescricao.Focus();
                    }
                    e.Handled = true;
                }
            }
        }

        private void datagridItems_GotFocus(object sender, RoutedEventArgs e)
        {
            datagridItems.SelectedIndex = 0;
        }

        private void textboxBuscaDescricao_GotFocus(object sender, RoutedEventArgs e)
        {
            textboxBuscaDescricao.SelectAll();
        }
    }
}
