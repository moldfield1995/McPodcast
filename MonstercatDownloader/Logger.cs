using System;
using System.IO;
using System.Collections.Generic;
namespace Loging
{
    /// <summary>
    /// Matthew O, GH:moldfield
    /// Vershon 0.9
    /// 
    /// </summary>
    static class Logger
    {
        public enum LogType
        {
            Log,
            Message,
            Error,
            Fatal,
        }
        private static string fileLocation;
        private static string currentLogs;
        public static void Initilize()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "MCApp");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            fileLocation = "MCLog " + DateTime.Now.ToFileTime() + ".txt";
            fileLocation = Path.Combine(path, fileLocation);
            File.Create(fileLocation).Close();
            currentLogs = "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        public static void Log(string message, LogType logType)
        {
            string temp = "[" + DateTime.Now.TimeOfDay.ToString() + "] " + logType.ToString() + " - " + message + " \r\n";
            currentLogs += temp;

        }
        /// <summary>
        /// 
        /// </summary>
        public static void PushLogs()
        {
            if (currentLogs == "")
                return;
            try
            {
                using (StreamWriter Stream = new StreamWriter(fileLocation,true))
                {
                    //Stream.NewLine = "\n";
                    Stream.WriteLine(currentLogs);
                    currentLogs = "";
                }
            }
            catch (IOException ioEx)
            {
                Log("Log Write Failed - " + ioEx.Message, LogType.Fatal);
            }
            catch (Exception e)
            {

            }
        }

    }
}
