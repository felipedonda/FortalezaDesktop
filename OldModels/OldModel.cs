using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;
using FortalezaDesktop.Utils;

namespace FortalezaDesktop.OldModels
{
    public abstract class OldModel<T> where T : OldModel<T>, new()
    {
        [JsonIgnore]
        public abstract string Path { get; }

        [JsonIgnore]
        public abstract int? Id { get; set; }
        
        public async Task<T> FindById(int id, Dictionary<string, string> options = null)
        {
            try
            {
                return await ServerEntry<T>.Get(Path, id, options);
            }
            catch (BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
                return null;
            }
        }


        public async Task<List<T>> FindAll(Dictionary<string, string> options = null)
        {
            try
            {
                return await ServerEntry<List<T>>.Get(Path, options);
            }
            catch (BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
                return null;
            }
        }

        public async Task<T> ReloadInstance(Dictionary<string, string> options = null)
        {
            try
            {
                return await ServerEntry<T>.Get(Path, Id ?? default, options);
            }
            catch (BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
                return null;
            }
        }

        public async Task<bool> SaveInstance()
        {
            try
            {
                T result = await ServerEntry<T>.Post(Path, this);
                Id = result.Id;
            }
            catch(BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
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
            catch(BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteInstance()
        {
            try
            {
                await ServerEntry.Delete(Path, Id ?? default);
            }
            catch(BadResponseStatusCodeException e)
            {
                ServerEntry.CommonExceptionHandler(e);
                return false;
            }
            return true;
        }
    }

    public class ServerEntry
    {
        public static async Task<Stream> GetFile(string path)
        {
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Server.Uri + path);

            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadAsStreamAsync();
            }
            else
            {
                throw new BadResponseStatusCodeException(httpResponse.ReasonPhrase, httpResponse.StatusCode.ToString(), (int)httpResponse.StatusCode, httpResponse.RequestMessage.RequestUri.ToString());
            }
        }

        public static async Task<string> PostFile(string path, Stream file, string fileName)
        {
            HttpClient httpClient = new HttpClient();
            file.Seek(0, SeekOrigin.Begin);
            byte[] fileBinaries = new byte[file.Length];
            await file.ReadAsync(fileBinaries, 0, (int)file.Length);

            ByteArrayContent fileContent = new ByteArrayContent(fileBinaries);
            MultipartFormDataContent httpContent = new MultipartFormDataContent();
            httpContent.Add(fileContent, "file", fileName);

            HttpResponseMessage httpResponse = await httpClient.PostAsync(Server.ApiUri + path, httpContent);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new BadResponseStatusCodeException(httpResponse.ReasonPhrase, httpResponse.StatusCode.ToString(), (int)httpResponse.StatusCode, httpResponse.RequestMessage.RequestUri.ToString());
            }
            else
            {
                return await httpResponse.Content.ReadAsStringAsync();
            }
        }

        public static async Task<bool> ActionPost(string path, object item, Dictionary<string, string> options = null)
        {

            HttpResponseMessage httpResponse;
            HttpClient httpClient = new HttpClient();

            string _path = path;

            if (options != null)
            {
                _path += "?";
                _path += await (new FormUrlEncodedContent(options)).ReadAsStringAsync();
            }


            if (item != null)
            {
                string itemJson = JsonConvert.SerializeObject(item, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                StringContent httpContent = new StringContent(itemJson, Encoding.UTF8, "application/json");
                httpResponse = await httpClient.PostAsync(Server.ApiUri + _path, httpContent);
            }
            else
            {
                httpResponse = await httpClient.PostAsync(Server.ApiUri + _path, null);
            }

            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new BadResponseStatusCodeException(httpResponse.ReasonPhrase, httpResponse.StatusCode.ToString(), (int)httpResponse.StatusCode, httpResponse.RequestMessage.RequestUri.ToString());
            }
        }


        public static async Task<bool> Delete(string path)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.DeleteAsync(Server.ApiUri + path);

            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new BadResponseStatusCodeException(httpResponse.ReasonPhrase, httpResponse.StatusCode.ToString(), (int)httpResponse.StatusCode, httpResponse.RequestMessage.RequestUri.ToString());
            }
        }

        public static async Task<bool> Delete(string path, int id)
        {
            return await Delete(path + "/" + id);
        }

        public static void CommonExceptionHandler(BadResponseStatusCodeException e)
        {
            switch (e.StatusCode)
            {
                case 401:
                    MessageBox.Show(e.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                default:
                    string errorMessage = e.Status + " (" + e.StatusCode.ToString() + ")";
                    if (!string.IsNullOrEmpty(e.Message))
                    {
                        errorMessage += ": " + e.Message;
                    }
                    errorMessage += "\nOn: " + e.RequestUri;
                    MessageBox.Show(errorMessage, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.Log(errorMessage + " Stack:" + e.StackTrace, Logger.LogType.Error);
                    break;
            };
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

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Server.ApiUri + _path);

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
                throw new BadResponseStatusCodeException(httpResponse.ReasonPhrase, httpResponse.StatusCode.ToString(), (int)httpResponse.StatusCode, httpResponse.RequestMessage.RequestUri.ToString());
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
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            HttpClient httpClient = new HttpClient();
            StringContent httpContent = new StringContent(itemJson, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await httpClient.PostAsync(Server.ApiUri + path, httpContent);

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
                string message = await httpResponse.Content.ReadAsStringAsync();
                throw new BadResponseStatusCodeException(message, httpResponse.StatusCode.ToString(), (int)httpResponse.StatusCode, httpResponse.RequestMessage.RequestUri.ToString());
            }
        }

        public static async Task<bool> Put(string path, object item)
        {
            string itemJson = JsonConvert.SerializeObject(item, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            HttpClient httpClient = new HttpClient();
            StringContent httpContent = new StringContent(itemJson, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await httpClient.PutAsync(Server.ApiUri + path, httpContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new BadResponseStatusCodeException(httpResponse.ReasonPhrase, httpResponse.StatusCode.ToString(), (int)httpResponse.StatusCode, httpResponse.RequestMessage.RequestUri.ToString());
            }
        }

        public static async Task<bool> Put(string path, int id, object item)
        {
            return await Put(path + "/" + id, item);
        }
    }
}
