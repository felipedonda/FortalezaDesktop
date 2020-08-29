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
    
    public enum OperacaoCaixa
    {
        Abertura,
        Suprimento,
        Sangria,
        Fechamento
    }

    public partial class AdicionarMovimento : Window
    {
        public Caixa Caixa { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public OperacaoCaixa Operacao { get; set; }
        public FormaPagamento MeioPagamento { get; set; }

        public AdicionarMovimento()
        {
            Tipo = "C";
            Descricao = "ABERTURA DE CAIXA";
            Operacao = OperacaoCaixa.Abertura;
            Caixa = new Caixa
            {
                Nome = UserPreferences.NomeCaixa,
                Aberto = true,
                Idresponsavel = UserControl.UsuarioLogado.Idusuario
            };
            InitializeComponent();
        }

        public AdicionarMovimento(Caixa caixa, OperacaoCaixa operacaoCaixa)
        {
            switch (operacaoCaixa)
            {
                case OperacaoCaixa.Suprimento:
                    Tipo = "C";
                    Descricao = "SUPRIMENTO";
                    break;
                case OperacaoCaixa.Sangria:
                    Tipo = "D";
                    Descricao = "SANGRIA";
                    break;
            }

            Caixa = caixa;
            Operacao = operacaoCaixa;
            InitializeComponent();
        }

        public async Task InitiateMovimento()
        {
            Movimento movimento = new Movimento
            {
                Descricao = Descricao,
                Valor = 0,
                HoraEntrada = DateTime.UtcNow,
                Tipo = Tipo,
                Responsavel = 1
                
            };
            gridAdicionarMovimento.DataContext = movimento;
            textboxCaixaNome.Text = Caixa.Nome;
            textboxResponsavel.Text = UserControl.UsuarioLogado.Nome;
        }

        public async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadFormaPagamentos();
            await InitiateMovimento();
        }

        public async void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            Movimento movimento = ((Movimento)gridAdicionarMovimento.DataContext);

            switch (Operacao)
            {
                case OperacaoCaixa.Abertura:
                    Caixa.HoraAbertura = movimento.HoraEntrada;
                    Caixa.Nome = textboxCaixaNome.Text;
                    if (textboxCaixaNome.Text != UserPreferences.NomeCaixa)
                    {
                        UserPreferences.NomeCaixa = textboxCaixaNome.Text;
                        UserPreferences.Save();
                    }
                    Caixa.Aberto = true;
                    try
                    {
                        Caixa = await Caixa.CreateCaixa(Caixa);
                    }
                    catch(BadResponseStatusCodeException ex)
                    {
                        MessageBox.Show(ex.Message,"Erro",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                        return;
                    }
                    break;
                case OperacaoCaixa.Fechamento:
                    Caixa.HoraFechamento = movimento.HoraEntrada;
                    Caixa.Aberto = false;
                    try
                    {
                        await Caixa.UpdateCaixa(Caixa.Idcaixa, Caixa);
                    }
                    catch (BadResponseStatusCodeException ex)
                    {
                        MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    break;
            }

            if(Operacao != OperacaoCaixa.Fechamento)
            {
                movimento.CaixaIdcaixa = Caixa.Idcaixa;
                movimento.FormaPagamentoIdformaPagamento = MeioPagamento.IdformaPagamento;
                try
                {
                    await Caixa.CreateMovimento(movimento);
                }
                catch (BadResponseStatusCodeException ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            Close();
        }

        public async Task LoadFormaPagamentos()
        {
            List<FormaPagamento> formaPagamentos = await FormaPagamento.GetFormaPagamentos();
            itemsFormasPagamentos.ItemsSource = null;
            itemsFormasPagamentos.ItemsSource = formaPagamentos.OrderBy((e) => e.Ordem);
            await SetFormaPagamento(formaPagamentos[0]);
        }

        public async Task SetFormaPagamento(FormaPagamento meioPagamento)
        {
            MeioPagamento = meioPagamento;
            textblockMeioPagamento.Text = MeioPagamento.Nome;
        }

        private async void FormaPagamento_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            await SetFormaPagamento((FormaPagamento)senderAsButton.Tag);
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
