using Microsoft.Win32;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common.SLExcelUtility;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class ImportWizardForm : HostControl
    {
        BackgroundWorker SyncBackgroundWorker = new BackgroundWorker();

        private List<IQueryPanel> _QueryPanels = null;
        private Folder _MainObject = null;
        private string _SelectedFileFullPath = string.Empty;
        private List<System.Windows.Controls.ComboBox> HeadersComboBoxes = null;
        private SLExcelData ExcelData = null;
        private Dictionary<string, string> ValueTransformationSyntaxes = new Dictionary<string, string>();
        private Dictionary<string, string> ManuelValues = new Dictionary<string, string>();
        private List<string> ExcludedInActiveRecordFields = new List<string>();
        public ImportWizardForm()
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
            this.OKButtonSelected += ViewRelationForm_OKButtonSelected;
            SyncBackgroundWorker.DoWork += SyncBackgroundWorker_DoWork;
            SyncBackgroundWorker.ProgressChanged += SyncBackgroundWorker_ProgressChanged;
            SyncBackgroundWorker.RunWorkerCompleted += SyncBackgroundWorker_RunWorkerCompleted;
            SyncBackgroundWorker.WorkerReportsProgress = true;
            SyncBackgroundWorker.WorkerSupportsCancellation = true;

            SelectedEntityNameLabel.Content = _MainObject.Title;
        }

        private void SyncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ImportDataProgressBar.Value = 100;
            ImportDataProgressTextBox.Content = "Completed.";
        }

        private void SyncBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ImportDataProgressBar.Value = e.ProgressPercentage;
            ImportDataProgressBar.InvalidateVisual();
            ImportDataProgressTextBox.Content = e.ProgressPercentage + "%  ->  " + e.UserState.ToString();
            ImportDataProgressTextBox.InvalidateVisual();
        }
        private void SyncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arguments = (object[])e.Argument;
            string action = arguments[0].ToString();
            Dictionary<string, object[]> mapping = (Dictionary<string, object[]>)arguments[1];
            DataView dataView = (DataView)arguments[2];

            if (action == "Validate") {
                ValidateImport(mapping, dataView);
            }
            else
            {
                ProcessImport(mapping, dataView);
            }

        }

        void ViewRelationForm_OKButtonSelected(object sender, EventArgs e)
        {

        }




        public void Initialize(List<IQueryPanel> queryPanels, Folder mainObject)
        {
            _QueryPanels = queryPanels;
            _MainObject = mainObject;
        }

        private void AddNode(ItemCollection itemCollection, Folder folder)
        {
            TreeViewItem rootNode = new TreeViewItem();

            DockPanel treenodeDock = new DockPanel();

            DockPanel folderTitleDock = new DockPanel();
            Image img = new Image();
            Uri uri = new Uri("/Sobiens.Connectors.Studio.UI.Controls;component/Images/" + folder.IconName + ".GIF", UriKind.Relative);
            ImageSource imgSource = new BitmapImage(uri);
            img.Source = imgSource;

            Label lbl = new Label();
            lbl.Content = folder.Title;

            folderTitleDock.Children.Add(img);
            folderTitleDock.Children.Add(lbl);
            treenodeDock.Children.Add(folderTitleDock);

            rootNode.Header = treenodeDock;
            //rootNode.Expanded += new RoutedEventHandler(rootNode_Expanded);
            rootNode.Tag = folder;
            itemCollection.Add(rootNode);

            foreach (Folder subfolder in folder.Folders)
            {
                AddNode(rootNode.Items, subfolder);
            }

            if (folder.Folders.Count > 0)
            {
                rootNode.IsExpanded = true;
            }

        }

        private void SelectImportFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _SelectedFileFullPath = openFileDialog.FileName;
                SelectedFileLabel.Content = (new FileInfo(openFileDialog.FileName)).Name;
            }
            else
            {
                return;
            }

            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new System.Uri("pack://application:,,,/Sobiens.Connectors.Studio.UI.Controls;component/Style/MainStyle.xaml", UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(dict);
            Style genericButtonStyle = this.FindResource("GenericButtonStyle") as Style;
            Style genericComboBoxStyle = this.FindResource("GenericComboBoxStyle") as Style;
            Style genericTextBoxStyle = this.FindResource("GenericTextBoxStyle") as Style;

            SLExcelReader reader = new SLExcelReader();
            ExcelData = reader.ReadExcel(_SelectedFileFullPath);
            SiteSetting siteSetting = ApplicationContext.Current.Configuration.SiteSettings[_MainObject.SiteSettingID];
            List<Field> fields = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetFields(siteSetting, _MainObject);

            HeadersComboBoxes = new List<ComboBox>();

            DynamicGrid.ColumnDefinitions.Clear();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            ColumnDefinition gridCol3 = new ColumnDefinition();
            ColumnDefinition gridCol4 = new ColumnDefinition();
            ColumnDefinition gridCol5 = new ColumnDefinition();
            ColumnDefinition gridCol6 = new ColumnDefinition();
            gridCol1.Width = GridLength.Auto;
            gridCol2.Width = GridLength.Auto;
            gridCol3.Width = GridLength.Auto;
            gridCol4.Width = GridLength.Auto;
            gridCol5.Width = GridLength.Auto;
            gridCol6.Width = GridLength.Auto;
            DynamicGrid.ColumnDefinitions.Add(gridCol1);
            DynamicGrid.ColumnDefinitions.Add(gridCol2);
            DynamicGrid.ColumnDefinitions.Add(gridCol3);
            DynamicGrid.ColumnDefinitions.Add(gridCol4);
            DynamicGrid.ColumnDefinitions.Add(gridCol5);
            DynamicGrid.ColumnDefinitions.Add(gridCol6);
            int currentRowIndex = 0;
            int currentFieldIndex = 0;
            InActiveRecordsExcludedFieldsMenuItem.Items.Clear();
            for (int i = 0; i < fields.Count; i++)
            {
                Field field = fields[i];
                if (field.ReadOnly == true)
                    continue;

                if(field.Type == FieldTypes.Lookup)
                {
                    MenuItem mi = new MenuItem();

                    CheckBox checkBox = new CheckBox();
                    checkBox.Tag = field;
                    checkBox.Content = field.DisplayName;
                    checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                    checkBox.Margin = new Thickness(0, 0, 0, 0);
                    checkBox.VerticalAlignment = VerticalAlignment.Top;
                    checkBox.Click += CheckBox_Click; ;
                    mi.Header = checkBox;
                    InActiveRecordsExcludedFieldsMenuItem.Items.Add(mi);
                }

                if (currentFieldIndex % 2 == 0)
                {
                    RowDefinition gridRow1 = new RowDefinition();
                    gridRow1.Height = new GridLength(30);
                    DynamicGrid.RowDefinitions.Add(gridRow1);
                }

                System.Windows.Controls.ComboBox comboBox = new ComboBox();
                comboBox.Width = 150;
                comboBox.Height = 25;
                comboBox.Background = Brushes.LightBlue;
                comboBox.Tag = field;
                comboBox.Style = genericComboBoxStyle;
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
                HeadersComboBoxes.Add(comboBox);
                ComboBoxItem emptycboxitem = new ComboBoxItem();
                emptycboxitem.Content = "--- Not Mapped";
                emptycboxitem.IsSelected = true;
                emptycboxitem.Tag = "NotMapped";
                comboBox.Items.Add(emptycboxitem);
                ComboBoxItem manuelValuecboxitem = new ComboBoxItem();
                manuelValuecboxitem.Content = "---Apply Manuel Value";
                manuelValuecboxitem.IsSelected = false;
                manuelValuecboxitem.Tag = "ManuelValue";
                comboBox.Items.Add(manuelValuecboxitem);

                bool mappedWithAnExcelHeader = false;
                foreach (string excelHeaderName in ExcelData.Headers)
                {
                    ComboBoxItem cboxitem = new ComboBoxItem();
                    cboxitem.Content = excelHeaderName;
                    cboxitem.Tag = excelHeaderName;
                    if (field.Name.Equals(excelHeaderName, StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        mappedWithAnExcelHeader = true;
                        cboxitem.IsSelected = true;
                    }
                    else if (field.DisplayName.Equals(excelHeaderName, StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        mappedWithAnExcelHeader = true;
                        cboxitem.IsSelected = true;
                    }
                    comboBox.Items.Add(cboxitem);
                }

                TextBox manuelValueTextBox = new TextBox();
                manuelValueTextBox.Name = field.Name + "_ManuelValueTextBox";
                manuelValueTextBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                manuelValueTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                manuelValueTextBox.Width = 100;
                manuelValueTextBox.Height = 30;
                manuelValueTextBox.Tag = field;
                manuelValueTextBox.Style = genericTextBoxStyle;
                manuelValueTextBox.Visibility = Visibility.Hidden;
                manuelValueTextBox.TextChanged += ManuelValueTextBox_TextChanged;
                Grid.SetRow(manuelValueTextBox, currentRowIndex);
                Grid.SetColumn(manuelValueTextBox, ((currentFieldIndex % 2) * 3) + 2);
                DynamicGrid.Children.Add(manuelValueTextBox);

                Button valueTransformationButton = new Button();
                valueTransformationButton.Name = field.Name + "_FieldValueTransformationButton";
                valueTransformationButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                valueTransformationButton.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                valueTransformationButton.Tag = field;
                valueTransformationButton.Visibility = Visibility.Hidden;
                if (mappedWithAnExcelHeader == true)
                {
                    valueTransformationButton.Visibility = Visibility.Visible;
                    string valueTransformationSyntax = string.Empty;
                    if (ValueTransformationSyntaxes.ContainsKey(field.Name) == true)
                        valueTransformationSyntax = ValueTransformationSyntaxes[field.Name];
                    if (string.IsNullOrEmpty(valueTransformationSyntax) == false)
                    {
                        valueTransformationButton.Background = Brushes.DarkOrange;
                    }

                }
                valueTransformationButton.Content = "Expression...";
                valueTransformationButton.Click += ValueTransformationButton_Click;
                valueTransformationButton.Height = 25;
                valueTransformationButton.Style = genericButtonStyle;
                Grid.SetRow(valueTransformationButton, currentRowIndex);
                Grid.SetColumn(valueTransformationButton, ((currentFieldIndex % 2) * 3) + 2);
                DynamicGrid.Children.Add(valueTransformationButton);

                Grid.SetRow(comboBox, currentRowIndex);
                Grid.SetColumn(comboBox, ((currentFieldIndex % 2)*3) + 1);
                DynamicGrid.Children.Add(comboBox);

                Label label = new Label();
                label.Content = field.DisplayName + (field.Required == true ? "*" : "");
                Grid.SetRow(label, currentRowIndex);
                Grid.SetColumn(label, ((currentFieldIndex % 2) * 3) + 0);
                DynamicGrid.Children.Add(label);
                currentFieldIndex++;
                if (currentFieldIndex % 2 == 0)
                {
                    currentRowIndex++;
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)e.Source;
            Field field = (Field)chk.Tag;
            if (chk.IsChecked == true)
            {
                if (ExcludedInActiveRecordFields.Contains(field.Name) == false)
                    ExcludedInActiveRecordFields.Add(field.Name);
            }
            else
            {
                if (ExcludedInActiveRecordFields.Contains(field.Name) == true)
                    ExcludedInActiveRecordFields.Remove(field.Name);
            }

            SetExcludeInActiveRecordFieldNamesLabel();
        }

        private void ManuelValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string fieldName = ((Field)textBox.Tag).Name;
            if (ManuelValues.ContainsKey(fieldName) == true)
            {
                ManuelValues[fieldName] = textBox.Text;
            }
            else
            {
                ManuelValues.Add(fieldName, textBox.Text);
            }

        }

        private void ValueTransformationButton_Click(object sender, RoutedEventArgs e)
        {
            ValueTransformationForm vtf = new ValueTransformationForm();
            bool? result = vtf.ShowDialog(this.ParentWindow, "Value expression editor");
            if (result == true)
            {
                Button button = (Button)sender;
                string fieldName = ((Field)button.Tag).Name;
                if(ValueTransformationSyntaxes.ContainsKey(fieldName) == true)
                {
                    ValueTransformationSyntaxes[fieldName] = vtf.ValueTransformationSyntax; 
                }
                else
                {
                    ValueTransformationSyntaxes.Add(fieldName, vtf.ValueTransformationSyntax);
                }
                if (string.IsNullOrEmpty(vtf.ValueTransformationSyntax) == false)
                {
                    button.Background = Brushes.DarkOrange;
                }
                else
                {
                    button.Background = Brushes.LightGray;
                }

                PopulateResultGrid();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImportButton.Content = "Validate";
            ComboBox comboBox = (ComboBox)sender;
            Field destinationField = (Field)comboBox.Tag;
            string textboxName = destinationField.Name + "_ManuelValueTextBox";
            string valueTransformationButtonName = destinationField.Name + "_FieldValueTransformationButton";
            foreach (UIElement element in DynamicGrid.Children)
            {
                if (element is TextBox == true)
                {
                    if (((TextBox)element).Name == textboxName)
                    {
                        TextBox textBox = (TextBox)element;
                        if (((ComboBoxItem)comboBox.SelectedItem).Tag.ToString() == "ManuelValue")
                        {
                            textBox.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            textBox.Visibility = Visibility.Hidden;
                        }
                    }
                }
                if (element is Button == true)
                {
                    if (((Button)element).Name == valueTransformationButtonName)
                    {
                        Button button = (Button)element;
                        if (((ComboBoxItem)comboBox.SelectedItem).Tag.ToString() != "NotMapped"
                            && ((ComboBoxItem)comboBox.SelectedItem).Tag.ToString() != "ManuelValue")
                        {
                            button.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            button.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
            PopulateResultGrid();
        }

        private void PopulateResultGrid()
        {
            Dictionary<string, int> excelHeaderIndexes = new Dictionary<string, int>();
            DataTable dataTable = new DataTable();
            ResultGrid.Columns.Clear();

            dataTable.Columns.Add("SobiensFieldStatus", typeof(string));
            dataTable.Columns.Add("SobiensFieldStatusMessage", typeof(string));
            DataGridTextColumn statusColumn = new DataGridTextColumn();
            statusColumn.IsReadOnly = true;
            statusColumn.Binding = new System.Windows.Data.Binding("SobiensFieldStatus");
            statusColumn.Header = "Status";
            statusColumn.Width = new DataGridLength(50);
            ResultGrid.Columns.Add(statusColumn);

            for (int i = 0; i < HeadersComboBoxes.Count; i++)
            {
                ComboBox headersComboBox = HeadersComboBoxes[i];
                Field field = (Field)headersComboBox.Tag;
                string selectedExcelHeaderName = ((ComboBoxItem)headersComboBox.SelectedItem).Tag.ToString();
                if (selectedExcelHeaderName == "")
                    continue;

                DataGridTextColumn column = new DataGridTextColumn();
                column.IsReadOnly = true;
                column.Binding = new System.Windows.Data.Binding(field.Name);
                column.Header = field.DisplayName;
                ResultGrid.Columns.Add(column);
                dataTable.Columns.Add(field.Name);
            }

            for (int i = 0; i < ExcelData.Headers.Count; i++)
            {
                excelHeaderIndexes.Add(ExcelData.Headers[i], i);
            }


            for (int i = 0; i < ExcelData.DataRows.Count; i++)
            {
                DataRow row = dataTable.NewRow();
                for (int x = 0; x < HeadersComboBoxes.Count; x++)
                {
                    ComboBoxItem headerMappingComboBox = HeadersComboBoxes[x].SelectedItem as ComboBoxItem;
                    string excelHeaderName = headerMappingComboBox.Tag.ToString();
                    if (string.IsNullOrEmpty(excelHeaderName) == true
                        || excelHeaderName == "NotMapped"
                        )
                        continue;

                    System.Windows.Forms.DataGridViewTextBoxCell cell = new System.Windows.Forms.DataGridViewTextBoxCell();
                    
                    string mappedFieldName = ((Field)HeadersComboBoxes[x].Tag).Name;
                    if(excelHeaderName == "ManuelValue")
                    {
                        if (ManuelValues.ContainsKey(mappedFieldName) == true)
                            row[mappedFieldName] = ManuelValues[mappedFieldName];
                        else
                            row[mappedFieldName] = string.Empty;
                    }
                    else
                    {
                        int excelHeaderIndex = excelHeaderIndexes[excelHeaderName];
                        string value = string.Empty;
                        if (ExcelData.DataRows[i].Count > excelHeaderIndex)
                            value= ExcelData.DataRows[i][excelHeaderIndex];

                        row[mappedFieldName] = value;
                        if (ValueTransformationSyntaxes.ContainsKey(mappedFieldName) == true
                            && string.IsNullOrEmpty(ValueTransformationSyntaxes[mappedFieldName]) == false)
                            row[mappedFieldName] = ValueTransformationHelper.Transform(value, ValueTransformationSyntaxes[mappedFieldName]);
                    }
                }

                dataTable.Rows.Add(row);
            }

            ResultGrid.ItemsSource = dataTable.AsDataView();

        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImportButton.Content.ToString() == "Canceling...")
                return;
            
            if (ImportButton.Content.ToString() == "Cancel")
            {
                ImportButton.Content = "Canceling...";
                SyncBackgroundWorker.CancelAsync();
                return;
            }

            Dictionary<string, object[]> mapping = new Dictionary<string, object[]>();

            for (int i = 0; i < HeadersComboBoxes.Count; i++)
            {
                ComboBox headersComboBox = HeadersComboBoxes[i];
                Field field = (Field)headersComboBox.Tag;
                string selectedExcelHeaderName = ((ComboBoxItem)headersComboBox.SelectedItem).Tag.ToString();
                if (selectedExcelHeaderName == "" && field.Required == true)
                {
                    MessageBox.Show(field.DisplayName + " is a required field, please select an excel header.");
                    headersComboBox.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(selectedExcelHeaderName) == false)
                {
                    mapping.Add(field.Name, new object[] { selectedExcelHeaderName, field });
                }
            }

            DataView dataView = (DataView)ResultGrid.ItemsSource;


            if (ImportButton.Content.ToString() == "Validate")
            {
                SyncBackgroundWorker.RunWorkerAsync(new object[] { "Validate", mapping, dataView });
            }
            else
            {
                SyncBackgroundWorker.RunWorkerAsync(new object[] { "Import", mapping, dataView });
            }

        }

        private void ProcessImport(Dictionary<string, object[]> mapping, DataView dataView)
        {
            Dispatcher.Invoke(() =>
            {
                ImportButton.Content = "Cancel";
            });

            int currentPercentage = 0;
            SyncBackgroundWorker.ReportProgress(currentPercentage, "0 / " + dataView.Table.Rows.Count);
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);

            for (int i = 0; i < dataView.Table.Rows.Count; i++)
            {
                if(SyncBackgroundWorker.CancellationPending == true)
                {
                    MessageBox.Show("Cancelled");
                    Dispatcher.Invoke(() =>
                    {
                        ImportButton.Content = "Import";
                    });
                    return;
                }

                currentPercentage = (i * 100 / dataView.Table.Rows.Count);
                SyncBackgroundWorker.ReportProgress(currentPercentage, i + " / " + dataView.Table.Rows.Count);

                Dictionary<string, object> fieldValues = new Dictionary<string, object>();
                foreach (string fieldName in mapping.Keys)
                {
                    string excelHeaderName = ((object[])mapping[fieldName])[0].ToString();

                    if (string.IsNullOrEmpty(excelHeaderName) == true)
                        continue;

                    Field field = (Field)((object[])mapping[fieldName])[1];
                    string mappedFieldName = field.Name;
                    string value = dataView.Table.Rows[i][mappedFieldName].ToString();
                    object objecValue = serviceManager.ConvertImportValueToFieldValue(siteSetting, field, value, new Dictionary<string, string>());
                    fieldValues.Add(field.Name, objecValue);
                }

                serviceManager.CreateListItem(siteSetting, _MainObject.GetWebUrl(), _MainObject.GetListName(), fieldValues);
                dataView.Table.Rows[i]["SobiensFieldStatus"] = "Imported";
            }

            //MessageBox.Show("Completed successfully");
        }
        private string GetExcludeInActiveRecordFieldNames()
        {
            string excludeInActiveRecordFieldNames = string.Empty;
            foreach (string fieldName in ExcludedInActiveRecordFields)
            {
                if (string.IsNullOrEmpty(excludeInActiveRecordFieldNames) == false)
                    excludeInActiveRecordFieldNames += ";#";
                excludeInActiveRecordFieldNames += fieldName;
            }

            return excludeInActiveRecordFieldNames;
        }

        private void SetExcludeInActiveRecordFieldNamesLabel()
        {
            string excludeInActiveRecordFieldNames = string.Empty;
            foreach (string fieldName in ExcludedInActiveRecordFields)
            {
                if (string.IsNullOrEmpty(excludeInActiveRecordFieldNames) == false)
                    excludeInActiveRecordFieldNames += ", ";
                excludeInActiveRecordFieldNames += fieldName;
            }

            if (string.IsNullOrEmpty(excludeInActiveRecordFieldNames) == true)
                excludeInActiveRecordFieldNames = "Not Selected";

            SelectedInActiveRecordsExcludedFieldsLabel.Content = excludeInActiveRecordFieldNames;
        }

        private void ValidateImport(Dictionary<string, object[]> mapping, DataView dataView)
        {
            Dispatcher.Invoke(() =>
            {
                ImportButton.Content = "Cancel";
            });
            bool isValid = true;
            try
            {
                Dictionary<string, string> validationParameters = new Dictionary<string, string>();
                validationParameters.Add("ExcludeInActiveRecordFieldNames", GetExcludeInActiveRecordFieldNames());

                ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
                IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);

                SyncBackgroundWorker.ReportProgress(0, "0 / " + dataView.Table.Rows.Count);
                /*
                for (int i = 0; i < HeadersComboBoxes.Count; i++)
                {
                    ComboBox headersComboBox = HeadersComboBoxes[i];
                    Field field = (Field)headersComboBox.Tag;
                    string selectedExcelHeaderName = ((ComboBoxItem)headersComboBox.SelectedItem).Tag.ToString();
                    if (selectedExcelHeaderName == "" && field.Required == true)
                    {
                        isValid = false;
                        MessageBox.Show(field.DisplayName + " is a required field, please select an excel header.");
                        headersComboBox.Focus();
                        return;
                    }
                }
                */
                for (int i = 0; i < dataView.Table.Rows.Count; i++)
                {
                    if(SyncBackgroundWorker.CancellationPending == true)
                    {
                        MessageBox.Show("Cancelled");
                        Dispatcher.Invoke(() =>
                        {
                            ImportButton.Content = "Validate";
                        });
                        return;
                    }

                    int currentPercentage = (i * 100 / dataView.Table.Rows.Count);
                    SyncBackgroundWorker.ReportProgress(currentPercentage, i + " / " + dataView.Table.Rows.Count);
                    List<string> errorMessages = new List<string>();
                    foreach (string fieldName in mapping.Keys)
                    {
                        string excelHeaderName = ((object[])mapping[fieldName])[0].ToString();
                        if (string.IsNullOrEmpty(excelHeaderName) == true || excelHeaderName == "NotMapped")
                            continue;

                        Field field = (Field)((object[])mapping[fieldName])[1];
                        string mappedFieldName = field.Name;
                        string value = dataView.Table.Rows[i][mappedFieldName].ToString();
                        string _errorMessage;
                        if (serviceManager.ValidateImportValue(siteSetting, field, value, validationParameters, out _errorMessage) == false)
                            errorMessages.Add(_errorMessage);
                    }
                    if (errorMessages.Count > 0)
                        isValid = false;

                    dataView.Table.Rows[i]["SobiensFieldStatus"] = (errorMessages.Count > 0 ? "Invalid" : "Valid");
                    string errorMessageString = string.Empty;
                    foreach (string _errorMessage in errorMessages)
                    {
                        if (string.IsNullOrEmpty(errorMessageString) == false)
                            errorMessageString += Environment.NewLine;

                        errorMessageString += _errorMessage;
                    }
                    dataView.Table.Rows[i]["SobiensFieldStatusMessage"] = errorMessageString;
                    if (errorMessages.Count == 0)
                        dataView.Table.Rows[i]["SobiensFieldStatus"] = "Valid";
                }
            }
            catch (Exception ex)
            {
                int c = 3;
            }

            if (isValid == true)
            {
                MessageBox.Show("Validated successfully");
                Dispatcher.Invoke(() =>
                {
                    ImportButton.Content = "Import";
                });
            }
            else
            {
                MessageBox.Show("There are some errors, please check");
                Dispatcher.Invoke(() =>
                {
                    ImportButton.Content = "Validate";
                });
            }
        }

        private void SelectedInActiveRecordsExcludedFieldsLabel_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
