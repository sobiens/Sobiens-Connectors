using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities.SkyDrive
{
    [Serializable]
    public class SDFolder : ISPCFolder
    {
        public SDFolder(EUSiteSetting siteSetting, string uniqueIdentifier, string title)
        {
            SiteSetting = siteSetting;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            ContainsItems = true;
        }
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public bool ContainsItems { get; set; }
        public EUSiteSetting SiteSetting { get; set; }

    }
}
