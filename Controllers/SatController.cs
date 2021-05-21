using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using FortalezaDesktop.Utils;

using System.Windows;

namespace FortalezaDesktop.Controllers
{
    class SatController
    {
        public static async Task<bool> RunTest()
        {
            ControliD.CIDSAT cIDSAT;
            cIDSAT = new ControliD.CIDSAT();

            XmlDocument TestCfe;

            try
            {
                TestCfe = await ServerEntry<XmlDocument>.Get("/fiscal/cfe-test");
            }
            catch (BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
                return false;
            }

            MessageBox.Show(cIDSAT.ConsultarSAT(1));

            MessageBox.Show(cIDSAT.ConsultarStatusOperacional(1, UserPreferences.Preferences.SenhaSat));


            return true;
        }
    }
}
