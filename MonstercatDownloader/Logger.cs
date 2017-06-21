using System;
using System.IO;
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
        /// <summary>
        /// Initalizes the singleton logger
        /// </summary>
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
        /// Addes the messaged to logs to be pushed to file
        /// </summary>
        /// <param name="message">The message to log</param>
        public static void Log(string message, LogType logType)
        {
            string temp = "[" + DateTime.Now.TimeOfDay.ToString() + "] " + logType.ToString() + " - " + message + " \r\n";
            currentLogs += temp;

        }
        /// <summary>
        /// Saves the current logs to file
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
