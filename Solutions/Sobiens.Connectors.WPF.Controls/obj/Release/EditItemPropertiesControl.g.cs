﻿#pragma checksum "..\..\EditItemPropertiesControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "609E4F32E33DBFAF27D13CE204200364428FA37DFD77C3F385B222553A42E530"
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
    /// EditItemPropertiesControl
    /// </summary>
    public partial class EditItemPropertiesControl : Sobiens.Connectors.WPF.Controls.HostControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\EditItemPropertiesControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ContentTypeLabel;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\EditItemPropertiesControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ContentTypeComboBox;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\EditItemPropertiesControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid FieldMappingsStackPanel;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\EditItemPropertiesControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelfieldRequired;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\EditItemPropertiesControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CheckInAfter;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\EditItemPropertiesControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GenericFieldStackPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/Sobiens.Connectors.WPF.Controls;component/edititempropertiescontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\EditItemPropertiesControl.xaml"
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
            this.ContentTypeLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.ContentTypeComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 11 "..\..\EditItemPropertiesControl.xaml"
            this.ContentTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ContentTypeComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.FieldMappingsStackPanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.labelfieldRequired = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.CheckInAfter = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.GenericFieldStackPanel = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

