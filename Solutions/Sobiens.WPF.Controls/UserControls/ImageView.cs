using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Sobiens.WPF.Controls.UserControls
{
    /// <summary>
    /// ImageView displays image files using themselves as their icons.
    /// In order to write our own visual tree of a view, we should override its
    /// DefaultStyleKey and ItemContainerDefaultKey. DefaultStyleKey specifies
    /// the style key of ListView; ItemContainerDefaultKey specifies the style
    /// key of ListViewItem.
    /// </summary>
    public class ImageView : ViewBase
    {
        #region DefaultStyleKey

        protected override object DefaultStyleKey
        {
            get { return new ComponentResourceKey(GetType(), "ImageView"); }
        }

        #endregion

        #region ItemContainerDefaultStyleKey

        protected override object ItemContainerDefaultStyleKey
        {
            get { return new ComponentResourceKey(GetType(), "ImageViewItem"); }
        }

        #endregion
    }

    /// <summary>
    /// FileCollection is used as ItemsSource of ListView. ListView will show files inside
    /// the collection
    /// </summary>
    public sealed class FileCollection : ObservableCollection<FileSystemInfo>
    {
        public FileCollection()
        {
            /*
            // put files in %windir%\web\wallpaper into the collection

            // get %windir%
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            if (string.IsNullOrEmpty(windir))
                return;

            // put files into collection
            string wallpaperPath = string.Format(@"{0}\Web\Wallpaper\Architecture", windir);
            DirectoryInfo info = new DirectoryInfo(wallpaperPath);
            FileSystemInfo[] files = info.GetFileSystemInfos();
            foreach (FileSystemInfo fi in files)
            {
                // exlude hidden files 
                if ((fi.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    Add(fi);
            }
             */ 
        }
    }
}