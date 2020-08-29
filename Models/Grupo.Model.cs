using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using FortalezaDesktop.Models;
using FortalezaDesktop.Utils;
using FortalezaDesktop;

namespace FortalezaDesktop.Models
{
    public class Grupo
    {
        public int Idgrupo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        
        [JsonConverter(typeof(BoolConverter))]
        public bool Visivel { get; set; }

        public static async Task<List<Grupo>> GetGrupos()
        {
            return await Model<List<Grupo>>.Get("/grupo");
        }

        public static async Task<Grupo> GetGrupo(int id)
        {
            return await Model<Grupo>.Get("/grupo", id);
        }

        public static async Task<Grupo> CreateGrupo(Grupo grupo)
        {
            return await Model<Grupo>.Post("/grupo", grupo);
        }

        public static async Task<bool> UpdateGrupo(int id, Grupo grupo)
        {
            return await Model<Grupo>.Put("/grupo", id, grupo);
        }

        public static async Task<bool> DeleteGrupo(int id)
        {
            return await Model<Grupo>.Delete("/grupo", id);
        }

        public static async Task<List<Item>> GetItems(int idGrupo)
        {
            return await Model<List<Item>>.Get("/grupo/" + idGrupo + "/item") ;
        }
    }
}
