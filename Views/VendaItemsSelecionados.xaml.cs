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
            public ItemVendaSelectedEventArgs(int iditem, int quantidade)
            {
                Iditem = iditem;
                Quantidade = quantidade;
            }
        }

        public class CancelarItemSelectedEventArgs : EventArgs
        {
            public int IdItem { get; set; }
            public CancelarItemSelectedEventArgs(int iditem)
            {
                IdItem = iditem;
            }
        }

        public event EventHandler<ItemVendaSelectedEventArgs> ItemVendaSelected;
        public event EventHandler ReceberClicked;
        public event EventHandler CancelarSelected;
        public event EventHandler CancelarItemSelected;

        public Venda Venda { get; set; }
        public bool CriandoVenda { get; set; }
        public int TipoVenda { get; set; }

        public VendaItemsSelecionados(int tipoVenda = 0)
        {
            InitializeComponent();
            CriandoVenda = false;
            TipoVenda = tipoVenda;
        }

        public async Task ReloadVenda()
        {
            Venda = await Venda.ReloadInstance(new Dictionary<string, string> {
                {"itemvendas", "true" }
            });
            await LoadVenda();
        }

        public async Task LoadVenda(int idvenda)
        {
            Venda = new Venda
            {
                Idvenda = idvenda
            };
            await ReloadVenda();
        }

        public async Task LoadVenda()
        {
            DataContext = null;
            DataContext = Venda;
        }

        public async Task LimparVenda()
        {
            Venda = null;
            await LoadVenda();
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
                Caixa caixaAberto = await (new Caixa()).GetCaixaAberto();
                if (caixaAberto != null)
                {
                    CriandoVenda = true;
                    Venda = new Venda
                    {
                        Tipo = TipoVenda,
                        Aberta = 1,
                        Idresponsavel = UserControl.UsuarioLogado.Idusuario,
                        Paga = 0,
                        HoraEntrada = DateTime.UtcNow,
                        Acrescimo = 0,
                        ValorPago = 0,
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

        public async Task AddProdutoVenda(int iditem, int quantidade)
        {
            if (await ValidateVenda())
            {
                ItemVenda itemVenda = new ItemVenda
                {
                    Iditem = iditem,
                    Quantidade = quantidade
                };

                //aviso de estoque indisponivel?

                await Venda.SaveItemVenda(itemVenda);
                await ReloadVenda();
            }
        }

        private void ButtonIncrease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textboxQuantidade.Text = (int.Parse(textboxQuantidade.Text) + 1).ToString();
            }
            catch
            {

            }
        }
        private void ButtonDecrease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textboxQuantidade.Text = (int.Parse(textboxQuantidade.Text) - 1).ToString();
            }
            catch
            {

            }
        }

        private async void buttonQuantidadeOk_Click(object sender, RoutedEventArgs e)
        {
            int quantidade;
            int codigo;
            
            try
            {
                quantidade = int.Parse(textboxQuantidade.Text);
                codigo = int.Parse(textboxCodigo.Text);
                ItemVendaSelected?.Invoke(this, new ItemVendaSelectedEventArgs(codigo, quantidade));
            }
            catch
            {
                return;
            }
        }

        private void ButtonReceber_Click(object sender, RoutedEventArgs e)
        {
            ReceberClicked?.Invoke(this, new EventArgs());
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            VendaCancelarItem cancelarItem = new VendaCancelarItem();
            cancelarItem.CancelarVenda += CancelarItem_CancelarVenda;
            cancelarItem.CancelarItem += CancelarItem_CancelarItem;
            cancelarItem.Show();
        }

        private void CancelarItem_CancelarVenda(object sender, EventArgs e)
        {
            CancelarSelected?.Invoke(this, new EventArgs());
        }

        private async void CancelarItem_CancelarItem(object sender, VendaCancelarItem.CancelarItemEventArgs e)
        {
            if (Venda.ItemVenda != null)
            {
                List<ItemVenda> query = Venda.ItemVenda.Where(q => q.Indice == e.IndiceItemVenda).ToList();
                if (query != null)
                {
                    if(query.Count > 0)
                    {
                        ItemVenda itemVenda = query.FirstOrDefault();
                        await itemVenda.DeleteInstance();
                        await Venda.RecontarItems();
                        await ReloadVenda();
                    }
                }
            }

        }

        private void SelecionarCodigo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
