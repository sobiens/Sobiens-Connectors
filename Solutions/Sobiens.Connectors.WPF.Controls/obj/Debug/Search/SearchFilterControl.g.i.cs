﻿#pragma checksum "..\..\..\Search\SearchFilterControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "695D3ACF8BD97697847905F64E41E8501E17BD26A4F9A7B447B4851B553CA903"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Sobiens.Connectors.WPF.Controls.Search {
    
    
    /// <summary>
    /// SearchFilterControl
    /// </summary>
    public partial class SearchFilterControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Filterlabel;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PropertyComboBox;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label PropertyLabel;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox FilterTypeComboBox;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FilterTypeLabel;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel FilterValueControlPanel;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FilterValueLabel;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AndOrComboBox;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label AndOrLabel;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddFilterButton;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Search\SearchFilterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveFilterButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Sobiens.Connectors.WPF.Controls;component/search/searchfiltercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Search\SearchFilterControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Filterlabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.PropertyComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 10 "..\..\..\Search\SearchFilterControl.xaml"
            this.PropertyComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.PropertyComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PropertyLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.FilterTypeComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 12 "..\..\..\Search\SearchFilterControl.xaml"
            this.FilterTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.FilterTypeComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.FilterTypeLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.FilterValueControlPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.FilterValueLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.AndOrComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 16 "..\..\..\Search\SearchFilterControl.xaml"
            this.AndOrComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.AndOrComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.AndOrLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.AddFilterButton = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\Search\SearchFilterControl.xaml"
            this.AddFilterButton.Click += new System.Windows.RoutedEventHandler(this.AddFilterButton_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.RemoveFilterButton = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\Search\SearchFilterControl.xaml"
            this.RemoveFilterButton.Click += new System.Windows.RoutedEventHandler(this.RemoveFilterButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

