using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Interfaces;
using System.Collections;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Services.SharePoint;
using System.IO;
using Sobiens.Connectors.Entities.Settings;
using System.Data;
using System.Diagnostics;
using Sobiens.Connectors.Entities.FileSystem;
using Sobiens.Connectors.Entities.Workflows;
using System.Net;
using Sobiens.Connectors.Entities.SQLServer;

namespace Sobiens.Connectors.Common.SharePoint
{
    public class FileSystemServiceManager : IServiceManager
    {
        private static FileSystemServiceManager _instance = null;
        public static FileSystemServiceManager GetInstance()
        {
            if (_instance == null)
                _instance = new FileSystemServiceManager();
            return _instance;
        }

        public bool CheckConnection(ISiteSetting siteSetting)
        {
            DirectoryInfo di = new DirectoryInfo(siteSetting.Url);
            return di.Exists == true ? true : false;
        }
        public uint AddFolder(ISiteSetting siteSetting, string webUrl, string folderName, string folderPath, string args)
        {
            throw new NotImplementedException();
        }
        public string GetTermsByLabel(ISiteSetting siteSetting, string webUrl, string label, int lcid, int resultCollectionSize, string termIds, bool addIfNotFound)
        {
            throw new NotImplementedException();
        }

        public string GetKeywordTermsByGuids(ISiteSetting siteSetting, string webUrl, int lcid, string termIds)
        {
            throw new NotImplementedException();
        }

        public SPTermSet GetTermSet(ISiteSetting siteSetting, Guid termSetId)
        {
            throw new NotImplementedException();
        }

