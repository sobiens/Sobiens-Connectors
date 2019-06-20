using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IConnectorExplorer
    {
        bool HasAnythingToDisplay { get; }
        IContentExplorer ContentExplorer { get; }
//        INavigatorExplorer BreadcrumbNavigatorExplorer { get; }
        INavigatorExplorer TreeviewNavigatorExplorer { get; }
        bool IsDataLoaded { get; set; }
        void Initialize(SiteSettings siteSettings, List<ExplorerLocation> explorerLocations);
        void RefreshControls();
    }
}
