using Microsoft.Win32;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.SLExcelUtility;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        }
        private void SyncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void SyncBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SyncDataProgressBar.Value = e.ProgressPercentage;
            //SyncDataProgressBar.InvalidateVisual();
            //SyncDataProgressBar.UpdateLayout();
            ProgressTextBox.Text = e.ProgressPercentage + "% " + e.UserState.ToString();
            //ProgressTextBox.InvalidateVisual();
            //ProgressTextBox.UpdateLayout();
        }

        private void SyncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arguments = (object[])e.Argument;

            SyncTask syncTask = (SyncTask)arguments[0];

            try
            {
                //SyncTasksManager.GetInstance().ExportSyncTaskItems(syncTask, shouldExportListItems, shouldExportDocuments, shouldImportListItems, SyncBackgroundWorker, null);

                    //SyncTasksManager.GetInstance().ProcessSyncTaskExportFiles(syncTask, SyncBackgroundWorker);
                    SyncTasksManager.GetInstance().ImportSyncTaskItems(syncTask, false, new string[] { }, SyncBackgroundWorker);

                string folderPath = ConfigurationManager.GetInstance().GetSyncTaskFolder(syncTask);
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured:" + ex.Message + Environment.NewLine + "StackTrace:" + ex.StackTrace);
                Logger.Error(ex, "ProcessSyncTaskExport");
            }

        }

        void ViewRelationForm_OKButtonSelected(object sender, EventArgs e)
        {

        }


        private void GenerateExcelImportTask()
        {
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
            List<string> excelHeaders = new List<string>();
            List<string> sharePointFieldNames = new List<string>();
            Dictionary<string, string> fieldMappings = new Dictionary<string, string>();
            foreach (UIElement element in DynamicGrid.Children)
            {
                if (element is ComboBox)
                {
                    string excelHeader = ((ComboBox)element).Tag.ToString();
                    string sharePointFieldName = ((ComboBoxItem)((ComboBox)element).SelectedItem).Tag.ToString();
                    if (string.IsNullOrEmpty(sharePointFieldName) == true)
                        continue;
                    excelHeaders.Add(excelHeader);
                    sharePointFieldNames.Add(sharePointFieldName);
                    fieldMappings.Add(excelHeader, sharePointFieldName);
                }
            }

            excelHeaders.Insert(0, "ID");
            sharePointFieldNames.Insert(0, "SourceItemID");

            //List<SyncTask> syncTasks = new List<SyncTask>();
            SyncTask syncTask = SyncTask.NewSyncTask();
            syncTask.ID = Guid.NewGuid();
            syncTask.ScheduleInterval = 240;

            string folderPath = ConfigurationManager.GetInstance().GetSyncTaskFolder(syncTask);
            System.IO.File.Copy(_SelectedFileFullPath, folderPath + "\\ProcessedExport.xlsx");

            QueryResult test1QueryResult = new QueryResult();
            test1QueryResult.Fields = excelHeaders.ToArray();
            test1QueryResult.ListName = "Test1";
            test1QueryResult.Name = "Test1";
            test1QueryResult.SiteSetting = null;
            test1QueryResult.Filters = new CamlFilters();

            /*
            test1QueryResult.ReferenceFields.Add(new QueryResultReferenceField()
            {
                SiteSetting = GetTestSiteSetting(),
                ReferenceFilterFieldName = "FilterValue",
                ReferenceListName = "TestLookup",
                ReferenceValueFieldName = "DataValue",
                SourceFieldName = "RefField",
                OutputName = "RefField1"
            });
            */

            QueryResultMapping test1QueryResultMapping = new QueryResultMapping();
            test1QueryResultMapping.QueryResult = test1QueryResult;
            List<QueryResultMappingSelectField> queryResultMappingSelectFields = new List<QueryResultMappingSelectField>();
            foreach (string excelHeader in excelHeaders)
            {
                queryResultMappingSelectFields.Add(new QueryResultMappingSelectField(excelHeader, excelHeader));
            }
            test1QueryResultMapping.SelectFields = queryResultMappingSelectFields.ToArray();
            syncTask.SourceQueryResultMapping.Mappings.Add(test1QueryResultMapping);

            syncTask.DestinationListName = _MainObject.GetListName();
            syncTask.DestinationRootFolderPath = _MainObject.GetListName();
            syncTask.Name = _MainObject.GetListName();
            syncTask.SourceUniqueFieldHeaderNames = new string[] { "ID" };
            syncTask.DestinationUniqueFieldNames = new string[] { "SourceItemID" };
            syncTask.DestinationIDFieldHeaderName = "ID";

            syncTask.SourceFieldHeaderMappings = excelHeaders.ToArray();
            //syncTask.DestinationFieldMappings = sharePointFieldNames.ToArray();
            syncTask.IsDestinationDocumentLibrary = false;
            syncTask.DestinationTermStoreName = "Taxonomy_BrjLUNqY3/3gqp8FAbbKiQ==";
            syncTask.DestinationSiteSetting = (SiteSetting)siteSetting;
            //syncTasks.Add(syncTask);
            //SyncTasksManager.GetInstance().SyncTasks.Clear();
            //SyncTasksManager.GetInstance().SyncTasks.Add(syncTask);
            //SyncTasksManager.GetInstance().SaveSyncTasks();
            SyncBackgroundWorker.RunWorkerAsync(new object[] { syncTask });


        }
        public string GetTempPath()
        {
            string folderPath = System.IO.Path.GetTempPath() + "SPCamlStudio";
            if (System.IO.Directory.Exists(folderPath) == false)
                System.IO.Directory.CreateDirectory(folderPath);
            //            folderPath = folderPath + "\\SPCamlStudio";
            //            if (System.IO.Directory.Exists(folderPath) == false)
            //                System.IO.Directory.CreateDirectory(folderPath);
            folderPath = folderPath + "\\" + Guid.NewGuid().ToString();
            if (System.IO.Directory.Exists(folderPath) == false)
                System.IO.Directory.CreateDirectory(folderPath);

            return folderPath;
        }



        public void Initialize(List<IQueryPanel> queryPanels, Folder mainObject)
        {
            _QueryPanels = queryPanels;
            _MainObject = mainObject;
            AddNode(ObjectsTreeView.Items, _MainObject);
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

            SLExcelReader reader = new SLExcelReader();
            SLExcelData excelData = reader.ReadExcel(_SelectedFileFullPath);
            SiteSetting siteSetting = ApplicationContext.Current.Configuration.SiteSettings[_MainObject.SiteSettingID];
            List<Field> fields = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetFields(siteSetting, _MainObject);

            DynamicGrid.ColumnDefinitions.Clear();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol1.Width = GridLength.Auto;
            gridCol2.Width = GridLength.Auto;
            DynamicGrid.ColumnDefinitions.Add(gridCol1);
            DynamicGrid.ColumnDefinitions.Add(gridCol2);
            for (int i = 0; i < excelData.Headers.Count; i++)
            {
                string headerTitle = excelData.Headers[i];
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(45);
                DynamicGrid.RowDefinitions.Add(gridRow1);


                System.Windows.Controls.ComboBox comboBox = new ComboBox();
                comboBox.Width = 200;
                comboBox.Height = 30;
                comboBox.Background = Brushes.LightBlue;
                ComboBoxItem emptycboxitem = new ComboBoxItem();
                emptycboxitem.Content = "Select a field";
                emptycboxitem.IsSelected = true;
                emptycboxitem.Tag = "";
                comboBox.Items.Add(emptycboxitem);
                foreach (Field field in fields)
                {
                    ComboBoxItem cboxitem = new ComboBoxItem();
                    cboxitem.Content = field.DisplayName;
                    cboxitem.Tag = field.Name;
                    if (field.Name.Equals(headerTitle, StringComparison.InvariantCultureIgnoreCase) == true)
                        cboxitem.IsSelected = true;
                    comboBox.Tag = headerTitle;
                    comboBox.Items.Add(cboxitem);
                }

                Grid.SetRow(comboBox, i);
                Grid.SetColumn(comboBox, 1);
                DynamicGrid.Children.Add(comboBox);

                Label label = new Label();
                label.Content = headerTitle;
                Grid.SetRow(label, i);
                Grid.SetColumn(label, 0);
                DynamicGrid.Children.Add(label);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(_MainObject.SiteSettingID);
            GenerateExcelImportTask();
            if (siteSetting.SiteSettingType == Entities.Settings.SiteSettingTypes.SQLServer)
            {
            }
            else if (siteSetting.SiteSettingType == Entities.Settings.SiteSettingTypes.SharePoint)
            {
            }
        }
    }
}
