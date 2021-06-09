using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FortalezaDesktop.OldModels
{
    public partial class Caixa : OldModel<Caixa>
    {
        public override string Path { get { return "/caixas"; } }

        public override int? Id
        {
            get { return Idcaixa; }
            set { Idcaixa = value ?? default; }
        }

        public async Task<Caixa> GetCaixaAberto(int IdnomeCaixa, Dictionary<string, string> options = null)
        {
            try
            {
                if(options == null)
                {
                    options = new Dictionary<string, string>();
                }
                options.Add("idnomeCaixa", IdnomeCaixa.ToString());
                return await ServerEntry<Caixa>.Get(Path + "/actions/aberto", options);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }

        public async Task<bool> AbrirCaixa(int idusuario, int idpdv)
        {
            try
            {
                Dictionary<string, string> options = new Dictionary<string, string> {
                    {"idusuario", idusuario.ToString()},
                    {"idpdv", idpdv.ToString()}
                };
                return await ServerEntry.ActionPost(Path + "/" + Idcaixa.ToString() + "/abrir", null , options);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> FecharCaixa(int idusuario, int idpdv)
        {
            try
            {
                Dictionary<string, string> options = new Dictionary<string, string> {
                    {"idusuario", idusuario.ToString()},
                    {"idpdv", idpdv.ToString()}
                };
                return await ServerEntry.ActionPost(Path + "/" + Idcaixa.ToString() + "/fechar", null, options);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
