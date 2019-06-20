using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class SyncTaskStatus
    {
        public SyncTaskStatus()
        {
        }

        public Guid SyncTaskID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CompletionTime { get; set; }
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
