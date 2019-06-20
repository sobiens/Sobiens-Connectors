using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.WPF.Controls.Classes
{
    public class ImageViewItem
    {
        public ImageViewItem(string title, string description, string fullName)
        {
            Title = title;
            Description = description;
            FullName = fullName;
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
    }
}
