
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
    public class SPTermGroup
    {
        public SPTermGroup() { }
        public SPTermGroup(Guid id, string title, bool isSystemGroup, bool isSiteCollectionGroup)
        {
            this.ID = id;
            this.Title = title;
            this.IsSystemGroup = isSystemGroup;
            this.IsSiteCollectionGroup = isSiteCollectionGroup;
        }

        public Guid ID  { get; set; }
        public string Title { get; set; }
        public bool IsSystemGroup { get; set; }
        public bool IsSiteCollectionGroup { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
