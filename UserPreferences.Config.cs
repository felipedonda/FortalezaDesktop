using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FortalezaDesktop
{
    public class ConfigImpressora
    {
        public string ImpressoraPadrao { get; set; }
        public bool Habilitada { get; set; }
        public bool Visualizar { get; set; }
        public bool SempreImprimir { get; set; }
        public double Tamanho { get; set; }
        public struct TamanhoImpressao
        {
            public const double Tamanho80mm = 302.36220472;
            public const double Tamanho50mm = 188.97637795;
        }
    }

    public class UserPreferencesFile
    {
        public bool GrupoTodos { get; set; }
        public bool ModoCaixa { get; set; }
        public bool ConcluirVendaPagamentoCompleto { get; set; }
        public int IdnomeCaixa { get; set; }
        public int Idpdv { get; set; }
        public ConfigImpressora ImpressoraCupom { get; set; }
        public ConfigImpressora ImpressoraRelatorio { get; set; }
        public ConfigImpressora ImpressoraCozinha { get; set; }
        public bool ModuloVenda { get; set; }
        public bool ModuloPedido { get; set; }
        public bool ModuloDelivery { get; set; }
        public bool ModuloTroca { get; set; }
        public string SenhaSat { get; set; }
        public bool SatHomologacao { get; set; }
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

                if(Preferences.Idpdv == default)
                {
                    Preferences.Idpdv = 1;
                    Save();
                }

                if(Preferences.IdnomeCaixa == default)
                {
                    Preferences.IdnomeCaixa = 1;
                    Save();
                }

                if(Preferences.ImpressoraCozinha == null)
                {
                    Preferences.ImpressoraCozinha = new ConfigImpressora
                    {
                        Habilitada = true,
                        ImpressoraPadrao = "",
                        SempreImprimir = true,
                        Visualizar = true
                    };
                    Save();
                }

                if (Preferences.ImpressoraCupom == null)
                {
                    Preferences.ImpressoraCupom = new ConfigImpressora
                    {
                        Habilitada = true,
                        ImpressoraPadrao = "",
                        SempreImprimir = true,
                        Visualizar = true,
                        Tamanho = ConfigImpressora.TamanhoImpressao.Tamanho80mm
                    };
                    Save();
                }

                if (Preferences.ImpressoraRelatorio == null)
                {
                    Preferences.ImpressoraRelatorio = new ConfigImpressora
                    {
                        Habilitada = true,
                        ImpressoraPadrao = "",
                        SempreImprimir = true,
                        Visualizar = true
                    };
                    Save();
                }
            }
            else
            {
                Preferences = new UserPreferencesFile
                {
                    SenhaSat = "",
                    SatHomologacao = false,
                    GrupoTodos = true,
                    ModoCaixa = false,
                    ConcluirVendaPagamentoCompleto = true,
                    IdnomeCaixa = 1,
                    Idpdv = 1,
                    ModuloDelivery = true,
                    ModuloPedido = true,
                    ModuloVenda = true,
                    ModuloTroca = true,
                    ImpressoraCupom = new ConfigImpressora
                    {
                        Habilitada = true,
                        ImpressoraPadrao = "",
                        SempreImprimir = true,
                        Visualizar = true,
                        Tamanho = ConfigImpressora.TamanhoImpressao.Tamanho80mm
                    },
                    ImpressoraCozinha = new ConfigImpressora
                    {
                        Habilitada = true,
                        ImpressoraPadrao = "",
                        SempreImprimir = true,
                        Visualizar = true
                    },
                    ImpressoraRelatorio = new ConfigImpressora
                    {
                        Habilitada = true,
                        ImpressoraPadrao = "",
                        SempreImprimir = true,
                        Visualizar = true
                    }

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
