using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loging;
using System.Xml;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MonstercatDownloader
{
    struct MonstercatPodcasts
    {
        public string title;
        public string URL;
        public int EPNum;
        public override string ToString() { return title; }
    }
    public partial class MCDownloader : Form
    {
        private List<string> SavedMonstercat;
        private List<MonstercatPodcasts> podcasts;
        private List<MonstercatPodcasts> DownloadList;
        private Downloader downloader;
        private bool Downloading = false;
        private int NewestPodcast;
        public MCDownloader()
        {
            InitializeComponent();
            GetXML();
            UpdateCheckList();
            DownloadList = new List<MonstercatPodcasts>();
            SavedMonstercat = new List<string>();
            NewestPodcast = GetNewestPodcast();
            downloader = new Downloader(StatueText, ProgressStatus);
        }
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (!UpdateFiles())
                return;
            EditCheckList();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.PathTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            UpdateDownloadList();
            downloader.DownloadFiles(PathTextBox.Text, DownloadList);
            //DownloadNextFile();
        }

        private void UpdateDownloadList()
        {
            DownloadList.Clear();
            for (int i = 0, length = podcasts.Count; i < length; i++)
            {
                if (DownloadCheckList.GetItemChecked(i))
                    DownloadList.Add(podcasts[i]);
            }
        }

        private void DownloadNextFile()
        {
            if (Directory.Exists(PathTextBox.Text) && DownloadList.Count > 0)
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += DownloadProgressChanged;
                    wc.DownloadFileCompleted += DownloadCompleted;
                    string downloadLocation = PathTextBox.Text + Path.DirectorySeparatorChar + DownloadList[0].title + ".m4a";
                    StatueText.Text = "Downloading " + DownloadList[0].title;

                    wc.DownloadFileAsync(new System.Uri(DownloadList[0].URL), downloadLocation);
                    DownloadList.RemoveAt(0);
                }
            }
            else
            {
                Downloading = false;
                StatueText.Text = "Complete";
            }
        }

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownloadNextFile();
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressStatus.Value = e.ProgressPercentage;
        }

        private bool UpdateFiles()
        {
            if (Directory.Exists(PathTextBox.Text))
            {
                SavedMonstercat.Clear();
                //Get All files in location
                string[] FileList = Directory.GetFiles(PathTextBox.Text);
                //Get File Name
                for (int i = 0, length = FileList.Length; i < length; i++)
                {
                    FileList[i] = Path.GetFileName(FileList[i]);
                }
                //Filter For MonsterCat
                foreach (string item in FileList)
                    if (item.ToLower().Contains("monstercat podcast"))
                        SavedMonstercat.Add(item);

                StatueText.Text = "Files Updated";
            }
            else
            {
                StatueText.Text = "Invalid Path";
                ErrorPopUp.SetError(this.BrowseButton, "Invalid Path");
                return false;
            }
            return true;
        }

        private void EditCheckList()
        {
            for (int i = 0, length = podcasts.Count; i < length; i++)
                DownloadCheckList.SetItemChecked(i, true);
            int MaxIndex = DownloadCheckList.Items.Count;
            //Filter out Monstercat
            foreach (string podcast in SavedMonstercat)
            {
                int index = 0;
                int EPNum = 0;
                if (podcast[19] == 'E')
                {
                    string podNum = "" + podcast[23] + podcast[24] + podcast[25];
                    EPNum = int.Parse(podNum);
                    if (NewestPodcast != 0)
                        index = NewestPodcast - EPNum;
                    else
                        index = 0;

                    bool serching = true;
                    while (serching && index < MaxIndex)
                    {
                        if (podcasts[index].EPNum == EPNum)
                        {
                            DownloadCheckList.SetItemChecked(index, false);
                            serching = false;
                        }
                        else
                            index++;
                    }
                }
                else if (podcast[19] == '-')
                {
                    string podNum = "" + podcast[19] + podcast[20] + podcast[21] + podcast[22] + podcast[23];
                    for (; index < MaxIndex; index++)
                    {
                        if (podcasts[index].EPNum == 0)
                            if (podcasts[index].title.Contains(podNum))
                                DownloadCheckList.SetItemChecked(index, false);
                    }
                }
            }
            SavedMonstercat.Clear();
        }

        private int GetNewestPodcast()
        {
            int i = 0;
            while (i < 1000)
            {
                if (podcasts[i].EPNum != 0)
                    return podcasts[i].EPNum;
            }
            return 0;
        }
        private void GetXML()
        {
            podcasts = new List<MonstercatPodcasts>();
            try
            {
                using (XmlReader Reader = XmlReader.Create("https://www.monstercat.com/podcast/feed.xml"))
                {
                    while (Reader.ReadToFollowing("item"))
                    {
                        MonstercatPodcasts newItem = new MonstercatPodcasts();
                        Reader.ReadToFollowing("title");
                        newItem.title = Reader.ReadElementContentAsString();
                        if (newItem.title[19] == 'E')
                        {
                            string podNum = "" + newItem.title[23] + newItem.title[24] + newItem.title[25];
                            newItem.EPNum = int.Parse(podNum);
                        }
                        else
                            newItem.EPNum = 0;
                        Reader.ReadToFollowing("enclosure");
                        Reader.MoveToFirstAttribute();
                        newItem.URL = Reader.Value;
                        podcasts.Add(newItem);
                    }
                }
            }
            catch (FileNotFoundException fnf)
            {
                Logger.Log("File Not Found -" + fnf.Message, Logger.LogType.Fatal);
                Logger.PushLogs();
                MessageBox.Show("Unable to Get XML", "XML Get Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            catch (Exception e)
            {
                Logger.Log("Unexspected Exseption In GetXML -" + e.Message, Logger.LogType.Fatal);
                Logger.PushLogs();
                MessageBox.Show("Unable to Get XML", "XML Get Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void UpdateCheckList()
        {
            CheckedListBox.ObjectCollection items = DownloadCheckList.Items;
            foreach (MonstercatPodcasts mcp in podcasts)
                items.Add(mcp);
        }

        private void MCDownloader_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.PushLogs();
        }
    }
}
