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
using FortalezaDesktop.Utils;

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

        public Page LoadedPage { get; set; }

        public async void LoadChildPage(Page page)
        {
            Server.LoadServerConfig();
            frameChildContainer.NavigationService.RemoveBackEntry();
            if (await Server.CheckConnection())
            {
                LoadedPage = page;
                frameChildContainer.Navigate(page);
            }
            else
            {
                Logger.Log("Falha de conexão com o servidor '" + Server.Uri + "'.", Logger.LogType.Warning);
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

        private bool Loggar()
        {
            if (UserController.UsuarioLogado == null)
            {
                UserController.Loggar("", "");
                return Loggar();
            }
            else
            {
                return true;
            }
        }

        private async Task<bool> InitialConnection()
        {
            if (await Server.CheckConnection())
            {
                return true;
            }
            else
            {
                Logger.Log("Falha de conexão com o servidor '" + Server.Uri + "'.", Logger.LogType.Warning);
                var msgBoxResult = MessageBox.Show("Falha de conexão com servidor. Tentar novamente?", "Falha de conexão", MessageBoxButton.OKCancel);
                if (msgBoxResult == MessageBoxResult.OK)
                {
                    return await InitialConnection();
                }
                else
                {
                    return false;
                }
            }
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            Server.LoadServerConfig();
            Logger.Log("Sistema iniciado.", Logger.LogType.Info);
            UserPreferences.Load();

            if (!UserPreferences.Preferences.ModuloVenda)
            {
                ButtonVenda.Visibility = Visibility.Collapsed;
            }

            if (!UserPreferences.Preferences.ModuloPedido)
            {
                ButtonPedido.Visibility = Visibility.Collapsed;
            }

            if (!UserPreferences.Preferences.ModuloDelivery)
            {
                ButtonDelivery.Visibility = Visibility.Collapsed;
            }

            if (!UserPreferences.Preferences.ModuloTroca)
            {
                ButtonTroca.Visibility = Visibility.Collapsed;
            }

            if (await InitialConnection())
            {
                Logger.Log("Conexão feita com servidor '" + Server.Uri + "'.", Logger.LogType.Info);
                if (Loggar())
                {
                    LoadChildPage(new VendaView());
                }
            }
        }

        private void buttonMenuGerencial_Click(object sender, RoutedEventArgs e)
        {
            MenuGerencial menuGerencialView = new MenuGerencial();
            menuGerencialView.Closed += MenuGerencialView_UpdateParent;
            menuGerencialView.ShowDialog();
        }

        private void MenuGerencialView_UpdateParent(object sender, EventArgs e)
        {
            if (!UserPreferences.Preferences.ModuloVenda)
            {
                ButtonVenda.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonVenda.Visibility = Visibility.Visible;
            }

            if (!UserPreferences.Preferences.ModuloPedido)
            {
                ButtonPedido.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonPedido.Visibility = Visibility.Visible;
            }

            if (!UserPreferences.Preferences.ModuloDelivery)
            {
                ButtonDelivery.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonDelivery.Visibility = Visibility.Visible;
            }

            if (!UserPreferences.Preferences.ModuloTroca)
            {
                ButtonTroca.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonTroca.Visibility = Visibility.Visible;
            }

            ReloadPage();
        }

        public void ReloadPage()
        {
            Type pageType = LoadedPage.GetType();
            Page newPage = (Page)Activator.CreateInstance(pageType);
            LoadChildPage(newPage);
        }

        public void ButtonCaixa_Click(object sender, RoutedEventArgs e)
        {
            CaixasView caixasView = new CaixasView();
            caixasView.ShowDialog();
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

        private void ButtonTroca_Click(object sender, RoutedEventArgs e)
        {
            LoadChildPage(new TrocaView());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Logger.Log("Sistema desligando.", Logger.LogType.Info);
        }
    }
}
