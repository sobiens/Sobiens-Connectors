using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUCamlFilter
    {
        public string FieldName { get; set; }
        public EUCamlFilterTypes FilterType  { get; set; }
        public string FilterValue { get; set; }
        public EUFieldTypes FieldType { get; set; }
        public EUCamlFilter(string fieldName, EUFieldTypes fieldType, EUCamlFilterTypes filterType, bool isOr, string filterValue)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            FilterType = filterType;
            FilterValue = filterValue;
        }
        public string GetCamlString()
        {
            string caml = String.Empty;
            string fieldTypeString = SPCamlManager.GetCamlFieldTypeString(this.FieldType);
            string filterTypeString = SPCamlManager.GetCamlFilterTypeString(this.FilterType);
            caml = "<" + filterTypeString + "><Value Type=\"" + fieldTypeString + "\"><![CDATA[" + this.FilterValue + "]]></Value><FieldRef Name=\"" + this.FieldName + "\"/></" + filterTypeString + ">";
            return caml;
        }
    }
}
