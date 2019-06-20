using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.WPF.Controls
{
    public class SearchViewItemMenu : ContextMenu
    {
        MenuItem openMI = new MenuItem();
        MenuItem attachMI = new MenuItem();
        MenuItem attachAsAHyperlinkMI = new MenuItem();
        MenuItem attachAsAnAttachmentMI = new MenuItem();
        MenuItem editPropertiesMI = new MenuItem();
        Separator separator1 = new Separator();
        MenuItem versionHistoryMI = new MenuItem();

        public SearchViewItemMenu()
        {
            openMI.Header = Languages.Translate("Open");
            attachMI.Header = Languages.Translate("Attach");
            attachAsAHyperlinkMI.Header = Languages.Translate("attach as a hyperlink");
            attachAsAnAttachmentMI.Header = Languages.Translate("attach as an attachment");
            editPropertiesMI.Header = Languages.Translate("Edit Properties");
            versionHistoryMI.Header = Languages.Translate("Version History");

            openMI.Icon = new System.Windows.Controls.Image
            {
               Source = new BitmapImage(new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/OPEN.GIF", UriKind.Relative))
            };
            attachMI.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/ATTACH.GIF", UriKind.Relative))
            };
            editPropertiesMI.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/EDIT.GIF", UriKind.Relative))
            };
            versionHistoryMI.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/VERSIONS.GIF", UriKind.Relative))
            };

            this.Items.Add(openMI);
            this.Items.Add(attachMI);
            attachMI.Items.Add(attachAsAHyperlinkMI);
            attachMI.Items.Add(attachAsAnAttachmentMI);
            this.Items.Add(editPropertiesMI);
            this.Items.Add(separator1);
            this.Items.Add(versionHistoryMI);

            this.ContextMenuOpening += new ContextMenuEventHandler(ListViewItemMenu_ContextMenuOpening);
            this.editPropertiesMI.Click += new System.Windows.RoutedEventHandler(editPropertiesMI_Click);
        }

        void editPropertiesMI_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           EditItemPropertiesControl editControl = new EditItemPropertiesControl();
           //if (UIHelper.ShowDialog(this.Parent editControl, "Edit Properties") == true)
           //{

           //}
           

        }

        void ListViewItemMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            /*
            object copiedListItem = null;
            DataGrid LibraryContentDataGridView = (DataGrid)e.Source;
            if (LibraryContentDataGridView.SelectedItems.Count != 1 || LibraryContentDataGridView.SelectedItems[0].Tag == null)
            {
                foreach (MenuItem toolStripItem in this.Items)
                {
                    toolStripItem.IsEnabled = false;
                }

                pasteMI.IsEnabled = (copiedListItem == null ? false : true);
                return;
            }
            IItem listItem = LibraryContentDataGridView.SelectedItems[0].Tag as IItem;
            foreach (MenuItem toolStripItem in this.Items)
            {
                toolStripItem.IsEnabled = true;
            }
            pasteMI.IsEnabled = (copiedListItem == null ? false : true);
            if (listItem == null)
            {
                attachMI.Visibility = System.Windows.Visibility.Hidden ;
                versionHistoryMI.Visibility = System.Windows.Visibility.Hidden;
                checkoutMI.Visibility = System.Windows.Visibility.Hidden;
                checkinMI.Visibility = System.Windows.Visibility.Hidden;
                undocheckoutMI.Visibility = System.Windows.Visibility.Hidden;
                approveRejectMI.Visibility = System.Windows.Visibility.Hidden;
                editPropertiesMI.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                if (GetCurrentInspactor() == null)
                {
                    attachMI.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    attachMI.Visibility = System.Windows.Visibility.Visible;
                    Outlook.MailItem emailItem = GetCurrentInspactor().CurrentItem as Outlook.MailItem;
                    attachMI.IsEnabled = false;
                    if (emailItem != null)
                    {
                        attachMI.IsEnabled = true;
                    }
                }
                Folder folder = SelectedFolder as Folder;
                if (folder != null)
                {
                    versionHistoryMI.Visibility = folder.EnableVersioning;
                }
                if (folder == null || folder.IsDocumentLibrary == false)
                {
                    checkoutMI.Visibility = System.Windows.Visibility.Hidden;
                    checkinMI.Visibility = System.Windows.Visibility.Hidden;
                    undocheckoutMI.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    if ((listItem as SPItem).CheckoutUser == String.Empty)
                    {
                        checkoutMI.Visibility = System.Windows.Visibility.Visible;
                        checkinMI.Visibility = System.Windows.Visibility.Hidden;
                        undocheckoutMI.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        checkoutMI.Visibility = System.Windows.Visibility.Hidden;
                        checkinMI.Visibility = System.Windows.Visibility.Visible;
                        undocheckoutMI.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
             */ 
        }
       
        
    }
}
