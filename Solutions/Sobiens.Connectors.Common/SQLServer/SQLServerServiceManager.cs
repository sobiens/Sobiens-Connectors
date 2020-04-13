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
using System.Data.SqlClient;
using Sobiens.Connectors.Entities.SQLServer;

namespace Sobiens.Connectors.Common.SQLServer
{
    public class SQLServerServiceManager : IServiceManager
    {
        private static SQLServerServiceManager _instance = null;
        public static SQLServerServiceManager GetInstance()
        {
            if (_instance == null)
                _instance = new SQLServerServiceManager();
            return _instance;
        }

        public bool CheckConnection(ISiteSetting siteSetting)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = new SQLServerService().GetConnectionString(siteSetting, string.Empty);
                con.Open();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetTermsByLabel(ISiteSetting siteSetting, string webUrl, string label, int lcid, int resultCollectionSize, string termIds, bool addIfNotFound)
        {
            throw new Exception("Not implemented yet");
        }

        public string GetKeywordTermsByGuids(ISiteSetting siteSetting, string webUrl, int lcid, string termIds)
        {
            throw new Exception("Not implemented yet");
        }

        public SPTermSet GetTermSet(ISiteSetting siteSetting, Guid termSetId)
        {
            throw new NotImplementedException();
        }

        private List<Folder> GetSubFoldersByBasicFolderDefinition(ISiteSetting siteSetting, Folder folder, BasicFolderDefinition basicFolderDefinition, bool returnAll)
        {
            throw new Exception("Not implemented yet");
        }

        public Folder GetFolderByBasicFolderDefinition(ISiteSetting siteSetting, BasicFolderDefinition basicFolderDefinition, bool returnAll)
        {
            throw new Exception("Not implemented yet");
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

        public void DownloadFile(ISiteSetting siteSetting, string url, string savePath)
        {
            throw new Exception("Not implemented yet");
        }


        public void OpenFile(ISiteSetting siteSetting, IItem item)
        {
            Process.Start("IExplore.exe", item.URL);
        }

        public void DeleteFile(ISiteSetting siteSetting, IItem item)
        {
            throw new Exception("Not implemented yet");
        }

        public CookieContainer GetCookieContainer(string url, string userName, string password)
        {
            throw new Exception("Not implemented yet");
        }

        public void StartWorkflow(ISiteSetting siteSetting, IItem item, WorkflowTemplate workflow)
        {
            throw new Exception("Not implemented yet");
        }

        public WorkflowData GetWorkflowDataForItem(ISiteSetting siteSetting, IItem item)
        {
            throw new Exception("Not implemented yet");
        }

        public TemplateData GetTemplatesForItem(ISiteSetting siteSetting, IItem item)
        {
            throw new Exception("Not implemented yet");
        }

        private void FillFoldersTreeFromURL(ISiteSetting siteSetting, SPBaseFolder parentFolder, string remainingUrl)
        {
            throw new Exception("Not implemented yet");
        }

        public Folder GetFoldersTreeFromURL(ISiteSetting siteSetting, string url)
        {
            throw new Exception("Not implemented yet");
        }

        private string FixSharePointFileName(string fileName)
        {
            return fileName.Replace('#', '_').Replace('%', '_').Replace('&', '_').Replace('*', '_').Replace(':', '_').Replace('<', '_').Replace('>', '_').Replace('?', '_').Replace('/', '_').Replace('{', '_').Replace('|', '_').Replace('}', '_');
        }

        public bool UploadFile(ISiteSetting siteSetting, UploadItem uploadItem, bool saveAsWord, out IItem listItem)
        {
            throw new Exception("Not implemented yet");
        }

        public string[] GetPrimaryKeys(ISiteSetting siteSetting, Folder folder)
        {
            return (new SQLServerService()).GetPrimaryKeys(siteSetting, ((SQLTable)folder).DBName, folder.Title);
        }

        public SQLForeignKey[] GetForeignKeys(ISiteSetting siteSetting, Folder folder)
        {
            return (new SQLServerService()).GetForeignKeys(siteSetting, ((SQLTable)folder).DBName, folder.Title);
        }

        public FieldCollection GetFields(ISiteSetting siteSetting, Folder folder)
        {
            return (new SQLServerService()).GetFields(siteSetting, ((SQLTable)folder).DBName, ((SQLTable)folder).ListName, ((SQLTable)folder).Schema);
        }

        public void CreateFields(ISiteSetting siteSetting, Folder folder, List<Field> fields)
        {
            (new SQLServerService()).CreateFields(siteSetting, ((SQLTable)folder).DBName, ((SQLTable)folder).ListName, fields);
        }

        public List<ContentType> GetContentTypes(ISiteSetting siteSetting, Folder folder, bool includeReadOnly)
        {
            throw new Exception("Not implemented yet");
        }

        public ContentType GetContentType(ISiteSetting siteSetting, Folder folder, string contentTypeID, bool includeReadOnly)
        {
            throw new Exception("Not implemented yet");
        }

        public List<IView> GetViews(ISiteSetting siteSetting, Folder folder)
        {
            return (new SQLServerService()).GetViews(siteSetting, null).ToList<IView>();
        }
        public List<IItem> GetAuditLogs(ISiteSetting siteSetting, string listName, string itemId)
        {
            throw new NotImplementedException();
        }
        public List<IItem> GetListItems(ISiteSetting siteSetting, Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            throw new Exception("Not implemented yet");
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            return new SQLServerService().GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
        }
        public List<IItem> GetListItemsWithoutPaging(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName)
        {
            throw new NotImplementedException();
        }


        //public List<IItem> GetListItems(ISiteSetting siteSetting, string webUrl, string listName, bool isRecursive)
        //{
        //    throw new Exception("Not implemented yet");
        //}

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder folder, int[] includedFolderTypes, string childFoldersCategoryName)
        {
            return new SQLServerService().GetFolders(siteSetting, folder,null, childFoldersCategoryName);
        }

