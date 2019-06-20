using Sobiens.Connectors.Entities.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IQueryPanel
    {
        Guid ID { get; set; }
        ISiteSetting SiteSetting { get; }
        Folder AttachedObject { get; }
        string FileName { get; }
        void Initialize(string filename, ISiteSetting siteSetting, Folder attachedObject);
        void PopulateResults(ISiteSetting siteSetting, string webUrl, string listName, CamlFilters _filters, List<CamlFieldRef> _viewFields, List<CamlOrderBy> _orderBys, CamlQueryOptions _queryOptions, string folderServerRelativePath);
        List<CamlFieldRef> GetViewFields();
        List<CamlFieldRef> GetAllFields();
        List<CamlOrderBy> GetOrderBys();
        CamlFilters GetFilters();
        CamlQueryOptions GetQueryOptions();
        void ChangeCriteriaPaneVisibility();
        void ChangeCamlTextPaneVisibility();
        void ChangeResultsPaneVisibility();
        QueryPanelObject GetQueryPanel();
        void Load(QueryPanelObject queryPanelObject);
    }
}
