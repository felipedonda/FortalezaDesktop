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
        public Caixa Caixa { get; set; }
        public Venda Venda { get; set; }

        public VendaView()
        {
            InitializeComponent();
        }

        public async void VendaViewLoad(object sender, RoutedEventArgs e)
        {
            if(!UserPreferences.Preferences.ModoCaixa)
            {
                await LoadGrupos();
            }
            else
            {
                gridColumn0.Width = new GridLength(0);
            }
            await LimparVenda();
        }

        public async Task LoadGrupos()
        {
            Grupo _grupo = new Grupo();
            List<Grupo> grupos = await _grupo.FindAll();
            if(UserPreferences.Preferences.GrupoTodos)
            {
                Grupo grupoTodos = new Grupo
                {
                    Visivel = 1,
                    Nome = "Todos",
                    Idgrupo = -1
                };
                grupos.Insert(0, grupoTodos);
            }
            itemsControlGrupos.ItemsSource = grupos.Where(e => e.Visivel == 1);
            await SelectGrupo(grupos[0].Idgrupo);
        }

        public async Task SelectGrupo(int idgrupo)
        {
            List<Item> items = new List<Item>();
            if (idgrupo == -1)
            {
                items = await (new Item()).FindAll();
            }
            else
            {
                Grupo _grupo = new Grupo();
                _grupo = await _grupo.FindById(idgrupo, new Dictionary<string, string>() { {"items","true" } });
                items = _grupo.ItemHasGrupo.Select(e => e.IditemNavigation).ToList();
            }
            itemsControlProdutos.ItemsSource = items;
        }

        public async void Grupo_Click(object sender, RoutedEventArgs e)
        {
            await SelectGrupo((int)((Button)sender).Tag);
        }

        public async void Produto_Click(object sender, RoutedEventArgs e)
        {
            await AddProdutoVenda((Item)((Button)sender).Tag, 1);
        }

        public async void ButtonReceber_Click(object sender, RoutedEventArgs e)
        {
            VendaPagamentos pagamentosVendaView = new VendaPagamentos(Venda);
            pagamentosVendaView.PagamentoRealizado += PagamentosVendaView_Closed;
            pagamentosVendaView.Show();
        }

        public async void PagamentosVendaView_Closed(object sender, EventArgs e)
        {
            await LimparVenda();
        }

        public async void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            await LimparVenda();
        }

        public async Task AddProdutoVenda(Item item, decimal quantidade)
        {
            if (Venda == null)
            {
                Caixa caixaAberto = await (new Caixa()).GetCaixaAberto();
                if(caixaAberto != null)
                {
                    Venda = new Venda { 
                        Tipo = 0,
                        Aberta = 1,
                        Idresponsavel = UserControl.UsuarioLogado.Idusuario,
                        Paga = 0,
                        HoraEntrada = DateTime.UtcNow
                    };
                    textboxValorTotal.Text = Venda.ValorTotal.ToString("C2");
                    await Venda.SaveInstance();
                }
                else
                {
                    MessageBox.Show("Nenhum caixa aberto.","Aviso",MessageBoxButton.OK,MessageBoxImage.Information);
                    return;
                }
            }
            if(item.Estoque == 1)
            {
                /*
                if(!(await item.ValidarEstoqueDisponivel(quantidade)))
                {
                    MessageBox.Show("EstoqueView disponível de " + item.Descricao + " é insuficiente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                */
            }

            ItemVenda itemVenda = new ItemVenda
            {
                Iditem = item.Iditem,
                Quantidade = quantidade,
                Valor = item.Valor ?? default,
            };

            await Venda.SaveItemVenda(itemVenda);

            textboxValorTotal.Text = Venda.ValorTotal.ToString("C2");
            gridProdutoVendas.ItemsSource = null;
            gridProdutoVendas.ItemsSource = Venda.ItemVenda;

        }

        public async Task FecharVenda()
        {
            LimparVenda();
        }

        public async Task<bool> GetCaixaAberto()
        {
            Caixa = await Caixa.GetCaixaAberto();
            return (Caixa == null);
        }

        public async Task LimparVenda()
        {
            Venda = null;
            textboxValorTotal.Text = 0.ToString("C2");
            gridProdutoVendas.ItemsSource = null;
        }

        public async Task AddProdutoVenda()
        {
            int Iditem = -1;
            decimal quantidade = -1;
            try
            {
                Iditem = int.Parse(textboxCodigo.Text);
            }
            catch
            {
                MessageBox.Show("Codigo mal inserido.");
                return;
            }

            try
            {
                quantidade = decimal.Parse(textboxQuantidade.Text);
            }
            catch
            {
                MessageBox.Show("Quantidade mal inserida.");
                return;
            }

            Item item = new Item();
            item = await item.FindById(Iditem);
            if(item != null & quantidade > -1)
            {
                await AddProdutoVenda(item, quantidade);
            }
            else
            {
                MessageBox.Show("Produto não encontrado.");
            }
            
            
        }

        private async void buttonQuantidadeOk_Click(object sender, RoutedEventArgs e)
        {
            await AddProdutoVenda();
        }
    }
}