        public CookieContainer GetCookieContainer(string url, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void StartWorkflow(ISiteSetting siteSetting, IItem item, WorkflowTemplate workflow)
        {
            throw new NotImplementedException();
        }

        public WorkflowData GetWorkflowDataForItem(ISiteSetting siteSetting, IItem item)
        {
            throw new NotImplementedException();
        }

        public TemplateData GetTemplatesForItem(ISiteSetting siteSetting, IItem item)
        {
            throw new NotImplementedException();
        }

        private List<Folder> GetSubFoldersByBasicFolderDefinition(ISiteSetting siteSetting, Folder folder, BasicFolderDefinition basicFolderDefinition, bool returnAll)
        {
            if (basicFolderDefinition.Folders.Count > 0)
            {
                List<Folder> subFolders = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetFolders(siteSetting, folder);
                foreach (Folder subFolder in subFolders)
                {
                    BasicFolderDefinition subBasicFolderDefinition = basicFolderDefinition.Folders.Single(t => t.FolderUrl == subFolder.GetUrl());
                    if (basicFolderDefinition != null)
                    {
                        subFolder.Selected = true;
                    }

                    if (returnAll == true || basicFolderDefinition != null)
                    {
                        folder.Folders.Add(subFolder);
                    }

                    subFolder.Folders = GetSubFoldersByBasicFolderDefinition(siteSetting, subFolder, subBasicFolderDefinition, returnAll);
                }

                return subFolders;
            }

            return new List<Folder>();
        }

        public Folder GetFolderByBasicFolderDefinition(ISiteSetting siteSetting, BasicFolderDefinition basicFolderDefinition, bool returnAll)
        {
            Folder folder = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetFolder(siteSetting, basicFolderDefinition);
            folder.Selected = true;

            folder.Folders = GetSubFoldersByBasicFolderDefinition(siteSetting, folder, basicFolderDefinition, returnAll);

            return folder;
        }

        public bool DownloadAdministrativeConfiguration(ISiteSetting siteSetting, string url, string savePath)
        {
            string startString = "BEGINSOBIENSXML";
            string endString = "ENDSOBIENSXML";

            string tempFile = ConfigurationManager.GetInstance().GetTempFolder() + "\\extconfig_" + Guid.NewGuid().ToString().Replace("-", "") + ".xml";
            new SharePointService().DownLoadFile(siteSetting, url, tempFile);
            string content = File.ReadAllText(tempFile);
            int startIndex = content.IndexOf(startString) + startString.Length;
            int endIndex = content.IndexOf(endString);
            if (startIndex > -1 && endIndex > -1)
            {
                string xmlContent = content.Substring(startIndex, endIndex - startIndex);
                File.WriteAllText(savePath, xmlContent);
            }
            return true;
        }

        public void OpenFile(ISiteSetting siteSetting, IItem item)
        {
            Process.Start("IExplore.exe", item.URL);
        }
        public void DownloadFile(ISiteSetting siteSetting, string url, string savePath)
        {
            new SharePointService().DownLoadFile(siteSetting, url, savePath);
        }
        public void DeleteFile(ISiteSetting siteSetting, IItem item)
        {
            FSItem fileItem = (FSItem)item;
            File.Delete(fileItem.URL);
        }

        private void FillFoldersTreeFromURL(ISiteSetting siteSetting, SPBaseFolder parentFolder, string remainingUrl)
        {
            parentFolder.Folders = this.GetFolders(siteSetting, parentFolder);
            SPBaseFolder currentFolder = null;

            string currentFolderName;

            if (remainingUrl.IndexOf('/') > -1)
            {
                currentFolderName = remainingUrl.Substring(0, remainingUrl.IndexOf('/'));
                remainingUrl = remainingUrl.Substring(remainingUrl.IndexOf(currentFolderName) + currentFolderName.Length + 1);
            }
            else
            {
                return;
                //currentFolderName = remainingUrl;
                //remainingUrl = string.Empty;
            }

            foreach (SPBaseFolder tempFolder in parentFolder.Folders)
            {
                string folderName = tempFolder.GetPath();
                if (folderName.LastIndexOf("/") > -1)
                {
                    folderName = folderName.Substring(folderName.LastIndexOf("/") + 1);
                }

                if (folderName.Equals(currentFolderName) == true)
                {
                    currentFolder = tempFolder;
                    break;
                }
            }

            if (currentFolder != null)
            {
                FillFoldersTreeFromURL(siteSetting, currentFolder, remainingUrl);
            }
        }

        public Folder GetFoldersTreeFromURL(ISiteSetting siteSetting, string url)
        {
            //string remainingURL = url.Substring(url.IndexOf(siteSetting.Url) + siteSetting.Url.Length + 1);

            SPBaseFolder topFolder = new SharePointOutlookConnector(siteSetting).GetRootFolder() as SPBaseFolder;
            string remainingUrl = url.absoluteTorelative(siteSetting.Url);
            //string remainingUrl = string.Empty;
            //if (url.Equals(siteSetting.Url, StringComparison.InvariantCultureIgnoreCase) == true)
            //{
            //    remainingUrl = string.Empty;
            //}
            //else
            //{
            //    remainingUrl = url.Substring(url.IndexOf(siteSetting.Url) + siteSetting.Url.Length + 1);
            //}

            FillFoldersTreeFromURL(siteSetting, topFolder, remainingUrl);
            //Folder currentFolder = topFolder;
            /*
            while (string.IsNullOrEmpty(remainingURL) == false)
            {
                if (remainingURL.IndexOf('/') > -1)
                {
                    string currentFolderName = remainingURL.Substring(0, remainingURL.IndexOf('/'));
                    remainingURL = url.Substring(url.IndexOf(currentFolderName) + currentFolderName.Length + 1);
                }
                else
                {
                    currentFolder.Folders = new SharePointOutlookConnector(siteSetting).GetSubFolders(topFolder);
                    remainingURL = string.Empty;
                }
            }
             */
            return topFolder;
        }

        public bool UploadFile(ISiteSetting siteSetting, UploadItem uploadItem, bool saveAsWord, out IItem listItem)
        {
            listItem = null;
            SPFolder spFolder = uploadItem.Folder as SPFolder;
            string rootFolderPath = spFolder.RootFolderPath;
            string siteURL = spFolder.SiteUrl;
            string webURL = spFolder.WebUrl;
            string destinationFolderUrl = spFolder.GetUrl(); // spFolder.WebUrl.TrimEnd(new char[] { '/' }) + "/" + spFolder.FolderPath.TrimStart(new char[] { '/' });
            string listName = spFolder.ListName;

            byte[] myByteArray = SharePointServiceManager.ReadByteArrayFromFile(uploadItem.FilePath);
            bool uploadSucceeded = false;

            string newDestinationUrl = destinationFolderUrl + "/";
            string copySource = uploadItem.FilePath;
            string filename = string.Empty;
            KeyValuePair<string, object> title = uploadItem.FieldInformations.Where(f => f.Key == "Title").FirstOrDefault();
            if (title.Value != null)
            {
                filename = title.Value.ToString() + new FileInfo(copySource).Extension;
            }
            else
            {
                filename = new FileInfo(copySource).Name;
            }
            string[] copyDest = new string[1] { destinationFolderUrl + "/" + filename };
            byte[] itemByteArray = SharePointServiceManager.ReadByteArrayFromFile(uploadItem.FilePath);

            SPListItem spListItem;

            string newFileName = copySource;

            if (spFolder.IsDocumentLibrary)
            {
                uint? result = SharePointService.UploadFile(siteSetting, listName, rootFolderPath, siteURL, webURL, copySource, copyDest, myByteArray, uploadItem.FieldInformations, uploadItem.ContentType, out spListItem);
                if (result.HasValue && spListItem != null)
                    uploadSucceeded = true;
            }
            else
            {
                int? result = SharePointService.UploadListItemWithAttachment(siteSetting, listName, rootFolderPath, uploadItem, webURL);
                uploadSucceeded = result.HasValue;
            }

            return uploadSucceeded;
        }

        public static byte[] ReadByteArrayFromFile(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

        public FieldCollection GetFields(ISiteSetting siteSetting, Folder folder)
        {
            SPFolder spFolder = folder as SPFolder;
            return new SharePointService().GetFields(siteSetting, spFolder.WebUrl, spFolder.ListName);
        }

        public void CreateFields(ISiteSetting siteSetting, Folder folder, List<Field> fields)
        {

        }

        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder, bool includeReadOnly)
        {
            SPFolder spFolder = folder as SPFolder;
            return new SharePointService().GetContentTypes(siteSetting, spFolder.WebUrl, spFolder.RootFolderPath, spFolder.ListName, includeReadOnly);
        }

        public ContentType GetContentType(ISiteSetting siteSetting, Folder folder, string contentTypeID, bool includeReadOnly)
        {
            SPFolder spFolder = folder as SPFolder;
            return SharePointService.GetContentType(siteSetting, spFolder.WebUrl, spFolder.RootFolderPath, spFolder.ListName, contentTypeID, includeReadOnly);
        }

        public List<IView> GetViews(ISiteSetting siteSetting, Folder folder)
        {
            List<IView> views = new List<IView>();
            FSFolder _folder = (FSFolder)folder;
            FSView view = new FSView(folder.SiteSettingID, _folder.Path, _folder.Title);
            views.Add(view);
            return views;
        }
        public List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId)
        {
            throw new NotImplementedException();
        }
        public List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            FSFolder _folder = (FSFolder)folder;
            listItemCollectionPositionNext = String.Empty;
            itemCount = Directory.GetFiles(_folder.Path).Count();
            List<IItem> items = new List<IItem>();
            foreach (string filePath in Directory.GetFiles(_folder.Path))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FSItem item = new FSItem(folder.SiteSettingID, fileInfo);
                items.Add(item);
            }
            return items;
        }

        //public List<IItem> GetListItems(ISiteSetting siteSetting, string webUrl, string listName, bool isRecursive)
        //{
        //    ISharePointService spService = new SharePointService();
        //    string next;
        //    int count;
        //    return spService.GetListItems(siteSetting, null, String.Empty, true, false, webUrl, listName, null, String.Empty, null, isRecursive, out next, out count); 
        //}

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder folder, int[] includedFolderTypes, string childFoldersCategoryName)
        {
            throw new NotImplementedException();
        }

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder folder)
        {
            List<Folder> folders = new List<Folder>();
            FSFolder fsFolder = (FSFolder)folder;
            foreach (DirectoryInfo di in fsFolder.GetDirectoryInfo().GetDirectories())
            {
                folders.Add(new FSFolder(siteSetting.ID, di));
            }
            return folders;
        }

        public Folder GetRootFolder(ISiteSetting siteSetting)
        {
            DirectoryInfo di = new DirectoryInfo(siteSetting.Url);
            return new FSFolder(siteSetting.ID, di);
        }

        public Folder GetParentFolder(ISiteSetting siteSetting, Folder folder)
        {
            FSFolder _folder = (FSFolder)folder;
            DirectoryInfo di = new DirectoryInfo(_folder.Path);
            if (di.Parent == null)
            {
                return new FSFolder(siteSetting.ID, di);
            }

            return null;
        }

        public IItem GetItem(ISiteSetting siteSetting, string itemUrl, out Sobiens.Connectors.Entities.Folder itemFolder)
        {
            FileInfo fi = new FileInfo(itemUrl);
            itemFolder = new FSFolder(siteSetting.ID, fi.Directory);
            return new FSItem(siteSetting.ID, fi);
        }

        public Folder GetWorkflowFolder(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder itemFolder, string itemUrl)
        {
            throw new Exception("Not implemented yet");
        }

        public Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folderDefinition)
        {
            DirectoryInfo di = new DirectoryInfo(folderDefinition.FolderUrl);
            return new FSFolder(siteSetting.ID, folderDefinition.FolderUrl, folderDefinition.Title, folderDefinition.FolderUrl);
        }

        public List<ItemVersion> GetListItemVersions(ISiteSetting siteSetting, IItem item)
        {
            string webUrl = ((SPListItem)item).WebURL;
            string fileURL = ((SPListItem)item).URL;
            ISharePointService spService = new SharePointService();
            return spService.GetListItemVersions(siteSetting, webUrl, fileURL);
        }

        public List<ItemVersion> RestoreVersion(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            ISharePointService spService = new SharePointService();
            return spService.RestoreVersion(siteSetting, itemVersion);
        }

        public Result CheckInFile(ISiteSetting siteSetting, IItem item, string comment, CheckinTypes checkinType)
        {
            SPListItem spLListItem = (SPListItem)item;
            ISharePointService spService = new SharePointService();
            return spService.CheckInFile(siteSetting, spLListItem.WebURL, spLListItem.URL, comment, checkinType);
        }

        public Result CheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            SPListItem spLListItem = (SPListItem)item;
            ISharePointService spService = new SharePointService();
            return spService.CheckOutFile(siteSetting, spLListItem.WebURL, spLListItem.URL);
        }

        public Result UndoCheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            SPListItem spLListItem = (SPListItem)item;
            ISharePointService spService = new SharePointService();
            return spService.UndoCheckOutFile(siteSetting, spLListItem.WebURL, spLListItem.URL);
        }

        public bool CheckItemCanBeCopied(ISiteSetting siteSetting, Folder targetFolder, IItem copyItem, string fileName)
        {
            SPFolder spFolder = (SPFolder)targetFolder;
            // list item does not anything unique like document has unique file name
            if (copyItem as SPListItem == null)
                return true;
            
            SPDocument spDocument = (SPDocument)copyItem;
            ISharePointService spService = new SharePointService();
            return spService.CheckFileExistency(siteSetting, spFolder.WebUrl, spFolder.ListName, spFolder.FolderPath, null, fileName);
        }

        public Result CopyItem(ISiteSetting siteSetting, Folder targetFolder, IItem copySource, string newFileName)
        {
            SPFolder spFolder = (SPFolder)targetFolder;
            SPListItem spListItem = (SPListItem)copySource;
            ISharePointService spService = new SharePointService();
            return spService.CopyFile(siteSetting, spFolder.WebUrl, spFolder.ListName, spFolder.FolderPath + "/" + newFileName);
        }

        public string GetProperty(Dictionary<string, string> properies, string key)
        {
            string actualKey = "ows_" + key;
            return properies.ContainsKey(actualKey) ? properies[actualKey] : null;
        }

        public void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, string listItemID, System.Collections.Generic.Dictionary<string, object> fields)
        {
            Hashtable changedProperties = SharePointService.getChangedProperties(null, fields);
            SharePointService.UpdateListItem(siteSetting, webUrl, listName, int.Parse(listItemID), changedProperties,true);
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            throw new NotImplementedException();
        }
        public List<IItem> GetListItemsWithoutPaging(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName)
        {
            throw new NotImplementedException();
        }


        public void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, string listItemID, Dictionary<string, object> fields, Dictionary<string, object> auditInformation)
        {
            throw new NotImplementedException();
        }

        public void CreateListItem(ISiteSetting siteSetting, string webUrl, string listName, Dictionary<string, object> fields)
        {
            throw new NotImplementedException();
        }

        public string GetUser(ISiteSetting siteSetting, string UserName)
        {
            throw new NotImplementedException();
        }

        public void DeleteUniquePermissions(ISiteSetting siteSetting, Folder folder, bool applyToAllSubItems)
        {
            throw new NotImplementedException();
        }


        public List<SPTermGroup> GetTermGroups(ISiteSetting siteSetting)
        {
            throw new NotImplementedException();
        }

        public List<SPTermSet> GetTermSets(ISiteSetting siteSetting, Guid termGroupId)
        {
            throw new NotImplementedException();
        }

        public List<SPTermSet> GetGroupTermSets(ISiteSetting siteSetting, Guid termGroupId)
        {
            throw new NotImplementedException();
        }

        public List<SPTerm> GetTerms(ISiteSetting siteSetting, Guid termSetId)
        {
            throw new NotImplementedException();
        }
        public List<SPTerm> GetTermTerms(ISiteSetting siteSetting, Guid termId)
        {
            throw new NotImplementedException();
        }
        public SPTermSet CreateTermSet(ISiteSetting siteSetting, SPTermSet termSet)
        {
            throw new NotImplementedException();
        }
        public SPTermGroup CreateTermGroup(ISiteSetting siteSetting, SPTermGroup termGroup)
        {
            throw new NotImplementedException();
        }
        public SPTerm CreateTerm(ISiteSetting siteSetting, SPTerm term)
        {
            throw new NotImplementedException();
        }
        public SPTermStore GetTermStore(ISiteSetting siteSetting)
        {
            throw new NotImplementedException();
        }
        public List<ContentType> GetContentTypes(ISiteSetting siteSetting)
        {
            throw new NotImplementedException();
        }
        public List<Workflow> GetWorkflows(ISiteSetting siteSetting, string listName)
        {
            return null;
        }

        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, string listName)
        {
            throw new NotImplementedException();
        }
        public Folder CreateFolder(ISiteSetting siteSetting, string title, int templateType)
        {
            throw new NotImplementedException();
        }
        public List<CompareObjectsResult> GetObjectDifferences(ISiteSetting sourceSiteSetting, Folder sourceObject, ISiteSetting destinationSiteSetting, Folder destinationObject, Action<int, string> reportProgressAction)
        {
            List<CompareObjectsResult> compareObjectsResults = new List<CompareObjectsResult>();

            return compareObjectsResults;
        }


        private bool CheckIfEquals(ISiteSetting sourceSiteSetting, Folder sourceObject, ISiteSetting destinationSiteSetting, Folder destinationObject)
        {
            return true;
        }
        public void ApplyMissingCompareObjectsResult(CompareObjectsResult compareObjectsResult, ISiteSetting sourceSiteSetting, ISiteSetting destinationSiteSetting) { }

        public bool ValidateImportValue(ISiteSetting siteSetting, Field field, string value, Dictionary<string, string> parameters, out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }

        public object ConvertImportValueToFieldValue(ISiteSetting siteSetting, Field field, string value, Dictionary<string, string> parameters)
        {
            return value;
        }


    }
}
