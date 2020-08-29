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
    public class Caixa
    {
        public int Idcaixa { get; set; }
        public string Nome { get; set; }
        public DateTime HoraAbertura { get; set; }
        public DateTime HoraFechamento { get; set; }
        public decimal SaldoAbertura { get; set; }
        public decimal SaldoFechamento { get; set; }
        public int Idresponsavel { get; set; }

        [JsonIgnore]
        public Usuario Responsavel { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Aberto { get; set; }

        public static async Task<List<Caixa>> GetCaixas()
        {
            return await Model<List<Caixa>>.Get("/caixa");
        }

        public static async Task<Caixa> GetCaixa(int id)
        {
            return await Model<Caixa>.Get("/caixa",id);
        }

        public static async Task<Caixa> CreateCaixa(Caixa caixa)
        {
            return await Model<Caixa>.Post("/caixa",caixa);
        }

        public static async Task<bool> UpdateCaixa(int id, Caixa caixa)
        {
            return await Model<Caixa>.Put("/caixa",id ,caixa);
        }

        public static async Task<bool> DeleteCaixa(int id)
        {
            return await Model<Caixa>.Delete("/caixa", id);
        }

        public static async Task<Caixa> GetCaixaAberto()
        {
            return await Model<Caixa>.Get("/caixa/actions/aberto");
        }

        public static async Task<List<Movimento>> GetMovimentos(int id)
        {
            return await Model<List<Movimento>>.Get("/caixa/" + id + "/movimento");
        }

        public async Task<List<Movimento>> GetMovimentos()
        {
            return await GetMovimentos(Idcaixa);
        }


        public static async Task<Movimento> GetMovimento(int idCaixa, int idMovimento)
        {
            return await Model<Movimento>.Get("/caixa/" + idCaixa + "/movimento", idMovimento);
        }

        public static async Task<Movimento> CreateMovimento(int idCaixa, Movimento movimento)
        {
            return await Model<Movimento>.Post("/caixa/" + idCaixa + "/movimento", movimento);
        }

        public async Task<Movimento> CreateMovimento(Movimento movimento)
        {
            return await CreateMovimento(Idcaixa, movimento);
        }

    }
}
