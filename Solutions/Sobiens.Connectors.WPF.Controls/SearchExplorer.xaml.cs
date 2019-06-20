using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.WPF.Controls.Search;
using Sobiens.Connectors.Entities.Search;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Documents;
using Sobiens.Connectors.WPF.Controls.Settings;
using Sobiens.Connectors.WPF.Controls.Selectors;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for SearchExplorer.xaml
    /// </summary>
    public partial class SearchExplorer : HostControl, ISearchExplorer
    {
        public bool IsDataLoaded { get; set; }
        public SiteSettings SiteSettings { get; set; }
        public DocumentTemplates DocumentTemplates { get; set; }

        private Dictionary<string, SearchFilters> GeneralSearchFilters = new Dictionary<string, SearchFilters>();
        private Dictionary<string, SearchFilters> ExcelSearchFilters = new Dictionary<string, SearchFilters>();
        private Dictionary<string, SearchFilters> WordSearchFilters = new Dictionary<string, SearchFilters>();
        private Dictionary<string, SearchFilters> OutlookSearchFilters = new Dictionary<string, SearchFilters>();

        private DocumentTemplate SelectedDocumentTemplate = null;
        private DocumentTemplateMapping SelectedDocumentTemplateMapping = null;

        public void RefreshControls()
        {
            if (this.IsDataLoaded == false)
            {
                radioButtonWord_Checked(null, null);
                this.IsDataLoaded = true;
            }
        }

        public void Initialize(SiteSettings siteSettings, DocumentTemplates documentTemplates)
        {
            this.IsDataLoaded = false;
            this.SiteSettings = siteSettings;
            this.DocumentTemplates = documentTemplates;
        }

        public SearchExplorer()
        {
            InitializeComponent();
            this.radioButtonWord.Checked += new RoutedEventHandler(radioButtonWord_Checked);
            this.radioButtonExcel.Checked += new RoutedEventHandler(radioButtonExcel_Checked);
            this.radioButtonOutlook.Checked += new RoutedEventHandler(radioButtonOutlook_Checked);
            this.radioButtonGeneral.Checked += new RoutedEventHandler(radioButtonGeneral_Checked);
            SearchResultDataGridView.ContextMenu = new SearchViewItemMenu();

        }

        public bool HasAnythingToDisplay
        {
            get
            {
                return ConfigurationManager.GetInstance().GetDocumentTemplateMappings().Count > 0 ? true : false;
            }
        }

        void radioButtonGeneral_Checked(object sender, RoutedEventArgs e)
        {
        }

        void radioButtonOutlook_Checked(object sender, RoutedEventArgs e)
        {
            TemplateComboBox.Items.Clear();
            TemplateComboBox.Items.Add(Languages.Translate("Select a template..."));
            foreach (DocumentTemplate documentTemplate in this.DocumentTemplates.GetDocumentTemplates(ApplicationTypes.Outlook))
            {
                TemplateComboBox.Items.Add(documentTemplate);
            }
            TemplateComboBox.SelectedIndex = 0;
            if (SelectedDocumentTemplate != null)
            {
                //TemplateComboBox.SelectedValue = SelectedExcelDocumentTemplate;
            }

            SearchTextBox.Focus();
            FiltersButton.IsEnabled = false;
            SearchButton.IsEnabled = false;
            UpdateFilterButtonText();
        }

        void radioButtonExcel_Checked(object sender, RoutedEventArgs e)
        {
            TemplateComboBox.Items.Clear();
            TemplateComboBox.Items.Add(Languages.Translate("Select a template..."));
            foreach (DocumentTemplate documentTemplate in this.DocumentTemplates.GetDocumentTemplates(ApplicationTypes.Excel))
            {
                TemplateComboBox.Items.Add(documentTemplate);
            }
            TemplateComboBox.SelectedIndex = 0;
            if (SelectedDocumentTemplate != null)
            {
                //TemplateComboBox.SelectedValue = SelectedExcelDocumentTemplate;
            }

            SearchTextBox.Focus();
            FiltersButton.IsEnabled = false;
            SearchButton.IsEnabled = false;
            UpdateFilterButtonText();
        }

        void radioButtonWord_Checked(object sender, RoutedEventArgs e)
        {
            TemplateComboBox.Items.Clear();
            TemplateComboBox.Items.Add(Languages.Translate("Select a template..."));
            foreach (DocumentTemplate documentTemplate in this.DocumentTemplates.GetDocumentTemplates(ApplicationTypes.Word))
            {
                TemplateComboBox.Items.Add(documentTemplate);
            }
            TemplateComboBox.SelectedIndex = 0;
            if (SelectedDocumentTemplate != null)
            {
                //TemplateComboBox.SelectedValue = SelectedWordDocumentTemplate;
            }

            SearchTextBox.Focus();
            FiltersButton.IsEnabled = false;
            SearchButton.IsEnabled = false;
            UpdateFilterButtonText();
        }

        private void FiltersButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = this.SiteSettings[SelectedDocumentTemplateMapping.Folder.SiteSettingID];
            //IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            //Folder targetFolder = serviceManager.GetFolder(siteSetting, SelectedDocumentTemplateMapping.Folder);
            //ContentType contentType = serviceManager.GetContentType(siteSetting, targetFolder, SelectedDocumentTemplateMapping.ContentTypeID, false);
            SearchFiltersPanelControl searchFiltersPanelControl = new SearchFiltersPanelControl();
            searchFiltersPanelControl.Initialize(siteSetting, SelectedDocumentTemplateMapping.Folder, SelectedDocumentTemplateMapping.ContentTypeID);

            if (radioButtonWord.IsChecked == true)
            {
                searchFiltersPanelControl.SearchFilters = WordSearchFilters;
            }
            else if (radioButtonOutlook.IsChecked == true)
            {
                searchFiltersPanelControl.SearchFilters = OutlookSearchFilters;
            }
            else
            {
                searchFiltersPanelControl.SearchFilters = ExcelSearchFilters;
            }

            if(searchFiltersPanelControl.ShowDialog(null, Languages.Translate("Search Filters"), 450, 755) == true)
            {
                if (radioButtonWord.IsChecked == true)
                {
                    WordSearchFilters = searchFiltersPanelControl.SearchFilters;
                }
                else if (radioButtonOutlook.IsChecked == true)
                {
                    OutlookSearchFilters = searchFiltersPanelControl.SearchFilters;
                }
                else
                {
                    ExcelSearchFilters = searchFiltersPanelControl.SearchFilters;
                }
                UpdateFilterButtonText();
            }

            CheckSearchButtonEnability();
        }

        private void UpdateFilterButtonText()
        {
            if (radioButtonWord.IsChecked == true)
            {
                FiltersButton.Content = string.Format(Languages.Translate("Filters ({0})"), (WordSearchFilters == null?"0":WordSearchFilters.Count.ToString()));
            }
            else if (radioButtonOutlook.IsChecked == true)
            {
                FiltersButton.Content = string.Format(Languages.Translate("Filters ({0})"), (OutlookSearchFilters == null ? "0" : OutlookSearchFilters.Count.ToString()));
            }
            else
            {
                FiltersButton.Content = string.Format(Languages.Translate("Filters ({0})"), (ExcelSearchFilters == null ? "0" : ExcelSearchFilters.Count.ToString()));
            }
        }

        private void CheckSearchButtonEnability()
        {
            if (radioButtonWord.IsChecked == true)
            {
                if (TemplateComboBox.SelectedItem as DocumentTemplate != null
                    && (string.IsNullOrEmpty(SearchTextBox.Text) == false || WordSearchFilters.Count > 0))
                {
                    SearchButton.IsEnabled = true;
                }
                else
                {
                    SearchButton.IsEnabled = false;
                }
            }
            else if (radioButtonOutlook.IsChecked == true)
            {
                if (TemplateComboBox.SelectedItem as DocumentTemplate != null
                    && (string.IsNullOrEmpty(SearchTextBox.Text) == false || OutlookSearchFilters.Count > 0))
                {
                    SearchButton.IsEnabled = true;
                }
                else
                {
                    SearchButton.IsEnabled = false;
                }
            }
            else
            {
                if (TemplateComboBox.SelectedItem as DocumentTemplate != null
                    && (string.IsNullOrEmpty(SearchTextBox.Text) == false || ExcelSearchFilters.Count > 0))
                {
                    SearchButton.IsEnabled = true;
                }
                else
                {
                    SearchButton.IsEnabled = false;
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = this.SiteSettings[SelectedDocumentTemplateMapping.Folder.SiteSettingID];
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            Folder targetFolder = serviceManager.GetFolder(siteSetting, SelectedDocumentTemplateMapping.Folder);

            int itemCount;
            string listItemCollectionPositionNext = String.Empty;
            Folder selectedFolder = targetFolder;
            IView selectedView = null;
            string sortedFieldName = string.Empty;
            bool isAsc = true;
            int currentPageIndex = 0;
            string currentListItemCollectionPositionNext = "0";

            CamlFilters customFilters = new CamlFilters();
            customFilters.IsOr = false;

            Dictionary<string, SearchFilters> searchFilters;
            if (radioButtonWord.IsChecked == true)
            {
                searchFilters = WordSearchFilters;
            }
            else if (radioButtonOutlook.IsChecked == true)
            {
                searchFilters = OutlookSearchFilters;
            }
            else
            {
                searchFilters = ExcelSearchFilters;
            }

            foreach (string key in searchFilters.Keys)
            {
                CamlFilter customFilter = new CamlFilter(searchFilters[key][0]);
                customFilters.Add(customFilter);
            }

            List<IItem> items = ApplicationContext.Current.GetListItems(siteSetting, selectedFolder, selectedView, sortedFieldName, isAsc, currentPageIndex, currentListItemCollectionPositionNext, customFilters, true, out listItemCollectionPositionNext, out itemCount);
            ApplicationContext.Current.BindSearchResultsToListViewControl(siteSetting, items, SearchResultDataGridView);
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckSearchButtonEnability();
        }

        private void TemplateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltersButton.IsEnabled = false;
            SearchButton.IsEnabled = false;
            SearchLocationComboBox.IsEnabled = false;
            SearchLocationComboBox.Items.Clear();
            if (TemplateComboBox.SelectedValue as DocumentTemplate != null)
            {
                DocumentTemplateLocationSelectionForm documentTemplateLocationSelectionForm = new DocumentTemplateLocationSelectionForm();

                SelectedDocumentTemplate = (DocumentTemplate)TemplateComboBox.SelectedValue;
                List<DocumentTemplateMapping> mappings = ConfigurationManager.GetInstance().GetDocumentTemplateMappings(SelectedDocumentTemplate.ID);
                if (mappings.Count == 0)
                {
                    MessageBox.Show(Languages.Translate("This template does not have any location mapping"));
                    return;
                }
                
                foreach (DocumentTemplateMapping documentTemplateMapping in mappings)
                {
                    SearchLocationComboBox.Items.Add(documentTemplateMapping);
                }
                SearchLocationComboBox.SelectedIndex = 0;
                if (SearchLocationComboBox.Items.Count > 1)
                {
                    SearchLocationComboBox.IsEnabled = true;
                }
                    /*
                else if (mappings.Count == 1)
                {
                    SelectedDocumentTemplateMapping = mappings[0];
                }
                else
                {
                    documentTemplateLocationSelectionForm.Initialize(mappings);
                    if (documentTemplateLocationSelectionForm.ShowDialog(null, "Select a location") == true)
                    {
                        SelectedDocumentTemplateMapping = documentTemplateLocationSelectionForm.SelectedDocumentTemplateMapping;
                    }
                    else
                    {
                        return;
                    }
                }
                     */ 

                FiltersButton.IsEnabled = true;
            }
            CheckSearchButtonEnability();
        }

        private void SearchLocationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDocumentTemplateMapping = SearchLocationComboBox.SelectedItem as DocumentTemplateMapping;
        }
    }
}
