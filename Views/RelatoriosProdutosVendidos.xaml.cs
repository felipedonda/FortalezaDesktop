using FortalezaDesktop.Models;
using FortalezaDesktop.Views.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for RelatoriosProdutosVendidos.xaml
    /// </summary>
    public partial class RelatoriosProdutosVendidos : Window
    {
        public RelatoriosProdutosVendidos()
        {
            InitializeComponent();
        }

        private void ButtonCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonOk(object sender, RoutedEventArgs e)
        {
            TemplateRelatorioProdutosVendidos relatorio = new TemplateRelatorioProdutosVendidos();
            relatorio.textSubtitulo.Text =
                "Período de: " + (datePickerFrom.SelectedDate ?? default).ToString("dd/MM/yy")
                + " até: " + (datePicker.SelectedDate ?? default).ToString("dd/MM/yy");
            List<ItemVenda> itemsVendidos = await (new ItemVenda()).FindAll( new Dictionary<string, string> {
                {"filtroData", "true"},
                {"filtroDataInicio", (datePickerFrom.SelectedDate ?? default).ToString()},
                {"filtroDataFinal", (datePicker.SelectedDate ?? default).ToString()}
            });

            if(checkboxCustoUnidade.IsChecked ?? false)
            {
                itemsVendidos = itemsVendidos
                    .GroupBy(e => e.Iditem)
                    .Select(e => new ItemVenda
                    {
                        IditemNavigation = e.FirstOrDefault().IditemNavigation,
                        Custo = e.Sum(v => v.Custo)/e.Count(),
                        Quantidade = e.Sum(v => v.Quantidade),
                        Valor = e.Sum(v => v.Valor) / e.Count()
                    })
                    .ToList();
            }

            relatorio.mainGrid.ItemsSource = itemsVendidos;

            RelatoriosVizualizador vizualizador = new RelatoriosVizualizador(RelatoriosVizualizador.TipoEmpressaoEnum.Relatorio);
            vizualizador.LoadChildPage(relatorio);
            vizualizador.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            DateTime dataInicial = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                0, 0, 0).AddDays(-1);

            DateTime dataFinal = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                23, 59, 59);

            datePickerFrom.SelectedDate = dataInicial;
            datePicker.SelectedDate = dataFinal;
        }
    }
}
