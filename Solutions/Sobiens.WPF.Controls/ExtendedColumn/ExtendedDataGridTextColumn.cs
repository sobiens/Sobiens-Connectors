using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Sobiens.WPF.Controls.Classes;
using Sobiens.WPF.Controls.ExtendedGridControl;
using Sobiens.WPF.Controls.Interface;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;

namespace Sobiens.WPF.Controls.ExtendedColumn
{
    public class ExtendedDataGridTextColumn : DataGridTextColumn, IExtendedColumn
    {

        #region Members

       

        #endregion

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

        #region Properties

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

        #endregion

       
    }
}
