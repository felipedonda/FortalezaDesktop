using System;
using System.Collections.Generic;

namespace FortalezaDesktop.Models
{
    public partial class ItemHasAdicional
    {
        public int Iditem { get; set; }
        public int Idadicional { get; set; }

        public virtual Adicional IdadicionalNavigation { get; set; }
        public virtual Item IditemNavigation { get; set; }
    }
}
