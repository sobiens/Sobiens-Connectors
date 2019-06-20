using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUCamlQuery
    {
        public List<EUCamlFieldRef> OrderBy = new List<EUCamlFieldRef>();
        public string WhereQuery = String.Empty;
        public EUCamlFilters Filters = new EUCamlFilters();

        public EUCamlQuery()
        {
            this.Filters.IsOr = false;
        }

        public string GetQueryXML()
        {
            string query1 = SPCamlManager.GetCamlString(Filters);
            List<string> queries = new List<string>();
            queries.Add(query1);
            queries.Add(this.WhereQuery);
            return SPCamlManager.GetCamlString(queries, false);
        }
    }
}
