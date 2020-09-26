using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FortalezaDesktop.Models
{
    public partial class Item : Model<Item>
    {
        public override int? Id
        {
            get { return Iditem; }
            set { Iditem = value ?? default; }
        }

        public override string Path { get { return "/items"; } }

        public Estoque EstoqueAtual { get; set; }

        public async Task<bool> SaveEstoque(Estoque estoque)
        {
            try
            {
                await ServerEntry<Estoque>.Post(Path + "/" + Id + "/estoques", estoque);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
