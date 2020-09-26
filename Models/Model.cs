using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace FortalezaDesktop.Models
{
    public abstract class Model<T> where T : Model<T>, new()
    {
        [JsonIgnore]
        public abstract string Path { get; }

        [JsonIgnore]
        public abstract int? Id { get; set; }
        
        public async Task<T> FindById(int id, Dictionary<string, string> options = null)
        {
            return await ServerEntry<T>.Get(Path, id, options);
        }
        
        public async Task<List<T>> FindAll(Dictionary<string, string> options = null)
        {
            return await ServerEntry<List<T>>.Get(Path, options);
        }

        public async Task<T> ReloadInstance(Dictionary<string, string> options = null)
        {
            try
            {
                return await ServerEntry<T>.Get(Path, Id ?? default, options);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }

        public async Task<bool> SaveInstance()
        {
            try
            {
                T result = await ServerEntry<T>.Post(Path, this);
                Id = result.Id;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateInstance()
        {
            try
            {
                await ServerEntry<T>.Put(Path + "/" + Id, this);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteInstance()
        {
            try
            {
                await ServerEntry<T>.Delete(Path, Id ?? default);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + "\n\nStack:\n" + e.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }

    public class ServerEntry<T> where T : new()
    {

        public static async Task<T> Get(string path, Dictionary<string, string> options = null)
        {
            HttpClient httpClient = new HttpClient();

            string _path = path;

            if (options != null)
            {
                _path += "?";
                _path += await (new FormUrlEncodedContent(options)).ReadAsStringAsync();
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Server.URI + _path);

            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonString = await httpResponse.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                return result;
            }
            else
            {
                throw new BadResponseStatusCodeException("Bad status code: "
                    + httpResponse.ReasonPhrase
                    + "\nOn: " + httpResponse.RequestMessage.RequestUri);
            }
        }

        public static async Task<T> Get(string path, int id, Dictionary<string, string> options = null)
        {
            return await Get(path + "/" + id, options);
        }

        public static async Task<T> Post(string path, object item)
        {
            string itemJson = JsonConvert.SerializeObject(item, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });

            HttpClient httpClient = new HttpClient();
            StringContent httpContent = new StringContent(itemJson, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await httpClient.PostAsync(Server.URI + path, httpContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonString = await httpResponse.Content.ReadAsStringAsync();
                try
                {
                    T result = JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    return result;
                }
                catch
                {
                    return default;
                }

            }
            else
            {
                throw new BadResponseStatusCodeException("Bad status code: "
                    + httpResponse.ReasonPhrase
                    + "\nOn: " + httpResponse.RequestMessage.RequestUri);
            }
        }

        public static async Task<bool> Put(string path, object item)
        {
            string itemJson = JsonConvert.SerializeObject(item, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });

            HttpClient httpClient = new HttpClient();
            StringContent httpContent = new StringContent(itemJson, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await httpClient.PutAsync(Server.URI + path, httpContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonString = await httpResponse.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                throw new BadResponseStatusCodeException("Bad status code: "
                    + httpResponse.ReasonPhrase
                    + "\nOn: " + httpResponse.RequestMessage.RequestUri);
            }
        }

        public static async Task<bool> Put(string path, int id, T item)
        {
            return await Put(path + "/" + id, item);
        }

        public static async Task<bool> Delete(string path)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.DeleteAsync(Server.URI + path);

            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new BadResponseStatusCodeException("Bad status code: "
                    + httpResponse.ReasonPhrase
                    + "\nOn: " + httpResponse.RequestMessage.RequestUri);
            }
        }

        public static async Task<bool> Delete(string path, int id)
        {
            return await Delete(path + "/" + id);
        }
    }
}
