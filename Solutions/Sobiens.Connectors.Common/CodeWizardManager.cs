using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.SQLServer;

namespace Sobiens.Connectors.Common
{
    public class CodeWizardManager
    {
        public static string FixTableNameForCode(string tableName)
        {
            return tableName.Replace(" ", "_");
        }
        public static string FixFieldNameForCode(string fieldName)
        {
            return fieldName.Replace(" ", "_");
        }
        public static string GetJsFieldTypeAsString(Field field)
        {
            string fieldTypeString = "SobyFieldTypes.Text";
            if (field.Type == FieldTypes.Number)
                fieldTypeString = "SobyFieldTypes.Number";

            return fieldTypeString;
        }
        public static string GetCsharpFieldTypeAsString(Field field)
        {
            string fieldTypeString = "string";
            switch (((Sobiens.Connectors.Entities.SQLServer.SQLField)field).SQLFieldTypeName)
            {
                case "int":
                    fieldTypeString = "int" + (field.Required == true ? "" : "?");
                    break;
                case "bigint":
                    fieldTypeString = "long" + (field.Required == true ? "" : "?");
                    break;
                case "float":
                case "decimal":
                case "numeric":
                case "money":
                    fieldTypeString = "decimal" + (field.Required == true ? "" : "?");
                    break;
                case "real":
                    fieldTypeString = "System.Single" + (field.Required == true ? "" : "?");
                    break;
                case "smallint":
                    fieldTypeString = "System.Int16" + (field.Required == true ? "" : "?");
                    break;
                case "tinyint":
                    fieldTypeString = "System.Byte" + (field.Required == true ? "" : "?");
                    break;
                case "uniqueidentifier":
                    fieldTypeString = "System.Guid" + (field.Required == true ? "" : "?");
                    break;
                case "bit":
                    fieldTypeString = "bool" + (field.Required == true ? "" : "?");
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    fieldTypeString = "System.DateTime" + (field.Required == true ? "" : "?");
                    break;
            }
/*
            string fieldTypeString = "string";
            if (field.Type == FieldTypes.Boolean)
                fieldTypeString = "bool";
            if (field.Type == FieldTypes.)
                fieldTypeString = "bool";
            else if (field.Type == FieldTypes.Number)
                fieldTypeString = "int";
            else if (field.Type == FieldTypes.DateTime)
                fieldTypeString = "System.DateTime";
            else if (field.Type == FieldTypes.Lookup)
            {
                fieldTypeString = "int";
                if (field as SQLField != null)
                {
                    string sqlFieldType = ((SQLField)field).SQLFieldTypeName;
                    switch (sqlFieldType.ToLower())
                    {
                        case "char":
                        case "nchar":
                        case "nvarchar(MAX)":
                        case "nvarchar":
                        case "varchar(MAX)":
                        case "varchar":
                        case "ntext":
                        case "text":
                        case "hierarchyid":
                        case "uniqueidentifier":
                            fieldTypeString = "string";
                            break;
                    }
                }
            }
            */
            return fieldTypeString;
        }
        public static string GetJsFieldQueryStringKeyPrefixSuffix(Field field)
        {
            string fieldQueryStringKeyPrefixSuffix = "'";
            switch (((Sobiens.Connectors.Entities.SQLServer.SQLField)field).SQLFieldTypeName)
            {
                case "int":
                case "bigint":
                case "float":
                case "decimal":
                case "numeric":
                case "money":
                case "smallint":
                case "tinyint":
                    fieldQueryStringKeyPrefixSuffix = "";
                    break;
            }

            return fieldQueryStringKeyPrefixSuffix;
        }
        public static string GetJsFieldTypeAsStringByFieldName(List<Folder> tables, string tableName, string fieldName)
        {
            SQLTable table = (SQLTable)tables.Where(t => t.Title == tableName).First();
            Field field = table.Fields.Where(t => t.Name == fieldName).First();
            string fieldTypeString = "SobyFieldTypes.Text";
            switch (((Sobiens.Connectors.Entities.SQLServer.SQLField)field).SQLFieldTypeName)
            {
                case "int":
                case "bigint":
                case "float":
                case "decimal":
                case "numeric":
                case "money":
                case "smallint":
                case "tinyint":
                    fieldTypeString = "SobyFieldTypes.Number";
                    break;
                case "bit":
                    fieldTypeString = "SobyFieldTypes.Boolean";
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    fieldTypeString = "SobyFieldTypes.DateTime";
                    break;
            }


            return fieldTypeString;
        }
        public static string GetFieldNavigationPropertyName(SQLTable sqlTable, Field field)
        {
            Sobiens.Connectors.Entities.SQLServer.SQLForeignKey foreignKey = sqlTable.ForeignKeys.Where(t => t.TableColumnNames.Contains(field.Name) == true).FirstOrDefault();
            string navigationPropertyName = field.List;
            for (int n = 0; n < foreignKey.TableColumnNames.Count; n++)
            {
                navigationPropertyName += foreignKey.TableColumnNames[n];
            }

            return navigationPropertyName;
        }
    }
}
