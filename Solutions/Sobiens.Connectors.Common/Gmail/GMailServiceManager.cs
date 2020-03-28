#if General
using System;
using System.Collections.Generic;
using Google.GData.Documents;
using Google.GData.Client;
using System.IO;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Gmail;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Workflows;
using System.Net;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;

namespace Sobiens.Connectors.Common
{
    public class GMailServiceManager : IServiceManager
    {
        private bool loggedIn = false;
        private DocumentsService service = null;

        private static GMailServiceManager _instance = null;
        public static GMailServiceManager GetInstance()
        {
            if (_instance == null)
                _instance = new GMailServiceManager();
            return _instance;
        }

        public bool CheckConnection(ISiteSetting siteSetting)
        {
            Login(siteSetting.Username, siteSetting.Password);
            return loggedIn;
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

        public Folder GetFoldersTreeFromURL(ISiteSetting siteSetting, string url)
        {
            return null;
        }

        public void OpenFile(ISiteSetting siteSetting, IItem item)
        {
        }

        public void DeleteFile(ISiteSetting siteSetting, IItem item)
        {
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

        public uint AddFolder(ISiteSetting siteSetting, string webUrl,string folderName,string folderPath, string args)
        {
            throw new NotImplementedException();
        }

        public bool UploadFile(ISiteSetting siteSetting, UploadItem uploadItem, bool saveAsWord, out IItem listItem) { listItem = null; return true; }

        public FieldCollection GetFields(ISiteSetting siteSetting, Folder folder) { return null; }
        public void CreateFields(ISiteSetting siteSetting, Folder folder, List<Field> fields)
        {

        }

        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder, bool includeReadOnly) { return null; }

        public ContentType GetContentType(ISiteSetting siteSetting, Folder folder, string contentTypeID, bool includeReadOnly) { return null; }


        /// <summary>
        /// Authenticates to Google servers
        /// </summary>
        /// <param name="username">The user's username (e-mail)</param>
        /// <param name="password">The user's password</param>
        /// <exception cref="AuthenticationException">Thrown on invalid credentials.</exception>
        public void Login(string username, string password)
        {
            try
            {
                if (service == null)
                {
                    service = new DocumentsService("DocListUploader");
                    ((GDataRequestFactory)service.RequestFactory).KeepAlive = false;
                    service.setUserCredentials(username, password);
                    //force the service to authenticate
                    DocumentsListQuery query = new DocumentsListQuery();
                    query.NumberToRetrieve = 1;
                    service.Query(query);
                    loggedIn = true;
                }
            }
            catch (AuthenticationException e)
            {
                loggedIn = false;
                service = null;
                throw e;
            }
        }

        public Folder GetParentFolder(ISiteSetting siteSetting, Folder folder)
        {
            throw new Exception("Not implemented yet");
        }
        public List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId)
        {
            throw new NotImplementedException();
        }
        public List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            listItemCollectionPositionNext = String.Empty;
            itemCount = 0;
            GFolder _folder = folder as GFolder;
            string webUrl = folder.GetUrl();
            //return this.GetListItems(siteSetting, folder.GetUrl(), string.Empty, false);

            Login(siteSetting.Username, siteSetting.Password);
            List<IItem> items = new List<IItem>();

            DocumentsListQuery query = new DocumentsListQuery();
            if (string.IsNullOrEmpty(webUrl) == false)
                query = new FolderQuery(webUrl);
            DocumentsFeed feed = service.Query(query);
            foreach (DocumentEntry entry in feed.Entries)
            {
                if (entry.IsFolder == false)
                {
                    if (String.IsNullOrEmpty(webUrl) == true)
                    {
                        if (entry.ParentFolders.Count > 0)
                            continue;
                    }
                    IItem item = new GItem(siteSetting.ID, entry.ResourceId, entry.Title.Text, entry.AlternateUri.ToString());
                    item.Properties.Add("ows_Editor", entry.LastModified.Name);
                    item.Properties.Add("ows_Modified", entry.Edited.DateValue.ToString());
                    items.Add(item);
                }
            }
            return items;

        }

