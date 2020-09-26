using System;
using System.Collections.Generic;

namespace FortalezaDesktop.Models
{
    public partial class UsuarioHasEndereco
    {
        public int Idusuario { get; set; }
        public int Idendereco { get; set; }

        public virtual Endereco IdenderecoNavigation { get; set; }
        public virtual Usuario IdusuarioNavigation { get; set; }
    }
}
