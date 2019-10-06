using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using System.Collections.Generic;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Extensions;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Studio.UI.Controls
{

    public class ClosableBrowserTab : TabItem
    {
        public Guid ID { get; set; }
        public string URL { get; set; }
        public WebBrowser Browser { get; set; }


        // Constructor
        public ClosableBrowserTab()
        {
            // Create an instance of the usercontrol
            CloseableHeader closableTabHeader = new CloseableHeader();

            // Assign the usercontrol to the tab header
            this.Header = closableTabHeader;

            // Attach to the CloseableHeader events (Mouse Enter/Leave, Button Click, and Label resize)
            closableTabHeader.button_close.MouseEnter += new MouseEventHandler(button_close_MouseEnter);
            closableTabHeader.button_close.MouseLeave += new MouseEventHandler(button_close_MouseLeave);
            closableTabHeader.button_close.Click += new RoutedEventHandler(button_close_Click);
            closableTabHeader.label_TabTitle.SizeChanged += new SizeChangedEventHandler(label_TabTitle_SizeChanged);
            this.Browser = new WebBrowser();
            this.Browser.Name = "myBrowser";
            //wb.Margin =  "0,0,0,20";
            //wb.Cursor = "Arrow";
            //wb.HorizontalScrollBarVisibility = "Auto" ScrollViewer.VerticalScrollBarVisibility = "Auto" />

            this.Content = this.Browser;
        }




        /// <summary>
        /// Property - Set the Title of the Tab
        /// </summary>
        public string Title
        {
            set
            {
                ((CloseableHeader)this.Header).label_TabTitle.Content = value;
            }
        }


        public void Initialize(string title, string url)
        {
            Title = title;
            URL = url;
            this.Browser.Navigate(this.URL);
        }



        //
        // - - - Overrides  - - -
        //


        // Override OnSelected - Show the Close Button
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnUnSelected - Hide the Close Button
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
        }

        // Override OnMouseEnter - Show the Close Button
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseLeave - Hide the Close Button (If it is NOT selected)
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
            {
                ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
            }
        }





        //
        // - - - Event Handlers  - - -
        //


        // Button MouseEnter - When the mouse is over the button - change color to Red
        void button_close_MouseEnter(object sender, MouseEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Foreground = Brushes.Red;
        }

        // Button MouseLeave - When mouse is no longer over button - change color back to black
        void button_close_MouseLeave(object sender, MouseEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Foreground = Brushes.Black;
        }


        // Button Close Click - Remove the Tab - (or raise an event indicating a "CloseTab" event has occurred)
        void button_close_Click(object sender, RoutedEventArgs e)
        {
            ((TabControl)this.Parent).Items.Remove(this);
        }


        // Label SizeChanged - When the Size of the Label changes (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Margin = new Thickness(((CloseableHeader)this.Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }




    }
}