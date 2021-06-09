using System;
using System.Collections.Generic;

namespace FortalezaDesktop.OldModels
{
    public partial class EstoqueHasVenda
    {
        public int Idestoque { get; set; }
        public int Idvenda { get; set; }

        public virtual Estoque IdestoqueNavigation { get; set; }
        public virtual Venda IdvendaNavigation { get; set; }
    }
}
