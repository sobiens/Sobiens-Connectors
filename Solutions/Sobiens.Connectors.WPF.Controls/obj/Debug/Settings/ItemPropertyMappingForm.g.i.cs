﻿#pragma checksum "..\..\..\Settings\ItemPropertyMappingForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9C74F8C00ED8D42D2A52D88C02EA15ABDD540FD47F4E0C5BFBE681F04E3955F8"
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


namespace Sobiens.Connectors.WPF.Controls.Settings {
    
    
    /// <summary>
    /// ItemPropertyMappingForm
    /// </summary>
    public partial class ItemPropertyMappingForm : Sobiens.Connectors.WPF.Controls.HostControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ApplicationItemPropertyLabel;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ApplicationItemPropertyComboBox;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ServicePropertyComboBox;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ServicePropertyTextBox;
        
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
            System.Uri resourceLocater = new System.Uri("/Sobiens.Connectors.WPF.Controls;component/settings/itempropertymappingform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
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
            this.ApplicationItemPropertyLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.ApplicationItemPropertyComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 10 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
            this.ApplicationItemPropertyComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ApplicationItemPropertyComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.ServicePropertyComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 12 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
            this.ServicePropertyComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ServicePropertyComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ServicePropertyTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 13 "..\..\..\Settings\ItemPropertyMappingForm.xaml"
            this.ServicePropertyTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ServicePropertyTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

