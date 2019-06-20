using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SQLServer
{
#if General
    [Serializable]
#endif
    public class SQLFolder : Folder
    {
        public SQLFolder() : base() { }
        public SQLFolder(Guid siteSettingID, string uniqueIdentifier, string title)
            : base()
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            ContainsItems = true;
            Selected = true;
            Properties = new Dictionary<string, string>();
        }

        public string ID = String.Empty;
        public string ListName = String.Empty;
        public string RootFolderPath = String.Empty;

        [XmlIgnore]
        public System.Collections.Generic.Dictionary<string, string> Properties { get; set; }

        public override string IconName
        {
            get
            {
                return "SQLFolder";
            }
        }

        public override bool CanUpload()
        {
            return false;
        }

        public override string GetRoot()
        {
            return RootFolderPath;
        }

        public override string GetListName()
        {
            return ListName;
        }
        public override string GetUrl()
        {
            return string.Empty;
        }

        public override string GetPath()
        {
            return string.Empty;
        }

        public override string GetWebUrl()
        {
            return string.Empty;
        }

    }
}
