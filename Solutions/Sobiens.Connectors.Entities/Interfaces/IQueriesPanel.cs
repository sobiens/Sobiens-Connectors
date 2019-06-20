using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IQueriesPanel
    {
        IQueryPanel AddNewQueryPanel(QueryPanelObject queryPanelObject);
        IQueryPanel AddNewQueryPanel(Folder folder, ISiteSetting siteSetting);
        IQueryPanel ActiveQueryPanel { get; }
        IQueryPanel GetQueryPanel(Guid id);
        List<IQueryPanel> QueryPanels { get; }
        QueryProjectObject QueryProject { get; }
        void Save();
        void Load();
    }
}
