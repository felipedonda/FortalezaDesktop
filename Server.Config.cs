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
        public static string ApiUri { get; set; }
        public static string Uri { get; set; }


        static public void LoadServerConfig()
        {
            string configurationFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Fortaleza Desktop");

            string configurationFilePath = Path.Combine(
                configurationFolderPath,
                "Server.Config.json");

            if (File.Exists(configurationFilePath))
            {
                string jsonString = File.ReadAllText(configurationFilePath);
                ServerConfigurationFile configuration = JsonConvert.DeserializeObject<ServerConfigurationFile>(jsonString);
                if (configuration.Https)
                {
                    Uri = "https://" + configuration.Hostname;
                }
                else
                {
                    Uri = "http://" + configuration.Hostname;
                }
                if (!string.IsNullOrEmpty(configuration.Port))
                {
                    Uri += ":" + configuration.Port;
                }

                ApiUri = Uri + "/api";
            }
            else
            {
                ServerConfigurationFile newConfiguration = new ServerConfigurationFile
                {
                    Hostname = "localhost",
                    Https = false,
                    Port = "8000"
                };

                if(!Directory.Exists(configurationFolderPath))
                {
                    Directory.CreateDirectory(configurationFolderPath);
                }

                File.WriteAllText(configurationFilePath, JsonConvert.SerializeObject(newConfiguration));
                Uri = "http://" + newConfiguration.Hostname + ":" + newConfiguration.Port;
                ApiUri = Uri + "/api";

            }
        }

        public static async Task<bool> CheckConnection()
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(ApiUri);
                return httpResponse.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }


    public class BadResponseStatusCodeException : Exception
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string RequestUri { get; set; }

        public BadResponseStatusCodeException()
        {

        }
        public BadResponseStatusCodeException(string message, string status, int statusCode, string requestUri)
            :base(message)
        {
            Status = status;
            StatusCode = statusCode;
            RequestUri = requestUri;
        }
    }
}
