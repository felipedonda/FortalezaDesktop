using FortalezaDesktop.Models;
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

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for VendaSelecaoProdutos.xaml
    /// </summary>
    public partial class VendaSelecaoProdutos : Page
    {
        public class ProdutoSelectedEventArgs : EventArgs
        {
            public int Iditem {get; set;}
            public ProdutoSelectedEventArgs(int iditem)
            {
                Iditem = iditem;
            }
        }

        public event EventHandler<ProdutoSelectedEventArgs> ProdutoSelected;

        public VendaSelecaoProdutos()
        {
            InitializeComponent();
        }

        public async Task LoadGrupos()
        {
            Grupo _grupo = new Grupo();
            List<Grupo> grupos = await _grupo.FindAll();
            if(grupos != null)
            {
                if (UserPreferences.Preferences.GrupoTodos)
                {
                    Grupo grupoTodos = new Grupo
                    {
                        Visivel = 1,
                        Nome = "Todos",
                        Idgrupo = -1
                    };
                    grupos.Insert(0, grupoTodos);
                }
                itemsControlGrupos.ItemsSource = grupos.Where(e => e.Visivel == 1);
                await SelectGrupo(grupos[0].Idgrupo);
            }
        }

        public async Task SelectGrupo(int idgrupo)
        {
            List<Item> items = new List<Item>();
            if (idgrupo == -1)
            {
                items = await (new Item()).FindAll();
            }
            else
            {
                Grupo _grupo = new Grupo();
                _grupo = await _grupo.FindById(idgrupo, new Dictionary<string, string>() { { "items", "true" } });
                items = _grupo.ItemHasGrupo.Select(e => e.IditemNavigation).ToList();
            }
            itemsControlProdutos.ItemsSource = items;
        }

        public async void Grupo_Click(object sender, RoutedEventArgs e)
        {
            await SelectGrupo((int)((Button)sender).Tag);
        }

        public void Produto_Click(object sender, RoutedEventArgs e)
        {
            Item item = (Item)((Button)sender).Tag;
            ProdutoSelected?.Invoke(this, new ProdutoSelectedEventArgs(item.Iditem));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadGrupos();
        }
    }
}
