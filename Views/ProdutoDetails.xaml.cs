using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ProdutoDetailsView.xaml
    /// </summary>
    public partial class ProdutoDetails : Window
    {
        public Item Item { get; set; }

        public class ItemTipo
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        public ProdutoDetails()
        {
            InitializeComponent();
            buttonAlterar.Visibility = Visibility.Collapsed;
            buttonRemover.Visibility = Visibility.Collapsed;

            List<ItemTipo> source = new List<ItemTipo> {
                new ItemTipo {Value = 1, Text = "Produto"},
                new ItemTipo {Value = 2, Text = "Pacote"}
            };
            comboboxTipo.ItemsSource = source;
            comboboxTipo.DisplayMemberPath = "Text";
            comboboxTipo.SelectedValuePath = "Value";

            Item = new Item
            {
                Tipo = 1,
                Visivel = 1,
                Estoque = 1,
                Disponivel = 1,
                Unidade = "UN",
                Fiscal = new Fiscal
                {
                    Ncm = null,
                    Cest = null,
                    Cfop = 5001,
                    CstIcms = 00,
                    AliquotaIcms = 10,
                    Origem = 0
                }
            };
            gridProdutoDetails.DataContext = Item;

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        public async Task TrocaTipo()
        {
            switch (Item.Tipo)
            {
                case 1:
                    TabPacote.Visibility = Visibility.Collapsed;
                    CheckboxControlarEstoque.IsEnabled = true;
                    break;
                case 2:
                    if(Item.Pacote == null)
                    {
                        Item.Pacote = new Pacote
                        {
                            Padrao = 1,
                            Quantidade = 2
                        };
                    }
                    CheckboxControlarEstoque.IsEnabled = false;
                    TabPacote.Visibility = Visibility.Visible;
                    CheckboxControlarEstoque.IsEnabled = false;
                    break;
            }
        }

        public async Task LoadItem(int id)
        {
            buttonAlterar.Visibility = Visibility.Visible;
            buttonRemover.Visibility = Visibility.Visible;
            buttonCriar.Visibility = Visibility.Collapsed;
            textboxEstoque.IsReadOnly = true;
            textboxCodigo.IsReadOnly = true;
            textboxCusto.IsReadOnly = true;
            comboboxTipo.IsReadOnly = true;
            Item _item = new Item();
            _item = await _item.FindById(id, new Dictionary<string, string> {
                {"grupos","true" },
                {"tipo","true" },
                {"estoqueatual","true" }
            });
            await LoadItem(_item);
        }

        public async Task LoadItem(Item item)
        {
            Item = item;
            gridProdutoDetails.DataContext = Item;

            try
            {
                if (Item.Estoque == 1)
                {
                    if(Item.EstoqueAtual != null)
                    {
                        textboxMargem.Text = ((1 - (Item.Valor / (Item.EstoqueAtual.Custo ?? default))) * 100).ToString();
                    }
                }
            }
            catch
            {

            }

            await TrocaTipo();
        }

        public async Task<bool> CreateProduto()
        {
            if (!string.IsNullOrEmpty(textboxEstoque.Text) & !string.IsNullOrEmpty(textboxCusto.Text))
            {
                Item.EstoqueAtual = new Estoque
                {
                    QuantidadeDisponivel = decimal.Parse(textboxEstoque.Text),
                    Custo = decimal.Parse(textboxCusto.Text, System.Globalization.NumberStyles.Currency)
                };
            }

            if (Item.Estoque == 1 & Item.EstoqueAtual != null & Item.Tipo == 1)
            {
                Estoque estoque = new Estoque
                {
                    Custo = Item.EstoqueAtual.Custo,
                    Quantidade = Item.EstoqueAtual.QuantidadeDisponivel,
                    QuantidadeDisponivel = Item.EstoqueAtual.QuantidadeDisponivel,
                    OrigemVenda = 0,
                    Saida = 0,
                    Disponivel = 1
                };

                Item.ItemHasEstoque.Add(new ItemHasEstoque
                {
                    IdestoqueNavigation = estoque
                });
            }

            if(Item.Tipo != 2)
            {
                Item.Pacote = null;
            }
            else
            {
                Item.Estoque = 1;
            }

            return await Item.SaveInstance();

        }

        public async Task<bool> UpdateProduto()
        {
            //await GetItemFromForm();
            return await Item.UpdateInstance();
        }

        private void ButtonGrupos_Click(object sender, RoutedEventArgs e)
        {
            ProdutoDetailsGrupos produtoDetailsGruposView = new ProdutoDetailsGrupos(Item.ItemHasGrupo.Select(e => e.IdgrupoNavigation).ToList());
            produtoDetailsGruposView.Closed += ProdutoDetailsGruposView_Closed;
            produtoDetailsGruposView.ShowDialog();
        }

        private void ProdutoDetailsGruposView_Closed(object sender, EventArgs e)
        {
            Item.ItemHasGrupo = new List<ItemHasGrupo>();
            foreach (var grupo in ((ProdutoDetailsGrupos)sender).SelectedGrupos)
            {
                Item.ItemHasGrupo.Add( new ItemHasGrupo {
                    Idgrupo = grupo.Idgrupo,
                    IdgrupoNavigation = grupo
                });
            }
            
        }

        private async void buttonCriar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await CreateProduto())
                {
                    Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void buttonRemover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Item.DeleteInstance();
            }
            catch (BadResponseStatusCodeException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            Close();
        }

        private async void buttonAlterar_Click(object sender, RoutedEventArgs e)
        {
            if( await UpdateProduto())
            {
                Close();
            }
        }

        private void ButtonSelecionarProduto_Click(object sender, RoutedEventArgs e)
        {
            ProdutoDetailsPacoteAdd produtoDetailsPacoteAdd = new ProdutoDetailsPacoteAdd();
            produtoDetailsPacoteAdd.ItemPacoteSelecionado += ProdutoDetailsPacoteAdd_ItemPacoteSelecionado;
            produtoDetailsPacoteAdd.ShowDialog();
        }

        private void ProdutoDetailsPacoteAdd_ItemPacoteSelecionado(object sender, ProdutoDetailsPacoteAdd.ItemPacoteSelecionadoEventArgs e)
        {
            if(Item.Pacote == null)
            {
                Item.Pacote = new Pacote
                {
                    IditemProduto = e.Item.Iditem,
                    IditemProdutoNavigation = e.Item,
                };
            }
            else
            {
                Item.Pacote.IditemProduto = e.Item.Iditem;
                Item.Pacote.IditemProdutoNavigation = e.Item;
            }

            Item.Fiscal = e.Item.Fiscal;

            textboxPacoteProduto.Text = Item.Pacote.IditemProdutoNavigation.Descricao;
        }

        private async void comboboxTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await TrocaTipo();
        }

        private void textboxMargem_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(textboxMargem.Text) & !string.IsNullOrEmpty(textboxCusto.Text))
            {
                try
                {
                    decimal custo = decimal.Parse(textboxCusto.Text, System.Globalization.NumberStyles.Currency);
                    decimal margem = decimal.Parse(textboxMargem.Text);
                    decimal valor = ((margem / 100) + 1) * custo;
                    textboxValor.Text = valor.ToString("C2");
                }
                catch
                {

                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
