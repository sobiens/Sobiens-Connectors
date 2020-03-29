using System;
using System.Collections.Generic;
using System.Linq;

using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using System.IO;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.WPF.Controls;
using Outlook = Microsoft.Office.Interop.Outlook;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.Workflows;
using Sobiens.Connectors.Entities.SQLServer;

namespace Sobiens.Connectors.OutlookConnector
{
    public class OutlookConnectorManager : ApplicationManager
    { 
        public OutlookConnectorManager(Microsoft.Office.Interop.Outlook.Application application, IConnectorMainView connectorExplorer)
        {
            this.Application = application;
            this.ConnectorExplorer = connectorExplorer;
            
        }

        public override void SetStatusBar(string text, int percentage)
        {
            this.ConnectorExplorer.StatusBar.SetStatusBar(text, percentage);
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
            List<string> _fileNames = new List<string>();
            List<MemoryStream> _filestreams = new List<MemoryStream>();

            string sourceFolder = ConfigurationManager.GetInstance().CreateATempFolder();
            Microsoft.Office.Interop.Outlook.Application application = Application as Microsoft.Office.Interop.Outlook.Application;
            for (int i = 1; i <= application.ActiveExplorer().Selection.Count; i++)
            {
                Object temp = application.ActiveExplorer().Selection[i];
                if (temp is Microsoft.Office.Interop.Outlook.MailItem)
                {
                    Microsoft.Office.Interop.Outlook.MailItem mailitem = (temp as Microsoft.Office.Interop.Outlook.MailItem);
                    string fileName = mailitem.Subject + ".msg";
                    string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

                    foreach (char c in invalid)
                    {
                        fileName = fileName.Replace(c.ToString(), "");
                    }
                    string filePath = sourceFolder + "\\" + fileName;
                    mailitem.SaveAs(filePath);
                    MemoryStream ms = new MemoryStream();
                    using (FileStream fs = File.OpenRead(filePath))
                    {
                        fs.CopyTo(ms);
                    }

                    _fileNames.Add(fileName);
                    _filestreams.Add(ms);
                }
            }
            filenames = _fileNames.ToArray();
            filestreams = _filestreams.ToArray();
            /*
            //wrap standard IDataObject in OutlookDataObject
            OutlookDataObject dataObject = new OutlookDataObject(data);

            try
            {
                //get the names and data streams of the files dropped
                filenames = (string[])dataObject.GetData("FileGroupDescriptorW");
                filestreams = (MemoryStream[])dataObject.GetData("FileContents");
            }
            catch (Exception ex)
            {
                string subject = ((string)dataObject.GetData("Text")).Split(new char[]{'\n'})[1].Split(new char[]{'\t'})[1];
                filenames = new string[]{subject};
                filestreams = new MemoryStream[] { (MemoryStream)dataObject.GetData("Object Descriptor") };
            }
             */ 
        }

