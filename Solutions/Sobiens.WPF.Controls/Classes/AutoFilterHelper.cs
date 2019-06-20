using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Sobiens.WPF.Controls.ExtendedGridControl;
using Sobiens.WPF.Controls.Styles;
using Microsoft.Windows.Controls;
using System;
using Microsoft.Windows.Controls.Primitives;

namespace Sobiens.WPF.Controls.Classes
{
    internal sealed class AutoFilterHelper
    {

        public static List<CheckedListItem> CurrentDistictValues { get; set; }
        public static ListBox CurrentListBox { get; set; }
        public static string CurrentColumName { get; set; }

        public static ExtendedDataGrid CurrentGrid { get; set; }
      

        public static List<CheckedListItem> GetDistictValues(Microsoft.Windows.Controls.DataGrid grid, string columnName)
        {
            var itemSource = grid.ItemsSource as DataView;
            var ditictValues = new List<CheckedListItem> { new CheckedListItem { IsChecked = false, Name = "(Select All)", IsSelectAll = "(Select All)" } };
            if (itemSource == null)
                return null;

            var table = itemSource.Table;
            var filteredValues = GetFilteredColumnValues(columnName, itemSource);
            foreach (DataRow row in table.Rows)
            {
                var value = Convert.ToString(row[columnName]);
                if (ditictValues.Count(c => c.Name.Equals(value)&&c.IsSelectAll!="(Select All)") == 0 )
                {
                    //value = AddEscapeSequece(value);
                    ditictValues.Add(new CheckedListItem { Name = value, IsChecked = filteredValues.Contains("'"+value+"'") });
                }
              
            }
            if (ditictValues.Count(c => c.IsChecked) == ditictValues.Count - 1)
                ditictValues[0].IsChecked = true;
            CurrentDistictValues = ditictValues;
            return ditictValues;
        }



        public static void ApplyFilters(Microsoft.Windows.Controls.DataGrid grid, string columnName, string value)
        {
            value = AddEscapeSequece(value);
            var itemSource = (DataView) grid.ItemsSource;
            if (itemSource == null) return;
            if (!string.IsNullOrEmpty(itemSource.RowFilter))
            {
                
                itemSource.RowFilter = FilterGenerator(itemSource.RowFilter, columnName, value);
            }
            else
                itemSource.RowFilter = columnName+" "+" IN "+"("+"'"+value+"'"+")" ;
            var count = CurrentDistictValues.Count(c => c.IsChecked);
            if (count == CurrentDistictValues.Count - 1)
            {
                CurrentDistictValues[0].IsChecked = true;
                if (CurrentListBox != null)
                {
                    CurrentListBox.ItemsSource = CurrentDistictValues;
                    CurrentListBox.UpdateLayout();
                    CurrentListBox.Items.Refresh();
                }
            }

            if (CurrentListBox != null)
            {
                var clearButton = FindControls.FindChild<Button>(CurrentListBox.Parent, "btnClear");
                if (clearButton != null)
                {
                    clearButton.IsEnabled = CurrentDistictValues.Count(c => c.IsChecked) > 0;
                }
            }
        }

        public static void ApplyFilters(Microsoft.Windows.Controls.DataGrid grid, string columnName, List<String> values)
        {
            var actualValue = "";
            foreach (var val in values)
            {
                var temp = AddEscapeSequece(val);
                if (actualValue == "")
                {
                    actualValue = temp + "'";
                }
                else
                {
                    actualValue = actualValue + "," + "'" + temp + "'";
                }
            }
            actualValue = actualValue.Substring(0, actualValue.Length - 1);
            var value = actualValue;
            var itemSource = (DataView)grid.ItemsSource;
            if (itemSource == null) return;
            if (!string.IsNullOrEmpty(itemSource.RowFilter))
            {

                itemSource.RowFilter = FilterGenerator(itemSource.RowFilter, columnName, value);
            }
            else
                itemSource.RowFilter = columnName + " " + " IN " + "(" + "'" + value + "'" + ")";
            var count = CurrentDistictValues.Count(c => c.IsChecked);
            if (count == CurrentDistictValues.Count - 1)
            {
                CurrentDistictValues[0].IsChecked = true;
                if (CurrentListBox != null)
                {
                    CurrentListBox.ItemsSource = CurrentDistictValues;
                    CurrentListBox.UpdateLayout();
                    CurrentListBox.Items.Refresh();
                }
            }

            if (CurrentListBox != null)
            {
                var clearButton = FindControls.FindChild<Button>(CurrentListBox.Parent, "btnClear");
                if (clearButton != null)
                {
                    clearButton.IsEnabled = CurrentDistictValues.Count(c => c.IsChecked) > 0;
                }
            }
        }

