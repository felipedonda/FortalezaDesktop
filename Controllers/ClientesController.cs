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
        public async Task<List<Cliente>> GetAllClientesAsync()
        {
            return await GetAllClientesAsync("");
        }

        public async Task<List<Cliente>> GetAllClientesAsync(string query)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            var clientes = await apiClient.ClientesAllAsync(query);
            return clientes.ToList();
        }

        public async Task<Cliente> GetClienteByIdAsync(int id, bool includeMovimentos = false)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.Clientes2Async(id, includeMovimentos);
        }

        public async Task<Cliente> GetClienteByCpfAsync(string cpf, bool includeMovimentos = false)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            throw new NotImplementedException();
            //return await apiClient.clie (cpf, includeMovimentos);
        }

        public async Task<Cliente> CreateClient(Cliente cliente)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return await apiClient.ClientesAsync(cliente);
        }

        public async Task UpdateClient(Cliente cliente)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            await apiClient.Clientes3Async(cliente.Idcliente, cliente);
        }

        public async Task<bool> DeleteClient(int id)
        {
            using var httpClient = new HttpClient();
            var apiClient = new FortalezaApiClient(Server.ApiUri, httpClient);
            return (await apiClient.Clientes4Async(id) != null);
        }
    }
}
