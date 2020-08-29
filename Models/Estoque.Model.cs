using FortalezaDesktop.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public class Estoque
    {
        public int? Idestoque { get; set; }
        public DateTime HoraEntrada { get; set; }
        public string NotaFiscal { get; set; }
        public string Observacao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeDisponivel { get; set; }
        public decimal Custo { get; set; }
        public int? ItemIditem { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Saida { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool OrigemVenda { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Disponivel { get; set; }


        [JsonIgnore]
        public int VendaIdvenda { get; set; }
            
        [JsonIgnore]
        public Venda Venda { get; set; }

        public Estoque()
        {
            OrigemVenda = false;
            Saida = false;
            Disponivel = false;
        }

        public async Task SaveInstance()
        {
            await CreateEstoque(this);
        }

        public async Task UpdateInstance()
        {
            await UpdateEstoque(Idestoque ?? default, this);
        }

        public static async Task<List<Estoque>> GetEstoques()
        {
            return await Model<List<Estoque>>.Get("/estoque");
        }

        public static async Task<Estoque> GetEstoque(int id)
        {
            return await Model<Estoque>.Get("/estoque",id);
        }


        public static async Task<Estoque> CreateEstoque(Estoque estoque)
        {
            return await Model<Estoque>.Post("/estoque", estoque);
        }


        public static async Task<bool> UpdateEstoque(int id, Estoque estoque)
        {
            return await Model<Estoque>.Put("/estoque/" + id, estoque);
        }
    }
}
