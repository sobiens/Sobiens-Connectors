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
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.WPF.Controls.Selectors
{
    /// <summary>
    /// Interaction logic for PictureSelectors.xaml
    /// </summary>
    public partial class PictureSelection : BaseUserControl
    {
        public PictureSelection()
        {
            InitializeComponent();
        }

        public Guid ImageID { get; private set; }
        public void Initialize(Guid imageID)
        {
            ImageID = imageID;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            if (ImageID != null && ImageID != Guid.Empty)
            {
                SC_Image img = ImageManager.GetInstance().Images.GetImageByID(ImageID);
                var uriSource = new Uri(img.ImagePath, UriKind.Absolute);
                image1.Source = new BitmapImage(uriSource);
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            PictureSelectors pictureSelectors = new PictureSelectors();
            if (pictureSelectors.ShowDialog(null,Languages.Translate("Picture Selection")) == true)
            {
                var uriSource = new Uri(pictureSelectors.SelectedImage.ImagePath, UriKind.Absolute);
                image1.Source = new BitmapImage(uriSource);
                ImageID = pictureSelectors.SelectedImage.ID;
            }

        }
    }
}
