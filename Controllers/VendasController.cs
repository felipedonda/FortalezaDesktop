using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FortalezaDesktop.Models;
using System.Net.Http;
using System.Linq;

namespace FortalezaDesktop.Controllers
{
    class VendasController
    {
        public static async Task<List<Venda>> FindAllAsync()
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var vendas = await apiClient.VendasAllAsync(false, null, null);
            return vendas.ToList();
        }

        public static async Task<List<Venda>> FindAllAsync(DateTime dataInicial, DateTime dataFinal)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var vendas = await apiClient.VendasAllAsync(true, dataInicial.ToString("yyyy-MM-dd"), dataFinal.ToString("yyyy-MM-dd"));
            return vendas.ToList();
        }

        public static async Task<Venda> FindByIdAsync(int id, bool includeMovimentos = false)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.Vendas2Async(id, includeMovimentos);
        }

        public static async Task<Venda> CreateAsync(Venda venda)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.VendasAsync(venda);
        }

        public static async Task UpdateAsync(Venda venda)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            await apiClient.Vendas3Async(venda.Idvenda, venda);
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return (await apiClient.Vendas4Async(id) != null);
        }
    }
}
