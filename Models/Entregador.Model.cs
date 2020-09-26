using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.Models
{
    public partial class Entregador : Model<Entregador>
    {
        public override string Path { get { return "/entregadores"; } }

        public override int? Id
        {
            get { return Identregador; }
            set { Identregador = value ?? default; }
        }
    }
}
