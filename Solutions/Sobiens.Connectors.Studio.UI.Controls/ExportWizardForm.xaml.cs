using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Extensions;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Studio.UI.Controls.CodeTemplates.WebAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
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
using System.Xml;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class ExportWizardForm : HostControl
    {
        private Folder _MainObject = null;
        private List<Folder> SelectedFolders = new List<Folder>();
        public ExportWizardForm()
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
            this.OKButtonSelected += ExportWizardForm_OKButtonSelected;
        }

        private void ExportWizardForm_OKButtonSelected(object sender, EventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            string tempPath = dialog.SelectedPath;
            foreach (Folder folder in this.SelectedFolders)
            {
                SiteSetting siteSetting = ApplicationContext.Current.Configuration.SiteSettings[folder.SiteSettingID];
                List<Field> fields = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetFields(siteSetting, folder);
                string fieldsPath = tempPath + "\\" + folder.Title + ".xml";

                SerializationManager.SaveConfiguration<List<Field>>(fields, fieldsPath);
            }

            if (ViewComboBox.SelectedItem == null)
                return;
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


        public void Initialize(Folder mainObject) 
        {
            _MainObject = mainObject;
            ViewComboBox.Items.Clear();

            List<Folder> itemSource = new List<Folder>();
            foreach(Folder folder in _MainObject.Folders)
            {
                if (folder as SPList == null)
                    continue;

                itemSource.Add(folder);
                //SelectedFolders.Add(folder);
            }

            FoldersListView.ItemsSource = itemSource;
        }

        private void FolderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Folder folder = ((CheckBox)e.Source).DataContext as Folder;
            SelectedFolders.Add(folder);
        }

        private void FolderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Folder folder = ((CheckBox)e.Source).DataContext as Folder;
            SelectedFolders.Remove(folder);
        }
    }
}
