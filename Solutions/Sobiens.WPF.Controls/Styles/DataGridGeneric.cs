using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using Sobiens.WPF.Controls.Classes;
using Sobiens.WPF.Controls.ExtendedGridControl;
using Sobiens.WPF.Controls.Interface;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using System.Linq;


namespace Sobiens.WPF.Controls.Styles
{
    public partial  class DataGridGeneric
    {
        private void AutoFilterMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                e.Handled = true;
               
                if (e.ChangedButton == MouseButton.Right)
                    return;
                var column = FindControls.FindParent<Microsoft.Windows.Controls.Primitives.DataGridColumnHeader>((ContentControl)sender);

                var extendedColumn = column.Column as IExtendedColumn;
                if(extendedColumn!=null)
                {
                    if(!extendedColumn.AllowAutoFilter)
                        return;
                }

                var popup = FindControls.FindChild<Popup>(column, "popupDrag");

                if (popup == null) return;
                popup.IsOpen = true;
                //Change the position of popup with the mouse
                var popupSize = new Size(popup.ActualWidth, popup.ActualHeight);
                var position = new Point { X = column.ActualWidth - 19, Y = column.ActualHeight };
                popup.PlacementRectangle = new Rect(position, popupSize);
                var listbox = FindControls.FindChild<ListBox>(popup.Child, "autoFilterList");
                listbox.Focus();
                listbox.LostFocus += PopLostFocus;
                var clearButton = FindControls.FindChild<Button>(popup.Child, "btnClear");
                //Get the data from
                var mainGrid = FindControls.FindParent<ExtendedDataGrid>(popup);
                if (mainGrid != null)
                {
                    mainGrid.CommitEdit();
                    mainGrid.CommitEdit();
                    AutoFilterHelper.CurrentListBox = listbox;
                    AutoFilterHelper.CurrentGrid = mainGrid;
                    CurrentColumn = column.Column.SortMemberPath;
                    CurrentGrid = mainGrid;

                    if(AutoFilterHelper.CurrentColumName==CurrentColumn)
                    {
                        List<CheckedListItem> previousValues = AutoFilterHelper.CurrentDistictValues;
                        var currentValues = AutoFilterHelper.GetDistictValues(CurrentGrid, CurrentColumn);

                        foreach (var checkedListItem in currentValues)
                        {
                            if(previousValues.Count(c=>c.Name==checkedListItem.Name&&c.IsSelectAll!="(Select All)")==0&&checkedListItem.IsSelectAll!="(Select All)")
                            {
                              
                                previousValues.Add(new CheckedListItem { Name = checkedListItem.Name,IsChecked =previousValues[0].IsChecked});
                            }
                           
                        }
                        
                        if (clearButton != null)
                        {
                            clearButton.IsEnabled = AutoFilterHelper.CurrentDistictValues.Count(c => c.IsChecked) > 0;
                            clearButton.UpdateLayout();
                        }
                        AutoFilterHelper.CurrentDistictValues = previousValues;
                        AutoFilterHelper.CurrentDistictValues[0].IsSelectAll = "(Select All)";
                        listbox.ItemsSource = AutoFilterHelper.CurrentDistictValues;
                        listbox.UpdateLayout();
                        listbox.Items.Refresh();
                        return;
                    }

                  
                    AutoFilterHelper.CurrentColumName = CurrentColumn;
                    var distict = AutoFilterHelper.GetDistictValues(CurrentGrid, CurrentColumn);

                    if (distict.Count(c=>!c.IsChecked)==distict.Count)
                    {
                        if(Convert.ToString(popup.Tag)=="True")
                        {
                            _isLoading = true;
                            foreach (var checkedListItem in distict)
                            {
                                checkedListItem.IsChecked = true;
                            }
                        }
                    }

                    distict[0].IsSelectAll = "(Select All)";
                    listbox.ItemsSource = distict;
                    if (clearButton != null)
                    {
                        clearButton.IsEnabled = AutoFilterHelper.CurrentDistictValues.Count(c => c.IsChecked) > 0;
                        clearButton.Foreground=new SolidColorBrush(Colors.Black);
                        clearButton.UpdateLayout();
                    }
                       

                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        

        private void AutoFilterRightMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void PopLostFocus(object sender, RoutedEventArgs e)
        {
            var stackPanel = sender as StackPanel == null ? FindControls.FindParent<StackPanel>((FrameworkElement)sender) : (StackPanel)sender;
            if(stackPanel==null)return;
            var popup = (Popup)stackPanel.Parent;
            if(popup==null)return;
            if(popup.IsMouseOver)return;
            var currentFocueedElement = FocusManager.GetFocusedElement(popup);
            if(currentFocueedElement==null)
             popup.IsOpen = false;
        }

        private bool _isLoading;

        private void Checked(object sender, RoutedEventArgs e)
        {
            if(_isLoading)return;
            try
            {
                var checkbox = (CheckBox)sender;
                var value = Convert.ToString(checkbox.Content);
                if (Convert.ToString(checkbox.Tag)!="(Select All)")
                {
                    AutoFilterHelper.ApplyFilters(CurrentGrid, CurrentColumn, value);
                }
                else
                {
                    _isLoading = true;
                    var distictictValues = AutoFilterHelper.CurrentDistictValues;
                    foreach (var distictictValue in distictictValues)
                    {
                        if (distictictValue.Name != "(Select All)" && distictictValue.IsSelectAll!="(Select All)")
                          distictictValue.IsChecked = true;
                    }
                    AutoFilterHelper.CurrentDistictValues = distictictValues;
                    AutoFilterHelper.RemoveAllFilter(CurrentGrid, CurrentColumn);
                    foreach (var c in distictictValues)
                    {
                        c.IsChecked = true;
                    }
                    var sp1 = FindControls.FindParent<StackPanel>(AutoFilterHelper.CurrentListBox);
                    var popup1 = sp1.Parent as Popup;
                    if (popup1 != null)
                    {
                        var clearButton = FindControls.FindChild<Button>(popup1.Child, "btnClear");
                        if (clearButton != null)
                            clearButton.IsEnabled = true;
                    }
                    AutoFilterHelper.CurrentListBox.UpdateLayout();
                    AutoFilterHelper.CurrentListBox.Items.Refresh();
                    return;
                }
               
                var sp = FindControls.FindParent<StackPanel>(AutoFilterHelper.CurrentListBox);
                var popup = sp.Parent as Popup;
                if (popup != null) popup.Tag = "True";
                AutoFilterHelper.CurrentListBox.UpdateLayout();
                AutoFilterHelper.CurrentListBox.Items.Refresh();
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void UnChecked(object sender, RoutedEventArgs e)
        {
            if (_isLoading) return;
            try
            {
                _isLoading = true;
                var checkbox = (CheckBox)sender;
                var listBox = FindControls.FindParent<ListBox>(checkbox);
                AutoFilterHelper.CurrentListBox = listBox;
                var popup = (Popup)((StackPanel)(listBox.Parent)).Parent;
                var grid = FindControls.FindParent<ExtendedDataGrid>(popup);
                CurrentGrid = AutoFilterHelper.CurrentGrid = grid;
                var value = Convert.ToString(checkbox.Content);
                if (Convert.ToString(checkbox.Tag) != "(Select All)")
                {
                    
                    var distictValues = AutoFilterHelper.CurrentListBox.ItemsSource as List<CheckedListItem>;
                    if(distictValues!=null)
                    {
                        var countOfFiltersSelected = distictValues.Count(c => c.IsChecked && c.Name != "(Select All)" && c.IsSelectAll!="(Select All)");
                        if(countOfFiltersSelected==distictValues.Count-1)
                        {
                            _isLoading = true;
                            distictValues[0].IsChecked = false;

                     
                            AutoFilterHelper.CurrentListBox.UpdateLayout();
                            AutoFilterHelper.CurrentListBox.Items.Refresh();
                        }

                        bool isFilterApplicable = !string.IsNullOrEmpty(((DataView)CurrentGrid.ItemsSource).RowFilter) && ((DataView)CurrentGrid.ItemsSource).RowFilter.Contains(CurrentColumn+"  IN ");
                        if (!isFilterApplicable)
                        {
                            if (string.IsNullOrEmpty(((DataView)CurrentGrid.ItemsSource).RowFilter))
                            {
                                var actualValues = ((DataView) CurrentGrid.ItemsSource).Table.Rows.Cast<DataRow>().Select(row => Convert.ToString(row[CurrentColumn])).Where(val => val != value).ToList();


                                AutoFilterHelper.ApplyFilters(CurrentGrid, CurrentColumn, actualValues);
                               
                               
                                var distictictValues = AutoFilterHelper.CurrentDistictValues;
                                distictictValues[0].IsChecked = false;
                                AutoFilterHelper.CurrentDistictValues = distictictValues;
                                AutoFilterHelper.CurrentListBox.Items.Refresh();
                                popup.Tag = distictictValues.Count(c => c.IsChecked) == 0 ? "False" : "True";
                                CurrentGrid.Items.Refresh();
                                CurrentGrid.UpdateLayout();
                                return;
                            }
                            else
                            {
                                AutoFilterHelper.CurrentDistictValues[0].IsChecked=countOfFiltersSelected==distictValues.Count-1;
                                AutoFilterHelper.CurrentListBox.Items.Refresh();
                                AutoFilterHelper.CurrentListBox.UpdateLayout();
                                popup.Tag = AutoFilterHelper.CurrentDistictValues.Count(c => c.IsChecked) == 0 ? "False" : "True";
                                CurrentGrid.Items.Refresh();
                                CurrentGrid.UpdateLayout();
                            }
                        }
                        else
                        {
                            AutoFilterHelper.RemoveFilters(CurrentGrid, CurrentColumn, value);
                            var distictictValues = AutoFilterHelper.CurrentDistictValues;
                            popup.Tag = distictictValues.Count(c => c.IsChecked) == 0 ? "False" : "True";
                        }
                    }
                }
                   
                else
                {
                    _isLoading = true;
                    var distictictValues = AutoFilterHelper.CurrentDistictValues;
                    distictictValues[0].IsChecked = false;
                    foreach (var distictictValue in distictictValues)
                    {
                        if (distictictValue.Name != "(Select All)" && distictictValue.IsSelectAll != "(Select All)")
                            distictictValue.IsChecked = false;
                    }
                    AutoFilterHelper.CurrentDistictValues = distictictValues;
                    AutoFilterHelper.RemoveAllFilter(CurrentGrid, CurrentColumn);
                    AutoFilterHelper.CurrentListBox.Items.Refresh();
                    var countOfFiltersSelected = distictictValues.Count(c => c.IsChecked && c.Name != "(Select All)");
                    popup.Tag =countOfFiltersSelected==0?"False":"True";
                }
                AutoFilterHelper.CurrentGrid.Items.Refresh();
            }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            finally
            {
                _isLoading = false;
            }
        }

        private string CurrentColumn { get;  set; }
        private Microsoft.Windows.Controls.DataGrid CurrentGrid { get; set; }

        private void SortByAscButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SortByDescButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ClearClicked(object sender, RoutedEventArgs e)
        {
            try
            {
             
                var mainGrid = AutoFilterHelper.CurrentGrid;
                if(mainGrid==null)return;
                _isLoading = true;
                var distictictValues = AutoFilterHelper.CurrentDistictValues;
                foreach (var distictictValue in distictictValues)
                {
                    if (distictictValue.IsChecked)
                        distictictValue.IsChecked = false;
                }
                AutoFilterHelper.CurrentDistictValues = distictictValues;
                AutoFilterHelper.RemoveAllFilter(CurrentGrid, CurrentColumn);
                AutoFilterHelper.CurrentListBox.Items.Refresh();
                AutoFilterHelper.RemoveAllFilter(mainGrid, AutoFilterHelper.CurrentColumName);
                var sp = FindControls.FindParent<StackPanel>(AutoFilterHelper.CurrentListBox);
                var popup = sp.Parent as Popup;
                if (popup != null) popup.Tag = "False";
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void AutoFilterGlphLoaded(object sender, RoutedEventArgs e)
        {

            var column = FindControls.FindParent<Microsoft.Windows.Controls.Primitives.DataGridColumnHeader>((ContentControl)sender);
            if(column==null)
                return;
            var extendedColumn = column.Column as IExtendedColumn;
              
            if(extendedColumn != null && !extendedColumn.AllowAutoFilter)
            {
                ((ContentControl)(sender)).Visibility = Visibility.Collapsed;
            }
        }

    }

    public class CheckedListItem
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public string IsSelectAll { get; set; }
    }
}
