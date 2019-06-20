using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.FileSystem;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Common.FileSystem
{
    public class FileSystemOutlookConnector : IOutlookConnector
    {
        private ISiteSetting SiteSetting = null;

        public FileSystemOutlookConnector(ISiteSetting siteSetting)
        {
            this.SiteSetting = siteSetting;
        }

        public Folder GetRootFolder()
        {
            string[] pathArray = this.SiteSetting.Url.Split(new string[] { "\\" }, StringSplitOptions.None);
            string folderName = pathArray[pathArray.Count()-1];
            return new FSFolder(this.SiteSetting.ID, this.SiteSetting.Url, folderName, this.SiteSetting.Url);
        }
        public List<Folder> GetSubFolders(Folder folder)
        {
            List<Folder> subFolders = new List<Folder>();
            FSFolder _folder = (FSFolder)folder;
            foreach (string folderPath in Directory.GetDirectories(_folder.Path))
            {
                string[] pathArray = folderPath.Split(new string[] { "\\" }, StringSplitOptions.None);
                string folderName = pathArray[pathArray.Count() - 1];

                FSFolder _folder1 = new FSFolder(folder.SiteSettingID, folderPath, folderName, folderPath);
                subFolders.Add(_folder1);
            }
            return subFolders;
        }

        public List<IView> GetViews(Folder folder)
        {
            List<IView> views = new List<IView>();
            FSFolder _folder = (FSFolder)folder;
            FSView view = new FSView(folder.SiteSettingID, _folder.Path, _folder.Title);
            views.Add(view);
            return views;
        }

        public IView GetView(string webUrl, string listName, string viewName)
        {
            return null;
        }

        public List<IItem> GetListItems(Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            FSFolder _folder = (FSFolder)folder;
            listItemCollectionPositionNext = String.Empty;
            itemCount = Directory.GetFiles(_folder.Path).Count();
            List<IItem> items = new List<IItem>();
            foreach (string filePath in Directory.GetFiles(_folder.Path))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FSItem item = new FSItem(folder.SiteSettingID, filePath, fileInfo.Name, filePath);
                items.Add(item);
            }
            return items;
        }

        public void BindSearchResultsToListViewControl(List<IItem> items, object LibraryContentDataGridView) { }
        public void BindFoldersToListViewControl(Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataGridView) { }

        public void BindItemsToListViewControl(Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataGridView)
        {
            /*
            for (int i = LibraryContentDataGridView.Columns.Count - 1; i > 1; i--)
            {
                LibraryContentDataGridView.Columns.RemoveAt(i);
            }
            LibraryContentDataGridView.Columns.Add("TitleColumn", "Name");
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
             */ 
        }

        /*
        public void UploadFiles(IFolder folder, List<EUEmailUploadFile> uploadFiles, List<Field> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            FSFolder _folder = (FSFolder)folder;
            foreach (EUEmailUploadFile uploadFile in uploadFiles)
            {
                string copySource = new FileInfo(uploadFile.FilePath).Name;
                File.Copy(uploadFile.FilePath, _folder.Path + "\\" + copySource);
            }
        }
        */

        public void DeleteListItem(IItem item)
        {
            FSItem fileItem = (FSItem)item;
            File.Delete(fileItem.URL);
        }

        public bool CheckFileExistency(Folder folder, IItem item, string newFileName)
        {
            FSFolder fsFolder = (FSFolder)folder;
            FSItem fsItem = (FSItem)item;
            return File.Exists(fsFolder.Path + "\\" + newFileName);
        }

        public void CopyFile(Folder folder, IItem item, string newFileName)
        {
            FSFolder fsFolder = (FSFolder)folder;
            FSItem fsItem = (FSItem)item;
            File.Copy(fsItem.URL, fsFolder.Path + "\\" + newFileName);
        }

        public void MoveFile(IItem item,Folder folder, string newFileName)
        {
            FSFolder fsFolder = (FSFolder)folder;
            FSItem fsItem = (FSItem)item;
            File.Move(fsItem.URL, fsFolder.Path + "\\" + newFileName);
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

        /// <summary>
        /// Occurs when [upload succeeded].
        /// </summary>
        public event EventHandler UploadSucceeded;
    }
}