        private static string AddEscapeSequece(string value)
        {
            var newValue = value.Replace("'","''");
          

            return newValue;
        }


        private static string FilterGenerator(string exisitngFilter,string columnName,string value)
        {
            var newFilter = exisitngFilter;

            if (newFilter.Contains(columnName + " " + " IN "))
            {
                var startIndex = newFilter.IndexOf(columnName + " " + " IN ", StringComparison.Ordinal);
                var actaulFilter = newFilter.Substring(startIndex + (columnName + " " + " IN ").Length + 1);
                var lastIndex = actaulFilter.IndexOf(")", StringComparison.Ordinal);
                actaulFilter = actaulFilter.Substring(0, lastIndex);
                var listOfFilter = actaulFilter.Split(',');
                if (listOfFilter.Contains("'" + value + "'"))
                    return exisitngFilter;
                newFilter = newFilter.Insert(startIndex + (columnName + " " + " IN ").Length + 1 + actaulFilter.Length, "," + "'" + value + "'");
                return newFilter;
            }
            newFilter = "(" + newFilter + ")";
            newFilter = newFilter + " AND " +"("+ columnName + " " + " IN " + "(" + "'" + value + "'" + ")"+")";

            return newFilter;
        }

        private static List<string> GetFilteredColumnValues(string columnName,DataView view)
        {
          
            var newFilter = view.RowFilter;
            if (newFilter.Contains(columnName + " " + " IN "))
            {
                var startIndex = newFilter.IndexOf(columnName + " " + " IN ", StringComparison.Ordinal);
                var actaulFilter = newFilter.Substring(startIndex + (columnName + " " + " IN ").Length + 1);
                var lastIndex = actaulFilter.IndexOf(")", StringComparison.Ordinal);
                actaulFilter = actaulFilter.Substring(0, lastIndex);
                var listOfFilter = actaulFilter.Split(',');
                var list= listOfFilter.ToList();
                var newList = new List<String>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].IndexOf("'") == 0 && list[i].LastIndexOf("'") == list[i].Length - 1)
                    {
                        newList.Add(list[i]);
                        continue;
                    }
                    else
                    {
                        var newfilterYoBeAdded = list[i];
                        
                        for (int j = i + 1; j < list.Count; j++)
                        {
                            i = j;
                            var current =  list[j];
                            if (current.LastIndexOf("'") == current.Length - 1)
                            {
                                if (current.Length > 1 && current[current.Length - 2].ToString() != "'")
                                {
                                    newfilterYoBeAdded = newfilterYoBeAdded + "," + current;
                                    newList.Add(newfilterYoBeAdded);
                                    break;
                                }
                            }
                            else
                            {
                                newfilterYoBeAdded = newfilterYoBeAdded+","+ current;
                            }
                        }
                    }

                }

