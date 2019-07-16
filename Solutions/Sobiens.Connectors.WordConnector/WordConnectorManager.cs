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
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.WPF.Controls;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;

namespace Sobiens.Connectors.WordConnector
{
    public class WordConnectorManager : ApplicationManager
    {
        private Microsoft.Office.Interop.Word.Application WordApplication
        {
            get
            {
                return (Microsoft.Office.Interop.Word.Application)this.Application;
            }
        }

        public WordConnectorManager(Word.Application application, IConnectorMainView connectorExplorer)
        {
            this.Application = application;
            this.ConnectorExplorer = connectorExplorer;
        }

        public override void Initialize()
        {
            this.IsInitialized = true;
            this.RefreshControlsFromConfiguration();
        }

        public override void SetStatusBar(string text, int percentage)
        {
            this.ConnectorExplorer.StatusBar.SetStatusBar(text, percentage);
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
            return ConfigurationManager.GetInstance().GetApplicationItemProperties(ApplicationTypes.Word);
        }

        public override string GetActiveFilePath()
        {
            return this.WordApplication.ActiveDocument.FullName;
        }

        public override string EnsureSaved()
        {
            if (string.IsNullOrEmpty(this.WordApplication.ActiveDocument.Path) == false)
            {
                return this.WordApplication.ActiveDocument.Path;
            }

            string folderPath = ConfigurationManager.GetInstance().CreateATempFolder();
            object fileName = folderPath + "\\wordconnector_" + Guid.NewGuid().ToString().Replace("-", string.Empty) + ".doc";
            object missing = Type.Missing;
            this.WordApplication.ActiveDocument.SaveAs2(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            return fileName.ToString();
        }

        public override void CloseActiveDocument()
        {
            this.WordApplication.ActiveDocument.Close();
        }

        public override void ActivateDocument(object document)
        {
            ((Word.Document)document).Activate();
        }

        public override object OpenFile(ISiteSetting siteSetting, string filePath)
        {
            if (siteSetting.UseClaimAuthentication == true)
            {
                string url = siteSetting.Url + "/_layouts/WordViewer.aspx?id=" + filePath.Replace(siteSetting.Url, string.Empty);
                //http://muratteknik.sharepoint.com/TeamSite/Functions/Tax/_layouts/WordViewer.aspx?id=/TeamSite/Functions/Tax/Weekly%20Reports/test/testttt.docx
                Process.Start("IEXPLORE.EXE", url);
                return null;
            }
            else
            {
                // set the file name from the open file dialog
                object fileName = filePath;
                object readOnly = false;
                object isVisible = true;
                object confirmConversations = true;
                // Here is the way to handle parameters you don't care about in .NET
                object missing = System.Reflection.Missing.Value;
                // Make word visible, so you can see what's happening
                // Open the document that was chosen by the dialog
                Word.Document doc = this.WordApplication.Documents.Open(ref fileName, ref confirmConversations, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible);
                // Activate the document so it shows up in front
                doc.Activate();
                return doc;
            }
        }

        public override void OpenNewFile(string templatePath)
        {
            // set the file name from the open file dialog
            object fileName = templatePath;
            object readOnly = false;
            object isVisible = true;
            // Here is the way to handle parameters you don't care about in .NET
            object missing = System.Reflection.Missing.Value;
            // Make word visible, so you can see what's happening
            // Open the document that was chosen by the dialog
            Word.Document doc = this.WordApplication.Documents.Add(ref fileName);
//            SetProperty(doc, "Path", "http://demo.sobiens.com/Sobiens.Connectors/Shared Documents");
//            doc.Path = "";
//                , ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible);
            // Activate the document so it shows up in front
            doc.Activate();
        }

        public override void CreateNewFile(string templatePath, string filePathToBeSaved)
        {
            object fileName = templatePath;
            object savefilePathLocation = filePathToBeSaved;
            object readOnly = false;
            object isVisible = false;
            // Here is the way to handle parameters you don't care about in .NET
            object missing = System.Reflection.Missing.Value;
            // Make word visible, so you can see what's happening
            // Open the document that was chosen by the dialog
            Word.Document doc = this.WordApplication.Documents.Add(ref fileName, missing, missing, ref isVisible);
            doc.SaveAs2(savefilePathLocation);
            doc.Close();
        }

        public override ApplicationTypes GetApplicationType()
        {
            return ApplicationTypes.Word;
        }

        public override void DoMenuItemAction(ISiteSetting siteSetting, SC_MenuItemTypes menuItemType, object item, object[] args)
        {
            switch (menuItemType)
            {
                case SC_MenuItemTypes.OpenFolder:
                    ItemsManager.OpenFolder(siteSetting, (Folder)item);
                    break;
                case SC_MenuItemTypes.OpenItem:
                    ItemsManager.OpenItem(siteSetting, (IItem)item);
                    break;
                case SC_MenuItemTypes.EditItem:
                    ItemsManager.EditItemProperties(siteSetting, (IItem)item, (Folder)args[1]);
                    break;
                case SC_MenuItemTypes.ShowItemVersionHistory:
                    ItemsManager.ShowVersionHistory(siteSetting, (IItem)item);
                    break;
                case SC_MenuItemTypes.EditItemPropertyMappings:
                    FoldersManager.EditItemPropertyMappings(siteSetting, (Folder)item);
                    break;
                case SC_MenuItemTypes.OpenVersionHistory:
                    ItemsManager.OpenVersionHistory(siteSetting, (ItemVersion)item);
                    break;
                case SC_MenuItemTypes.RollbackVersionHistory:
                    ItemsManager.RollbackVersion(siteSetting, (ItemVersion)item);
                    break;
                case SC_MenuItemTypes.CheckInItem:
                    ItemsManager.CheckInItem(siteSetting, (IItem)item, this.ConnectorExplorer.ConnectorExplorer);
                    break;
                case SC_MenuItemTypes.CheckOutItem:
                    ItemsManager.CheckOutItem(siteSetting, (IItem)item, this.ConnectorExplorer.ConnectorExplorer);
                    break;
                case SC_MenuItemTypes.UndoCheckOutItem:
                    ItemsManager.UndoCheckOutItem(siteSetting, (IItem)item, this.ConnectorExplorer.ConnectorExplorer);
                    break;
                case SC_MenuItemTypes.ApproveRejectItem:
                    ItemsManager.ApproveRejectItem(siteSetting, (IItem)item);
                    break;
                case SC_MenuItemTypes.CopyItem:
                    ItemsManager.SetCopiedItemInfo(siteSetting, (IItem)item,false);
                    break;
                case SC_MenuItemTypes.Cut:
                    ItemsManager.SetCopiedItemInfo(siteSetting, (IItem)item, true);
                    break;
                case SC_MenuItemTypes.PasteItem:
                    ItemsManager.PasteItem(siteSetting, item, (Folder)args[1]);;
                    break;
                case SC_MenuItemTypes.AttachAsAHyperlink:
                    ApplicationContext.Current.AttachAsAHyperLink(siteSetting, (IItem)item, args[0]);
                    break;
                case SC_MenuItemTypes.AttachAsAnAttachment:
                    ApplicationContext.Current.AttachAsAnAttachment(siteSetting, (IItem)item, args[0]);
                    break;
                case SC_MenuItemTypes.Workflow:
                    ItemsManager.OpenWorkflowDialog(siteSetting, (Folder)args[1], (IItem)item);
                    break;
                case SC_MenuItemTypes.EditTask:
                    ItemsManager.EditTask(siteSetting, (Sobiens.Connectors.Entities.Workflows.Task)item);
                    break;
                case SC_MenuItemTypes.OpenTaskDocument:
                    ItemsManager.OpenTaskDocument(siteSetting, (Sobiens.Connectors.Entities.Workflows.Task)item);
                    break;
                case SC_MenuItemTypes.AddFolder:
                    ItemsManager.AddFolder(siteSetting, (IItem)item, "Nouveau");
                    break;
                case SC_MenuItemTypes.Inexplorer:
                    ItemsManager.displayFolder(siteSetting,item,true);
                    break;
                case SC_MenuItemTypes.Innavigator:
                    ItemsManager.displayFolder(siteSetting,item,false);
                    break;
            }
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
            /*
            EUListItem listItem = (EUListItem)item;
            SharePointManager.DeleteListItem(listItem.SiteSetting, listItem.WebURL, listItem.ListName, listItem.URL, listItem.ID);
             */
        }

        /// <summary>
        /// Checks the folder exists.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        public override bool CheckFolderExists(Folder folder, string newFolderName)
        {
            return false;
            /*
            SPFolder listFolder = (SPFolder)folder;
            return SharePointManager.CheckFolderExists(listFolder.SiteSetting, listFolder.WebUrl, listFolder.ListName, listFolder.FolderPath, newFolderName);
             */
        }

        public override bool CheckFileExistency(Folder folder, IItem item, string newFileName)
        {
            return false;
            /*
            SPFolder listFolder = (SPFolder)folder;
            return SharePointManager.CheckFileExistency(listFolder.SiteSetting, listFolder.WebUrl, listFolder.ListName, listFolder.FolderPath, null, newFileName);
             */
        }

        public override void CopyFile(Folder folder, IItem item, string newFileName)
        {
            return;
            /*
            EUListItem listItem = (EUListItem)item;
            SPFolder listFolder = (SPFolder)folder;

            string folderPath = listFolder.WebUrl + "/" + listFolder.FolderPath;
            string webUrl = listFolder.WebUrl;
            string listName = listFolder.ListName;

            SharePointCopyWS.CopyResult[] myCopyResultArray = null;
            SharePointManager.CopyFile(listFolder.SiteSetting, webUrl, listItem.URL, folderPath + "/" + newFileName, out myCopyResultArray);
             */
        }
        //public override void MoveFile(IItem item, Folder folder, string newFileName)
        //{
        //    return;
        //}
        public override Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folderDefinition)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetFolder(siteSetting, folderDefinition);
        }
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
            return serviceManager.GetListItems(siteSetting, folder, view, sortField, isAsc, currentPageIndex, currentListItemCollectionPositionNext, filters, isRecursive, out listItemCollectionPositionNext, out itemCount);
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
            // Create the Outlook application by using inline initialization.
            Outlook.Application oApp = new Outlook.Application();
            //Create the new message by using the simplest approach.
            Outlook.MailItem emailItem = (Outlook.MailItem)((Outlook.Application)this.Application).CreateItem(Outlook.OlItemType.olMailItem);
            emailItem.Body = item.URL + Environment.NewLine + emailItem.Body;
            emailItem.Display(true);
        }

        public override void AttachAsAnAttachment(ISiteSetting siteSetting, IItem item, object _inspector)
        {
            // Create the Outlook application by using inline initialization.
            Outlook.Application oApp = new Outlook.Application();
            //Create the new message by using the simplest approach.
            Outlook.MailItem emailItem = (Outlook.MailItem)((Outlook.Application)this.Application).CreateItem(Outlook.OlItemType.olMailItem);
            emailItem.Attachments.Add(item.URL, Outlook.OlAttachmentType.olByValue, 1, item.Title);
            emailItem.Display(true);
        }

        public override IQueryPanel AddNewQueryPanel(Folder folder, ISiteSetting siteSetting)
        {
            throw new NotImplementedException();
        }

        public override ISiteSetting GetSiteSetting(Guid siteSettingID)
        {
            throw new NotImplementedException();
        }

        public override List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            throw new NotImplementedException();
        }

        public override FieldCollection GetFields(ISiteSetting siteSetting, Folder folder)
        {
            throw new NotImplementedException();
        }

        public override string[] GetPrimaryKeys(ISiteSetting siteSetting, Folder folder)
        {
            throw new NotImplementedException();
        }

        public override void DeleteUniquePermissions(ISiteSetting siteSetting, Folder folder, bool applyToAllSubItems)
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void ShowSyncDataWizard()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override void CreateFields(ISiteSetting siteSetting, Folder folder, List<Field> fields)
        {
            throw new NotImplementedException();
        }

        public override List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetAuditLogs(siteSetting, listName, itemId);
        }
    }
}
