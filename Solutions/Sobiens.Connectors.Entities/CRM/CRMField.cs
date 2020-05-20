using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.CRM
{
#if General
    [Serializable]
#endif
    public class CRMField : Field
    {
        public CRMField():base()
        {
        }
        public string CRMFieldTypeName
        {
            get;set;
        }
    }
}
