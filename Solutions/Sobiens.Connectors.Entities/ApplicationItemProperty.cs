using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class ApplicationItemProperty
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }

        public ApplicationItemProperty(string name, string displayName, Type type)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.Type = type;
        }
    }
}
