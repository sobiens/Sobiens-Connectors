// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomizedContactSearchResultList.cs" company="Microsoft Corporation">
//   Copyright © Microsoft Corporation.  All Rights Reserved.
// </copyright>
// <summary>
//   Add-In components for the a Lync contact presence field
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LyncCommunicationAddIn
{
    using Microsoft.Lync.Controls;
    using Microsoft.Lync.Controls.Internal;

    /// <summary>Custom search result list, providing custom list items that do not call on doubleclick</summary>
    public class CustomizedContactSearchResultList : ContactSearchResultList
    {        
        /// <summary>Provides a list item of the desired type</summary>
        /// <returns>Returns the an item</returns>
        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            var orig = base.GetContainerForItemOverride();
            var origItem = orig as ContactSearchResultListItem;
            if (origItem == null)
            {
                return orig;
            }

            var customizedItem = new CustomizedContactSearchResultListItem();
            if (origItem.Style != null)
            {
                customizedItem.Style = origItem.Style;
            }
            if (origItem.PersonContentTemplate != null)
            {
                customizedItem.PersonContentTemplate = origItem.PersonContentTemplate;
            }
            if (origItem.BotContentTemplate != null)
            {
                customizedItem.BotContentTemplate = origItem.BotContentTemplate;
            }
            if (origItem.TelephoneContentTemplate != null)
            {
                customizedItem.TelephoneContentTemplate = origItem.TelephoneContentTemplate;
            }
            if (origItem.GroupContentTemplate != null)
            {
                customizedItem.GroupContentTemplate = origItem.GroupContentTemplate;
            }

            return customizedItem;
        }
    }
}