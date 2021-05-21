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
    /// Interaction logic for PedidoDetailsPagamentos.xaml
    /// </summary>
    public partial class PedidoDetailsPagamentos : Window
    {
        public Venda Venda { get; set; }
        public PedidoDetailsPagamentos(Venda venda)
        {
            Venda = venda;
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPagamentos();
        }

        private async Task LoadPagamentos()
        {
            Venda = await Venda.ReloadInstance(new Dictionary<string, string> {
                { "pagamentos","true" }
            });
            List<Movimento> movimentos = Venda.Pagamento.Select(e => e.IdmovimentoNavigation).ToList();
            datagridPrincipal.ItemsSource = null;
            datagridPrincipal.ItemsSource = movimentos;
        }

        private async void buttonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            Caixa caixaAberto = await new Caixa().GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
            if (caixaAberto != null)
            {
                PedidoDetailsPagamentosAdd pagamentosAdd = new PedidoDetailsPagamentosAdd(Venda, caixaAberto);
                pagamentosAdd.Closed += PagamentosAdd_Closed;
                pagamentosAdd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nenhum caixa aberto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private async void PagamentosAdd_Closed(object sender, EventArgs e)
        {
            await LoadPagamentos();
        }

        private async void buttonRemover_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            Movimento movimento = await new Movimento().FindById((int)senderAsButton.Tag);
            MessageBoxResult result = MessageBox.Show(
                "Deseja realmente remover esse adiantamento?",
                "Remover Adiantamento",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await movimento.DeleteInstance();
                    await LoadPagamentos();
                }
                catch (BadResponseStatusCodeException ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
        }

        private async void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;

            Caixa caixaAberto = await new Caixa().GetCaixaAberto(UserPreferences.Preferences.IdnomeCaixa);
            if (caixaAberto != null)
            {
                PedidoDetailsPagamentosAdd pagamentosAdd = new PedidoDetailsPagamentosAdd((int)senderAsButton.Tag, Venda, caixaAberto);
                pagamentosAdd.Closed += PagamentosAdd_Closed;
                pagamentosAdd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nenhum caixa aberto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
