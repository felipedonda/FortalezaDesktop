using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.OldModels
{
    public partial class Entregador : OldModel<Entregador>
    {
        public override string Path { get { return "/entregadores"; } }

        public override int? Id
        {
            get { return Identregador; }
            set { Identregador = value ?? default; }
        }
    }
}
