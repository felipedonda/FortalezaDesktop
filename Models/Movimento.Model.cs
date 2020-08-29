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
    public class Movimento
    {
        public int? Idmovimento { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime HoraEntrada { get; set; }
        public int CaixaIdcaixa { get; set; }
        public int Responsavel { get; set; }
        public int FormaPagamentoIdformaPagamento { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Debito { get; set; }


        [JsonIgnore]
        public FormaPagamento FormaPagamento { get; set; }

        [JsonIgnore]
        public Bandeira Bandeira { get; set; }

        [JsonIgnore]
        public int BandeiraIdbandeira { get; set; }

        public async Task LoadFormaPagamento()
        {
            FormaPagamento = await FormaPagamento.GetFormaPagamento(FormaPagamentoIdformaPagamento);
        }

    }
}
