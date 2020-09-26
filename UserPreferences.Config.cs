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
        public static UserPreferencesFile Preferences { get; set; }

        public static void Load()
        {
            string configurationFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Fortaleza Desktop");

            string configurationFilePath = Path.Combine(
                configurationFolderPath,
                "UserPreferences.Config.json");

            if (File.Exists(configurationFilePath))
            {
                string jsonString = File.ReadAllText(configurationFilePath);
                Preferences = JsonConvert.DeserializeObject<UserPreferencesFile>(jsonString);
            }
            else
            {
                Preferences = new UserPreferencesFile
                {
                    GrupoTodos = true,
                    ModoCaixa = false,
                    ConcluirVendaPagamentoCompleto = false,
                    NomeCaixa = "Caixa 1"
                };
                Save();
            }
        }

        public static void Save()
        {
            string configurationFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Fortaleza Desktop");

            string configurationFilePath = Path.Combine(
                configurationFolderPath,
                "UserPreferences.Config.json");

            if (!Directory.Exists(configurationFolderPath))
            {
                Directory.CreateDirectory(configurationFolderPath);
            }

            File.WriteAllText(configurationFilePath, JsonConvert.SerializeObject(Preferences));
        }
    }
}
