﻿#pragma checksum "..\..\ImportWizardForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "15172C1E7E82B6C85155A4B6326514851201A4B9EBDFABF6A954773A3537369B"
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
    /// ImportWizardForm
    /// </summary>
    public partial class ImportWizardForm : Sobiens.Connectors.Studio.UI.Controls.HostControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\ImportWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView ObjectsTreeView;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\ImportWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SelectImportFileButton;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\ImportWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label SelectedFileLabel;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\ImportWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid DynamicGrid;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\ImportWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar SyncDataProgressBar;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\ImportWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ProgressTextBox;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\ImportWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ImportButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Sobiens.Connectors.Studio.UI.Controls;component/importwizardform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ImportWizardForm.xaml"
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
            this.ObjectsTreeView = ((System.Windows.Controls.TreeView)(target));
            return;
            case 2:
            this.SelectImportFileButton = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\ImportWizardForm.xaml"
            this.SelectImportFileButton.Click += new System.Windows.RoutedEventHandler(this.SelectImportFileButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SelectedFileLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.DynamicGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.SyncDataProgressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 6:
            this.ProgressTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.ImportButton = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\ImportWizardForm.xaml"
            this.ImportButton.Click += new System.Windows.RoutedEventHandler(this.ImportButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

