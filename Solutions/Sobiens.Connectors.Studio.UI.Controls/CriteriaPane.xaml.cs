using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Data;
using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    public delegate void CriteriaPaneAfter_CriteriaChange();

    /// <summary>
    /// Interaction logic for CriteriaPane.xaml
    /// </summary>
    public partial class CriteriaPane : UserControl
    {
        private bool isInitialized = false;
        public event CriteriaPaneAfter_CriteriaChange After_CriteriaChange;

        public List<string> SortTypes
        {
            get
            {
                List<string> sortTypes = new List<string>();
                sortTypes.Add("Ascending");
                sortTypes.Add("Descending");
                sortTypes.Add("Unsorted");
                return sortTypes;
            }
        }

        public CriteriaPane()
        {
            InitializeComponent();
            SortTypeColumn.ItemsSource = this.SortTypes;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void Initialize(Folder selectedObject)
        {
            SiteSetting siteSetting = ApplicationContext.Current.Configuration.SiteSettings[selectedObject.SiteSettingID];
            FieldCollection fields = ApplicationContext.Current.GetFields(siteSetting, selectedObject);
            var query = from field in fields
                        select new CriteriaPaneItem() { FieldInternalName = field.Name, FieldName = field.DisplayName, FieldType = field.Type, Output = false, SortType = string.Empty, SortOrder = string.Empty, Filter1 = string.Empty, Filter2 = string.Empty, Filter3 = string.Empty, Filter4 = string.Empty };

            List<CriteriaPaneItem> items = query.ToList();
            List<string> sortOrders = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                sortOrders.Add((i + 1).ToString());
            }
            sortOrders.Add("Unsorted");
            SortOrderColumn.ItemsSource = sortOrders;
            CriteriaGrid.ItemsSource = items;
            isInitialized = true;
        }

        public void Initialize(Folder selectedObject, List<CamlFieldRef> viewFields, CamlQueryOptions queryOptions, List<CamlOrderBy> orderBys, CamlFilters filters)
        { 
        
        }

        public List<CamlFieldRef> GetViewFields()
        {
            List<CamlFieldRef> viewFields = new List<CamlFieldRef>();
            List<CriteriaPaneItem> items = (List<CriteriaPaneItem>)CriteriaGrid.ItemsSource;
            foreach(CriteriaPaneItem item in items)
            {
                if (item.Output == true)
                    viewFields.Add(new CamlFieldRef(item.FieldInternalName, item.FieldName) );
            }

            return viewFields;
        }

        public List<CamlFieldRef> GetAllFields()
        {
            List<CamlFieldRef> viewFields = new List<CamlFieldRef>();
            List<CriteriaPaneItem> items = (List<CriteriaPaneItem>)CriteriaGrid.ItemsSource;
            foreach (CriteriaPaneItem item in items)
            {
                viewFields.Add(new CamlFieldRef(item.FieldInternalName, item.FieldName));
            }

            return viewFields;
        }
        public List<CamlOrderBy> GetOrderBys()
        {
            List<CamlOrderBy> orderBys = new List<CamlOrderBy>();
            List<CriteriaPaneItem> items = (List<CriteriaPaneItem>)CriteriaGrid.ItemsSource;
            var query = from item in items
                        orderby item.SortOrder
                        where string.IsNullOrEmpty(item.SortType) == false
                            && item.SortType.Equals("Unsorted", StringComparison.InvariantCultureIgnoreCase) == false
                            && item.SortOrder.Equals("Unsorted", StringComparison.InvariantCultureIgnoreCase) == false
                        select item;
            List<CriteriaPaneItem> items1 = query.ToList();
            foreach (CriteriaPaneItem item in items1)
            {
                    orderBys.Add(new CamlOrderBy()
                    {
                        FieldName = item.FieldInternalName,
                        IsAsc = (item.SortType.Equals("Ascending", StringComparison.InvariantCultureIgnoreCase) == true ? true : false)
                    });
            }

            return orderBys;
        }

        public CamlFilters GetFilters()
        {
            CamlFilters filters = new CamlFilters();
            filters.IsOr = false;
            List<CriteriaPaneItem> items = (List<CriteriaPaneItem>)CriteriaGrid.ItemsSource;
            foreach (CriteriaPaneItem item in items)
            {
                CamlFilters filters1 = new CamlFilters();
                filters1.IsOr = true;
                if (string.IsNullOrEmpty(item.Filter1) == false)
                {
                    CamlFilter filter = new CamlFilter(item.FieldInternalName, item.FieldType, CamlFilterTypes.Equals, item.Filter1);
                    filters1.Add(filter);
                }
                if (string.IsNullOrEmpty(item.Filter2) == false)
                {
                    CamlFilter filter = new CamlFilter(item.FieldInternalName, item.FieldType, CamlFilterTypes.Equals, item.Filter2);
                    filters1.Add(filter);
                }
                if (string.IsNullOrEmpty(item.Filter3) == false)
                {
                    CamlFilter filter = new CamlFilter(item.FieldInternalName, item.FieldType, CamlFilterTypes.Equals, item.Filter3);
                    filters1.Add(filter);
                }
                if (string.IsNullOrEmpty(item.Filter4) == false)
                {
                    CamlFilter filter = new CamlFilter(item.FieldInternalName, item.FieldType, CamlFilterTypes.Equals, item.Filter4);
                    filters1.Add(filter);
                }

                if (filters1.Filters.Count > 0)
                    filters.Add(filters1);
            }
            return filters;
        }

        public CamlQueryOptions GetQueryOptions()
        {
            CamlQueryOptions queryOptions = new CamlQueryOptions()
            {
                IncludeMandatoryColumns = false
            };
            return queryOptions;
        }

        private void CriteriaGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (After_CriteriaChange != null)
                After_CriteriaChange();
        }

    }
}
