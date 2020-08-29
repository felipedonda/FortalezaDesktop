using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using FortalezaDesktop.Models;
using FortalezaDesktop.Utils;
using FortalezaDesktop;

namespace FortalezaDesktop.Models
{
    public class ItemVenda
    {
        public int IditemVenda { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal Custo { get; set; }
        public int VendaIdvenda { get; set; }
        public int ItemIditem { get; set; }

        [JsonIgnore]
        public decimal Lucro { get; set; }

        [JsonIgnore]
        public decimal LucroTotal { get; set; }

        [JsonIgnore]
        public decimal ValorTotal {get; set;}

        [JsonIgnore]
        public decimal CustoTotal { get; set; }

        [JsonIgnore]
        public Item Item { get; set; }

        public async Task CalculateLucro()
        {
            await Task.Run(() => {
                Lucro = Valor - Custo;
                LucroTotal = Lucro * Quantidade;
            });
        }

        public async Task CalculateTotais()
        {
            await Task.Run(() => {
                ValorTotal = Valor * Quantidade;
                CustoTotal = Custo * Quantidade;
            });
        }

        public async Task LoadItem()
        {
            Item = await Item.GetItem(ItemIditem);
        }

        public static async Task<List<ItemVenda>> GetItemVendas()
        {
            return await Model<List<ItemVenda>>.Get("/item-venda");
        }
    }
}
