using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FortalezaDesktop.OldModels
{
    public partial class Item : OldModel<Item>
    {
        public override int? Id
        {
            get { return Iditem; }
            set { Iditem = value ?? default; }
        }

        public override string Path { get { return "/items"; } }

        public Estoque EstoqueAtual { get; set; }

        public async Task<bool> SaveEstoque(Estoque estoque)
        {
            try
            {
                await ServerEntry<Estoque>.Post(Path + "/" + Id + "/estoques", estoque);
            }
            catch (BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
                return false;
            }
            return true;
        }

        public async Task<bool> ItemExists(int id)
        {
            try
            {
                return await ServerEntry<bool>.Get(Path + "/" + id.ToString() + "/exists");
            }
            catch (BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
                return false;
            }
        }

        [JsonIgnore]
        public string TipoString
        {
            get
            {
                return Tipo switch
                {
                    1 => "Produto",
                    2 => "Pacote",
                    _ => "Sistema",
                };
            }

            set
            {
                Tipo = value switch
                {
                    "Produto" => 1,
                    "Pacote" => 2,
                    "Sistema" => 9,
                    _ => -1,
                };
            }
        }
    }
}
