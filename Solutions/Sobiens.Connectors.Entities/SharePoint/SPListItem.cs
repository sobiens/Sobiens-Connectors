using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SharePoint
{
    #if General
    [Serializable]
    #endif
    public class SPListItem : IItem
    {
        public SPListItem(Guid siteSettingID)
        {
            Properties = new Dictionary<string, string>();
            SiteSettingID = siteSettingID;
        }
        public int ID = int.MinValue;
        public string UniqueIdentifier { get; set; }
        public string ContentTypeName
        {
            get
            {
                if (Properties["ContentType"] != null)
                    return Properties["ContentType"];
                return String.Empty;
            }
        }
        public Guid SiteSettingID { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string WebApplicationURL = String.Empty;
        public string SiteCollectionURL = String.Empty;
        public string WebURL = String.Empty;
        public string ListName = String.Empty;
        public string WebRelativePath
        {
            get
            {
                return this.URL.Substring(this.WebURL.Length);
            }
        }

        public string CheckoutUser = String.Empty;
        public System.Collections.Generic.Dictionary<string, string> Properties { get; set; }
        public string GetListItemURL()
        {
            return URL.Substring(0, URL.LastIndexOf("/")) + "/DispForm.aspx?ID=" + ID.ToString() ;
        }
        public int GetMinorVersion()
        {
            string versionString = string.Empty;
            if (this.Properties.ContainsKey("UIVersionString") == true)
                versionString = this.Properties["UIVersionString"];
            else
                versionString = this.Properties["_UIVersionString"];

            return int.Parse(versionString.Split(new string[]{"."}, StringSplitOptions.None)[1]);
        }
        public int GetMajorVersion()
        {
            string versionString = string.Empty;
            if (this.Properties.ContainsKey("UIVersionString") == true)
                versionString = this.Properties["UIVersionString"];
            else
                versionString = this.Properties["_UIVersionString"];

            return int.Parse(versionString.Split(new string[] { "." }, StringSplitOptions.None)[0]);
        }

        public string GetID()
        {
            if(this.Properties.ContainsKey("ID") == true)
                return this.Properties["ID"];
            else if (this.Properties.ContainsKey("ows_ID") == true)
                return this.Properties["ows_ID"];

            return string.Empty;
        }
        public bool isExtracted()
        {
            return false;
            //return this.Properties["Level"] == "255";
        }
        public bool isFolder()
        {
            if (this.Properties.ContainsKey("FSObjType") == true)
            {
                return this.Properties["FSObjType"].ToString().Equals("1");
            }
            else
                return false;
        }

    }
}
