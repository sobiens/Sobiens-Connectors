using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class SyncTask
    {
        public SyncTask()
        {
            SourceQueryResultMapping = new QueryResultMappings();
            SourceUniqueFieldHeaderNames = new string[] { };
            DestinationUniqueFieldNames = new string[] { };
            SourceFieldHeaderMappings = new string[] { };
            DestinationFieldMappings = new List<QueryResultMappingSelectField>();
            DestinationReferenceFields = new List<Entities.QueryResultReferenceField>();
        }

        public static SyncTask NewSyncTask()
        {
            SyncTask syncTask = new SyncTask();
            syncTask.ID = Guid.NewGuid();
            return syncTask;
        }

        public Guid ProcessID { get; set; }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool Scheduled { get; set; }
        public string Status { get; set; }

        public QueryResultMappings SourceQueryResultMapping { get; set; }
        public string[] SourceUniqueFieldHeaderNames { get; set; }
        public string[] SourceFieldHeaderMappings { get; set; }
         
        public string DestinationFolderPath { get; set; }
        public string DestinationRootFolderPath { get; set; }
        public string DestinationIDFieldHeaderName { get; set; }
        public string[] DestinationUniqueFieldNames { get; set; }
        public SiteSetting DestinationSiteSetting { get; set; }
        public bool IsDestinationDocumentLibrary { get; set; }
        public string DestinationListName { get; set; }
        //public string[] DestinationFieldMappings { get; set; }
        public string DestinationTermStoreName { get; set; }
        public List<QueryResultReferenceField> DestinationReferenceFields { get; set; }
        public string DestinationPrimaryIdFieldName { get; set; }
        public string DestinationPrimaryNameFieldName { get; set; }
        public string DestinationPrimaryFileReferenceFieldName { get; set; }
        public List<QueryResultMappingSelectField> DestinationFieldMappings { get; set; }

        public bool ShouldSkipUpdates { get; set; }

        public DateTime LastRunStartDate { get; set; }
        public DateTime LastRunEndDate { get; set; }
        public int ScheduleInterval { get; set; }
        public int ScheduleIntervalAsMinute {
            get
            {
                return this.ScheduleInterval / 1000 / 60;
            }
        }

    }
}
