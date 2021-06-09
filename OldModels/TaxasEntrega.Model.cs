using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.OldModels
{
    partial class TaxasEntrega : OldModel<TaxasEntrega>
    {
        public override string Path { get { return "/taxasentregas"; } }

        public override int? Id
        {
            get { return IdtaxasEntrega; }
            set { IdtaxasEntrega = value ?? default; }
        }


    }
}
