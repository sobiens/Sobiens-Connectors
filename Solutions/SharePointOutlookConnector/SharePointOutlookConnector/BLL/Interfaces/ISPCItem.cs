using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces
{
    public interface ISPCItem
    {
        string UniqueIdentifier { get; set; }
        string Title { get; set; }
        string URL { get; set; }
        EUSiteSetting SiteSetting { get; set; }
    }
}
