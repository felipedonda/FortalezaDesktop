using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FortalezaDesktop.OldModels
{
    public partial class FormaPagamento : OldModel<FormaPagamento>
    {
        public override string Path { get { return "/formapagamentos"; } }

        public override int? Id
        {
            get { return IdformaPagamento; }
            set { IdformaPagamento = value ?? default; }
        }

        public string DebitarClienteString
        {
            get
            {
                if(DebitarCliente == 1)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        public string GerarContasReceberString
        {
            get
            {
                if (GerarContasReceber == 1)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        public string BandeiraString
        {
            get
            {
                if (Bandeira == 1)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        public string DebitoString
        {
            get
            {
                if (Debito == 1)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        public async Task<int> GetOrdem()
        {
            try
            {
                return await ServerEntry<int>.Get(Path + "/actions/ordem");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }

    }
}
