// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomizedPresenceIndicator.cs" company="Microsoft Corporation">
//   Copyright © Microsoft Corporation.  All Rights Reserved.
// </copyright>
// <summary>
//   Add-In components for the a Lync contact presence field
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LyncCommunicationAddIn
{
    using System.Windows.Controls;
    using Microsoft.Lync.Controls;
    using Microsoft.Lync.Controls.Internal;

    /// <summary>
    /// PresenceIndicator class that provides access to the contact popup card object
    /// </summary>
    public class CustomizedPresenceIndicator : PresenceIndicator
    {
        /// <summary>
        /// When an template is applied
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.ContactPopupCard = this.ExtractTemplatePart<PopupContactCard>("PART_PopupContactCard");

            var button = this.ExtractTemplatePart<Button>("PART_IndicatorButton");
            if (button != null)
            {
                button.IsTabStop = false;
            }
        }

        /// <summary>
        /// Gets the contact card
        /// </summary>
        public PopupContactCard ContactPopupCard { get; private set; }
    }
}

