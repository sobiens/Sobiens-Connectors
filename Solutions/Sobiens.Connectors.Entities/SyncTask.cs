using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Entities
{
    [XmlInclude(typeof(SyncTaskListItemsCopy))]
    [XmlInclude(typeof(SyncTaskSchemaCopy))]
    public class SyncTask
    {
        public SyncTask()
        {
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


        public DateTime LastRunStartDate { get; set; }
        public DateTime LastRunEndDate { get; set; }
        public DateTime? LastSuccessfullyCompletedStartDate { get; set; }
        public int ScheduleInterval { get; set; }
        public int ScheduleIntervalAsMinute {
            get
            {
                return this.ScheduleInterval / 1000 / 60;
            }
        }

    }
}
