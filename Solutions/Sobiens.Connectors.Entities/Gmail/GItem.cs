using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.Gmail
{
#if General
    [Serializable]
#endif
    public class GItem : IItem
    {
        public GItem(Guid siteSettingID, string uniqueIdentifier, string title, string url)
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            URL = url;
            Properties = new Dictionary<string, string>();
        }
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public Guid SiteSettingID { get; set; }
        public System.Collections.Generic.Dictionary<string, string> Properties { get; set; }

        public string GetListItemURL()
        {
            throw new Exception("Not implemented yet");
        }
        public int GetMinorVersion()
        {
            throw new Exception("Not implemented yet");
        }

        public int GetMajorVersion()
        {
            throw new Exception("Not implemented yet");
        }

        public string GetID()
        {
            throw new Exception("Not implemented yet");
        }
        public bool isExtracted()
        {
            throw new Exception("Not implemented yet");
        }

        public bool isFolder()
        {
            throw new Exception("Not implemented yet");
        }

    }
}
