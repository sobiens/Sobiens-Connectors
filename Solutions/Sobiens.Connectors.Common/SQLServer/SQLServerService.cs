using System;
using System.Collections.Generic;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Sobiens.Connectors.Entities.SQLServer;
using System.Linq;
using System.Text;

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
            return GetFolders(siteSetting, parentFolder, null, string.Empty);
        }

        public List<Sobiens.Connectors.Entities.Folder> GetFolders(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Folder parentFolder, int[] includedFolderTypes, string childFoldersCategoryName)
        {
            List<Sobiens.Connectors.Entities.Folder> subFolders = new List<Sobiens.Connectors.Entities.Folder>();
            if (parentFolder as Sobiens.Connectors.Entities.SQLServer.SQLServer != null)
            {
                //Sobiens.Connectors.Entities.SQLServer.SQLServer sqlServer = (Sobiens.Connectors.Entities.SQLServer.SQLServer)parentFolder;
                try
                {
                    subFolders = GetDatabases(siteSetting, null).ToList< Sobiens.Connectors.Entities.Folder>();
                }
                catch (Exception ex)
                {
                    string message = string.Format("SharePointService GetWebs method returned error:{0}", ex.Message);
                    Logger.Error(message, "Service");
                }

            }
            else if (parentFolder as SQLDB != null)
            {
                if (childFoldersCategoryName.Equals("Tables", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    subFolders = GetTables(siteSetting, parentFolder).ToList< Sobiens.Connectors.Entities.Folder>();
                }
                else if (childFoldersCategoryName.Equals("Functions", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    subFolders = GetFunctions(siteSetting, parentFolder).ToList<Sobiens.Connectors.Entities.Folder>();
                }
                else if (childFoldersCategoryName.Equals("Views", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    List<SQLView> views = GetViews(siteSetting, parentFolder);
                    foreach (SQLView view in views) {
                        subFolders.Add((Folder)view);
                    }
                }
                else if (childFoldersCategoryName.Equals("Stored Procedures", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    subFolders = GetStoredProcedures(siteSetting, parentFolder).ToList<Sobiens.Connectors.Entities.Folder>();
                }
            }
            return subFolders;
        }


        public void ExecuteQuery(ISiteSetting siteSetting, string dbName, string sqlQuery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
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

        public SQLForeignKey[] GetForeignKeys(ISiteSetting siteSetting, string dbName, string tableName)
        {
            try
            {
                List<SQLForeignKey> foreignKeys = new List<SQLForeignKey>();
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("SELECT " +
                             "KCU1.CONSTRAINT_SCHEMA AS FK_CONSTRAINT_SCHEMA " +
                            ", KCU1.CONSTRAINT_NAME AS FK_CONSTRAINT_NAME " +
                            ", KCU1.TABLE_SCHEMA AS FK_TABLE_SCHEMA " +
                            ", KCU1.TABLE_NAME AS FK_TABLE_NAME " +
                            ", KCU1.COLUMN_NAME AS FK_COLUMN_NAME " +
                            ", KCU1.ORDINAL_POSITION AS FK_ORDINAL_POSITION " +
                            ", KCU2.CONSTRAINT_SCHEMA AS REFERENCED_CONSTRAINT_SCHEMA " +
                            ", KCU2.CONSTRAINT_NAME AS REFERENCED_CONSTRAINT_NAME " +
                            ", KCU2.TABLE_SCHEMA AS REFERENCED_TABLE_SCHEMA " +
                            ", KCU2.TABLE_NAME AS REFERENCED_TABLE_NAME " +
                            ", KCU2.COLUMN_NAME AS REFERENCED_COLUMN_NAME " +
                            ", KCU2.ORDINAL_POSITION AS REFERENCED_ORDINAL_POSITION " +
                        "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC " +
                        "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 " +
                            "ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG " +
                            "AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA " +
                            "AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME " +
                        "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU2 " +
                            "ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG " +
                            "AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA " +
                            "AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME " +
                            "AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION" +
                            " WHERE KCU1.TABLE_NAME ='" + tableName + "'"
                        , con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SQLForeignKey foreignKey = new SQLForeignKey();
                                foreignKey.Title = dr[1].ToString();
                                foreignKey.TableSchema = dr[2].ToString();
                                foreignKey.TableName = dr[3].ToString();
                                foreignKey.TableColumnName = dr[4].ToString();
                                foreignKey.ReferencedTableSchema = dr[8].ToString();
                                foreignKey.ReferencedTableName = dr[9].ToString();
                                foreignKey.ReferencedTableColumnName = dr[10].ToString();
                                foreignKeys.Add(foreignKey);
                            }
                        }
                    }
                }

                return foreignKeys.ToArray();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }
        public List<SQLTable> GetTables(ISiteSetting siteSetting, Folder folder)
        {
            try
            {
                List<SQLTable> tables = new List<SQLTable>();
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString(siteSetting, folder.Title)))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT schm.name, o.name " +
                                                                "FROM sys.objects AS o " +
                                                                "JOIN sys.schemas AS schm ON o.schema_id = schm.schema_id " +
                                                                "WHERE o.type = 'U' " +
                                                                "ORDER BY o.name ", connection))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string tableName = dr[1].ToString();
                                SQLTable table = new SQLTable(tableName, siteSetting.ID, tableName, folder.Title);
                                table.SiteSettingID = siteSetting.ID;
                                //table.ListName = tableName;
                                tables.Add(table);
                            }
                        }
                    }
                }

                foreach(SQLTable table in tables)
                {
                    table.Fields = GetFields(siteSetting, folder.Title, table.Title);
                }

                return tables;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public void CreateTable(ISiteSetting siteSetting, string dbName, SQLTable table, List<Field> fields)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    connection.Open();
                    StringBuilder sqlStringBuilder = new StringBuilder();
                    sqlStringBuilder.Append("CREATE TABLE " + table.Title + " (");
                    foreach (Field field in fields) {
                        sqlStringBuilder.Append(((SQLField)field).ToSQL() + ",");
                    }
                    sqlStringBuilder.Remove(sqlStringBuilder.Length - 1, 1);
                    sqlStringBuilder.Append(")");

                    using (SqlCommand cmd = new SqlCommand(sqlStringBuilder.ToString(), connection))
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

        public void CreateView(ISiteSetting siteSetting, string dbName, SQLView view)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    connection.Open();
                    StringBuilder sqlStringBuilder = new StringBuilder();
                    using (SqlCommand cmd = new SqlCommand(view.ToSQLSyntax(), connection))
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

        public void CreateTrigger(ISiteSetting siteSetting, string dbName, SQLTrigger trigger)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    connection.Open();
                    StringBuilder sqlStringBuilder = new StringBuilder();
                    using (SqlCommand cmd = new SqlCommand(trigger.ToSQLSyntax(), connection))
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

        public void CreateStoredProcedure(ISiteSetting siteSetting, string dbName, SQLStoredProcedure storedProcedure)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    connection.Open();
                    StringBuilder sqlStringBuilder = new StringBuilder();
                    using (SqlCommand cmd = new SqlCommand(storedProcedure.ToSQLSyntax(), connection))
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
        public void CreateFunction(ISiteSetting siteSetting, string dbName, SQLFunction function)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString(siteSetting, dbName)))
                {
                    connection.Open();
                    StringBuilder sqlStringBuilder = new StringBuilder();
                    using (SqlCommand cmd = new SqlCommand(function.ToSQLSyntax(), connection))
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

        public List<SQLView> GetViews(ISiteSetting siteSetting, Folder folder)
        {
            try
            {
                SQLDB db = (SQLDB)folder;
                List<SQLView> views = new List<SQLView>();
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, db.Title)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("SELECT schm.name, " +
                                                               "OBJECT_NAME(sm.object_id) AS object_name, " +
                                                               "sm.definition " +
                                                            "FROM sys.sql_modules AS sm " +
                                                                "JOIN sys.objects AS o ON sm.object_id = o.object_id " +
                                                            "JOIN sys.schemas AS schm " +
                                                                "ON o.schema_id = schm.schema_id " +
                                                            "WHERE o.type = 'V' " +
                                                            "ORDER BY o.type ", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SQLView view = new SQLView();
                                view.Schema = dr[0].ToString();
                                view.Name= dr[1].ToString();
                                view.Title = dr[1].ToString();
                                view._SQLSyntax = dr[2].ToString();
                                view.SiteSettingID = siteSetting.ID;
                                view.ListName = db.Title;
                                views.Add(view);
                            }
                        }
                    }
                }

                return views;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public List<SQLDB> GetDatabases(ISiteSetting siteSetting, Folder folder)
        {
            try
            {
                List<SQLDB> dbs = new List<SQLDB>();
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
                                    SQLDB db = new SQLDB(dbName, siteSetting.ID, dbName);
                                    dbs.Add(db);
                                }
                            }
                        }
                    }
                }

                return dbs;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }
        public List<SQLFunction> GetFunctions(ISiteSetting siteSetting, Folder folder)
        {
            try
            {
                SQLDB db = (SQLDB)folder;
                List<SQLFunction> functions = new List<SQLFunction>();
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, db.Title)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("SELECT schm.name, " +
                                                               "OBJECT_NAME(sm.object_id) AS object_name, " +
                                                               "sm.definition " +
                                                            "FROM sys.sql_modules AS sm " +
                                                                "JOIN sys.objects AS o ON sm.object_id = o.object_id " +
                                                            "JOIN sys.schemas AS schm " +
                                                                "ON o.schema_id = schm.schema_id " +
                                                            "WHERE o.type = 'FN' " +
                                                            "ORDER BY o.type ", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SQLFunction function = new SQLFunction();
                                function.Schema = dr[0].ToString();
                                function.Name = dr[1].ToString();
                                function.Title = dr[1].ToString();
                                function._SQLSyntax = dr[2].ToString();
                                function.SiteSettingID = siteSetting.ID;
                                function.ListName = db.Title;
                                functions.Add(function);
                            }
                        }
                    }
                }

                return functions;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public List<SQLStoredProcedure> GetStoredProcedures(ISiteSetting siteSetting, Folder folder)
        {
            try
            {
                SQLDB db = (SQLDB)folder;
                List<SQLStoredProcedure> storedProcedures = new List<SQLStoredProcedure>();
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, db.Title)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("SELECT schm.name,   " +
                                                               "OBJECT_NAME(sm.object_id) AS object_name, " +
                                                               "sm.definition " +
                                                            "FROM sys.sql_modules AS sm " +
                                                                "JOIN sys.objects AS o ON sm.object_id = o.object_id " +
                                                            "JOIN sys.schemas AS schm " +
                                                                "ON o.schema_id = schm.schema_id " +
                                                            "WHERE o.type = 'P' ", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SQLStoredProcedure storedProcedure = new SQLStoredProcedure();
                                storedProcedure.Schema = dr[0].ToString();
                                storedProcedure.Name = dr[1].ToString();
                                storedProcedure.Title = dr[1].ToString();
                                storedProcedure._SQLSyntax = dr[2].ToString();
                                storedProcedure.SiteSettingID = siteSetting.ID;
                                storedProcedure.ListName = folder.Title;
                                storedProcedures.Add(storedProcedure);
                            }
                        }
                    }
                }

                return storedProcedures;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //LogManager.LogAndShowException(methodName, ex);
                throw ex;
            }
        }

        public List<SQLTrigger> GetTriggers(ISiteSetting siteSetting, Folder folder)
        {
            try
            {
                SQLDB db = (SQLDB)folder;
                List<SQLTrigger> triggers = new List<SQLTrigger>();
                using (SqlConnection con = new SqlConnection(this.GetConnectionString(siteSetting, db.Title)))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("SELECT schm.name,   " +
                                                               "OBJECT_NAME(sm.object_id) AS object_name, " +
                                                               "sm.definition " +
                                                            "FROM sys.sql_modules AS sm " +
                                                                "JOIN sys.objects AS o ON sm.object_id = o.object_id " +
                                                            "JOIN sys.schemas AS schm " +
                                                                "ON o.schema_id = schm.schema_id " +
                                                            "WHERE o.type = 'TR' ", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SQLTrigger trigger = new SQLTrigger();
                                trigger.Schema = dr[0].ToString();
                                trigger.Name = dr[1].ToString();
                                trigger.Title = dr[1].ToString();
                                trigger._SQLSyntax = dr[2].ToString();
                                trigger.SiteSettingID = siteSetting.ID;
                                trigger.ListName = folder.Title;
                                triggers.Add(trigger);
                            }
                        }
                    }
                }

                return triggers;
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
                                int maxLength = int.Parse(dr["max_length"].ToString())/2;
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

                                SQLField field = new SQLField();
                                field.Name = fieldName;
                                field.DisplayName = fieldName;
                                field.Required = required;
                                field.MaxLength = maxLength;
                                field.Type = fieldType;
                                field.SQLFieldTypeName = dr["columntype"].ToString().ToLower();
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
