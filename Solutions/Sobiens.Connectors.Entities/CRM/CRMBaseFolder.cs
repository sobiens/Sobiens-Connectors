using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.CRM
{
    [Serializable]
    public abstract class CRMBaseFolder : Folder
    {
        public CRMBaseFolder() : base() { }
        public CRMBaseFolder(Guid siteSettingID, string uniqueIdentifier, string logicalName, string schemaName, string title)
            : base()
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            LogicalName = logicalName;
            SchemaName = schemaName;
            Title = title;
            ContainsItems = true;
        }
        // path without site URL
        public string LogicalName = String.Empty;
        public string SchemaName = String.Empty;
        public string FolderPath = String.Empty;
        public string Url { get; set; }
        public string WebUrl = String.Empty;
        public string SiteUrl = String.Empty;

        public override string GetUrl()
        {
            string serverRelativeUrl = this.WebUrl.Substring(this.WebUrl.IndexOf('/', 9));
            string folderPath = this.FolderPath;
            if (this.FolderPath.IndexOf(serverRelativeUrl) == 0)
            {
                folderPath = this.FolderPath.Substring(serverRelativeUrl.Length) ;
            }
            //serverRelativeUrl
            return WebUrl.CombineUrl(folderPath).TrimEnd(new char[] { '/' });
        }

        public override string GetPath()
        {
            if(this.WebUrl.Split(new char[]{'/'}).Length<4)
                return FolderPath;
            else
                return FolderPath.TrimStart(new char[]{'/'});
        }

        public override string GetWebUrl()
        {
            return WebUrl;
        }

    }
}
