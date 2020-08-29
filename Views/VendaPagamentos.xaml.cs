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
        private UserPreferences UserPreferences { get; set; }

        public VendaPagamentos(Venda venda)
        {
            Venda = venda;
            UserPreferences = new UserPreferences();
            UserPreferences.Load();
            InitializeComponent();
        }

        public event EventHandler PagamentoRealizado;

        public async void FinalizarPagamento()
        {
            await Venda.SaveInstance();
            PagamentoRealizado?.Invoke(this, new EventArgs());
            Close();
        }

        private async void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if((await GetValorRemanescente()) > 0)
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
            FormaPagamento formaPagamento = (FormaPagamento)((Button)sender).Tag;
            VendaPagamentosValor pagamentosVendaValorView = new VendaPagamentosValor();
            pagamentosVendaValorView.LoadFormaPagamento(formaPagamento);
            pagamentosVendaValorView.SetValorRemanescente(await GetValorRemanescente());
            pagamentosVendaValorView.InserirValor += PagamentosVendaValorView_InserirValor;
            pagamentosVendaValorView.Show();
        }

        private async void PagamentosVendaValorView_InserirValor(object sender, EventArgs e)
        {
            VendaPagamentosValor senderAsForm = (VendaPagamentosValor)sender;
            Pagamento pagamento = new Pagamento
            {
                Movimento = new Movimento
                {
                    CaixaIdcaixa = Venda.Idcaixa ?? default,
                    FormaPagamento = senderAsForm.MeioPagamento,
                    FormaPagamentoIdformaPagamento = senderAsForm.MeioPagamento.IdformaPagamento,
                    HoraEntrada = DateTime.UtcNow,
                    Responsavel = Venda.Idresponsavel,
                    Tipo = "C",
                    Valor = senderAsForm.Valor,
                    Descricao = "VENDA - CONSUMIDOR NÃO IDENTIFICADO"
                }
            };
            Venda.Pagamentos.Add(pagamento);
            if ((await GetValorRemanescente() <= 0))
                FinalizarPagamento();
            await UpdatePagamentos();
            await UpdateValores();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadFormaPagamentos();
            await UpdateValores();
        }

        public async Task UpdateValores()
        {
            textboxRecebido.Text = (await GetValorPago()).ToString("C2");
            textboxValorTotal.Text = Venda.ValorTotal.ToString("C2");
        }

        public async Task<decimal> GetValorPago()
        {
            decimal ValorPago = new decimal();
            await Task.Run(() =>
            {
                ValorPago = Venda.Pagamentos.Sum(e => e.Movimento.Valor);
            });
            return ValorPago;
        }

        public async Task<decimal> GetValorRemanescente()
        {
            decimal valorRemanescente = Venda.ValorTotal - await GetValorPago();
            return valorRemanescente;
        }

        public async Task UpdatePagamentos()
        {
            datagridPagamentos.ItemsSource = null;
            datagridPagamentos.ItemsSource = Venda.Pagamentos;
        }

        public async Task LoadFormaPagamentos()
        {
            List<FormaPagamento> formaPagamentos = await FormaPagamento.GetFormaPagamentos();
            itemsMeiosPagamentos.ItemsSource = null;
            itemsMeiosPagamentos.ItemsSource = formaPagamentos;
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
