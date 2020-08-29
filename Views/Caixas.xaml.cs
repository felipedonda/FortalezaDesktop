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
    public partial class Caixas : Window
    {
        public Caixa CaixaAberto { get; set; }

        public Caixas()
        {
            InitializeComponent();
        }

        public async void OnLoad(object sender, RoutedEventArgs e)
        {
            await LoadCaixaAberto();
        }

        public async Task LoadCaixaAberto()
        {
            CaixaAberto = await Caixa.GetCaixaAberto();
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
                List<Movimento> movimentos = await Caixa.GetMovimentos(CaixaAberto.Idcaixa);
                foreach(Movimento movimento in movimentos)
                {
                    await movimento.LoadFormaPagamento();
                }
                datagridMovimentacoes.ItemsSource = null;
                datagridMovimentacoes.ItemsSource = movimentos;
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
    }
}
