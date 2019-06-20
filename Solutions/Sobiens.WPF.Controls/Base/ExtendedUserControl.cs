using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Sobiens.WPF.Controls.Classes;

namespace Sobiens.WPF.Controls.Base
{
    public class ExtendedUserControl : UserControl
    {
        public Window Owner
        {
            get
            {
                return FindControls.FindParent<Window>(this);
            }
        }
    }
}
