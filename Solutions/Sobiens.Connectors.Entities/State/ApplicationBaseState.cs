using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.State
{
    public class ApplicationBaseState
    {
        public bool ConnectorOpen { get; set; }
        public bool IsHierarchyTreeViewOpen { get; set; }
        public int ConnectorWidth { get; set; }
        public int ConnectorHeight { get; set; }
        public ConnectorPaneDockPosition DockPosition { get; set; }
        public ApplicationBaseState() { DockPosition = ConnectorPaneDockPosition.DockPositionRight; }
    }
}
