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
                Visivel = true,
                Estoque = true,
                Disponivel = true,
                Unidade = "UN"
            };
            LoadItem(item);
        }

        public ProdutoDetails(Item item)
        {
            InitializeComponent();
            buttonCriar.Visibility = Visibility.Collapsed;
            textboxEstoque.IsReadOnly = true;
            textboxCodigo.IsReadOnly = true;
            textboxCusto.IsReadOnly = true;
            comboboxTipo.IsReadOnly = true;
            FillCombobox();
            LoadItem(item);
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

        public async void LoadItem(Item item)
        {
            Item = item;
            await Item.LoadGrupos();
            await Item.LoadEstoqueAtual();

            switch(Item.Tipo)
            {
                case "Pacote":
                    await Item.LoadPacote();
                    break;
            }

            gridProdutoDetails.DataContext = Item;

            for(int i =0; i < ((List<string>)comboboxTipo.ItemsSource).Count; i++)
            {
                if(((List<string>)comboboxTipo.ItemsSource)[i] == Item.Tipo)
                {
                    comboboxTipo.SelectedIndex = i;
                }
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
                if(Item.Pacote != null)
                {
                    Item.Pacote.Quantidade = decimal.Parse(textboxQuantidade.Text);
                }
            }
        }

        public async void CreateProduto()
        {
            await GetItemFromForm();
            try
            {
                await Item.SaveInstance();
            }
            catch(BadResponseStatusCodeException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Item.Estoque & Item.EstoqueAtual != null & Item.Tipo == "Produto")
            {
                try
                {
                    await Item.CreateEstoque(Item.EstoqueAtual.QuantidadeDisponivel, Item.EstoqueAtual.Custo);
                }
                catch (BadResponseStatusCodeException ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            Close();
        }

        public async void UpdateProduto()
        {
            await GetItemFromForm();
            try
            {
                await Item.UpdateInstance();
            }
            catch (BadResponseStatusCodeException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            Close();
        }

        private void ButtonGrupos_Click(object sender, RoutedEventArgs e)
        {
            ProdutoDetailsGrupos produtoDetailsGruposView = new ProdutoDetailsGrupos(Item.Grupos);
            produtoDetailsGruposView.Closed += ProdutoDetailsGruposView_Closed;
            produtoDetailsGruposView.Show();
        }

        private void ProdutoDetailsGruposView_Closed(object sender, EventArgs e)
        {

            Item.Grupos = ((ProdutoDetailsGrupos)sender).SelectedGrupos;
        }

        private void buttonCriar_Click(object sender, RoutedEventArgs e)
        {
            CreateProduto();
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void buttonRemover_Click(object sender, RoutedEventArgs e)
        {
            Item item = (Item)gridProdutoDetails.DataContext;
            try
            {
                await Item.DeleteItem(item.Iditem ?? default);
            }
            catch (BadResponseStatusCodeException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            Close();
        }

        private void buttonAlterar_Click(object sender, RoutedEventArgs e)
        {
            UpdateProduto();
        }

        private void updateValorMargem(object sender, TextChangedEventArgs e)
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

        private void ButtonSelecionarProduto_Click(object sender, RoutedEventArgs e)
        {
            ProdutoDetailsPacoteAdd produtoDetailsPacoteAdd = new ProdutoDetailsPacoteAdd();
            produtoDetailsPacoteAdd.Closed += ProdutoDetailsPacoteAdd_Closed;
            produtoDetailsPacoteAdd.Show();
        }

        private void ProdutoDetailsPacoteAdd_Closed(object sender, EventArgs e)
        {
            Item.Pacote = new Pacote
            {
                ItemPacote = ((ProdutoDetailsPacoteAdd)sender).ItemSelecionado
            };
            textboxPacoteProduto.Text = Item.Pacote.ItemPacote.Descricao;

        }

        private async void comboboxTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item.Tipo = ((ComboBox)sender).SelectedItem.ToString();
            await TrocaTipo();
        }
    }
}
