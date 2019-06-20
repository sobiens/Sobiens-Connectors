using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class ContentType
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string TemplateURL { get; set; }
        public string Version { get; set; }
        public FieldCollection Fields { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
