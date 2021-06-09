using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.OldModels
{
    public partial class Endereco
    {
        public string StringExtenso
        {
            get
            {
                string extenso = Logradouro + ", " + Numero;
                if (Complemento != null)
                {
                    extenso += " - " + Complemento;
                }
                return extenso;
            }
        }
    }
}