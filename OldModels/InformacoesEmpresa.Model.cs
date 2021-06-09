using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace FortalezaDesktop.OldModels
{
    public partial class InformacoesEmpresa : OldModel<InformacoesEmpresa>
    {
        public override string Path { get { return "/informacoesempresa"; } }

        public override int? Id
        {
            get { return IdinformacoesEmpresa; }
            set { IdinformacoesEmpresa = value ?? default; }
        }

        public async Task<bool> UploadLogo(Stream image, string fileName)
        {
            try
            {
                string logo = await ServerEntry.PostFile(Path + "/" + IdinformacoesEmpresa + "/imagem", image, fileName);
                if(logo != null)
                {
                    Logo = logo;
                    return true;
                }
                else
                {
                    MessageBox.Show("Falha na hora de subir o logo. Nome do arquivo nulo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<Stream> GetImage()
        {
            try
            {
                return await ServerEntry.GetFile("/images/" + Logo);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            
        }

        [JsonIgnore]
        public string LinkLogo
        {
            get
            {
                if(Logo != null)
                {
                    return Server.Uri + "/images/" + Logo;
                }
                return null;
            }
        }

        [JsonIgnore]
        public bool IsCpf
        {
            get
            {
                return Cpf.Length == 11;
            }
        }

        [JsonIgnore]
        public string CpfFormatted
        {
            get
            {
                if (Cpf != null)
                {
                    string formatted = "";
                    if (IsCpf)
                    {
                        formatted += Cpf.Substring(0, 3) + ".";
                        formatted += Cpf.Substring(3, 3) + ".";
                        formatted += Cpf.Substring(6, 3) + "-";
                        formatted += Cpf.Substring(9, 2);
                    }
                    else
                    {
                        formatted += Cpf.Substring(0, 2) + ".";
                        formatted += Cpf.Substring(2, 3) + ".";
                        formatted += Cpf.Substring(5, 3) + "/";
                        formatted += Cpf.Substring(8, 4) + "-";
                        formatted += Cpf.Substring(12, 2);
                    }
                    return formatted;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore]
        public string RgFormatted
        {
            get
            {
                if (Rg != null)
                {
                    if(Rg.Length >= 9)
                    {
                        string formatted = "";
                        if (IsCpf)
                        {
                            formatted += Cpf.Substring(0, 2) + ".";
                            formatted += Cpf.Substring(2, 3) + ".";
                            formatted += Cpf.Substring(5, 3) + "-";
                            formatted += Cpf.Substring(8, 1);
                        }
                        else
                        {
                            //todo
                            formatted += Cpf.Substring(0, 2) + ".";
                            formatted += Cpf.Substring(2, 3) + ".";
                            formatted += Cpf.Substring(5, 3) + "/";
                            formatted += Cpf.Substring(8, 4) + "-";
                            formatted += Cpf.Substring(12, 2);
                        }
                        return formatted;
                    }
                }
                return null;
            }
        }

        [JsonIgnore]
        public string CnaeFormatted
        {
            get
            {
                if (Cnae != null)
                {
                    if(Cnae.Length == 7)
                    {
                        string formatted = Cpf.Substring(0, 2) + ".";
                        formatted += Cpf.Substring(2, 2) + "-";
                        formatted += Cpf.Substring(4, 1) + "-";
                        formatted += Cpf.Substring(5, 2);
                        return formatted;
                    }
                }
                return null;
            }
        }
    }
}
