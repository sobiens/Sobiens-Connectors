using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SkyDrive;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Common.SkyDrive
{
    public class SkyDriveOutlookConnector : IOutlookConnector
    {
        private ISiteSetting SiteSetting = null;
        public SkyDriveOutlookConnector(ISiteSetting siteSetting)
        {
            this.SiteSetting = siteSetting;
        }

        public Folder GetRootFolder()
        {
            SDFolder rootFolder = new SDFolder(this.SiteSetting.ID, this.SiteSetting.Username, this.SiteSetting.Username);
            return rootFolder;
        }
        public List<Folder> GetSubFolders(Folder folder)
        {
            throw new Exception("Not implemented");
        }
        public List<IView> GetViews(Folder folder)
        {
            throw new Exception("Not implemented");
        }

        public IView GetView(string webUrl, string listName, string viewName)
        {
            return null;
        }

        public List<IItem> GetListItems(Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            throw new Exception("Not implemented");
        }

        public void BindSearchResultsToListViewControl(List<IItem> items, object LibraryContentDataGridView) { }
        public void BindFoldersToListViewControl(Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataGridView) { }

        public void BindItemsToListViewControl(Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataGridView)
        {
            //throw new Exception("Not implemented");
        }

        /*
        public void UploadFiles(IFolder folder, List<EUEmailUploadFile> uploadFiles, List<Field> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            throw new Exception("Not implemented");
        }
         */
        public void DeleteListItem(IItem item)
        {
            throw new Exception("Not implemented");
        }
        public bool CheckFileExistency(Folder folder, IItem item, string newFileName)
        {
            throw new Exception("Not implemented");
        }
        public void CopyFile(Folder folder, IItem item, string newFileName)
        {
            throw new Exception("Not implemented");
        }
        public void MoveFile(IItem item, Folder folder, string newFileName)
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
    }
}
