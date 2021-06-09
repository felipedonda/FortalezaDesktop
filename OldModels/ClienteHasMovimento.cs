using System;
using System.Collections.Generic;

namespace FortalezaDesktop.OldModels
{
    public partial class ClienteHasMovimento
    {
        public int Idcliente { get; set; }
        public int Idmovimento { get; set; }

        public virtual Cliente IdclienteNavigation { get; set; }
        public virtual Movimento IdmovimentoNavigation { get; set; }
    }
}
