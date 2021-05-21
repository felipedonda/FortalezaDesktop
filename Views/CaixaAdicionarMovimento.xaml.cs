using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
    /// Interaction logic for AdicionarMovimentoView.xaml
    /// </summary>

    public partial class AdicionarMovimento : Window
    {

        public enum TipoMovimento
        {
            Venda = 1,
            Suprimento = 2,
            Sangria = 3,
            Abertura = 4,
            Fechamento = 5
        }

        public TipoMovimento Tipo { get; set; }

        public Movimento Movimento { get; set; }

        public bool AberturaEmAndamento { get; set; }

        public AdicionarMovimento(TipoMovimento tipoMovimento)
        {
            InitializeComponent();
            Tipo = tipoMovimento;
            AberturaEmAndamento = false;
            PadNumerico.OkClick += PadNumerico_OkClick;
            PadNumerico.CancelarClick += PadNumerico_CancelarClick;
            PadNumerico.SetValorRemanescente(0);
        }

        private void PadNumerico_CancelarClick(object sender, EventArgs e)
        {
            Close();
        }

        private async void PadNumerico_OkClick(object sender, EventArgs e)
        {
            if (!AberturaEmAndamento)
            {
                AberturaEmAndamento = true;
                if (Tipo == TipoMovimento.Abertura)
                {
                    if (await Movimento.IdcaixaNavigation.SaveInstance())
                    {
                        if (!await Movimento.IdcaixaNavigation.AbrirCaixa(UserController.UsuarioLogado.Idusuario, UserPreferences.Preferences.Idpdv))
                        {
                            AberturaEmAndamento = false;
                            return;
                        }
                    }
                    else
                    {
                        AberturaEmAndamento = false;
                        return;
                    }
                }
                if (await Movimento.SaveInstance())
                {
                    Close();
                }
                else
                {
                    AberturaEmAndamento = false;
                    return;
                }
            }
        }

        public async Task LoadMovimento(Movimento movimento)
        {
            Movimento = movimento;
            MainGrid.DataContext = null;
            MainGrid.DataContext = Movimento;
        }

        public async void OnLoad(object sender, RoutedEventArgs e)
        {
            Caixa caixaAberto = await new Caixa().GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
            if (caixaAberto == null)
            {
                if (Tipo == TipoMovimento.Abertura)
                {
                    NomeCaixa nomeCaixa = await new NomeCaixa().FindById(UserPreferences.Preferences.IdnomeCaixa);
                    caixaAberto = new Caixa
                    {
                        IdnomeCaixaNavigation = nomeCaixa
                    };
                }
                else
                {
                    MessageBox.Show("Nenhum caixa aberto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
            else
            {
                if (Tipo == TipoMovimento.Abertura)
                {
                    MessageBox.Show("Caixa já está aberto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }


            Pdv pdv = await new Pdv().FindById(UserPreferences.Preferences.Idpdv);
            Movimento movimento = new Movimento
            {
                HoraEntrada = DateTime.Now,
                IdusuarioNavigation = UserController.UsuarioLogado,
                IdcaixaNavigation = caixaAberto,
                IdpdvNavigation = pdv,
                Valor = 0,
                Tipo = (int)Tipo,
                Descricao = Tipo switch
                {
                    TipoMovimento.Abertura => "ABERTURA CAIXA",
                    TipoMovimento.Sangria => "SANGRIA",
                    TipoMovimento.Suprimento => "SUPRIMENTO",
                    _ => ""
                }
            };
            await LoadMovimento(movimento);
            await LoadFormaPagamentos();
        }

        public async Task LoadFormaPagamentos()
        {
            FormaPagamento FormaPagamento = new FormaPagamento();
            List<FormaPagamento> formaPagamentos = await FormaPagamento.FindAll();
            itemsFormasPagamentos.ItemsSource = null;
            itemsFormasPagamentos.ItemsSource = formaPagamentos.OrderBy((e) => e.Ordem);
            Movimento.IdformaPagamentoNavigation = formaPagamentos[0];
            TextBlockFormaPagamento.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }

        private async void FormaPagamento_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            FormaPagamento formaPagamento = (FormaPagamento)senderAsButton.Tag;
            Movimento.IdformaPagamentoNavigation = formaPagamento;
            TextBlockFormaPagamento.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            if (formaPagamento.Bandeira == 1)
            {
                VendaPagamentoBandeira vendaPagamentoBandeira = new VendaPagamentoBandeira(formaPagamento);
                vendaPagamentoBandeira.BandeiraSelecionada += VendaPagamentoBandeira_Selecionado;
                vendaPagamentoBandeira.ShowDialog();
            }
            else
            {
                Movimento.Idbandeira = null;
            }
        }

        private void VendaPagamentoBandeira_Selecionado(object sender, VendaPagamentoBandeira.BandeiraSelecionadaArgs e)
        {
            Movimento.Idbandeira = e.Bandeira.Idbandeira;
        }
    }
}
