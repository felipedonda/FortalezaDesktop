using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for VendaView.xaml
    /// </summary>
    public partial class VendaView : Page
    {
        public VendaItemsSelecionados ItemsSelecionados { get; set; }

        public VendaView()
        {
            InitializeComponent();
        }

        public async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserPreferences.Load();

            if (UserPreferences.Preferences.ModoCaixa)
            {
                gridColumn0.Width = new GridLength(0);
            }
            else
            {
                LoadSelecaoProdutos();
            }

            LoadItemsSelecionados();

            int vendaAberta = await new Venda().GetVendaAberta();
            if(vendaAberta > 0)
            {
                await ItemsSelecionados.LoadVenda(vendaAberta);
            }
        }

        public async void LoadSelecaoProdutos()
        {
            VendaSelecaoProdutos selecaoProdutos = new VendaSelecaoProdutos();
            selecaoProdutos.ProdutoSelected += SelecaoProdutos_ProdutoSelected;
            gridSelecaoProdutos.NavigationService.RemoveBackEntry();
            gridSelecaoProdutos.Navigate(selecaoProdutos);
        }

        private async void SelecaoProdutos_ProdutoSelected(object sender, VendaSelecaoProdutos.ProdutoSelectedEventArgs e)
        {
            await ItemsSelecionados.AddProdutoVenda(e.Iditem, 1);
        }

        public async void LoadItemsSelecionados()
        {
            ItemsSelecionados = new VendaItemsSelecionados();
            ItemsSelecionados.ItemVendaSelected += ItemsSelecionados_ItemVendaSelected;
            ItemsSelecionados.ReceberClicked += ItemsSelecionados_ReceberClicked;
            gridItemsSelecionados.NavigationService.RemoveBackEntry();
            gridItemsSelecionados.Navigate(ItemsSelecionados);
        }

        private async void ItemsSelecionados_ReceberClicked(object sender, EventArgs e)
        {
            VendaPagamentos vendaPagamentos = new VendaPagamentos();
            await vendaPagamentos.LoadVenda(ItemsSelecionados.Venda.Idvenda);
            vendaPagamentos.PagamentoRealizado += VendaPagamentos_PagamentoRealizado;
            vendaPagamentos.Show();
        }

        private async void VendaPagamentos_PagamentoRealizado(object sender, EventArgs e)
        {
            ItemsSelecionados.Venda = null;
            await ItemsSelecionados.LoadVenda();
        }

        private async void ItemsSelecionados_ItemVendaSelected(object sender, VendaItemsSelecionados.ItemVendaSelectedEventArgs e)
        {
            await ItemsSelecionados.AddProdutoVenda(e.Iditem, e.Quantidade);
        }
    }
}
