using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public partial class ItemVenda : Model<ItemVenda>
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
    }
}
