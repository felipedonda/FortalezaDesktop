using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public partial class Usuario : Model<Usuario>
    {
        public override string Path { get { return "/usuarios"; } }

        public override int? Id
        {
            get { return Idusuario; }
            set { Idusuario = value ?? default; }
        }
    }
}
