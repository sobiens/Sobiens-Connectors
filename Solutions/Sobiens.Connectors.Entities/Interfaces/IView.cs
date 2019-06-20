using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IView
    {
        string UniqueIdentifier { get; set; }
        string Name { get; set; }
        int RowLimit { get; set; }
        Guid SiteSettingID { get; set; }
        List<CamlFieldRef> ViewFields { get; set; }
    }
}
