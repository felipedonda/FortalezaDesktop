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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FortalezaDesktop.Views;

namespace FortalezaDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async void LoadChildPage(Page page)
        {
            Server.LoadServerConfig();
            frameChildContainer.NavigationService.RemoveBackEntry();
            if (await Server.CheckConnection())
            {
                frameChildContainer.Navigate(page);
            }
            else
            {
                var msgBoxResult = MessageBox.Show("Falha de conexão com servidor. Tentar novamente?", "Falha de conexão" ,MessageBoxButton.OKCancel);
                if (msgBoxResult == MessageBoxResult.OK)
                {
                    LoadChildPage(page);
                }
                else
                {
                    Close();
                }
            }
        }

        bool Loggar()
        {
            if (UserControl.UsuarioLogado == null)
            {
                UserControl.Loggar("", "");
                return Loggar();
            }
            else
            {
                return true;
            }
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            UserPreferences.Load();
            if(Loggar())
            {
                LoadChildPage(new VendaView());
            }
        }

        private void buttonMenuGerencial_Click(object sender, RoutedEventArgs e)
        {
            MenuGerencial menuGerencialView = new MenuGerencial();
            menuGerencialView.UpdateParent += MenuGerencialView_UpdateParent;
            menuGerencialView.Show();
        }

        private void MenuGerencialView_UpdateParent(object sender, EventArgs e)
        {
            LoadChildPage(new VendaView());
        }

        public void ButtonCaixa_Click(object sender, RoutedEventArgs e)
        {
            CaixasView caixasView = new CaixasView();
            caixasView.Show();
        }

        private void ButtonPedido_Click(object sender, RoutedEventArgs e)
        {
            LoadChildPage(new PedidosView());
        }

        private void ButtonVenda_Click(object sender, RoutedEventArgs e)
        {
            LoadChildPage(new VendaView());
        }

        private void ButtonDelivery_Click(object sender, RoutedEventArgs e)
        {
            LoadChildPage(new DeliveryView());
        }
    }
}
