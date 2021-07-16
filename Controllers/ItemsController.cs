using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FortalezaDesktop.Models;
using System.Net.Http;
using System.Linq;

namespace FortalezaDesktop.Controllers
{
    class ItemsController
    {
        public static async Task<List<Item>> FindAllAsync(bool estoqueAtual = false, bool apenasVisiveis = true, bool apenasComEstoque = false)
        {
            return await FindAllAsync(null, estoqueAtual, apenasVisiveis, apenasComEstoque);
        }

        private static async Task<List<Item>> FindAllAsync(string query, bool estoqueAtual = false, bool apenasVisiveis = true, bool apenasComEstoque = false)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var items = await apiClient.ItemsAllAsync(estoqueAtual, apenasComEstoque, apenasVisiveis, query);
            return items.ToList();
        }

        public static async Task<Item> FindByIdAsync(int id, bool includeMovimentos = false)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.Items2Async(id, includeMovimentos);
        }

        public static async Task<Item> CreateAsync(Item item)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.ItemsAsync(item);
        }

        public static async Task UpdateAsync(Item item)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            await apiClient.Items3Async(item.Iditem, item);
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return (await apiClient.Items4Async(id) != null);
        }
    }
}
