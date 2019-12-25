using System;
using System.Collections.Generic;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.SharePoint;
using System.IO;
using System.Diagnostics;
using Sobiens.Connectors.Entities.Workflows;
using System.Net;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Services.CRM;
using System.Linq;

namespace Sobiens.Connectors.Common.CRM
{
    public class CRMServiceManager : IServiceManager
    {
        private static CRMServiceManager _instance = null;
        public static CRMServiceManager GetInstance()
        {
            if (_instance == null)
                _instance = new CRMServiceManager();
            return _instance;
        }

        public bool CheckConnection(ISiteSetting siteSetting)
        {
            return true;
            /*
            CRMService spService = new CRMService();
            SPWeb web = null;
            try
            {
                web = spService.GetWeb(siteSetting, siteSetting.Url);
            }
            catch
            {
            }
            return web == null ? false : true;
            */
        }

        public string GetTermsByLabel(ISiteSetting siteSetting, string webUrl, string label, int lcid, int resultCollectionSize, string termIds, bool addIfNotFound)
        {
            return string.Empty;
            //return new CRMService().GetTermsByLabel(siteSetting, webUrl, label, lcid, resultCollectionSize, termIds, addIfNotFound);
        }

        public string GetKeywordTermsByGuids(ISiteSetting siteSetting, string webUrl, int lcid, string termIds)
        {
            return string.Empty;
            //return new CRMService().GetKeywordTermsByGuids(siteSetting, webUrl, lcid, termIds);
        }

        public SPTermSet GetTermSet(ISiteSetting siteSetting, Guid termSetId)
        {
            return new SPTermSet();
            //return new CRMService().GetTermSets(siteSetting, webUrl, lcid, sspIds, termIds);
        }

