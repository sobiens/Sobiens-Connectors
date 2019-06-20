using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IItem
    {
        string UniqueIdentifier { get; set; }
        string Title { get; set; }
        string URL { get; set; }
        Guid SiteSettingID { get; set; }
        System.Collections.Generic.Dictionary<string, string> Properties { get; set; }
        string GetListItemURL();
        int GetMinorVersion();
        int GetMajorVersion();
        string GetID();
        bool isExtracted();
        bool isFolder();
    }
}
