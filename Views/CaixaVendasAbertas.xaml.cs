using FortalezaDesktop.Models;
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

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for CaixaVendasAbertas.xaml
    /// </summary>
    public partial class CaixaVendasAbertas : Window
    {
        public CaixaVendasAbertas()
        {
            InitializeComponent();
        }

        private async void ButtonConfirmar_Click(object sender, RoutedEventArgs e)
        {
            Caixa caixa = await new Caixa().GetCaixaAberto();
            AdicionarMovimento adicionarMovimentoView = new AdicionarMovimento(caixa, OperacaoCaixa.Fechamento);
            //adicionarMovimentoView.Closing += AdicionarMovimentoView_Closing;
            adicionarMovimentoView.Show();
        }

        public async Task LoadVendasAbertas()
        {
            List<Venda> vendas = await new Venda().FindAll(new Dictionary<string, string> {
                {"abertas","true" }
            });
            DatagridVendasAbertas.ItemsSource = null;
            DatagridVendasAbertas.ItemsSource = vendas;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadVendasAbertas();
        }
    }
}
