using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class QueryProjectObject
    {
        public QueryProjectObject() { }

        public string FolderName {get;set;}
        public string Name { get; set; }
        public List<QueryPanelObject> QueryPanels { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
