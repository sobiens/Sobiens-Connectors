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
    public class SQLFunction : SQLFolder
    {
        public SQLFunction() : base() { }
        public SQLFunction(string title, Guid siteSettingID, string uniqueIdentifier)
            : base(siteSettingID, uniqueIdentifier, title)
        {
        }



        public override bool Equals(object value)
        {
            SQLFunction function = value as SQLFunction;

            return (function != null)
                && (ToSQLSyntax() == function.ToSQLSyntax());
        }
    }
}
