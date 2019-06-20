using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUListItem : ISPCItem
    {
        public EUListItem(EUSiteSetting siteSetting)
        {
            SiteSetting = siteSetting;
        }
        public int ID = int.MinValue;
        public string UniqueIdentifier { get; set; }
        public string ContentTypeName
        {
            get
            {
                if (Properties["ows_ContentType"] != null)
                    return Properties["ows_ContentType"].Value;
                return String.Empty;
            }
        }
        public EUSiteSetting SiteSetting { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string WebURL = String.Empty;
        public string ListName = String.Empty;
        public string FolderPath = String.Empty;
        public string CheckoutUser = String.Empty;
        public XmlAttributeCollection Properties = null;
        public string GetListItemURL()
        {
            return URL.Substring(0, URL.LastIndexOf("/")) + "/DispForm.aspx?ID=" + ID.ToString() ;
        }
        public int GetMinorVersion()
        {
            string versionString = this.Properties["ows__UIVersionString"].InnerText;
            return int.Parse(versionString.Split(new string[]{"."}, StringSplitOptions.None)[1]);
        }
        public int GetMajorVersion()
        {
            string versionString = this.Properties["ows__UIVersionString"].InnerText;
            return int.Parse(versionString.Split(new string[] { "." }, StringSplitOptions.None)[0]);
        }
    }
}
