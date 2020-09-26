using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FortalezaDesktop.Models
{
    public partial class Caixa : Model<Caixa>
    {
        public override string Path { get { return "/caixas"; } }

        public override int? Id
        {
            get { return Idcaixa; }
            set { Idcaixa = value ?? default; }
        }

        public async Task<Caixa> GetCaixaAberto(Dictionary<string, string> options = null)
        {
            try
            {
                return await ServerEntry<Caixa>.Get(Path + "/actions/aberto", options);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }
    }
}
