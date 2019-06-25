
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SharePoint
{
#if General
    [Serializable]
#endif
    /// <summary>
    /// SharePoint Foundation Web site
    /// </summary>
    public class SPTermSet
    {
        public SPTermSet() { }
        public SPTermSet(Guid id, string title, IDictionary<string, string> names)
        {
            this.ID = id;
            this.Title = title;
            this.Names = names;
        }

        public Guid ID  { get; set; }
        public string Title { get; set; }
        public IDictionary<string, string> Names { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
