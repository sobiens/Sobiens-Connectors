﻿#pragma checksum "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B88AE008240F45A5F078A83E54F9FE90F2AB291A5C05CC198D3DC38FF7408DF5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Sobiens.Connectors.WPF.Controls;
using Sobiens.Connectors.WPF.Controls.Selectors;
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


namespace Sobiens.Connectors.WPF.Controls.Selectors {
    
    
    /// <summary>
    /// DocumentTemplateLocationSelectionForm
    /// </summary>
    public partial class DocumentTemplateLocationSelectionForm : Sobiens.Connectors.WPF.Controls.HostControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LocationLabel;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox LocationsComboBox;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SelectFromSubFoldersButton;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FolderLabel;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label SelectedFolderLabel;
        
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
            System.Uri resourceLocater = new System.Uri("/Sobiens.Connectors.WPF.Controls;component/selectors/documenttemplatelocationsele" +
                    "ctionform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml"
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
            this.LocationLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.LocationsComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 11 "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml"
            this.LocationsComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.LocationsComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SelectFromSubFoldersButton = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\Selectors\DocumentTemplateLocationSelectionForm.xaml"
            this.SelectFromSubFoldersButton.Click += new System.Windows.RoutedEventHandler(this.SelectFromSubFoldersButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.FolderLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.SelectedFolderLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

