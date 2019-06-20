using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.SharePoint
{
    [Serializable]
    public abstract class SPBaseFolder : Folder
    {
        public SPBaseFolder() : base() { }
        public SPBaseFolder(Guid siteSettingID, string uniqueIdentifier, string title)
            : base()
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            ContainsItems = true;
        }
        // path without site URL
        public string FolderPath = String.Empty;
        public string Url { get; set; }
        public string WebUrl = String.Empty;
        public string SiteUrl = String.Empty;
        public string ServerRelativePath = String.Empty;

        public override string GetUrl()
        {
            string url = this.WebUrl;
            /*
            if (url.StartsWith(this.SiteUrl) == true)
            {
                url = url.Substring(this.SiteUrl.Length);
            }
            */
            return url.CombineUrl(this.FolderPath).TrimEnd(new char[] { '/' });
        }

        public override string GetPath()
        {
            if (this.WebUrl.Split(new char[] { '/' }).Length < 4)
                return FolderPath;
            else
            {
                if(this is SPList)
                    return FolderPath.TrimStart(new char[] { '/' });
                else
                    return FolderPath.TrimStart(new char[] { '/' }) + "/" + this.Title;
            }
        }

        public override string GetWebUrl()
        {
            return WebUrl;
        }

    }
}
