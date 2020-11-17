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

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for GruposView.xaml
    /// </summary>
    public partial class GruposView : Window
    {
        public GruposView()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadGrupos();
        }

        public async Task LoadGrupos()
        {
            var Grupos = await new Grupo().FindAll();
            datagridGrupos.ItemsSource = null;
            datagridGrupos.ItemsSource = Grupos;
        }

        private void buttonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            GrupoDetails grupoDetails = new GrupoDetails();
            grupoDetails.AlteracaoRealizada += GrupoDetails_AlteracaoRealizada;
            grupoDetails.Show();
        }

        private async void GrupoDetails_AlteracaoRealizada(object sender, EventArgs e)
        {
            await LoadGrupos();
        }

        private async void ButtonRemoverTabela_Click(object sender, RoutedEventArgs e)
        {
            Grupo grupo = new Grupo();
            Button senderAsButton = (Button)sender;
            grupo = await grupo.FindById((int)senderAsButton.Tag);
            var result = MessageBox.Show(
                "Deseja realmente remover o grupo " + grupo.Nome + "?",
                "Remover Grupo",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
               await grupo.DeleteInstance();
               await LoadGrupos();
            }
        }

        private async void ButtonEditTabela_Click(object sender, RoutedEventArgs e)
        {
            Button senderAsButton = (Button)sender;
            GrupoDetails grupoDetails = new GrupoDetails();
            grupoDetails.AlteracaoRealizada += GrupoDetails_AlteracaoRealizada;
            await grupoDetails.LoadGrupo((int)senderAsButton.Tag);
            grupoDetails.Show();
        }
    }
}
