using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using FortalezaDesktop.Views;
using FortalezaDesktop.Views.Relatorios;

namespace FortalezaDesktop.OldModels
{
    public partial class Pedido : OldModel<Pedido>
    {
        public override string Path { get { return "/pedidos"; } }

        public override int? Id
        {
            get { return Idvenda; }
            set { Idvenda = value ?? default; }
        }

        public async Task<bool> LoadTaxaAplicavel(Dictionary<string, string> options = null)
        {
            try
            {
                ItemVenda itemVenda = await ServerEntry<ItemVenda>.Get(Path + "/" + Id + "/taxaentrega" , options);
                if(itemVenda != null)
                {
                    await IdvendaNavigation.SaveItemVenda(itemVenda);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        [JsonIgnore]
        public ItemVenda TaxaEntrega
        {
            get
            {
                if(IdvendaNavigation != null)
                {
                    if (IdvendaNavigation.ItemVenda != null)
                    {
                        if (IdvendaNavigation.ItemVenda.Count > 0)
                        {
                            return IdvendaNavigation.ItemVenda
                                .Where(e => e.IditemNavigation != null)
                                .Where(e => e.IditemNavigation.Tipo == 9)
                                .FirstOrDefault();
                        }
                    }
                }
                return null;
            }
            set
            {
                if (IdvendaNavigation.ItemVenda != null)
                {
                    if (IdvendaNavigation.ItemVenda.Count > 0)
                    {
                        var itemVenda = IdvendaNavigation.ItemVenda
                            .Where(e => e.IditemNavigation.Tipo == 9);

                        if(itemVenda.Count() > 0)
                        {
                            IdvendaNavigation.ItemVenda.Remove(itemVenda.First());
                            IdvendaNavigation.ItemVenda.Add(value);
                        }
                    }
                }
            }
        }

        [JsonIgnore]
        public string StatusView
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "Novo Pedido";
                    case 2:
                        return "Pedido Faturado";
                    case 3:
                        return "Saiu para Entrega";
                    case 4:
                        return "Pedido Concluído";
                    case 5:
                        return "Cancelado";
                }
                return null;
            }
        }
    }
}
