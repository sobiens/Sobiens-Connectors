using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class SyncTaskSchemaCopy:SyncTask
    {
        public List<Folder> SourceObjects;
        public Folder DestinationObject;
        public bool IncludeData;

        public SyncTaskSchemaCopy()
        {
        }

        public static SyncTaskSchemaCopy NewSyncTask()
        {
            SyncTaskSchemaCopy syncTask = new SyncTaskSchemaCopy();
            syncTask.ID = Guid.NewGuid();
            return syncTask;
        }
    }
}
