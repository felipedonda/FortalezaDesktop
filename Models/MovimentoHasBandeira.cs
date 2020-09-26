using System;
using System.Collections.Generic;

namespace FortalezaDesktop.Models
{
    public partial class MovimentoHasBandeira
    {
        public int Idmovimento { get; set; }
        public int Idbandeira { get; set; }

        public virtual Bandeira IdbandeiraNavigation { get; set; }
        public virtual Movimento IdmovimentoNavigation { get; set; }
    }
}
