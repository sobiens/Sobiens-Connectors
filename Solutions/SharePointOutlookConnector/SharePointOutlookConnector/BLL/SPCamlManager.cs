using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class SPCamlManager
    {
        public static string GetCamlFilterTypeString(EUCamlFilterTypes filterType)
        {
            string camlString = String.Empty;
            switch (filterType)
            {
                case EUCamlFilterTypes.BeginsWith:
                    camlString = "BeginsWith";
                    break;
                case EUCamlFilterTypes.Contains:
                    camlString = "Contains";
                    break;
                case EUCamlFilterTypes.Equals:
                    camlString = "Eq";
                    break;
                case EUCamlFilterTypes.EqualsGreater:
                    camlString = "Geq";
                    break;
                case EUCamlFilterTypes.EqualsLesser:
                    camlString = "Leq";
                    break;
                case EUCamlFilterTypes.Greater:
                    camlString = "Gt";
                    break;
                case EUCamlFilterTypes.NotEqual:
                    camlString = "Neq";
                    break;
            }
            return camlString;
        }
        public static string GetCamlFieldTypeString(EUFieldTypes fieldType)
        {
            string camlString = String.Empty;
            switch (fieldType)
            {
                case EUFieldTypes.Boolean:
                    camlString = "boolean";
                    break;
                case EUFieldTypes.Choice:
                    camlString = "text";
                    break;
                case EUFieldTypes.Computed:
                    camlString = "text";
                    break;
                case EUFieldTypes.ContentType:
                    camlString = "text";
                    break;
                case EUFieldTypes.ContentTypeId:
                    camlString = "text";
                    break;
                case EUFieldTypes.Counter:
                    camlString = "number";
                    break;
                case EUFieldTypes.DateTime:
                    camlString = "DateTime";
                    break;
                case EUFieldTypes.File:
                    camlString = "text";
                    break;
                case EUFieldTypes.Lookup:
                    camlString = "number";
                    break;
                case EUFieldTypes.Note:
                    camlString = "text";
                    break;
                case EUFieldTypes.Number:
                    camlString = "number";
                    break;
                case EUFieldTypes.Text:
                    camlString = "text";
                    break;
                case EUFieldTypes.User:
                    camlString = "text";
                    break;
            }
            return camlString;
        }
        public static string GetCamlString(EUCamlFilters filters)
        {
            StringBuilder camlStringBuilder = new StringBuilder();
            for (int i = 0; i < filters.Count; i = i + 2)
            {
                string camlString1 = filters[i].GetCamlString();
                string combinationFilterTypeString = filters.GetCombinationFilterTypeString();

                if (filters.Count > i + 1)
                {
                    string camlString2 = filters[i + 1].GetCamlString();
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

        /*
        public static string GetCamlString(EUCamlFilters filters)
        {
            StringBuilder camlStringBuilder = new StringBuilder();
            for (int i=0;i<filters.Count;i++)
            {
                string camlString = filters[i].GetCamlString();
                if (i % 2 == 1)
                {
                    string combinationFilterTypeString = filters.GetCombinationFilterTypeString();
                    camlString = "<" + combinationFilterTypeString + ">" +
                        camlString +
                        "</" + combinationFilterTypeString + ">";
                }
                camlStringBuilder.Append(camlString);
            }
            return camlStringBuilder.ToString();
        }
        public static string GetCamlString(List<string> filters, bool isOr)
        {
            if (filters.Count == 0)
                return String.Empty;
            StringBuilder camlStringBuilder = new StringBuilder();
            for (int i = 0; i < filters.Count; i++)
            {
                string camlString = filters[i];
                if (i % 2 == 1)
                {
                    string combinationFilterTypeString = (isOr==true?"Or":"And");
                    camlString = "<" + combinationFilterTypeString + ">" +
                        camlString +
                        "</" + combinationFilterTypeString + ">";
                }
                camlStringBuilder.Append(camlString);
            }
            return "<Where>" + camlStringBuilder.ToString() + "</Where>";
        }
         */ 

    }
}
