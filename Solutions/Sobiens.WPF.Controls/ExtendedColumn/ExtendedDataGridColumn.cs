using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using Sobiens.WPF.Controls.Classes;
using Sobiens.WPF.Controls.ExtendedGridControl;
using Sobiens.WPF.Controls.Interface;
using Microsoft.Windows.Controls;

namespace Sobiens.WPF.Controls.ExtendedColumn
{
    public class ExtendedDataGridBoundColumn : DataGridBoundColumn, IExtendedColumn
    {
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return null;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return null;
          
        }

        #region "Attached Properties"

        public Boolean AllowAutoFilter
        {
            get { return (Boolean)GetValue(AllowAutoFilterProperty); }
            set
            {
                SetValue(AllowAutoFilterProperty, value);

            }
        }
        public static readonly DependencyProperty AllowAutoFilterProperty = DependencyProperty.Register(
          "AllowAutoFilter", typeof(Boolean), typeof(ExtendedDataGrid), new PropertyMetadata(true));

        #endregion


        public bool HasAutoFilter
        {
            get
            {
                var header = FindControls.GetColumnHeaderFromColumn(this);
                if (header != null)
                {
                    var popUp = FindControls.FindChild<Popup>(header, "popupDrag");
                    if (popUp != null)
                    {
                        return Convert.ToString(popUp.Tag) == "True";
                    }
                }


                return false;
            }
        }
    }
}
