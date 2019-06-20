using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities
{
    public interface IConnectorMainView
    {
        object Inspector{get;set;}
        IDocumentTemplateSelection DocumentTemplateSelection { get; }
        ISearchExplorer SearchExplorer { get; }
        IConnectorExplorer ConnectorExplorer { get; }
        IWorkflowExplorer WorkflowExplorer { get; }
        IStatusBar StatusBar { get; }
        void RefreshControls();
        DateTime InitializedDate { get; set; }
    }
}
