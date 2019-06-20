//++
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// Module Name:
//
//  ButtonAdornmentFactory.cs
//    
// Abstract:
//
//  This implements the IWpfTextViewCreationListener to create a ButtonAdornment for each text view that is created.
//  
//--
using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace VisualStudioSearchExtension
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class ButtonAdornmentFactory : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name(ButtonAdornment.adornmentLayerName)]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        public void TextViewCreated(IWpfTextView textView)
        {
            new ButtonAdornment(textView);
        }
    }
}
