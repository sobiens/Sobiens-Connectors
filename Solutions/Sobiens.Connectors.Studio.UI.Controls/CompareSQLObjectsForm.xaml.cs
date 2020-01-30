using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using System.Windows.Data;
using Sobiens.Connectors.Entities.SQLServer;
using System.IO;
using System.Windows.Documents;
using System.Text;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class CompareSQLObjectsForm : HostControl
    {
        private const int LoadingNodeTagValue = -1;
        public Folder SourceObject;
        public Folder DestinationObject;

        /*
        public Folder SelectedObject
        {
            get
            {
                if (FoldersTreeView.SelectedItem != null && ((TreeViewItem)FoldersTreeView.SelectedItem).Tag as Folder != null)
                    return (Folder)((TreeViewItem)FoldersTreeView.SelectedItem).Tag;

                return null;
            }
        }
        */

        public CompareSQLObjectsForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad() {
            SourceObjectLabel.Content = ((SQLFolder)SourceObject).Name;
            DestinationObjectLabel.Content = ((SQLFolder)DestinationObject).Name;

            string sourceSQLSyntax = ((SQLFolder)SourceObject).SQLSyntax.Replace(Environment.NewLine, "\\line");
            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(sourceSQLSyntax));
            TextRange range = new TextRange(SourceObjectSchemaTextBlock.Document.ContentStart, SourceObjectSchemaTextBlock.Document.ContentEnd);
            range.Load(stream, DataFormats.Rtf);

            string destinationSQLSyntax = ((SQLFolder)DestinationObject).SQLSyntax.Replace(Environment.NewLine, "\\line");
            MemoryStream stream1 = new MemoryStream(ASCIIEncoding.Default.GetBytes(destinationSQLSyntax));
            TextRange range1 = new TextRange(DestinationObjectSchemaTextBlock.Document.ContentStart, DestinationObjectSchemaTextBlock.Document.ContentEnd);
            range1.Load(stream1, DataFormats.Rtf);
            MarkDifferentLines();
        }

        private void MarkDifferentLines()
        {
            TextPointer sourceTP = SourceObjectSchemaTextBlock.Document.ContentStart;
            TextPointer destinationStartTP = DestinationObjectSchemaTextBlock.Document.ContentStart;
            TextRange textrange = new TextRange(sourceTP.GetLineStartPosition(1), sourceTP.GetLineStartPosition(2));

            while (true)
            {
                if (sourceTP == null)
                    break;

                string afterText = sourceTP.GetTextInRun(LogicalDirection.Forward);
                string beforeText = sourceTP.GetTextInRun(LogicalDirection.Backward);
                string fullText = beforeText + afterText;

                sourceTP = sourceTP.GetLineStartPosition(1);
            }
        }


        public void Initialize(Folder sourceObject, Folder objectToCompareWith)
        {
            this.SourceObject = sourceObject;
            this.DestinationObject = objectToCompareWith;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
        }


        private void ApplyChangeButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            SiteSetting sourceSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[SourceObject.SiteSettingID];
            SiteSetting destinationSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[DestinationObject.SiteSettingID];

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    CompareObjectsResult compareObjectsResult = (CompareObjectsResult)row.Item;
                    if(compareObjectsResult.DifferenceType == "Missing")
                    {
                        ApplicationContext.Current.ApplyMissingCompareObjectsResult(compareObjectsResult, sourceSiteSetting, destinationSiteSetting);
                    }
                }
                */
            this.OnLoad();
        }
    }
}
