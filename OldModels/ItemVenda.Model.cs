using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.OldModels
{
    public partial class ItemVenda : OldModel<ItemVenda>
    {
        public override string Path { get { return "/itemvendas"; } }

        public override int? Id
        {
            get { return IditemVenda; }
            set { IditemVenda = value ?? default; }
        }

        public decimal ValorTotal {
            get
            {
                return Quantidade * Valor ?? default;
            }
        }

        public decimal ValorTotalDisponivel
        {
            get
            {
                return QuantidadeDisponivel * Valor ?? default;
            }
        }

        public decimal ValorTotalCancelado
        {
            get
            {
                return Cancelado * Valor ?? default;
            }
        }

        public decimal CustoTotal {
            get
            {
                return Quantidade * Custo ?? default;
            }
        }
        public decimal LucroTotal {
            get
            {
                return ValorTotal - CustoTotal;
            }
        }

        public decimal QuantidadeDisponivel
        {
            get
            {
                return Quantidade - Cancelado;
            }
        }
    }
}
