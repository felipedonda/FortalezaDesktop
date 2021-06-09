using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.OldModels
{
    public partial class Pdv : OldModel<Pdv>
    {
        public override string Path { get { return "/pdvs"; } }

        public override int? Id
        {
            get { return Idpdv; }
            set { Idpdv = value ?? default; }
        }
    }
}
