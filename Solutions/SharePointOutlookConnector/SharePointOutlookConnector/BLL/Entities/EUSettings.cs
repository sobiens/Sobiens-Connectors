using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    // JOEL JEFFERY 20110713
    [XmlInclude(typeof(EUListSetting))]
    [XmlInclude(typeof(EUSiteSetting))]
    public class EUSettings
    {
        public bool IsMultipleConfigurationSettingVersion = false;
        public string EmailAttachmentRootFolderUrl = String.Empty;
        public string EmailAttachmentFolderUrl = String.Empty;
        public string EmailAttachmentRootWebUrl = String.Empty;
        public string EmailAttachmentWebUrl = String.Empty;
        public string EmailAttachmentListName = String.Empty;

        public bool SaveAsWord = false;
        public bool UseDefaultCredential = true;
        public bool UploadAutomatically = true;
        public bool DetailedLogMode = true;

        public string Url = String.Empty;
        public string User = String.Empty;
        public string Password = String.Empty;
        public string EmailTitleSPFieldName = String.Empty;
        public string EmailToSPFieldName = String.Empty;
        public string EmailFromSPFieldName = String.Empty;
        public string EmailCCSPFieldName = String.Empty;
        public string EmailBCCSPFieldName = String.Empty;
        public string EmailBodySPFieldName = String.Empty;
        public string EmailSentSPFieldName = String.Empty;
        public string EmailReceivedSPFieldName = String.Empty;

        public EUListSetting DefaultListSetting = new EUListSetting();
        public List<EUSiteSetting> SiteSettings = new List<EUSiteSetting>();
        public List<EUListSetting> ListSettings = new List<EUListSetting>();

        public EUSettings() { } // keeps XmlSeralizer happy
    }
}
