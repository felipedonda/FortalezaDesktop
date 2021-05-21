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

        public string TipoString
        {
            get
            {
                return Tipo switch
                {
                    1 => "Venda",
                    2 => "Suprimento",
                    3 => "Sangria",
                    4 => "Abertura",
                    5 => "Fechamento",
                    _ => "Indefinido"
                };
            }

            set
            {
                Tipo = value switch
                {
                    "Venda" => 1,
                    "Suprimento" => 2,
                    "Sangria" => 3,
                    "Abertura" => 4,
                    "Fechamento" => 5,
                    _ => 0
                };
            }
        }
    }
}
