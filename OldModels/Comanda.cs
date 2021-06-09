using System;
using System.Collections.Generic;

namespace FortalezaDesktop.OldModels
{
    public partial class Comanda
    {
        public Comanda()
        {
            VendaHasComanda = new HashSet<VendaHasComanda>();
        }

        public int Idcomanda { get; set; }
        public int Codigo { get; set; }

        public virtual ICollection<VendaHasComanda> VendaHasComanda { get; set; }
    }
}