                return newList;
            }

            return new List<string>();
        }


        public static void RemoveAllFilter(Microsoft.Windows.Controls.DataGrid currentGrid, string currentColumn)
        {
            var itemSource = (DataView)currentGrid.ItemsSource;
            if (itemSource == null) return;
            var newFilter = itemSource.RowFilter;
            if(string.IsNullOrEmpty(newFilter))return;

            if (newFilter.Contains(currentColumn + " " + " IN "))
            {
                var startIndex = newFilter.IndexOf(currentColumn + " " + " IN ", StringComparison.Ordinal);
                var actaulFilter = newFilter.Substring(startIndex + (currentColumn + " " + " IN ").Length + 1);
                var lastIndex = actaulFilter.IndexOf(")", StringComparison.Ordinal);
                actaulFilter = actaulFilter.Substring(0, lastIndex);
                var relalValue = currentColumn + " " + " IN " + "(" + actaulFilter + ")";
                if(newFilter.Contains("("+relalValue+")"))
                {
                    newFilter = newFilter.Replace("(" + relalValue + ")", "");
                    if(newFilter.IndexOf("( AND (", StringComparison.Ordinal)==0)
                    {
                        newFilter = newFilter.Replace("( AND (", "");
                    }
                    if (newFilter.IndexOf("(((", StringComparison.Ordinal) == 0)
                    {
                        newFilter = newFilter.Substring(2);
                    }
                    if (newFilter.Contains("))) AND "))
                    {
                       newFilter= newFilter.Replace("))) AND ", ")");
                    }

                    if (newFilter.LastIndexOf(" AND", StringComparison.Ordinal)==newFilter.Length-5)
                    {
                        newFilter = newFilter.Substring(0, newFilter.Length - 5);
                    }
                    if (newFilter.Contains(" AND )"))
                    {
                        newFilter = newFilter.Replace(" AND )", "");
                    }
                    if (newFilter.IndexOf("((", StringComparison.Ordinal)==0)
                    {
                        newFilter = newFilter.Substring(2);
                    }
                    if (newFilter.IndexOf(" AND ", StringComparison.Ordinal)==0)
                    {
                        newFilter = newFilter.Substring(5);
                    }
                    itemSource.RowFilter = newFilter;
                }
                else
                {
                    newFilter = newFilter.Replace(relalValue, "");
                    if (newFilter == "')")
                    {
                        newFilter = "";
                        itemSource.RowFilter = newFilter;

                        var stackPanel = CurrentListBox.Parent as StackPanel;
                        if (stackPanel != null)
                        {
                            var popup = stackPanel.Parent as Popup;
                            if(popup!=null)
                            {
                                popup.Tag = "True";
                                
                            }
                        }

                        return;
                    }
                       
                    itemSource.RowFilter = newFilter;
                }

            }
            var count = CurrentDistictValues.Count(c => c.IsChecked);
            if (count == CurrentDistictValues.Count - 1)
            {
                CurrentDistictValues[0].IsChecked = true;
                if (CurrentListBox != null)
                {
                    CurrentListBox.ItemsSource = CurrentDistictValues;
                    CurrentListBox.UpdateLayout();
                    CurrentListBox.Items.Refresh();
                }
            }
            if (CurrentListBox != null)
            {
                var clearButton = FindControls.FindChild<Button>(CurrentListBox.Parent, "btnClear");
                if (clearButton != null)
                {
                    clearButton.IsEnabled = CurrentDistictValues.Count(c => c.IsChecked) > 0;
                }
            }
        }

        public static void RemoveFilters(Microsoft.Windows.Controls.DataGrid grid, string columnName, string value)
        {
            value = AddEscapeSequece(value);
            var itemSource = (DataView)grid.ItemsSource;
            if (itemSource == null) return;
            if (!string.IsNullOrEmpty(itemSource.RowFilter))
            {

                var newFilter = itemSource.RowFilter;
                if (newFilter.Contains(columnName + " " + " IN "))
                {
                    var startIndex = newFilter.IndexOf(columnName + " " + " IN ", StringComparison.Ordinal);
                    var actaulFilter = newFilter.Substring(startIndex + (columnName + " " + " IN ").Length + 1);
                    var lastIndex = actaulFilter.IndexOf(")", StringComparison.Ordinal);
                    actaulFilter = actaulFilter.Substring(0, lastIndex);
                    var listOfFilter = actaulFilter.Split(',');
                    if (listOfFilter.Contains("'" + value + "'"))
                    {
                        var realFilter = columnName + " " + " IN " + "(" + actaulFilter + ")";
                        var replaced = realFilter.Replace("'" + value + "'", "");
                        if(replaced.Contains(",,"))
                        {
                            replaced = replaced.Replace(",,", ",");
                        }
                        if (replaced.Contains(",)"))
                        {
                            replaced = replaced.Replace(",)", ")");
                        }
                        if (replaced.Contains("()"))
                        {
                            
                            replaced = "";
                        }
                        if(replaced.Contains("(,"))
                        {
                            replaced = replaced.Replace("(,", "(");
                        }
                        if (replaced.Contains(",)"))
                        {
                            replaced = replaced.Replace(",)", ")");
                        }
                        if (newFilter.Contains(" AND ()"))
                        {
                           newFilter= newFilter.Replace(" AND ()", "");
                        }
                        newFilter = newFilter.Replace(realFilter, replaced);
                        if (newFilter.Contains("() AND "))
                        {
                            newFilter = newFilter.Replace("() AND ", "");
                        }
                        if (newFilter.IndexOf("(((", StringComparison.Ordinal) == 0)
                        {
                            newFilter = newFilter.Substring(2);
                        }
                        if (newFilter.Contains(")))"))
                        {
                            newFilter = newFilter.Replace(")))", "))");
                        }
                        if (newFilter.Contains("()"))
                        {
                            newFilter = newFilter.Replace("()", "");
                        }
                        if (newFilter.LastIndexOf(" AND ", StringComparison.Ordinal) == newFilter.Length-5)
                        {
                            newFilter = newFilter.Substring(0, newFilter.Length - 5);
                        }
                        if(newFilter.IndexOf("((", StringComparison.Ordinal)==0)
                        {
                            newFilter = newFilter.Substring(1);
                        }
                        
                       switch (newFilter)
                       {
                           case "":
                               itemSource.RowFilter = null;
                               return;
                           default:
                               itemSource.RowFilter = newFilter;
                               break;
                       }

                        var count1 = CurrentDistictValues.Count(c => c.IsChecked&&c.Name!="(Select All)");
                        if (count1 == CurrentDistictValues.Count - 1)
                        {
                            CurrentDistictValues[0].IsChecked = true;
                            if (CurrentListBox != null)
                            {
                                CurrentListBox.ItemsSource = CurrentDistictValues;
                                CurrentListBox.UpdateLayout();
                                CurrentListBox.Items.Refresh();
                            }
                        }
                        else
                        {
                            CurrentDistictValues[0].IsChecked = false;
                            if (CurrentListBox != null)
                            {
                                CurrentListBox.ItemsSource = CurrentDistictValues;
                                CurrentListBox.UpdateLayout();
                                CurrentListBox.Items.Refresh();
                            }
                        }
                    }
                }
                else
                {
                    string actualValue = "";
                    foreach (DataRow row in itemSource.Table.Rows)
                    {
                        var val = Convert.ToString(row[columnName]);
                        if (val == value)
                            continue;
                        if (actualValue == "")
                        {
                            actualValue = val + "'";
                        }
                        else
                        {
                            actualValue = actualValue + "," + "'" + val + "'";
                        }


                    }
                    actualValue = actualValue.Substring(0, actualValue.Length - 1);
                    ApplyFilters(grid, columnName, actualValue);
                    CurrentDistictValues[0].IsChecked = false;
                    grid.Items.Refresh();
                }
               
            }
            else
            {
                string actualValue = "";
                 foreach (DataRow row in itemSource.Table.Rows)
                 {
                      var val = Convert.ToString(row[columnName]);
                     if(val==value)
                         continue;
                     if(actualValue=="")
                     {
                         actualValue = val + "'";
                     }
                     else
                     {
                         actualValue = actualValue + "," + "'" + val + "'";
                     }


                 }
                actualValue = actualValue.Substring(0, actualValue.Length - 1);
                ApplyFilters(grid, columnName, actualValue);
                CurrentDistictValues[0].IsChecked = false;
                grid.Items.Refresh();
            }
            if (CurrentListBox != null)
            {
                if (CurrentListBox != null)
                {
                    var clearButton = FindControls.FindChild<Button>(CurrentListBox.Parent, "btnClear");
                    if (clearButton != null)
                    {
                        if (CurrentDistictValues != null)
                            clearButton.IsEnabled = CurrentDistictValues.Count(c => c.IsChecked) > 0;
                    }
                }
            }
          
            grid.Items.Refresh();
        }
    }
    
}
