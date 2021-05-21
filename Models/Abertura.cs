﻿using System;
using System.Collections.Generic;

namespace FortalezaDesktop.Models
{
    public partial class Abertura
    {
        public int Idcaixa { get; set; }
        public int Idpdv { get; set; }
        public int Idusuario { get; set; }
        public DateTime Hora { get; set; }

        public virtual Caixa IdcaixaNavigation { get; set; }
        public virtual Pdv IdpdvNavigation { get; set; }
        public virtual Usuario IdusuarioNavigation { get; set; }
    }
}
