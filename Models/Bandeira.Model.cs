using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.Models
{
    public class Bandeira
    {
        public int Idbandeira { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public decimal TaxaCredito { get; set; }
        public decimal TaxaDebito { get; set; }
        public int PrazoCredito { get; set; }
        public int PrazoDebito { get; set; }
    }
}
