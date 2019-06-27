﻿using Sobiens.Connectors.Common;
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
    public partial class MMSCopyWizardForm : HostControl
    {
        private int CurrentTabIndex = 0;
        private SPWeb SourceWeb = null;
        private SPWeb DestinationWeb = null;
        //private ISiteSetting DestinationSiteSetting = null;

        public MMSCopyWizardForm()
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

        private void SourceSelectButton_Click(object sender, RoutedEventArgs e)
        {
            //SyncTask s = new SyncTask();
            //s.SourceQueryResultMapping.Mappings[0].QueryResult.
            SelectEntityForm selectEntityForm = new SelectEntityForm();
            selectEntityForm.Initialize(new Type[] { typeof(SPWeb) });
            //HostControl hc = ((HostControl)this.Parent);

            if (selectEntityForm.ShowDialog(null, "Select an object to sync from") == true)
            {
                SourceWeb = (SPWeb)selectEntityForm.SelectedObject;
                ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(SourceWeb.SiteSettingID);
                
                SourceSelectButton.Content = SourceWeb.GetPath();
                SourceTermObjectSelectorControl.Initialize(siteSetting);
            }
        }
        public void Initialize()
        {
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
            CurrentTabIndex = 1;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
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


        private void DestinationSelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectEntityForm selectEntityForm = new SelectEntityForm();
            selectEntityForm.Initialize(new Type[] { typeof(SPWeb) });
            if (selectEntityForm.ShowDialog(this.ParentWindow, "Select an object to sync to") == true)
            {
                DestinationWeb = (SPWeb)selectEntityForm.SelectedObject;
                ISiteSetting siteSetting = ApplicationContext.Current.GetSiteSetting(DestinationWeb.SiteSettingID);
                DestinationSelectButton.Content = DestinationWeb.GetPath();
                DestinationTermObjectSelectorControl.Initialize(siteSetting);
            }
        }


        private void HostControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DestinationNextButton_Click(object sender, RoutedEventArgs e)
        {
            object sourceObject = SourceTermObjectSelectorControl.SelectedObject;
            object destinationObject = DestinationTermObjectSelectorControl.SelectedObject;

            if (sourceObject == null)
            {
                MessageBox.Show("Please select a term from source list");
                return;
            }

            if (destinationObject == null)
            {
                MessageBox.Show("Please select a term from destination list");
                return;
            }

            if(sourceObject.GetType() != destinationObject.GetType())
            {
                MessageBox.Show("Please select the same object type from source and destiny");
                return;
            }

            try
            {
                if (sourceObject as SPTermGroup != null)
                {
                    SPTermGroup sourceTermGroup = sourceObject as SPTermGroup;
                    SPTermGroup destinationTermGroup = destinationObject as SPTermGroup;
                    ISiteSetting sourceSiteSetting = ApplicationContext.Current.GetSiteSetting(SourceWeb.SiteSettingID);
                    ISiteSetting destinationSiteSetting = ApplicationContext.Current.GetSiteSetting(DestinationWeb.SiteSettingID);

                    SynchronizeTermGroup(sourceSiteSetting, sourceTermGroup, destinationSiteSetting, destinationTermGroup);
                    MessageBox.Show("Completed");
                }
                else if (sourceObject as SPTermSet != null)
                {
                    SPTermSet sourceTermSet = sourceObject as SPTermSet;
                    SPTermSet destinationTermSet = destinationObject as SPTermSet;
                    ISiteSetting sourceSiteSetting = ApplicationContext.Current.GetSiteSetting(SourceWeb.SiteSettingID);
                    ISiteSetting destinationSiteSetting = ApplicationContext.Current.GetSiteSetting(DestinationWeb.SiteSettingID);

                    SynchronizeTermSet(sourceSiteSetting, sourceTermSet, destinationSiteSetting, destinationTermSet);
                    MessageBox.Show("Completed");
                }
                else if (sourceObject as SPTerm != null)
                {
                    SPTerm sourceTerm = sourceObject as SPTerm;
                    SPTerm destinationTerm = destinationObject as SPTerm;
                    ISiteSetting sourceSiteSetting = ApplicationContext.Current.GetSiteSetting(SourceWeb.SiteSettingID);
                    ISiteSetting destinationSiteSetting = ApplicationContext.Current.GetSiteSetting(DestinationWeb.SiteSettingID);

                    SynchronizeTerm(sourceSiteSetting, sourceTerm, destinationSiteSetting, destinationTerm);
                    MessageBox.Show("Completed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            CurrentTabIndex = 2;
            WizardTabControl.SelectedIndex = CurrentTabIndex;

        }

        private void SynchronizeTermGroup(ISiteSetting sourceSiteSetting, SPTermGroup sourceTermGroup, ISiteSetting destinationSiteSetting, SPTermGroup destinationTermGroup)
        {
            List<SPTermSet> sourceTermSets = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType).GetGroupTermSets(sourceSiteSetting, sourceTermGroup.ID);
            List<SPTermSet> destinationTermSets = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType).GetGroupTermSets(destinationSiteSetting, destinationTermGroup.ID);

            foreach(SPTermSet termSetToSynch in sourceTermSets)
            {
                SPTermSet termSetFound = destinationTermSets.Where(t => t.Title.Equals(termSetToSynch.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if(termSetFound == null)
                {
                    // Create termset
                    termSetFound = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType).CreateTermSet(destinationSiteSetting, termSetToSynch);
                }

                SynchronizeTermSet(sourceSiteSetting, termSetToSynch, destinationSiteSetting, termSetFound);
            }

        }

        private void SynchronizeTermSet(ISiteSetting sourceSiteSetting, SPTermSet sourceTermSet, ISiteSetting destinationSiteSetting, SPTermSet destinationTermSet)
        {
            List<SPTerm> sourceTerms = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType).GetTerms(sourceSiteSetting, sourceTermSet.ID);
            List<SPTerm> destinationTerms = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType).GetTerms(destinationSiteSetting, destinationTermSet.ID);

            foreach (SPTerm termToSynch in sourceTerms)
            {
                SPTerm termFound = destinationTerms.Where(t => t.Title.Equals(termToSynch.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (termFound == null)
                {
                    termFound = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType).CreateTerm(destinationSiteSetting, termToSynch);
                }

                SynchronizeTerm(sourceSiteSetting, termToSynch, destinationSiteSetting, termFound);
            }

        }

        private void SynchronizeTerm(ISiteSetting sourceSiteSetting, SPTerm sourceTerm, ISiteSetting destinationSiteSetting, SPTerm destinationTerm)
        {
            List<SPTerm> sourceTerms = ServiceManagerFactory.GetServiceManager(sourceSiteSetting.SiteSettingType).GetTermTerms(sourceSiteSetting, sourceTerm.ID);
            List<SPTerm> destinationTerms = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType).GetTermTerms(destinationSiteSetting, destinationTerm.ID);

            foreach (SPTerm termToSynch in sourceTerms)
            {
                SPTerm termFound = destinationTerms.Where(t => t.Title.Equals(termToSynch.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (termFound == null)
                {
                    // Create term
                    termFound = ServiceManagerFactory.GetServiceManager(destinationSiteSetting.SiteSettingType).CreateTerm(destinationSiteSetting, termToSynch);
                }

                SynchronizeTerm(sourceSiteSetting, termToSynch, destinationSiteSetting, termFound);
            }
        }

        private void DestinationBackButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTabIndex = 0;
            WizardTabControl.SelectedIndex = CurrentTabIndex;
        }
    }
}
