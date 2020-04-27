using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities.Settings
{
    public class SiteSetting :ISiteSetting
    {
        public SiteSettingTypes SiteSettingType { get; set; }
        public Guid ID { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Parameters { get; set; }
        public bool UseDefaultCredential { get; set; }
        public bool UseClaimAuthentication { get; set; }
        public bool CheckInAfterCopy { get; set; }
        public bool useMajorVersionAsDefault { get; set; }
        public bool CheckInAfterEditProperties { get; set; }
        public bool limitFolderEditableProperties { get; set; }
        

        public override string ToString()
        {
            if (SiteSettingType == SiteSettingTypes.GMail)
                return this.Username;
            return this.Url;
        }
        public SiteSetting() 
        {
            this.SiteSettingType = SiteSettingTypes.SharePoint;
            this.UseDefaultCredential = true;
        }
        public object Clone()
        {
            SiteSetting siteSetting = new SiteSetting();
            siteSetting.ID = this.ID;
            siteSetting.Url = this.Url;
            siteSetting.Username = this.Username;
            siteSetting.Password = this.Password;
            siteSetting.UseDefaultCredential = this.UseDefaultCredential;
            siteSetting.UseClaimAuthentication = this.UseClaimAuthentication;
            siteSetting.CheckInAfterCopy = this.CheckInAfterCopy;
            siteSetting.useMajorVersionAsDefault = this.useMajorVersionAsDefault;
            siteSetting.CheckInAfterEditProperties = this.CheckInAfterEditProperties;
            siteSetting.limitFolderEditableProperties = this.limitFolderEditableProperties;
            return siteSetting;
        }
    }
}
