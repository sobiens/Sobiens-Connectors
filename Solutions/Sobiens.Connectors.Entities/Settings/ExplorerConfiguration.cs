using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Entities.Settings
{
    public class ExplorerConfiguration
    {
        //public List<Guid> SiteSettings = new List<Guid>();
        public ExplorerLocations ExplorerLocations { get; set; }
        public ExplorerConfiguration() 
        {
            ExplorerLocations = new ExplorerLocations();
        }
    }
}
