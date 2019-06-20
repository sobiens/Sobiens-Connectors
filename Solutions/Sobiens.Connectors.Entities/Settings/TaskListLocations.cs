using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Entities.Settings
{
    public class TaskListLocations : List<TaskListLocation>
    {
        public TaskListLocations() 
        {
        }

        public List<TaskListLocation> this[ApplicationTypes applicationType]
        {
            get
            {
                var query = from el in this
                            where el.ApplicationTypes.Contains(applicationType) == true
                            select el;
                return query.ToList();
            }
        }

    }
}
