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
    public class SQLField : Field
    {
        public SQLField():base()
        {
        }
        public string SQLFieldTypeName
        {
            get;set;
        }
        public string ReferenceFieldName { get; set; }


        public string ToSQL()
        {
            string sql = string.Empty;
            sql = this.Name + " " + this.SQLFieldTypeName + " ";
            switch (this.SQLFieldTypeName.ToLower())
            {
                case "varchar":
                case "nvarchar":
                case "nchar":
                    sql += "(" + this.MaxLength + ")";
                    break;
            }
            sql += (this.Required == true ? " NOT NULL" : " NULL");
            return sql;
        }
    }
}
