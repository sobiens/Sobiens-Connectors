using System;
using System.Net;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using Sobiens.Connectors.Entities;
using System.Xml.XPath;
using System.Xml;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SharePoint;
using System.Collections;
using Sobiens.Connectors.Entities.Settings;
using System.Web;
using System.Diagnostics;
using Sobiens.Connectors.Entities.Workflows;
using System.Globalization;
using System.Text;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using Sobiens.Connectors.Entities.SQLServer;

namespace Sobiens.Connectors.Common.SQLServer
{
    public class SQLServerService
    {
        public string GetConnectionString(ISiteSetting siteSetting, string initialCatalog)
        {
            string userNamePasswordPart = "Integrated Security=True";
            if (siteSetting.UseDefaultCredential == false)
            {
                userNamePasswordPart = "Integrated Security=True";
            }
            string initialCatalogPart = string.Empty;
            if(string.IsNullOrEmpty(initialCatalog) == false)
                initialCatalogPart = "Initial Catalog = " + initialCatalog + ";";

            return string.Format(
                "Data Source={0}; {1} {2}", siteSetting.Url, initialCatalogPart, userNamePasswordPart);
        }

        public List<Sobiens.Connectors.Entities.Folder> GetFolders(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder parentFolder)
        {
            return GetFolders(siteSetting, parentFolder, null);
        }

