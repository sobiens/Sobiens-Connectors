//++
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// Module Name:
//
//  SearchMarginProvider.cs
//    
// Abstract:
//
//  This implements the IWpfTextViewMarginProvider to create a SearchMargin for each text view that is created.
//  
//--
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace VisualStudioSearchExtension
{
    /// <summary>
    /// Export a <see cref="IWpfTextViewMarginProvider"/>, which returns an instance of the margin for the editor
    /// to use.
    /// </summary>
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(SearchMargin.MarginName)]
    [Order(After = PredefinedMarginNames.VerticalScrollBar)] //Ensure that the margin occurs below the horizontal scrollbar
    [MarginContainer(PredefinedMarginNames.Right)] //Set the container to the right of the editor window
    [ContentType("text")] //Show this margin for all text-based types
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal sealed class SearchMarginProvider : IWpfTextViewMarginProvider
    {
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost textViewHost, IWpfTextViewMargin containerMargin)
        {
            Microsoft.Lync.Utilities.Logging.Logger.Level = Microsoft.Lync.Utilities.Logging.LogLevel.Verbose;
            Microsoft.Lync.Utilities.Logging.Logger.Listeners[0].Level = Microsoft.Lync.Utilities.Logging.LogLevel.Verbose;

            return textViewHost.TextView.Properties.GetOrCreateSingletonProperty<SearchMargin>(SearchMargin.MarginName,
                delegate { return new SearchMargin(); });

        }
    }
}