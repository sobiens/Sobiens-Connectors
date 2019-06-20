using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SharePoint
{
#if General
    [Serializable]
#endif
    public class SPDocument : IItem
    {
        public SPDocument(Guid siteSettingID)
        {
            SiteSettingID = siteSettingID;
            Properties = new Dictionary<string, string>();
        }
        public string ID = String.Empty;
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public string FileName = String.Empty;
        public string WebUrl = String.Empty;
        public string ListName = String.Empty;
        public string FolderPath = String.Empty;
        public string URL { get; set; }
        public Guid SiteSettingID { get; set; }
        public System.Collections.Generic.Dictionary<string, string> Properties { get; set; }
        public string GetListItemURL()
        {
            return URL.Substring(0, URL.LastIndexOf("/")) + "/DispForm.aspx?ID=" + ID.ToString();
        }
        public int GetMinorVersion()
        {
            string versionString = this.Properties["ows__UIVersionString"];
            return int.Parse(versionString.Split(new string[] { "." }, StringSplitOptions.None)[1]);
        }
        public int GetMajorVersion()
        {
            string versionString = this.Properties["ows__UIVersionString"];
            return int.Parse(versionString.Split(new string[] { "." }, StringSplitOptions.None)[0]);
        }

        public string GetID()
        {
            return this.Properties["ows_ID"];
        }
        public bool isExtracted()
        {
            return this.Properties["ows__Level"]=="255";
        }

        public bool isFolder()
        {
            return this.isFolder();
        }
    }
}