        private List<Folder> GetSubFoldersByBasicFolderDefinition(ISiteSetting siteSetting, Folder folder, BasicFolderDefinition basicFolderDefinition, bool returnAll)
        {
            /*
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
            */
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
        {/*
            string startString = "BEGINSOBIENSXML";
            string endString = "ENDSOBIENSXML";

            string tempFile = ConfigurationManager.GetInstance().GetTempFolder() + "\\extconfig_" + Guid.NewGuid().ToString().Replace("-", "") + ".xml";
            new CRMService().DownLoadFile(siteSetting, url, tempFile);
            string content = File.ReadAllText(tempFile);
            int startIndex = content.IndexOf(startString) + startString.Length;
            int endIndex = content.IndexOf(endString);
            if (startIndex > -1 && endIndex > -1)
            {
                string xmlContent = content.Substring(startIndex, endIndex - startIndex);
                File.WriteAllText(savePath, xmlContent);
            }
            */
            return true;
        }

        public void DownloadFile(ISiteSetting siteSetting, string url, string savePath)
        {
            //new CRMService().DownLoadFile(siteSetting, url, savePath);
        }

        public void OpenFile(ISiteSetting siteSetting, IItem item)
        {
            Process.Start("IExplore.exe", item.URL);
        }

        public void DeleteFile(ISiteSetting siteSetting, IItem item)
        {
            SPListItem listItem = (SPListItem)item;
            //CRMService.DeleteListItem((SiteSetting)siteSetting, listItem.WebURL, listItem.ListName, listItem.URL, listItem.ID);
        }

        public CookieContainer GetCookieContainer(string url, string userName, string password)
        {
            return null;
            //return CRMService.GetCookieContainer(url, userName, password);
        }

        public void StartWorkflow(ISiteSetting siteSetting, IItem item, WorkflowTemplate workflow)
        {
            SPListItem listItem = (SPListItem)item;
            //CRMService.StartWorkflow(siteSetting, listItem.WebURL, listItem.URL, workflow);
        }

        public WorkflowData GetWorkflowDataForItem(ISiteSetting siteSetting, IItem item)
        {
            SPListItem listItem = (SPListItem)item;
            return null;
            //return CRMService.GetWorkflowDataForItem((SiteSetting)siteSetting, listItem.WebURL, listItem.URL);
        }

        public TemplateData GetTemplatesForItem(ISiteSetting siteSetting, IItem item)
        {
            SPListItem listItem = (SPListItem)item;
            return null;
            //return CRMService.GetTemplatesForItem((SiteSetting)siteSetting, listItem.WebURL, listItem.URL);
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

                if (folderName.Equals(currentFolderName, StringComparison.OrdinalIgnoreCase) == true)
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

            //SPBaseFolder topFolder = new SharePointOutlookConnector(siteSetting).GetRootFolder() as SPBaseFolder;
            //string remainingUrl = url.absoluteTorelative(siteSetting.Url);
            //string remainingUrl = string.Empty;
            //if (url.Equals(siteSetting.Url, StringComparison.InvariantCultureIgnoreCase) == true)
            //{
            //    remainingUrl = string.Empty;
            //}
            //else
            //{
            //    remainingUrl = url.Substring(url.IndexOf(siteSetting.Url,StringComparison.InvariantCultureIgnoreCase) + siteSetting.Url.Length + 1);
            //}

            //FillFoldersTreeFromURL(siteSetting, topFolder, remainingUrl);
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
            return null;
        }

        private string FixSharePointFileName(string fileName)
        {
            return fileName.Replace('#', '_').Replace('%', '_').Replace('&', '_').Replace('*', '_').Replace(':', '_').Replace('<', '_').Replace('>', '_').Replace('?', '_').Replace('/', '_').Replace('{', '_').Replace('|', '_').Replace('}', '_');
        }

        public bool UploadFile(ISiteSetting siteSetting, UploadItem uploadItem, bool saveAsWord, out IItem listItem)
        {
            listItem = null;
            return true;
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

        public string[] GetPrimaryKeys(ISiteSetting siteSetting, Folder folder)
        {
            throw new Exception("Not implemented yet.");
        }

        public FieldCollection GetFields(ISiteSetting siteSetting, Folder folder)
        {
            return (new CRMService()).GetFields(siteSetting, folder.GetListName());

            //SPFolder spFolder = folder as SPFolder;
            //return null;
        }

        public void CreateFields(ISiteSetting siteSetting, Folder folder, List<Field> fields)
        {

        }


        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder, bool includeReadOnly)
        {
            SPFolder spFolder = folder as SPFolder;
            return null;
        }

        public ContentType GetContentType(ISiteSetting siteSetting, Folder folder, string contentTypeID, bool includeReadOnly)
        {
            SPFolder spFolder = folder as SPFolder;
            return null;
        }

        public List<Workflow> GetWorkflows(ISiteSetting siteSetting, string listName)
        {
            return null;
        }


        public List<IView> GetViews(ISiteSetting siteSetting, Folder folder)
        {
            SPFolder spFolder = folder as SPFolder;
            return (new CRMService()).GetPersonalViews(siteSetting, (CRMEntity)folder);
        }
        public List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId)
        {
            return (new CRMService()).GetAuditLogs(siteSetting, listName, itemId);
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            itemCount = 0;
            listItemCollectionPositionNext = "";
            return new List<IItem>();
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            return (new CRMService()).GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
        }

