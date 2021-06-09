using System;
using System.Collections.Generic;

namespace FortalezaDesktop.OldModels
{
    public partial class InformacoesEmpresaHasEndereco
    {
        public int InformacoesEmpresaIdinformacoesEmpresa { get; set; }
        public int EnderecoIdendereco { get; set; }

        public virtual Endereco EnderecoIdenderecoNavigation { get; set; }
        public virtual InformacoesEmpresa InformacoesEmpresaIdinformacoesEmpresaNavigation { get; set; }
    }
}
