using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUListItemVersion
    {
        public EUListItemVersion(EUSiteSetting siteSetting)
        {
            SiteSetting = siteSetting;
        }
        public string Version;
        public string URL;
        public EUSiteSetting SiteSetting;
        public string WebURL;
        public string Created;
        public string CreatedBy;
        public string Comments;
        public string Size;
    }
}
