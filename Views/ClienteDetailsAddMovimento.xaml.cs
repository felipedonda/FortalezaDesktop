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
    /// Interaction logic for ClienteDetailsAddMovimento.xaml
    /// </summary>
    public partial class ClienteDetailsAddMovimento : Window
    {
        public enum TipoMovimento
        {
            Suprimento = 2,
            Sangria = 3
        }

        public TipoMovimento Tipo { get; set; }

        public Movimento Movimento { get; set; }

        public Cliente Cliente { get; set; }

        public bool AdicionandoMovimento { get; set; }

        public event EventHandler MovimentoAdicionado;

        public ClienteDetailsAddMovimento(TipoMovimento tipoMovimento, Cliente cliente)
        {
            InitializeComponent();
            Tipo = tipoMovimento;
            Cliente = cliente;
            TextboxCliente.Text = cliente.Nome;
            PadNumerico.OkClick += PadNumerico_OkClick;
            PadNumerico.CancelarClick += PadNumerico_CancelarClick;
            AdicionandoMovimento = false;
            if(Tipo == TipoMovimento.Sangria)
            {
                CheckboxDebitarCaixa.Visibility = Visibility.Hidden;
                CheckboxDebitarCaixa.IsEnabled = false;
            }
        }

        public async Task LoadMovimento(Movimento movimento)
        {
            Movimento = movimento;
            await ReloadMovimento();
        }

        public async Task ReloadMovimento()
        {
            GridPrincipal.DataContext = null;
            GridPrincipal.DataContext = Movimento;
        }

        private void PadNumerico_CancelarClick(object sender, EventArgs e)
        {
            Close();
        }

        private async void PadNumerico_OkClick(object sender, EventArgs e)
        {
            if(!AdicionandoMovimento)
            {
                AdicionandoMovimento = true;
                if (CheckboxDebitarCaixa.IsChecked != null)
                {
                    if ((CheckboxDebitarCaixa.IsChecked ?? default) && Tipo == TipoMovimento.Suprimento)
                    {
                        Caixa caixaAberto = await new Caixa().GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
                        if (caixaAberto == null)
                        {
                            MessageBox.Show("Nenhum caixa aberto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                            AdicionandoMovimento = false;
                            return;
                        }

                        Pdv pdv = await new Pdv().FindById(UserPreferences.Preferences.Idpdv);

                        Movimento movimentoCaixa = new Movimento
                        {
                            HoraEntrada = DateTime.Now,
                            IdusuarioNavigation = UserController.UsuarioLogado,
                            IdcaixaNavigation = caixaAberto,
                            IdpdvNavigation = pdv,
                            Valor = Movimento.Valor,
                            Tipo = 3,
                            IdformaPagamentoNavigation = Movimento.IdformaPagamentoNavigation,
                            Descricao = "CRÉDITO CLIENTE - " + Cliente.Nome
                        };

                        if (!await movimentoCaixa.SaveInstance())
                        {
                            AdicionandoMovimento = false;
                            return;
                        }
                    }
                }
                if (await Cliente.SaveMovimento(Movimento))
                {
                    Close();
                }
                else
                {
                    AdicionandoMovimento = false;
                }
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Pdv pdv = await new Pdv().FindById(UserPreferences.Preferences.Idpdv);
            Movimento = new Movimento
            {
                HoraEntrada = DateTime.Now,
                IdusuarioNavigation = UserController.UsuarioLogado,
                IdpdvNavigation = pdv,
                Tipo = (int)Tipo,
                Descricao = Tipo switch
                {
                    TipoMovimento.Sangria => "SANGRIA",
                    TipoMovimento.Suprimento => "SUPRIMENTO",
                    _ => ""
                }
            };
            await LoadFormaPagamentos();
        }

        public async Task LoadFormaPagamentos()
        {
            FormaPagamento FormaPagamento = new FormaPagamento();
            List<FormaPagamento> formaPagamentos = await FormaPagamento.FindAll();
            GridFormasPagamentos.ItemsSource = null;
            GridFormasPagamentos.ItemsSource = formaPagamentos.OrderBy((e) => e.Ordem);
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
