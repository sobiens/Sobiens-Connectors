using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUList : EUFolder
    {
        // JOEL JEFFERY 20110713
        public EUList() {} // keep XmlSerializer happy 
        public EUList(EUSiteSetting siteSetting, string uniqueIdentifier, string title):base(siteSetting,uniqueIdentifier,title)
        {
//            SiteSetting = siteSetting;
//            UniqueIdentifier = uniqueIdentifier;
//            Title = title;
//            RootFolder = new EUFolder(siteSetting,uniqueIdentifier,title);
//            ContainsItems = true;
        }
        /*
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public bool ContainsItems { get; set; }
        public EUSiteSetting SiteSetting { get; set; }
        public int ServerTemplate = int.MinValue;
        public string ID = String.Empty;
        public int BaseType = int.MinValue;
        public bool AllowDeletion = false;
        public bool AllowMultiResponses = false;
        public bool EnableAttachments = false;
        public bool EnableModeration = false;
        public bool EnableVersioning = false;
        public bool EnableMinorVersion = false;
        public bool RequireCheckout = false;

        public bool IsDocumentLibrary
        {
            get
            {
                if (BaseType == 1)
                    return true;
                else
                    return false;
            }
        }

         * */
        //public EUFolder RootFolder = null;
        public string Url = String.Empty;
        public bool Hidden = false;
        
        /*
        public override string ToString()
        {
            return this.Title;
        }
         */ 
    }
}