        public List<Folder> GetFolders(ISiteSetting siteSetting, Folder folder)
        {
            return new SQLServerService().GetFolders(siteSetting, folder);
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
            throw new Exception("Not implemented yet");
        }

        public void UpdateListItem(ISiteSetting siteSetting, string webUrl, string listName, int listItemID, System.Collections.Generic.Dictionary<object, object> fields, System.Collections.Generic.Dictionary<string, object> auditInformation)
        {
            throw new Exception("Not implemented yet");
        }

        public void CreateListItem(ISiteSetting siteSetting, string dbName, string tableName, System.Collections.Generic.Dictionary<object, object> fields)
        {
            new SQLServerService().CreateListItem(siteSetting, dbName, tableName, fields);
        }
        public uint AddFolder(ISiteSetting siteSetting, string webUrl, string folderName, string folderPath, string args)
        {
            throw new Exception("Not implemented yet");
        }

        public string GetUser(ISiteSetting siteSetting, string UserName)
        {
            throw new Exception("Not implemented yet");
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

            List<SQLTable> sourceTables = new SQLServerService().GetTables(sourceSiteSetting, sourceObject);
            List<SQLFunction> sourceFunctions = new SQLServerService().GetFunctions(sourceSiteSetting, sourceObject);
            List<SQLView> sourceViews = new SQLServerService().GetViews(sourceSiteSetting, sourceObject);
            List<SQLStoredProcedure> sourceStoredProcedures = new SQLServerService().GetStoredProcedures(sourceSiteSetting, sourceObject);
            List<SQLTrigger> sourceTriggers = new SQLServerService().GetTriggers(sourceSiteSetting, sourceObject);

            List<SQLTable> destinationTables = new SQLServerService().GetTables(destinationSiteSetting, destinationObject);
            List<SQLFunction> destinationFunctions = new SQLServerService().GetFunctions(destinationSiteSetting, destinationObject);
            List<SQLView> destinationViews = new SQLServerService().GetViews(destinationSiteSetting, destinationObject);
            List<SQLStoredProcedure> destinationStoredProcedures = new SQLServerService().GetStoredProcedures(destinationSiteSetting, destinationObject);
            List<SQLTrigger> destinationTriggers = new SQLServerService().GetTriggers(destinationSiteSetting, destinationObject);

            compareObjectsResults.AddRange(CompareManager.Instance.GetObjectsDifferences(sourceSiteSetting, sourceObject, sourceTables.ToList<Folder>(), destinationSiteSetting, destinationTables.ToList<Folder>(), destinationObject, "Table", CheckIfEquals));
            compareObjectsResults.AddRange(CompareManager.Instance.GetObjectsDifferences(sourceSiteSetting, sourceObject, sourceFunctions.ToList<Folder>(), destinationSiteSetting, destinationFunctions.ToList<Folder>(), destinationObject, "Function", CheckIfEquals));
            compareObjectsResults.AddRange(CompareManager.Instance.GetObjectsDifferences(sourceSiteSetting, sourceObject, sourceViews.ToList<Folder>(), destinationSiteSetting, destinationViews.ToList<Folder>(), destinationObject, "View", CheckIfEquals));
            compareObjectsResults.AddRange(CompareManager.Instance.GetObjectsDifferences(sourceSiteSetting, sourceObject, sourceStoredProcedures.ToList<Folder>(), destinationSiteSetting, destinationStoredProcedures.ToList<Folder>(), destinationObject, "Stored Procedure", CheckIfEquals));
            compareObjectsResults.AddRange(CompareManager.Instance.GetObjectsDifferences(sourceSiteSetting, sourceObject, sourceTriggers.ToList<Folder>(), destinationSiteSetting, destinationTriggers.ToList<Folder>(), destinationObject, "Trigger", CheckIfEquals));

            return compareObjectsResults;
        }


