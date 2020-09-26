using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public partial class Bandeira : Model<Bandeira>
    {

        public override string Path { get { return "/bandeiras"; } }

        public override int? Id
        {
            get { return Idbandeira; }
            set { Idbandeira = value ?? default; }
        }
    }
}
