using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.OldModels
{
    public partial class Cliente
    {
        [JsonIgnore]
        public bool IsCpf
        {
            get
            {
                return Cpf.Length == 11;
            }
        }

        [JsonIgnore]
        public string CpfFormatted {
            get
            {
                if(Cpf != null)
                {
                    string formatted = "";
                    if (IsCpf)
                    {
                        formatted += Cpf.Substring(0,3) + ".";
                        formatted += Cpf.Substring(3,3) + ".";
                        formatted += Cpf.Substring(6,3) + "-";
                        formatted += Cpf.Substring(9,2);
                    }
                    else
                    {
                        formatted += Cpf.Substring(0,2) + ".";
                        formatted += Cpf.Substring(2, 3) + ".";
                        formatted += Cpf.Substring(5, 3) + "/";
                        formatted += Cpf.Substring(8, 4) + "-" ;
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
                    string formatted = "";
                    formatted += Cpf.Substring(0, 2) + ".";
                    formatted += Cpf.Substring(2, 3) + ".";
                    formatted += Cpf.Substring(5, 3) + "-";
                    formatted += Cpf.Substring(8, 1);
                    return formatted;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
