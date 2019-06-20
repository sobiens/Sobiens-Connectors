using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.SkyDrive
{
#if General
    [Serializable]
#endif
    public class SDFolder : Folder
    {
        public SDFolder(Guid siteSettingID, string uniqueIdentifier, string title)
            : base()
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            ContainsItems = true;
        }
        public override string IconName
        {
            get
            {
                return "FOLDER";
            }
        }

        public override string GetWebUrl()
        {
            throw new NotImplementedException();
        }

        public override string GetUrl()
        {
            throw new NotImplementedException();
        }

        public override string GetRoot()
        {
            throw new NotImplementedException();
        }

        public override string GetPath()
        {
            throw new NotImplementedException();
        }

        public override string GetListName()
        {
            throw new NotImplementedException();
        }
    }
}
