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
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for ProdutosView.xaml
    /// </summary>
    public partial class Produtos : Window
    {
        public Produtos()
        {
            InitializeComponent();
        }

        public async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadProdutos();
        }

        public async Task LoadProdutos()
        {
            Item _item = new Item();
            List<Item> items = await _item.FindAll(new Dictionary<string, string> {
                {"visibleOnly","false" }
            });
            datagridProdutos.ItemsSource = null;
            datagridProdutos.ItemsSource = items;
        }

        public async void buttonRemoverProduto_Click(object sender, RoutedEventArgs e)
        {
            Item item = new Item();
            Button senderAsButton = (Button)sender;
            item = await item.FindById((int)senderAsButton.Tag);
            MessageBoxResult result = MessageBox.Show(
                "Deseja realmente remover o " + item.Tipo.ToLower() + ": " + item.Descricao + "?",
                "Remover " + item.Tipo.ToLower(),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                try
                {
                    await item.DeleteInstance();
                    await LoadProdutos();
                }
                catch (BadResponseStatusCodeException ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
        }

        public async void buttonEditProduto_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            ProdutoDetails produtoDetailsView = new ProdutoDetails((int)senderAsButton.Tag);
            produtoDetailsView.Closing += ProdutoDetailsView_Closing;
            produtoDetailsView.Show();
        }

        private async void ProdutoDetailsView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await LoadProdutos();
        }

        private void buttonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            ProdutoDetails produtoDetailsView = new ProdutoDetails();
            produtoDetailsView.Closing += ProdutoDetailsView_Closing;
            produtoDetailsView.Show();
        }
    }
}
