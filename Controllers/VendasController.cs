using FortalezaDesktop.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace FortalezaDesktop.Controllers
{
    class VendasController
    {
        public static async Task<List<Venda>> FindAllAsync(
            DateTime dataInicial,
            DateTime dataFinal,
            int tipo = -1,
            string query = "")
        {
            string _dataInicial = dataInicial.ToString("yyyy-MM-dd");
            string _dataFinal = dataFinal.ToString("yyyy-MM-dd");

            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var vendas = await apiClient.VendasAllAsync(tipo,false,true,_dataInicial,_dataFinal,query);
            return vendas.ToList();
        }

        public static async Task<List<Venda>> FindAllAsync(
            string query,
            int tipo = -1)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var vendas = await apiClient.VendasAllAsync(tipo, false, false, "", "", query);
            return vendas.ToList();
        }

        public static async Task<List<Venda>> FindAllAbertasAsync()
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var vendas = await apiClient.VendasAllAsync(-1, true, false, "", "", "");
            return vendas.ToList();
        }
    }
}
