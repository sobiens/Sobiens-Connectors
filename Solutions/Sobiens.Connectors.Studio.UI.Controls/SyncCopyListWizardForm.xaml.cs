using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class SyncCopyListWizardForm : HostControl
    {
        Dictionary<string, string> RequiredMappings
        {
            get
            {
                Dictionary<string, string> mappings = new Dictionary<string, string>();
                mappings.Add("SourceItemID", "ID"); // DestinationField, SourceField
                mappings.Add("Title", "Title"); // DestinationField, SourceField
                mappings.Add("FileRef", "FileRef"); // DestinationField, SourceField
                return mappings;
            }
        }
        private int CurrentTabIndex = 0;
        //private Folder SourceFolder = null;
        private Folder DestinationFolder = null;
        //private ISiteSetting SourceSiteSetting = null;
        private ISiteSetting DestinationSiteSetting = null;
        private FieldCollection DestinationFolderFields = null;
        //private FieldCollection SourceFolderFields = null;
        private SyncTask SyncTask = null;
        private bool IsInEditMode = false;

        public SyncCopyListWizardForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();
        }

        private void InitializeForm()
        {
            this.OKButtonSelected += SyncDataWizardForm_OKButtonSelected;
        }

        private void SyncDataWizardForm_OKButtonSelected(object sender, EventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

        }

        public void Initialize(SyncTask syncTask)
        {
            SyncTask = syncTask;
            IsInEditMode = true;
            //SourceDataFoldersSelector.Initialize();
            //DestinationDataFoldersSelector.Initialize();
            //ApplicationContext.Current.SPCamlStudio.ServerObjectExplorer.
        }

        private void MigrateButton_Click(object sender, RoutedEventArgs e)
        {
            //SyncBackgroundWorker.RunWorkerAsync(new object[] { syncTask, shouldImportListItems, shouldExportListItems, shouldImportDocuments, shouldExportDocuments, shouldSkipUpdates, includeVersionsLimit, excludeFields });

            /*
            SyncTask syncTask = (SyncTask)SyncTasksComboBox.SelectedItem;
            bool shouldImportListItems = ImportListItemsCheckBox.IsChecked == true ? true : false;
            bool shouldExportListItems = ExportListItemsCheckBox.IsChecked == true ? true : false;
            bool shouldImportDocuments = ImportDocumentsCheckBox.IsChecked == true ? true : false;
            bool shouldExportDocuments = ExportDocumentsCheckBox.IsChecked == true ? true : false;
            bool shouldSkipUpdates = SkipUpdateCheckBox.IsChecked == true ? true : false;
            string[] excludeFields = ExcludeFieldsTextBox.Text.Split(new string[] { ";" }, StringSplitOptions.None);

            int includeVersionsLimit = 0;
            if (LimitVersionsCheckBox.IsChecked == true)
            {
                string limitVersions = LimitVersionsTextBox.Text;
                if (int.TryParse(limitVersions, out includeVersionsLimit) == true)
                {
                }
            }
            */
            //SyncBackgroundWorker.RunWorkerAsync(new object[] { syncTask, shouldImportListItems, shouldExportListItems, shouldImportDocuments, shouldExportDocuments, shouldSkipUpdates, includeVersionsLimit, excludeFields });

            /*
            SyncDataWizardForm syncDataWizardForm = new SyncDataWizardForm();
            syncDataWizardForm.Initialize(syncTasks);
            if (syncDataWizardForm.ShowDialog(this.ParentWindow, "Data Import Wizard", false, true) == true)
            {
            }
            */
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            e.Handled = true;

            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        private void SourceNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourceQueryGeneratorControl.QueryResults.Count ==0)
            {
                MessageBox.Show("Please select a list object for source");
                return;
            }

            //SourceSiteSetting = ApplicationContext.Current.GetSiteSetting(SourceFolder.SiteSettingID);
            //Logger.Error("10", "SyncCopyListWizardForm");
            //SourceFolderFields = ApplicationContext.Current.GetFields(SourceSiteSetting, SourceFolder);
            //SourceFolderFields = SourceQueryGeneratorControl.QueryResults[0].Fields;
            SyncTask.SourceQueryResultMapping.Mappings.Clear();
            Logger.Error("11", "SyncCopyListWizardForm");
            for (int i = 0; i < SourceQueryGeneratorControl.QueryResults.Count; i++)
            {
                /*
                QueryResult test1QueryResult = new QueryResult();
                test1QueryResult.Fields = sourceFieldNames.ToArray();
                test1QueryResult.ListName = SourceFolder.GetListName();
                test1QueryResult.Name = SourceFolder.GetListName();
                test1QueryResult.SiteSetting = (SiteSetting)SourceSiteSetting;
                test1QueryResult.Filters = new CamlFilters();
                */
                QueryResultMapping test1QueryResultMapping = new QueryResultMapping();
                test1QueryResultMapping.QueryResult = SourceQueryGeneratorControl.QueryResults[i];
                test1QueryResultMapping.SelectFields = SourceQueryGeneratorControl.QueryResultMappingSelectFields[i];

                SyncTask.SourceQueryResultMapping.Mappings.Add(test1QueryResultMapping);
            }

            /*
            Field ctField = new Field()
            {
                ID = Guid.Empty,
                Name = "ContentType",
                DisplayName = "ContentType",
                FromBaseType = false
            };
            SourceFolderFields.Add(ctField);
            */
            CurrentTabIndex = 1;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        private void MappingNextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<QueryResultMappingSelectField> destinationFieldMappings = new List<QueryResultMappingSelectField>();
                //sourceSelectFieldNames.Add(new QueryResultMappingSelectField("ID", "ID"));
                //sourceSelectFieldNames.Add(new QueryResultMappingSelectField("Title", "Title"));
                //sourceSelectFieldNames.Add(new QueryResultMappingSelectField("FileRef", "FileRef"));

                List<string> sourceFieldNames = new List<string>();
                //sourceFieldNames.Add("ID");
                //sourceFieldNames.Add("Title");
                //sourceFieldNames.Add("FileRef");

                List<string> destinationFieldNames = new List<string>();
                //destinationFieldNames.Add("SourceItemID");
                //destinationFieldNames.Add("Title");
                //destinationFieldNames.Add("FileRef");

                Logger.Error("12", "SyncCopyListWizardForm");

                foreach (Field destionationField in DestinationFolderFields)
                {
                    if (destionationField.ReadOnly == true && destionationField.Name.Equals("Title", StringComparison.InvariantCultureIgnoreCase) == false)
                        continue;

                    /*
                    if (destionationField.Name.Equals("SourceItemID", StringComparison.InvariantCultureIgnoreCase) == true)
                        continue;

                    if (destionationField.Name.Equals("Title", StringComparison.InvariantCultureIgnoreCase) == true)
                        continue;

                    if (destionationField.Name.Equals("FileRef", StringComparison.InvariantCultureIgnoreCase) == true)
                        continue;
                        */

                    string textboxName = destionationField.Name + "_ManuelValueTextBox";
                    string comboBoxName = destionationField.Name + "_SourceComboBox";
                    string manuelValue = string.Empty;

                    foreach (UIElement element in FieldMappingsStackPanel.Children)
                    {
                        if (element is TextBox == true)
                        {
                            if (((TextBox)element).Name == textboxName)
                            {
                                manuelValue = ((TextBox)element).Text;
                            }
                        }
                    }

                    foreach (UIElement element in FieldMappingsStackPanel.Children)
                    {
                        if (element is ComboBox == true)
                        {
                            if (((ComboBox)element).Name == comboBoxName)
                            {
                                ComboBox comboBox = (ComboBox)element;
                                if (((QueryResultMappingSelectField)comboBox.SelectedItem).OutputHeaderName == "--- Not Mapped")
                                {
                                    continue;
                                }
                                else if (((QueryResultMappingSelectField)comboBox.SelectedItem).OutputHeaderName == "--- Apply Manuel Value")
                                {
                                    //sourceFieldNames.Add(destionationField.Name);
                                    destinationFieldNames.Add(destionationField.Name);
                                    destinationFieldMappings.Add(new QueryResultMappingSelectField(string.Empty, manuelValue, destionationField.Name));
                                }
                                else
                                {
                                    QueryResultMappingSelectField sourceField = ((QueryResultMappingSelectField)comboBox.SelectedItem);
                                    sourceFieldNames.Add(sourceField.OutputHeaderName);
                                    destinationFieldNames.Add(destionationField.Name);
                                    destinationFieldMappings.Add(new QueryResultMappingSelectField(destionationField.Name, sourceField.StaticValue, destionationField.Name));
                                    //destinationFieldMappings.Add(new QueryResultMappingSelectField(sourceField.FieldName, sourceField.StaticValue, sourceField.OutputHeaderName));
                                }
                            }
                        }
                    }
                }
                Logger.Error("13", "SyncCopyListWizardForm");

                Logger.Error("14", "SyncCopyListWizardForm");

                SyncTask.DestinationListName = DestinationFolder.GetListName();
                SyncTask.DestinationRootFolderPath = DestinationFolder.GetPath();
                SyncTask.DestinationFolderPath = DestinationFolder.GetPath();
                SyncTask.Name = DestinationFolder.GetListName();
                SyncTask.DestinationPrimaryIdFieldName = DestinationFolder.PrimaryIdFieldName;
                SyncTask.DestinationPrimaryNameFieldName = DestinationFolder.PrimaryNameFieldName;
                SyncTask.DestinationPrimaryFileReferenceFieldName = DestinationFolder.PrimaryFileReferenceFieldName;
                //if (string.IsNullOrEmpty(SyncTask.SourceQueryResultMapping.Mappings[0].QueryResult.PrimaryIdFieldName) == false)
                if (AllowUpdateCheckBox.IsChecked == true)
                {
                    QueryResultMappingSelectField sourceQueryResultMappingSelectField = (QueryResultMappingSelectField)UpdateSourceFieldComboBox.SelectedItem;
                    string destinationUniqueFieldName = ((Field)UpdateDestinationFieldComboBox.SelectedItem).Name;
                    SyncTask.SourceUniqueFieldHeaderNames = new string[] { sourceQueryResultMappingSelectField.OutputHeaderName };
                    SyncTask.DestinationUniqueFieldNames = new string[] { destinationUniqueFieldName };
                    //SyncTask.SourceQueryResultMapping.Mappings[0].QueryResult.PrimaryIdFieldName = sourceQueryResultMappingSelectField.FieldName;
                    if(destinationFieldNames.Contains(destinationUniqueFieldName) == false)
                    {
                        destinationFieldNames.Add(destinationUniqueFieldName);
                        destinationFieldMappings.Add(new QueryResultMappingSelectField(destinationUniqueFieldName, destinationUniqueFieldName));
                        sourceFieldNames.Add(sourceQueryResultMappingSelectField.FieldName);
                    }
                    //SyncTask.DestinationPrimaryIdFieldName = destinationUniqueFieldName;
/*
                    if (sourceFieldNames.Contains(sourceQueryResultMappingSelectField.FieldName) == false)
                    {
                    }
                    */
                    //sourceFieldNames.Add(destionationField.Name);
/*
                    if (destinationFieldMappings.Where(t => t.FieldName.Equals(sourceQueryResultMappingSelectField.FieldName, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                    {
                        destinationFieldMappings.Add(sourceQueryResultMappingSelectField);
                    }
                    */
                }
                else
                {
                    SyncTask.SourceUniqueFieldHeaderNames = new string[] {  };
                    SyncTask.DestinationUniqueFieldNames = new string[] {  };
                }

                SyncTask.DestinationIDFieldHeaderName = DestinationFolder.PrimaryIdFieldName;

                /*
                if (DestinationFolder.IsDocumentLibrary == true)
                {
                    sourceFieldNames.RemoveAt(2);
                    destinationFieldNames.RemoveAt(2);
                    sourceSelectFieldNames.RemoveAt(2);
                }
                */
                Logger.Error("15", "SyncCopyListWizardForm");

                //SyncTask.SourceFieldHeaderMappings = sourceSelectFieldNames.Select(t => t.OutputHeaderName).ToArray();
                SyncTask.SourceFieldHeaderMappings = sourceFieldNames.ToArray();
                //destinationFieldNames[0] = "SourceItemID";
                SyncTask.DestinationFieldMappings = destinationFieldMappings; // destinationFieldNames.ToArray();
                SyncTask.IsDestinationDocumentLibrary = DestinationFolder.IsDocumentLibrary;


                SyncTask.DestinationTermStoreName = "Taxonomy_BrjLUNqY3/3gqp8FAbbKiQ==";
                SyncTask.DestinationSiteSetting = (SiteSetting)DestinationSiteSetting;
                //syncTasks.Add(syncTask);
                //syncTask.ScheduleInterval = 0;
                Logger.Error("16", "SyncCopyListWizardForm");
                CurrentTabIndex = 3;
                WizardTabControl.SelectedIndex = CurrentTabIndex;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "SyncCopyListWizardForm");
            }
        }

        private void ScheduleNextButton_Click(object sender, RoutedEventArgs e)
        {
            SyncTask.ScheduleInterval = 0;
            SyncTask.ShouldSkipUpdates = SkipUpdateEventsCheckBox.IsChecked.Value;
            if (RunOnceRadioButtonYes.IsChecked == false)
            {
                int frequency = 0;
                if (int.TryParse(FrequencyTextBox.Text, out frequency) == false)
                {
                    MessageBox.Show("This should be a numeric value");
                    //FrequencyTextBox.Select();
                    FrequencyTextBox.Focus();
                    return;
                }

                SyncTask.ScheduleInterval = frequency * 60 * 1000;
            }
            SyncTask.Scheduled = true;
            SyncTask.Status = "Awaiting to be queued";
            Logger.Error("17", "SyncCopyListWizardForm");
            if (IsInEditMode == false)
            {
                SyncTasksManager.GetInstance().AddSyncTask(SyncTask);
            }
            Logger.Error("18", "SyncCopyListWizardForm");
            SyncTasksManager.GetInstance().SaveSyncTasks();
            Logger.Error("19", "SyncCopyListWizardForm");
            MessageBox.Show("Synchronization has started.");
            this.Close(true);

            Logger.Error("20", "SyncCopyListWizardForm");
            SyncTaskProgressForm form = new SyncTaskProgressForm();
            form.SyncTask = SyncTask;
            form.IsCompleted = false;
            form.ShowDialog(null, "Progress", false, true);

        }

        private void RunOnceRadioButtonYes_Checked(object sender, RoutedEventArgs e)
        {
            FrequencyTextBox.IsEnabled = !RunOnceRadioButtonYes.IsChecked.Value;
        }

        private void RunOnceRadioButtonNo_Checked(object sender, RoutedEventArgs e)
        {
            FrequencyTextBox.IsEnabled = !RunOnceRadioButtonYes.IsChecked.Value;
        }

        private void PopulateMappingFields()
        {
            int top = 0;
            int height = 20;
            FieldMappingsStackPanel.Children.Clear();
            List<QueryResultMappingSelectField> sourceFields = new List<QueryResultMappingSelectField>();
            foreach(QueryResultMapping sourceQueryResultMapping in SyncTask.SourceQueryResultMapping.Mappings){
                sourceFields.AddRange(sourceQueryResultMapping.SelectFields);
            }


            QueryResultMappingSelectField notMappedField = new QueryResultMappingSelectField()
            {
                FieldName = string.Empty,
                OutputHeaderName = "--- Not Mapped"
            };
            QueryResultMappingSelectField manuelValue = new QueryResultMappingSelectField()
            {
                FieldName = string.Empty,
                OutputHeaderName = "--- Apply Manuel Value"
            };
            UpdateSourceFieldComboBox.ItemsSource = sourceFields.ToList();
            UpdateDestinationFieldComboBox.ItemsSource = DestinationFolderFields.ToList();
            if(string.IsNullOrEmpty(SyncTask.SourceQueryResultMapping.Mappings[0].QueryResult.PrimaryIdFieldName)== false)
            {
                QueryResultMappingSelectField primaryIdQueryResultMappingSelectField = sourceFields.Where(t => t.FieldName.Equals(SyncTask.SourceQueryResultMapping.Mappings[0].QueryResult.PrimaryIdFieldName)).FirstOrDefault();
                UpdateSourceFieldComboBox.SelectedItem = primaryIdQueryResultMappingSelectField;
            }

            sourceFields.Insert(0, manuelValue);
            sourceFields.Insert(0, notMappedField);

            FieldCollection destinationFields = new FieldCollection();
            destinationFields.AddRange(DestinationFolderFields);

            foreach (Field field in destinationFields)
            {
                if (field.ReadOnly == true && field.Name.Equals("Title", StringComparison.InvariantCultureIgnoreCase) == false)
                    continue;

                Label label = new Label() { Content = field.DisplayName.removeTextInsideParenthesis() };

                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                label.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                label.Margin = new Thickness(0, top, 5, 0);
                FieldMappingsStackPanel.Children.Add(label);
                ComboBox sourceComboBox = new ComboBox();
                sourceComboBox.Name = field.Name + "_SourceComboBox";
                sourceComboBox.ItemsSource = sourceFields;
                sourceComboBox.Tag = field;

                QueryResultMappingSelectField matchField = null;
                /*
                if (RequiredMappings.ContainsKey(field.Name) == true)
                {
                    string sourceFieldName = RequiredMappings[field.Name];
                    matchField = sourceFields.Where(t => t.FieldName.Equals(sourceFieldName)).FirstOrDefault();
                    sourceComboBox.IsReadOnly = true;
                    sourceComboBox.IsEnabled = false;
                }
                else
                */
                if (SyncTask.DestinationFieldMappings.Count > 0)
                {
                    for(int i=0;i< SyncTask.DestinationFieldMappings.Count; i++)
                    {
                        if(SyncTask.DestinationFieldMappings[i].FieldName.Equals(field.Name, StringComparison.InvariantCultureIgnoreCase) == true)
                        {
                            string sourceFieldName = SyncTask.SourceFieldHeaderMappings[i];
                            matchField = sourceFields.Where(t => t.FieldName.Equals(sourceFieldName)).FirstOrDefault();
                        }
                    }
                }
                else
                {
                    matchField = sourceFields.Where(t => t.FieldName.Equals(field.Name)).FirstOrDefault();
                }

                if (matchField != null)
                {
                    sourceComboBox.SelectedItem = matchField;
                }
                else
                {
                    sourceComboBox.SelectedItem = notMappedField;
                }

                sourceComboBox.SelectionChanged += SourceComboBox_SelectionChanged;
                sourceComboBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                sourceComboBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                sourceComboBox.Margin = new Thickness(160, top, 0, 0);//

                FieldMappingsStackPanel.Children.Add(sourceComboBox);

                TextBox manuelValueTextBox = new TextBox();
                manuelValueTextBox.Name = field.Name + "_ManuelValueTextBox";
                manuelValueTextBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                manuelValueTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                manuelValueTextBox.Margin = new Thickness(350, top, 0, 0);
                manuelValueTextBox.Width = 100;
                manuelValueTextBox.Height = height;
                manuelValueTextBox.Visibility = Visibility.Hidden;
                FieldMappingsStackPanel.Children.Add(manuelValueTextBox);

                top += height + 5;
            }

            FieldMappingsStackPanel.Height = top;

        }

        private void SourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)e.Source;
            Field destinationField = (Field)comboBox.Tag;
            string textboxName = destinationField.Name + "_ManuelValueTextBox";
            foreach (UIElement element in FieldMappingsStackPanel.Children)
            {
                if (element is TextBox == true)
                {
                    if (((TextBox)element).Name == textboxName)
                    {
                        TextBox textBox = (TextBox)element;
                        if (((QueryResultMappingSelectField)comboBox.SelectedItem).OutputHeaderName == "--- Apply Manuel Value")
                        {
                            textBox.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            textBox.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }

        private void MappingBackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 1;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        private void ScheduleBackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 2;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        /*
        private void SourceSelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectEntityForm selectEntityForm = new SelectEntityForm();
            selectEntityForm.Initialize(new Type[] { typeof(SPList) });
            if (selectEntityForm.ShowDialog(this.ParentWindow, "Select an object to sync from") == true)
            {
                SourceFolder = selectEntityForm.SelectedObject;
            }
        }
        */

        private void DestinationSelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectEntityForm selectEntityForm = new SelectEntityForm();
            selectEntityForm.Initialize(new Type[] { typeof(SPList), typeof(SQLTable) });
            if (selectEntityForm.ShowDialog(this.ParentWindow, "Select an object to sync to") == true)
            {
                DestinationFolder = selectEntityForm.SelectedObject;
                DestinationSelectButton.Content = DestinationFolder.GetPath();
                PopulateDestinationDetails();
            }
        }


        private void HostControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SyncTask == null)
            {
                SyncTask = SyncTask.NewSyncTask();
            }
            else
            {
                SourceQueryGeneratorControl.Initialize(SyncTask.SourceQueryResultMapping);
                DestinationSiteSetting = SyncTask.DestinationSiteSetting;
                QueryResult sourceQueryResult = SyncTask.SourceQueryResultMapping.Mappings[0].QueryResult;
                //SourceSiteSetting = sourceQueryResult.SiteSetting;

                DestinationFolder = ApplicationContext.Current.GetFolder(SyncTask.DestinationSiteSetting, new BasicFolderDefinition() {
                    FolderUrl= DestinationSiteSetting.Url + "/" + SyncTask.DestinationFolderPath,
                    SiteSettingID = DestinationSiteSetting.ID,
                    Title = SyncTask.DestinationListName
                });
                DestinationSelectButton.Content = DestinationFolder.GetPath();
                PopulateDestinationDetails();
                SkipUpdateEventsCheckBox.IsChecked = SyncTask.ShouldSkipUpdates;
                if(SyncTask.ScheduleInterval == 0)
                {
                    RunOnceRadioButtonNo.IsChecked = false;
                    RunOnceRadioButtonYes.IsChecked = true;
                    FrequencyTextBox.Text = "0";
                }
                else
                {
                    RunOnceRadioButtonNo.IsChecked = true;
                    RunOnceRadioButtonYes.IsChecked = false;
                    FrequencyTextBox.Text = SyncTask.ScheduleIntervalAsMinute.ToString();
                }

                /*
                SourceFolder = ApplicationContext.Current.GetFolder(SourceSiteSetting, new BasicFolderDefinition()
                {
                    FolderUrl = SourceSiteSetting.Url + "/" + sourceQueryResult.ListName,
                    SiteSettingID = SourceSiteSetting.ID,
                    Title = SyncTask.DestinationListName
                });

                SourceSelectButton.Content = SourceFolder.GetPath();
                DestinationSelectButton.Content = DestinationFolder.GetPath();
                */
            }

        }

        private void DestinationNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (DestinationFolder == null)
            {
                MessageBox.Show("Please select a list object for destination");
                return;
            }

            CurrentTabIndex = 2;
            WizardTabControl.SelectedIndex = CurrentTabIndex;

        }

        private void PopulateDestinationDetails()
        {
            DestinationSiteSetting = ApplicationContext.Current.GetSiteSetting(DestinationFolder.SiteSettingID);
            Logger.Error("6", "SyncCopyListWizardForm");
            DestinationFolderFields = ApplicationContext.Current.GetFields(DestinationSiteSetting, DestinationFolder);
            Logger.Error("7", "SyncCopyListWizardForm");
            /*
            if (DestinationFolder.IsDocumentLibrary == false)
            {
                Logger.Error("8", "SyncCopyListWizardForm");

                Field field = DestinationFolderFields.Where(t => t.Name.Equals("SourceItemID")).FirstOrDefault();
                if (field == null)
                {
                    MessageBoxResult result = MessageBox.Show("Destination list requires SourceItemID field which is missing\nWould you like create the field and continue?", "Destination list requires SourceItemID field which is missing", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        List<Field> sourceItemIdFields = new List<Field>();
                        sourceItemIdFields.Add(
                            new Field()
                            {
                                Type = FieldTypes.Text,
                                Name = "SourceItemID",
                                DisplayName = "SourceItemID"
                            });
                        ApplicationContext.Current.CreateFields(DestinationSiteSetting, DestinationFolder, sourceItemIdFields);
                        DestinationFolderFields = ApplicationContext.Current.GetFields(DestinationSiteSetting, DestinationFolder);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            */
            Field ctField1 = new Field()
            {
                ID = Guid.Empty,
                Name = "ContentType",
                DisplayName = "ContentType",
                FromBaseType = false
            };
            DestinationFolderFields.Add(ctField1);

            PopulateMappingFields();
        }

        private void DestinationBackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 0;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }

        private void AllowUpdateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetUpdateControlsEnability();
        }

        private void AllowUpdateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SetUpdateControlsEnability();
        }

        private void SetUpdateControlsEnability()
        {
            if (SyncTask.SourceQueryResultMapping.Mappings[0].QueryResult.SiteSetting.SiteSettingType == SiteSettingTypes.SQLServer)
            {
                UpdateSourceFieldComboBox.IsEnabled = AllowUpdateCheckBox.IsChecked.Value;
            }

            UpdateDestinationFieldComboBox.IsEnabled = AllowUpdateCheckBox.IsChecked.Value;
        }
    }
}
