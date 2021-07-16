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
    public class InformacoesEmpresaController
    {
        public static async Task<List<InformacoesEmpresa>> GetAllAsync()
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var informacoesEmpresa = await apiClient.InformacoesEmpresaAllAsync();
            return informacoesEmpresa.ToList();
        }

        public static async Task<InformacoesEmpresa> FindByIdAsync(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.InformacoesEmpresa2Async(id);
        }

        public static async Task<InformacoesEmpresa> CreateAsync(InformacoesEmpresa informacoesEmpresa)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.InformacoesEmpresaAsync(informacoesEmpresa);
        }

        public static async Task UpdateAsync(InformacoesEmpresa informacoesEmpresa)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            await apiClient.InformacoesEmpresa3Async(informacoesEmpresa.IdinformacoesEmpresa, informacoesEmpresa);
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return (await apiClient.InformacoesEmpresa4Async(id) != null);
        }

        public static async Task<bool> UploadLogoAsync(MemoryStream logo, string logoFileName)
        {
            throw new NotImplementedException();
        }

        public static Task<Stream> GetImage()
        {
            throw new NotImplementedException();
        }
    }
}
