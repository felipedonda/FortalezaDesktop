using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
    /// Interaction logic for RelatoriosVizualizador.xaml
    /// </summary>
    public partial class RelatoriosVizualizador : Window
    {
        public Page LoadedPage { get; set; }
        public RelatoriosVizualizador()
        {
            InitializeComponent();
        }

        public async void LoadChildPage(Page page)
        {
            LoadedPage = page;
            frameRelatorio.NavigationService.RemoveBackEntry();
            frameRelatorio.Navigate(LoadedPage);
        }

        public async Task Print()
        {
            if(LoadedPage != null)
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.ShowDialog();
                printDialog.PrintVisual(LoadedPage, "");
            }
        }

        private async void ButtonImprimir(object sender, RoutedEventArgs e)
        {
            await Print();
        }
    }
}
