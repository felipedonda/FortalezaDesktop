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
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for FormaPagamentosView.xaml
    /// </summary>
    public partial class FormaPagamentosView : Window
    {
        public FormaPagamentosView()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadFormaPagamentos();
            await LoadBandeiras();
        }

        public async Task LoadFormaPagamentos()
        {
            List<FormaPagamento> formaPagamentos = await new FormaPagamento().FindAll();
            DatagridFormasPagamentos.ItemsSource = null;
            DatagridFormasPagamentos.ItemsSource = formaPagamentos;
        }

        public async Task LoadBandeiras()
        {
            List<Bandeira> bandeiras = await new Bandeira().FindAll();
            DatagridBandeiras.ItemsSource = null;
            DatagridBandeiras.ItemsSource = bandeiras;
        }

        private async void ButtonRemoverFormaPagamento_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            FormaPagamento model = await new FormaPagamento().FindById((int)senderAsButton.Tag);
            var result = MessageBox.Show(
                "Deseja realmente a forma de pagamento " + model.Nome + "?",
                "Remover Forma Pagamento",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await model.DeleteInstance();
                await LoadFormaPagamentos();
            }
        }

        private async void ButtonEditFormaPagamento_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            FormaPagamentoDetails model = new FormaPagamentoDetails();
            model.AlteracaoRealizada += FormaPagamento_AlteracaoRealizada; ;
            await model.Load((int)senderAsButton.Tag);
            model.ShowDialog();
        }

        private async void FormaPagamento_AlteracaoRealizada(object sender, EventArgs e)
        {
            await LoadFormaPagamentos();
        }

        private void ButtonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            if(TabControlMain.SelectedIndex == 0)
            {
                FormaPagamentoDetails form = new FormaPagamentoDetails();
                form.AlteracaoRealizada += FormaPagamento_AlteracaoRealizada;
                form.ShowDialog();
            }
            else
            {
                BandeiraDetails form = new BandeiraDetails();
                form.AlteracaoRealizada += BandeiraDetails_AlteracaoRealizada;
                form.ShowDialog();
            }
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonRemoverBandeira_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            Bandeira model = await new Bandeira().FindById((int)senderAsButton.Tag);
            var result = MessageBox.Show(
                "Deseja realmente a bandeira " + model.Nome + "?",
                "Remover Bandeira",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await model.DeleteInstance();
                await LoadBandeiras();
            }
        }

        private async void ButtonEditBandeira_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            BandeiraDetails BandeiraDetails = new BandeiraDetails();
            BandeiraDetails.AlteracaoRealizada += BandeiraDetails_AlteracaoRealizada; ;
            await BandeiraDetails.Load((int)senderAsButton.Tag);
            BandeiraDetails.ShowDialog();
        }

        private async void BandeiraDetails_AlteracaoRealizada(object sender, EventArgs e)
        {
            await LoadBandeiras();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
