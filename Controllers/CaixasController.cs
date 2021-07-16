using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FortalezaDesktop.Models;
using System.Net.Http;
using System.Linq;

namespace FortalezaDesktop.Controllers
{
    class CaixasController
    {
        public static async Task<List<Caixa>> FindAllAsync()
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var caixas = await apiClient.CaixasAllAsync(false,null,null);
            return caixas.ToList();
        }

        public static async Task<List<Caixa>> FindAllAsync(DateTime dataInicial, DateTime dataFinal)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var caixas = await apiClient.CaixasAllAsync(true, dataInicial.ToString("yyyy-MM-dd"), dataFinal.ToString("yyyy-MM-dd"));
            return caixas.ToList();
        }

        public static async Task<Caixa> FindByIdAsync(int id, bool includeMovimentos = false)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.Caixas2Async(id,includeMovimentos);
        }

        public static async Task<Caixa> CreateAsync(Caixa caixa)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.CaixasAsync(caixa);
        }

        public static async Task UpdateAsync(Caixa caixa)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            await apiClient.Caixas3Async(caixa.Idcaixa, caixa);
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return (await apiClient.Caixas4Async(id) != null);
        }
    }
}
