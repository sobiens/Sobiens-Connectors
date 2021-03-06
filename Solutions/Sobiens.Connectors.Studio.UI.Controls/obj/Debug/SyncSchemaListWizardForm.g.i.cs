#pragma checksum "..\..\SyncSchemaListWizardForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E2C1EEB3C553480982772DE4A8854D68577A6F9E57556B61CFD11AFC47BFB9E8"
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


namespace Sobiens.Connectors.Studio.UI.Controls
{


    /// <summary>
    /// SyncSchemaListWizardForm
    /// </summary>
    public partial class SyncSchemaListWizardForm : Sobiens.Connectors.Studio.UI.Controls.HostControl, System.Windows.Markup.IComponentConnector
    {


#line 10 "..\..\SyncSchemaListWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl WizardTabControl;

#line default
#line hidden


#line 22 "..\..\SyncSchemaListWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SourceNextButton;

#line default
#line hidden


#line 36 "..\..\SyncSchemaListWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DestinationBackButton;

#line default
#line hidden


#line 37 "..\..\SyncSchemaListWizardForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DestinationNextButton;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Sobiens.Connectors.Studio.UI.Controls;component/syncschemalistwizardform.xaml", System.UriKind.Relative);

#line 1 "..\..\SyncSchemaListWizardForm.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.WizardTabControl = ((System.Windows.Controls.TabControl)(target));

#line 10 "..\..\SyncSchemaListWizardForm.xaml"
                    this.WizardTabControl.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TabControl_SelectionChanged);

#line default
#line hidden
                    return;
                case 2:
                    this.SourceNextButton = ((System.Windows.Controls.Button)(target));

#line 22 "..\..\SyncSchemaListWizardForm.xaml"
                    this.SourceNextButton.Click += new System.Windows.RoutedEventHandler(this.SourceNextButton_Click);

#line default
#line hidden
                    return;
                case 3:
                    this.DestinationBackButton = ((System.Windows.Controls.Button)(target));

#line 36 "..\..\SyncSchemaListWizardForm.xaml"
                    this.DestinationBackButton.Click += new System.Windows.RoutedEventHandler(this.DestinationBackButton_Click);

#line default
#line hidden
                    return;
                case 4:
                    this.DestinationNextButton = ((System.Windows.Controls.Button)(target));

#line 37 "..\..\SyncSchemaListWizardForm.xaml"
                    this.DestinationNextButton.Click += new System.Windows.RoutedEventHandler(this.DestinationNextButton_Click);

#line default
#line hidden
                    return;
            }
            this._contentLoaded = true;
        }

        internal Sobiens.Connectors.Studio.UI.Controls.SelectEntityForm SourceSelectEntityForm;
        internal Sobiens.Connectors.Studio.UI.Controls.SelectEntityForm DestinationSelectEntityForm;
    }
}

