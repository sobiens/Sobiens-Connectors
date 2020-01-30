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
    public class SQLTrigger:SQLFolder
    {
        public SQLTrigger() : base() { }
        public SQLTrigger(string title, Guid siteSettingID, string uniqueIdentifier)
            : base(siteSettingID, uniqueIdentifier, title)
        {
        }

        public override bool Equals(object value)
        {
            SQLTrigger trigger = value as SQLTrigger;

            return (trigger != null)
                && (SQLSyntax == trigger.SQLSyntax);
        }
    }
}
