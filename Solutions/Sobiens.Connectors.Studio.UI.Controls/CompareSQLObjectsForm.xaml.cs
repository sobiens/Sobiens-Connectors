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
using Sobiens.Connectors.Common.SQLServer;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class CompareSQLObjectsForm : HostControl
    {
        private bool IsInProgress = false;
        public Folder SourceObject;
        public Folder DestinationObject;

        public CompareSQLObjectsForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad() {
            SQLFolder sourceSQLFolder = (SQLFolder)SourceObject;
            SQLFolder destinationSQLFolder = (SQLFolder)DestinationObject;

            SourceObjectLabel.Content = (sourceSQLFolder.ListName!=null?sourceSQLFolder.ListName:"") + "-" + (sourceSQLFolder.Schema != null ?sourceSQLFolder.Schema:"") + "-" + (sourceSQLFolder.Name!=null?sourceSQLFolder.Name:"");
            DestinationObjectLabel.Content = (destinationSQLFolder.ListName != null ? destinationSQLFolder.ListName : "") + "-" + (destinationSQLFolder.Schema != null ? destinationSQLFolder.Schema : "") + "-" + (destinationSQLFolder.Name != null ? destinationSQLFolder.Name : "");

            IsInProgress = true;

            bool isExecutable = false;
            if(SourceObject as SQLFunction != null || SourceObject as SQLView != null || SourceObject as SQLStoredProcedure != null || SourceObject as SQLTrigger != null)
            {
                isExecutable = true;
            }

            string sourceSQLSyntax = ((SQLFolder)SourceObject).SQLSyntax;
            string destinationSQLSyntax = ((SQLFolder)DestinationObject).SQLSyntax;

            if(isExecutable == true)
            {
                string _sourceSQLSyntax = string.Empty;
                int firstCreateWordIndex = sourceSQLSyntax.ToLower().IndexOf("create");
                if (firstCreateWordIndex > -1)
                {
                    _sourceSQLSyntax = sourceSQLSyntax.Substring(0, firstCreateWordIndex) + "ALTER" + sourceSQLSyntax.Substring(firstCreateWordIndex+6);
                    sourceSQLSyntax = _sourceSQLSyntax;
                }

                string _destinationSQLSyntax = string.Empty;
                firstCreateWordIndex = destinationSQLSyntax.ToLower().IndexOf("create");
                if (firstCreateWordIndex > -1)
                {
                    _destinationSQLSyntax = destinationSQLSyntax.Substring(0, firstCreateWordIndex) + "ALTER" + destinationSQLSyntax.Substring(firstCreateWordIndex + 6);
                    destinationSQLSyntax = _destinationSQLSyntax;
                }
            }
            else
            {
                ExecuteLeftSideButton.Visibility = Visibility.Hidden;
                ExecuteRightSideButton.Visibility = Visibility.Hidden;
            }

            TextRange textRange = new TextRange(SourceObjectSchemaTextBlock.Document.ContentStart, SourceObjectSchemaTextBlock.Document.ContentEnd);
            textRange.Text = sourceSQLSyntax;

            TextRange textRange1 = new TextRange(DestinationObjectSchemaTextBlock.Document.ContentStart, DestinationObjectSchemaTextBlock.Document.ContentEnd);
            textRange1.Text = destinationSQLSyntax;

            IsInProgress = false;
            MarkDifferentLines();
        }

        private void MarkDifferentLines()
        {
            try
            {
                IsInProgress = true;

                    List<Block> sourceBlocks = new List<Block>();
                sourceBlocks.AddRange(SourceObjectSchemaTextBlock.Document.Blocks);

                List<Block> destinationBlocks = new List<Block>();
                destinationBlocks.AddRange(DestinationObjectSchemaTextBlock.Document.Blocks);

                for (int i = 0; i < sourceBlocks.Count; i++)
                {
                    if (destinationBlocks.Count >= i)
                    {
                        TextRange sourceLineTextRange = new TextRange(sourceBlocks[i].ContentStart, sourceBlocks[i].ContentEnd);
                        TextRange destinationLineTextRange = new TextRange(destinationBlocks[i].ContentStart, destinationBlocks[i].ContentEnd);

                        string sourceLineText = sourceLineTextRange.Text;
                        string destinationLineText = destinationLineTextRange.Text;

                        if (sourceLineText.Equals(destinationLineText, StringComparison.InvariantCultureIgnoreCase) == false)
                        {
                            sourceLineTextRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGray);
                            destinationLineTextRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGray);
                        }
                        else
                        {
                            sourceLineTextRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
                            destinationLineTextRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
                        }
                    }
                }
            }
            catch (Exception ex) {
                int c = 1;
            }
            IsInProgress = false;
        }


        public void Initialize(Folder sourceObject, Folder objectToCompareWith)
        {
            this.SourceObject = sourceObject;
            this.DestinationObject = objectToCompareWith;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            MarkDifferentLines();
        }


        private void ApplyChangeButton_Click(object sender, RoutedEventArgs e)
        {
            MarkDifferentLines();
            this.OnLoad();
        }

        private void CopySelectedLines(RichTextBox sourceTextBox, RichTextBox destinationTextBox) {
            List<Paragraph> selectedParagraphs = new List<Paragraph>();
            int startParagraphIndex = 0;
            int endParagraphIndex = 0;

            Paragraph paragraph = sourceTextBox.Selection.Start.Paragraph;

            while (paragraph != null && sourceTextBox.Selection.Contains(paragraph.ContentStart) == true)
            {
                selectedParagraphs.Add(paragraph);
                paragraph = (Paragraph)paragraph.NextBlock;
            }

            List<Block> sourceBlocks = new List<Block>();
            sourceBlocks.AddRange(sourceTextBox.Document.Blocks);

            List<Block> destinationBlocks = new List<Block>();
            destinationBlocks.AddRange(destinationTextBox.Document.Blocks);

            for (int i = 0; i < sourceBlocks.Count; i++)
            {
                if (sourceTextBox.Selection.Start.Paragraph.ContentStart.CompareTo(sourceBlocks[i].ContentStart) == 0)
                {
                    startParagraphIndex = i;
                }

                if (selectedParagraphs[selectedParagraphs.Count - 1].ContentStart.CompareTo(sourceBlocks[i].ContentStart) == 0)
                {
                    endParagraphIndex = i;
                }
            }

            for (int i = startParagraphIndex; i <= endParagraphIndex; i++)
            {
                TextRange sourceTextRange = new TextRange(sourceBlocks[i].ContentStart, sourceBlocks[i].ContentEnd);
                TextRange destinationTextRange = new TextRange(destinationBlocks[i].ContentStart, destinationBlocks[i].ContentEnd);
                destinationTextRange.Text = sourceTextRange.Text;
            }

        }

        private void CopyToRightButton_Click(object sender, RoutedEventArgs e)
        {
            CopySelectedLines(SourceObjectSchemaTextBlock, DestinationObjectSchemaTextBlock);
            MarkDifferentLines();
        }

        private void CopyToLeftButton_Click(object sender, RoutedEventArgs e)
        {
            CopySelectedLines(DestinationObjectSchemaTextBlock, SourceObjectSchemaTextBlock);
            MarkDifferentLines();
        }

        private void SourceObjectSchemaTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsInProgress == false)
                MarkDifferentLines();
        }

        private void DestinationObjectSchemaTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInProgress == false)
                MarkDifferentLines();
        }

        private void ExecuteLeftSideButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange sourceLineTextRange = new TextRange(SourceObjectSchemaTextBlock.Document.ContentStart, SourceObjectSchemaTextBlock.Document.ContentEnd);
            string sqlQuery = sourceLineTextRange.Text;
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(SourceObject.SiteSettingID);

            ExecuteQuery(siteSetting, SourceObject.GetListName(), sqlQuery);
        }

        private void ExecuteRightSideButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange destinationLineTextRange = new TextRange(DestinationObjectSchemaTextBlock.Document.ContentStart, DestinationObjectSchemaTextBlock.Document.ContentEnd);
            string sqlQuery = destinationLineTextRange.Text;
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(DestinationObject.SiteSettingID);

            ExecuteQuery(siteSetting, DestinationObject.GetListName(), sqlQuery);
        }

        private void ExecuteQuery(ISiteSetting siteSetting, string dbName, string sqlQuery)
        {
            try
            {
                this.ShowLoadingStatus("Executing query");
                (new SQLServerService()).ExecuteQuery(siteSetting, dbName, sqlQuery);
                this.HideLoadingStatus();
                this.SetStatusText("Execution completed successfully");
            }
            catch (Exception ex)
            {
                this.HideLoadingStatus();
                this.SetErrorMessage("Execution failed:" + ex.Message);
            }
        }
    }
}
