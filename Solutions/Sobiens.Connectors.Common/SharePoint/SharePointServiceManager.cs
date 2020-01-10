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
using Sobiens.Connectors.Entities.Workflows;
using System.Net;

namespace Sobiens.Connectors.Common.SharePoint
{
    public class SharePointServiceManager : IServiceManager
    {
        private static SharePointServiceManager _instance = null;
        public static SharePointServiceManager GetInstance()
        {
            if (_instance == null)
                _instance = new SharePointServiceManager();
            return _instance;
        }

        public bool CheckConnection(ISiteSetting siteSetting)
        {
            SharePointService spService = new SharePointService();
            SPWeb web = null;
            try
            {
                web = spService.GetWeb(siteSetting, siteSetting.Url);
            }
            catch
            {
            }
            return web == null ? false : true;
        }

        public string GetTermsByLabel(ISiteSetting siteSetting, string webUrl, string label, int lcid, int resultCollectionSize, string termIds, bool addIfNotFound)
        {
            return new SharePointService().GetTermsByLabel(siteSetting, webUrl, label, lcid, resultCollectionSize, termIds, addIfNotFound);
        }

        public string GetKeywordTermsByGuids(ISiteSetting siteSetting, string webUrl, int lcid, string termIds)
        {
            return new SharePointService().GetKeywordTermsByGuids(siteSetting, webUrl, lcid, termIds);
        }

        public SPTermSet GetTermSet(ISiteSetting siteSetting, Guid termSetId)//JD
        {
            return new SharePointService().GetTermSet(siteSetting, termSetId);
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
            SPListItem listItem = (SPListItem)item;
            SharePointService.DeleteListItem((SiteSetting)siteSetting, listItem.WebURL, listItem.ListName, listItem.URL, listItem.ID);
        }

        public CookieContainer GetCookieContainer(string url, string userName, string password)
        {
            return SharePointService.GetCookieContainer(url, userName, password);
        }

        public void StartWorkflow(ISiteSetting siteSetting, IItem item, WorkflowTemplate workflow)
        {
            SPListItem listItem = (SPListItem)item;
            SharePointService.StartWorkflow(siteSetting, listItem.WebURL, listItem.URL, workflow);
        }

        public WorkflowData GetWorkflowDataForItem(ISiteSetting siteSetting, IItem item)
        {
            SPListItem listItem = (SPListItem)item;
            return SharePointService.GetWorkflowDataForItem((SiteSetting)siteSetting, listItem.WebURL, listItem.URL);
        }

