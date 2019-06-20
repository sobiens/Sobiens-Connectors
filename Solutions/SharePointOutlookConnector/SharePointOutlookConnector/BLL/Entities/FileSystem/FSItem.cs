using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities.FileSystem
{
    [Serializable]
    public class FSItem : ISPCItem
    {
        public FSItem(EUSiteSetting siteSetting, string uniqueIdentifier, string title, string url)
        {
            SiteSetting = siteSetting;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            URL = url;
        }
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public EUSiteSetting SiteSetting { get; set; }

    }
}
