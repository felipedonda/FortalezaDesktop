using FortalezaDesktop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

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
        
        public async static Task<Endereco> ConsultarCEP(string CEP)
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
                return new Endereco
                {
                    Cep = CEP,
                    Bairro = result.bairro,
                    Municipio = result.localidade,
                    Logradouro = result.logradouro,
                    Uf = result.uf

                };
            }
            else
            {
                throw new BadResponseStatusCodeException("Bad status code: " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
            }

        }

    }

}
