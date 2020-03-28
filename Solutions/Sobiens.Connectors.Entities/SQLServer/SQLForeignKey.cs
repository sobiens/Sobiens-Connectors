using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sobiens.Connectors.Entities.SQLServer
{
#if General
    [Serializable]
#endif
    public class SQLForeignKey : SQLFolder
    {
        public SQLForeignKey() : base() { }
        public SQLForeignKey(string title, Guid siteSettingID, string uniqueIdentifier)
            : base(siteSettingID, uniqueIdentifier, title)
        {
        }

        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string TableColumnName { get; set; }

        public string ReferencedTableSchema { get; set; }
        public string ReferencedTableName { get; set; }
        public string ReferencedTableColumnName { get; set; }


        public override bool Equals(object value)
        {
            SQLFunction function = value as SQLFunction;

            return (function != null)
                && (ToSQLSyntax() == function.ToSQLSyntax());
        }
    }
}
