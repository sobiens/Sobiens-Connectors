using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class CamlFieldRef
    {
        public CamlFieldRef() { }
        public CamlFieldRef(string name):this(name, name)
        {
        }

        public CamlFieldRef(string name, string displayName)
        {
            this.Name = name;
            this.DisplayName = displayName;
        }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
