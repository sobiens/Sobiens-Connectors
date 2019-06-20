using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities.FileSystem
{
    [Serializable]
    public class FSFolder : ISPCFolder
    {
        public FSFolder(EUSiteSetting siteSetting, string uniqueIdentifier, string title, string path)
        {
            SiteSetting = siteSetting;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            Path = path;
            ContainsItems = true;
        }
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public bool ContainsItems { get; set; }
        public EUSiteSetting SiteSetting { get; set; }

    }
}
