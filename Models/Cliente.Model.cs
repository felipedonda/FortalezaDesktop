using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FortalezaDesktop.Models
{
    public class Cliente
    {
        public int? Idcliente { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public Endereco Endereco { get; set; }
        
        public async Task SaveInstance()
        {
            Idcliente = (await CreateCliente(this)).Idcliente;
            if(Endereco != null)
            {
                Endereco.Idendereco = (await CreateEndereco(Idcliente, Endereco)).Idendereco;
            }
        }

        public async Task LoadEndereco()
        {
            Endereco = await GetEndereco(Idcliente);
        }

        public async Task UpdateInstance()
        {
            await UpdateCliente(Idcliente, this);
            if(Endereco != null)
            {
                await UpdateEndereco(Idcliente, Endereco);
            }
        }

        public async static Task<List<Cliente>> GetClientes()
        {
            return await Model<List<Cliente>>.Get("/cliente");
        }

        public async static Task<Cliente> GetCliente(int Idcliente)
        {
            return await Model<Cliente>.Get("/cliente", Idcliente);
        }

        private async static Task<Cliente> CreateCliente(Cliente cliente)
        {
            return await Model<Cliente>.Post("/cliente", cliente);
        }

        private async static Task<bool> UpdateCliente(int? Idcliente, Cliente cliente)
        {
            return await Model<Cliente>.Put("/cliente", Idcliente ?? default, cliente);
        }

        private async static Task<Endereco> CreateEndereco(int? Idcliente, Endereco endereco)
        {
            return await Model<Endereco>.Post("/cliente/" + Idcliente + "/endereco", endereco);
        }

        private async static Task<Endereco> GetEndereco(int? Idcliente)
        {
            return await Model<Endereco>.Get("/cliente/" + Idcliente + "/endereco");
        }

        private async static Task<bool> UpdateEndereco(int? Idcliente, Endereco endereco)
        {
            return await Model<Endereco>.Put("/cliente/" + (Idcliente ?? default) + "/endereco", endereco);
        }
    }
}
