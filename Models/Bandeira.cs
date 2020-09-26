﻿using System;
using System.Collections.Generic;

namespace FortalezaDesktop.Models
{
    public partial class Bandeira
    {
        public Bandeira()
        {
            Movimento = new HashSet<Movimento>();
        }

        public int Idbandeira { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public decimal? TaxaCredito { get; set; }
        public int? PrazoCredito { get; set; }
        public decimal? TaxaDebito { get; set; }
        public int? PrazoDebito { get; set; }

        public virtual ICollection<Movimento> Movimento { get; set; }
    }
}
