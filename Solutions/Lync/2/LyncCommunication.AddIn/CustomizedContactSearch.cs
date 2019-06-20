// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomizedContactSearch.cs" company="Microsoft Corporation">
//   Copyright © Microsoft Corporation.  All Rights Reserved.
// </copyright>
// <summary>
//   Add-In components for the a Lync contact presence field
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LyncCommunicationAddIn
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Lync.Controls;

    /// <summary>
    /// My custom ContactSearch class
    /// It exposes the Search input field and Search result list through properties
    /// </summary>
    public class CustomizedContactSearch : ContactSearch
    {
        private readonly ResourceDictionary resourceDictionary;

        /// <summary>Initializes a new instance of the <see cref="CustomizedContactSearch"/> class.</summary>
        public CustomizedContactSearch()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var uri = new Uri(assemblyName.Name + ";component/ContactListTemplates.xaml", UriKind.Relative);
            this.resourceDictionary = (ResourceDictionary)Application.LoadComponent(uri);
            this.Resources.MergedDictionaries.Add(this.resourceDictionary);
            this.Template = (ControlTemplate)this.resourceDictionary["CustomizedContactSearchTemplate"];
        }

        /// <summary>
        /// Gets the Serach input box
        /// </summary>
        public ContactSearchInputBox SearchInputBox { get; private set; }

        /// <summary>
        /// Gets the Serach input box
        /// </summary>
        public ContactSearchResultList SearchResultList { get; private set; }

        /// <summary>Template application time</summary>
        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();

            this.SearchInputBox = this.GetTemplateChild("ContactSearchInputBox") as ContactSearchInputBox;

            this.SearchResultList = this.GetTemplateChild("ContactSearchResultList") as ContactSearchResultList;
            if (this.SearchResultList != null)
            {                
                this.SearchResultList.SelectionMode = SelectionMode.Single;

                this.SearchResultList.PersonItemTemplate = (DataTemplate)this.resourceDictionary["CustomizedPersonContactTemplate"];
                this.SearchResultList.TelephoneItemTemplate = (DataTemplate)this.resourceDictionary["CustomizedTelefoneContactTemplate"];
                this.SearchResultList.GroupItemTemplate = (DataTemplate)this.resourceDictionary["CustomizedGroupContactTemplate"];
            }
        }
    }
}
