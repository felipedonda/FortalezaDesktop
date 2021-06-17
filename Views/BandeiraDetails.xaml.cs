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
using FortalezaDesktop.Models;
using FortalezaDesktop.Controllers;
using FortalezaDesktop.Utils;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for BandeiraDetails.xaml
    /// </summary>
    public partial class BandeiraDetails : Window
    {
        public event EventHandler AlteracaoRealizada;
        public bool ModelLoaded { get; set; }
        public Bandeira Model { get; set; }

        public BandeiraDetails()
        {
            InitializeComponent();
            ButtonAlterar.Visibility = Visibility.Collapsed;
            ModelLoaded = false;
            Model = new Bandeira();
            GridPrincipal.DataContext = Model;
        }

        public async Task<bool> Load(int id)
        {
            

            Bandeira model = await BandeirasController.GetBandeiraByIdAsync(id);
            if (model != null)
            {
                ButtonAlterar.Visibility = Visibility.Visible;
                ButtonCriar.Visibility = Visibility.Collapsed;
                ModelLoaded = true;
                Title = model.Nome;
                Model = model;
                GridPrincipal.DataContext = null;
                GridPrincipal.DataContext = Model;
                return true;
            }
            return false;
        }

        private async void ButtonCriar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await BandeirasController.CreateBandeiraAsync(Model);
                AlteracaoRealizada?.Invoke(this, new EventArgs());
                Close();
            }
            catch(ApiException ex)
            {
                ErrorHandler.ApiGenericErrorHandler(ex);
            }
        }

        private async void ButtonAlterar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await BandeirasController.UpdateBandeiraAsync(Model);
                AlteracaoRealizada?.Invoke(this, new EventArgs());
                Close();
            }
            catch (ApiException ex)
            {
                ErrorHandler.ApiGenericErrorHandler(ex);
            }
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ModelLoaded)
            {
                Model.Ordem = await Model.GetOrdem();
                GridPrincipal.DataContext = null;
                GridPrincipal.DataContext = Model;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
