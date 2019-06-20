using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Entities.Settings
{
    public class WorkflowConfiguration
    {
        public TaskListLocations TaskListLocations { get; set; }
        public WorkflowConfiguration() 
        {
            TaskListLocations = new TaskListLocations();
        }
    }
}
