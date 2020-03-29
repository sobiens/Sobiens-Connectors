using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SQLServer
{
#if General
    [Serializable]
#endif
    public class SQLTable : SQLFolder
    {
        public SQLTable(string title, Guid siteSettingID, string uniqueIdentifier, string dbName) : base(siteSettingID, uniqueIdentifier, title)
        {
            ContainsItems = false;
            this.ListName = title;
            this.DBName = dbName;
        }
        public string DBName
        {
            get;set;
        }

        public string[] PrimaryKeys
        {
            get; set;
        }

        public SQLForeignKey[] ForeignKeys
        {
            get; set;
        }

        public SQLTrigger[] Triggers
        {
            get; set;
        }
        public override string GetWebUrl()
        {
            return this.DBName;
        }
        public override string IconName
        {
            get
            {
                return "SQLTable";
            }
        }

        public override string ToString()
        {
            return this.DBName + "." + this.ListName;
        }

        public override string GetPath()
        {
            return this.DBName + "." + this.ListName;
        }

        public FieldCollection Fields { get; set; }

        public override string ToSQLSyntax()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CREATE TABLE " + this.Name + "(" + Environment.NewLine);
            foreach (SQLField field in this.Fields)
            {
                sb.Append(field.ToSQL() + "," + Environment.NewLine);
            }
            sb.Remove(sb.Length-1, 1);
            sb.Append(")");
            return sb.ToString();
        }
    }
}
