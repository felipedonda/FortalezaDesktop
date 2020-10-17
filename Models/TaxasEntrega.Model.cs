using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.Models
{
    partial class TaxasEntrega : Model<TaxasEntrega>
    {
        public override string Path { get { return "/taxasentregas"; } }

        public override int? Id
        {
            get { return IdtaxasEntrega; }
            set { IdtaxasEntrega = value ?? default; }
        }


    }
}
