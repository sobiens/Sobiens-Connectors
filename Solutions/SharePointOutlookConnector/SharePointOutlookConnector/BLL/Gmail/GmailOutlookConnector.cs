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
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.FileSystem;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.Gmail;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.SharePoint
{
    public class GmailOutlookConnector:IOutlookConnector
    {
        public ISPCFolder GetRootFolder(EUSiteSetting siteSetting)
        {
            return new GFolder(siteSetting, String.Empty, siteSetting.User, String.Empty);
        }
        public List<ISPCFolder> GetSubFolders(ISPCFolder folder)
        {
            return GMailManager.GetInstance().GetFolders(folder.SiteSetting, folder);
        }

        public List<ISPCView> GetViews(ISPCFolder folder)
        {
            List<ISPCView> views = new List<ISPCView>();
            GFolder _folder = (GFolder)folder;
            GView view = new GView(folder.SiteSetting, _folder.Path, _folder.Title);
            views.Add(view);
            return views;
        }

        public List<ISPCItem> GetListItems(ISPCFolder folder, ISPCView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, EUCamlFilters filters, out string listItemCollectionPositionNext, out int itemCount)
        {
            listItemCollectionPositionNext = String.Empty;
            itemCount = 0;
            GFolder _folder = folder as GFolder;
            return GMailManager.GetInstance().GetItems(folder.SiteSetting, folder);
        }

        public void BindItemsToListViewControl(ISPCFolder folder, ISPCView view, List<ISPCItem> items, DataGridView LibraryContentDataGridView)
        {
            for (int i = LibraryContentDataGridView.Columns.Count - 1; i > 1; i--)
            {
                LibraryContentDataGridView.Columns.RemoveAt(i);
            }
            LibraryContentDataGridView.Columns.Add("TitleColumn", "Title");
            LibraryContentDataGridView.Columns["TitleColumn"].Tag = "TitleColumn";

            LibraryContentDataGridView.Rows.Clear();
            foreach (ISPCItem item in items)
            {
                int newRowIndex = LibraryContentDataGridView.Rows.Add();
                DataGridViewRow newRow = LibraryContentDataGridView.Rows[newRowIndex];

                string fileName = item.Title;
                string extensionName = String.Empty;
                if (fileName.LastIndexOf(".") > 0)
                {
                    extensionName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
                }

                newRow.Cells["ExtensionImageColumn"].Value = ImageManager.GetInstance().GetExtensionImageFromResource(extensionName);
                newRow.Cells["FilePathColumn"].Value = item.URL;
                newRow.Cells["TitleColumn"].Value = item.Title;
                newRow.Tag = item;
            }
        }


        public void UploadFiles(ISPCFolder folder, List<EUEmailUploadFile> uploadFiles, List<EUField> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            GFolder _folder = folder as GFolder;
            foreach(EUEmailUploadFile emailUploadFile in uploadFiles)
            {
                GMailManager.GetInstance().UploadFile(folder.SiteSetting, emailUploadFile.FilePath, folder.UniqueIdentifier);
            }
        }
        public void DeleteListItem(ISPCItem item)
        {
            GItem fileItem = (GItem)item;
            GMailManager.GetInstance().DeleteFile(fileItem.SiteSetting, fileItem.UniqueIdentifier);
        }

        public bool CheckFileExistency(ISPCFolder folder, ISPCItem item, string newFileName)
        {
            GItem fileItem = (GItem)item;
            GFolder _folder = folder as GFolder;
            return GMailManager.GetInstance().CheckFileExistency(fileItem.SiteSetting, _folder.UniqueIdentifier, fileItem.Title);
        }

        public void CopyFile(ISPCFolder folder, ISPCItem item, string newFileName)
        {
            GFolder _folder = folder as GFolder;
//            GMailManager.GetInstance().UploadFile(folder.SiteSetting, emailUploadFile.FilePath, folder.UniqueIdentifier);
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
            throw new Exception("Not implemented");
        }

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

    }
}
