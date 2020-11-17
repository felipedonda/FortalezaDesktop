using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FortalezaDesktop.Views.Relatorios;
using System.Threading.Tasks;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for DeliveryDetails.xaml
    /// </summary>
    public partial class DeliveryDetails : Page
    {
        public Pedido Pedido { get; set; }

        public DeliveryDetails()
        {
            InitializeComponent();
        }

        public async Task LoadPedido(int idpedido)
        {
            Pedido = await new Pedido().FindById(idpedido, new Dictionary<string, string> {
                {"itemvenda","true"},
                {"pagamento","true" }
            });
            gridPrincipal.DataContext = Pedido;
            await LoadCupomPedido();
        }

        private void ButtonMap_Click(object sender, RoutedEventArgs e)
        {

        }

        public async Task LoadCupomPedido()
        {
            if(Pedido != null)
            {
                TemplateDeliveryDetails cupomRelatorio = new TemplateDeliveryDetails();
                cupomRelatorio.mainGrid.ItemsSource = Pedido.IdvendaNavigation.ItemVenda;
                cupomRelatorio.datagridPagamentos.ItemsSource = Pedido.IdvendaNavigation.Pagamento;
                frameCupomFiscal.NavigationService.RemoveBackEntry();
                frameCupomFiscal.Navigate(cupomRelatorio);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
