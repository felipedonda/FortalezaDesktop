using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.Models
{
    public partial class Troca : Model<Troca>
    {
        public override string Path { get { return "/trocas"; } }

        public override int? Id
        {
            get { return Idvenda; }
            set { Idvenda = value ?? default; }
        }

        public decimal ValorTotal
        {
            get
            {
                decimal value = 0;
                if(TrocaHasItemVenda.Count > 0)
                {
                    foreach(var itemTroca in TrocaHasItemVenda)
                    {
                        value += itemTroca.Quantidade * itemTroca.IditemVendaNavigation.Valor ?? default;
                    }
                }
                return value;
            }
        }

        public void AddItemVenda(ItemVenda itemVenda, decimal quantidade)
        {
            bool jaExistente = false;

            foreach (var trocaItem in TrocaHasItemVenda)
            {
                if (trocaItem.IditemVendaNavigation.IditemVenda == itemVenda.IditemVenda)
                {
                    trocaItem.Quantidade += quantidade;
                    jaExistente = true;
                }
            }

            if (!jaExistente)
            {
                TrocaHasItemVenda.Add(new TrocaHasItemVenda
                {
                    Indice = TrocaHasItemVenda.Count + 1,
                    IditemVenda = itemVenda.IditemVenda,
                    IditemVendaNavigation = itemVenda,
                    Quantidade = quantidade
                });
            }
        }
    }
}
