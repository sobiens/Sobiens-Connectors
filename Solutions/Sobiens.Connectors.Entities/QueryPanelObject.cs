using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class QueryPanelObject
    {
        public Folder Folder {get;set;}
        public SiteSetting SiteSetting { get; set; }
        public List<CamlFieldRef> ViewFields { get; set; }
        public CamlQueryOptions QueryOptions { get; set; }
        public CamlFilters Filters { get; set; }
        public List<CamlOrderBy> OrderBys { get; set; }
    }
}
