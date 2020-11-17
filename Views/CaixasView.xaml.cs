using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for CaixasView.xaml
    /// </summary>
    public partial class CaixasView : Window
    {
        public Caixa CaixaAberto { get; set; }
        private DateTime DataInicial { get; set; }
        private DateTime DataFinal { get; set; }

        public CaixasView()
        {
            InitializeComponent();
        }

        public async void OnLoad(object sender, RoutedEventArgs e)
        {
            AssignDataInicial(DateTime.UtcNow.AddDays(-7));
            AssignDataFinal(DateTime.UtcNow);

            DataFinal = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day,
                23, 59, 59);

            TextboxDataInicial.Text = DataInicial.ToString("dd/MM/yy");
            TextboxDataFinal.Text = DataFinal.ToString("dd/MM/yy");

            await LoadCaixaAberto();
            await LoadCaixas();
        }


        public void AssignDataInicial(DateTime dateTime)
        {
            DataInicial = new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                0, 0, 0);
        }

        public void AssignDataFinal(DateTime dateTime)
        {
            DataFinal = new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                23, 59, 59);
        }

        public async Task LoadCaixas()
        {
            try
            {
                DateTime _dataInicial = DateTime.ParseExact(TextboxDataInicial.Text, "dd/MM/yy", CultureInfo.InvariantCulture);
                DateTime _dataFinal = DateTime.ParseExact(TextboxDataFinal.Text, "dd/MM/yy", CultureInfo.InvariantCulture);
                AssignDataInicial(_dataInicial);
                AssignDataFinal(_dataFinal);

                List<Caixa> caixas = await new Caixa().FindAll(new Dictionary<string, string> {
                    {"filtroData","true"},
                    {"dataInicial",DataInicial.ToString()},
                    {"dataFinal",DataFinal.ToString() }
                });
                DatagridFechamentos.ItemsSource = null;
                DatagridFechamentos.ItemsSource = caixas;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task LoadCaixaAberto()
        {
            Caixa caixa = new Caixa();
            CaixaAberto = await caixa.GetCaixaAberto(new Dictionary<string, string> {
                { "movimentos","true"}
            });
            if(CaixaAberto != null)
            {
                gridTituloCaixaAberto.DataContext = CaixaAberto;
                await LoadMovimentos();
            }
            else
            {
                //MessageBox.Show("Nenhum caixa aberto.");
            }
            
        }

        public async Task LoadMovimentos()
        {
            if (CaixaAberto != null)
            {
                datagridMovimentacoes.ItemsSource = null;
                datagridMovimentacoes.ItemsSource = CaixaAberto.Movimento;
            }
        }

        private async void AdicionarMovimentoView_Closing(object sender, EventArgs e)
        {
            await LoadCaixaAberto();
        }

        private void ButtonAbrirCaixa_Click(object sender, RoutedEventArgs e)
        {
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento();
            adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.Show();
        }

        private void ButtonSuprimento_Click(object sender, RoutedEventArgs e)
        {
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(CaixaAberto,OperacaoCaixa.Suprimento);
            adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.Show();
        }


        private void ButtonFechamento_Click(object sender, RoutedEventArgs e)
        {
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(CaixaAberto, OperacaoCaixa.Sangria);
            adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.Show();
        }

        private void ButtonSangria_Click(object sender, RoutedEventArgs e)
        {
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(CaixaAberto, OperacaoCaixa.Sangria);
            adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.Show();
        }

        private async void ButtonFecharCaixa_Click(object sender, RoutedEventArgs e)
        {
            if(CaixaAberto != null)
            {
                if (await new Venda().HasVendaAberta())
                {
                    CaixaVendasAbertas vendasAbertas = new CaixaVendasAbertas();
                    vendasAbertas.Show();
                }
                AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(CaixaAberto, OperacaoCaixa.Fechamento);
                adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
                adicionarMovimentoView.Show();
            }
        }

        private async void DatagridFechamentos_MouseDoubleClick(object sender, SelectedCellsChangedEventArgs e)
        {
            if(DatagridFechamentos.SelectedItem != null)
            {
                Caixa itemAsCaixa = (Caixa)DatagridFechamentos.SelectedItem;
                Caixa caixa = await new Caixa().FindById(itemAsCaixa.Idcaixa, new Dictionary<string, string> {
                    {"movimentos","true" }
                });
                DatagridMovimentacoesFechamento.ItemsSource = null;
                DatagridMovimentacoesFechamento.ItemsSource = caixa.Movimento;
            }
        }
    }
}
