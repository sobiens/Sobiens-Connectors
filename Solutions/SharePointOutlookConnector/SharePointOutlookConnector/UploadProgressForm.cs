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
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class UploadProgressForm : Form
    {
        public string CurrentUploadLog = String.Empty;
        public string _rootFolderPath = String.Empty;
        public EUSiteSetting _siteSetting = null;
        public string _siteURL = String.Empty;
        public string _webURL = String.Empty;
        public string _listName = String.Empty;
        public bool _isListItemAndAttachmentMode = false;
        public EUFieldInformations _fieldInfoArray = null;
        public string _destinationFolderUrl = String.Empty;
        public string _sourceFolder = String.Empty;
        public UploadProgressForm()
        {
            InitializeComponent();
        }

        public void Initialize(ISPCFolder folder, string sourceFolder, List<EUEmailUploadFile> emailUploadFiles, bool isListItemAndAttachmentMode, EUFieldInformations fieldInfoArray)
        {
            // string rootFolderPath, string destinationFolderUrl, string sourceFolder, string webURL, string listName,
            EUFolder _folder = folder as EUFolder;

            _destinationFolderUrl = _folder.WebUrl.TrimEnd(new char[]{'/'}) + "/" + _folder.FolderPath.TrimStart(new char[]{'/'});
            _sourceFolder = sourceFolder;
            _rootFolderPath = _folder.RootFolderPath;
            _siteSetting = _folder.SiteSetting;
            _siteURL = _folder.SiteUrl;
            _webURL = _folder.WebUrl;
            _listName = _folder.ListName;
            _isListItemAndAttachmentMode = isListItemAndAttachmentMode;
            _fieldInfoArray = fieldInfoArray;
            DestinationValueLabel.Text = _destinationFolderUrl;

            foreach (EUEmailUploadFile emailUploadFile in emailUploadFiles)
            {
                string groupID = String.Empty;
                string groupTitle = String.Empty;
                if (emailUploadFile.IsEmail == false)
                {
                    groupID = new FileInfo(emailUploadFile.FilePath).Name;
                    groupTitle = groupID;
                }
                else
                {
                    groupID = emailUploadFile.EntryID;
                    groupTitle = emailUploadFile.MetaData.Subject;
                }

                ListViewGroup listViewGroup = EmailsListView.Groups[groupID];
                if (listViewGroup == null)
                {
                    EmailsListView.Groups.Add(groupID, groupTitle);
                }
            }
            foreach (EUEmailUploadFile emailUploadFile in emailUploadFiles)
            {
                InsertEmailUploadFile(emailUploadFile);
                foreach (EUEmailUploadFile emailUploadFile1 in emailUploadFile.Attachments)
                {
                    InsertEmailUploadFile(emailUploadFile1);
                }
            }
        }

        private void InsertEmailUploadFile(EUEmailUploadFile emailUploadFile)
        {
            string groupID = String.Empty;
            if (emailUploadFile.IsEmail == false)
            {
                groupID = new FileInfo(emailUploadFile.FilePath).Name;
            }
            else
                groupID = emailUploadFile.EntryID;

            ListViewGroup listViewGroup = EmailsListView.Groups[groupID];
            FileInfo fileInfo = new FileInfo(emailUploadFile.FilePath);
            ListViewItem listViewItem = new ListViewItem(listViewGroup);
            listViewItem.Checked = true;
            listViewItem.Tag = emailUploadFile.MetaData;
            listViewItem.Text = fileInfo.Name;
            listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, fileInfo.Name));
            EmailsListView.Items.Add(listViewItem);
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
                if (_isListItemAndAttachmentMode == false)
                {
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
                        EUEmailMetaData metaData = listViewItem.Tag as EUEmailMetaData;
                        string filename = listViewItem.SubItems[1].Text;
                        string newFilename = listViewItem.Text ;
                        ProgressValueLabel.Text = newFilename + " is being uploaded...";
                        ProgressValueLabel.Refresh();
                        string filePath = _sourceFolder + "\\" + filename;
                        byte[] itemByteArray = SharePointManager.ReadByteArrayFromFile(filePath);
                        string sourceDest = _destinationFolderUrl + "/" + newFilename;

                        string[] sourceDests = new string[] { sourceDest };
                        EUListItem listItem;
                        SharePointManager.UploadFile(_siteSetting, _listName, _rootFolderPath, _siteURL, _webURL, newFilename, sourceDests, itemByteArray, fields, metaData, _fieldInfoArray, out listItem);

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
                }
                else
                {
                    Hashtable emails = new Hashtable();

                    for (int i = 0; i < EmailsListView.Items.Count; i++)
                    {
                        ListViewItem listViewItem = EmailsListView.Items[i];
                        //DataGridViewRow dataGridViewRow = EmailsDataGridView.Rows[i];
                        //bool included = (bool)dataGridViewRow.Cells["Included"].Value;
                        if (listViewItem.Checked == false)
                            continue;
                        Microsoft.Office.Interop.Outlook.MailItem emailItem = listViewItem.Tag as Microsoft.Office.Interop.Outlook.MailItem;
                        if(emails[emailItem.EntryID] == null)
                        {
                            emails[emailItem.EntryID] = new List<ListViewItem>();
                        }
                        ((List<ListViewItem>)emails[emailItem.EntryID]).Add(listViewItem);
                    }
                    totalRows = emails.Count;
                    int currentIndex = 0;
                    foreach(object attachmentsObject in emails.Values)
                    {
                        if (backgroundWorker1.CancellationPending == true)
                        {
                            ProgressValueLabel.Text = "Cancelled.";
                            ProgressValueLabel.Refresh();
                            return;
                        }
                        List<ListViewItem> attachments = (List<ListViewItem>)attachmentsObject;
                        ListViewItem attachmentRow = attachments[0];
                        Microsoft.Office.Interop.Outlook.MailItem emailItem = attachmentRow.Tag as Microsoft.Office.Interop.Outlook.MailItem;
                        string body = emailItem.Body;
                        int id = SharePointManager.CreateListItem(_siteSetting, _rootFolderPath, _webURL, _listName, emailItem.Subject, body);
                        
                        foreach (ListViewItem attachmentRow1 in attachments)
                        {
                            //string filename = attachmentRow1.Cells["Filename"].Value.ToString();
                            //string newFilename = attachmentRow1.Cells["NewFilename"].Value.ToString();
                            string filename = attachmentRow1.SubItems[0].Text;
                            string newFilename = attachmentRow1.Text;
                            ProgressValueLabel.Text = newFilename + " is being uploaded...";
                            ProgressValueLabel.Refresh();
                            string filePath = _sourceFolder + "\\" + filename;
                            byte[] itemByteArray = SharePointManager.ReadByteArrayFromFile(filePath);
                            string result = SharePointManager.AddAttachment(_siteSetting, _webURL, _listName, id, newFilename, itemByteArray);

                            log += "WebURL:" + _webURL + Environment.NewLine + "ListName:" + _listName + Environment.NewLine + "filePath:" + filePath + Environment.NewLine + "newFilename:" + newFilename + Environment.NewLine + "result:" + result + Environment.NewLine;
                            LogManager.Log(log, EULogModes.Normal);
                        }
                        SetProgressBar(currentIndex, totalRows);
                        currentIndex++;
                    }
                }
                CurrentUploadLog = log;
                if (hasError == true)
                {
                    ErrorLabel.Visible = true;
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
