using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FortalezaDesktop.Models;
using System.Net.Http;
using System.Linq;
using System.IO;


namespace FortalezaDesktop.Controllers
{
    class InformacoesEmpresaController
    {
        public static async Task<InformacoesEmpresa> GetInformacoesEmpresa()
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.InformacoesEmpresa2Async(1);
        }

        public static async Task UpdateInformacoesEmpresa(InformacoesEmpresa informacoesEmpresa)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            await apiClient.InformacoesEmpresa3Async(1, informacoesEmpresa);
        }
    }
}
