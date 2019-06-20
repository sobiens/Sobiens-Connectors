using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.State
{
    public enum ConnectorPaneDockPosition
    {
        // Summary:
        //     Dock the task pane on the left side of the document window.
        DockPositionLeft = 0,
        //
        // Summary:
        //     Dock the task pane at the top of the document window.
        DockPositionTop = 1,
        //
        // Summary:
        //     Dock the task pane on the right side of the document window.
        DockPositionRight = 2,
        //
        // Summary:
        //     Dock the task pane at the bottom of the document window.
        DockPositionBottom = 3,
        //
        // Summary:
        //     Don't dock the task pane.
        DockPositionFloating = 4,
    }
}
