using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.OldModels
{
    public partial class Grupo : OldModel<Grupo>
    {
        public override string Path { get { return "/grupos"; } }

        public override int? Id
        {
            get { return Idgrupo; }
            set { Idgrupo = value ?? default; }
        }
    }
}
