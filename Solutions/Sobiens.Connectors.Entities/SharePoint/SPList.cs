using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SharePoint
{
#if General
    [Serializable]
#endif
    public class SPList : SPFolder
    {
        public SPList() : base() { }
        public SPList(Guid siteSettingID, string uniqueIdentifier, string title)
            : base(siteSettingID, uniqueIdentifier, title)
        {
        }
        public bool Hidden = false;
        public override string IconName
        {
            get
            {
                return "SPList";
            }
        }
    }
}
