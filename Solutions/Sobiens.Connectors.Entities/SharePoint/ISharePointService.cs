using System;
using System.Net;
using System.Collections.Generic;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SharePoint;

namespace Sobiens.Connectors.Entities.SharePoint
{
    public interface ISharePointService
    {
        string GetTermsByLabel(ISiteSetting siteSetting, string webUrl, string label, int lcid, int resultCollectionSize, string termIds, bool addIfNotFound);
        string GetKeywordTermsByGuids(ISiteSetting siteSetting, string webUrl, int lcid, string termIds);
        List<Folder> GetFolders(ISiteSetting siteSetting, Folder currentFolder);
        List<Folder> GetFolders(ISiteSetting siteSetting, Folder currentFolder, int[] includedFolderTypes);
        List<IView> GetViews(ISiteSetting siteSetting, string webUrl, string listName);
        List<IItem> GetListItems(ISiteSetting siteSetting, IView view, string sortField, bool isAsc, bool isDocumentLibrary, string webUrl, string listName, string folderName, string currentListItemCollectionPositionNext, CamlFilters customFilters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount);
        List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount);
        SPWeb GetWeb(ISiteSetting siteSetting, string webUrl);
        SPList GetList(ISiteSetting siteSetting, string listRootFolderUrl);
        SPFolder GetFolder(ISiteSetting siteSetting, string folderUrl);
        SPListItem GetItem(ISiteSetting siteSetting, string itemUrl, out Sobiens.Connectors.Entities.Folder itemFolder);
        string GetUrlContent(ISiteSetting siteSetting, string url);
        void DownLoadFile(ISiteSetting siteSetting, string fileUrl, string saveFilePath);
        List<ItemVersion> GetListItemVersions(ISiteSetting siteSetting, string webUrl, string fileURL);
        List<ItemVersion> RestoreVersion(ISiteSetting siteSetting, ItemVersion itemVersion);
        Result CheckInFile(ISiteSetting siteSetting, string webUrl, string pageURL, string comment, CheckinTypes checkinType);
        Result CheckOutFile(ISiteSetting siteSetting, string webUrl, string pageURL);
        Result UndoCheckOutFile(ISiteSetting siteSetting, string webUrl, string pageURL);
        bool CheckFileExistency(ISiteSetting siteSetting, string webUrl, string listName, string folderName, int? listItemID, string fileName);
        Result CopyFile(ISiteSetting siteSetting, string webURL, string copySource, string copyDest);
        uint AddFolder(ISiteSetting siteSetting, string webURL, string folderName, string folderPath, string listName);
        string GetUser(ISiteSetting siteSetting, string UserName);
        void DeleteUniquePermissions(ISiteSetting siteSetting, Folder folder, bool applyToAllSubItems);
        List<SPTermGroup> GetTermGroups(ISiteSetting siteSetting);
        List<SPTermSet> GetTermSets(ISiteSetting siteSetting, Guid termGroupId);

        List<SPTerm> GetTerms(ISiteSetting siteSetting, Guid termSetId);
        List<SPTerm> GetTermTerms(ISiteSetting siteSetting, Guid termId);
    }
}
