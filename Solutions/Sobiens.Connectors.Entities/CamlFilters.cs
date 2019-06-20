using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Search;

namespace Sobiens.Connectors.Entities
{
    public class CamlFilters : ICamlFilter
    {
        public bool IsOr = false;
        public List<CamlFilter> Filters = new List<CamlFilter>();
        public List<CamlFilters> FilterCollections = new List<CamlFilters>();

        public CamlFilters() { }
        public CamlFilters(SearchFilters searchFilters)
        {
            this.IsOr = searchFilters.IsOr;
            foreach (SearchFilter searchFilter in searchFilters)
            {
                CamlFilter filter = new CamlFilter(searchFilter);
                this.Filters.Add(filter);
            }
        }

        public void Add(CamlFilter filter)
        {
            this.Filters.Add(filter);
        }

        public void Add(CamlFilters filters)
        {
            this.FilterCollections.Add(filters);
        }

        public void Add(string caml)
        {
            CamlFilter filter = new CamlFilter(caml);
            this.Filters.Add(filter);
        }

        /*
        public void AddFilterCollection(CamlFilters filters) {
            this.Filters.Add(filters);
        }
        */
        public string ToCaml()
        {
            List<ICamlFilter> filters = new List<ICamlFilter>();
            filters.AddRange(this.Filters);
            filters.AddRange(this.FilterCollections);

            string camlString = "";
            string filterCompareString = this.GetCombinationFilterTypeString();
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters.Count == 1)
                {
                    camlString += filters[i].ToCaml();
                }
                else if (i == 1)
                {
                    camlString += "<" + filterCompareString + ">" + filters[i - 1].ToCaml() + filters[i].ToCaml() + "</" + filterCompareString + ">";
                }
                else if (i % 2 == 1)
                {
                    camlString = "<" + filterCompareString + ">" + camlString + "<" + filterCompareString + ">" + filters[i - 1].ToCaml() + filters[i].ToCaml() + "</" + filterCompareString + "></" + filterCompareString + ">";
                }
                else if (i == filters.Count - 1)
                {
                    camlString = "<" + filterCompareString + ">" + camlString + filters[i].ToCaml() + "</" + filterCompareString + ">";
                }
            }

            return camlString;
        }

        public string GetCombinationFilterTypeString()
        {
            if (this.IsOr == true)
                return "Or";
            else
                return "And";
        }
        public CamlFilters Clone()
        {
            CamlFilters filters = new CamlFilters();
            filters.IsOr = this.IsOr;
            foreach (CamlFilter filter in this.Filters)
            {
                filters.Add(filter);
            }
            foreach (CamlFilters filter in this.FilterCollections)
            {
                filters.Add(filter);
            }
            return filters;
        }
    }
}
