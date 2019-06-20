using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface ISiteSetting
    {
        SiteSettingTypes SiteSettingType { get; set; }
        Guid ID { get; set; }
        string Url { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool UseDefaultCredential { get; set; }
        bool UseClaimAuthentication { get; set; }
        bool CheckInAfterCopy { get; set; }
        bool useMajorVersionAsDefault { get; set; }
        bool CheckInAfterEditProperties { get; set; }
        bool limitFolderEditableProperties { get; set; }
        
        object Clone();
    }
}
