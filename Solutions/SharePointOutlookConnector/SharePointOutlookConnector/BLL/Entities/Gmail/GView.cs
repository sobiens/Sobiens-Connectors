using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities.Gmail
{
    [Serializable]
    public class GView : ISPCView
    {
        public GView(EUSiteSetting siteSetting, string uniqueIdentifier, string name)
        {
            SiteSetting = siteSetting;
            UniqueIdentifier = uniqueIdentifier;
            Name = name;
            RowLimit = 100;
            ViewFields = new List<EUCamlFieldRef>();
            ViewFields.Add(new EUCamlFieldRef("File Name"));
        }
        public string UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public EUSiteSetting SiteSetting { get; set; }
        public List<EUCamlFieldRef> ViewFields { get; set; }
        public int RowLimit { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
