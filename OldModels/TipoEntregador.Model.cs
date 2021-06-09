using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.OldModels
{
    partial class TipoEntregador : OldModel<TipoEntregador>
    {
        public override string Path { get { return "/tipoentregadores"; } }

        public override int? Id
        {
            get { return IdtipoEntregador; }
            set { IdtipoEntregador = value ?? default; }
        }
    }
}
