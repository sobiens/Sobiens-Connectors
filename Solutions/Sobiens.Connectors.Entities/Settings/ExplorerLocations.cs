using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Entities.Settings
{
    public class ExplorerLocations:List<ExplorerLocation>
    {
        public ExplorerLocations() 
        {
        }

        public List<ExplorerLocation> this[ApplicationTypes applicationType]
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