        public override List<ApplicationItemProperty> GetApplicationFields(string filePath)
        {
            List<ApplicationItemProperty> returnApplicationItemProperties = new List<ApplicationItemProperty>();
            List<ApplicationItemProperty> applicationItemProperties = ConfigurationManager.GetInstance().GetApplicationItemProperties(ApplicationTypes.Outlook);
            Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.MailItem mailItem = null;
            if (filePath == null)
            {
                mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlook.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            }
            else
            {
                mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlook.CreateItemFromTemplate(filePath);
            }
            Microsoft.Office.Interop.Outlook.ItemProperties properties = mailItem.ItemProperties;

            for (int i = 0; i < properties.Count; i++)
            {
                Microsoft.Office.Interop.Outlook.ItemProperty property = properties[i];
                ApplicationItemProperty applicationItemProperty = applicationItemProperties.FirstOrDefault(item => item.Name == property.Name);
                if (applicationItemProperty != null)
                {
                    ApplicationItemProperty returnApplicationItemProperty = new ApplicationItemProperty(
                        applicationItemProperty.Name,
                        applicationItemProperty.DisplayName,
                        applicationItemProperty.Type);
                    returnApplicationItemProperty.Value = Convert.ChangeType(property.Value, applicationItemProperty.Type);
                    returnApplicationItemProperties.Add(returnApplicationItemProperty);
                }
            }
            #region Writing the xml for field definitions
            /*
                System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter("c:\\fields.xml", Encoding.UTF8);
                // start writing!
                writer.WriteStartDocument();
                writer.WriteStartElement("Outlook");
                writer.WriteStartElement("MailItem");
                writer.WriteStartElement("ItemProperties");
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                System.Xml.XmlNode mainNode = document.CreateNode(System.Xml.XmlNodeType.Element, "MailItem", "Outlook");
                for (int i = 0; i < properties.Count; i++)
                {
                    writer.WriteStartElement("Property");
                    writer.WriteAttributeString("Name", properties[i].Name);
                    writer.WriteAttributeString("DisplayName", properties[i].Name);
                    writer.WriteAttributeString("Type", properties[i].Type.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
                */
            #endregion

            return returnApplicationItemProperties;
        }

        protected override List<UploadItem> SetUploadItemFilePath(string sourceFolder, string filePath, UploadItem uploadItem)
        {
            if (ConfigurationManager.GetInstance().Configuration.OutlookConfigurations.SaveAsWord && Path.GetExtension(filePath)==".msg")
            {
                List<UploadItem> additionalItems = new List<UploadItem>();
                Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.MailItem mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlook.CreateItemFromTemplate(filePath);
                string newFileName = filePath.Substring(0, filePath.LastIndexOf('.')) + ".doc";
                mailItem.SaveAs(newFileName, Microsoft.Office.Interop.Outlook.OlSaveAsType.olDoc);
                foreach (Microsoft.Office.Interop.Outlook.Attachment attachment in mailItem.Attachments)
                {
                    string extensionName = String.Empty;
                    string filenameWithoutExtension = String.Empty;
                    string fileName = String.Empty;
                    GetFileNameAndExtension(attachment.FileName, out filenameWithoutExtension, out extensionName);
                    filePath = GetUnuqieFileName(sourceFolder, filenameWithoutExtension, extensionName, out fileName);//keep original name

                    attachment.SaveAsFile(filePath);
                    UploadItem attachmentItem = new UploadItem();
                    attachmentItem.Folder = uploadItem.Folder;
                    attachmentItem.ContentType = uploadItem.ContentType;
                    attachmentItem.FieldInformations = uploadItem.FieldInformations;
                    attachmentItem.FilePath = filePath;

                    additionalItems.Add(attachmentItem);
                }
                uploadItem.FilePath = newFileName;
                return additionalItems;
            }
            else
            {
                base.SetUploadItemFilePath(sourceFolder, filePath, uploadItem);
                return null;
            }
        }

        public override void ActivateDocument(object document)
        {
            throw new NotImplementedException();
        }

        public override object OpenFile(ISiteSetting siteSetting, string filePath)
        {
            throw new NotImplementedException();
        }

        public override void OpenNewFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public override void CloseActiveDocument()
        {
            throw new NotImplementedException();
        }

        public override void CreateNewFile(string templatePath, string fileToBeSaved)
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

        public override ApplicationTypes GetApplicationType()
        {
            return ApplicationTypes.Outlook;
        }

