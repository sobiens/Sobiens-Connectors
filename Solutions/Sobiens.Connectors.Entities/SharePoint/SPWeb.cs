
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
    public class SPWeb : SPBaseFolder
    {

        public SPWeb() : base() { }
        public SPWeb(string url, string title, Guid siteSettingID, string uniqueIdentifier, string siteUrl, string webUrl, string serverRelativePath)
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
            this.ServerRelativePath = serverRelativePath;
        }

        public SPWeb(string url, string title, Guid siteSettingID, string uniqueIdentifier, string siteUrl, string webUrl)
    : this(url, title, siteSettingID, uniqueIdentifier, siteUrl, webUrl, string.Empty)
        {
            string serverRelativePath = webUrl;
            serverRelativePath = serverRelativePath.Substring(serverRelativePath.IndexOf("//") + 2);
            if (serverRelativePath.IndexOf("/") > 0)
            {
                serverRelativePath = serverRelativePath.Substring(serverRelativePath.IndexOf("/"));
            }
            else
            {
                serverRelativePath = "/";
            }

            this.ServerRelativePath = serverRelativePath;
        }

        public List<SPList> DocumentLibraries = new List<SPList>();
        public override string IconName
        {
            get
            {
                return "SPWeb";
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
