using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.Models
{
    public partial class Pedido : Model<Pedido>
    {
        public override string Path { get { return "/pedidos"; } }

        public override int? Id
        {
            get { return Idvenda; }
            set { Idvenda = value ?? default; }
        }
    }
}
