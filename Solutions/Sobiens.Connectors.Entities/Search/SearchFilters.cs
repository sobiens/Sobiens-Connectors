using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Search
{
    public class SearchFilters : List<SearchFilter>
    {
        public bool IsOr = false;
        public string GetCombinationFilterTypeString()
        {
            if (this.IsOr == true)
                return Languages.Translate("Or");
            else
                return Languages.Translate("And");
        }
        public SearchFilters Clone()
        {
            SearchFilters filters = new SearchFilters();
            foreach (SearchFilter filter in this)
            {
                filters.Add(filter);
            }
            return filters;
        }
    }
}
