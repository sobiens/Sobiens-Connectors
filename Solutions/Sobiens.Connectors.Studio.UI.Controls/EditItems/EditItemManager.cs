using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Studio.UI.Controls.EditItems
{
    public class EditItemManager
    {
        public static EditItemControl GetEditItemControl(string webURL, Field field, IItem listItem, ContentType contentType, ISiteSetting siteSetting, string rootFolder)
        {
            EditItemControl editControl;
            if (field.ReadOnly) editControl = new EditItemLabelControl();
            else
            switch (field.Type)
            {
                case FieldTypes.TaxonomyFieldType:
                    editControl = new EditItemTaxonomyControl();
                    editControl.Width = 200;
                    break;
                case FieldTypes.Note:
                    if (field.RichText == false)
                        editControl = new EditItemMultiLineTextBoxControl();
                    else
                        editControl = new EditItemRichTextBoxControl();
                    break;
                case FieldTypes.Boolean:
                    editControl = new EditItemBooleanControl();
                    break;
                case FieldTypes.DateTime:
                    editControl = new EditItemDateTimeControl();
                    break;
                case FieldTypes.Number:
                    editControl = new EditItemNumberTextBoxControl();
                    break;
                case FieldTypes.Lookup:
                    if (field.Mult == true)
                        editControl = new EditItemCheckedListBoxControl();
                    else
                        editControl = new EditItemComboBoxControl();
                    break;
                case FieldTypes.Choice:
                    if (field.Mult == true)
                        editControl = new EditItemCheckedListBoxControl();
                    else
                        editControl = new EditItemComboBoxControl();
                    break;
                case FieldTypes.File:
                    editControl = new EditItemFileControl();
                    break;
                case FieldTypes.Text:
                default:
                    editControl = new EditItemTextBoxControl();
                    break;
            }

            editControl.InitializeControl(webURL, field, listItem, contentType, siteSetting, rootFolder);
            return editControl;
        }
    }
}
