using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces
{
    public interface ISPCFolder
    {
        string UniqueIdentifier { get; set; }
        string Title { get; set; }
        bool ContainsItems { get; set; }
        EUSiteSetting SiteSetting { get; set; }
    }
}
