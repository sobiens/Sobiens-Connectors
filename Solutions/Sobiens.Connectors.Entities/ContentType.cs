using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class ContentType:Folder
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

        public override string GetUrl() { throw new NotImplementedException(); }
        public override string GetPath() { throw new NotImplementedException(); }
        public override string GetListName() { throw new NotImplementedException(); }
        public override string GetRoot() { throw new NotImplementedException(); }
        public override string GetWebUrl() { throw new NotImplementedException(); }
    }
}
