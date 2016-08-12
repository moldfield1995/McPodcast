using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;
using System;
using Loging;

namespace MonstercatDownloader
{
    /// <summary>
    /// Matthew O, GH:moldfield
    /// Vershon 0.9
    /// 
    /// </summary>
    class Downloader 
    {
        private enum DownloadState
        {
            Waiting,
            Downloading,
            Canseling,
        }
        private List<MonstercatPodcasts> DownloadList;
        private Label StatueText;
        private ProgressBar ProgressBar;
        private string DownloadLocation;
        private DownloadState downloadState = DownloadState.Waiting;
        /// <summary>
        /// Initalizes the refrences for the progress bar and the satus text
        /// </summary>
        /// <param name="statusText">The Label to be updated with download state.</param>
        /// <param name="progressBar">The ProgressBar to be updated with the current progress on file beeing downloaded.</param>
        public Downloader(Label statusText, ProgressBar progressBar)
        {
            StatueText = statusText;
            ProgressBar = progressBar;
            DownloadList = new List<MonstercatPodcasts>();
        }

        /// <summary>
        /// Gets if the downloader is currently working
        /// </summary>
        /// <returns>Returns true if ready to download</returns>
        public bool GetDownloading() {
#pragma warning disable CS0162 // Unreachable code detected(Keep strucher of switch case)
            switch (downloadState)
            {
                case DownloadState.Waiting:
                    return true;
                    break;
                case DownloadState.Downloading:
                case DownloadState.Canseling:
                    return false;
                    break;
                default:
                    // log error
                    return false;
                    break;
#pragma warning restore CS0162 // Unreachable code detected
            }

        }

        public void DownloadFiles(string downloadLocation, List<MonstercatPodcasts> downloadList)
        {
            if (downloadState != DownloadState.Waiting)
                return;
            downloadState = DownloadState.Downloading;
            if (Directory.Exists(downloadLocation) && downloadList.Count > 0)
            {
                DownloadLocation = downloadLocation;
                DownloadList.AddRange(downloadList);
                DownloadNextFile();
            }
            else
            {
                StatueText.Text = "Invalid Dowload Location or No items Selected";
                downloadState = DownloadState.Waiting;
            }
        }

        private void DownloadNextFile()
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += DownloadProgressChanged;
                wc.DownloadFileCompleted += DownloadCompleted;
                string downloadLocation = DownloadLocation + Path.DirectorySeparatorChar + DownloadList[0].title + ".m4a";
                StatueText.Text = "Downloading " + DownloadList[0].title;
                wc.DownloadFileAsync(new System.Uri(DownloadList[0].URL), downloadLocation);
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.Log("We Where Downloading " + DownloadList[0].title, Logger.LogType.Message);
                Logger.Log(e.Error.Message, Logger.LogType.Error);
                HttpStatusCode hCode = GetHttpStatusCode(e.Error);
                Logger.Log("Http Code was " + hCode.ToString(), Logger.LogType.Error);
                //TODO: check if the file exsits, if so delete it
                //check if we have internet accsess if not stop
            }
            DownloadList.RemoveAt(0);

            if(DownloadList.Count > 0)
                DownloadNextFile();
            else
            {
                StatueText.Text = "Completed";
                ProgressBar.Value = 0;
                Logger.PushLogs();
                downloadState = DownloadState.Waiting;
            }
        }

        HttpStatusCode GetHttpStatusCode(Exception err)
        {
            if (err is WebException)
            {
                WebException we = (WebException)err;
                if (we.Response is HttpWebResponse)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    return response.StatusCode;
                }
            }
            return HttpStatusCode.Unused;
            
        }
    }
}
