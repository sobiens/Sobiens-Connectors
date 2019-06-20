// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddInControlContext.cs" company="Microsoft Corporation">
//   Copyright © Microsoft Corporation.  All Rights Reserved.
// </copyright>
// <summary>
//   The control context for the addin field sets the style
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LyncCommunicationAddIn
{
    /// <summary>
    /// AddInControl context: TaskPage / FactBox
    /// </summary>
    public enum AddInControlContext
    {
        /// <summary>
        /// Field in a task page
        /// </summary>
        TaskPage,

        /// <summary>
        /// Field in a factbox
        /// </summary>
        FactBox,
    }
}
