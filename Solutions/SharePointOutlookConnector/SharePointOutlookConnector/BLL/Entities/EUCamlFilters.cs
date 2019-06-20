using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUCamlFilters:List<EUCamlFilter>
    {
        public bool IsOr = false;
        public string GetCombinationFilterTypeString()
        {
            if (this.IsOr == true)
                return "Or";
            else
                return "And";
        }
        public EUCamlFilters Clone()
        {
            EUCamlFilters filters = new EUCamlFilters();
            foreach (EUCamlFilter filter in this)
            {
                filters.Add(filter);
            }
            return filters;
        }
    }
}
