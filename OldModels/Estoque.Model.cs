using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.OldModels
{
    public partial class Estoque : OldModel<Estoque>
    {
        public override string Path { get { return "/estoques"; } }

        public override int? Id
        {
            get { return Idestoque; }
            set { Idestoque = value ?? default; }
        }
    }
}
