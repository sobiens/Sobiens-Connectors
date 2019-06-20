using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities.FileSystem
{
#if General
    [Serializable]
#endif
    public class FSView : IView
    {
        public FSView(Guid siteSettingID, string uniqueIdentifier, string name)
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Name = name;
            RowLimit = 100;
            ViewFields = new List<CamlFieldRef>();
            ViewFields.Add(new CamlFieldRef("File Name"));
        }
        public string UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public Guid SiteSettingID { get; set; }
        public List<CamlFieldRef> ViewFields { get; set; }
        public int RowLimit { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
