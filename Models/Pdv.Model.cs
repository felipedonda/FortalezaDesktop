using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.Models
{
    public partial class Pdv : Model<Pdv>
    {
        public override string Path { get { return "/pdvs"; } }

        public override int? Id
        {
            get { return Idpdv; }
            set { Idpdv = value ?? default; }
        }
    }
}
