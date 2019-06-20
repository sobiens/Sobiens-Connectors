using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IWorkflowExplorer
    {
        bool HasAnythingToDisplay { get; }
        IConnectorExplorer ConnectorExplorer { get; }
        void SetConnectorExplorer(IConnectorExplorer connectorExplorer);
        bool IsDataLoaded { get; set; }
        void Initialize(SiteSettings siteSettings, List<WorkflowConfiguration> workflowConfiguration);
        void RefreshControls();
    }
}