        public override void DoMenuItemAction(ISiteSetting siteSetting, SC_MenuItemTypes menuItemType, object item, object[] args)
        {
            bool reloadNeeded = false;
            switch (menuItemType)
            {
                case SC_MenuItemTypes.OpenFolder:
                    ItemsManager.OpenFolder(siteSetting, (Folder)item);
                    break;
                case SC_MenuItemTypes.OpenItem:
                    ItemsManager.OpenItem(siteSetting, (IItem)item);
                    break;
                case SC_MenuItemTypes.DeleteItem:
                    ItemsManager.DeleteItem(siteSetting, (IItem)item);
                    reloadNeeded = true;
                    break;
                case SC_MenuItemTypes.EditItem:
                    ItemsManager.EditItemProperties(siteSetting, item, (Folder) args[1]);
                    reloadNeeded = true;
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
                    reloadNeeded = ItemsManager.CheckInItem(siteSetting, (IItem)item, this.ConnectorExplorer.ConnectorExplorer);
                    break;
                case SC_MenuItemTypes.CheckOutItem:
                    reloadNeeded = ItemsManager.CheckOutItem(siteSetting, (IItem)item, this.ConnectorExplorer.ConnectorExplorer);
                    break;
                case SC_MenuItemTypes.UndoCheckOutItem:
                    reloadNeeded = ItemsManager.UndoCheckOutItem(siteSetting, (IItem)item, this.ConnectorExplorer.ConnectorExplorer);
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
                    ItemsManager.PasteItem(siteSetting, item, (Folder)args[1]);
                    reloadNeeded = true;
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
                    ItemsManager.AddFolder(siteSetting, item, "Nouveau");
                    this.ConnectorExplorer.ConnectorExplorer.ContentExplorer.reloadItemList();
                    break;
                case SC_MenuItemTypes.Inexplorer:
                    ItemsManager.displayFolder(siteSetting, item,true);
                    break;
                case SC_MenuItemTypes.Innavigator:
                    ItemsManager.displayFolder(siteSetting, item,false);
                    break;
            }
            if (reloadNeeded) this.ConnectorExplorer.ConnectorExplorer.ContentExplorer.reloadItemList();
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

        public override List<Folder> GetSubFolders(ISiteSetting siteSetting, Folder folder, int[] includedFolderTypes, string childFoldersCategoryName)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            return serviceManager.GetFolders(siteSetting, folder, includedFolderTypes, childFoldersCategoryName);
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

        public override void BindSearchResultsToListViewControl(ISiteSetting siteSetting, List<IItem> items, object libraryContentDataGridView)
        {
            ItemsManager.BindSearchResultsToListViewControl(siteSetting, items, libraryContentDataGridView);
        }

        public override void BindItemsToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataListView, object LibraryContentDataGridView)
        {
            ItemsManager.BindItemsToListViewControl(siteSetting, parentFolder, folder, view, folders, items, LibraryContentDataListView, LibraryContentDataGridView);
            
            /*
            DataGrid libraryContentDataGridView = (DataGrid)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataSet ds = libraryContentDataGridView.Tag as DataSet;
                if (ds == null)
                {
                    ds = this.GetBaseDataSet();
                }
                else
                {
                    RemoveAdditionalColumns(ds);
                }

                if (folders != null)
                {
                    RemoveFolders(ds);
                    AddFolders((SPBaseFolder)parentFolder, ds, folders);
                }

                if (items != null)
                {
                    RemoveItems(ds);
                    AddItems(ds, items);
                }

                this.SetBaseGridColumns(libraryContentDataGridView);
                libraryContentDataGridView.ItemsSource = ds.Tables[0].AsDataView();
            }));
             */ 
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
            Outlook.MailItem emailItem;
            if (_inspector != null)
            {
                Outlook.Inspector inspector = (Outlook.Inspector)_inspector;
                emailItem = inspector.CurrentItem as Outlook.MailItem;
            }
            else
            {
                // Create the Outlook application by using inline initialization.
                //Outlook.Application oApp = new Outlook.Application();
                //Create the new message by using the simplest approach.
                emailItem = (Outlook.MailItem)((Outlook.Application)this.Application).CreateItem(Outlook.OlItemType.olMailItem);
            }
            emailItem.Body = item.URL + Environment.NewLine + emailItem.Body;
            if (_inspector == null)
            {
                emailItem.Display(true);
            }
        }

