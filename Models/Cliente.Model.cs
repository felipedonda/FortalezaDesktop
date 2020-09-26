using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public partial class Cliente : Model<Cliente>
    {
        public override string Path { get { return "/clientes"; } }

        public override int? Id
        {
            get { return Idcliente; }
            set { Idcliente = value ?? default; }
        }

        public async Task<Cliente> FindByCPF(string CPF)
        {
            return await ServerEntry<Cliente>.Get(Path + "/" + CPF, new Dictionary<string, string> { {"cpf","true"} });
        }
    }
}
