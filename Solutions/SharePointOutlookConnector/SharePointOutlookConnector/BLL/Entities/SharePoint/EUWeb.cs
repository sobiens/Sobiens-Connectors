using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUWeb:ISPCFolder
    {
        // JOEL JEFFERY 20110713
        public EUWeb() { } // keeps XmlSeralizer happy
        public EUWeb(string url, string title, EUSiteSetting siteSetting, string uniqueIdentifier)
        {
            Url = url;
            Title = title;
            SiteSetting = siteSetting;
            UniqueIdentifier = uniqueIdentifier;
            ContainsItems = false;
        }
        public bool ContainsItems { get; set; }
        public string UniqueIdentifier { get; set; }
        public List<EUList> DocumentLibraries = new List<EUList>();
        public string Url {get;set;}
        public string Title {get;set;}
        public EUSiteSetting SiteSetting { get; set; }
    }
}
