using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using EmailUploader.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using EmailUploader.BLL;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class UploadAttachmentsProgressForm : Form
    {
        private Outlook.MailItem CurrentMailItem = null;

        public string AttachmentLinks { get; set; }
        public string CurrentUploadLog = String.Empty;
        public string _rootFolderPath = String.Empty;
        public EUSiteSetting _siteSetting = null;
        public string _siteURL = String.Empty;
        public string _webURL = String.Empty;
        public string _listName = String.Empty;

        public string _destinationFolderUrl = String.Empty;
        public string _sourceFolder = String.Empty;
        public UploadAttachmentsProgressForm()
        {
            InitializeComponent();
        }

        public void Initialize(string destinationFolderUrl, string sourceFolder, EUSiteSetting siteSetting, string rootFolderPath, string siteURL, string webURL, string listName, List<EUEmailUploadFile> emailUploadFiles, Outlook.MailItem mailItem)
        {
//            CurrentMailItem.MessageClass
            _destinationFolderUrl = destinationFolderUrl;
            _sourceFolder = sourceFolder;
            _webURL = webURL;
            _siteURL = siteURL;
            _siteSetting = siteSetting;
            _listName = listName;
            DestinationValueLabel.Text = destinationFolderUrl;
            CurrentMailItem = mailItem;
            foreach (EUEmailUploadFile emailUploadFile in emailUploadFiles)
            {
                ListViewGroup listViewGroup = EmailsListView.Groups[emailUploadFile.EntryID];
                if (listViewGroup == null)
                {
                    EmailsListView.Groups.Add(emailUploadFile.EntryID, emailUploadFile.MetaData.Subject);
                }
            }
            foreach (EUEmailUploadFile emailUploadFile in emailUploadFiles)
            {
                ListViewGroup listViewGroup = EmailsListView.Groups[emailUploadFile.EntryID];
                FileInfo fileInfo = new FileInfo(emailUploadFile.FilePath);
                ListViewItem listViewItem = new ListViewItem(listViewGroup);
                listViewItem.Checked = true;
                listViewItem.Tag = emailUploadFile.MetaData;
                listViewItem.Text = fileInfo.Name;
                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, fileInfo.Name));
                EmailsListView.Items.Add(listViewItem);
            }
        }

        public byte[] ReadByteArrayFromFile(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (CancelButton.Text == "&Cancel")
            {
                backgroundWorker1.CancelAsync();
            }
            else
            {
                this.Close();
            }

        }
        public delegate void UploadDelegate();
        public void Upload()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UploadDelegate(Upload));
            }
            else
            {
                ErrorLabel.Visible = false;

                ProgressValueLabel.Text = "Started.";
                ProgressValueLabel.Refresh();
                SetButtonEnabilityOnUploading();
                int totalRows = EmailsListView.Items.Count;
                List<EUField> fields = SharePointManager.GetFields(_siteSetting, _webURL, _listName);
                SharePointCopyWS.CopyResult[] myCopyResultArray = null;
                string log = String.Empty;
                bool hasError = false;

                for (int i = 0; i < EmailsListView.Items.Count; i++)
                {
                    if (backgroundWorker1.CancellationPending == true)
                    {
                        ProgressValueLabel.Text = "Cancelled.";
                        ProgressValueLabel.Refresh();
                        return;
                    }
                    ListViewItem listViewItem = EmailsListView.Items[i];
                    if (listViewItem.Checked == false)
                        continue;
                    EUEmailMetaData metaData = (EUEmailMetaData)listViewItem.Tag;
                    string filename = listViewItem.SubItems[1].Text;
                    string newFilename = listViewItem.Text;
                    ProgressValueLabel.Text = newFilename + " is being uploaded...";
                    ProgressValueLabel.Refresh();
                    string filePath = _sourceFolder + "\\" + filename;
                    byte[] itemByteArray = ReadByteArrayFromFile(filePath);
                    string sourceDest = _destinationFolderUrl + "/" + newFilename;
                    AttachmentLinks += sourceDest.Replace(" ","%20") + Environment.NewLine;

                    string[] sourceDests = new string[] { sourceDest };
                    EUListItem listItem;
                    SharePointManager.UploadFile(_siteSetting, _listName, _rootFolderPath, _siteURL,  _webURL, newFilename, sourceDests, itemByteArray, fields, metaData, null, out listItem);
                    /*
                    foreach (SharePointCopyWS.CopyResult copyResult in myCopyResultArray)
                    {
                        if (copyResult.ErrorCode != Sobiens.Office.SharePointOutlookConnector.SharePointCopyWS.CopyErrorCode.Success)
                            hasError = true;
                        log += "DestinationUrl:" + copyResult.DestinationUrl + Environment.NewLine + "ErrorCode:" + copyResult.ErrorCode.ToString() + Environment.NewLine + "ErrorMessage:" + copyResult.ErrorMessage + Environment.NewLine;
                    }
                    LogManager.Log(log, EULogModes.Normal);
                     */ 
                    SetProgressBar(i, totalRows);
                }
                CurrentUploadLog = log;
                if (hasError == true)
                {
                    ErrorLabel.Visible = true;
                }
                else
                {
                    CurrentMailItem.Body = AttachmentLinks + CurrentMailItem.Body;
                    for (int i = 0; i < EmailsListView.Items.Count; i++)
                    {
                        if (EmailsListView.Items[i].Checked == true)
                        {
                            string filename = EmailsListView.Items[i].SubItems[1].Text;
                            for (int x = CurrentMailItem.Attachments.Count; x > 0; x--)
                            {
                                if (CurrentMailItem.Attachments[x].FileName == filename)
                                    CurrentMailItem.Attachments[x].Delete();
                            }
                        }
                    }
                }
            }
        }

        private void SetProgressBar(int currentIndex, int totalRows)
        {
            int percentage = (int)((float)currentIndex * (float)100 / (float)totalRows);
            if (percentage == 0)
            {
                percentage = 5;
            }
            progressBar1.Value = percentage;
            progressBar1.Refresh();
        }

        public void SetButtonEnabilityOnUploading()
        {
            UploadButton.Enabled = false;
            CancelButton.Enabled = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Upload();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UploadButton.Enabled = false;
            CancelButton.Enabled = true;
            CancelButton.Text = "&Close";
            progressBar1.Value = 100;
            ProgressValueLabel.Text = "Completed.";
            ProgressValueLabel.Refresh();

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void LogButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(CurrentUploadLog);
        }
    }
}
