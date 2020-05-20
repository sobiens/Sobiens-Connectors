using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface ISPCamlStudio
    {
        IQueriesPanel QueriesPanel { get; }
        IQueryDesignerToolbar QueryDesignerToolbar { get; }
        IServerObjectExplorer ServerObjectExplorer { get; }

        void ReportProgress(int? percentage, string message);
    }
}
