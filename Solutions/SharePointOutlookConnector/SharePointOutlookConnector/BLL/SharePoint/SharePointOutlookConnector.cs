using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using System.Windows.Forms;
using EmailUploader.BLL.Entities;
using System.IO;
using System.Drawing;
using System.Collections;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.SharePoint
{
    public class SharePointOutlookConnector:IOutlookConnector
    {
        // JOEL JEFFERY 20110711
        /// <summary>
        /// Occurs when [upload failed].
        /// </summary>
        public event EventHandler UploadFailed;

        // JON SILVER JULY 2011
        /// <summary>
        /// Occurs when [upload succeeded].
        /// </summary>
        public event EventHandler UploadSucceeded;

        private bool addedEventHandler = false;
        // JON SILVER JULY 2011
        EventHandler eventUploadSucceededHandler = null;
        // JOEL JEFFERY 20110712
        EventHandler eventUploadFailedHandler = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointOutlookConnector"/> class.
        /// </summary>
        public SharePointOutlookConnector() {
            if (!addedEventHandler)
            {
                eventUploadFailedHandler = new EventHandler(SharePointOutlookConnector_UploadFailed);
                eventUploadSucceededHandler = new EventHandler(SharePointOutlookConnector_UploadSucceeded);
                BackgroundThreadManager.GetInstance().UploadSucceeded += eventUploadSucceededHandler;
                addedEventHandler = true;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SharePointOutlookConnector"/> is reclaimed by garbage collection.
        /// </summary>
        ~SharePointOutlookConnector()
        {
            if (eventUploadSucceededHandler != null)
            {
                BackgroundThreadManager.GetInstance().UploadFailed -= eventUploadFailedHandler;
                BackgroundThreadManager.GetInstance().UploadSucceeded -= eventUploadSucceededHandler;
            }
        }

        // JOEL JEFFERY 20110712
        /// <summary>
        /// Handles the UploadFailed event of the SharePointOutlookConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SharePointOutlookConnector_UploadFailed(object sender, EventArgs e)
        {
            if (UploadFailed != null)
                UploadFailed(sender, e);

        }

        // JON SILVER JULY 2011
        /// <summary>
        /// Handles the UploadSucceeded event of the SharePointOutlookConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SharePointOutlookConnector_UploadSucceeded(object sender, EventArgs e)
        {
            if (UploadSucceeded != null)
                UploadSucceeded(sender, e);

        }

        public ISPCFolder GetRootFolder(EUSiteSetting siteSetting)
        {
            return new EUWeb(siteSetting.Url, siteSetting.Url, siteSetting, siteSetting.Url);
        }
        public List<ISPCFolder> GetSubFolders(ISPCFolder folder)
        {
            List<ISPCFolder> subFolders = new List<ISPCFolder>();
            if (folder as EUWeb != null)
            {
                EUWeb web = (EUWeb)folder;
                List<EUWeb> webs = SharePointManager.GetWebs(web.Url, web.SiteSetting);
                foreach (EUWeb _web in webs)
                {
                    subFolders.Add(_web);
                }
                List<EUList> lists = SharePointManager.GetLists(web.Url, web.SiteSetting);
                foreach (EUList list in lists)
                {
                    if (
                        (list.ServerTemplate == 101 || list.ServerTemplate == 100 || list.BaseType == 1) //or BaseType == 1 - JOEL JEFFERY 20110708
                        && list.Hidden == false
                        )
                    {
                        subFolders.Add(list);
                    }
                }
            }
            else if (folder as EUFolder != null)
            {
                EUFolder _folder = (EUFolder)folder;
                IEnumerable<EUFolder> folders = SharePointManager.GetFolders(_folder);
                foreach (EUFolder __folder in folders)
                {
                    subFolders.Add(__folder);
                }
            }
            return subFolders;
        }

        public List<ISPCView> GetViews(ISPCFolder folder)
        {
            EUFolder _folder = folder as EUFolder;
            return SharePointManager.GetViews(_folder.WebUrl, _folder.ListName, folder.SiteSetting);
        }

        public List<ISPCItem> GetListItems(ISPCFolder folder, ISPCView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, EUCamlFilters filters, out string listItemCollectionPositionNext, out int itemCount)
        {
            EUFolder _folder = folder as EUFolder;
            return SharePointManager.GetListItems(folder.SiteSetting, (EUView)view, sortField, isAsc, _folder.IsDocumentLibrary, _folder.WebUrl, _folder.ListName, _folder.FolderPath, currentListItemCollectionPositionNext, filters, out listItemCollectionPositionNext, out itemCount);
        }

        public void BindItemsToListViewControl(ISPCFolder folder, ISPCView view, List<ISPCItem> items, DataGridView LibraryContentDataGridView)
        {
            EUFolder _folder = folder as EUFolder;
            for (int i = LibraryContentDataGridView.Columns.Count - 1; i > 2; i--)
            {
                LibraryContentDataGridView.Columns.RemoveAt(i);
            }
            for (int i = 0; i < view.ViewFields.Count; i++)
            {
                string fieldName = view.ViewFields[i].Name;
                LibraryContentDataGridView.Columns.Add(fieldName, fieldName);
                if (fieldName == "DocIcon" || fieldName == "Attachments")
                    LibraryContentDataGridView.Columns[fieldName].Visible = false;
                LibraryContentDataGridView.Columns[fieldName].Tag = fieldName;
            }
            LibraryContentDataGridView.Rows.Clear();
            foreach (EUListItem item in items)
            {
                int newRowIndex = LibraryContentDataGridView.Rows.Add();
                DataGridViewRow newRow = LibraryContentDataGridView.Rows[newRowIndex];
                BindListItemToRow(item, newRow, _folder.IsDocumentLibrary, view);                
            }
        }

        public static void BindListItemToRow(EUListItem listItem, DataGridViewRow row, bool isDocumentLibrary, ISPCView view)
        {
            string title = String.Empty;
            if (isDocumentLibrary == true)
            {
                string fileName = listItem.Title;
                string extensionName = String.Empty;
                if (fileName.LastIndexOf(".") > 0)
                {
                    extensionName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
                }
                title = fileName;
                row.Cells["ExtensionImageColumn"].Value = ImageManager.GetInstance().GetExtensionImageFromResource(extensionName);
            }
            else
            {
                title = listItem.Title;
                row.Cells["ExtensionImageColumn"].Value = ImageManager.GetInstance().GetExtensionImageFromResource("list");
            }
            if (listItem.CheckoutUser != String.Empty)
            {
                row.Cells["ExtensionImageColumn"].Style.BackColor = Color.MediumSeaGreen;
            }

            row.Cells["TitleColumn"].Value = title;
            row.Cells["FilePathColumn"].Value = listItem.URL;
            for (int i = 0; i < view.ViewFields.Count; i++)
            {
                string fieldName = view.ViewFields[i].Name;
                if (listItem.Properties["ows_" + fieldName] != null)
                    row.Cells[fieldName].Value = listItem.Properties["ows_" + fieldName].Value;
            }
            row.Tag = listItem;
        }

        public void UploadFiles(ISPCFolder folder, List<EUEmailUploadFile> uploadFiles, List<EUField> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            EUFolder _folder = folder as EUFolder;
            string destinationFolderUrl = _folder.WebUrl.TrimEnd(new char[] { '/' }) + "/" + _folder.FolderPath.TrimStart(new char[] { '/' });

            BackgroundThreadManager.GetInstance().QueueUploadItems(_folder, destinationFolderUrl, uploadFiles, fields, fieldInformations, sharePointListViewControl);

            /*
            List<EUField> fields = SharePointManager.GetFields(_folder.SiteSetting, _folder.WebUrl, _folder.ListName);
            foreach (EUEmailUploadFile uploadFile in uploadFiles)
            {
                string copySource = new FileInfo(uploadFile.FilePath).Name;
                string[] copyDest = new string[1] { destinationFolderUrl + "/" + copySource };
                byte[] itemByteArray = SharePointManager.ReadByteArrayFromFile(uploadFile.FilePath);

                EUUploadItem uploadItem = new EUUploadItem(_folder.SiteSetting, _folder.ListName, _folder.RootFolderPath, _folder.WebUrl, copySource, copyDest, itemByteArray, fields, uploadFile.MailItem, fieldInformations);
                BackgroundThreadManager.GetInstance().QueueUploadItems(uploadItem);
            }
            */ 
        }

        public void DeleteListItem(ISPCItem item)
        {
            EUListItem listItem = (EUListItem)item;
            SharePointManager.DeleteListItem(listItem.SiteSetting, listItem.WebURL, listItem.ListName, listItem.URL, listItem.ID);
        }
        
        // JOEL JEFFERY 20110710
        /// <summary>
        /// Checks the folder exists.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        public bool CheckFolderExists(ISPCFolder folder, string newFolderName)
        {
            //            EUListItem listItem = (EUListItem)item;
            EUFolder listFolder = (EUFolder)folder;
            return SharePointManager.CheckFolderExists(listFolder.SiteSetting, listFolder.WebUrl, listFolder.ListName, listFolder.FolderPath, newFolderName);
        }

        public bool CheckFileExistency(ISPCFolder folder, ISPCItem item, string newFileName)
        {
//            EUListItem listItem = (EUListItem)item;
            EUFolder listFolder = (EUFolder)folder;
            return SharePointManager.CheckFileExistency(listFolder.SiteSetting, listFolder.WebUrl, listFolder.ListName, listFolder.FolderPath, null, newFileName);
        }

        public void CopyFile(ISPCFolder folder, ISPCItem item, string newFileName)
        {
            EUListItem listItem = (EUListItem)item;
            EUFolder listFolder = (EUFolder)folder;

            string folderPath = listFolder.WebUrl + "/" + listFolder.FolderPath;
            string webUrl = listFolder.WebUrl;
            string listName = listFolder.ListName;

            SharePointCopyWS.CopyResult[] myCopyResultArray = null;
            SharePointManager.CopyFile(listFolder.SiteSetting, webUrl, listItem.URL, folderPath + "/" + newFileName, out myCopyResultArray);
        }
    }
}
