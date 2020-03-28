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
    public class SQLStoredProcedure:SQLFolder
    {
        public SQLStoredProcedure() : base() { }
        public SQLStoredProcedure(string title, Guid siteSettingID, string uniqueIdentifier)
            : base(siteSettingID, uniqueIdentifier, title)
        {
        }

        public override bool Equals(object value)
        {
            SQLStoredProcedure storedProcedure = value as SQLStoredProcedure;

            return (storedProcedure != null)
                && (ToSQLSyntax() == storedProcedure.ToSQLSyntax());
        }
    }
}
