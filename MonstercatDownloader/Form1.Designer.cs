namespace MonstercatDownloader
{
    partial class MCDownloader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.ProgressStatus = new System.Windows.Forms.ProgressBar();
            this.StatueText = new System.Windows.Forms.Label();
            this.ErrorPopUp = new System.Windows.Forms.ErrorProvider(this.components);
            this.DownloadCheckList = new System.Windows.Forms.CheckedListBox();
            this.DownloadButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorPopUp)).BeginInit();
            this.SuspendLayout();
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(12, 62);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(99, 42);
            this.UpdateButton.TabIndex = 0;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // PathTextBox
            // 
            this.PathTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PathTextBox.Location = new System.Drawing.Point(12, 23);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(273, 20);
            this.PathTextBox.TabIndex = 1;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(285, 23);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(24, 20);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // ProgressStatus
            // 
            this.ProgressStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressStatus.Location = new System.Drawing.Point(273, 255);
            this.ProgressStatus.Name = "ProgressStatus";
            this.ProgressStatus.Size = new System.Drawing.Size(100, 23);
            this.ProgressStatus.TabIndex = 3;
            // 
            // StatueText
            // 
            this.StatueText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatueText.AutoSize = true;
            this.StatueText.Location = new System.Drawing.Point(-2, 265);
            this.StatueText.Name = "StatueText";
            this.StatueText.Size = new System.Drawing.Size(184, 13);
            this.StatueText.TabIndex = 4;
            this.StatueText.Text = "Welcome To Monstercat Downloader";
            // 
            // ErrorPopUp
            // 
            this.ErrorPopUp.ContainerControl = this;
            // 
            // DownloadCheckList
            // 
            this.DownloadCheckList.BackColor = System.Drawing.SystemColors.Menu;
            this.DownloadCheckList.FormattingEnabled = true;
            this.DownloadCheckList.Location = new System.Drawing.Point(13, 111);
            this.DownloadCheckList.Name = "DownloadCheckList";
            this.DownloadCheckList.Size = new System.Drawing.Size(347, 94);
            this.DownloadCheckList.TabIndex = 5;
            // 
            // DownloadButton
            // 
            this.DownloadButton.Location = new System.Drawing.Point(259, 62);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(101, 42);
            this.DownloadButton.TabIndex = 6;
            this.DownloadButton.Text = "Download";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // MCDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 277);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.DownloadCheckList);
            this.Controls.Add(this.StatueText);
            this.Controls.Add(this.ProgressStatus);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.PathTextBox);
            this.Controls.Add(this.UpdateButton);
            this.Name = "MCDownloader";
            this.Text = "MC Downloader";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MCDownloader_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorPopUp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox PathTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.ProgressBar ProgressStatus;
        private System.Windows.Forms.Label StatueText;
        private System.Windows.Forms.ErrorProvider ErrorPopUp;
        private System.Windows.Forms.CheckedListBox DownloadCheckList;
        private System.Windows.Forms.Button DownloadButton;
    }
}

