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
    public class Pagamento
    {
        public int IdPagamento { get; set; }
        public int? MovimentoIdmovimento { get; set; }
        public int VendaIdvenda { get; set; }
        
        [JsonIgnore]
        public Movimento Movimento { get; set; }
    }
}
