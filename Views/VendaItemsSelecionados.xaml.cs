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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for VendaItemsSelecionados.xaml
    /// </summary>
    public partial class VendaItemsSelecionados : Page
    {
        public class ItemVendaSelectedEventArgs : EventArgs
        {
            public int Iditem { get; set; }
            public int Quantidade { get; set; }
            public ItemVendaSelectedEventArgs(int iditem)
            {
                Iditem = iditem;
            }
        }

        public event EventHandler<ItemVendaSelectedEventArgs> ItemVendaSelected;
        public event EventHandler ReceberClicked;
        public event EventHandler CancelarSelected;
        public event EventHandler CancelarItemSelected;

        public Venda Venda { get; set; }
        public bool CriandoVenda { get; set; }
        public bool AdicionandoProduto { get; set; }
        public bool ReceberWasClicked { get; set; }
        public int TipoVenda { get; set; }

        public VendaItemsSelecionados(int tipoVenda = 0)
        {
            InitializeComponent();
            ReceberWasClicked = false;
            CriandoVenda = false;
            AdicionandoProduto = false;
            TipoVenda = tipoVenda;
        }

        public async Task ReloadVenda()
        {
            Venda = await Venda.ReloadInstance(new Dictionary<string, string> {
                {"itemvendas", "true" }
            });
            LoadVenda();
        }

        public async Task LoadVenda(int idvenda)
        {
            Venda = new Venda
            {
                Idvenda = idvenda
            };
            await ReloadVenda();
        }

        public void LoadVenda()
        {
            DataContext = null;
            DataContext = Venda;
        }

        public void LimparVenda()
        {
            Venda = null;
            LoadVenda();
            textboxCodigo.Focus();
        }

        public async Task<bool> ValidateVenda()
        {
            if(CriandoVenda)
            {
                await Task.Delay(5);
                return await ValidateVenda();
            }

            if (Venda == null)
            {
                Caixa caixaAberto = await (new Caixa()).GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
                if (caixaAberto != null)
                {
                    CriandoVenda = true;
                    Venda = new Venda
                    {
                        Tipo = TipoVenda,
                        Aberta = 1,
                        Idresponsavel = UserController.UsuarioLogado.Idusuario,
                        Paga = 0,
                        HoraEntrada = DateTime.Now,
                        Acrescimo = 0,
                        ValorPago = 0,
                        Idpdv = UserPreferences.Preferences.Idpdv,
                        Idcaixa = caixaAberto.Idcaixa
                    };
                    bool success = await Venda.SaveInstance();
                    if(!success)
                    {
                        Venda = null;
                    }
                    CriandoVenda = false;
                    return success;
                }
                else
                {
                    MessageBox.Show("Nenhum caixa aberto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }

            return true;
        }

        public async Task AddProdutoVenda(int iditem)
        {
            if(!AdicionandoProduto)
            {
                AdicionandoProduto = true;
                if (await ValidateVenda())
                {
                    int quantidade;
                    try
                    {
                        quantidade = int.Parse(textboxQuantidade.Text);
                        textboxQuantidade.Text = "1";
                    }
                    catch
                    {
                        MessageBox.Show("Quantidade inserida inválida.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                        AdicionandoProduto = false;
                        return;
                    }

                    ItemVenda itemVenda = new ItemVenda
                    {
                        Iditem = iditem,
                        Quantidade = quantidade
                    };

                    //aviso de estoque indisponivel?

                    await Venda.SaveItemVenda(itemVenda);
                    await ReloadVenda();
                    textboxQuantidade.Text = 1.ToString();
                    textboxCodigo.Text = "";
                    textboxCodigo.Focus();
                    AdicionandoProduto = false;
                }
                else
                {
                    AdicionandoProduto = false;
                }
            }

        }

        private void ButtonIncrease_Click(object sender, RoutedEventArgs e)
        {
            QuantidadeIncrease();
        }

        private void QuantidadeIncrease()
        {
            try
            {
                textboxQuantidade.Text = (int.Parse(textboxQuantidade.Text) + 1).ToString();
            }
            catch
            {
                textboxQuantidade.Text = 2.ToString();
            }
        }

        private void ButtonDecrease_Click(object sender, RoutedEventArgs e)
        {
            QuantidadeDecrease();
        }

        private void QuantidadeDecrease()
        {
            try
            {
                int current = int.Parse(textboxQuantidade.Text);
                if (current > 1)
                {
                    textboxQuantidade.Text = (current - 1).ToString();
                }
            }
            catch
            {
                textboxQuantidade.Text = 1.ToString();
            }
        }

        private async void buttonQuantidadeOk_Click(object sender, RoutedEventArgs e)
        {
            await SelecionarItemCodigo();
        }

        private async Task SelecionarItemCodigo()
        {
            if (string.IsNullOrEmpty(textboxCodigo.Text))
            {
                AbrirBuscaProdutos();
                return;
            }

            try
            {
                int codigo = int.Parse(textboxCodigo.Text);
                bool exists = await new Item().ItemExists(codigo);
                if (exists)
                {
                    ItemVendaSelected?.Invoke(this, new ItemVendaSelectedEventArgs(codigo));
                }
                else
                {
                    AbrirBuscaProdutos(textboxCodigo.Text);
                }
            }
            catch
            {
                AbrirBuscaProdutos(textboxCodigo.Text);
                return;
            }
        }

        private async void AbrirBuscaProdutos(string busca = "")
        {
            PedidoDetailsProdutos pedidoDetailsProdutos = new PedidoDetailsProdutos();
            pedidoDetailsProdutos.ItemSelecionado += PedidoDetailsProdutos_ItemSelecionado;
            pedidoDetailsProdutos.Closed += PedidoDetailsProdutos_Closed;

            if (!string.IsNullOrEmpty(busca))
            {
                pedidoDetailsProdutos.textboxBuscaDescricao.Text = textboxCodigo.Text;
            }

            pedidoDetailsProdutos.ShowDialog();
        }

        private async Task ConfirmarVenda()
        {
            if (!ReceberWasClicked)
            {
                ReceberWasClicked = true;
                Caixa caixaAberto = await (new Caixa()).GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
                if (caixaAberto != null)
                {
                    ReceberClicked?.Invoke(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Nenhum caixa aberto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                ReceberWasClicked = false;
            }
        }

        private async void ButtonReceber_Click(object sender, RoutedEventArgs e)
        {
            await ConfirmarVenda();
        }

        private void Cancelar()
        {
            VendaCancelarItem cancelarItem = new VendaCancelarItem();
            cancelarItem.CancelarVenda += CancelarItem_CancelarVenda;
            cancelarItem.CancelarItem += CancelarItem_CancelarItem;
            cancelarItem.ShowDialog();
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cancelar();
        }

        private void CancelarItem_CancelarVenda(object sender, EventArgs e)
        {
            CancelarSelected?.Invoke(this, new EventArgs());
        }

        private async void CancelarItem_CancelarItem(object sender, VendaCancelarItem.CancelarItemEventArgs e)
        {
            if(Venda != null)
            {
                if (Venda.ItemVenda != null)
                {
                    List<ItemVenda> query = Venda.ItemVenda.Where(q => q.Indice == e.IndiceItemVenda).ToList();
                    if (query != null)
                    {
                        if (query.Count > 0)
                        {
                            ItemVenda itemVenda = query.FirstOrDefault();
                            await itemVenda.DeleteInstance();
                            await Venda.RecontarItems();
                            await ReloadVenda();
                        }
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            textboxCodigo.Focus();
        }

        private void SelecionarCodigo_Click(object sender, RoutedEventArgs e)
        {
            PedidoDetailsProdutos pedidoDetailsProdutos = new PedidoDetailsProdutos();
            pedidoDetailsProdutos.ItemSelecionado += PedidoDetailsProdutos_ItemSelecionado;
            pedidoDetailsProdutos.Closed += PedidoDetailsProdutos_Closed;
            pedidoDetailsProdutos.ShowDialog();
        }

        private void PedidoDetailsProdutos_Closed(object sender, EventArgs e)
        {
            textboxQuantidade.SelectAll();
        }

        private async void PedidoDetailsProdutos_ItemSelecionado(object sender, PedidoDetailsProdutos.ItemSelecionadoArgs e)
        {
            textboxCodigo.Text = e.Item.Iditem.ToString();
            await SelecionarItemCodigo();
        }

        private void textboxQuantidade_GotFocus(object sender, RoutedEventArgs e)
        {
            textboxQuantidade.SelectAll();
        }

        private async void TextboxCodQuan_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                await SelecionarItemCodigo();
            }
        }

        private async void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                await ConfirmarVenda();
            }

            if (e.Key == Key.F11)
            {
                Cancelar();
            }

            if (e.Key == Key.Add)
            {
                QuantidadeIncrease();
            }

            if (e.Key == Key.Subtract)
            {
                QuantidadeDecrease();
            }
        }

        private void textboxCodigo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(e.Text == "+" || e.Text == "-")
            {
                e.Handled = true;
            }
        }
    }
}
