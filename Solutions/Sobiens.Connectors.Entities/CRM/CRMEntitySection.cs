using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.CRM
{
    [Serializable]
    public class CRMEntitySection : CRMEntity
    {
        public CRMEntitySection() : base() { }
        public CRMEntitySection(Guid siteSettingID, string uniqueIdentifier, string schemaName, string title)
            : base()
        {
            SiteSettingID = siteSettingID;
            UniqueIdentifier = uniqueIdentifier;
            Title = title;
            this.SchemaName = schemaName;
            ContainsItems = true;
        }

        public override string IconName
        {
            get
            {
                return "CRMEntitySection";
            }
        }
    }
}
