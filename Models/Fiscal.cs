﻿using System;
using System.Collections.Generic;

namespace FortalezaDesktop.Models
{
    public partial class Fiscal
    {
        public int Iditem { get; set; }
        public string Ncm { get; set; }
        public string Cest { get; set; }
        public string ImpostoFederal { get; set; }
        public string ImpostoEstadual { get; set; }
        public string ImpostoMunicipal { get; set; }
        public string Cfop { get; set; }
        public string Origem { get; set; }
        public string Csosn { get; set; }

        public virtual Item IditemNavigation { get; set; }
    }
}