        public List<Sobiens.Connectors.Entities.Folder> GetFolders(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder parentFolder, int[] includedFolderTypes)
        {
            List<Sobiens.Connectors.Entities.Folder> subFolders = new List<Sobiens.Connectors.Entities.Folder>();
            if (parentFolder as Sobiens.Connectors.Entities.SQLServer.SQLServer != null)
            {
                Sobiens.Connectors.Entities.SQLServer.SQLServer sqlServer = (Sobiens.Connectors.Entities.SQLServer.SQLServer)parentFolder;
                try
                {
                    using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, string.Empty)))
                    {
                        con.Open();

                        // Set up a command with the given query and associate
                        // this with the current connection.
                        using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                        {
                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string dbName = dr[0].ToString();
                                    if (dbName.Equals("master", StringComparison.InvariantCultureIgnoreCase) == false
                                        && dbName.Equals("model", StringComparison.InvariantCultureIgnoreCase) == false
                                        && dbName.Equals("tempdb", StringComparison.InvariantCultureIgnoreCase) == false
                                        && dbName.Equals("msdb", StringComparison.InvariantCultureIgnoreCase) == false
                                        )
                                    {
                                        SQLDB list = new SQLDB(dbName, siteSetting.ID, dbName);
                                        subFolders.Add(list);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("SharePointService GetWebs method returned error:{0}", ex.Message);
                    Logger.Error(message, "Service");
                }

            }
            else if (parentFolder as SQLDB != null)
            {
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString(siteSetting, parentFolder.Title)))
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");
                    foreach (DataRow row in schema.Rows)
                    {
                        string tableName = row[2].ToString();
                        SQLTable table = new SQLTable(tableName, siteSetting.ID, tableName, parentFolder.Title);
                        
                        //table.ListName = parentFolder.Title;
                        subFolders.Add(table);
                    }
                }
            }
            return subFolders;
        }

        

        public string[] GetPrimaryKeys(ISiteSetting siteSetting, string dbName, string tableName)
        {
            try
            {
                List<string> primaryKeys = new List<string>();
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("SELECT ccu.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON tc.CONSTRAINT_NAME = ccu.Constraint_name WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' and tc.table_name = '" + tableName + "'", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string fieldName = dr[0].ToString();
                                primaryKeys.Add(fieldName);
                            }
                        }
                    }
                }

                return primaryKeys.ToArray();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public void CreateField(ISiteSetting siteSetting, string dbName, string tableName, Sobiens.Connectors.Entities.Field field)
        {
            try
            {
                string sqlSyntax = "ALTER TABLE " + tableName + " ADD " + field.Name + " ";
                switch (field.Type)
                {
                    case FieldTypes.Boolean:
                        sqlSyntax += "[bit] ";
                        break;
                    case FieldTypes.DateTime:
                        sqlSyntax += "[datetime] ";
                        break;
                    case FieldTypes.Number:
                        sqlSyntax += "[int] ";
                        break;
                    default:
                        sqlSyntax += "[nvarchar](" + field.MaxLength + ") ";
                        break;
                }

                sqlSyntax += (field.Required == true ? "NOT NULL" : "NULL");
                //                    varchar(255); ";
                Sobiens.Connectors.Entities.FieldCollection fields = new FieldCollection();
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand(sqlSyntax, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public void CreateFields(ISiteSetting siteSetting, string dbName, string tableName, List<Field> fields)
        {
            try
            {
                foreach(Field field in fields)
                {
                    CreateField(siteSetting, dbName, tableName, field);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public Sobiens.Connectors.Entities.FieldCollection GetFields(ISiteSetting siteSetting, string dbName, string tableName)
        {
            try
            {
                Sobiens.Connectors.Entities.FieldCollection fields = new FieldCollection();
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("SELECT A.name columnname, B.name columntype, A.is_nullable is_nullable, A.is_identity is_identity, A.max_length max_length FROM sys.columns A" +
                                                           " INNER JOIN sys.types B ON A.system_type_id = B.system_type_id" +
                                                           " WHERE object_id = OBJECT_ID('" + tableName + "') and B.name <> 'sysname'", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string fieldName = dr["columnname"].ToString();
                                bool required = !(bool)dr["is_nullable"];
                                int maxLength = int.Parse(dr["max_length"].ToString());
                                FieldTypes fieldType = FieldTypes.Text;
                                switch (dr["columntype"].ToString().ToLower())
                                {
                                    case "int":
                                    case "bigint":
                                    case "float":
                                    case "decimal":
                                    case "numeric":
                                    case "smallint":
                                    case "tinyint":
                                        fieldType = FieldTypes.Number;
                                        break;
                                    case "bit":
                                        fieldType = FieldTypes.Boolean;
                                        break;
                                    case "date":
                                    case "datetime":
                                    case "datetime2":
                                    case "smalldatetime":
                                        fieldType = FieldTypes.DateTime;
                                        break;
                                }

                                Field field = new Field();
                                field.Name = fieldName;
                                field.DisplayName = fieldName;
                                field.Required = required;
                                field.MaxLength = maxLength;
                                field.Type = fieldType;
                                field.FromBaseType = false;
                                fields.Add(field);
                            }
                        }
                    }
                }

                return fields;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public List<IItem> GetListItems(ISiteSetting siteSetting, List<CamlOrderBy> orderBys, CamlFilters filters, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, string webUrl, string listName, out string listItemCollectionPositionNext, out int itemCount)
        {
            try
            {
                List<IItem> items = new List<IItem>();
                listItemCollectionPositionNext = string.Empty;
                itemCount = 100;
                string sqlString = SQLManager.GetSQLString(listName, filters, viewFields, orderBys, queryOptions);
                string dbName = webUrl;
                DataSet ds = new DataSet();

                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = sqlString;
                    da.SelectCommand = cmd;

                    da.Fill(ds);
                }
                DataTable table = ds.Tables[0];
                foreach(DataRow row in table.Rows)
                {
                    SQLTableRecord record = new SQLTableRecord(siteSetting.ID);
                    foreach(CamlFieldRef viewField in viewFields)
                    {
                        string value = string.Empty;
                        if(row[viewField.Name] != null)
                        {
                            value = row[viewField.Name].ToString();
                        }
                        record.Properties.Add(viewField.Name, value);
                    }
                    items.Add(record);
                }

                /*
                SPView view = selectedView as SPView;
                string orderBy = String.Empty;
                if (sortField == String.Empty)
                {
                    if (view != null)
                    {
                        orderBy = view.GetOrderByXML();
                    }
                }
                else
                {
                    orderBy = "<OrderBy><FieldRef Name=\"" + sortField + "\" " + (isAsc == false ? "Ascending=\"FALSE\"" : "") + " /></OrderBy>";
                }
                SharePointListsWS.Lists ws = GetListsWebService(siteSetting, webUrl);
                XmlDocument xmlDoc = new XmlDocument();

                XmlNode query = xmlDoc.CreateNode(XmlNodeType.Element, "Query", "");
                XmlNode viewFields = SPCamlManager.GetViewFieldsXmlNode(new List<CamlFieldRef>());
                //XmlNode queryOptionsNode = xmlDoc.CreateNode(XmlNodeType.Element, "QueryOptions", "");

                int? rowLimit = null;
                if (view != null && view.RowLimit > 0)
                    rowLimit = view.RowLimit;

                CamlQueryOptions queryOptions = new CamlQueryOptions()
                {
                    IncludeMandatoryColumns = true,
                    DateInUtc = true,
                    Folder = folderName,
                    Scope = (isRecursive == true ? "RecursiveAll" : string.Empty),
                    RowLimit = rowLimit,
                    ListItemCollectionPositionNext = currentListItemCollectionPositionNext
                };
                XmlNode queryOptionsNode = SPCamlManager.GetQueryOptionsXmlNode(queryOptions);

                CamlFilters filters = new CamlFilters();
                filters.Add(@"<Eq><FieldRef Name='FSObjType' /><Value Type='Lookup'>0</Value></Eq>");
                if (view != null && view.WhereXML != String.Empty)
                {
                    filters.Add(view.WhereXML);
                }
                if (customFilters != null && customFilters.Filters.Count > 0)
                {
                    filters.Add(customFilters);
                }

                query.InnerXml = orderBy + "<Where>" + filters.ToCaml() + "</Where>";
                XmlNode ndListItems = ws.GetListItems(listName, null, query, viewFields, null, queryOptionsNode, null);
                string message = string.Format("SharePointService GetListItems method returned ListName:{0} queryOptions:{1} \n query:{2} \n xml:{3}", listName, queryOptionsNode.OuterXml, query.OuterXml, ndListItems.OuterXml);
                Logger.Info(message, "Service");

                xmlDoc.LoadXml(ndListItems.OuterXml);

                itemCount = int.Parse(xmlDoc.GetElementsByTagName("rs:data")[0].Attributes["ItemCount"].Value);
                XmlAttribute listItemCollectionPositionNextAttribute = xmlDoc.GetElementsByTagName("rs:data")[0].Attributes["ListItemCollectionPositionNext"];
                listItemCollectionPositionNext = String.Empty;
                if (listItemCollectionPositionNextAttribute != null)
                    listItemCollectionPositionNext = listItemCollectionPositionNextAttribute.Value;
                XmlNodeList _items = xmlDoc.GetElementsByTagName("z:row");
                string titleFieldName = "Title";
                if (isDocumentLibrary == true)
                    titleFieldName = "LinkFilename";
                foreach (XmlNode item in _items)
                {
                    SPListItem listItem = ParseSPListItem(item, siteSetting.ID, folderName, webUrl, listName, titleFieldName);
                    items.Add(listItem);
                }*/
                return items;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }


        private SqlDbType GetFieldDBType(FieldTypes fieldType) {
            switch (fieldType)
            {
                case FieldTypes.Boolean:
                    return SqlDbType.Bit;
                case FieldTypes.DateTime:
                    return SqlDbType.DateTime;
                case FieldTypes.Number:
                    return SqlDbType.Int;
            }

            return SqlDbType.NVarChar;
        }

        public void CreateListItem(ISiteSetting siteSetting, string dbName, string tableName, System.Collections.Generic.Dictionary<object, object> fields)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string sqlSyntax = "INSERT INTO " + tableName + "  ";
                        string fieldNameSyntax = "";
                        string parameterNameSyntax = "";
                        foreach (object _field in fields.Keys)
                        {
                            Field field = (Field)_field;
                            object value = fields[_field];
                            fieldNameSyntax += field.Name + ",";
                            parameterNameSyntax += "@" + field.Name + ",";
                            SqlParameter parameter = cmd.Parameters.Add("@" + field.Name, GetFieldDBType(field.Type));
                            parameter.Value = value;
                        }

                        fieldNameSyntax = "(" + fieldNameSyntax.Substring(0, fieldNameSyntax.Length - 1) + ")";
                        parameterNameSyntax = "(" + parameterNameSyntax.Substring(0, parameterNameSyntax.Length - 1) + ")";

                        sqlSyntax += fieldNameSyntax + " VALUES " + parameterNameSyntax;
                        cmd.CommandText = sqlSyntax;
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

    }
}
