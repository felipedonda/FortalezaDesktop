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
using FortalezaDesktop.Controllers;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for RelatoriosVizualizador.xaml
    /// </summary>
    public partial class RelatoriosVizualizador : Window
    {
        public enum TipoEmpressaoEnum {
            Cupom,
            Cozinha,
            Relatorio
        }
        public Page LoadedPage { get; set; }
        public TipoEmpressaoEnum TipoEmpressao { get; set; }

        public RelatoriosVizualizador(TipoEmpressaoEnum tipoEmpressao)
        {
            InitializeComponent();
            TipoEmpressao = tipoEmpressao;
        }

        public void LoadChildPage(Page page)
        {
            LoadedPage = page;
            frameRelatorio.NavigationService.RemoveBackEntry();
            frameRelatorio.Navigate(LoadedPage);
        }

        public void Print()
        {
            if (LoadedPage != null)
            {
                switch(TipoEmpressao)
                {
                    case TipoEmpressaoEnum.Cupom:
                        PrintersController.PrintCupom(LoadedPage);
                        break;
                    case TipoEmpressaoEnum.Cozinha:
                        PrintersController.PrintCozinha(LoadedPage);
                        break;
                    case TipoEmpressaoEnum.Relatorio:
                        PrintersController.PrintRelatorio(LoadedPage);
                        break;
                }
            }
        }

        private async void ButtonImprimir(object sender, RoutedEventArgs e)
        {
            Print();
        }
    }
}
