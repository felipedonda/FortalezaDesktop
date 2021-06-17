using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FortalezaDesktop.Models;
using System.Net.Http;
using System.Linq;


namespace FortalezaDesktop.Controllers
{
    class ClientesController
    {
        public static async Task<List<Cliente>> FindAllAsync()
        {
            return await FindAllAsync("");
        }

        public static async Task<List<Cliente>> FindAllAsync(string query)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var clientes = await apiClient.ClientesAllAsync(query);
            return clientes.ToList();
        }

        public static async Task<Cliente> FindByIdAsync(int id, bool includeMovimentos = false)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.Clientes2Async(id, includeMovimentos);
        }

        public static async Task<Cliente> FindByCpfAsync(string cpf, bool includeMovimentos = false)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            throw new NotImplementedException();
            //return await apiClient.clie (cpf, includeMovimentos);
        }

        public static async Task<Cliente> Create(Cliente cliente)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.ClientesAsync(cliente);
        }

        public static async Task Update(Cliente cliente)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            await apiClient.Clientes3Async(cliente.Idcliente, cliente);
        }

        public static async Task<bool> Delete(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return (await apiClient.Clientes4Async(id) != null);
        }
    }
}
