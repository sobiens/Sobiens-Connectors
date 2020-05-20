using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using System.Collections;
using Sobiens.Connectors.Entities.Settings;
using System.Data;
using Sobiens.Connectors.Entities.Workflows;
using System.Net;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;

namespace Sobiens.Connectors.Common.Interfaces
{
    public interface IServiceManager
    {
        bool CheckConnection(ISiteSetting siteSetting);

        void OpenFile(ISiteSetting siteSetting, IItem item);

        void DownloadFile(ISiteSetting siteSetting, string url, string savePath);

        void DeleteFile(ISiteSetting siteSetting, IItem item);

        bool UploadFile(ISiteSetting siteSetting, UploadItem uploadItem, bool saveAsWord, out IItem listItem);

        bool ValidateImportValue(ISiteSetting siteSetting, Field field, string value, Dictionary<string, string> parameters, out string errorMessage);
        object ConvertImportValueToFieldValue(ISiteSetting siteSetting, Field field, string value, Dictionary<string, string> parameters);
        bool DownloadAdministrativeConfiguration(ISiteSetting ss, string url, string savePath);

        string GetTermsByLabel(ISiteSetting siteSetting, string webUrl, string label, int lcid, int resultCollectionSize, string termIds, bool addIfNotFound);

        string GetKeywordTermsByGuids(ISiteSetting siteSetting, string webUrl, int lcid, string termIds);

        //        TermSet GetTermSets(ISiteSetting siteSetting, string webUrl, int lcid, string sspIds, string termIds);
        SPTermSet GetTermSet(ISiteSetting siteSetting, Guid termSetId);

        SPTermStore GetTermStore(ISiteSetting siteSetting);
        List<SPTermGroup> GetTermGroups(ISiteSetting siteSetting);
        List<SPTermSet> GetGroupTermSets(ISiteSetting siteSetting, Guid termGroupId);
        List<SPTerm> GetTerms(ISiteSetting siteSetting, Guid termSetId);
        List<SPTerm> GetTermTerms(ISiteSetting siteSetting, Guid termId);

        List<IView> GetViews(ISiteSetting siteSetting, Folder folder);

        Folder GetFoldersTreeFromURL(ISiteSetting siteSetting, string url);

        FieldCollection GetFields(ISiteSetting siteSetting, Folder folder);
        void CreateFields(ISiteSetting siteSetting, Folder folder, List<Field> fields);

        //string[] GetPrimaryKeys(ISiteSetting siteSetting, Folder folder);
        //SQLForeignKey[] GetForeignKeys(ISiteSetting siteSetting, Folder folder);
        

            List<Workflow> GetWorkflows(ISiteSetting siteSetting, string listName);
        List<ContentType> GetContentTypes(ISiteSetting siteSetting, string listName);
        List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder, bool includeReadOnly);
        List<ContentType> GetContentTypes(ISiteSetting siteSetting);
        ContentType GetContentType(ISiteSetting siteSetting, Folder folder, string contentTypeID, bool includeReadOnly);

        Folder GetFolderByBasicFolderDefinition(ISiteSetting siteSetting, BasicFolderDefinition basicFolderDefinition, bool returnAll);

        //Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folder);

        Folder GetRootFolder(ISiteSetting siteSetting);

        Folder GetParentFolder(ISiteSetting siteSetting, Folder folder);

        Folder GetFolder(ISiteSetting siteSetting, BasicFolderDefinition folderDefinition);

        Folder GetWorkflowFolder(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder itemFolder, string itemUrl);

        IItem GetItem(ISiteSetting siteSetting, string itemUrl, out Sobiens.Connectors.Entities.Folder itemFolder);

        List<Folder> GetFolders(ISiteSetting siteSetting, Folder parentFolder, int[] includedFolderTypes, string childFoldersCategoryName);

        List<Folder> GetFolders(ISiteSetting siteSetting, Folder parentFolder);
        List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId);
        List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView selectedView, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters customFilters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount);

        //List<IItem> GetListItems(ISiteSetting siteSetting, string webUrl, string listName, bool isRecursive);

        List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount);
        List<IItem> GetListItemsWithoutPaging(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName);
        List<ItemVersion> GetListItemVersions(ISiteSetting siteSetting, IItem item); //string webUrl, string fileURL

        List<ItemVersion> RestoreVersion(ISiteSetting siteSetting, ItemVersion itemVersion); //string webUrl, string fileURL

        Result CheckInFile(ISiteSetting siteSetting, IItem item, string comment, CheckinTypes checkinType);

        Result CheckOutFile(ISiteSetting siteSetting, IItem item);

        Result UndoCheckOutFile(ISiteSetting siteSetting, IItem item);

        bool CheckItemCanBeCopied(ISiteSetting siteSetting, Folder targetFolder, IItem copyItem, string fileName);

        Result CopyItem(ISiteSetting siteSetting, Folder targetFolder, IItem copySource, string newFileName);

        uint AddFolder(ISiteSetting siteSetting,string webUrl, string folderName,string folderPath, string args);

        string GetProperty(Dictionary<string, string> properies, string key);

//        void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, int listItemID, System.Collections.Generic.Dictionary<object, object> fields);

        TemplateData GetTemplatesForItem(ISiteSetting siteSetting, IItem item);

        WorkflowData GetWorkflowDataForItem(ISiteSetting siteSetting, IItem item);

        void StartWorkflow(ISiteSetting siteSetting, IItem item, WorkflowTemplate workflow);

        CookieContainer GetCookieContainer(string url, string userName, string password);



        void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, string listItemID, System.Collections.Generic.Dictionary<string, object> fields, System.Collections.Generic.Dictionary<string, object> auditInformation);
        void CreateListItem(ISiteSetting siteSetting, string webUrl, string listName, System.Collections.Generic.Dictionary<string, object> fields);
        string GetUser(ISiteSetting siteSetting, string UserName);
        void DeleteUniquePermissions(ISiteSetting siteSetting, Folder folder, bool applyToAllSubItems);
        SPTermSet CreateTermSet(ISiteSetting siteSetting, SPTermSet termSet);
        SPTermGroup CreateTermGroup(ISiteSetting siteSetting, SPTermGroup termGroup);
        SPTerm CreateTerm(ISiteSetting siteSetting, SPTerm term);
        //void SyncSchema(ISiteSetting sourceSiteSetting, Entities.Folder sourceObject, ISiteSetting destinationSiteSetting, Entities.Folder destinationObject);
        Folder CreateFolder(ISiteSetting siteSetting, string title, int templateType);
        List<CompareObjectsResult> GetObjectDifferences(ISiteSetting sourceSiteSetting, Folder sourceFolder, ISiteSetting destinationSiteSetting, Folder destinationFolder, Action<int, string> reportProgressAction);
        void ApplyMissingCompareObjectsResult(CompareObjectsResult compareObjectsResult, ISiteSetting sourceSiteSetting, ISiteSetting destinationSiteSetting);

    }
}
