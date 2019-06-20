using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SQLServer
{
    #if General
    [Serializable]
    #endif
    public class SQLTableRecord : IItem
    {
        public SQLTableRecord(Guid siteSettingID)
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
                if (Properties["ows_ContentType"] != null)
                    return Properties["ows_ContentType"];
                return String.Empty;
            }
        }
        public Guid SiteSettingID { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string WebURL = String.Empty;
        public string ListName = String.Empty;
        public string FolderPath = String.Empty;
        public string CheckoutUser = String.Empty;
        public System.Collections.Generic.Dictionary<string, string> Properties { get; set; }
        public string GetListItemURL()
        {
            return URL.Substring(0, URL.LastIndexOf("/")) + "/DispForm.aspx?ID=" + ID.ToString() ;
        }
        public int GetMinorVersion()
        {
            string versionString = this.Properties["ows__UIVersionString"];
            return int.Parse(versionString.Split(new string[]{"."}, StringSplitOptions.None)[1]);
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
            return this.Properties["ows__Level"] == "255";
        }
        public bool isFolder()
        {
            return false;
        }
    }
}
