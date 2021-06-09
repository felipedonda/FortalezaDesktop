using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.OldModels
{
    partial class Metodo : OldModel<Metodo>
    {
        public override string Path { get { return "/metodos"; } }

        public override int? Id
        {
            get { return Idmetodo; }
            set { Idmetodo = value ?? default; }
        }
    }
}
