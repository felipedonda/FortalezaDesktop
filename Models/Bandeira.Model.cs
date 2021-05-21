using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FortalezaDesktop.Models
{
    public partial class Bandeira : Model<Bandeira>
    {

        public override string Path { get { return "/bandeiras"; } }

        public override int? Id
        {
            get { return Idbandeira; }
            set { Idbandeira = value ?? default; }
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
