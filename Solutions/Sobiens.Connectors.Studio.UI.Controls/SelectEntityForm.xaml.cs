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
using System.ComponentModel;
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
using System.Windows.Threading;
using System.Xml;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeWizardForm.xaml
    /// </summary>
    public partial class SelectEntityForm : HostControl
    {
        public Type[] AllowedTypes;
        public Folder SelectedObject;
        public SelectEntityForm()
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
            List<Folder> selectedSourceFolders = SourceDataFoldersSelector.SelectedObjects;
            if (selectedSourceFolders.Count == 0)
            {
                MessageBox.Show("Please select a list object for source");
                return;
            }

            bool allowed = false;
            foreach (Type type in AllowedTypes)
            {
                if (selectedSourceFolders[0].GetType() == type)
                {
                    this.SelectedObject = selectedSourceFolders[0];
                    allowed = true;
                    break;
                }
            }

            if (allowed == false)
            {
                MessageBox.Show("Please select an allowed object");
                return;
            }

            this.Close(true);

        }

        public void Initialize(Type[] allowedTypes)
        {
            this.AllowedTypes = allowedTypes;
            SourceDataFoldersSelector.Initialize();
        }
    }
}
