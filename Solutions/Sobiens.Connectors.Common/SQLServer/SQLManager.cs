using Sobiens.Connectors.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sobiens.Connectors.Common.SQLServer
{
    public class SQLManager
    {
        public static string GetSQLString(string tableName, CamlFilters filters, List<CamlFieldRef> viewFields, List<CamlOrderBy> orderBys, CamlQueryOptions queryOptions)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (CamlFieldRef fieldRef in viewFields)
            {
                sb.Append(fieldRef.Name + " ,");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" FROM " + tableName + " ");
            sb.Append(GetSQLWhereString(filters));
            sb.Append(GetSQLOrderByString(orderBys));

            return sb.ToString();
        }

        private static string GetSQLOrderByString(List<CamlOrderBy> orderBys)
        {
            string orderByString = string.Empty;
            foreach (CamlOrderBy orderBy in orderBys)
            {
                if (string.IsNullOrEmpty(orderByString) == false)
                    orderByString += ", ";
                orderByString += orderBy.FieldName + " " + (orderBy.IsAsc ? "ASC" : "DESC");
            }

            if (string.IsNullOrEmpty(orderByString) == false)
                orderByString = " order by " + orderByString;

            return orderByString;
        }

        private static string GetSQLWhereString(CamlFilters filters)
        {
            string whereString = GetSQLFiltersString(filters);
            if (string.IsNullOrEmpty(whereString) == false)
            {
                whereString = " WHERE " + whereString;
            }

            return whereString;
        }

        private static string GetSQLFilterString(CamlFilter filter)
        {
            string filterString = string.Empty;
            string fieldValue = "";
            string fieldName = filter.FieldName;

            switch (filter.FieldType)
            {
                case FieldTypes.Boolean:
                    bool boolValue;
                    if (filter.FilterValue == "0")
                        fieldValue = "0";
                    else
                        fieldValue = "1";
                    if(Boolean.TryParse(filter.FilterValue, out boolValue) == true)
                    {
                        fieldValue = (boolValue ? "1" : "0");
                    }
                    break;
                case FieldTypes.Number:
                    decimal decimalValue;
                    if (Decimal.TryParse(filter.FilterValue, out decimalValue) == true)
                    {
                        fieldValue = decimalValue.ToString();
                    }
                    break;
                default:
                    fieldValue = "'" + filter.FilterValue + "'";
                    break;
            }

            switch (filter.FilterType)
            {
                case CamlFilterTypes.BeginsWith:
                    filterString = "charindex(" + fieldValue + ", " + filter.FieldName + ") = 1" ;
                    break;
                case CamlFilterTypes.Contains:
                    filterString = "charindex(" + fieldValue + ", " + filter.FieldName + ") >= 1";
                    break;
                case CamlFilterTypes.Equals:
                    filterString = filter.FieldName + " = " + fieldValue;
                    break;
                case CamlFilterTypes.EqualsGreater:
                    filterString = filter.FieldName + " >= " + fieldValue;
                    break;
                case CamlFilterTypes.EqualsLesser:
                    filterString = filter.FieldName + " <= " + fieldValue;
                    break;
                case CamlFilterTypes.Greater:
                    filterString = filter.FieldName + " > " + fieldValue;
                    break;
                case CamlFilterTypes.Lesser:
                    filterString = filter.FieldName + " < " + fieldValue;
                    break;
                case CamlFilterTypes.NotEqual:
                    filterString = filter.FieldName + " <> " + fieldValue;
                    break;
                default:
                    filterString = filter.FieldName + "=" + fieldValue;
                    break;
            }


            return filterString;
        }

        private static string GetSQLFiltersString(CamlFilters filters)
        {
            string filtersString = string.Empty;
            foreach (CamlFilter filter in filters.Filters)
            {
                if (string.IsNullOrEmpty(filtersString) == false)
                    filtersString += filters.IsOr ? " OR " : " AND ";

                filtersString += GetSQLFilterString(filter);
            }

            foreach (CamlFilters filters1 in filters.FilterCollections)
            {
                if (string.IsNullOrEmpty(filtersString) == false)
                    filtersString += filters.IsOr ? " OR " : " AND ";

                filtersString += GetSQLFiltersString(filters1);
            }

            if (string.IsNullOrEmpty(filtersString) == false)
                filtersString = "(" + filtersString + ")";

            return filtersString;
        }

    }
}
