// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomizedContactSearchResultListItem.cs" company="Microsoft Corporation">
//   Copyright © Microsoft Corporation.  All Rights Reserved.
// </copyright>
// <summary>
//   Add-In components for the a Lync contact presence field
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LyncCommunicationAddIn
{
    using Microsoft.Lync.Controls.Internal;

    /// <summary>Contact Search result list item that does not call on double click</summary>
    public class CustomizedContactSearchResultListItem : ContactSearchResultListItem
    {
        /// <summary>
        /// Simply does not initiate default converstaion on double click
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnPreviewMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
        }
    }
}