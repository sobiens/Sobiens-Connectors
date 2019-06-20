using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.Services.SharePoint
{
    public class SPCamlManager
    {
        /*
        public static string GetCamlFilterTypeString(CamlFilterTypes filterType)
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
        public static string GetCamlFieldTypeString(FieldTypes fieldType)
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

        private static string ReplaceSpecialCharachters(string value)
        {
            if (value.Equals("[Me]", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                return "<UserID Type=\"Integer\" />";
            }

            return "<![CDATA[" + value + "]]>";
        }

        private static string GetCamlString(CamlFilter filter)
        {
            string caml = String.Empty;
            string fieldTypeString = SPCamlManager.GetCamlFieldTypeString(filter.FieldType);
            string filterTypeString = SPCamlManager.GetCamlFilterTypeString(filter.FilterType);
            caml = "<" + filterTypeString + "><FieldRef Name=\"" + filter.FieldName + "\"/><Value Type=\"" + fieldTypeString + "\">" + ReplaceSpecialCharachters(filter.FilterValue) + "</Value></" + filterTypeString + ">";
            return caml;
        }

        public static string GetCamlString(CamlFilters filters)
        {
            StringBuilder camlStringBuilder = new StringBuilder();
            for (int i = 0; i < filters.Count; i = i + 2)
            {
                string camlString1 = GetCamlString(filters[i]);
                string combinationFilterTypeString = filters.GetCombinationFilterTypeString();

                if (filters.Count > i + 1)
                {
                    string camlString2 = GetCamlString(filters[i + 1]);
                    camlStringBuilder.Append("<" + combinationFilterTypeString + ">");
                    camlStringBuilder.Append(camlString1);
                    camlStringBuilder.Append(camlString2);
                    camlStringBuilder.Append("</" + combinationFilterTypeString + ">");
                }
                else
                {
                    camlStringBuilder.Append(camlString1);
                }
                if (i > 1)
                {
                    camlStringBuilder.Insert(0, "<" + combinationFilterTypeString + ">");
                    camlStringBuilder.Append("</" + combinationFilterTypeString + ">");
                }
            }
            return camlStringBuilder.ToString();
        }
        public static string GetCamlString(List<string> filters, bool isOr)
        {
            StringBuilder camlStringBuilder = new StringBuilder();
            string combinationFilterTypeString = (isOr == true ? "Or" : "And");
            for (int i = 0; i < filters.Count; i = i + 2)
            {
                string camlString1 = filters[i];
                if (filters.Count > i + 1)
                {
                    string camlString2 = filters[i + 1];
                    camlStringBuilder.Append("<" + combinationFilterTypeString + ">");
                    camlStringBuilder.Append(camlString1);
                    camlStringBuilder.Append(camlString2);
                    camlStringBuilder.Append("</" + combinationFilterTypeString + ">");
                }
                else
                {
                    camlStringBuilder.Append(camlString1);
                }
                if (i > 1)
                {
                    camlStringBuilder.Insert(0, "<" + combinationFilterTypeString + ">");
                    camlStringBuilder.Append("</" + combinationFilterTypeString + ">");
                }
            }
            return camlStringBuilder.ToString();
        }
        */
        public static string GetWhereCamlString(CamlFilters filters)
        {
            return "<Where>" + filters.ToCaml() + "</Where>";
        }

        public static string GetOrderByCamlString(List<CamlOrderBy> orderBys)
        {
            StringBuilder camlStringBuilder = new StringBuilder();
            camlStringBuilder.Append("<OrderBy>");
            foreach (CamlOrderBy orderBy in orderBys)
            {
                camlStringBuilder.Append("<FieldRef Name=\"" + orderBy.FieldName + "\" " + (orderBy.IsAsc == false ? "Ascending=\"FALSE\"" : "") + " />");
            }
            camlStringBuilder.Append("</OrderBy>");
            return camlStringBuilder.ToString();
        }
        public static string GetCamlString(CamlFilters filters, List<CamlOrderBy> orderBys)
        {
            return GetWhereCamlString(filters) + GetOrderByCamlString(orderBys);
        }
        public static string GetCamlString(CamlFilters filters)
        {
            List<CamlOrderBy> orderBys = new List<CamlOrderBy>();
            return GetWhereCamlString(filters) + GetOrderByCamlString(orderBys);
        }


        public static string GetViewFieldsCamlString(List<CamlFieldRef> viewFields)
        {
            StringBuilder camlStringBuilder = new StringBuilder();
            for (int i = 0; i < viewFields.Count; i++)
            {
                camlStringBuilder.Append("<FieldRef Name=\"" + viewFields[i].Name + "\" />");
            }
            return camlStringBuilder.ToString();
        }

        public static XmlNode GetQueryOptionsXmlNode(CamlQueryOptions queryOptions)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode queryOptionsNode = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

            XmlNode includeMandatoryColumnsNode = xmlDoc.CreateNode(XmlNodeType.Element, "IncludeMandatoryColumns", String.Empty);
            includeMandatoryColumnsNode.InnerText = queryOptions.IncludeMandatoryColumns == true ? "TRUE" : "FALSE";
            queryOptionsNode.AppendChild(includeMandatoryColumnsNode);

            XmlNode dateInUtcNode = xmlDoc.CreateNode(XmlNodeType.Element, "DateInUtc", String.Empty);
            dateInUtcNode.InnerText = queryOptions.DateInUtc == true ? "TRUE" : "FALSE";
            queryOptionsNode.AppendChild(dateInUtcNode);

            if (string.IsNullOrEmpty(queryOptions.Folder) == false)
            {
                XmlNode folderNode = xmlDoc.CreateNode(XmlNodeType.Element, "Folder", String.Empty);
                folderNode.InnerText = queryOptions.Folder;
                queryOptionsNode.AppendChild(folderNode);
            }

            if (string.IsNullOrEmpty(queryOptions.Scope) == false)
            {
                XmlNode viewAttributesNode = xmlDoc.CreateNode(XmlNodeType.Element, "ViewAttributes", String.Empty);
                XmlAttribute scopeAttribute = xmlDoc.CreateAttribute(string.Empty, "Scope", String.Empty);
                scopeAttribute.InnerText = queryOptions.Scope;
                viewAttributesNode.Attributes.Append(scopeAttribute);
                queryOptionsNode.AppendChild(viewAttributesNode);
            }


            if (queryOptions.RowLimit.HasValue == true)
            {
                XmlNode rowLimitNode = xmlDoc.CreateNode(XmlNodeType.Element, "RowLimit", String.Empty);
                rowLimitNode.InnerText = queryOptions.RowLimit.Value.ToString();
                queryOptionsNode.AppendChild(rowLimitNode);
            }
            if (string.IsNullOrEmpty(queryOptions.ListItemCollectionPositionNext) == false)
            {
                XmlNode pagingNode = xmlDoc.CreateNode(XmlNodeType.Element, "Paging", String.Empty);
                XmlAttribute positionNextAttribute = xmlDoc.CreateAttribute("ListItemCollectionPositionNext");
                positionNextAttribute.Value = queryOptions.ListItemCollectionPositionNext.ToString();
                pagingNode.Attributes.Append(positionNextAttribute);
                queryOptionsNode.AppendChild(pagingNode);
            }

            return queryOptionsNode;
        }

        public static XmlNode GetViewFieldsXmlNode(List<CamlFieldRef> viewFields)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode viewFieldsNode = xmlDoc.CreateNode(XmlNodeType.Element, "ViewFields", "");
            viewFieldsNode.InnerXml = GetViewFieldsCamlString(viewFields);
            return viewFieldsNode;
        }
    }
}
