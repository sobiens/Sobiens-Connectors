using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using System.Collections;
using System.IO;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Workflows;

namespace Sobiens.Connectors.Common
{
    public abstract class ApplicationManager
    {
        public AppConfiguration Configuration = new AppConfiguration();
        public ISPCamlStudio SPCamlStudio { get; set; }
        public abstract IQueryPanel AddNewQueryPanel(Folder folder, ISiteSetting siteSetting);

        public delegate Dictionary<object, object> GetFieldMappings(string webURL, List<ApplicationItemProperty> properties, List<ContentType> contentTypes, FolderSettings folderSettings, FolderSetting defaultFolderSetting, ISiteSetting siteSetting, string rootFolder, out ContentType contentType,bool displayFileName);

        protected object Application { get; set; }
        public bool IsInitialized = false;

        public abstract ISiteSetting GetSiteSetting(Guid siteSettingID);
        public abstract string GetActiveFilePath();
        public abstract string EnsureSaved();
        public abstract void Initialize();
        public abstract void SetStatusBar(string text, int percentage);
        public abstract void RefreshControlsFromConfiguration();
        public abstract Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folderDefinition);
        public abstract Folder GetRootFolder(ISiteSetting siteSetting);
        public abstract Folder GetParentFolder(ISiteSetting siteSetting, Folder folder);
        public abstract List<Folder> GetSubFolders(ISiteSetting siteSetting, Folder folder, int[] includedFolderTypes);
        public abstract List<IView> GetViews(ISiteSetting siteSetting, Folder folder);
        public abstract IView GetView(ISiteSetting siteSetting, Folder folder);
        public abstract List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount);
        public abstract List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount);
        public abstract List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId);
        public abstract void BindFoldersToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataListView, object LibraryContentDataGridView);
        public abstract void BindItemsToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataListView, object LibraryContentDataGridView);
        public abstract void BindSearchResultsToListViewControl(ISiteSetting siteSetting, List<IItem> items, object LibraryContentDataGridView);
        public abstract void DeleteListItem(IItem item);
        public abstract bool CheckFileExistency(Folder folder, IItem item, string newFileName);
        public abstract void CopyFile(Folder folder, IItem item, string newFileName);
        public abstract bool CheckFolderExists(Folder folder, string newFolderName);
        public abstract FieldCollection GetFields(ISiteSetting siteSetting, Folder folder);
        public abstract string[] GetPrimaryKeys(ISiteSetting siteSetting, Folder folder);
        public abstract void CreateFields(ISiteSetting siteSetting, Folder folder, List<Field> fields);
        public abstract void DeleteUniquePermissions(ISiteSetting siteSetting, Folder folder, bool applyToAllSubItems);
        public abstract void Save();
        public abstract void ShowSyncDataWizard();
        public abstract void Load();
        public IConnectorMainView ConnectorExplorer { get; set; }
        public abstract List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder);


        public void MoveFile(ISiteSetting siteSetting, IItem item, Folder folder, string newFileName)
        {

        }

        public void UploadFile(ISiteSetting siteSetting, UploadItem uploadItem, IConnectorExplorer connectorExplorer, bool canUpdateItemInGrid, bool saveAsWord, UploadEventHandler UploadSucceeded, UploadEventHandler UploadFailed)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            bool isListItemAndAttachment = ConfigurationManager.GetInstance().GetListItemAndAttachmentOption();
            IItem item = null;
            bool isSuccess = serviceManager.UploadFile(siteSetting, uploadItem, isListItemAndAttachment, out item);
            UploadEventArgs e = new UploadEventArgs();
            e.UploadItem = uploadItem;
            e.UploadedItem = item;
            e.CanUpdateItemInGrid = canUpdateItemInGrid;
            e.ConnectorExplorer = connectorExplorer;



            if (isSuccess == true)
            {
                UploadSucceeded(null, e);
            }
            else
            {
                e.ErrorMessage = Languages.Translate("Upload failed");
                UploadFailed(null, e);
            }
        }

        public List<UploadItem> GetUploadItems(string webURL, ISiteSetting siteSetting, Folder destinationFolder, System.Windows.DragEventArgs e, GetFieldMappings getFieldMappings)
        {
            try
            {
                IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                //List<Field> fieldCollection = serviceManager.GetFields(destinationFolder).GetEditableFields();
                List<ContentType> contentTypes = serviceManager.GetContentTypes(siteSetting, destinationFolder, false);

                if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, false) == true)
                {
                    // Files are dragged from outside Outlook
                    List<UploadItem> uploadItems = new List<UploadItem>();
                    string[] fileNames = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                    // handle each file passed as needed
                    foreach (string fileName in fileNames)
                    {
                        Dictionary<object, object> fileFields = new Dictionary<object, object>();
                        FileInfo info = new FileInfo(fileName);
                        fileFields.Add("Name", info.Name);
                        fileFields.Add("CreationTime", info.CreationTime.ToLongDateString()); //TODO: Date formatting may be optional
                        fileFields.Add("CreationTimeUtc", info.CreationTimeUtc.ToLongDateString());
                        fileFields.Add("DirectoryName", info.DirectoryName);
                        fileFields.Add("Extension", info.Extension);
                        fileFields.Add("FullName", info.FullName);
                        fileFields.Add("LastWriteTime", info.LastWriteTime.ToLongDateString());
                        fileFields.Add("LastWriteTimeUtc", info.LastWriteTimeUtc.ToLongDateString());
                        fileFields.Add("Length", info.Length.ToString());

                        UploadItem uploadItem = new UploadItem();
                        uploadItem.UniqueID = Guid.NewGuid();
                        //uploadItem.ContentType = user defined //SharePointManager.GetContentTypes(folder.SiteSetting, folder.WebUrl, folder.ListName);
                        //uploadItem.FieldInformations = field -> value mapping
                        uploadItem.FieldInformations = new System.Collections.Generic.Dictionary<object, object>();
                        //                        //uploadItem.FieldInformations.Add(fieldCollection[0], fileName);
                        //                      //uploadItem.FieldInformations.Add(fieldCollection[1], fileName);
                        uploadItem.FilePath = fileName;
                        uploadItem.Folder = destinationFolder;
                        //uploadItem.Fields = not needed //SharePointManager.GetFields(dragedSPFolder.SiteSetting, dragedSPFolder.WebUrl, dragedSPFolder.ListName);
                        //uploadItem.UniqueID = Unique ID used in UI for delegate status update (uploading -> done)
                        uploadItems.Add(uploadItem);
                    }
                    return uploadItems;
                }
                else if (e.Data.GetDataPresent("FileGroupDescriptorW"))
                {
                    string[] fileFieldsData = null;
                    string[] fileFieldsKeys = null;
                    bool isApplicationFileDrop = e.Data.GetDataPresent(typeof(string));

                    if (isApplicationFileDrop)
                    {
                        // Application files are dragged
                        string[] separator = { "\r\n" };
                        fileFieldsData = ((string)e.Data.GetData(typeof(string))).Split(separator, StringSplitOptions.None);
                        fileFieldsKeys = fileFieldsData[0].Split('\t');
                    }

                    List<UploadItem> uploadItems = new List<UploadItem>();
                    string tempPath = Path.GetTempPath();

                    string[] filenames;
                    MemoryStream[] filestreams;
                    GetApplicationDragDropInformation(e.Data, out filenames, out filestreams);
                    List<ApplicationItemProperty> generalProperties = GetApplicationFields(null);
                    ContentType contentType;

                    FolderSettings folderSettings = ConfigurationManager.GetInstance().GetFolderSettings(ApplicationContext.Current.GetApplicationType()).GetRelatedFolderSettings(destinationFolder.GetUrl());
                    FolderSetting defaultFolderSetting = ConfigurationManager.GetInstance().GetFolderSettings(ApplicationContext.Current.GetApplicationType()).GetDefaultFolderSetting();

                    Dictionary<object, object> fieldMappings;
                    string initialFileName = tempPath + filenames[0];

                    if (Path.GetExtension(initialFileName) == ".msg")//this is a mail
                    fieldMappings = getFieldMappings(destinationFolder.GetWebUrl(), generalProperties, contentTypes, folderSettings, defaultFolderSetting, siteSetting, destinationFolder.GetUrl(), out contentType,false);
                    else//this an attachement
                        fieldMappings=getFieldMappings(destinationFolder.GetWebUrl(), null, contentTypes, folderSettings, defaultFolderSetting, siteSetting, destinationFolder.GetUrl(), out contentType,true);

                    if (fieldMappings == null || fieldMappings.Count == 0)
                        return null;

                    for (int fileIndex = 0; fileIndex < filenames.Length; fileIndex++)
                    {
                        //use the fileindex to get the name and data stream
                        string filename = tempPath + filenames[fileIndex];
                        MemoryStream filestream = filestreams[fileIndex];

                        //save the file stream using its name to the application path
                        FileStream outputStream = File.Create(filename);
                        filestream.WriteTo(outputStream);
                        outputStream.Close();

                        FileInfo tempFile = new FileInfo(filename);

                        // always good to make sure we actually created the file
                        if (tempFile.Exists == true)
                        {
                            Hashtable fileFields = new Hashtable();
                            if (isApplicationFileDrop)
                            {
                                // Application files are dragged
                                string[] fileFieldsValues = fileFieldsData[fileIndex + 1].Split('\t');
                                for (int i = 0; i < fileFieldsKeys.Count() - 1; i++)
                                {
                                    fileFields.Add(fileFieldsKeys[i], fileFieldsValues[i]);
                                }
                            }
                            else
                            {
                                //Application attachments are dragged
                                fileFields.Add("Name", filenames[fileIndex]);
                            }

                            UploadItem uploadItem = new UploadItem();
                            uploadItem.UniqueID = Guid.NewGuid();
                            uploadItem.FieldInformations = new System.Collections.Generic.Dictionary<object, object>();

                            if (Path.GetExtension(filename) == ".msg")
                            {//for message mapping needed
                                List<ApplicationItemProperty> properties = GetApplicationFields(filename);

                                foreach (Field field in fieldMappings.Keys)
                                {
                                    object obj = fieldMappings[field];
                                    object value = string.Empty;
                                    if (obj is ApplicationItemProperty)
                                    {
                                        value = properties.FirstOrDefault(p => p.Name == ((ApplicationItemProperty)obj).Name).Value;
                                    }
                                    else
                                    {
                                        value = obj;
                                    }
                                    uploadItem.FieldInformations.Add(field, value);
                                }
                            }
                            else
                            {//for single attachement file
                            uploadItem.FieldInformations=fieldMappings;
                            }

                            uploadItem.Folder = destinationFolder;
                            uploadItem.ContentType = contentType;
                            List<UploadItem> additionalItems = SetUploadItemFilePath(tempPath, filename, uploadItem);
                            //uploadItem.Fields = not needed //SharePointManager.GetFields(dragedSPFolder.SiteSetting, dragedSPFolder.WebUrl, dragedSPFolder.ListName);
                            //uploadItem.UniqueID = Unique ID used in UI for delegate status update (uploading -> done)
                            uploadItems.Add(uploadItem);

                            if (additionalItems != null)
                            {
                                foreach (UploadItem item in additionalItems)
                                {
                                    uploadItems.Add(item);
                                }
                            }

                            //tempFile.Delete(); TODO: cannot delete so soon, delete later
                        }
                        else
                        {
                            //Trace.WriteLine("File was not created!"); 
                        }
                    }
                    return uploadItems;
                }
            }
            catch (Exception ex)
            {
                int y = 3;
                //Trace.WriteLine("Error in DragDrop function: " + ex.Message);
                // don't use MessageBox here - Outlook or Explorer is waiting !
            }

            return null;
        }

        public List<UploadItem> GetUploadItems(string webURL, ISiteSetting siteSetting, Folder destinationFolder, string[] fileNames)
        {
            try
            {
                IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                //List<Field> fieldCollection = serviceManager.GetFields(destinationFolder).GetEditableFields();
                List<ContentType> contentTypes = serviceManager.GetContentTypes(siteSetting, destinationFolder, false);

                // Files are dragged from outside Outlook
                List<UploadItem> uploadItems = new List<UploadItem>();
                // handle each file passed as needed
                foreach (string fileName in fileNames)
                {
                    Dictionary<object, object> fileFields = new Dictionary<object, object>();
                    FileInfo info = new FileInfo(fileName);
                    fileFields.Add("Name", info.Name);
                    fileFields.Add("CreationTime", info.CreationTime.ToLongDateString()); //TODO: Date formatting may be optional
                    fileFields.Add("CreationTimeUtc", info.CreationTimeUtc.ToLongDateString());
                    fileFields.Add("DirectoryName", info.DirectoryName);
                    fileFields.Add("Extension", info.Extension);
                    fileFields.Add("FullName", info.FullName);
                    fileFields.Add("LastWriteTime", info.LastWriteTime.ToLongDateString());
                    fileFields.Add("LastWriteTimeUtc", info.LastWriteTimeUtc.ToLongDateString());
                    fileFields.Add("Length", info.Length.ToString());

                    UploadItem uploadItem = new UploadItem();
                    uploadItem.UniqueID = Guid.NewGuid();
                    uploadItem.FieldInformations = new System.Collections.Generic.Dictionary<object, object>();
                    uploadItem.FilePath = fileName;
                    uploadItem.Folder = destinationFolder;
                    uploadItems.Add(uploadItem);
                }
                return uploadItems;
            }
            catch (Exception ex)
            {
                //Trace.WriteLine("Error in DragDrop function: " + ex.Message);
                // don't use MessageBox here - Outlook or Explorer is waiting !
            }

            return null;
        }

        protected virtual List<UploadItem> SetUploadItemFilePath(string sourceFolder, string filePath, UploadItem uploadItem)
        {
            uploadItem.FilePath = filePath;
            return null;
        }

        public abstract void GetApplicationDragDropInformation(System.Windows.IDataObject data, out string[] filenames, out MemoryStream[] filestreams);

        public abstract List<ApplicationItemProperty> GetApplicationFields(string filePath);

        public abstract void ActivateDocument(object document);

        public abstract object OpenFile(ISiteSetting siteSetting, string filePath);

        public abstract void OpenNewFile(string templatePath);

        public abstract void CreateNewFile(string templatePath, string fileToBeSaved);

        public abstract void CloseActiveDocument();

        public abstract ApplicationTypes GetApplicationType();

        public abstract SC_MenuItems GetTaskMenuItems(ISiteSetting siteSetting, Task task);

        public abstract SC_MenuItems GetItemMenuItems(ISiteSetting siteSetting, IItem item);

        public abstract SC_MenuItems GetFolderMenuItems(ISiteSetting siteSetting, Folder folder);

        public abstract SC_MenuItems GetItemVersionMenuItems(ISiteSetting siteSetting, ItemVersion itemVersion);

        public abstract void DoMenuItemAction(ISiteSetting siteSetting, SC_MenuItemTypes menuItemType, object item, object[] args);

        public abstract void CheckInFile(ISiteSetting siteSetting, IItem item, string comment, CheckinTypes checkinType);

        public abstract void CheckOutFile(ISiteSetting siteSetting, IItem item);

        public abstract void UndoCheckOutFile(ISiteSetting siteSetting, IItem item);

        public abstract void AttachAsAHyperLink(ISiteSetting siteSetting, IItem item, object _inspector);

        public abstract void AttachAsAnAttachment(ISiteSetting siteSetting, IItem item, object _inspector);
    }
}
