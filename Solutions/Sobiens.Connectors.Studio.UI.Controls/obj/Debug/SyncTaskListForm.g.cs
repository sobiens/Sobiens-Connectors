﻿#pragma checksum "..\..\SyncTaskListForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "73D5DD6C276333203886B1A49DAC0A60D36B9A3309CBE5542F270F1CB6C4A77A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Sobiens.Connectors.Studio.UI.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace Sobiens.Connectors.Studio.UI.Controls {
    
    
    /// <summary>
    /// SyncTaskListForm
    /// </summary>
    public partial class SyncTaskListForm : Sobiens.Connectors.Studio.UI.Controls.HostControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 11 "..\..\SyncTaskListForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RefreshButton;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\SyncTaskListForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteTaskButton;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\SyncTaskListForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OpenFolderButton;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\SyncTaskListForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl tabControl;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\SyncTaskListForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid TasksDataGrid;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\SyncTaskListForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid PastTasksDataGrid;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\SyncTaskListForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker HistoryDatePicker;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\SyncTaskListForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
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
            System.Uri resourceLocater = new System.Uri("/Sobiens.Connectors.Studio.UI.Controls;component/synctasklistform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SyncTaskListForm.xaml"
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
            this.RefreshButton = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\SyncTaskListForm.xaml"
            this.RefreshButton.Click += new System.Windows.RoutedEventHandler(this.RefreshButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DeleteTaskButton = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\SyncTaskListForm.xaml"
            this.DeleteTaskButton.Click += new System.Windows.RoutedEventHandler(this.DeleteTaskButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.OpenFolderButton = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\SyncTaskListForm.xaml"
            this.OpenFolderButton.Click += new System.Windows.RoutedEventHandler(this.OpenFolderButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tabControl = ((System.Windows.Controls.TabControl)(target));
            return;
            case 5:
            this.TasksDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 23 "..\..\SyncTaskListForm.xaml"
            this.TasksDataGrid.Loaded += new System.Windows.RoutedEventHandler(this.DataGrid_Loaded);
            
            #line default
            #line hidden
            
            #line 23 "..\..\SyncTaskListForm.xaml"
            this.TasksDataGrid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.TasksDataGrid_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.PastTasksDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 52 "..\..\SyncTaskListForm.xaml"
            this.PastTasksDataGrid.Loaded += new System.Windows.RoutedEventHandler(this.PastTasksDataGrid_Loaded);
            
            #line default
            #line hidden
            
            #line 52 "..\..\SyncTaskListForm.xaml"
            this.PastTasksDataGrid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.PastTasksDataGrid_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.HistoryDatePicker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 64 "..\..\SyncTaskListForm.xaml"
            this.HistoryDatePicker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.HistoryDatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 33 "..\..\SyncTaskListForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EditButton_Click);
            
            #line default
            #line hidden
            break;
            case 7:
            
            #line 34 "..\..\SyncTaskListForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ProgressButton_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
