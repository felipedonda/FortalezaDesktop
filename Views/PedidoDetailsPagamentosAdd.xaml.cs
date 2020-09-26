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
using System.Linq;
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for PedidoDetailsPagamentosAdd.xaml
    /// </summary>
    public partial class PedidoDetailsPagamentosAdd : Window
    {
        public Movimento Movimento { get; set; }
        public Venda Venda { get; set; }
        public bool Update { get; set; }
        public Caixa CaixaAberto { get; set; }

        public PedidoDetailsPagamentosAdd(Venda venda, Caixa caixaAberto)
        {
            InitializeComponent();
            Update = false;
            Venda = venda;
            CaixaAberto = caixaAberto;
            buttonRemover.Visibility = Visibility.Collapsed;
            LoadMovimento();
        }

        public PedidoDetailsPagamentosAdd(int idmovimento, Venda venda, Caixa caixaAberto)
        {
            InitializeComponent();
            Update = true;
            Venda = venda;
            buttonAdicionar.Content = "Atualizar";
            LoadMovimento(idmovimento);
        }

        public async void LoadMovimento(int idmovimento)
        {
            Movimento = await new Movimento().FindById(idmovimento);
            await _LoadMovimento();
        }

        public async void LoadMovimento()
        {

            Movimento = new Movimento
            {
                IdformaPagamento = 1,
                HoraEntrada = DateTime.UtcNow,
                Descricao = "VENDA - " + Venda.IdclienteNavigation.Nome,
                Tipo = "C",
                Idcaixa = CaixaAberto.Idcaixa
            };

            await _LoadMovimento();
        }

        public async Task _LoadMovimento()
        {
            await LoadFormaPagamentos();
            panelMovimento.DataContext = null;
            panelMovimento.DataContext = Movimento;

            for (int i = 0; i < ((List<FormaPagamento>)comboboxFormaPagamento.ItemsSource).Count; i++)
            {
                if (((List<FormaPagamento>)comboboxFormaPagamento.ItemsSource)[i].IdformaPagamento == Movimento.IdformaPagamento)
                {
                    comboboxFormaPagamento.SelectedIndex = i;
                }
            }
        }

        public async Task LoadFormaPagamentos()
        {
            List<FormaPagamento> formaPagamentos = await new FormaPagamento().FindAll();
            comboboxFormaPagamento.ItemsSource = null;
            comboboxFormaPagamento.ItemsSource = formaPagamentos;
            comboboxFormaPagamento.DisplayMemberPath = "Nome";
            comboboxFormaPagamento.SelectedValuePath = "IdformaPagamento";

            for (int i = 0; i < ((List<FormaPagamento>)comboboxFormaPagamento.ItemsSource).Count; i++)
            {
                if (((List<FormaPagamento>)comboboxFormaPagamento.ItemsSource)[i].IdformaPagamento == Movimento.IdformaPagamento)
                {
                    comboboxFormaPagamento.SelectedIndex = i;
                }
            }

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void ButtonAdicionar(object sender, RoutedEventArgs e)
        {
            FormaPagamento formaPagamento = (FormaPagamento)comboboxFormaPagamento.SelectedItem;
            Movimento.IdformaPagamento = formaPagamento.IdformaPagamento;
            if(formaPagamento.Bandeira == 1)
            {
                VendaPagamentoBandeira vendaPagamentoBandeira = new VendaPagamentoBandeira();
                vendaPagamentoBandeira.Selecionado += VendaPagamentoBandeira_Selecionado; ;
                vendaPagamentoBandeira.Show();
            }
            else
            {
                await FinalizarPagamento();
            }
        }

        private async void VendaPagamentoBandeira_Selecionado(object sender, EventArgs e)
        {
            Movimento.Idbandeira = null;
            VendaPagamentoBandeira senderAsForm = (VendaPagamentoBandeira)sender;
            if(senderAsForm.BandeiraSelecionada != null)
            {
                Movimento.Idbandeira = senderAsForm.BandeiraSelecionada.Idbandeira;
                await FinalizarPagamento();
            }
        }

        private async Task FinalizarPagamento()
        {
            if(Update)
            {
                await Movimento.UpdateInstance();
            }
            else
            {
                Pagamento pagamento = new Pagamento
                {
                    Idvenda = Venda.Idvenda,
                    IdmovimentoNavigation = Movimento
                };
                await Venda.SavePagamento(pagamento);
            }
            Close();
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonRemover(object sender, RoutedEventArgs e)
        {
            await Movimento.DeleteInstance();
            Close();
        }
    }
}
