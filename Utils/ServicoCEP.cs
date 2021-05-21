using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Utils
{
    public class ServicoCEP
    {
        private class EnderecoCEP
        {
            public string cep { get; set; }
            public string logradouro { get; set; }
            public string bairro { get; set; }
            public string localidade { get; set; }
            public string uf { get; set; }

        }

        private static string BaseURL = "https://viacep.com.br/ws/";
        
        public async static Task<Endereco> ConsultarCEP(string CEP, Endereco endereco)
        {
            if(CEP.Length != 8)
            {
                throw new Exception("CEP inválido.");
            }

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync(BaseURL + CEP.ToString() + "/json/");
            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonString = await httpResponse.Content.ReadAsStringAsync();
                EnderecoCEP result = JsonConvert.DeserializeObject<EnderecoCEP>(jsonString, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                if(endereco == null)
                {
                    endereco = new Endereco();
                }

                endereco.Cep = CEP;
                endereco.Bairro = result.bairro;
                endereco.Municipio = result.localidade;
                endereco.Logradouro = result.logradouro;
                endereco.Uf = result.uf;
                return endereco;
            }
            else
            {
                throw new BadResponseStatusCodeException(httpResponse.ReasonPhrase, httpResponse.StatusCode.ToString(), (int)httpResponse.StatusCode, httpResponse.RequestMessage.RequestUri.ToString());
            }

        }

    }

}
