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

        public string Schema { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public override bool Equals(object value)
        {
            SQLFunction function = value as SQLFunction;

            return (function != null)
                && (Content == function.Content);
        }
    }
}
