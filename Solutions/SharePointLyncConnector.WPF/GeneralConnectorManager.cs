using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Interfaces;
using System.Collections;
using Sobiens.Connectors.Entities;
using System.IO;
using Sobiens.Connectors.Common;
using System.Reflection;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.WPF.Controls;

namespace Sobiens.Connectors.WordConnector
{
    public class GeneralConnectorManager : ApplicationManager
    {

        public GeneralConnectorManager(IConnectorMainView connectorExplorer)
        {
            this.ConnectorExplorer = connectorExplorer;
        }

        public override void SetStatusBar(string text, int percentage)
        {
            
        }

        public override void CloseActiveDocument()
        {
            throw new NotImplementedException();
        }

        public override string EnsureSaved()
        {
            throw new NotImplementedException();
        }

        public override string GetActiveFilePath()
        {
            throw new NotImplementedException();
        }

        public override void ActivateDocument(object document)
        {
            throw new NotImplementedException();
        }

        public override object OpenFile(ISiteSetting siteSetting, string filePath)
        {
            throw new NotImplementedException();
        }

        protected override List<UploadItem> SetUploadItemFilePath(string sourceFolder, string filePath, UploadItem uploadItem)
        {
            return base.SetUploadItemFilePath(sourceFolder, filePath, uploadItem);
        }

        public override void Initialize()
        {
            this.IsInitialized = true;
            this.RefreshControlsFromConfiguration();
        }

        public override void RefreshControlsFromConfiguration()
        {
            this.ConnectorExplorer.RefreshControls();
        }

        public override void GetApplicationDragDropInformation(System.Windows.IDataObject data, out string[] filenames, out MemoryStream[] filestreams)
        {
            ////wrap standard IDataObject in OutlookDataObject
            //OutlookDataObject dataObject = new OutlookDataObject(data);

            ////get the names and data streams of the files dropped
            //filenames = (string[])dataObject.GetData("FileGroupDescriptorW");
            //filestreams = (MemoryStream[])dataObject.GetData("FileContents");
            filenames = null;
            filestreams = null;
        }

        public override List<ApplicationItemProperty> GetApplicationFields(string filePath)
        {
            throw new NotImplementedException();
        }

        public override void OpenNewFile(string templatePath)
        {
            throw new NotImplementedException();
        }

        public override void CreateNewFile(string templatePath, string filePathToBeSaved)
        {
            throw new NotImplementedException();
        }

        public override ApplicationTypes GetApplicationType()
        {
            return ApplicationTypes.General;
        }

        public override void DoMenuItemAction(ISiteSetting siteSetting, SC_MenuItemTypes menuItemType, object item, object[] args)
        {
        }

        public override SC_MenuItems GetTaskMenuItems(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Workflows.Task task)
        {
            return ItemsManager.GetTaskMenuItems(siteSetting, task);
        }

        public override SC_MenuItems GetItemMenuItems(ISiteSetting siteSetting, IItem item)
        {
            return ItemsManager.GetItemMenuItems(siteSetting, item);
        }

        public override SC_MenuItems GetFolderMenuItems(ISiteSetting siteSetting, Folder folder)
        {
            return FoldersManager.GetFolderMenuItems(siteSetting, folder);
        }
        
        public override SC_MenuItems GetItemVersionMenuItems(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            return ItemsManager.GetItemVersionMenuItems(siteSetting, itemVersion);
        }

        public override void DeleteListItem(IItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks the folder exists.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        public override bool CheckFolderExists(Folder folder, string newFolderName)
        {
            throw new NotImplementedException();
        }

        public override bool CheckFileExistency(Folder folder, IItem item, string newFileName)
        {
            throw new NotImplementedException();
        }

        public override void CopyFile(Folder folder, IItem item, string newFileName)
        {
            throw new NotImplementedException();
        }
        //public override void MoveFile(IItem item, Folder folder,string newFileName)
        //{
        //    throw new NotImplementedException();
        //}
        public override Folder GetRootFolder(ISiteSetting siteSetting)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetRootFolder(siteSetting);
        }

        public override Folder GetParentFolder(ISiteSetting siteSetting, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetParentFolder(siteSetting, folder);
        }

        public override List<Folder> GetSubFolders(ISiteSetting siteSetting, Folder folder, int[] includedFolderTypes)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetFolders(siteSetting, folder, includedFolderTypes);
        }

        public override List<IView> GetViews(ISiteSetting siteSetting, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetViews(siteSetting, folder);
        }

        public override IView GetView(ISiteSetting siteSetting, Folder folder)
        {
            return null;
        }

        public override List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetListItems(siteSetting, folder, view, sortField, isAsc, currentPageIndex, currentListItemCollectionPositionNext, filters, true, out listItemCollectionPositionNext, out itemCount);
        }
        public override void BindFoldersToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataListView, object LibraryContentDataGridView) 
        {
            ItemsManager.BindFoldersToListViewControl(siteSetting, parentFolder, folder, folders, LibraryContentDataListView, LibraryContentDataGridView);
        }

        public override void BindItemsToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataListView, object LibraryContentDataGridView) 
        {
            ItemsManager.BindItemsToListViewControl(siteSetting, parentFolder, folder, view, folders, items, LibraryContentDataListView, LibraryContentDataGridView);
        }
        public override void BindSearchResultsToListViewControl(ISiteSetting siteSetting, List<IItem> items, object libraryContentDataGridView) 
        {
            ItemsManager.BindSearchResultsToListViewControl(siteSetting, items, libraryContentDataGridView);
        }
        public override void CheckInFile(ISiteSetting siteSetting, IItem item, string comment, CheckinTypes checkinType)
        {
            ItemsManager.CheckInItem(siteSetting, item, this.ConnectorExplorer.ConnectorExplorer);
        }

        public override void CheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            ItemsManager.CheckOutItem(siteSetting, item, this.ConnectorExplorer.ConnectorExplorer);
        }

        public override void UndoCheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            ItemsManager.UndoCheckOutItem(siteSetting, item, this.ConnectorExplorer.ConnectorExplorer);
        }

        public override void AttachAsAHyperLink(ISiteSetting siteSetting, IItem item, object _inspector)
        {
            throw new NotImplementedException();
        }

        public override void AttachAsAnAttachment(ISiteSetting siteSetting, IItem item, object _inspector)
        {
            throw new NotImplementedException();
        }
    }
}
