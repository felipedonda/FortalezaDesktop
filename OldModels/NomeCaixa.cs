using System;
using System.Collections.Generic;

namespace FortalezaDesktop.OldModels
{
    public partial class NomeCaixa
    {
        public NomeCaixa()
        {
            Caixa = new HashSet<Caixa>();
        }

        public int IdnomeCaixa { get; set; }
        public int Nome { get; set; }

        public virtual ICollection<Caixa> Caixa { get; set; }
    }
}
