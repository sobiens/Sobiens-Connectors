using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUSiteSetting
    {
        public EUSiteSettingTypes SiteSettingType = EUSiteSettingTypes.SharePoint;
        public string Url = String.Empty;
        public string User = String.Empty;
        public string Password = String.Empty;
        public bool UseDefaultCredential = true;
        public override string ToString()
        {
            if (SiteSettingType == EUSiteSettingTypes.GMail)
                return this.User;
            return this.Url;
        }
        public EUSiteSetting() { } // keeps XmlSeralizer happy
    }
}