        public bool CheckIfEquals(ISiteSetting sourceSiteSetting, Folder sourceObject, ISiteSetting destinationSiteSetting, Folder destinationObject)
        {
            if (sourceObject as SQLTable != null)
            {
                FieldCollection sourceFields = (sourceObject as SQLTable).Fields;
                FieldCollection objectToCompareWithFields = (destinationObject as SQLTable).Fields;
                bool hasFieldChange = false;
                foreach (Field field in objectToCompareWithFields)
                {
                    if (sourceFields.Where(t => t.Name.Equals(field.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                    {
                        hasFieldChange = true;
                    }
                }

                if (hasFieldChange == true)
                {
                    return false;
                }
            }
            else if (sourceObject as SQLFunction != null)
            {
                if (((SQLFunction)sourceObject).ToSQLSyntax().Equals(((SQLFunction)destinationObject).ToSQLSyntax()) == false)
                {
                    return false;
                }
            }
            else if (sourceObject as IView != null)
            {
                /*
                if (((IView)sourceObject).Content.Equals(((IView)destinationObject).Content) == false)
                {
                    return false;
                }
                */
            }
            else if (sourceObject as SQLStoredProcedure != null)
            {
                if (((SQLStoredProcedure)sourceObject).ToSQLSyntax().Equals(((SQLStoredProcedure)destinationObject).ToSQLSyntax()) == false)
                {
                    return false;
                }
            }
            else if (sourceObject as SQLTrigger != null)
            {
                if (((SQLTrigger)sourceObject).ToSQLSyntax().Equals(((SQLTrigger)destinationObject).ToSQLSyntax()) == false)
                {
                    return false;
                }
            }

            return true;
        }
        public void ApplyMissingCompareObjectsResult(CompareObjectsResult compareObjectsResult, ISiteSetting sourceSiteSetting, ISiteSetting destinationSiteSetting) {
            if (compareObjectsResult.ObjectToCompareWith as SQLTable != null)
            {
                SQLDB sourceDB = compareObjectsResult.SourceParentObject as SQLDB;
                SQLTable destinationTable = compareObjectsResult.ObjectToCompareWith as SQLTable;
                List<Field> fields = new SQLServerService().GetFields(sourceSiteSetting, destinationTable.DBName, destinationTable.Title, destinationTable.Schema);
                new SQLServerService().CreateTable(sourceSiteSetting, sourceDB.Title, destinationTable, fields);
            }
            else if (compareObjectsResult.ObjectToCompareWith as SQLFunction != null)
            {
                SQLDB sourceDB = compareObjectsResult.SourceParentObject as SQLDB;
                SQLFunction destinationFunction = compareObjectsResult.ObjectToCompareWith as SQLFunction;
                new SQLServerService().CreateFunction(sourceSiteSetting, sourceDB.Title, destinationFunction);
            }
            else if (compareObjectsResult.ObjectToCompareWith as SQLStoredProcedure != null)
            {
                SQLDB sourceDB = compareObjectsResult.SourceParentObject as SQLDB;
                SQLStoredProcedure destinationSQLStoredProcedure = compareObjectsResult.ObjectToCompareWith as SQLStoredProcedure;
                new SQLServerService().CreateStoredProcedure(sourceSiteSetting, sourceDB.Title, destinationSQLStoredProcedure);
            }
            else if (compareObjectsResult.ObjectToCompareWith as SQLView != null)
            {
                SQLDB sourceDB = compareObjectsResult.SourceParentObject as SQLDB;
                SQLView destinationView = compareObjectsResult.ObjectToCompareWith as SQLView;
                new SQLServerService().CreateView(sourceSiteSetting, sourceDB.Title, destinationView);
            }
            else if (compareObjectsResult.ObjectToCompareWith as SQLTrigger != null)
            {
                SQLDB sourceDB = compareObjectsResult.SourceParentObject as SQLDB;
                SQLTrigger trigger = compareObjectsResult.ObjectToCompareWith as SQLTrigger;
                new SQLServerService().CreateTrigger(sourceSiteSetting, sourceDB.Title, trigger);
            }
        }

    }
}