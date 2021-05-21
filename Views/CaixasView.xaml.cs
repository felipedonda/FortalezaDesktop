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

        public CaixasView()
        {
            InitializeComponent();
            DatePickerDataInicial.SelectedDate = DateTime.Now.AddDays(-7);
            DatePickerDataFinal.SelectedDate = DateTime.Now;
        }

        public async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadCaixaAberto();
            await LoadCaixas();
        }


        public DateTime GetDataInicial()
        {
            return DatePickerDataInicial.SelectedDate ?? default;
        }

        public DateTime GetDataFinal()
        {
            return DatePickerDataFinal.SelectedDate ?? default;
        }

        public async Task LoadCaixas()
        {
            try
            {

                List<Caixa> caixas = await new Caixa().FindAll(new Dictionary<string, string> {
                    {"filtroData","true"},
                    {"dataInicial",GetDataInicial().ToString("yyyy-MM-dd")},
                    {"dataFinal",GetDataFinal().ToString("yyyy-MM-dd") }
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
            CaixaAberto = await new Caixa().GetCaixaAberto(
                UserPreferences.Preferences.IdnomeCaixa,
                new Dictionary<string, string> {
                { "movimentos","true"}
            });
            if(CaixaAberto != null)
            {
                gridTituloCaixaAberto.DataContext = CaixaAberto;
                ButtonFecharCaixa.IsEnabled = true;
                await LoadMovimentos();
            }
            else
            {
                gridTituloCaixaAberto.DataContext = null;
                ButtonFecharCaixa.IsEnabled = false;
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
            await LoadCaixas();
        }

        private void ButtonAbrirCaixa_Click(object sender, RoutedEventArgs e)
        {
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(AdicionarMovimento.TipoMovimento.Abertura);
            adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.ShowDialog();
        }

        private void ButtonSuprimento_Click(object sender, RoutedEventArgs e)
        {
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(AdicionarMovimento.TipoMovimento.Suprimento);
            adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.ShowDialog();
        }

        private void ButtonSangria_Click(object sender, RoutedEventArgs e)
        {
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(AdicionarMovimento.TipoMovimento.Sangria);
            adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.ShowDialog();
        }

        private async void ButtonFecharCaixa_Click(object sender, RoutedEventArgs e)
        {
            if(CaixaAberto != null)
            {
                if (await new Venda().HasVendaAberta())
                {
                    CaixaVendasAbertas vendasAbertas = new CaixaVendasAbertas();
                    vendasAbertas.ShowDialog();
                }
                else
                {
                    // IMPLEMENTAR SALDO PRÉ FECHAMENTO
                    var result = MessageBox.Show(
                        "Deseja realmente fechar o caixa?",
                        "Fechar Caixa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if(await CaixaAberto.FecharCaixa(UserController.UsuarioLogado.Idusuario, UserPreferences.Preferences.Idpdv))
                        {
                            CaixaAberto = null;
                            gridTituloCaixaAberto.DataContext = null;
                            ButtonFecharCaixa.IsEnabled = false;
                        }
                    }
                }
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

        private async void DatePicked(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded)
            {
                await LoadCaixas();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