        public List<IItem> GetListItemsWithoutPaging(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName)
        {
            CRMService spService = new CRMService();
            string listItemCollectionPositionNext = string.Empty;
            int itemCount = 0;

            List<IItem> items = spService.GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
            Logger.Info("webUrl:" + webUrl + " - listName:" + listName + " - itemCount:" + itemCount + " - listItemCollectionPositionNext:" + listItemCollectionPositionNext, "CRMServiceManager.GetListItemsWithoutPaging");
            while (string.IsNullOrEmpty(listItemCollectionPositionNext) == false)
            {
                queryOptions.ListItemCollectionPositionNext = listItemCollectionPositionNext;
                List<IItem> _items = spService.GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
                Logger.Info("webUrl:" + webUrl + " - listName:" + listName + " - itemCount:" + itemCount + " - listItemCollectionPositionNext:" + listItemCollectionPositionNext, "CRMServiceManager.GetListItemsWithoutPaging");
                items.AddRange(_items);
            }

            return items;
        }
        //public List<IItem> GetListItems(ISiteSetting siteSetting, string webUrl, string listName, bool isRecursive)
        //{
        //    return new List<IItem>();
        //}

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder folder, int[] includedFolderTypes, string childFoldersCategoryName)
        {
            ICRMService crmService = new CRMService();
            return crmService.GetFolders(siteSetting, folder, includedFolderTypes, childFoldersCategoryName);
        }

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder folder)
        {
            ICRMService crmService = new CRMService();
            return crmService.GetFolders(siteSetting, folder, null, string.Empty);
        }

        public Folder GetRootFolder(ISiteSetting siteSetting)
        {
            return new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, siteSetting.Url, siteSetting.Url, siteSetting.Url, siteSetting.Url);
        }
        /// <summary>
        /// Return Parent Folder with object
        /// </summary>
        /// <param name="siteSetting">Site Setting</param>
        /// <param name="folder">Folder to retreive</param>
        /// <returns></returns>
        public Folder GetParentFolder(ISiteSetting siteSetting, Folder folder)
        {
            return null;
        }

        public IItem GetItem(ISiteSetting siteSetting, string itemUrl, out Sobiens.Connectors.Entities.Folder itemFolder)
        {
            ICRMService spService = new CRMService();
            itemFolder = null;
            return null;
        }

        public Folder GetWorkflowFolder(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder itemFolder, string itemUrl)
        {
            ICRMService spService = new CRMService();
            return null;
        }

        public Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folderDefinition)
        {
            ICRMService crmService = new CRMService();
            CRMWeb crmWeb = new CRMWeb();
            crmWeb.SiteSettingID = siteSetting.ID;
            crmWeb.Url = siteSetting.Url;
            crmWeb.WebUrl = siteSetting.Url;
            List<Folder> folders = crmService.GetFolders(siteSetting, crmWeb, null, string.Empty);
            Folder folder = (from x in folders where x.Title == folderDefinition.Title select x).FirstOrDefault();
            return folder;

        }

        public List<ItemVersion> GetListItemVersions(ISiteSetting siteSetting, IItem item)
        {
            return null;
        }

        public List<ItemVersion> RestoreVersion(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            return null;
        }

        public Result CheckInFile(ISiteSetting siteSetting, IItem item, string comment, CheckinTypes checkinType)
        {
            return null;
        }

        public Result CheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            return null;
        }

        public Result UndoCheckOutFile(ISiteSetting siteSetting, IItem item)
        {
            return null;
        }

        public bool CheckItemCanBeCopied(ISiteSetting siteSetting, Folder targetFolder, IItem copyItem, string fileName)
        {
            return true;
        }

        public Result CopyItem(ISiteSetting siteSetting, Folder targetFolder, IItem copySource, string newFileName)
        {
            return null;
        }

        public string GetProperty(Dictionary<string, string> properies, string key)
        {
            string actualKey = "ows_" + key;
            return properies.ContainsKey(actualKey) ? properies[actualKey] : null;
        }

        public void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, int listItemID, System.Collections.Generic.Dictionary<object, object> fields, System.Collections.Generic.Dictionary<string, object> auditInformation)
        {

        }

        public void CreateListItem(ISiteSetting siteSetting, string webUrl, string listName, System.Collections.Generic.Dictionary<object, object> fields)
        {

        }


        public uint AddFolder(ISiteSetting siteSetting, string webUrl, string folderName, string folderPath, string args)
        {
            return 1;
        }

        public string GetUser(ISiteSetting siteSetting, string UserName)
        {
            return null;
        }

        public void DeleteUniquePermissions(ISiteSetting siteSetting, Folder folder, bool applyToAllSubItems)
        {
            throw new Exception("Not implemented");
        }

        public void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, int listItemID, Dictionary<object, object> fields)
        {
            throw new NotImplementedException();
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

        public Folder CreateFolder(ISiteSetting siteSetting, string title, int templateType)
        {
            throw new NotImplementedException();
        }

    }
}
