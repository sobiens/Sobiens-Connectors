using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUDocument : ISPCItem
    {
        public EUDocument(EUSiteSetting siteSetting)
        {
            SiteSetting = siteSetting;
        }
        public string ID = String.Empty;
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public string FileName = String.Empty;
        public string WebUrl = String.Empty;
        public string ListName = String.Empty;
        public string FolderPath = String.Empty;
        public string URL { get; set; }
        public EUSiteSetting SiteSetting { get; set; }
    }
}
