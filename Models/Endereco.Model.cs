using System;
using System.Collections.Generic;
using System.Text;

namespace FortalezaDesktop.Models
{
    public class Endereco
    {
        public int Idendereco { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Completemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
    }
}
