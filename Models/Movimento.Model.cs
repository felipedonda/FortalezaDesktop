using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public partial class Movimento : Model<Movimento>
    {
        public override string Path { get { return "/movimentos"; } }

        public override int? Id
        {
            get { return Idmovimento; }
            set { Idmovimento = value ?? default; }
        }
    }
}
