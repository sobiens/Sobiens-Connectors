using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Settings
{
    public class ItemPropertyMapping
    {
        public Guid ID { get; set; }
        public string ContentTypeID { get; set; }
        public string ApplicationPropertyName { get; set; }
        public string ServicePropertyName  { get; set; }
        public string FolderDisplayName { get; set; }
    }
}
