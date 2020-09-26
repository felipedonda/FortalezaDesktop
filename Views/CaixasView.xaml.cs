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
        }

        public async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadCaixaAberto();
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

        private void ButtonFecharCaixa_Click(object sender, RoutedEventArgs e)
        {
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(CaixaAberto, OperacaoCaixa.Fechamento);
            adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.Show();
        }
    }
}