        public Folder GetFolderByBasicFolderDefinition(ISiteSetting siteSetting, BasicFolderDefinition basicFolderDefinition, bool returnAll)
        {
            throw new Exception("Not implemented yet");
        }

        public bool DownloadAdministrativeConfiguration(ISiteSetting siteSetting, string url, string savePath)
        {
            throw new Exception("Not implemented yet");
        }

        public IItem GetItem(ISiteSetting siteSetting, string itemUrl, out Sobiens.Connectors.Entities.Folder itemFolder)
        {
            throw new Exception("Not implemented yet");
        }

        public Folder GetWorkflowFolder(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder itemFolder, string itemUrl)
        {
            throw new Exception("Not implemented yet");
        }

        public Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folderDefinition)
        {
            throw new Exception("Not implemented yet");
        }

        //public List<IItem> GetListItems(ISiteSetting siteSetting, string webUrl, string listName, bool isRecursive)
        //{
        //    Login(siteSetting.Username, siteSetting.Password);
        //    List<IItem> items = new List<IItem>();

        //    DocumentsListQuery query = new DocumentsListQuery();
        //    if (string.IsNullOrEmpty(webUrl) ==false)
        //        query = new FolderQuery(webUrl);
        //    DocumentsFeed feed = service.Query(query);
        //    foreach (DocumentEntry entry in feed.Entries)
        //    {
        //        if (entry.IsFolder == false)
        //        {
        //            if (String.IsNullOrEmpty(webUrl) == true)
        //            {
        //                if (entry.ParentFolders.Count > 0)
        //                    continue;
        //            }
        //            IItem item = new GItem(siteSetting.ID, entry.ResourceId, entry.Title.Text, entry.AlternateUri.ToString());
        //            item.Properties.Add("ows_Editor", entry.LastModified.Name);
        //            item.Properties.Add("ows_Modified", entry.Edited.DateValue.ToString());
        //            items.Add(item);
        //        }
        //    }
        //    return items;
        //}


        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder parentFolder, int[] includedFolderTypes, string childFoldersCategoryName)
        {
            return this.GetFolders(siteSetting, parentFolder, null, childFoldersCategoryName);
        }

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder parentFolder)
        {
            Login(siteSetting.Username, siteSetting.Password);
            List<Folder> folders = new List<Folder>();
            DocumentsListQuery query = new DocumentsListQuery();
            if (parentFolder.UniqueIdentifier != String.Empty)
                query = new FolderQuery(parentFolder.UniqueIdentifier);
            query.ShowFolders = true;
            DocumentsFeed feed = service.Query(query);
            foreach (DocumentEntry entry in feed.Entries)
            {
                if (entry.IsFolder)
                {
                    if (parentFolder == null || parentFolder.UniqueIdentifier == String.Empty)
                    {
                        if (entry.ParentFolders.Count > 0)
                            continue;
                    }
                    Folder folder = new GFolder(siteSetting.ID, entry.ResourceId, entry.Title.Text, entry.Id.AbsoluteUri);
                    folder.Selected = true;
                    folders.Add(folder);
                }
            }
            return folders;
        }

        public void UploadFile(ISiteSetting siteSetting, string filePath, string parentResourceId)
        {
            Login(siteSetting.Username, siteSetting.Password);
            FileInfo fileInfo = new FileInfo(filePath);
            string fileName = fileInfo.Name;
            string fileExtension = fileInfo.Extension.ToUpper().Substring(1);
            fileName = fileName + "." + fileExtension;
            String contentType = (String)DocumentsService.DocumentTypes[fileExtension];
            string feed = string.Format(DocumentsListQuery.foldersUriTemplate, parentResourceId);

            FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var result = service.Insert(new Uri(feed), stream, contentType, fileName);
        }

        public void DeleteFile(ISiteSetting siteSetting, string fileResourceId)
        {
            Login(siteSetting.Username, siteSetting.Password);
            AtomEntry entry = service.Get(DocumentsListQuery.documentsBaseUri + "/" + fileResourceId);
            entry.Delete();
        }
        public bool CheckFileExistency(ISiteSetting siteSetting, string folderResourceId, string fileName)
        {
//            Login(siteSetting.Username, siteSetting.Password);
//            AtomEntry entry = service.Get(DocumentsListQuery.documentsBaseUri + "/" + fileResourceId);
//            if (entry == null)
                return false;
//            return true;
        }


        public List<ItemVersion> GetListItemVersions(ISiteSetting siteSetting, IItem item)
        {
            throw new Exception("Not implemented yet");
        }

        public List<ItemVersion> RestoreVersion(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            throw new Exception("Not implemented yet");
        }

        public Result CheckInFile(ISiteSetting siteSetting, IItem item, string comment, CheckinTypes checkinType)
        {
            throw new Exception("Not implemented yet");
        }

        public Result CheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            throw new Exception("Not implemented yet");
        }

        public Result UndoCheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            throw new Exception("Not implemented yet");
        }

        public bool CheckItemCanBeCopied(ISiteSetting siteSetting, Folder targetFolder, IItem copyItem, string fileName)
        {
            throw new Exception("Not implemented yet");
        }

        public Result CopyItem(ISiteSetting siteSetting, Folder targetFolder, IItem copySource, string newFileName)
        {
            throw new Exception("Not implemented yet");
        }

        public string GetProperty(Dictionary<string, string> properies, string key)
        {
            string actualKey = "ows_" + key;
            return properies.ContainsKey(actualKey) ? properies[actualKey] : null;
        }

        public Folder GetRootFolder(ISiteSetting siteSetting)
        {
            return new GFolder(siteSetting.ID, String.Empty, siteSetting.Username, String.Empty);
        }

        public List<IView> GetViews(ISiteSetting siteSetting, Folder folder)
        {
            List<IView> views = new List<IView>();
            GFolder gFolder = folder as GFolder;
            GView view = new GView(siteSetting.ID, gFolder.Path, gFolder.Title);
            views.Add(view);
            return views;
        }

        public void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, int listItemID, System.Collections.Generic.Dictionary<object, object> fields)
        {
            throw new Exception("Not implemented yet");
        }

        public bool UploadFile(ISiteSetting siteSetting, UploadItem uploadItem, bool saveAsWord)
        {
            throw new Exception("Not implemented yet");
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            throw new NotImplementedException();
        }
        public List<IItem> GetListItemsWithoutPaging(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName)
        {
            throw new NotImplementedException();
        }

        public void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, int listItemID, Dictionary<object, object> fields, Dictionary<string, object> auditInformation)
        {
            throw new NotImplementedException();
        }

        public void CreateListItem(ISiteSetting siteSetting, string webUrl, string listName, Dictionary<object, object> fields)
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

        public void DownloadFile(ISiteSetting siteSetting, string url, string savePath)
        {
            throw new NotImplementedException();
        }

        public string[] GetPrimaryKeys(ISiteSetting siteSetting, Folder folder)
        {
            throw new NotImplementedException();
        }
        public SQLForeignKey[] GetForeignKeys(ISiteSetting siteSetting, Folder folder)
        {
            throw new Exception("Not implemented yet.");
        }
        public List<SPTermGroup> GetTermGroups(ISiteSetting siteSetting)
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
        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, string listName)
        {
            throw new NotImplementedException();
        }
        public List<Workflow> GetWorkflows(ISiteSetting siteSetting, string listName)
        {
            return null;
        }

        public Folder CreateFolder(ISiteSetting siteSetting, string title, int templateType)
        {
            throw new NotImplementedException();
        }
        public List<CompareObjectsResult> GetObjectDifferences(ISiteSetting sourceSiteSetting, Folder sourceObject, ISiteSetting destinationSiteSetting, Folder destinationObject)
        {
            List<CompareObjectsResult> compareObjectsResults = new List<CompareObjectsResult>();

            return compareObjectsResults;
        }


        private bool CheckIfEquals(ISiteSetting sourceSiteSetting, Folder sourceObject, ISiteSetting destinationSiteSetting, Folder destinationObject)
        {
            return true;
        }
        public void ApplyMissingCompareObjectsResult(CompareObjectsResult compareObjectsResult, ISiteSetting sourceSiteSetting, ISiteSetting destinationSiteSetting) { }

    }
}
#endif
