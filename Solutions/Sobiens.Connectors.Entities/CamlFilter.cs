using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities
{
    public class CamlFilter : ICamlFilter
    {
        private string GetCamlFilterTypeString(CamlFilterTypes filterType)
        {
            string camlString = String.Empty;
            switch (filterType)
            {
                case CamlFilterTypes.BeginsWith:
                    camlString = "BeginsWith";
                    break;
                case CamlFilterTypes.Contains:
                    camlString = "Contains";
                    break;
                case CamlFilterTypes.Equals:
                    camlString = "Eq";
                    break;
                case CamlFilterTypes.EqualsGreater:
                    camlString = "Geq";
                    break;
                case CamlFilterTypes.EqualsLesser:
                    camlString = "Leq";
                    break;
                case CamlFilterTypes.Greater:
                    camlString = "Gt";
                    break;
                case CamlFilterTypes.NotEqual:
                    camlString = "Neq";
                    break;
            }
            return camlString;
        }
        private string GetCamlFieldTypeString(FieldTypes fieldType)
        {
            string camlString = String.Empty;
            switch (fieldType)
            {
                case FieldTypes.Boolean:
                    camlString = "boolean";
                    break;
                case FieldTypes.Choice:
                    camlString = "Text";
                    break;
                case FieldTypes.Computed:
                    camlString = "text";
                    break;
                case FieldTypes.ContentType:
                    camlString = "text";
                    break;
                case FieldTypes.ContentTypeId:
                    camlString = "text";
                    break;
                case FieldTypes.Counter:
                    camlString = "number";
                    break;
                case FieldTypes.DateTime:
                    camlString = "DateTime";
                    break;
                case FieldTypes.File:
                    camlString = "text";
                    break;
                case FieldTypes.Lookup:
                    camlString = "number";
                    break;
                case FieldTypes.Note:
                    camlString = "text";
                    break;
                case FieldTypes.Number:
                    camlString = "number";
                    break;
                case FieldTypes.Text:
                    camlString = "text";
                    break;
                case FieldTypes.User:
                    camlString = "text";
                    break;
            }
            return camlString;
        }
        private string ReplaceSpecialCharachters(string value)
        {
            if (value.Equals("[Me]", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                return "<UserID Type=\"Integer\" />";
            }

            return "<![CDATA[" + value + "]]>";
        }

        public string CamlAsString { get; set; }
        public string FieldName { get; set; }
        public CamlFilterTypes FilterType { get; set; }
        public string FilterValue { get; set; }
        public FieldTypes FieldType { get; set; }
        public CamlFilter() { }
        public CamlFilter(string caml)
        {
            this.CamlAsString = caml;
        }
        public CamlFilter(Search.SearchFilter searchFilter)
        {
            this.FieldName = searchFilter.FieldName;
            this.FieldType = searchFilter.FieldType;
            this.FilterType = searchFilter.FilterType;
            this.FilterValue = searchFilter.FilterValue;
        }

        public CamlFilter(string fieldName, FieldTypes fieldType, CamlFilterTypes filterType, string filterValue)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            FilterType = filterType;
            FilterValue = filterValue;
        }
        public string ToCaml()
        {
            if (string.IsNullOrEmpty(CamlAsString) == false)
                return CamlAsString;

            string caml = String.Empty;
            string fieldTypeString = GetCamlFieldTypeString(this.FieldType);
            string filterTypeString = GetCamlFilterTypeString(this.FilterType);
            caml = "<" + filterTypeString + "><FieldRef Name=\"" + this.FieldName + "\"/><Value " +
                (this.FieldType == FieldTypes.DateTime ? " IncludeTimeValue = 'TRUE' ":"")
                + " Type=\"" + fieldTypeString + "\">" + ReplaceSpecialCharachters(this.FilterValue) + "</Value></" + filterTypeString + ">";
            return caml;
        }
    }
}
