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
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public override bool Equals(object value)
        {
            SQLTrigger trigger = value as SQLTrigger;

            return (trigger != null)
                && (Content == trigger.Content);
        }
    }
}
