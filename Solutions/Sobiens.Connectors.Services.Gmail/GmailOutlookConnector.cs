using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Gmail;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Services.Gmail
{
    /*
    public class GmailOutlookConnector:IOutlookConnector
    {
        private ISiteSetting SiteSetting = null;

        public GmailOutlookConnector(ISiteSetting siteSetting)
        {
            this.SiteSetting = siteSetting;
        }

        public Folder GetRootFolder()
        {
            return new GFolder(this.SiteSetting.ID, String.Empty, this.SiteSetting.Username, String.Empty);
        }
        public List<Folder> GetSubFolders(Folder folder)
        {
            return GMailServiceManager.GetInstance().GetFolders(this.SiteSetting, folder);
        }

        public List<IView> GetViews(Folder folder)
        {
            List<IView> views = new List<IView>();
            GFolder _folder = (GFolder)folder;
            GView view = new GView(folder.SiteSettingID, _folder.Path, _folder.Title);
            views.Add(view);
            return views;
        }
        public IView GetView(string webUrl, string listName, string viewName)
        {
            return null;
        }

        public List<IItem> GetListItems(Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, out string listItemCollectionPositionNext, out int itemCount)
        {
            listItemCollectionPositionNext = String.Empty;
            itemCount = 0;
            GFolder _folder = folder as GFolder;
            return GMailServiceManager.GetInstance().GetItems(this.SiteSetting, folder);
        }

        public void BindSearchResultsToListViewControl(List<IItem> items, object LibraryContentDataGridView) { }
        public void BindFoldersToListViewControl(Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataGridView) { }

        public void BindItemsToListViewControl(Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataGridView)
        {
            for (int i = LibraryContentDataGridView.Columns.Count - 1; i > 1; i--)
            {
                LibraryContentDataGridView.Columns.RemoveAt(i);
            }
            LibraryContentDataGridView.Columns.Add("TitleColumn", "Title");
            LibraryContentDataGridView.Columns["TitleColumn"].Tag = "TitleColumn";

            LibraryContentDataGridView.Rows.Clear();
            foreach (IItem item in items)
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

        public void UploadFiles(Folder folder, List<EUEmailUploadFile> uploadFiles, List<Field> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            GFolder _folder = folder as GFolder;
            foreach(EUEmailUploadFile emailUploadFile in uploadFiles)
            {
                GMailServiceManager.GetInstance().UploadFile(folder.SiteSetting, emailUploadFile.FilePath, folder.UniqueIdentifier);
            }
        }

        public void DeleteListItem(IItem item)
        {
            GItem fileItem = (GItem)item;
            GMailServiceManager.GetInstance().DeleteFile(this.SiteSetting, fileItem.UniqueIdentifier);
        }

        public bool CheckFileExistency(Folder folder, IItem item, string newFileName)
        {
            GItem fileItem = (GItem)item;
            GFolder _folder = folder as GFolder;
            return GMailServiceManager.GetInstance().CheckFileExistency(this.SiteSetting, _folder.UniqueIdentifier, fileItem.Title);
        }

        public void CopyFile(Folder folder, IItem item, string newFileName)
        {
            GFolder _folder = folder as GFolder;
            //            GMailServiceManager.GetInstance().UploadFile(folder.SiteSetting, emailUploadFile.FilePath, folder.UniqueIdentifier);
        }

        /// <summary>
        /// Checks the folder exists.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        public bool CheckFolderExists(Folder folder, string newFolderName)
        {
            throw new Exception("Not implemented");
        }

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
     */ 
}
