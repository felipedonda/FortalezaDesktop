using FortalezaDesktop.Models;
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
    /// Interaction logic for ProdutoDetailsGruposView.xaml
    /// </summary>
    ///


    public partial class ProdutoDetailsGrupos : Window
    {
        private class GrupoGridItem
        {
            public Grupo Grupo { get; set; }
            public bool Selected { get; set; }
        }

        public List<Grupo> SelectedGrupos { get; set; }
        public ProdutoDetailsGrupos(List<Grupo> grupos)
        {
            if(grupos != null)
            {
                SelectedGrupos = grupos;
            }
            else
            {
                SelectedGrupos = new List<Grupo>();
            }
            
            InitializeComponent();
        }
        public async void OnLoad(object sender, RoutedEventArgs e)
        {
            List<Grupo> grupos = await Grupo.GetGrupos();
            List<GrupoGridItem> grupoGridItems = grupos.Select(g => new GrupoGridItem { Grupo = g, Selected = false}).ToList();
            foreach(Grupo grupo in SelectedGrupos)
            {
                foreach(GrupoGridItem grupoGrid in grupoGridItems)
                {
                    if(grupoGrid.Grupo.Idgrupo == grupo.Idgrupo)
                    {
                        grupoGrid.Selected = true;
                    }
                }
            }
            datagridGrupos.ItemsSource = grupoGridItems;
        }

        private void ButtonConcluir_Click(object sender, RoutedEventArgs e)
        {
            SelectedGrupos = ((List<GrupoGridItem>)datagridGrupos.ItemsSource).Where(g => g.Selected == true).Select(g => g.Grupo).ToList();
            Close();
        }
    }
}
