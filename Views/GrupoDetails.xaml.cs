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
    /// Interaction logic for GrupoDetails.xaml
    /// </summary>
    public partial class GrupoDetails : Window
    {
        public event EventHandler AlteracaoRealizada;

        public GrupoDetails()
        {
            InitializeComponent();
            ButtonAlterar.Visibility = Visibility.Collapsed;
            gridGrupoDetails.DataContext = new Grupo();
        }

        public async Task<bool> LoadGrupo(int id)
        {
            ButtonAlterar.Visibility = Visibility.Visible;
            ButtonCriar.Visibility = Visibility.Collapsed;

            Grupo grupo = await new Grupo().FindById(id);
            if(grupo != null)
            {
                gridGrupoDetails.DataContext = grupo;
                return true;
            }
            return false;
        }

        private async void ButtonCriar_Click(object sender, RoutedEventArgs e)
        {
            Grupo grupo = (Grupo)gridGrupoDetails.DataContext;
            if (await grupo.SaveInstance())
            {
                Close();
                AlteracaoRealizada?.Invoke(this, new EventArgs());
            }
        }

        private async void ButtonAlterar_Click(object sender, RoutedEventArgs e)
        {
            Grupo grupo = (Grupo)gridGrupoDetails.DataContext;
            if (await grupo.UpdateInstance())
            {
                Close();
                AlteracaoRealizada?.Invoke(this, new EventArgs());
            }
        }

        private async void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
