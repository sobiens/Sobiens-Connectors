
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.CRM
{
#if General
    [Serializable]
#endif
    /// <summary>
    /// SharePoint Foundation Web site
    /// </summary>
    public class CRMDashboard : Folder
    {
        public string UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public int RowLimit { get; set; }
        public Guid SiteSettingID { get; set; }
        public List<CamlFieldRef> ViewFields { get; set; }

        public CRMDashboard() : base() { }
        public CRMDashboard(string uniqueIdentifier, string viewName, Guid siteSettingID)
        {
            UniqueIdentifier = uniqueIdentifier;
            Name = viewName;
            SiteSettingID = siteSettingID;
            RowLimit = 100;
            ViewFields = new List<CamlFieldRef>();
        }
        public override string GetUrl() { return string.Empty; }
        public override string GetPath() { throw new NotImplementedException(); }
        public override string GetListName() { throw new NotImplementedException(); }
        public override string GetRoot() { throw new NotImplementedException(); }
        public override string GetWebUrl() { throw new NotImplementedException(); }

    }
}
