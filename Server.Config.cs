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
        public static string APIURI { get; set; }
        public static string URI { get; set; }


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
                    URI = "https://" + configuration.Hostname;
                }
                else
                {
                    URI = "http://" + configuration.Hostname;
                }
                if (!string.IsNullOrEmpty(configuration.Port))
                {
                    URI += ":" + configuration.Port;
                }

                APIURI = URI + "/api";
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
                URI = "http://" + newConfiguration.Hostname + ":" + newConfiguration.Port;
                APIURI = URI + "/api";

            }
        }

        public static async Task<bool> CheckConnection()
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(APIURI);
                return httpResponse.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                //if (ex.InnerException is SocketException) return false;
                //throw ex;
                return false;
            }
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