        public override void AttachAsAnAttachment(ISiteSetting siteSetting, IItem item, object _inspector)
        {
            Outlook.MailItem emailItem;
            if (_inspector != null)
            {
                Outlook.Inspector inspector = (Outlook.Inspector)_inspector;
                emailItem = inspector.CurrentItem as Outlook.MailItem;
            }
            else
            {
                // Create the Outlook application by using inline initialization.
                //Outlook.Application oApp = new Outlook.Application();
                //Create the new message by using the simplest approach.
                emailItem = (Outlook.MailItem)((Outlook.Application)this.Application).CreateItem(Outlook.OlItemType.olMailItem);
            }
            emailItem.Attachments.Add(item.URL, Outlook.OlAttachmentType.olByValue, 1, item.Title);
            if (_inspector == null)
            {
                emailItem.Display(true);
            }
        }

        private static string GetUnuqieFileName(string sourceFolder, string fileNameWithoutExtension, string extensionName, out string fileName)
        {
            fileName = fileNameWithoutExtension.TrimEnd(new char[] { '.' });
            fileName = MakeFileNameSafe(fileName);
            if (fileName.Length > 75)
                fileName = fileName.Substring(0, 75);
            string filePath = Path.Combine(sourceFolder, fileName + "." + extensionName);
            int i = 1;
            while (File.Exists(filePath) == true)
            {
                filePath = Path.Combine(sourceFolder, fileName + " (" + i.ToString() + ")." + extensionName);
                i++;
            }
            return filePath;
        }

        /// <summary>
        /// Makes the file name safe.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private static string MakeFileNameSafe(string fileName)
        {
            fileName = fileName
                .Replace("/", "_")
                .Replace("\\", "_")
                .Replace(":", "_")
                .Replace("*", "_")
                .Replace("?", "_")
                .Replace("<", "_")
                .Replace(">", "_")
                .Replace("|", "_")
                .Replace("&", "_")
                .Replace("#", "_")
                .Replace("%", "_")
                .Replace("{", "_")
                .Replace("}", "_")
                .Replace("'", "_")
                ;
            return fileName;
        }

        public static void GetFileNameAndExtension(string fileFullPath, out string filenameWithoutExtension, out string fileExtension)
        {
            fileExtension = String.Empty;
            filenameWithoutExtension = String.Empty;
            if (fileFullPath.LastIndexOf(".") > -1)
            {
                fileExtension = fileFullPath.Substring(fileFullPath.LastIndexOf(".") + 1);
                filenameWithoutExtension = fileFullPath.Substring(0, fileFullPath.LastIndexOf("."));
            }
            else
            {
                filenameWithoutExtension = fileFullPath;
            }
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

        public override List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            if (folder as SPWeb != null)
                return serviceManager.GetContentTypes(siteSetting, folder, true);
            else if (folder as SPList != null)
                return serviceManager.GetContentTypes(siteSetting, folder.GetListName());
            else
                throw new NotImplementedException();
        }

        public override List<Workflow> GetWorkflows(ISiteSetting siteSetting, Folder folder)
        {
            throw new NotImplementedException();
        }

        public override List<CompareObjectsResult> GetObjectDifferences(ISiteSetting sourceSiteSetting, Folder sourceFolder, ISiteSetting destinationSiteSetting, Folder destinationFolder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType);
            return serviceManager.GetObjectDifferences(sourceSiteSetting, sourceFolder, destinationSiteSetting, destinationFolder);
        }

        public override void ApplyMissingCompareObjectsResult(CompareObjectsResult compareObjectsResult, ISiteSetting sourceSiteSetting, ISiteSetting destinationSiteSetting)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType);
            serviceManager.ApplyMissingCompareObjectsResult(compareObjectsResult, sourceSiteSetting, destinationSiteSetting);
        }

    }
}
