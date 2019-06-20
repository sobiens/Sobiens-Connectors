using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.FileSystem;
using System.IO;
using System.Windows.Forms;
using EmailUploader.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.FileSystem
{
    public class FileSystemOutlookConnector : IOutlookConnector
    {
        public ISPCFolder GetRootFolder(EUSiteSetting siteSetting)
        {
            string[] pathArray = siteSetting.Url.Split(new string[] { "\\" }, StringSplitOptions.None);
            string folderName = pathArray[pathArray.Count()-1];
            return new FSFolder(siteSetting, siteSetting.Url, folderName, siteSetting.Url);
        }
        public List<ISPCFolder> GetSubFolders(ISPCFolder folder)
        {
            List<ISPCFolder> subFolders = new List<ISPCFolder>();
            FSFolder _folder = (FSFolder)folder;
            foreach (string folderPath in Directory.GetDirectories(_folder.Path))
            {
                string[] pathArray = folderPath.Split(new string[] { "\\" }, StringSplitOptions.None);
                string folderName = pathArray[pathArray.Count() - 1];

                FSFolder _folder1 = new FSFolder(folder.SiteSetting, folderPath, folderName, folderPath);
                subFolders.Add(_folder1);
            }
            return subFolders;
        }

        public List<ISPCView> GetViews(ISPCFolder folder)
        {
            List<ISPCView> views = new List<ISPCView>();
            FSFolder _folder = (FSFolder)folder;
            FSView view = new FSView(folder.SiteSetting, _folder.Path, _folder.Title);
            views.Add(view);
            return views;
        }

        public List<ISPCItem> GetListItems(ISPCFolder folder, ISPCView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, EUCamlFilters filters, out string listItemCollectionPositionNext, out int itemCount)
        {
            FSFolder _folder = (FSFolder)folder;
            listItemCollectionPositionNext = String.Empty;
            itemCount = Directory.GetFiles(_folder.Path).Count();
            List<ISPCItem> items = new List<ISPCItem>();
            foreach (string filePath in Directory.GetFiles(_folder.Path))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FSItem item = new FSItem(folder.SiteSetting, filePath, fileInfo.Name, filePath);
                items.Add(item);
            }
            return items;
        }
        public void BindItemsToListViewControl(ISPCFolder folder, ISPCView view, List<ISPCItem> items, DataGridView LibraryContentDataGridView)
        {
            for (int i = LibraryContentDataGridView.Columns.Count - 1; i > 1; i--)
            {
                LibraryContentDataGridView.Columns.RemoveAt(i);
            }
            LibraryContentDataGridView.Columns.Add("TitleColumn", "Name");
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
            FSFolder _folder = (FSFolder)folder;
            foreach (EUEmailUploadFile uploadFile in uploadFiles)
            {
                string copySource = new FileInfo(uploadFile.FilePath).Name;
                File.Copy(uploadFile.FilePath, _folder.Path + "\\" + copySource);
            }
        }

        public void DeleteListItem(ISPCItem item)
        {
            FSItem fileItem = (FSItem)item;
            File.Delete(fileItem.URL);
        }

        public bool CheckFileExistency(ISPCFolder folder, ISPCItem item, string newFileName)
        {
            FSFolder fsFolder = (FSFolder)folder;
            FSItem fsItem = (FSItem)item;
            return File.Exists(fsFolder.Path + "\\" + newFileName);
        }

        public void CopyFile(ISPCFolder folder, ISPCItem item, string newFileName)
        {
            FSFolder fsFolder = (FSFolder)folder;
            FSItem fsItem = (FSItem)item;
            File.Copy(fsItem.URL, fsFolder.Path + "\\" + newFileName);
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
