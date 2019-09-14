using Sobiens.Connectors.Entities;
using System;
using System.Collections.Generic;
using System.Windows;

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
