using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FortalezaDesktop
{
    public class UserPreferencesFile
    {
        public bool GrupoTodos { get; set; }
        public bool ModoCaixa { get; set; }
        public bool ConcluirVendaPagamentoCompleto { get; set; }
        public string NomeCaixa { get; set; }
    }

    public class UserPreferences
    {
        public static bool GrupoTodos { get; set; }
        public static bool ModoCaixa { get; set; }
        public static bool ConcluirVendaPagamentoCompleto { get; set; }
        public static string NomeCaixa { get; set; }

        public static void Load()
        {
            string configurationFilePath = Directory.GetCurrentDirectory() + "\\UserPreferences.Config.json";
            if (File.Exists(configurationFilePath))
            {
                string jsonString = File.ReadAllText(configurationFilePath);
                UserPreferencesFile configuration = JsonConvert.DeserializeObject<UserPreferencesFile>(jsonString);
                GrupoTodos = configuration.GrupoTodos;
                ModoCaixa = configuration.ModoCaixa;
                ConcluirVendaPagamentoCompleto = configuration.ConcluirVendaPagamentoCompleto;
                NomeCaixa = configuration.NomeCaixa;
            }
            else
            {
                GrupoTodos = true;
                ModoCaixa = false;
                ConcluirVendaPagamentoCompleto = false;
                NomeCaixa = "Caixa 1";
                Save();
            }
        }

        public static void Save()
        {
            string configurationFilePath = Directory.GetCurrentDirectory() + "\\UserPreferences.Config.json";
            UserPreferencesFile configuration = new UserPreferencesFile {
                GrupoTodos = GrupoTodos,
                ModoCaixa = ModoCaixa,
                ConcluirVendaPagamentoCompleto = ConcluirVendaPagamentoCompleto,
                NomeCaixa = NomeCaixa
            };
            File.WriteAllText(configurationFilePath, JsonConvert.SerializeObject(configuration));
        }
    }
}
