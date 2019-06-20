using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUFolder : ISPCFolder
    {
        // JOEL JEFFERY 20110713
        public EUFolder() { } // keep XmlSerializer happy 
        public EUFolder(EUSiteSetting siteSetting, string uniqueIdentifier, string title)
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

        public List<EUFolder> Folders = new List<EUFolder>();
        public int ServerTemplate = int.MinValue;
        public int BaseType = int.MinValue;
        public string ID = String.Empty;
        public string SiteUrl = String.Empty;
        public string WebUrl = String.Empty;
        public string ListName = String.Empty;
        public string FolderPath = String.Empty;
        public string RootFolderPath = String.Empty;

        public bool AllowDeletion = false;
        public bool AllowMultiResponses = false;
        public bool EnableAttachments = false;
        public bool EnableModeration = false;
        public bool EnableVersioning = false;
        public bool EnableMinorVersion = false;
        public bool RequireCheckout = false;

        public bool IsDocumentLibrary
        {
            get{
                if (BaseType == 1)
                    return true;
                else
                    return false;
            }
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
