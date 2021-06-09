using System;
using System.Collections.Generic;

namespace FortalezaDesktop.OldModels
{
    public partial class VendaHasComanda
    {
        public int Idvenda { get; set; }
        public int Idcomanda { get; set; }

        public virtual Comanda IdcomandaNavigation { get; set; }
        public virtual Venda IdvendaNavigation { get; set; }
    }
}
