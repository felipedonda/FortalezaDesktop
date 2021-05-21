using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FortalezaDesktop.Utils
{
    public class Logger
    {
        public enum LogType
        {
            Info = 0,
            Warning = 1,
            Error = 2
        }

        private static string LogName { get; set; }

        public async static void Log(string message, LogType logType)
        {
            if (string.IsNullOrEmpty(LogName))
            {
                LogName = (DateTime.Now).ToString("yyyy-MM-dd HH-mm");
            }

            string fortalezaFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Fortaleza Desktop");

            if(!Directory.Exists(fortalezaFolderPath))
            {
                Directory.CreateDirectory(fortalezaFolderPath);
            }

            string logFolderPath = Path.Combine(
                fortalezaFolderPath,
                "Logs");

            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            string logFilePath = Path.Combine(
                logFolderPath,
                LogName + ".log");

            string fullMessage = (DateTime.Now).ToString("yyyy-MM-dd | HH:mm:ss:ffff") + " | " +
                logType.ToString().ToUpper() +
                " | " + message.Replace("\n", " ").Replace("\r", "") + "\n";

            await File.AppendAllTextAsync(logFilePath,fullMessage);
        }
    }
}
