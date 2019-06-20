using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface INavigatorExplorer
    {
        bool HasAnythingToDisplay { get; }
        void RefreshControls();
        IConnectorExplorer ConnectorExplorer { get; }
        void SetConnectorExplorer(IConnectorExplorer connectorExplorer);
        void SelectFolder(Folder folder);
    }
}
