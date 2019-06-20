using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Search
{
    public class SearchFilter
    {
        public string FieldName { get; set; }
        public CamlFilterTypes FilterType  { get; set; }
        public string FilterValue { get; set; }
        public FieldTypes FieldType { get; set; }
        public SearchFilter(string fieldName, FieldTypes fieldType, CamlFilterTypes filterType, string filterValue)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            FilterType = filterType;
            FilterValue = filterValue;
        }

        public SearchFilter Clone()
        {
            return new SearchFilter(this.FieldName, this.FieldType, this.FilterType, this.FilterValue);
        }
    }
}
