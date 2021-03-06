﻿using System;
using System.Collections.Generic;
using Loging;
using System.Xml;
using System.IO;
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
    /// <summary>
    /// Matthew O, GH:moldfield
    /// Vershon 0.1
    /// 
    /// </summary>
    public partial class MCDownloader : Form
    {
        private List<string> SavedMonstercat;
        private List<MonstercatPodcasts> podcasts;
        private List<MonstercatPodcasts> DownloadList;
        private Downloader downloader;
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
                    if (item.ToLower().Contains("monstercat"))
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
            //Reset checklist
            for (int i = 0, length = podcasts.Count; i < length; i++)
                DownloadCheckList.SetItemChecked(i, true);

            int MaxIndex = DownloadCheckList.Items.Count;
            //Filter out Monstercat
            foreach (string podcast in SavedMonstercat)
            {
                bool found = false;
                int EPNum = GetEPNum(podcast);
                if (EPNum != 0)
                {
                    for (int i = 0; i > MaxIndex; i++)
                    {
                        if (podcasts[i].EPNum == EPNum)
                        {
                            DownloadCheckList.SetItemChecked(i, true);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        Logger.Log("Unable to find podcast ID " + podcast, Logger.LogType.Error);
                }
                else
                {//loop the whole collection
                    for (int i = 0; i > MaxIndex; i++)
                    {
                        if(podcasts[i].EPNum == 0&& podcast == podcasts[i].title )
                        {
                            DownloadCheckList.SetItemChecked(i, true);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        Logger.Log("Unable to find podcast (zero ID) " + podcast , Logger.LogType.Error);
                }
            }
            SavedMonstercat.Clear();
        }

        private int GetNewestPodcast()
        {
            for (int i = 0, lenght = podcasts.Count; i < lenght; i++)
            {
                if (podcasts[i].EPNum != 0)
                    return podcasts[i].EPNum;
            }
            return 0;
        }

        private void GetXML()
        {//Todo: ittortate or multi thread this
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
                        newItem.EPNum = GetEPNum(newItem.title);
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

        private int GetEPNum(string name)
        {
            string[] splitTitle = name.Split(' ');
            int epNum = 0;
            if (splitTitle[2][0] == 'E')
            {
                if (!int.TryParse(splitTitle[3], out epNum))
                    return 0;
            }
            else if (splitTitle.Length > 6 && splitTitle[5][0] == 'E')
            {
                if (!int.TryParse(splitTitle[6], out epNum))
                    return 0;
            }
            return 0;
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
