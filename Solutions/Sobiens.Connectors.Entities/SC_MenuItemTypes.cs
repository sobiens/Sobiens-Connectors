using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
   
    public enum SC_MenuItemTypes
    {
        [SC_MenuItemInfoAttribute(0, "Separator", "")]
        Separator = 0,
        [SC_MenuItemInfoAttribute(1, "Open Item", "OPEN.gif")]
        OpenItem =1,
        [SC_MenuItemInfoAttribute(2, "Edit", "EDIT.gif")]
        EditItem =2,
        [SC_MenuItemInfoAttribute(3, "Attach", "ATTACH.gif")]
        Attach = 3,
        [SC_MenuItemInfoAttribute(4, "Item Property Mappings", "EDIT.gif")]
        EditItemPropertyMappings = 4,
        [SC_MenuItemInfoAttribute(5, "Version History", "VERSIONS.gif")]
        ShowItemVersionHistory = 5,
        [SC_MenuItemInfoAttribute(6, "Open", "Open.gif")]
        OpenVersionHistory = 6,
        [SC_MenuItemInfoAttribute(7, "Rollback", "Rollback.gif")]
        RollbackVersionHistory = 7,
        [SC_MenuItemInfoAttribute(8, "Approve/Reject", "APPRJ.gif")]
        ApproveRejectItem = 8,
        [SC_MenuItemInfoAttribute(9, "Check In", "CheckIn.gif")]
        CheckInItem = 9,
        [SC_MenuItemInfoAttribute(10, "Undo Check Out", "UNCHKOUT.gif")]
        UndoCheckOutItem = 10,
        [SC_MenuItemInfoAttribute(11, "Check Out", "CheckOut.gif")]
        CheckOutItem = 11,
        [SC_MenuItemInfoAttribute(12, "Attach as a hyperlink", "")]
        AttachAsAHyperlink = 12,
        [SC_MenuItemInfoAttribute(13, "Attach as an attachment", "")]
        AttachAsAnAttachment = 13,
        [SC_MenuItemInfoAttribute(14, "Copy", "Copy.gif")]
        CopyItem = 14,
        [SC_MenuItemInfoAttribute(26, "Cut", "Cut.png")]
        Cut = 26,
        [SC_MenuItemInfoAttribute(15, "Paste", "Paste.gif")]
        PasteItem = 15,
        [SC_MenuItemInfoAttribute(16, "Delete", "Delete.gif")]
        DeleteItem = 16,
        [SC_MenuItemInfoAttribute(17, "Open Folder", "OPEN.gif")]
        OpenFolder = 17,
        [SC_MenuItemInfoAttribute(18, "Workflow", "WORKFLOWS.gif")]
        Workflow = 18,
        [SC_MenuItemInfoAttribute(19, "Edit Task", "Edit.gif")]
        EditTask = 19,
        [SC_MenuItemInfoAttribute(20, "Open Task Document", "OPEN.gif")]
        OpenTaskDocument = 20,
        [SC_MenuItemInfoAttribute(21, "New", "advadd.png")]
        New = 21,
        [SC_MenuItemInfoAttribute(22, "Add folder", "Folder.gif")]
        AddFolder = 22,
        [SC_MenuItemInfoAttribute(23, "Display", "view.png")]
        Display = 23,
        [SC_MenuItemInfoAttribute(24, "In explorer", "Open.gif")]
        Inexplorer = 24,
        [SC_MenuItemInfoAttribute(25, "In navigator", "Internet.gif")]
        Innavigator = 25
    }
}
