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
    /// Interaction logic for PagamentosVendaView.xaml
    /// </summary>
    public partial class VendaPagamentos : Window
    {
        public Venda Venda { get; set; }
        public decimal Desconto { get; set; }
        public decimal Troco { get; set; }
        public Caixa CaixaAberto { get; set; }
        public bool AllowPagamentoIncompleto { get; set; }
        public bool ModoAlteracao { get; set; }
        public bool RealizandoPagamento { get; set; }
        private int FormaPagamentosGeneratorCounter { get; set; }

        public VendaPagamentos(bool allowPagamentoIncompleto = false, bool modoAlteracao = false)
        {
            InitializeComponent();
            RealizandoPagamento = false;
            AllowPagamentoIncompleto = allowPagamentoIncompleto;
            ModoAlteracao = modoAlteracao;
            FormaPagamentosGeneratorCounter = 0;
            UserPreferences.Load();
        }

        public event EventHandler PagamentoRealizado;

        public async Task LoadVenda(int idvenda)
        {
            Venda = new Venda { Idvenda = idvenda };
            await ReloadVenda();
        }

        public async Task ReloadVenda()
        {
            Venda = await Venda.ReloadInstance(new Dictionary<string, string> { { "pagamentos", "true" } });
            await LoadVenda();
        }

        public async Task LoadVenda()
        {
            gridCliente.DataContext = null;
            gridCliente.DataContext = Venda.IdclienteNavigation;
            datagridPagamentos.ItemsSource = null;
            datagridPagamentos.ItemsSource = Venda.Pagamento;
            await UpdateValues();
        }

        public async Task FinalizarPagamento()
        {
            //Check para evitar requisição dupla em double click.
            if(!RealizandoPagamento)
            {
                RealizandoPagamento = true;

                if (!AllowPagamentoIncompleto && (await GetValorRemanescente()) > 0)
                {
                    MessageBox.Show("Valor pendente de pagamento: " + (await GetValorRemanescente()).ToString("C2"), "Pagamento", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    RealizandoPagamento = false;
                    return;
                }

                if (!AllowPagamentoIncompleto)
                {
                    if(await Venda.FecharVenda())
                    {
                        if (Troco > 0)
                        {
                            MessageBox.Show("Troco total: " + Troco.ToString("C2"), "Troco", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        PagamentoRealizado?.Invoke(this, new EventArgs());
                        Close();
                        return;
                    }
                    else
                    {
                        RealizandoPagamento = false;
                        return;
                    }
                }
                else
                {
                    //Caminho caso permita pagamento incompleto (pedidos e delivery).
                    PagamentoRealizado?.Invoke(this, new EventArgs());
                    Close();
                }     
            }
        }

        private async void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            await FinalizarPagamento();
        }

        private async void Pagamento_Click(object sender, RoutedEventArgs e)
        {
            await SelecionarFormaPagamento((FormaPagamento)((Button)sender).Tag);
        }

        private async Task SelecionarFormaPagamento(FormaPagamento formaPagamento)
        {
            if (formaPagamento.Bandeira == 1)
            {
                VendaPagamentoBandeira vendaPagamentoBandeira = new VendaPagamentoBandeira(formaPagamento);
                vendaPagamentoBandeira.BandeiraSelecionada += VendaPagamentoBandeira_Selecionado;
                vendaPagamentoBandeira.ShowDialog();
            }
            else
            {
                await OpenPagamentosVendaValor(formaPagamento, null);
            }
        }

        public async Task OpenPagamentosVendaValor(FormaPagamento formaPagamento, Bandeira bandeira)
        {
            VendaPagamentosValor pagamentosVendaValorView = new VendaPagamentosValor(formaPagamento, bandeira);
            pagamentosVendaValorView.SetValorRemanescente(await GetValorRemanescente());
            pagamentosVendaValorView.InserirValor += PagamentosVendaValorView_InserirValor;
            pagamentosVendaValorView.ShowDialog();
        }

        private async void VendaPagamentoBandeira_Selecionado(object sender, VendaPagamentoBandeira.BandeiraSelecionadaArgs e)
        {
            await OpenPagamentosVendaValor(e.FormaPagamento, e.Bandeira);
        }

        private async void PagamentosVendaValorView_InserirValor(object sender, VendaPagamentosValor.ValorInseridoArgs e)
        {
            Pagamento pagamento = new Pagamento
            {
                IdmovimentoNavigation = new Movimento
                {
                    Idcaixa = CaixaAberto.Idcaixa,
                    IdformaPagamentoNavigation = e.FormaPagamento,
                    IdformaPagamento = e.FormaPagamento.IdformaPagamento,
                    HoraEntrada = DateTime.Now,
                    Tipo = 1,
                    Valor = e.Valor,
                    Descricao = "VENDA - CONSUMIDOR NÃO IDENTIFICADO",
                    Idpdv = UserPreferences.Preferences.Idpdv,
                    Idusuario = UserController.UsuarioLogado.Idusuario
                }
            };

            if(e.FormaPagamento.Bandeira == 1 && e.Bandeira != null)
            {
                if(e.Bandeira == null)
                {
                    MessageBox.Show("Bandeira não selecionada.", "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                pagamento.IdmovimentoNavigation.Idbandeira = e.Bandeira.Idbandeira;
            }

            if(Venda.IdclienteNavigation != null)
            {
                pagamento.IdmovimentoNavigation.Descricao = "VENDA - " + Venda.IdclienteNavigation.Nome;
            }

            await Venda.SavePagamento(pagamento);
            await ReloadVenda();

            if(await GetValorRemanescente() <= 0)
            {
                if (!ModoAlteracao & UserPreferences.Preferences.ConcluirVendaPagamentoCompleto)
                {
                    await FinalizarPagamento();
                }
                else
                {
                    ButtonOk.Focus();
                }
            }
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            CaixaAberto = await (new Caixa()).GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
            if(CaixaAberto == null)
            {
                MessageBox.Show("Caixa fechado.", "Caixa", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            await LoadFormaPagamentos();
            await UpdateValues();
        }

        public async Task UpdateValues()
        {
            decimal valorTotal = Venda.ValorTotal - Venda.Desconto;
            decimal valorPago = Venda.ValorPago;
            textboxRecebido.Text = valorPago.ToString("C2");
            textboxValorTotal.Text = valorTotal.ToString("C2");
            if (valorPago > valorTotal)
            {
                Troco = valorPago - valorTotal;
            }
            else
            {
                Troco = 0;
            }
            textboxTroco.Text = Troco.ToString("C2");

        }


        public async Task<decimal> GetValorRemanescente()
        {
            decimal valorRemanescente = Venda.ValorTotal - Venda.ValorPago;
            return (valorRemanescente - Desconto);
        }


        public async Task LoadFormaPagamentos()
        {
            List<FormaPagamento> formaPagamentos = await (new FormaPagamento()).FindAll();
            itemsMeiosPagamentos.ItemsSource = null;
            itemsMeiosPagamentos.ItemsSource = formaPagamentos;
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void textboxValorDesconto_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (textboxValorDesconto.Text.EndsWith("%"))
                {
                    decimal porcentagem = decimal.Parse(textboxValorDesconto.Text[0..^1], System.Globalization.NumberStyles.Currency);
                    if(porcentagem > 0 & porcentagem < 100)
                    {
                        Desconto = Venda.ValorTotal * (porcentagem / 100);
                    }
                    else
                    {
                        Desconto = 0;
                    }
                }
                else
                {
                    Desconto = decimal.Parse(textboxValorDesconto.Text, System.Globalization.NumberStyles.Currency);
                }
            }
            catch
            {
                Desconto = 0;
            }
            if(Desconto != Venda.Desconto)
            {
                Venda.Desconto = Desconto;
                await Venda.UpdateInstance();
                await ReloadVenda();
            }
        }

        private async void textboxCPF_KeyDown(object sender, KeyEventArgs e)
        {
            Venda.IdclienteNavigation = null;
            KeyConverter kc = new KeyConverter();
            var digit = kc.ConvertToString(e.Key);
            if (digit.Contains("NumPad"))
                digit = digit.Substring(6);
            if(textboxCPF.Text.Length == 10 | textboxCPF.Text.Length == 13)
            {
                try
                {
                    Venda.IdclienteNavigation = await (new Cliente()).FindByCPF(textboxCPF.Text + digit);
                    Venda.Idcliente = Venda.IdclienteNavigation.Idcliente;
                }
                catch
                {
                }
            }
            await ReloadVenda();
        }

        private async void ButtonAlterar_Click(object sender, RoutedEventArgs e)
        {
            if(datagridPagamentos.SelectedItem != null)
            {
                var messageResult = MessageBox.Show(
                    "Tem certeza que deseja alterar esse pagamento?",
                    "Pagamentos",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if(messageResult == MessageBoxResult.Yes)
                {
                    Pagamento pagamento = (Pagamento)datagridPagamentos.SelectedItem;
                    await pagamento.IdmovimentoNavigation.DeleteInstance();
                    await ReloadVenda();
                }
            }
        }

        private void BuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            PedidoDetailsCliente detailsCliente = new PedidoDetailsCliente();
            detailsCliente.Selecionado += DetailsCliente_Selecionado;
            detailsCliente.ShowDialog();
        }

        private async void DetailsCliente_Selecionado(object sender, PedidoDetailsCliente.ClienteSelecionadoEventArgs e)
        {
            Venda.Idcliente = e.Cliente.Idcliente;
            await Venda.UpdateInstance();
            await ReloadVenda();
        }

        private async void LimparCliente_Click(object sender, RoutedEventArgs e)
        {
            Venda.Idcliente = null;
            await Venda.UpdateInstance();
            await ReloadVenda();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case (Key.Escape):
                    Close();
                    break;
                case (Key.F11):
                    Close();
                    break;
                case (Key.F12):
                    await FinalizarPagamento();
                    break;
            }

            if(!textboxValorDesconto.IsKeyboardFocusWithin && !textboxCPF.IsKeyboardFocusWithin)
            {
                if (e.Key >= Key.NumPad1 && e.Key <= Key.NumPad9)
                {
                    await FormaPagamentoFromNumpad(e.Key);
                }
            }
        }

        private async Task FormaPagamentoFromNumpad(Key key)
        {
            int ordem = (int)key - 74;
            if (ordem > 0 && ordem < 10)
            {
                var formaPagamento = ((List<FormaPagamento>)itemsMeiosPagamentos.ItemsSource).Where(e => e.Ordem == ordem).FirstOrDefault();
                if (formaPagamento != null)
                {
                    await SelecionarFormaPagamento(formaPagamento);
                }
            }
        }
    }
}
