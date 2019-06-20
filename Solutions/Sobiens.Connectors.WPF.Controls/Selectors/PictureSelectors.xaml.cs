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
using Sobiens.Connectors.Entities;
using Microsoft.Win32;
using Sobiens.Connectors.Common;

namespace Sobiens.Connectors.WPF.Controls.Selectors
{
    /// <summary>
    /// Interaction logic for PictureSelectors.xaml
    /// </summary>
    public partial class PictureSelectors : HostControl
    {
        public SC_Image SelectedImage { get; private set; } 
        public PictureSelectors()
        {
            InitializeComponent();
            OKButtonSelected += new EventHandler(PictureSelectors_OKButtonSelected);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            PicturesListBox.ItemsSource = ImageManager.GetInstance().Images;
        }

        void PictureSelectors_OKButtonSelected(object sender, EventArgs e)
        {
            if (PicturesListBox.SelectedValue == null)
            {
                MessageBox.Show(Languages.Translate("Please select an image"));
                return;
            }

            Guid selectedID = (Guid)PicturesListBox.SelectedValue;
            this.SelectedImage = ImageManager.GetInstance().Images.GetImageByID(selectedID);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog(this.ParentWindow) == true)
            {
                string errorMessage = string.Empty;
                if (ImageManager.GetInstance().AddImage(openFileDialog.FileName, Languages.Translate("Template Image"), out errorMessage) == true)
                {
                    PicturesListBox.ItemsSource = ImageManager.GetInstance().Images;
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage;
            Guid selectedID = (Guid)PicturesListBox.SelectedValue;

            if (ImageManager.GetInstance().DeleteImage(selectedID, out errorMessage) == true)
            {
                PicturesListBox.ItemsSource = ImageManager.GetInstance().Images;
            }
            else
            {
                MessageBox.Show(errorMessage);
            }
        }
    }
}
