
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
    public class SPWebpart
    {
        public SPWebpart() { }
        public SPWebpart(Dictionary<string, object> properties)
        {
            this.Properties = properties;
        }

        public string ID  { get; set; }
        public string Title { get; set; }
        public Dictionary<string, object> Properties = new Dictionary<string, object>();
    }
}