        public TemplateData GetTemplatesForItem(ISiteSetting siteSetting, IItem item)
        {
            SPListItem listItem = (SPListItem)item;
            return SharePointService.GetTemplatesForItem((SiteSetting)siteSetting, listItem.WebURL, listItem.URL);
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

            SPBaseFolder topFolder = new SharePointOutlookConnector(siteSetting).GetRootFolder() as SPBaseFolder;
            string remainingUrl = url.absoluteTorelative(siteSetting.Url);
            //string remainingUrl = string.Empty;
            //if (url.Equals(siteSetting.Url, StringComparison.InvariantCultureIgnoreCase) == true)
            //{
            //    remainingUrl = string.Empty;
            //}
            //else
            //{
            //    remainingUrl = url.Substring(url.IndexOf(siteSetting.Url,StringComparison.InvariantCultureIgnoreCase) + siteSetting.Url.Length + 1);
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

        private string FixSharePointFileName(string fileName)
        {
            return fileName.Replace('#', '_').Replace('%', '_').Replace('&', '_').Replace('*', '_').Replace(':', '_').Replace('<', '_').Replace('>', '_').Replace('?', '_').Replace('/', '_').Replace('{', '_').Replace('|', '_').Replace('}', '_');
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
            string filename = new FileInfo(uploadItem.FilePath).Name;
            /*
            KeyValuePair<object, object> title = uploadItem.FieldInformations.Where(f => ((Field)f.Key).Name == "Title").FirstOrDefault();
            if (title.Value != null)
            {
                filename = title.Value.ToString() + new FileInfo(copySource).Extension;
            }
            else
            {
                filename = new FileInfo(copySource).Name;
            }
             */
            string[] copyDest = new string[1] { destinationFolderUrl + "/" + FixSharePointFileName(filename) };
            byte[] itemByteArray = SharePointServiceManager.ReadByteArrayFromFile(uploadItem.FilePath);

            string newFileName = copySource;

            #region Conflicts
            /* This should be done before this function is called
            IOutlookConnector connector = OutlookConnector.GetConnector(uploadItem.Folder.SiteSetting);
            bool doThisForNextConflicts = false;
            if ((doThisForNextConflicts == true && lastFileExistDialogResults == FileExistDialogResults.Skip) || lastFileExistDialogResults == FileExistDialogResults.Cancel)
            {
                uploadItem.SharePointListViewControl.DeleteUploadItemInvoke(uploadItem.UniqueID);
                return;
            }

            bool isCurrentFileUploadCanceled = false;
            if ((doThisForNextConflicts == false) || (doThisForNextConflicts == true && lastFileExistDialogResults == FileExistDialogResults.Copy))
            {
                while (connector.CheckFileExistency(uploadItem.Folder, null, newFileName) == true)
                {
                    FileExistConfirmationForm fileExistConfirmationForm = new FileExistConfirmationForm(copyDest[0]);
                    fileExistConfirmationForm.ShowDialog();
                    lastFileExistDialogResults = fileExistConfirmationForm.FileExistDialogResult;
                    doThisForNextConflicts = fileExistConfirmationForm.DoThisForNextConflicts;

                    newFileName = fileExistConfirmationForm.NewFileName;
                    if (lastFileExistDialogResults == FileExistDialogResults.Skip || lastFileExistDialogResults == FileExistDialogResults.Cancel)
                    {
                        uploadItem.SharePointListViewControl.DeleteUploadItemInvoke(uploadItem.UniqueID);
                        isCurrentFileUploadCanceled = true;
                        break;
                    }
                    if (lastFileExistDialogResults == FileExistDialogResults.CopyAndReplace)
                    {
                        break;
                    }
                    string newCopyDest = copyDest[0].Substring(0, copyDest[0].LastIndexOf("/")) + "/" + newFileName;
                    copyDest = new string[] { newCopyDest };
                }
            }
            if (isCurrentFileUploadCanceled == true)
                return;
            */
            #endregion Conflicts

            if (spFolder.IsDocumentLibrary)
            {
                SPListItem spListItem = null;
                uint? result = SharePointService.UploadFile(siteSetting, listName, rootFolderPath, siteURL, webURL, copySource, copyDest, myByteArray, uploadItem.FieldInformations, uploadItem.ContentType, out spListItem);
                if (result.HasValue && spListItem != null)
                {
                    listItem = spListItem;
                    uploadSucceeded = true;
                }
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
            SPFolder spFolder = folder as SPFolder;
            new SharePointService().CreateFields(siteSetting, spFolder.WebUrl, spFolder.ListName, fields);
        }

        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder, bool includeReadOnly)
        {
            SPFolder spFolder = folder as SPFolder;
            return SharePointService.GetContentTypes(siteSetting, spFolder.WebUrl, spFolder.RootFolderPath, spFolder.ListName, includeReadOnly);
        }

        public ContentType GetContentType(ISiteSetting siteSetting, Folder folder, string contentTypeID, bool includeReadOnly)
        {
            SPFolder spFolder = folder as SPFolder;
            return SharePointService.GetContentType(siteSetting, spFolder.WebUrl, spFolder.RootFolderPath, spFolder.ListName, contentTypeID, includeReadOnly);
        }

        public List<IView> GetViews(ISiteSetting siteSetting, Folder folder)
        {
            SPFolder spFolder = folder as SPFolder;
            return new SharePointService().GetViews(siteSetting, spFolder.WebUrl, spFolder.ListName);
        }
        public List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId)
        {
            throw new NotImplementedException();
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
        }
        public List<IItem> GetListItemsWithoutPaging(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName)
        {
            ISharePointService spService = new SharePointService();
            string listItemCollectionPositionNext = string.Empty;
            int itemCount = 0;

            List<IItem> items = spService.GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
            Logger.Info("webUrl:" + webUrl + " - listName:" + listName + " - itemCount:" + itemCount + " - listItemCollectionPositionNext:" + listItemCollectionPositionNext, "SharePointServiceManager.GetListItemsWithoutPaging");
            while (string.IsNullOrEmpty(listItemCollectionPositionNext) == false)
            {
                queryOptions.ListItemCollectionPositionNext = listItemCollectionPositionNext;
                List<IItem> _items = spService.GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
                Logger.Info("webUrl:" + webUrl + " - listName:" + listName + " - itemCount:" + itemCount + " - listItemCollectionPositionNext:" + listItemCollectionPositionNext, "SharePointServiceManager.GetListItemsWithoutPaging");
                items.AddRange(_items);
            }

            return items;
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            SPFolder _folder = folder as SPFolder;
            if (_folder == null)
            {
                listItemCollectionPositionNext = string.Empty;
                itemCount = 0;
                return new List<IItem>();
            }

            ISharePointService spService = new SharePointService();
            string folderPath = string.Empty;
            if (folder as SPList == null)
            {
                folderPath = _folder.GetPath();
            }

            return spService.GetListItems(siteSetting, view, sortField, isAsc, _folder.IsDocumentLibrary, _folder.WebUrl, _folder.ListName, folderPath, currentListItemCollectionPositionNext, filters, isRecursive, out listItemCollectionPositionNext, out itemCount);
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
            ISharePointService spService = new SharePointService();
            return spService.GetFolders(siteSetting, folder, includedFolderTypes, childFoldersCategoryName);
        }

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder folder)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetFolders(siteSetting, folder);
        }

        public Folder GetRootFolder(ISiteSetting siteSetting)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetWeb(siteSetting, siteSetting.Url);
            //return new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, siteSetting.Url, siteSetting.Url, siteSetting.Url);
        }
        /// <summary>
        /// Return Parent Folder with object
        /// </summary>
        /// <param name="siteSetting">Site Setting</param>
        /// <param name="folder">Folder to retreive</param>
        /// <returns></returns>
        public Folder GetParentFolder(ISiteSetting siteSetting, Folder folder)
        {
            SPBaseFolder spFolder = folder as SPBaseFolder;
            string folderPath = spFolder.WebUrl.CombineUrl(spFolder.FolderPath).TrimEnd(new char[] { '/' });
            if (folderPath.Equals(siteSetting.Url, StringComparison.OrdinalIgnoreCase) == true)
                return null;

            //folderPath = folderPath.Substring(0, folderPath.LastIndexOf('/'));//JD
            folderPath = folderPath.TrimEnd('/');

            ISharePointService spService = new SharePointService();
            if (folder as SPList != null)
            {
                //return spService.GetWeb(siteSetting, folderPath);
                return spService.GetList(siteSetting, folderPath);

            }
            else if (folder as SPWeb != null)
            {
                return spService.GetWeb(siteSetting, folderPath);
            }
            else
            {
                if (folder.GetRoot() == folder.GetUrl())
                {
                    return spService.GetList(siteSetting, folderPath);
                }
                return spService.GetList(siteSetting, folderPath);
                //return spService.GetFolder(siteSetting, folderPath);
            }
        }

        public IItem GetItem(ISiteSetting siteSetting, string itemUrl, out Sobiens.Connectors.Entities.Folder itemFolder)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetItem(siteSetting, itemUrl, out itemFolder);
        }

        public Folder GetWorkflowFolder(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder itemFolder, string itemUrl)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetList(siteSetting, itemFolder.GetWebUrl() + itemFolder.GetRoot());
        }

        public Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folderDefinition)
        {
            ISharePointService spService = new SharePointService();
            if (typeof(SPList).FullName.Equals(folderDefinition.FolderType, StringComparison.OrdinalIgnoreCase) == true)
            {
                return spService.GetList(siteSetting, folderDefinition.FolderUrl);
            }
            else if (typeof(SPWeb).FullName.Equals(folderDefinition.FolderType, StringComparison.OrdinalIgnoreCase) == true)
            {
                return spService.GetWeb(siteSetting, folderDefinition.FolderUrl);
            }
            else
            {
                return spService.GetFolder(siteSetting, folderDefinition.FolderUrl);
            }
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

            //SPDocument spDocument = (SPDocument)copyItem;
            ISharePointService spService = new SharePointService();
            return !spService.CheckFileExistency(siteSetting, spFolder.WebUrl, spFolder.ListName, spFolder.GetUrl(), null, fileName);//JD spFolder.FolderPath
        }

        public Result CopyItem(ISiteSetting siteSetting, Folder targetFolder, IItem copySource, string newFileName)
        {

            SPFolder spFolder = (SPFolder)targetFolder;
            SPListItem spListItem = (SPListItem)copySource;
            ISharePointService spService = new SharePointService();
            //return spService.CopyFile(siteSetting, spFolder.WebUrl, spFolder.ListName, spFolder.FolderPath + "/" + newFileName, out myCopyResultArray);
            return spService.CopyFile(siteSetting, spFolder.WebUrl, copySource.URL, spFolder.GetUrl() + "/" + newFileName);

        }

        public string GetProperty(Dictionary<string, string> properies, string key)
        {
            string actualKey = "ows_" + key;
            return properies.ContainsKey(actualKey) ? properies[actualKey] : null;
        }

        public void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, int listItemID, System.Collections.Generic.Dictionary<object, object> fields, System.Collections.Generic.Dictionary<string, object> auditInformation)
        {
            Hashtable changedProperties = SharePointService.getChangedProperties(null, fields);
            //if (auditInformation["Editor"] != null && auditInformation["Editor"] != string.Empty)
            //    changedProperties.Add("Editor", auditInformation["Editor"]);

            if (auditInformation["Modified"] != null && auditInformation["Modified"] != string.Empty)
                changedProperties.Add("Modified", auditInformation["Modified"]);

            SharePointService.UpdateListItem(siteSetting, webUrl, listName, listItemID, changedProperties, true);
        }

        public void CreateListItem(ISiteSetting siteSetting, string webUrl, string listName, System.Collections.Generic.Dictionary<object, object> fields)
        {
            SharePointService.CreateListItem(siteSetting, string.Empty, webUrl, listName, fields);
        }
        public uint AddFolder(ISiteSetting siteSetting, string webUrl, string folderName, string folderPath, string args)
        {
            ISharePointService spService = new SharePointService();
            return spService.AddFolder(siteSetting, webUrl, folderName, folderPath, args);
        }

        public string GetUser(ISiteSetting siteSetting, string UserName)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetUser(siteSetting, UserName);
        }

        public void DeleteUniquePermissions(ISiteSetting siteSetting, Folder folder, bool applyToAllSubItems)
        {
            ISharePointService spService = new SharePointService();
            spService.DeleteUniquePermissions(siteSetting, folder, applyToAllSubItems);
        }

        public string[] GetPrimaryKeys(ISiteSetting siteSetting, Folder folder)
        {
            throw new NotImplementedException();
        }
        public SPTermStore GetTermStore(ISiteSetting siteSetting)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetTermStore(siteSetting);
        }
        public List<SPTermGroup> GetTermGroups(ISiteSetting siteSetting)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetTermGroups(siteSetting);
        }

        public List<SPTermSet> GetGroupTermSets(ISiteSetting siteSetting, Guid termGroupId)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetTermSets(siteSetting, termGroupId);
        }

        public List<SPTerm> GetTerms(ISiteSetting siteSetting, Guid termSetId)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetTerms(siteSetting, termSetId);
        }

        public List<SPTerm> GetTermTerms(ISiteSetting siteSetting, Guid termId)
        {
            ISharePointService spService = new SharePointService();
            return spService.GetTermTerms(siteSetting, termId);
        }

        public SPTermSet CreateTermSet(ISiteSetting siteSetting, SPTermSet termSet)
        {
            return (new SharePointService()).CreateTermSet(siteSetting, termSet);
        }
        public SPTermGroup CreateTermGroup(ISiteSetting siteSetting, SPTermGroup termGroup)
        {
            return (new SharePointService()).CreateTermGroup(siteSetting, termGroup);
        }
        public SPTerm CreateTerm(ISiteSetting siteSetting, SPTerm term)
        {
            return (new SharePointService()).CreateTerm(siteSetting, term);
        }

        public List<ContentType> GetContentTypes(ISiteSetting siteSetting)
        {
            return (new SharePointService()).GetContentTypes(siteSetting, siteSetting.Url, ((SPWeb)siteSetting).GetRoot(), true);
        }
        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, string listName)
        {
            return SharePointService.GetContentTypes(siteSetting, siteSetting.Url, ((SPWeb)siteSetting).GetRoot(), listName, true);
        }
        public List<Workflow> GetWorkflows(ISiteSetting siteSetting, string listName)
        {
            return (new SharePointService()).GetWorkflows(siteSetting, listName);
        }

        public Folder CreateFolder(ISiteSetting siteSetting, string title, int templateType)
        {
            SharePointService sharePointService = new SharePointService();
            return sharePointService.CreateList(siteSetting, title, templateType);

        }

        public List<CompareObjectsResult> GetObjectDifferences(ISiteSetting sourceSiteSetting, Folder sourceObject, ISiteSetting destinationSiteSetting, Folder destinationObject)
        {
            List<CompareObjectsResult> compareObjectsResults = new List<CompareObjectsResult>();

            List<SPList> sourceLists = new SharePointService().GetLists(sourceSiteSetting, ((SPWeb)sourceObject).Url);
            List<ContentType> sourceContentTypes = new SharePointService().GetContentTypes(sourceSiteSetting, ((SPWeb)sourceObject).Url, string.Empty, false);
            List<Field> sourceFields = new SharePointService().GetFields(sourceSiteSetting, ((SPWeb)sourceObject).Url);

            List<SPList> destinationLists = new SharePointService().GetLists(destinationSiteSetting, ((SPWeb)destinationObject).Url);
            List<ContentType> destinationContentTypes = new SharePointService().GetContentTypes(destinationSiteSetting, ((SPWeb)destinationObject).Url, string.Empty, false);
            List<Field> destinationFields = new SharePointService().GetFields(destinationSiteSetting, ((SPWeb)destinationObject).Url);

            compareObjectsResults.AddRange(CompareManager.Instance.GetObjectsDifferences(sourceSiteSetting, sourceObject, sourceLists.ToList<Folder>(), destinationSiteSetting, destinationLists.ToList<Folder>(), destinationObject, "List"));
            compareObjectsResults.AddRange(CompareManager.Instance.GetObjectsDifferences(sourceSiteSetting, sourceObject, sourceContentTypes.ToList<Folder>(), destinationSiteSetting, destinationContentTypes.ToList<Folder>(), destinationObject, "Content Type"));
            //compareObjectsResults.AddRange(GetObjectsDifferences(sourceSiteSetting, sourceViews.ToList<Folder>(), destinationSiteSetting, destinationViews.ToList<Folder>(), "View"));
            compareObjectsResults.AddRange(CompareManager.Instance.GetObjectsDifferences(sourceSiteSetting, sourceObject, sourceFields.ToList<Folder>(), destinationSiteSetting, destinationFields.ToList<Folder>(), destinationObject, "Field"));

            return compareObjectsResults;
        }


        private bool CheckIfEquals(ISiteSetting sourceSiteSetting, Folder sourceObject, ISiteSetting destinationSiteSetting, Folder destinationObject)
        {
            return true;
        }
        public void ApplyMissingCompareObjectsResult(CompareObjectsResult compareObjectsResult, ISiteSetting sourceSiteSetting, ISiteSetting destinationSiteSetting) {
            SPWeb sourceWeb = compareObjectsResult.SourceParentObject as SPWeb;
            SPWeb destinationWeb = compareObjectsResult.ObjectToCompareWithParentObject as SPWeb;
            if (compareObjectsResult.ObjectToCompareWith as SPList != null)
            {
                SPList destinationList = compareObjectsResult.ObjectToCompareWith as SPList;
                List<Field> fields = new SharePointService().GetFields(destinationSiteSetting, destinationWeb.Url, destinationList.Title);
                fields = fields.Where(t => t.FromBaseType == false).ToList();
                new SharePointService().CreateList(sourceSiteSetting, destinationList.Title, destinationList.ServerTemplate);
                new SharePointService().CreateFields(sourceSiteSetting, sourceWeb.Url, destinationList.Title, fields);
            }
            else if (compareObjectsResult.ObjectToCompareWith as ContentType != null)
            {
                ContentType destinationFunction = compareObjectsResult.ObjectToCompareWith as ContentType;
//                new SharePointService().cre(sourceSiteSetting, sourceDB.Title, destinationFunction);
            }
            else if (compareObjectsResult.ObjectToCompareWith as Field != null)
            {
                Field destinationField = compareObjectsResult.ObjectToCompareWith as Field;
//                new SharePointService().CreateFields(sourceSiteSetting, sourceDB.Title, destinationSQLStoredProcedure);
            }
        }

    }
}
