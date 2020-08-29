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
    public class FormaPagamento
    {
        public int IdformaPagamento { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public bool DebitarCliente { get; set; }
        public bool GerarContasReceber { get; set; }
        public bool PossuiBandeira { get; set; }
        public bool Debito { get; set; }

        public static async Task<List<FormaPagamento>> GetFormaPagamentos()
        {
            return await Model<List<FormaPagamento>>.Get("/forma-pagamento");
        }

        public static async Task<FormaPagamento> GetFormaPagamento(int id)
        {
            return await Model<FormaPagamento>.Get("/forma-pagamento", id);
        }
    }
}
