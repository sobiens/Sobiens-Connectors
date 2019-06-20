using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Sobiens.WPF.Controls.Base;
using Sobiens.WPF.Controls.Classes;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;

namespace Sobiens.WPF.Controls.ExtendedGridControl
{
    public sealed class ExtendedDataGrid : CopyDataGrid
    {
        #region Members

        private static readonly Uri DataGriduri = new Uri("/Sobiens.WPF.Controls;component/Styles/DataGrid.Generic.xaml",
                                                          UriKind.Relative);

        private readonly ResourceDictionary _datagridStyle = new ResourceDictionary {Source = DataGriduri};

        #endregion

        #region Constructors

        public ExtendedDataGrid()
        {
            AddResouces();
            (Items.SortDescriptions as INotifyCollectionChanged).CollectionChanged += SortDescriptionsCollectionChanged;
            Sorting += DataGridStandardSorting;
        }

        #endregion

        #region "Attached Properties"

        public Boolean HideColumnChooser
        {
            get { return (Boolean) GetValue(HideColumnChooseProperty); }
            set { SetValue(HideColumnChooseProperty, value); }
        }

        public static readonly DependencyProperty HideColumnChooseProperty = DependencyProperty.Register(
            "HideColumnChooser", typeof (Boolean), typeof (ExtendedDataGrid), new PropertyMetadata(false));

        public Boolean ShowSortOrder
        {
            get { return (Boolean)GetValue(ShowSortOrderProperty); }
            set { SetValue(ShowSortOrderProperty, value); }
        }

        public static readonly DependencyProperty ShowSortOrderProperty = DependencyProperty.Register(
            "ShowSortOrder", typeof(Boolean), typeof(ExtendedDataGrid), new PropertyMetadata(false));

        #endregion

        #region Methods

        private void AddResouces()
        {
            if (!UriParser.IsKnownScheme("pack"))
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);


            //Datagrid
            if (!Resources.MergedDictionaries.Contains(_datagridStyle))
                Resources.MergedDictionaries.Add(_datagridStyle);
        }

        public void ClearFilter(string columnName)
        {
            AutoFilterHelper.RemoveAllFilter(this, columnName);
            Microsoft.Windows.Controls.Primitives.DataGridColumnHeader header =
                FindControls.GetColumnHeaderFromColumn(Columns[GetColumnIndex(Columns, columnName)]);
            if (header != null)
            {
                var popUp = FindControls.FindChild<Popup>(header, "popupDrag");
                if (popUp != null)
                {
                    popUp.Tag = "False";
                }
            }
        }

        private static int GetColumnIndex(IEnumerable<DataGridColumn> columns, string filedName)
        {
            int index = 0;
            foreach (DataGridColumn col in columns)
            {
                if (col.SortMemberPath == filedName)
                {
                    return index;
                }
                index++;
            }

            return index;
        }

        #endregion

        #region Events

        private void SortDescriptionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // clear all the headers of sort order
            foreach (DataGridColumn column in Columns)
            {
                string sortPath = SortHelpers.GetSortMemberPath(column);
                if (sortPath != null)
                {
                    column.Header = sortPath;
                }
            }

            // add sort order 
            int sortIndex = 0;
            foreach (SortDescription sortDesc in Items.SortDescriptions)
            {


                foreach (DataGridColumn column in Columns)
                {
                    if (sortDesc.PropertyName == SortHelpers.GetSortMemberPath(column))
                    {
                        var sb = new StringBuilder();
                        sb.Append(sortDesc.PropertyName);
                        if (Items.SortDescriptions.Count>1&&ShowSortOrder)
                        {
                            sb.Append(string.Format(" (Sort Order: {0})", sortIndex));
                            column.Header = sb.ToString();
                        }
                    }
                }
                sortIndex++;
            }
        }

        private void DataGridStandardSorting(object sender, DataGridSortingEventArgs e)
        {
           

            string sortPropertyName = SortHelpers.GetSortMemberPath(e.Column);
            if (!string.IsNullOrEmpty(sortPropertyName))
            {
                // sorting is cleared when the previous state is Descending
                if (e.Column.SortDirection.HasValue && e.Column.SortDirection.Value == ListSortDirection.Descending)
                {
                    int index = SortHelpers.FindSortDescription(Items.SortDescriptions, sortPropertyName);
                    if (index != -1)
                    {
                        e.Column.SortDirection = null;

                        // remove the sort description
                        Items.SortDescriptions.RemoveAt(index);
                        Items.Refresh();

                        if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
                        {
                            // clear any other sort descriptions for the multisorting case
                            Items.SortDescriptions.Clear();
                            Items.Refresh();
                        }

                        // stop the default sort
                        e.Handled = true;
                    }
                }
            }
        }

        #endregion
    }
}
