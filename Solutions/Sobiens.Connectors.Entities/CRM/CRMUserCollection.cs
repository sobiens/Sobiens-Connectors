
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
    public class CRMUserCollection : CRMBaseFolder
    {
        
        public CRMUserCollection() : base() { }
        public CRMUserCollection(string url, string title, Guid siteSettingID, string uniqueIdentifier, string siteUrl, string webUrl)
            : base()
        {
            Url = url;
            Title = title;
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            ContainsItems = false;
            this.Selected = true;
            this.SiteUrl = siteUrl;
            this.WebUrl = webUrl;
            this.FolderPath = string.Empty;
        }
        //public List<SPList> DocumentLibraries = new List<SPList>();
        public override string IconName
        {
            get
            {
                return "CRMUserCollection";
            }
        }

        public override string GetRoot()
        {
            // If we try to get the root of an SPWeb, we should fail!
            throw new NotImplementedException();
        }

        public override string GetListName()
        {
            throw new NotImplementedException();
        }
    }
}
