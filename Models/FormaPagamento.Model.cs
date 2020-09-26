using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public partial class FormaPagamento : Model<FormaPagamento>
    {
        public override string Path { get { return "/formapagamentos"; } }

        public override int? Id
        {
            get { return IdformaPagamento; }
            set { IdformaPagamento = value ?? default; }
        }
    }
}
