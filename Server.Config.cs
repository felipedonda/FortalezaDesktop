using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using FortalezaDesktop.Models;
using System.Diagnostics;

namespace FortalezaDesktop
{
    class ServerConfigurationFile
    {
        public string Hostname { get; set; }
        public string Port { get; set; }
        public bool Https { get; set; }
    }

    class ServerResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public int Id { get; set; }
    }

    public class Server
    {
        public static string URI { get; set; }

        static public void LoadServerConfig()
        {
            string configurationFilePath = Directory.GetCurrentDirectory() + "\\Server.Config.json";
            if (File.Exists(configurationFilePath))
            {
                string jsonString = File.ReadAllText(configurationFilePath);
                ServerConfigurationFile configuration = JsonConvert.DeserializeObject<ServerConfigurationFile>(jsonString);
                if (configuration.Https)
                {
                    URI = "https://" + configuration.Hostname + ":" + configuration.Port;
                }
                else
                {
                    URI = "http://" + configuration.Hostname + ":" + configuration.Port;
                }
            }
            else
            {
                ServerConfigurationFile newConfiguration = new ServerConfigurationFile
                {
                    Hostname = "localhost",
                    Https = false,
                    Port = "8000"
                };
                File.WriteAllText(configurationFilePath, JsonConvert.SerializeObject(newConfiguration));
                URI = "http://" + newConfiguration.Hostname + ":" + newConfiguration.Port;
            }
        }

        public static async Task<bool> CheckConnection()
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(URI);
                return httpResponse.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                if (ex.InnerException is SocketException) return false;
                throw ex;
            }
        }
    }

    public class Model<T> where T : new()
    {
        public static async Task<T> Get(string path)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync(Server.URI + path);
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
                throw new BadResponseStatusCodeException("Bad status code: " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
            }
        }

        public static async Task<T> Get(string path, int id)
        {
            return await Get(path + "/" + id);
        }

        public static async Task<T> Post(string path, T item)
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
                throw new BadResponseStatusCodeException("Bad status code: " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
            }
        }

        public static async Task<bool> Put(string path, T item)
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
                /*
                JToken result = JsonConvert.DeserializeObject<JToken>(jsonString);

                if ((bool)result["success"])
                {
                    return true;
                }
                else
                {
                    throw new GenericServerException("Error: " + (bool)result["error"]);
                }
                */
                return true;
            }
            else
            {
                throw new BadResponseStatusCodeException("Bad status code: " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
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
                throw new BadResponseStatusCodeException("Bad status code: " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
            }
        }

        public static async Task<bool> Delete(string path, int id)
        {
            return await Delete(path + "/" + id);
        }

    }


    class BadResponseStatusCodeException : Exception
    {
        public BadResponseStatusCodeException()
        {

        }
        public BadResponseStatusCodeException(string message)
            :base(message)
        {

        }
    }

    class MissingPropertyException : Exception
    {
        public MissingPropertyException()
        {

        }
        public MissingPropertyException(string message)
            : base(message)
        {

        }
    }

    class GenericServerException : Exception
    {
        public GenericServerException()
        {

        }
        public GenericServerException(string message)
            : base(message)
        {

        }
    }

    class EmptyObjectException : Exception
    {
        public EmptyObjectException()
        {

        }
        public EmptyObjectException(string message)
            :base(message)
        {
            
        }
    }
}
