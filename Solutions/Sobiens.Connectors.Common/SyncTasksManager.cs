#if General
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common.Extensions;
using Sobiens.Connectors.Common.SLExcelUtility;
using Sobiens.Connectors.Entities.SharePoint;
using System.ComponentModel;
using Sobiens.Connectors.Entities.Interfaces;
using Microsoft.SharePoint.Client;
using Sobiens.Connectors.Entities.SQLServer;
using Sobiens.Connectors.Entities.CRM;
using Field = Sobiens.Connectors.Entities.Field;

namespace Sobiens.Connectors.Common
{
    public class SyncTasksManager
    {
        private static object taskHistoryLockObject = new object();
        private static object tasksSaveLockObject = new object();
        private static SyncTasksManager _Instance = null;

        public static SyncTasksManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new SyncTasksManager();
            }
            return _Instance;
        }
        public int QueueLength = 0;
        //public static Dictionary<string, Microsoft.SharePoint.Client.Taxonomy.TermCollection> TermValues = new Dictionary<string, Microsoft.SharePoint.Client.Taxonomy.TermCollection>();

        private SyncTasksManager()
        {
        }

        private Dictionary<string, SLExcelData> ProcessData = new Dictionary<string, SLExcelData>();
        private List<SyncTask> _SyncTasks = null;
        public List<SyncTask> SyncTasks
        {
            get
            {
                if (_SyncTasks == null)
                {
                    _SyncTasks = this.LoadState();
                }

                return _SyncTasks;
            }
        }

        public void RemoveProcess(Guid processID)
        {
            lock (tasksSaveLockObject)
            {
                SyncTask syncTask = SyncTasks.Where(t => t.ProcessID == processID).FirstOrDefault();
                SyncTasks.Remove(syncTask);
                SaveSyncTasks();
            }
        }

        public void AddSyncTask(SyncTask syncTask)
        {
            lock (tasksSaveLockObject)
            {
                SyncTasks.Add(syncTask);
                SaveSyncTasks();
            }
        }

        public void SaveProcessStatus(Guid processID, string status, DateTime? lastRunStartDate, DateTime? lastSuccessfullyCompletedStartDate)
        {
            lock (tasksSaveLockObject) {
                SyncTask syncTask = SyncTasks.Where(t => t.ProcessID == processID).FirstOrDefault();

                if(string.IsNullOrEmpty(status) == false)
                    syncTask.Status = status;

                if(lastRunStartDate.HasValue == true)
                    syncTask.LastRunStartDate = lastRunStartDate.Value;

                if (lastSuccessfullyCompletedStartDate.HasValue == true)
                    syncTask.LastSuccessfullyCompletedStartDate = lastSuccessfullyCompletedStartDate.Value;

                SaveSyncTasks();
            }
        }

        public SLExcelData GetProcessData(Guid syncTaskID, Guid processId, bool isCompleted)
        {
            if (isCompleted == false)
            {
                if (ProcessData.ContainsKey(syncTaskID.ToString()) == true)
                {
                    return ProcessData[syncTaskID.ToString()];
                }
            }
            else
            {
                string folderPath = ConfigurationManager.GetInstance().GetSyncTaskFolder(processId);
                string processedExportFilePath = folderPath + "\\ProcessedExport.xlsx";
                if (System.IO.File.Exists(processedExportFilePath) == true)
                {
                    SLExcelReader reader = new SLExcelReader();
                    return reader.ReadExcel(processedExportFilePath, true);
                }
            }

            return null;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary> 
        public void SaveSyncTasks()
        {
            string configurationFilePath = ConfigurationManager.GetInstance().GetSyncTasksFilePath();
            Logger.Info("SyncTasks are being save to " + configurationFilePath, "SyncTasksManager");
            SerializationManager.SaveConfiguration<List<SyncTask>>(this.SyncTasks, configurationFilePath);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void AddSyncTaskHistory(SyncTask syncTask)
        {
            List<SyncTask> syncTasks = new List<SyncTask>();
            string taskHistoryFilePath = ConfigurationManager.GetInstance().GetCurrentSyncTaskHistoryFilePath();
            Logger.Info("SyncTaskHistories are being save to " + taskHistoryFilePath, "SyncTasksManager");
            lock (taskHistoryLockObject)
            {
                if(System.IO.File.Exists(taskHistoryFilePath) == true)
                {
                    syncTasks = SerializationManager.ReadSettings<List<SyncTask>>(taskHistoryFilePath);
                }
                else
                {
                    syncTasks = new List<SyncTask>();
                }

                syncTasks.Add(syncTask);
            }
            SerializationManager.SaveConfiguration<List<SyncTask>>(syncTasks, taskHistoryFilePath);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public List<SyncTask> GetSyncTaskHistories(DateTime date)
        {
            List<SyncTask> syncTasks = new List<SyncTask>();
            string taskHistoryFilePath = ConfigurationManager.GetInstance().GetSyncTaskHistoryFilePath(date);
            Logger.Info("SyncTaskHistories are being read from " + taskHistoryFilePath, "SyncTasksManager");
            lock (taskHistoryLockObject)
            {
                if (System.IO.File.Exists(taskHistoryFilePath) == true)
                {
                    syncTasks = SerializationManager.ReadSettings<List<SyncTask>>(taskHistoryFilePath);
                }
                else
                {
                    syncTasks = new List<SyncTask>();
                }
            }

            return syncTasks;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSyncTaskStatus(SyncTask syncTask, DateTime startDate, DateTime endDate, bool isSuccessful, string errorMessage)
        {
            SyncTaskStatus status = new SyncTaskStatus() {
                CompletionTime = endDate,
                ErrorMessage = errorMessage,
                StartTime = startDate,
                Successful = isSuccessful,
                SyncTaskID = syncTask.ID
            };

            string configurationFilePath = ConfigurationManager.GetInstance().GetSyncTaskStatusFilePath(syncTask);
            Logger.Info("SyncTaskStatus are being save to " + configurationFilePath, "SyncTasksManager");
            SerializationManager.SaveConfiguration<SyncTaskStatus>(status, configurationFilePath);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public SyncTaskStatus GetLastSyncTaskStatus(SyncTask syncTask)
        {
            string syncTaskStatusFilePath = ConfigurationManager.GetInstance().GetSyncTaskStatusFilePath(syncTask);
            if (System.IO.File.Exists(syncTaskStatusFilePath) == false)
                return null;

            Logger.Info("SyncTaskStatus are being read from " + syncTaskStatusFilePath, "SyncTasksManager");
            return SerializationManager.ReadSettings<SyncTaskStatus>(syncTaskStatusFilePath);
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        private List<SyncTask> LoadState()
        {
            string settingFilePath = ConfigurationManager.GetInstance().GetSyncTasksFilePath();

            Logger.Info("SyncTasks are being loaded from " + settingFilePath, "SyncTasksManager");
            if (System.IO.File.Exists(settingFilePath) == false)
                return new List<SyncTask>();

            List<SyncTask> configuration = SerializationManager.ReadSettings<List<SyncTask>>(settingFilePath);
            return configuration;
        }

        public void ExportSyncTaskItems(SyncTaskListItemsCopy syncTask, bool shouldExportSourceListItems, bool shouldSourceExportDocuments, bool shouldExportDestinationtListItems, BackgroundWorker backgroundWorker, DateTime? lastProcessStartDate, int includeVersionsLimit)
        {
            string folderPath = ConfigurationManager.GetInstance().GetSyncTaskFolder(syncTask);
            string sourceExportFilePath = folderPath + "\\SourceExport.xlsx";
            string destinationExportFilePath = folderPath + "\\DestinationExport.xlsx";
            string sourceExportDocumentsFolderPath = folderPath + "\\SourceDocuments";

            if (shouldExportSourceListItems == true && System.IO.File.Exists(sourceExportFilePath) == true)
                System.IO.File.Delete(sourceExportFilePath);

            if (shouldSourceExportDocuments == true && System.IO.File.Exists(sourceExportDocumentsFolderPath) == true)
                System.IO.Directory.Delete(sourceExportDocumentsFolderPath, true);

            if (shouldExportDestinationtListItems == true && System.IO.File.Exists(destinationExportFilePath) == true)
                System.IO.File.Delete(destinationExportFilePath);

            if (System.IO.File.Exists(sourceExportDocumentsFolderPath) == false)
                System.IO.Directory.CreateDirectory(sourceExportDocumentsFolderPath);

            sourceExportDocumentsFolderPath = sourceExportDocumentsFolderPath + "\\" + syncTask.DestinationRootFolderPath;
            if (System.IO.File.Exists(sourceExportDocumentsFolderPath) == false)
                System.IO.Directory.CreateDirectory(sourceExportDocumentsFolderPath);

            string destinationExportDocumentsFolderPath = string.Empty;

            //ProcessSyncTaskExport(sourceExportFilePath, sourceExportDocumentsFolderPath, syncTask.SourceSiteSetting, syncTask.SourceListName, syncTask.SourceFieldMappings);

            backgroundWorker.ReportProgress(10, "Exporting source items...");
            ProcessSyncTaskExport(sourceExportFilePath, sourceExportDocumentsFolderPath, syncTask.SourceQueryResultMapping, syncTask.DestinationFieldMappings, shouldExportSourceListItems, shouldSourceExportDocuments, backgroundWorker, lastProcessStartDate, includeVersionsLimit, syncTask.DestinationListName);
            backgroundWorker.ReportProgress(100, "Exporting source items completed");

            QueryResultMappings queryResultMappings = new QueryResultMappings();
            List<QueryResultMappingSelectField> queryResultMappingSelectFields = new List<QueryResultMappingSelectField>();
            foreach (QueryResultMappingSelectField queryResultMappingSelectField in syncTask.DestinationFieldMappings)
            {
                if (string.IsNullOrEmpty(queryResultMappingSelectField.FieldName) == true)
                    continue;

                queryResultMappingSelectFields.Add(new QueryResultMappingSelectField(queryResultMappingSelectField.FieldName, string.Empty, queryResultMappingSelectField.FieldName, queryResultMappingSelectField.ValueTransformationSyntax));
            }
            QueryResult queryResult = new QueryResult()
            {
                Fields = syncTask.DestinationFieldMappings.Where(t=>string.IsNullOrEmpty(t.FieldName) == false).Select(t=>t.FieldName).ToArray(),
                ListName = syncTask.DestinationListName,
                FolderPath = syncTask.DestinationFolderPath,
                SiteSetting = syncTask.DestinationSiteSetting,
                PrimaryIdFieldName = syncTask.DestinationPrimaryIdFieldName
            };
            QueryResultMapping queryResultMapping = new QueryResultMapping()
            {
                DestinationFilterField = string.Empty,
                QueryResult = queryResult,
                SelectFields = queryResultMappingSelectFields.ToArray(),
                SourceFilterField = string.Empty
            };
            queryResultMappings.Mappings.Add(queryResultMapping);

            backgroundWorker.ReportProgress(10, "Exporting destination items...");
            ProcessSyncTaskExport(destinationExportFilePath, string.Empty, queryResultMappings, syncTask.DestinationFieldMappings, shouldExportDestinationtListItems, false, backgroundWorker, null, 1, syncTask.DestinationListName);
            backgroundWorker.ReportProgress(100, "Exporting destination items completed");
        }

        public void ProcessSyncTaskExportFiles(SyncTaskListItemsCopy syncTask, BackgroundWorker backgroundWorker)
        {
            backgroundWorker.ReportProgress(1, "Processing source and destination exports...");
            string folderPath = ConfigurationManager.GetInstance().GetSyncTaskFolder(syncTask);
            string sourceExportFilePath = folderPath + "\\SourceExport.xlsx";
            string destinationExportFilePath = folderPath + "\\DestinationExport.xlsx";
            string processedExportFilePath = folderPath + "\\ProcessedExport.xlsx";
            SLExcelReader reader = new SLExcelReader();
            SLExcelData sourceData = reader.ReadExcel(sourceExportFilePath, true);
            SLExcelData destinationData = reader.ReadExcel(destinationExportFilePath, true);
            SLExcelData processedData = new SLExcelData();
            List<string> headerRow = new List<string>();
            foreach (string headerName in destinationData.Headers)
            {
                headerRow.Add(headerName);
            }
            processedData.DataRows.Add(headerRow);
            Logger.Info("Processing items count:" + sourceData.DataRows.Count, "ProcessSyncTaskExportFiles");

            for (int i=0;i< sourceData.DataRows.Count;i++)
            {
                Logger.Info("Processing item indes:" + i, "ProcessSyncTaskExportFiles");

                int percentage = Convert.ToInt32((float)i / (float)sourceData.DataRows.Count * 100);
                backgroundWorker.ReportProgress(percentage, "Processing item...");

                List<string> sourceDataItem = sourceData.DataRows[i];
                if (syncTask.IsDestinationDocumentLibrary == true)
                {
                    string filePath = sourceDataItem[4];
                    if (string.IsNullOrEmpty(filePath) == true)
                        continue;
                }

                List<string> matchedDestinationDataItem = null;
                if (syncTask.SourceUniqueFieldHeaderNames.Length > 0)
                {
                    string sourceUniqueString = GetUniqueItemValue(sourceDataItem, syncTask.SourceUniqueFieldHeaderNames, sourceData.Headers.ToArray());
                    foreach (List<string> destinationDataItem in destinationData.DataRows)
                    {
                        string destionationUniqueString = GetUniqueItemValue(destinationDataItem, syncTask.DestinationUniqueFieldNames, destinationData.Headers.ToArray());
                        if (destionationUniqueString.Equals(sourceUniqueString) == true)
                        {
                            matchedDestinationDataItem = destinationDataItem;
                            continue;
                        }
                    }
                }

                List<string> dataRow = GenerateDataRow(sourceDataItem, matchedDestinationDataItem, syncTask.SourceFieldHeaderMappings, sourceData.Headers.ToArray(), syncTask.DestinationFieldMappings, destinationData.Headers.ToArray());
                if (matchedDestinationDataItem == null)
                {
                    dataRow[0] = "Add";
                }
                else
                {
                    dataRow[0] = "Update";
                    dataRow[3] = matchedDestinationDataItem[3];
                }
                processedData.DataRows.Add(dataRow);
            }

            SLExcelWriter writer = new SLExcelWriter();
            byte[] excelData = writer.GenerateExcel(processedData);
            using (var fs = new FileStream(processedExportFilePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(excelData, 0, excelData.Length);
            }
        }

        private List<string> GenerateDataRow(List<string> sourceDataItem, List<string> destionationDataItem, string[] sourceFieldNames, string[] sourceHeaders, List<QueryResultMappingSelectField> destinationFieldNames, string[] destinationHeaders)
        {
            List<string> dataRow = new List<string>();
            dataRow.Add("");
            dataRow.Add("");
            dataRow.Add(sourceDataItem[2]);
            dataRow.Add(sourceDataItem[3]);
            dataRow.Add(sourceDataItem[4]);
            dataRow.Add(sourceDataItem[5]);
            dataRow.Add(sourceDataItem[6]);
            dataRow.Add(sourceDataItem[7]);
            dataRow.Add(sourceDataItem[8]);
            int currentMappedColumnIndex = -1;
            for (int i = 0; i < destinationFieldNames.Count; i++)
            {
                if (string.IsNullOrEmpty(destinationFieldNames[i].FieldName) == false)
                {
                    string value = string.Empty;
                    currentMappedColumnIndex++;
                    int sourceColumnIndex = GetHeaderColumnIndex(sourceFieldNames[currentMappedColumnIndex], sourceHeaders);
                    if (sourceDataItem.Count > sourceColumnIndex)
                        value = sourceDataItem[sourceColumnIndex];
                    dataRow.Add(value);
                }
            }

            for (int i = 0; i < destinationFieldNames.Count; i++)
            {
                if (string.IsNullOrEmpty(destinationFieldNames[i].FieldName) == true)
                {
                    dataRow.Add(destinationFieldNames[i].StaticValue);
                }
            }

            return dataRow;
        }

        private int GetHeaderColumnIndex(string headerName, string[] headers) {
            for (int i = 0; i < headers.Length; i++)
            {
                if (headerName.Equals(headers[i]) == true)
                {
                    return i;
                }
            }

            return -1;
        }

        private string GetUniqueItemValue(List<string> data, string[] fieldNames, string[] headers)
        {
            string sourceUniqueString = string.Empty;
            foreach (string fieldName in fieldNames)
            {
                int columnIndex = GetHeaderColumnIndex(fieldName, headers);
                /*
                if(columnIndex == -1)
                {
                    columnIndex = GetHeaderColumnIndex("[" + fieldName + "]", headers);
                }
                */

                if (columnIndex < data.Count)
                    sourceUniqueString += data[columnIndex];
            }

            return sourceUniqueString;
        }

        public void SyncSchema(BackgroundWorker backgroundWorker, List<Entities.Folder> sourceObjects, Entities.Folder destinationObject) {
            foreach(Entities.Folder sourceObject in sourceObjects)
            {
                SyncSchema(backgroundWorker, sourceObject, destinationObject);
            }
        }

        public void SyncSchema(BackgroundWorker backgroundWorker, Entities.Folder sourceObject, Entities.Folder destinationObject)
        {
            ISiteSetting sourceSiteSetting = ApplicationContext.Current.GetSiteSetting(sourceObject.SiteSettingID);
            ISiteSetting destinationSiteSetting = ApplicationContext.Current.GetSiteSetting(destinationObject.SiteSettingID);
            IServiceManager sourceServiceManager = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType);
            IServiceManager destinationServiceManager = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType);

            List<Entities.Folder> destinationLists = destinationServiceManager.GetFolders(destinationSiteSetting, destinationObject);
            List<Entities.Folder> sourceLists;
            if (sourceObject as SPWeb != null)
            {
                sourceLists = sourceServiceManager.GetFolders(sourceSiteSetting, sourceObject);
            }
            else
            {
                sourceLists = new List<Entities.Folder>();
                sourceLists.Add((SPList)sourceObject);
            }

            foreach (SPList sourceList in sourceLists)
            {
                Entities.Folder matchList = destinationLists.Where(t => t.GetListName() == sourceList.GetListName()).FirstOrDefault();
                if (matchList == null)
                {
                    matchList = destinationServiceManager.CreateFolder(destinationSiteSetting, sourceList.Title, ((SPList)sourceList).ServerTemplate);
                    destinationLists.Add(matchList);
                }

                SyncFields(sourceSiteSetting, sourceList, destinationSiteSetting, matchList);
            }

            foreach (SPList sourceList in sourceLists)
            {
                Entities.Folder destinationList = destinationLists.Where(t => t.GetListName() == sourceList.GetListName()).FirstOrDefault();
                SyncData(backgroundWorker, sourceSiteSetting, sourceList, destinationSiteSetting, destinationList);
            }
        }

        public void SyncData(BackgroundWorker backgroundWorker, ISiteSetting sourceSiteSetting, Entities.Folder sourceObject, ISiteSetting destinationSiteSetting, Entities.Folder destinationObject)
        {
            IServiceManager sourceServiceManager = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType);
            IServiceManager destinationServiceManager = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType);

            List<Entities.Field> destinationFields = destinationServiceManager.GetFields(destinationSiteSetting, destinationObject);
            List<Entities.Field> sourceFields = sourceServiceManager.GetFields(sourceSiteSetting, sourceObject);


            List<QueryResultMappingSelectField> destinationFieldMappings = new List<QueryResultMappingSelectField>();
            List<string> sourceFieldNames = new List<string>();
            List<string> destinationFieldNames = new List<string>();
            foreach (Field destionationField in destinationFields)
            {
                if (destionationField.ReadOnly == true/* && destionationField.Name.Equals("Title", StringComparison.InvariantCultureIgnoreCase) == false*/)
                    continue;

                Field sourceField = sourceFields.Where(t=>t.Name==destionationField.Name).FirstOrDefault();
                sourceFieldNames.Add(sourceField.Name);
                destinationFieldNames.Add(destionationField.Name);
                destinationFieldMappings.Add(new QueryResultMappingSelectField(destionationField.Name, string.Empty, destionationField.Name));
            }
            SyncTaskListItemsCopy syncTaskListItemsCopy = SyncTaskListItemsCopy.NewSyncTask();
            syncTaskListItemsCopy.DestinationListName = destinationObject.GetListName();
            syncTaskListItemsCopy.DestinationRootFolderPath = destinationObject.GetPath();
            syncTaskListItemsCopy.DestinationFolderPath = destinationObject.GetPath();
            syncTaskListItemsCopy.Name = destinationObject.GetListName();
            syncTaskListItemsCopy.DestinationPrimaryIdFieldName = destinationObject.PrimaryIdFieldName;
            syncTaskListItemsCopy.DestinationPrimaryNameFieldName = destinationObject.PrimaryNameFieldName;
            syncTaskListItemsCopy.DestinationPrimaryFileReferenceFieldName = destinationObject.PrimaryFileReferenceFieldName;
            syncTaskListItemsCopy.SourceUniqueFieldHeaderNames = new string[] { };
            syncTaskListItemsCopy.DestinationUniqueFieldNames = new string[] { };
            syncTaskListItemsCopy.DestinationIDFieldHeaderName = destinationObject.PrimaryIdFieldName;
            syncTaskListItemsCopy.SourceFieldHeaderMappings = sourceFieldNames.ToArray();
            syncTaskListItemsCopy.DestinationFieldMappings = destinationFieldMappings;
            syncTaskListItemsCopy.IsDestinationDocumentLibrary = destinationObject.IsDocumentLibrary;
            syncTaskListItemsCopy.DestinationTermStoreName = "Taxonomy_BrjLUNqY3/3gqp8FAbbKiQ==";
            syncTaskListItemsCopy.DestinationSiteSetting = (SiteSetting)destinationSiteSetting;

            QueryResultMapping test1QueryResultMapping = new QueryResultMapping();
            test1QueryResultMapping.QueryResult = new QueryResult();
            test1QueryResultMapping.QueryResult.Fields = sourceFieldNames.ToArray();
            test1QueryResultMapping.QueryResult.ListName = sourceObject.GetListName();
            test1QueryResultMapping.QueryResult.Name = sourceObject.GetListName();
            test1QueryResultMapping.QueryResult.SiteSetting = (SiteSetting)sourceSiteSetting;

            List<QueryResultMappingSelectField> _fields = new List<QueryResultMappingSelectField>();
            foreach (string fieldName in sourceFieldNames)
            {
                _fields.Add(new QueryResultMappingSelectField(fieldName, fieldName));
            }
            test1QueryResultMapping.SelectFields = _fields.ToArray();
            syncTaskListItemsCopy.SourceQueryResultMapping.Mappings.Add(test1QueryResultMapping);

            SyncTasksManager.GetInstance().ExportSyncTaskItems(syncTaskListItemsCopy, true, true, true, backgroundWorker, null, 0);
            SyncTasksManager.GetInstance().ProcessSyncTaskExportFiles(syncTaskListItemsCopy, backgroundWorker);
            SyncTasksManager.GetInstance().ImportSyncTaskItems(syncTaskListItemsCopy, syncTaskListItemsCopy.ShouldSkipUpdates, new string[] { }, backgroundWorker);

        }

        public void SyncFields(ISiteSetting sourceSiteSetting, Entities.Folder sourceObject, ISiteSetting destinationSiteSetting, Entities.Folder destinationObject)
        {
            IServiceManager sourceServiceManager = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType);
            IServiceManager destinationServiceManager = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType);

            List<Entities.Field> destinationFields = destinationServiceManager.GetFields(destinationSiteSetting, destinationObject);
            List<Entities.Field> sourceFields = sourceServiceManager.GetFields(sourceSiteSetting, sourceObject);

            List<Entities.Field> fieldsToBeAdded = new List<Entities.Field>();
            foreach (Entities.Field sourceField in sourceFields)
            {
                Entities.Field matchField = destinationFields.Where(t => t.Name == sourceField.Name).FirstOrDefault();
                if (matchField == null)
                {
                    fieldsToBeAdded.Add(sourceField);
                }
            }

            destinationServiceManager.CreateFields(destinationSiteSetting, sourceObject, fieldsToBeAdded);
        }

        public void ImportSyncTaskItems(SyncTaskListItemsCopy syncTask, bool shouldSkipUpdates, string[] excludeFields, BackgroundWorker backgroundWorker)
        {
            backgroundWorker.ReportProgress(1, "Importing items...");
            string folderPath = ConfigurationManager.GetInstance().GetSyncTaskFolder(syncTask);
            string sourceDocumentsFolderPath = folderPath + "\\SourceDocuments";
            string processedExportFilePath = folderPath + "\\ProcessedExport.xlsx";
            string syncTaskStatusFilePath = folderPath + "\\SyncTaskStatus.xml";
            SLExcelReader reader = new SLExcelReader();
            SLExcelData processedData = reader.ReadExcel(processedExportFilePath, true);
            if(ProcessData.ContainsKey(syncTask.ID.ToString())==true)
            {
                ProcessData.Remove(syncTask.ID.ToString());
            }

            ProcessData.Add(syncTask.ID.ToString(), processedData);

            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(syncTask.DestinationSiteSetting.SiteSettingType);
            Entities.Folder folder = null;
            if(syncTask.DestinationSiteSetting.SiteSettingType == SiteSettingTypes.SharePoint)
            {
                folder = new SPFolder()
                {
                    RootFolderPath = syncTask.DestinationRootFolderPath,
                    FolderPath = syncTask.DestinationRootFolderPath,
                    WebUrl = syncTask.DestinationSiteSetting.Url,
                    SiteUrl = syncTask.DestinationSiteSetting.Url,
                    ListName = syncTask.DestinationListName,
                    BaseType = 1
                };

            }
            else if (syncTask.DestinationSiteSetting.SiteSettingType == SiteSettingTypes.SQLServer)
            {
                string dbName = syncTask.DestinationFolderPath.Substring(0, syncTask.DestinationFolderPath.LastIndexOf(syncTask.DestinationListName)-1);
                folder = new SQLTable(syncTask.DestinationListName, syncTask.DestinationSiteSetting.ID, Guid.NewGuid().ToString(), dbName)
                {
                    RootFolderPath = syncTask.DestinationRootFolderPath,
                    //FolderPath = syncTask.DestinationRootFolderPath,
                    //WebUrl = syncTask.DestinationSiteSetting.Url,
                    //SiteUrl = syncTask.DestinationSiteSetting.Url,
                    ListName = syncTask.DestinationListName,
                    //BaseType = 1
                };

            }
            else if (syncTask.DestinationSiteSetting.SiteSettingType == SiteSettingTypes.SQLServer)
            {
                folder = new CRMEntity()
                {
                    //RootFolderPath = syncTask.DestinationRootFolderPath,
                    FolderPath = syncTask.DestinationRootFolderPath,
                    WebUrl = syncTask.DestinationSiteSetting.Url,
                    SiteUrl = syncTask.DestinationSiteSetting.Url,
                    Title = syncTask.DestinationListName,
                    //BaseType = 1
                };

            }

            Entities.FieldCollection fields = serviceManager.GetFields(syncTask.DestinationSiteSetting, folder);


            for (int y=0;y< processedData.DataRows.Count;y++)
            {
                int percentage = Convert.ToInt32((float)(y) / (float)processedData.DataRows.Count * 100);

                List<string> dataRow = processedData.DataRows[y];
                backgroundWorker.ReportProgress(percentage, dataRow);
                object[] args = new object[] { syncTask, shouldSkipUpdates, dataRow, processedData, fields, excludeFields, sourceDocumentsFolderPath };
                string[] returnValue = ImportItem(args);
                processedData.DataRows[y][1] = returnValue[0];
                processedData.DataRows[y][2] = returnValue[1];
                //QueueImportItem(syncTask, shouldSkipUpdates, dataRow, processedData, fields);
            }

            SLExcelWriter writer = new SLExcelWriter();
            byte[] excelData = writer.GenerateExcel(processedData);
            using (var fs = new FileStream(processedExportFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(excelData, 0, excelData.Length);
            }
            backgroundWorker.ReportProgress(100, "Importing completed");
        }

        private void QueueImportItem(SyncTask syncTask, bool shouldSkipUpdates, List<string> dataRow, SLExcelData processedData, Entities.FieldCollection fields)
        {
            object[] args = new object[] { syncTask, shouldSkipUpdates, dataRow, processedData, fields };
            //System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(ImportItem), args);
            QueueLength++;
        }

        private string[] ImportItem(object _args)
        {
            string[] returnValue = new string[] { "Completed", "" };
            try
            {
                
                
                object[] args = (object[])_args;
                SyncTaskListItemsCopy syncTask = (SyncTaskListItemsCopy)args[0];
                bool shouldSkipUpdates = (bool)args[1];
                List<string> dataRow = (List<string>)args[2];
                SLExcelData processedData = (SLExcelData)args[3];
                Entities.FieldCollection fields = (Entities.FieldCollection)args[4];
                string[] excludeFields = (string[])args[5];
                string sourceDocumentsFolderPath = args[6].ToString();
                System.Collections.Generic.Dictionary<string, object> auditInformation = new Dictionary<string, object>();
                auditInformation.Add("Editor", dataRow[5]);
                auditInformation.Add("Modified", dataRow[6]);
//                auditInformation.Add("IsFolder", dataRow[7]);

                IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(syncTask.DestinationSiteSetting.SiteSettingType);
                Dictionary<object, object> values = new Dictionary<object, object>();

                for (int i = 0; i < syncTask.DestinationFieldMappings.Count; i++)
                {
                    Sobiens.Connectors.Entities.Field field = new Sobiens.Connectors.Entities.Field();
                    field.Name = syncTask.DestinationFieldMappings[i].OutputHeaderName;
                    field.DisplayName = syncTask.DestinationFieldMappings[i].OutputHeaderName;
                    field.Type = FieldTypes.Text;
                    string destinationFieldName = syncTask.DestinationFieldMappings[i].OutputHeaderName;
                    int destinationColumnIndex = GetHeaderColumnIndex(destinationFieldName, processedData.Headers.ToArray());
                    string value = dataRow[destinationColumnIndex];
                    Sobiens.Connectors.Entities.Field currentField = fields.Where(t => t.Name.Equals(destinationFieldName)).FirstOrDefault();
                    if (currentField == null && destinationFieldName!="ContentType")
                        continue;
                    if (excludeFields.Contains(destinationFieldName) == true)
                        continue;

                    if (string.IsNullOrEmpty(syncTask.DestinationFieldMappings[i].ValueTransformationSyntax) == false)
                    {
                        object transformedValue = ValueTransformationHelper.Transform(value, syncTask.DestinationFieldMappings[i].ValueTransformationSyntax);
                        if (transformedValue != null)
                            value = transformedValue.ToString();
                        else
                            value = string.Empty;
                    }

                    if (currentField == null)
                    {
                        // no need to do anything
                    }
                    else if (currentField.Type == FieldTypes.Lookup)
                    {
                        if (string.IsNullOrEmpty(value) == false)
                        {
                            string[] lookupValues = value.Split(new string[] { ";#" }, StringSplitOptions.None);
                            string newValue = string.Empty;
                            for (int x = 0; x < lookupValues.Length; x = x + 2)
                            {
                                List<CamlFieldRef> viewFields = new List<CamlFieldRef>();
                                viewFields.Add(new CamlFieldRef("ID", "ID"));
                                viewFields.Add(new CamlFieldRef("Title", "Title"));
                                List<CamlOrderBy> orderBys = new List<CamlOrderBy>();
                                CamlQueryOptions queryOptions = new CamlQueryOptions() { RowLimit = 10000 };
                                CamlFilters filters = new CamlFilters();
                                filters.Add(new CamlFilter("Title", FieldTypes.Text, CamlFilterTypes.Equals, lookupValues[x + 1]));
                                List<Entities.Interfaces.IItem> items = serviceManager.GetListItemsWithoutPaging(syncTask.DestinationSiteSetting, orderBys, filters, viewFields, queryOptions, syncTask.DestinationSiteSetting.Url, currentField.List);
                                if (items.Count > 0)
                                {
                                    if (string.IsNullOrEmpty(newValue) == false)
                                        newValue += ";#";
                                    newValue += items[0].Properties["ID"] + ";#" + items[0].Properties["Title"];
                                }
                                else
                                {
                                    Logger.Info("No item returned for " + value, "Service");
                                }
                            }
                            Logger.Info("Lookup value replace - old value:" + value + " newvalue:" + newValue, "Service");

                            value = newValue;
                        }
                    }
                    else if (currentField.Type == FieldTypes.User)
                    {
                        if (value.IndexOf(";#") > -1)
                        {
                            //19;#i:0#.f|membership|emma.chorley@core.co.uk;#25;#i:0#.f|membership|serkant.samurkas@core.co.uk
                            string newValue = string.Empty;
                            string[] values1 = value.Split(new string[] { ";#" }, StringSplitOptions.None);
                            for (int v = 0; v < values1.Length; v = v + 2)
                            {
                                if (v > 0)
                                    newValue += ";#";
                                newValue += "-1;#" + values1[v + 1];
                            }
                            value = newValue;
                        }
                    }
                    else if (currentField.Type == FieldTypes.TaxonomyFieldType)
                    {
                        Guid termStoreId, termGroupId, termSetId, termId;
                        new Services.SharePoint.SharePointService().GetTermValuesByPath(syncTask.DestinationSiteSetting, value, out termStoreId, out termGroupId, out termSetId, out termId);
                        string termLabel = value.Split(new char[] { ';' })[0];
                        value = "-1;#" + termLabel + "|" + termId; // GetTaxonomyValue(ctx, syncTask.DestinationTermStoreName, ((SPTaxonomyField)currentField).TermSetId, value);
                    }
                    else if (currentField.Type == FieldTypes.Text || currentField.Type == FieldTypes.Note || currentField.Type == FieldTypes.Choice)
                    {
                        if (currentField.Mult == false && value.IndexOf(";#") > -1)
                        {
                            value = value.Split(new string[] { ";#" }, StringSplitOptions.None)[1];
                        }
                    }
                    else if (currentField.Type == FieldTypes.DateTime && string.IsNullOrEmpty(value) == true)
                    {
                        continue;
                    }
                    values.Add(field, value);
                }

                string action = dataRow[0];
                string webUrl = syncTask.DestinationSiteSetting.Url;
                if(syncTask.DestinationSiteSetting.SiteSettingType == SiteSettingTypes.SQLServer)
                {
                    webUrl = syncTask.DestinationFolderPath.Substring(0, syncTask.DestinationFolderPath.LastIndexOf(syncTask.DestinationListName)-1);
                }

                if (action.Equals("Add") == true)
                {
                    string listName = dataRow[8];
                    bool isFolder = dataRow[7] == "1"?true:false;
                    if (isFolder == true)
                    {
                        string folderRelativePath = dataRow[4];
                        string folderName = folderRelativePath.Substring(folderRelativePath.LastIndexOf("\\") + 1);
                        string folderPath = folderRelativePath.Substring(0, folderRelativePath.LastIndexOf("\\"));

                        serviceManager.AddFolder(syncTask.DestinationSiteSetting, webUrl, folderName, folderPath, listName);
                    }
                    else if (syncTask.IsDestinationDocumentLibrary == true)
                    {
                        UploadItem uploadItem = new UploadItem();
                        uploadItem.FilePath = sourceDocumentsFolderPath + "\\" + dataRow[4];
                        //uploadItem.FieldInformations = new Dictionary<object, object>() ;
                        uploadItem.FieldInformations = values;
                        uploadItem.Folder = new SPFolder()
                        {
                            RootFolderPath = syncTask.DestinationRootFolderPath,
                            FolderPath = syncTask.DestinationRootFolderPath,
                            WebUrl = syncTask.DestinationSiteSetting.Url,
                            SiteUrl = syncTask.DestinationSiteSetting.Url,
                            ListName = syncTask.DestinationListName,
                            BaseType = 1
                        };
                        Entities.Interfaces.IItem item = null;
                        Logger.Info("Uploading " + uploadItem.FilePath + " ...", "Service");
                        serviceManager.UploadFile(syncTask.DestinationSiteSetting, uploadItem, false, out item);
                        serviceManager.UpdateListItem(syncTask.DestinationSiteSetting, webUrl, syncTask.DestinationListName, int.Parse(item.GetID()), values, auditInformation);
                    }
                    else
                    {
                        Logger.Info("Creating list item... ", "Service");
                        serviceManager.CreateListItem(syncTask.DestinationSiteSetting, webUrl, syncTask.DestinationListName, values);
                    }
                }
                else if (action.Equals("Update") == true && shouldSkipUpdates == false)
                {
                    int listItemId = int.Parse(dataRow[3]);
                    Logger.Info("Updating list item listItemId:" + listItemId + " ... ", "Service");
                    serviceManager.UpdateListItem(syncTask.DestinationSiteSetting, webUrl, syncTask.DestinationListName, listItemId, values, auditInformation);
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex, "Service");
                returnValue = new string[] { "Failed", "Error:" + ex.Message + " - StackTrace:" + ex.StackTrace };
            }
            QueueLength--;

            return returnValue;
        }

        private SiteSetting GetTestSiteSetting()
        {
            return new SiteSetting()
            {
                ID = Guid.NewGuid(),
                SiteSettingType = SiteSettingTypes.SharePoint,
                Url = "https://coretechnology.sharepoint.com/sites/Projects",
                UseClaimAuthentication = true,
                UseDefaultCredential = false,
                Username = "serkant.samurkas@core.co.uk",
                Password = "Trinity103"
            };
        }

        public List<SyncTaskListItemsCopy> GetTestListItemSyncTasks()
        {
            //List<SyncTask> syncTasks = SyncTasksManager.GetInstance().SyncTasks;
            List<SyncTaskListItemsCopy> syncTasks = new List<SyncTaskListItemsCopy>();
            SyncTaskListItemsCopy syncTask = SyncTaskListItemsCopy.NewSyncTask();
            syncTask.ID = new Guid("{8AD5BE50-1DB4-4C64-B8F2-833A48F02EC2}");
            syncTask.ScheduleInterval = 240;

            QueryResult test1QueryResult = new QueryResult();
            test1QueryResult.Fields = new string[] { "ID", "Title", "_x0054_L1", "User1", "_x004d_MS1", "Deadline" };
            test1QueryResult.ListName = "Test1";
            test1QueryResult.Name = "Test1";
            test1QueryResult.SiteSetting = GetTestSiteSetting();
            test1QueryResult.Filters = new CamlFilters();
            /*
            test1QueryResult.Filters.IsOr = true;
            test1QueryResult.Filters.Add(new CamlFilter()
            {
                FieldName = "Title",
                FieldType = FieldTypes.Text,
                FilterType = CamlFilterTypes.Equals,
                FilterValue = "serkant"
            });
            */

            test1QueryResult.ReferenceFields.Add(new QueryResultReferenceField()
            {
                SiteSetting = GetTestSiteSetting(),
                ReferenceFilterFieldName = "FilterValue",
                ReferenceListName = "TestLookup",
                ReferenceValueFieldName = "DataValue",
                SourceFieldName = "RefField",
                OutputName = "RefField1"
            });

            QueryResultMapping test1QueryResultMapping = new QueryResultMapping();
            test1QueryResultMapping.QueryResult = test1QueryResult;
            test1QueryResultMapping.SelectFields = new QueryResultMappingSelectField[]
            {
                new QueryResultMappingSelectField("ID", "ID"),
                new QueryResultMappingSelectField("Title", "Title"),
                new QueryResultMappingSelectField("_x0054_L1", "_x0054_L1"),
                new QueryResultMappingSelectField("RefField1", "RefField"),
                new QueryResultMappingSelectField("User1", "User1"),
                new QueryResultMappingSelectField("_x004d_MS1", "_x004d_MS1"),
                new QueryResultMappingSelectField("Deadline", "Deadline")
            };

            syncTask.SourceQueryResultMapping.Mappings.Add(test1QueryResultMapping);

            syncTask.DestinationListName = "Test2";
            syncTask.DestinationRootFolderPath = "Test2";
            syncTask.Name = "Test2";
            syncTask.SourceUniqueFieldHeaderNames = new string[] { "ID" };
            syncTask.DestinationUniqueFieldNames = new string[] { "SourceItemID" };
            syncTask.DestinationIDFieldHeaderName = "ID";

            syncTask.SourceFieldHeaderMappings = new string[] { "ID", "Title", "_x0054_L1", "RefField", "User1", "_x004d_MS1", "Deadline" };
            //syncTask.DestinationFieldMappings = new string[] { "SourceItemID", "Title", "_x0054_L1", "RefField", "User1", "_x004d_MS1", "Deadline" };
            syncTask.IsDestinationDocumentLibrary = false;
            syncTask.DestinationTermStoreName = "Taxonomy_BrjLUNqY3/3gqp8FAbbKiQ==";
            syncTask.DestinationSiteSetting = GetTestSiteSetting();
            syncTasks.Add(syncTask);
            SyncTasksManager.GetInstance().SyncTasks.Clear();
            SyncTasksManager.GetInstance().SyncTasks.Add(syncTask);
            SyncTasksManager.GetInstance().SaveSyncTasks();
            return syncTasks;
        }

        public List<SyncTaskListItemsCopy> GetTestDocumentSyncTasks()
        {
            //List<SyncTask> syncTasks = SyncTasksManager.GetInstance().SyncTasks;
            List<SyncTaskListItemsCopy> syncTasks = new List<SyncTaskListItemsCopy>();
            SyncTaskListItemsCopy syncTask = SyncTaskListItemsCopy.NewSyncTask();

            QueryResult test1QueryResult = new QueryResult();
            test1QueryResult.Fields = new string[] { "ID", "Title", "DocumentLocation" };
            test1QueryResult.ListName = "Test1";
            test1QueryResult.Name = "Test1";
            test1QueryResult.SiteSetting = new SiteSetting()
            {
                ID = Guid.NewGuid(),
                SiteSettingType = SiteSettingTypes.SharePoint,
                Url = "https://coretechnology.sharepoint.com/sites/Projects",
                UseClaimAuthentication = true,
                UseDefaultCredential = false,
                Username = "serkant.samurkas@core.co.uk",
                Password = "Trinity103"
            };

            QueryResult documentsQueryResult = new QueryResult();
            documentsQueryResult.Fields = new string[] { "ID", "Title", "FileRef" };
            documentsQueryResult.ListName = "Site Collection Documents";
            documentsQueryResult.Name = "SiteCollectionDocuments";
            documentsQueryResult.Filters = new CamlFilters();
            documentsQueryResult.Filters.IsOr = true;
            documentsQueryResult.Filters.Add(new CamlFilter()
            {
                FieldName="Title",
                FieldType = FieldTypes.Text,
                FilterType = CamlFilterTypes.Equals,
                FilterValue = "serkant"
            });
            documentsQueryResult.SiteSetting = new SiteSetting()
            {
                ID = Guid.NewGuid(),
                SiteSettingType = SiteSettingTypes.SharePoint,
                Url = "https://coretechnology.sharepoint.com/sites/Projects",
                UseClaimAuthentication = true,
                UseDefaultCredential = false,
                Username = "serkant.samurkas@core.co.uk",
                Password = "Trinity103"
            };

            QueryResultMapping test1QueryResultMapping = new QueryResultMapping();
            test1QueryResultMapping.QueryResult = test1QueryResult;
            test1QueryResultMapping.SelectFields = new QueryResultMappingSelectField[]
            {
                new QueryResultMappingSelectField("ID", "Test1ID"),
                new QueryResultMappingSelectField("Title", "Test1Title"),
                new QueryResultMappingSelectField("DocumentLocation", "DocumentLocation")
            };

            QueryResultMapping documentsQueryResultMapping = new QueryResultMapping();
            documentsQueryResultMapping.QueryResult = documentsQueryResult;
            documentsQueryResultMapping.SourceFilterField = "DocumentLocation";
            documentsQueryResultMapping.DestinationFilterField = "$FolderFilter";
            documentsQueryResultMapping.SelectFields = new QueryResultMappingSelectField[]
            {
                new QueryResultMappingSelectField("ID", "DocumentID"),
                new QueryResultMappingSelectField("Title", "DocumentTitle"),
                new QueryResultMappingSelectField("FileRef", "DocumentFileRef")
            };

            syncTask.SourceQueryResultMapping.Mappings.Add(test1QueryResultMapping);
            syncTask.SourceQueryResultMapping.Mappings.Add(documentsQueryResultMapping);

            /*
            syncTask.SourceSiteSetting = new SiteSetting()
            {
                ID = Guid.NewGuid(),
                SiteSettingType = SiteSettingTypes.SharePoint,
                Url = "https://coretechnology.sharepoint.com/sites/Projects",
                UseClaimAuthentication = true,
                UseDefaultCredential = false,
                Username = "serkant.samurkas@core.co.uk",
                Password = "Trinity103"
            };
            syncTask.SourceListName = "Test1";
            syncTask.DestinationSiteSetting = new SiteSetting()
            {
                ID = Guid.NewGuid(),
                SiteSettingType = SiteSettingTypes.SharePoint,
                Url = "https://coretechnology.sharepoint.com/sites/Projects",
                UseClaimAuthentication = true,
                UseDefaultCredential = false,
                Username = "serkant.samurkas@core.co.uk",
                Password = "Trinity103"
            };
            */
            syncTask.DestinationListName = "Test Documents";
            syncTask.DestinationRootFolderPath = "Test Documents";
            syncTask.Name = "Test Documents";
            syncTask.SourceUniqueFieldHeaderNames = new string[] { "DocumentID" };
            syncTask.DestinationUniqueFieldNames = new string[] { "SourceItemID" };
            syncTask.DestinationIDFieldHeaderName = "DocumentID";

            syncTask.SourceFieldHeaderMappings = new string[] { "DocumentID", "Test1Title" };
            //syncTask.DestinationFieldMappings = new string[] { "SourceItemID", "Title" };
            syncTask.IsDestinationDocumentLibrary = true;
            syncTask.DestinationSiteSetting = new SiteSetting()
            {
                ID = Guid.NewGuid(),
                SiteSettingType = SiteSettingTypes.SharePoint,
                Url = "https://coretechnology.sharepoint.com/sites/Projects",
                UseClaimAuthentication = true,
                UseDefaultCredential = false,
                Username = "serkant.samurkas@core.co.uk",
                Password = "Trinity103"
            };
            syncTasks.Add(syncTask);
            //SyncTasksManager.GetInstance().SyncTasks = syncTasks;
            //SyncTasksManager.GetInstance().SaveSyncTasks();
            return syncTasks;
        }

        private void ProcessSyncTaskExport(string filePath, string documentsFolder, QueryResultMappings queryResultMappings, List<QueryResultMappingSelectField> destinationFieldMappings, bool shouldExportSourceListItems, bool shouldSourceExportDocuments, BackgroundWorker backgroundWorker, DateTime? lastProcessStartDate, int includeVersionsLimit /* Positive -> version limit, 1-> last version, 0 -> all versions*/, string destionationListName)
        {
            if (shouldExportSourceListItems == false && shouldSourceExportDocuments == false)
                return;

            bool versioningEnabled = queryResultMappings.Mappings[0].QueryResult.VersioningEnabled;
            SLExcelData data = new SLExcelData();
            List<string> headerRow = new List<string>();
            headerRow.Add("[ImportAction]");
            headerRow.Add("[ImportProcessStatus]");
            headerRow.Add("[ImportProcessMessage]");
            headerRow.Add("[ID]");
            headerRow.Add("[FilePath]");
            headerRow.Add("[Editor]");
            headerRow.Add("[Modified]");
            headerRow.Add("[IsFolder]");
            headerRow.Add("[DestinationListName]");
            for (int i = 0; i < queryResultMappings.Mappings.Count; i++)
            {
                QueryResultMapping queryResultMapping = queryResultMappings.Mappings[i];
                foreach (QueryResultMappingSelectField queryResultMappingSelectField in queryResultMapping.SelectFields)
                {
                    headerRow.Add(queryResultMappingSelectField.FieldName);
                }
            }
            for (int i = 0; i < destinationFieldMappings.Count; i++)
            {
                if(string.IsNullOrEmpty(destinationFieldMappings[i].FieldName) == true)
                    headerRow.Add(destinationFieldMappings[i].OutputHeaderName);
            }
            data.DataRows.Add(headerRow);

            //ProcessSyncTaskExport1(data, documentsFolder, queryResultMappings, 0, -1, headerRow.ToArray(), shouldSourceExportDocuments, backgroundWorker);
            ProcessSyncTaskExport2(data, documentsFolder, queryResultMappings, 0, headerRow.ToArray(), shouldSourceExportDocuments, backgroundWorker, lastProcessStartDate, destionationListName);

            if(includeVersionsLimit != 1 && versioningEnabled == true)
            {
                ProcessSyncTaskExportVersions(data, documentsFolder, documentsFolder, queryResultMappings, 0, headerRow.ToArray(), shouldSourceExportDocuments, backgroundWorker, lastProcessStartDate, destionationListName);
            }


            if (shouldExportSourceListItems == true)
            {
                SLExcelWriter writer = new SLExcelWriter();
                byte[] excelData = writer.GenerateExcel(data);
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(excelData, 0, excelData.Length);
                }
            }
        }

        private void ProcessSyncTaskExport1(SLExcelData data, string documentsFolder, QueryResultMappings queryResultMappings, int queryResultMappingIndex, int parentDataRowIndex, string[] headers, bool shouldSourceExportDocuments, BackgroundWorker backgroundWorker)
        {
            List<string> parentDataRow = null;
            if(parentDataRowIndex>-1)
                parentDataRow = data.DataRows[parentDataRowIndex];
            QueryResultMapping queryResultMapping = queryResultMappings.Mappings[queryResultMappingIndex];

            SiteSetting siteSetting = queryResultMapping.QueryResult.SiteSetting;
            QueryResult queryResult = queryResultMapping.QueryResult;
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            List<CamlFieldRef> viewFields = new List<CamlFieldRef>();
            foreach (string fieldName in queryResultMapping.QueryResult.Fields)
            {
                viewFields.Add(new CamlFieldRef(fieldName, fieldName));
            }
            List<CamlOrderBy> orderBys = new List<CamlOrderBy>();
            CamlQueryOptions queryOptions = new CamlQueryOptions() { RowLimit = 10000 };
            CamlFilters filters = new CamlFilters();
            bool shouldSkip = false;
            if (parentDataRow != null)
            {
                int sourceColumnIndex = GetHeaderColumnIndex(queryResultMapping.SourceFilterField, headers);
                string filterValue = parentDataRow[sourceColumnIndex];
                if (string.IsNullOrEmpty(filterValue) == true)
                {
                    shouldSkip = true;
                }
                else
                {
                    if (queryResultMapping.DestinationFilterField.Equals("$FolderFilter") == true)
                    {
                        string[] vals = filterValue.Split(new string[] { "/" }, StringSplitOptions.None);
                        filterValue = filterValue.Replace(vals[0] + "/" + vals[1] + "/" + vals[2], "");
                        queryOptions.Folder = filterValue;
                        //filters.Add(new CamlFilter(queryResultMapping.DestinationFilterField, FieldTypes.Text, CamlFilterTypes.BeginsWith, filterValue));
                    }
                    else
                    {
                        if (filterValue.IndexOf(";#") > -1)
                            filterValue = filterValue.Split(new string[] { ";#" }, StringSplitOptions.None)[0];

                        filters.Add(new CamlFilter(queryResultMapping.DestinationFilterField, FieldTypes.Text, CamlFilterTypes.Equals, filterValue));
                    }
                }
            }

            if (shouldSkip == false)
            {
                List<Entities.Interfaces.IItem> items = serviceManager.GetListItemsWithoutPaging(siteSetting, orderBys, filters, viewFields, queryOptions, siteSetting.Url, queryResult.ListName);

                for (int i = 0; i < items.Count; i++)
                {
                    //if (i > 0)
                    //    continue;

                    int percentage = Convert.ToInt32((float)i / (float)items.Count * 100);
                    backgroundWorker.ReportProgress(percentage, "Retriving related items ...");

                    Entities.Interfaces.IItem item = items[i];
                    List<string> dataRow = new List<string>();
                    if (parentDataRow != null)
                    {
                        dataRow = parentDataRow.ToList();
                        if (i == 0)
                        {
                            data.DataRows.RemoveAt(parentDataRowIndex);
                        }
                    }
                    else
                    {
                        dataRow.Add("");
                        dataRow.Add(item.Properties["ID"]);
                        dataRow.Add("");
                    }

                    //if (queryResultMapping.QueryResult.Fields.Contains("FileRef") == true)
                    if(queryResultMapping.QueryResult.IsDocumentLibrary == true)
                    {
                        string fileServerRelativeUrl = item.Properties["FileRef"].Split(new string[] { ";#" }, StringSplitOptions.None)[1];
                        string[] vals = siteSetting.Url.Split(new string[] { "/" }, StringSplitOptions.None);
                        string fileUrl = vals[0] + "/" + vals[1] + "/" + vals[2] + "/" + fileServerRelativeUrl;
                        string fileName = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);
                        string documentFilePath = documentsFolder + "\\" + fileName;
                        dataRow[4] = documentFilePath;
                        if (string.IsNullOrEmpty(documentsFolder) == false && queryResultMapping.QueryResult.Fields.Contains("FileRef") == true && shouldSourceExportDocuments == true)
                        {
                            Logger.Info("Downloading " + fileUrl + " => " + documentFilePath, "Document Download");
                            backgroundWorker.ReportProgress(percentage, "Downloading " + fileName + " ...");
                            serviceManager.DownloadFile(siteSetting, fileUrl, documentFilePath);
                        }
                    }

                    foreach (QueryResultMappingSelectField queryResultMappingSelectField in queryResultMapping.SelectFields)
                    {
                        string value = string.Empty;
                        if (item.Properties.ContainsKey(queryResultMappingSelectField.FieldName) == true)
                            value = item.Properties[queryResultMappingSelectField.FieldName].Replace(".00000000000000", "").Replace(".0000000000000", "").Replace(".000000000000", "").Replace(".00000000000", "").Replace(".0000000000", "").Replace(".000000000", "");
                        dataRow.Add(value);
                    }

                    data.DataRows.Add(dataRow);

                    if (queryResultMappingIndex < queryResultMappings.Mappings.Count - 1)
                        ProcessSyncTaskExport1(data, documentsFolder, queryResultMappings, (queryResultMappingIndex + 1), data.DataRows.Count - 1, headers, shouldSourceExportDocuments, backgroundWorker);

                }
            }
        }

        private void ProcessSyncTaskExportVersions(SLExcelData data, string documentsRootFolder, string documentsFolder, QueryResultMappings queryResultMappings, int queryResultMappingIndex, string[] headers, bool shouldSourceExportDocuments, BackgroundWorker backgroundWorker, DateTime? lastProcessStartDate, string destionationListName)
        {
            QueryResultMapping queryResultMapping = queryResultMappings.Mappings[queryResultMappingIndex];

            SiteSetting siteSetting = queryResultMapping.QueryResult.SiteSetting;
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            List<List<string>> dataRows = data.DataRows.ToList();
            for (int i=1;i< dataRows.Count;i++)
            {
                List<string> dataRow = dataRows[i];
                if (dataRow[7] == "1") // It is a folder
                    continue;

                SPListItem item = new SPListItem(siteSetting.ID);

                item.Properties = new Dictionary<string, string>();
                item.ID = int.Parse(dataRow[3]);
                item.WebURL = siteSetting.Url;
                //item.WebPath = dataRow[6];
                if (shouldSourceExportDocuments == true)
                {
                    item.URL = dataRow[11].ToString();
                    if (item.URL.StartsWith("/") == false)
                        item.URL = "/" + item.URL;
                    List<ItemVersion> versions = serviceManager.GetListItemVersions(siteSetting, item);
                    string fileVersionFolder = documentsFolder + "\\Versions";
                    if (Directory.Exists(fileVersionFolder) == false)
                        Directory.CreateDirectory(fileVersionFolder);
                    fileVersionFolder = fileVersionFolder + "\\" + item.ID;
                    if (Directory.Exists(fileVersionFolder) == false)
                        Directory.CreateDirectory(fileVersionFolder);

                    versions = versions.OrderBy(t => t.Version).ToList();
                    for (int c = versions.Count - 1; c > -1; c--)
                    {
                        ItemVersion itemVersion = versions[c];
                        if (itemVersion.Version.Contains("@") == true)
                            continue;

                        string _fileVersionFolder = fileVersionFolder + "\\" + itemVersion.Version;
                        if (Directory.Exists(_fileVersionFolder) == false)
                            Directory.CreateDirectory(_fileVersionFolder);
                        string[] values = itemVersion.URL.Split(new string[] { "/" }, StringSplitOptions.None);

                        _fileVersionFolder = _fileVersionFolder + "\\" + values[values.Length - 1];
                        string relativeFileVersionFolder = documentsRootFolder.Substring(documentsRootFolder.LastIndexOf("\\")+1) + "\\" + _fileVersionFolder.Replace(documentsRootFolder + "\\", "");

                        serviceManager.DownloadFile(siteSetting, itemVersion.URL, _fileVersionFolder);
                        string createdBy = serviceManager.GetUser(siteSetting, itemVersion.CreatedBy);

                        List<string> versionDataRow = new List<string>();
                        versionDataRow.Add("");
                        versionDataRow.Add("");
                        versionDataRow.Add("");
                        versionDataRow.Add(item.ID.ToString());
                        versionDataRow.Add(relativeFileVersionFolder);
                        versionDataRow.Add(createdBy);
                        versionDataRow.Add(itemVersion.CreatedRaw);
                        versionDataRow.Add("0");
                        versionDataRow.Add(destionationListName);
                        data.DataRows.Insert(i, versionDataRow);
                    }
                }
            }
        }

        private string GetModifiedFieldName(SiteSetting siteSetting)
        {
            if (siteSetting.SiteSettingType == SiteSettingTypes.SharePoint)
                return "Modified";
            if (siteSetting.SiteSettingType == SiteSettingTypes.CRM)
                return "modifiedon";

            throw new NotImplementedException();
        }

        private string GetDateTimeFilterString(SiteSetting siteSetting, DateTime date)
        {
            if (siteSetting.SiteSettingType == SiteSettingTypes.SharePoint)
                return date.ToString("yyyy-MM-ddTHH:mm:ssZ");
            if (siteSetting.SiteSettingType == SiteSettingTypes.CRM)
                return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            throw new NotImplementedException();
        }
        private void ProcessSyncTaskExport2(SLExcelData data, string documentsFolder, QueryResultMappings queryResultMappings, int queryResultMappingIndex, string[] headers, bool shouldSourceExportDocuments, BackgroundWorker backgroundWorker, DateTime? lastProcessStartDate, string destionationListName)
        {
            QueryResultMapping queryResultMapping = queryResultMappings.Mappings[queryResultMappingIndex];

            SiteSetting siteSetting = queryResultMapping.QueryResult.SiteSetting;
            QueryResult queryResult = queryResultMapping.QueryResult;
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            //Folder rootFolder = serviceManager.GetRootFolder(siteSetting);
            //Folder rootFolder = serviceManager.get

            List<CamlFieldRef> viewFields = new List<CamlFieldRef>();
            foreach (string fieldName in queryResultMapping.QueryResult.Fields)
            {
                    if (string.IsNullOrEmpty(fieldName) == false)
                    viewFields.Add(new CamlFieldRef(fieldName, fieldName));
            }
            //viewFields.Add(new CamlFieldRef("FileRef", "FileRef"));

            if (queryResultMapping.DestinationFilterField != null && queryResultMapping.DestinationFilterField.Equals("$FolderFilter") == true)
                viewFields.Add(new CamlFieldRef("FileDirRef", "FileDirRef"));

            foreach (QueryResultReferenceField refField in queryResultMapping.QueryResult.ReferenceFields)
            {
                if (viewFields.Where(t => t.Name.Equals(refField.SourceFieldName)).Count() == 0)
                {
                    viewFields.Add(new CamlFieldRef(refField.SourceFieldName, refField.SourceFieldName));
                }
            }

            List<CamlOrderBy> orderBys = new List<CamlOrderBy>();
            CamlQueryOptions queryOptions = new CamlQueryOptions() { RowLimit = 10000 };
            CamlFilters filters = new CamlFilters();
            filters.IsOr = true;
            bool shouldSkip = true;

            if (string.IsNullOrEmpty(queryResultMapping.SourceFilterField) == false)
            {
                for (int i = 1; i < data.DataRows.Count; i++)
                {
                    //if (i > 0)
                    //    continue;
                    int sourceColumnIndex = GetHeaderColumnIndex(queryResultMapping.SourceFilterField, headers);
                    string filterValue = data.DataRows[i][sourceColumnIndex];
                    if (string.IsNullOrEmpty(filterValue) == false)
                    {
                        shouldSkip = false;
                        if (queryResultMapping.DestinationFilterField.Equals("$FolderFilter") == true)
                        {
                            string[] vals = filterValue.Split(new string[] { "/" }, StringSplitOptions.None);
                            filterValue = filterValue.Replace(vals[0] + "/" + vals[1] + "/" + vals[2], "");
                            queryOptions.Scope = "RecursiveAll";
                            filters.Add(new CamlFilter("FileDirRef", FieldTypes.Text, CamlFilterTypes.Equals, filterValue));
                        }
                        else
                        {
                            if (filterValue.IndexOf(";#") > -1)
                                filterValue = filterValue.Split(new string[] { ";#" }, StringSplitOptions.None)[0];

                            filters.Add(new CamlFilter(queryResultMapping.DestinationFilterField, FieldTypes.Text, CamlFilterTypes.Equals, filterValue));
                        }
                    }
                }
            }
            else
            {
                shouldSkip = false;
            }

            if (shouldSkip == false)
            {
                List<Entities.Interfaces.IItem> items = new List<IItem>();
                if (filters.Filters.Count > 0)
                {
                    while (filters.Filters.Count > 0)
                    {
                        Logger.Info("Filters count:" + filters.Filters.Count, "Service");
                        CamlFilters currentFilters = new CamlFilters();
                        currentFilters.IsOr = false;

                        if (filters.Filters.Count > 100)
                        {
                            CamlFilters _filters = new CamlFilters();
                            _filters.IsOr = true;
                            _filters.Filters.AddRange(filters.Filters.GetRange(0, 100));
                            currentFilters.Add(_filters);
                            filters.Filters.RemoveRange(0, 100);
                        }
                        else
                        {
                            CamlFilters _filters = new CamlFilters();
                            _filters.IsOr = true;
                            _filters.Filters.AddRange(filters.Filters);
                            currentFilters.Add(_filters);
                            filters.Filters.Clear();
                        }

                        if (queryResultMapping.QueryResult.Filters != null)
                            if (queryResultMapping.QueryResult.Filters.Filters != null)
                                if (queryResultMapping.QueryResult.Filters.Filters.Count>0)
                                    currentFilters.Add(queryResultMapping.QueryResult.Filters);

                        if (lastProcessStartDate.HasValue == true && lastProcessStartDate != DateTime.MinValue)
                        {
                            string lastProcessStartDateString = GetDateTimeFilterString(siteSetting, lastProcessStartDate.Value);
                            currentFilters.Add(new CamlFilter(GetModifiedFieldName(siteSetting), FieldTypes.DateTime, CamlFilterTypes.EqualsGreater, lastProcessStartDateString));
                        }

                        List<Entities.Interfaces.IItem> _items = serviceManager.GetListItemsWithoutPaging(siteSetting, orderBys, currentFilters, viewFields, queryOptions, siteSetting.Url, queryResult.ListName);
                        items.AddRange(_items);
                    }
                }
                else
                {
                    CamlFilters currentFilters = new CamlFilters();
                    currentFilters.IsOr = false;

                    if (queryResultMapping.QueryResult.Filters != null)
                        if (queryResultMapping.QueryResult.Filters.Filters != null)
                            if (queryResultMapping.QueryResult.Filters.Filters.Count > 0)
                            {
                                filters = queryResultMapping.QueryResult.Filters;

                                CamlFilters _filters = new CamlFilters();
                                _filters.IsOr = true;
                                _filters.Filters.AddRange(filters.Filters);
                                currentFilters.Add(_filters);
                            }

                    if (lastProcessStartDate.HasValue == true && lastProcessStartDate != DateTime.MinValue)
                    {
                        //var result = new DateTimeOffset(input.DateTime, TimeZoneInfo.FindSystemTimeZoneById(input.TimeZoneId).GetUtcOffset(input.DateTime));

                        string lastProcessStartDateString = GetDateTimeFilterString(siteSetting, lastProcessStartDate.Value);
                        currentFilters.Add(new CamlFilter(GetModifiedFieldName(siteSetting), FieldTypes.DateTime, CamlFilterTypes.EqualsGreater, lastProcessStartDateString));
                    }

                    string webUrl = siteSetting.Url;
                    if(siteSetting.SiteSettingType == SiteSettingTypes.SQLServer)
                    {
                        webUrl = queryResult.FolderPath.Substring(0, queryResult.FolderPath.LastIndexOf(queryResult.ListName) - 1);
                    }

                    items = serviceManager.GetListItemsWithoutPaging(siteSetting, orderBys, currentFilters, viewFields, queryOptions, webUrl, queryResult.ListName);
                }

                ApplyReferenceFieldValues(items, queryResultMapping);

                Logger.Info("Filters1 count:" + filters.Filters.Count, "Service");

                if (string.IsNullOrEmpty(queryResultMapping.DestinationFilterField) == true)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        Entities.Interfaces.IItem item = items[i];

                        string primaryId = string.Empty;
                        if (string.IsNullOrEmpty(queryResultMapping.QueryResult.PrimaryIdFieldName) == false)
                            primaryId = item.Properties[queryResultMapping.QueryResult.PrimaryIdFieldName];

                        string modifiedBy = string.Empty;
                        if (string.IsNullOrEmpty(queryResultMapping.QueryResult.ModifiedByFieldName) == false)
                            modifiedBy = item.Properties[queryResultMapping.QueryResult.ModifiedByFieldName];

                        string modifiedOn = string.Empty;
                        if (string.IsNullOrEmpty(queryResultMapping.QueryResult.ModifiedOnFieldName) == false)
                            modifiedOn = item.Properties[queryResultMapping.QueryResult.ModifiedOnFieldName];

                        List<string> dataRow = new List<string>();
                        dataRow.Add("");
                        dataRow.Add("");
                        dataRow.Add("");
                        dataRow.Add(primaryId);
                        dataRow.Add("");
                        dataRow.Add(modifiedBy);
                        dataRow.Add(modifiedOn);
                        dataRow.Add(item.isFolder()==true?"1":"0");
                        /*
                        string webPath = string.Empty;
                        if(item as SPFolder != null)
                        {
                            webPath = ((SPFolder)item).WebRelativePath;
                        }
                        else if (item as SPListItem != null)
                        {
                            webPath = ((SPListItem)item).WebRelativePath;
                        }
                        if (webPath.StartsWith("/") == true)
                            webPath = webPath.Substring(1);
                        */
                        dataRow.Add(destionationListName);

                        DownloadFileAndSetDataRow(serviceManager, dataRow, queryResultMapping, item, siteSetting, documentsFolder, documentsFolder, shouldSourceExportDocuments);
                        SetSelectFieldsData(dataRow, item, queryResultMapping.SelectFields);
                        data.DataRows.Add(dataRow);
                    }
                }
                else
                {
                    Logger.Info("Item returned", "Service");

                    Dictionary<string, List<Entities.Interfaces.IItem>> groupedItems = new Dictionary<string, List<Entities.Interfaces.IItem>>();
                    for (int i = 0; i < items.Count; i++)
                    {
                        string propertyName = queryResultMapping.DestinationFilterField;
                        if (propertyName.Equals("$FolderFilter") == true)
                            propertyName = "FileDirRef";
                        else
                            propertyName = propertyName;

                        Logger.Info("Property search :" + propertyName, "Service");

                        if (items[i].Properties.ContainsKey(propertyName) == false)
                        {
                            Logger.Info("Property could not be found :" + propertyName, "Service");
                            throw new Exception("Property could not be found :" + propertyName);
                        }

                        string filteredValue = items[i].Properties[propertyName];
                        Logger.Info("filteredValue :" + filteredValue, "Service");
                        if (filteredValue.IndexOf(";#") > -1)
                            filteredValue = filteredValue.Split(new string[] { ";#" }, StringSplitOptions.None)[1];
                        Logger.Info("filteredValue1 :" + filteredValue, "Service");
                        if (groupedItems.ContainsKey(filteredValue) == false)
                            groupedItems.Add(filteredValue, new List<Entities.Interfaces.IItem>());

                        groupedItems[filteredValue].Add(items[i]);
                    }


                    for (int x = data.DataRows.Count - 1; x > 0; x--)
                    {
                        string[] dataRowValues = data.DataRows[x].ToArray();

                        foreach (string key in groupedItems.Keys)
                        {
                            List<Entities.Interfaces.IItem> _items = groupedItems[key];

                            List<IItem> relatedItems = new List<IItem>();
                            for (int i = 0; i < _items.Count; i++)
                            {
                                List<string> dataRow = dataRowValues.ToList();

                                Entities.Interfaces.IItem item = _items[i];
                                int sourceColumnIndex = GetHeaderColumnIndex(queryResultMapping.SourceFilterField, headers);
                                string sourceFilteredValue = dataRow[sourceColumnIndex];
                                if (queryResultMapping.DestinationFilterField.Equals("$FolderFilter") == true)
                                {
                                    if (sourceFilteredValue.EndsWith(key) == false)
                                        continue;
                                }
                                else if (sourceFilteredValue.Equals(key) == false)
                                    continue;

                                relatedItems.Add(item);
                            }

                            if (relatedItems.Count > 0)
                            {
                                data.DataRows.RemoveAt(x);
                            }

                            for (int i = 0; i < relatedItems.Count; i++)
                            {
                                List<string> dataRow = dataRowValues.ToList();

                                Entities.Interfaces.IItem item = relatedItems[i];

                                DownloadFileAndSetDataRow(serviceManager, dataRow, queryResultMapping, item, siteSetting, documentsFolder, documentsFolder, shouldSourceExportDocuments);
                                SetSelectFieldsData(dataRow, item, queryResultMapping.SelectFields);
                                data.DataRows.Insert(x, dataRow);
                            }
                        }
                    }
                }

                if (queryResultMappingIndex < queryResultMappings.Mappings.Count - 1)
                    ProcessSyncTaskExport2(data, documentsFolder, queryResultMappings, (queryResultMappingIndex + 1), headers, shouldSourceExportDocuments, backgroundWorker, lastProcessStartDate, destionationListName);
            }
        }

        private void ApplyReferenceFieldValues(List<Entities.Interfaces.IItem> items, QueryResultMapping queryResultMapping)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(SiteSettingTypes.SharePoint);
            List<CamlOrderBy> orderBys1 = new List<CamlOrderBy>();
            CamlQueryOptions queryOptions1 = new CamlQueryOptions() { RowLimit = 10000 };

            foreach (QueryResultReferenceField refField in queryResultMapping.QueryResult.ReferenceFields)
            {
                List<CamlFieldRef> viewFields1 = new List<CamlFieldRef>();
                viewFields1.Add(new CamlFieldRef("ID", "ID"));
                viewFields1.Add(new CamlFieldRef(refField.ReferenceValueFieldName, refField.ReferenceValueFieldName));
                viewFields1.Add(new CamlFieldRef(refField.ReferenceFilterFieldName, refField.ReferenceFilterFieldName));
                CamlFilters filters1 = new CamlFilters();
                filters1.IsOr = true;
                for (int i = 0; i < items.Count; i++)
                {
                    Entities.Interfaces.IItem item = items[i];
                    if (item.Properties[refField.SourceFieldName] == null)
                        continue;

                    string filterValue = item.Properties[refField.SourceFieldName];
                    if (filterValue.IndexOf(";#") > -1)
                        filterValue = filterValue.Split(new string[] { ";#" }, StringSplitOptions.None)[1];

                    filters1.Add(new CamlFilter(refField.ReferenceFilterFieldName, FieldTypes.Text, CamlFilterTypes.Equals, filterValue));

                    if(filters1.Filters.Count> 100 || i == items.Count - 1)
                    {
                        List<Entities.Interfaces.IItem> items1 = serviceManager.GetListItemsWithoutPaging(refField.SiteSetting, orderBys1, filters1, viewFields1, queryOptions1, refField.SiteSetting.Url, refField.ReferenceListName);
                        foreach (Entities.Interfaces.IItem item1 in items1)
                        {
                            string filteredValue = item1.Properties[refField.ReferenceFilterFieldName];
                            string referenceValue = item1.Properties[refField.ReferenceValueFieldName];
                            if (string.IsNullOrEmpty(filteredValue) == true || string.IsNullOrEmpty(referenceValue) == true)
                                continue;

                            List<Entities.Interfaces.IItem> relatedItems = items.Where(t =>
                            t.Properties[refField.SourceFieldName] != null
                            && 
                            (
                                t.Properties[refField.SourceFieldName] == filteredValue
                                ||
                                (
                                    t.Properties[refField.SourceFieldName].IndexOf(";#") > -1
                                    && t.Properties[refField.SourceFieldName].Split(new string[] { ";#" }, StringSplitOptions.None)[1] == filteredValue
                                )
                                )
                            ).ToList();
                            foreach (Entities.Interfaces.IItem relatedItem in relatedItems)
                            {
                                if (relatedItem.Properties.ContainsKey(refField.OutputName) == false)
                                    relatedItem.Properties.Add(refField.OutputName, string.Empty);

                                relatedItem.Properties[refField.OutputName] = referenceValue;
                            }
                        }

                        filters1 = new CamlFilters();
                        filters1.IsOr = true;
                    }
                }
            }

            /*
            List<Entities.Interfaces.IItem> items1 = serviceManager.GetListItems(refField.SiteSetting, orderBys1, filters1, viewFields1, queryOptions1, refField.SiteSetting.Url, refField.ReferenceListName, out listItemCollectionPositionNext, out itemCount);
            if (items1.Count > 0)
            {
                if (string.IsNullOrEmpty(items1[0].Properties["ows_" + refField.ReferenceValueFieldName]) == true)
                    continue;

                if (item.Properties.ContainsKey("ows_" + refField.SourceFieldName) == false)
                    item.Properties.Add("ows_" + refField.SourceFieldName, string.Empty);

                item.Properties["ows_" + refField.OutputName] = items1[0].Properties["ows_" + refField.ReferenceValueFieldName];
            }



            List<Entities.Interfaces.IItem> items = new List<IItem>();
            if (filters.Filters.Count > 0)
            {
                while (filters.Filters.Count > 0)
                {
                    Logger.Info("Filters count:" + filters.Filters.Count, "Service");
                    CamlFilters currentFilters = new CamlFilters();
                    currentFilters.IsOr = false;

                    if (filters.Filters.Count > 100)
                    {
                        CamlFilters _filters = new CamlFilters();
                        _filters.IsOr = true;
                        _filters.Filters.AddRange(filters.Filters.GetRange(0, 100));
                        currentFilters.Add(_filters);
                        filters.Filters.RemoveRange(0, 100);
                    }
                    else
                    {
                        CamlFilters _filters = new CamlFilters();
                        _filters.IsOr = true;
                        _filters.Filters.AddRange(filters.Filters);
                        currentFilters.Add(_filters);
                        filters.Filters.Clear();
                    }

                    if (queryResultMapping.QueryResult.Filters != null)
                        if (queryResultMapping.QueryResult.Filters.Filters != null)
                            if (queryResultMapping.QueryResult.Filters.Filters.Count > 0)
                                currentFilters.Add(queryResultMapping.QueryResult.Filters);

                    List<Entities.Interfaces.IItem> _items = serviceManager.GetListItems(siteSetting, orderBys, currentFilters, viewFields, queryOptions, siteSetting.Url, queryResult.ListName, out listItemCollectionPositionNext, out itemCount);
                    items.AddRange(_items);
                }
                            */

        }

        private void SetSelectFieldsData(List<string> dataRow, IItem item, QueryResultMappingSelectField[] selectFields)
        {
            foreach (QueryResultMappingSelectField queryResultMappingSelectField in selectFields)
            {
                string value = string.Empty;
                if (string.IsNullOrEmpty(queryResultMappingSelectField.FieldName) == true)
                {
                    value = queryResultMappingSelectField.StaticValue;
                }
                else if (item.Properties.ContainsKey(queryResultMappingSelectField.FieldName) == true)
                {
                    value = item.Properties[queryResultMappingSelectField.FieldName].Replace(".00000000000000", "").Replace(".0000000000000", "").Replace(".000000000000", "").Replace(".00000000000", "").Replace(".0000000000", "").Replace(".000000000", "");
                }

                dataRow.Add(value);
            }
        }

        private void DownloadFileAndSetDataRow(IServiceManager serviceManager, List<string> dataRow, QueryResultMapping queryResultMapping, IItem item, SiteSetting siteSetting, string documentsRootFolder, string documentsFolder, bool shouldSourceExportDocuments)
        {
            if (queryResultMapping.QueryResult.IsDocumentLibrary == true)
            {
                string fileServerRelativeUrl = item.Properties["FileRef"].ToString();
                string[] vals = siteSetting.Url.Split(new string[] { "/" }, StringSplitOptions.None);
                string fileUrl = vals[0] + "/" + vals[1] + "/" + vals[2] + "/" + fileServerRelativeUrl;
                string fileName = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);
                string documentFilePath = documentsFolder + "\\" + fileName;
                string documentRelativeFilePath = documentFilePath.Replace(documentsRootFolder + "\\", string.Empty);
                string documentsRootFolderName = documentsRootFolder.Substring(documentsRootFolder.LastIndexOf("\\") + 1);
                string webRelativePath = ((SPListItem)item).WebRelativePath;
                webRelativePath = webRelativePath.Replace("/", "\\");
                if (webRelativePath.StartsWith("\\") == true)
                {
                    webRelativePath = webRelativePath.Substring(1);
                }
                webRelativePath = documentsRootFolderName + webRelativePath.Substring(webRelativePath.LastIndexOf("\\"));

                dataRow[4] = webRelativePath;
                if (string.IsNullOrEmpty(documentsFolder) == false && queryResultMapping.QueryResult.Fields.Contains("FileRef") == true && shouldSourceExportDocuments == true && item.isFolder() == false)
                {
                    Logger.Info("Downloading " + fileUrl + " => " + documentFilePath, "Document Download");
                    //backgroundWorker.ReportProgress(percentage, "Downloading " + fileName + " ...");
                    serviceManager.DownloadFile(siteSetting, fileUrl, documentFilePath);
                }
            }

        }

        /*
        public static string GetTaxonomyValue(ClientContext ctx, string termStoreName, Guid termSetId, string termPath)
        {
            string key = ctx.Url + "_" + termStoreName + "_" + termSetId;
            if (TermValues.ContainsKey(key) == false)
            {
                TermValues[key] = GetTermSetValues(ctx, termStoreName, termSetId);
            }
            Microsoft.SharePoint.Client.Taxonomy.TermCollection termValues = TermValues[key];
            string termLabel = termPath.Trim().ToLower().Replace("&", "＆").Replace(" > ", ";");
            foreach (Microsoft.SharePoint.Client.Taxonomy.Term term in termValues)
            {
                if (term.PathOfTerm.Trim().ToLower().Equals(termLabel, StringComparison.InvariantCultureIgnoreCase))
                {
                    return ";" + term.Name + "|" + term.Id.ToString();
                }
            }

            return string.Empty;
        }

        private static Microsoft.SharePoint.Client.Taxonomy.TermCollection GetTermSetValues(ClientContext ctx, string termStoreName, Guid termSetId)
        {
            try
            {
                Microsoft.SharePoint.Client.Taxonomy.TaxonomySession taxonomySession = Microsoft.SharePoint.Client.Taxonomy.TaxonomySession.GetTaxonomySession(ctx);
                Microsoft.SharePoint.Client.Taxonomy.TermStoreCollection termStores = taxonomySession.TermStores;
                ctx.Load(termStores);
                ctx.ExecuteQuery();

                Microsoft.SharePoint.Client.Taxonomy.TermStore termStore = termStores[0];

                Microsoft.SharePoint.Client.Taxonomy.TermSet termSet = termStore.GetTermSet(termSetId);
                ctx.Load(termSet);
                ctx.ExecuteQuery();

                Microsoft.SharePoint.Client.Taxonomy.TermCollection allTermsInTermSet = termSet.GetAllTerms();
                ctx.Load(allTermsInTermSet);
                ctx.ExecuteQuery();

                return allTermsInTermSet;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static ClientContext GetToken(string siteUrl, string userName, string password, string domainName)
        {
            ClientContext clientContext = new ClientContext(siteUrl);
            clientContext.RequestTimeout = Timeout.Infinite;

            if (string.IsNullOrEmpty(userName) == true)
            {
                clientContext.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            }
            else if (string.IsNullOrEmpty(domainName) == false)
            {
                clientContext.Credentials = new System.Net.NetworkCredential(userName, password, domainName);
            }
            else
            {
                System.Security.SecureString passWord = new System.Security.SecureString();
                foreach (char c in password.ToCharArray())
                    passWord.AppendChar(c);

                clientContext.Credentials = new SharePointOnlineCredentials(userName, passWord);
            }
            return clientContext;
        }
        */

    }
}
#endif
