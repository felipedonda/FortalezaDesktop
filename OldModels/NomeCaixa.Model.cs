using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.OldModels
{
    public partial class NomeCaixa : OldModel<NomeCaixa>
    {
        public override string Path { get { return "/nomecaixas"; } }

        public override int? Id
        {
            get { return IdnomeCaixa; }
            set { IdnomeCaixa = value ?? default; }
        }
    }
}
