using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public class Pacote
    {
        public int ItemIditem { get; set; }
        public int? IditemPacote { get; set; }
        public decimal Quantidade { get; set; }
        public bool Padrao { get; set; }
        [JsonIgnore]
        public Item ItemPacote { get; set; }
    }
}
