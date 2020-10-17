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

        public ProdutoDetails()
        {
            InitializeComponent();
            buttonAlterar.Visibility = Visibility.Collapsed;
            buttonRemover.Visibility = Visibility.Collapsed;
            FillCombobox();
            Item item = new Item
            {
                Tipo = "Produto",
                Visivel = 1,
                Estoque = 1,
                Disponivel = 1,
                Unidade = "UN"
            };
            LoadItem(item);
        }

        public ProdutoDetails(int id)
        {
            InitializeComponent();
            buttonCriar.Visibility = Visibility.Collapsed;
            textboxEstoque.IsReadOnly = true;
            textboxCodigo.IsReadOnly = true;
            textboxCusto.IsReadOnly = true;
            comboboxTipo.IsReadOnly = true;
            FillCombobox();
            LoadItem(id);
        }

        public async void FillCombobox()
        {
            List<string> source = new List<string>();
            source.Add("Produto");
            source.Add("Pacote");
            comboboxTipo.ItemsSource = source;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        public async Task TrocaTipo()
        {
            switch (Item.Tipo)
            {
                case "Produto":
                    tabPacote.Visibility = Visibility.Collapsed;
                    gridPacote.Visibility = Visibility.Collapsed;
                    break;
                case "Pacote":
                    tabPacote.Visibility = Visibility.Visible;
                    gridPacote.Visibility = Visibility.Visible;
                    break;
            }
        }

        public async void LoadItem(int id)
        {
            Item _item = new Item();
            _item = await _item.FindById(id, new Dictionary<string, string> {
                {"grupos","true" },
                {"tipo","true" },
                {"estoqueatual","true" }
            });
            LoadItem(_item);
        }

        public async void LoadItem(Item item)
        {
            Item = item;

            gridProdutoDetails.DataContext = Item;

            for(int i =0; i < ((List<string>)comboboxTipo.ItemsSource).Count; i++)
            {
                if(((List<string>)comboboxTipo.ItemsSource)[i] == Item.Tipo)
                {
                    comboboxTipo.SelectedIndex = i;
                }
            }

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

        public async Task GetItemFromForm()
        {
            Item = (Item)gridProdutoDetails.DataContext;
            Item.Valor = decimal.Parse(textboxValor.Text, System.Globalization.NumberStyles.Currency);
            Item.Tipo = (string)comboboxTipo.SelectedItem;
            if (!string.IsNullOrEmpty(textboxEstoque.Text) & !string.IsNullOrEmpty(textboxCusto.Text))
            {
                Item.EstoqueAtual = new Models.Estoque
                {
                    QuantidadeDisponivel = decimal.Parse(textboxEstoque.Text),
                    Custo = decimal.Parse(textboxCusto.Text, System.Globalization.NumberStyles.Currency)
                };
            }
            
            if(Item.Tipo == "Pacote")
            {
                if(Item.PacoteIditemNavigation != null)
                {
                    Item.PacoteIditemNavigation.Quantidade = decimal.Parse(textboxQuantidade.Text);
                }
            }
        }

        public async Task<bool> CreateProduto()
        {
            await GetItemFromForm();

            if (Item.Estoque == 1 & Item.EstoqueAtual != null & Item.Tipo == "Produto")
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

            return await Item.SaveInstance();

        }

        public async Task<bool> UpdateProduto()
        {
            await GetItemFromForm();
            return await Item.UpdateInstance();
        }

        private void ButtonGrupos_Click(object sender, RoutedEventArgs e)
        {
            ProdutoDetailsGrupos produtoDetailsGruposView = new ProdutoDetailsGrupos(Item.ItemHasGrupo.Select(e => e.IdgrupoNavigation).ToList());
            produtoDetailsGruposView.Closed += ProdutoDetailsGruposView_Closed;
            produtoDetailsGruposView.Show();
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
            if(await CreateProduto())
            {
                Close();
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
            produtoDetailsPacoteAdd.Show();
        }

        private void ProdutoDetailsPacoteAdd_ItemPacoteSelecionado(object sender, ProdutoDetailsPacoteAdd.ItemPacoteSelecionadoEventArgs e)
        {
            if(Item.PacoteIditemNavigation == null)
            {
                Item.PacoteIditemNavigation = new Pacote
                {
                    IditemProduto = e.Item.Iditem,
                    IditemProdutoNavigation = e.Item
                };
            }
            else
            {
                Item.PacoteIditemNavigation.IditemProduto = e.Item.Iditem;
                Item.PacoteIditemNavigation.IditemProdutoNavigation = e.Item;
            }

            textboxPacoteProduto.Text = Item.PacoteIditemNavigation.IditemProdutoNavigation.Descricao;
        }

        private async void comboboxTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item.Tipo = ((ComboBox)sender).SelectedItem.ToString();
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
    }
}
