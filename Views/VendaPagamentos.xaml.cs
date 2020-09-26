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
        public int Idbandeira { get; set; }
        public FormaPagamento FormaPagamentoSelecionada { get; set; }

        public VendaPagamentos(Venda venda, bool modoAdiantamento = false)
        {
            Venda = venda;
            UserPreferences.Load();
            InitializeComponent();
        }

        public event EventHandler PagamentoRealizado;

        public async Task LoadCliente()
        {
            gridCliente.DataContext = null;
            gridCliente.DataContext = Venda.IdclienteNavigation;
        }

        public async void FinalizarPagamento()
        {
            Venda.Desconto = Desconto;
            await Venda.UpdateInstance();
            await Venda.FecharVenda();
            if(Troco > 0)
            {
                MessageBox.Show("Troco total: " + Troco.ToString("C2"), "Troco", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            PagamentoRealizado?.Invoke(this, new EventArgs());
            Close();
        }

        private async void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if ((await GetValorRemanescente()) > 0)
            {
                MessageBox.Show("Valor pendente de pagamento: " + (await GetValorRemanescente()).ToString("C2"));
            }
            else
            {
                FinalizarPagamento();
            }
        }

        private async void Pagamento_Click(object sender, RoutedEventArgs e)
        {
            FormaPagamentoSelecionada = (FormaPagamento)((Button)sender).Tag;
            
            if (FormaPagamentoSelecionada.Bandeira == 1)
            {
                VendaPagamentoBandeira vendaPagamentoBandeira = new VendaPagamentoBandeira();
                vendaPagamentoBandeira.Selecionado += VendaPagamentoBandeira_Selecionado;
                vendaPagamentoBandeira.Show();
            }
            else
            {
                await OpenPagamentosVendaValor();
            }
        }

        public async Task OpenPagamentosVendaValor()
        {
            VendaPagamentosValor pagamentosVendaValorView = new VendaPagamentosValor();
            pagamentosVendaValorView.LoadFormaPagamento(FormaPagamentoSelecionada);
            pagamentosVendaValorView.SetValorRemanescente(await GetValorRemanescente());
            pagamentosVendaValorView.InserirValor += PagamentosVendaValorView_InserirValor;
            pagamentosVendaValorView.Show();
        }

        private async void VendaPagamentoBandeira_Selecionado(object sender, EventArgs e)
        {
            Idbandeira = -1;
            VendaPagamentoBandeira senderAsForm = (VendaPagamentoBandeira)sender;
            Idbandeira = senderAsForm.BandeiraSelecionada.Idbandeira;
            await OpenPagamentosVendaValor();
        }

        private async void PagamentosVendaValorView_InserirValor(object sender, EventArgs e)
        {
            VendaPagamentosValor senderAsForm = (VendaPagamentosValor)sender;
            Pagamento pagamento = new Pagamento
            {
                IdmovimentoNavigation = new Movimento
                {
                    Idcaixa = CaixaAberto.Idcaixa,
                    IdformaPagamentoNavigation = senderAsForm.MeioPagamento,
                    IdformaPagamento = senderAsForm.MeioPagamento.IdformaPagamento,
                    HoraEntrada = DateTime.UtcNow,
                    Tipo = "C",
                    Valor = senderAsForm.Valor,
                    Descricao = "VENDA - CONSUMIDOR NÃO IDENTIFICADO"
                }
            };

            if(senderAsForm.MeioPagamento.Bandeira == 1)
            {
                if(Idbandeira > -1)
                {
                    pagamento.IdmovimentoNavigation.Idbandeira = Idbandeira;
                }
            }

            if(Venda.IdclienteNavigation != null)
            {
                pagamento.IdmovimentoNavigation.Descricao = "VENDA - " + Venda.IdclienteNavigation.Nome;
            }

            await Venda.SavePagamento(pagamento);

            await UpdatePagamentos();
            await UpdateValues();

            if ((await GetValorRemanescente() <= 0) & UserPreferences.Preferences.ConcluirVendaPagamentoCompleto)
            {
                FinalizarPagamento();
            }
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            CaixaAberto = await (new Caixa()).GetCaixaAberto();
            await LoadFormaPagamentos();
            await LoadCliente();
            await UpdateValues();
        }

        public async Task UpdateValues()
        {
            decimal valorTotal = Venda.ValorTotal + (Venda.Desconto ?? default);
            decimal valorPago = Venda.ValorPago ?? default;
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

        public async Task LoadVenda()
        {
            Venda = await Venda.ReloadInstance(new Dictionary<string, string> { { "pagamentos", "true" } });
            await UpdateValues();
        }

        public async Task<decimal> GetValorRemanescente()
        {
            decimal valorRemanescente = Venda.ValorTotal - (Venda.ValorPago ?? default);
            return (valorRemanescente - Desconto);
        }

        public async Task UpdatePagamentos()
        {
            datagridPagamentos.ItemsSource = null;
            datagridPagamentos.ItemsSource = Venda.Pagamento;
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
                await LoadVenda();
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
            await LoadCliente();
        }
    }
}
