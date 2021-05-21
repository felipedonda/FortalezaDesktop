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
    /// Interaction logic for BuscaVenda.xaml
    /// </summary>
    public partial class BuscaVenda : Window
    {
        public class VendaSelecionadaArgs : EventArgs
        {
            public Venda Venda { get; set; }
            public VendaSelecionadaArgs(Venda venda)
            {
                Venda = venda;
            }
        }

        public event EventHandler<VendaSelecionadaArgs> VendaSelecionada;

        public BuscaVenda()
        {
            InitializeComponent();
            DatePickerDataInicial.SelectedDate = DateTime.Now.AddMonths(-1);
            DatePickerDataFinal.SelectedDate = DateTime.Now;
        }

        public async Task LoadVendas()
        {
            List<Venda> vendas = await new Venda().FindAll(new Dictionary<string, string> {
                {"filtroData","true"},
                {"dataInicial", (DatePickerDataInicial.SelectedDate ?? default).ToString("yyyy-MM-dd") },
                {"dataFinal", (DatePickerDataFinal.SelectedDate ?? default).ToString("yyyy-MM-dd")}
            });
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = vendas;
        }

        public async Task LoadVendas(string query)
        {
            List<Venda> vendas = await new Venda().FindAll(new Dictionary<string, string> {
                {"filtroData","true"},
                {"dataInicial", (DatePickerDataInicial.SelectedDate ?? default).ToString("yyyy-MM-dd") },
                {"dataFinal", (DatePickerDataFinal.SelectedDate ?? default).ToString("yyyy-MM-dd")},
                {"query", query}
            });
            datagridItems.ItemsSource = null;
            datagridItems.ItemsSource = vendas;
        }

        private void ButtonAdicionar(object sender, RoutedEventArgs e)
        {
            SelecionarVenda();
        }

        private void SelecionarVenda()
        {
            if (datagridItems.SelectedItem != null)
            {
                Venda venda = (Venda)datagridItems.SelectedItem;
                VendaSelecionada?.Invoke(this, new VendaSelecionadaArgs(venda));
                Close();
            }
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void TextboxBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadVendas(TextboxBusca.Text);
        }

        private async void DatePicked(object sender, SelectionChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(TextboxBusca.Text))
            {
                await LoadVendas();
            }
            else
            {
                await LoadVendas(TextboxBusca.Text);
            }
        }

        private void datagridItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarVenda();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape | e.Key == Key.F11)
            {
                Close();
            }

            if (e.Key == Key.Return | e.Key == Key.F12)
            {
                if (datagridItems.SelectedItem == null)
                {
                    datagridItems.SelectedIndex = 0;
                }
                SelecionarVenda();
            }
        }
    }
}
