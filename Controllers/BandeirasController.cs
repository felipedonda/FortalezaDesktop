using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FortalezaDesktop.Models;
using System.Net.Http;
using System.Linq;


namespace FortalezaDesktop.Controllers
{
    class BandeirasController
    {
        public static async Task<List<Bandeira>> GetAllBandeirasAsync()
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var bandeiras = await apiClient.BandeirasAllAsync();
            return bandeiras.ToList();
        }

        public static async Task<Bandeira> GetBandeiraByIdAsync(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.Bandeiras2Async(id);
        }

        public static async Task<Bandeira> CreateBandeiraAsync(Bandeira bandeira)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.BandeirasAsync(bandeira);
        }

        public static async Task UpdateBandeiraAsync(Bandeira bandeira)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            await apiClient.Bandeiras3Async(bandeira.Idbandeira, bandeira);
        }

        public static async Task<bool> DeleteBandeiraAsync(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return (await apiClient.Bandeiras4Async(id) != null);
        }
    }
}
