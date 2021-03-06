﻿#pragma checksum "..\..\HierarchyBreadcrumbBar.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B067E19B750C8CF25153F8FFE6DB64C3ACC8D0CC42D71C537642DF577CBEC251"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Sobiens.WPF.Controls.BreadcrumbBarControl;
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


namespace Sobiens.Connectors.WPF.Controls {
    
    
    /// <summary>
    /// HierarchyBreadcrumbBar
    /// </summary>
    public partial class HierarchyBreadcrumbBar : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\HierarchyBreadcrumbBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Sobiens.WPF.Controls.BreadcrumbBarControl.BreadcrumbBar bar;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\HierarchyBreadcrumbBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RefreshButton;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\HierarchyBreadcrumbBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OpenInExplorerButton;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\HierarchyBreadcrumbBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OpenInNavigatorButton;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\HierarchyBreadcrumbBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EditButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Sobiens.Connectors.WPF.Controls;component/hierarchybreadcrumbbar.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\HierarchyBreadcrumbBar.xaml"
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
            this.bar = ((Sobiens.WPF.Controls.BreadcrumbBarControl.BreadcrumbBar)(target));
            
            #line 11 "..\..\HierarchyBreadcrumbBar.xaml"
            this.bar.PopulateItems += new Sobiens.WPF.Controls.BreadcrumbBarControl.BreadcrumbItemEventHandler(this.BreadcrumbBar_PopulateItems);
            
            #line default
            #line hidden
            
            #line 12 "..\..\HierarchyBreadcrumbBar.xaml"
            this.bar.SelectedBreadcrumbChanged += new System.Windows.RoutedEventHandler(this.bar_SelectedBreadcrumbChanged);
            
            #line default
            #line hidden
            
            #line 12 "..\..\HierarchyBreadcrumbBar.xaml"
            this.bar.PathChangedByText += new Sobiens.WPF.Controls.BreadcrumbBarControl.BreadcrumbBar_PathChangedByText(this.bar_PathChangedByText);
            
            #line default
            #line hidden
            return;
            case 2:
            this.RefreshButton = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\HierarchyBreadcrumbBar.xaml"
            this.RefreshButton.Click += new System.Windows.RoutedEventHandler(this.RefreshButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.OpenInExplorerButton = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\HierarchyBreadcrumbBar.xaml"
            this.OpenInExplorerButton.Click += new System.Windows.RoutedEventHandler(this.OpenInExplorerButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.OpenInNavigatorButton = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\HierarchyBreadcrumbBar.xaml"
            this.OpenInNavigatorButton.Click += new System.Windows.RoutedEventHandler(this.OpenInNavigatorButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.EditButton = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\HierarchyBreadcrumbBar.xaml"
            this.EditButton.Click += new System.Windows.RoutedEventHandler(this.EditButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

