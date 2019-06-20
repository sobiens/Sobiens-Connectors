using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces
{
    public interface ISPCView
    {
        string UniqueIdentifier { get; set; }
        string Name { get; set; }
        int RowLimit { get; set; }
        EUSiteSetting SiteSetting { get; set; }
        List<EUCamlFieldRef> ViewFields { get; set; }
    }
}
