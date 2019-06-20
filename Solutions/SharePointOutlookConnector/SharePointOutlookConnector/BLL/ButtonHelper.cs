using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using OfficeCore = Microsoft.Office.Core;
using System.Windows.Forms;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class ButtonHelper : System.Windows.Forms.AxHost 
    {
        private static ButtonHelper _instance = null;
        public static ButtonHelper GetInstance()
        {
            if (_instance == null)
                _instance = new ButtonHelper();
            return _instance;
        }
        /// <summary>
        /// Overloaded constructor without clsid
        /// </summary>
        public ButtonHelper() : base(Guid.Empty.ToString())
        {
        }


        /// <summary>
        /// This function sets the Picture and Mask  for Office Buttons
        /// it can be used in for all Office Applications not only for Outlook.
        /// Created in VS2005 with VSTO Tools, but works also with VS2003 without VSTO.
        /// </summary>
        /// <example>
        /// ButtonHelper buttonHelper = new ButtonHelper();
        /// buttonHelper.SetButtonPicture( ref _button, "X4UButton" );
        /// </example>
        /// this sample applies the embedded Bitmap 
        /// "X4UButtonPicture.bmp"
        /// and the Bitmap 
        /// "X4UButtonMask.bmp"
        /// to the button given by reference.
        /// <param name="button">the office button by reference</param>
        /// <param name="pictureName">the name of the embedded Picture</param>
        public void SetButtonPicture(ref OfficeCore.CommandBarButton button, string pictureName)
        {
            try
            {
                // define the ButtonPicture and MASK as IPictureDisp
                stdole.IPictureDisp buttonFace = null;
                stdole.IPictureDisp buttonMask = null;

                // get the current assembly
                Assembly assembly = Assembly.GetExecutingAssembly();

                // load the embedded Bitmaps from assembly
                // the bitmaps should be 2 32 x 32 Bitmaps
                // described in MSDN article: http://support.microsoft.com/kb/286460/en-us
                Stream pictureStream = assembly.GetManifestResourceStream("X4UTools.Images." + pictureName + ".bmp");
                Stream maskStream = assembly.GetManifestResourceStream("X4UTools.Images.Mask.bmp");

                // Convert the Resourcestream to an Image and 
                // convert the Images to stdole.IPictureDisp 
                buttonFace = ImageToIPictureDisp(Image.FromStream(pictureStream));
                buttonMask = ImageToIPictureDisp(Image.FromStream(maskStream));

                // apply the Face and Mask to the Button.
                // here is a BUG when setting this Picture in an Outlook MailItem Inspector
                // set the Picture before mask...
                button.Picture = buttonFace;
                button.Mask = buttonMask;

            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// This function sets the Picture and Mask  for Office Buttons
        /// it can be used in for all Office Applications not only for Outlook.
        /// Created in VS2005 with VSTO Tools, but works also with VS2003 without VSTO.
        /// </summary>
        /// <example>
        /// ButtonHelper buttonHelper = new ButtonHelper();
        /// buttonHelper.SetButtonPicture( ref _button, "X4UButton" );
        /// </example>
        /// this sample applies the embedded Bitmap 
        /// "X4UButtonPicture.bmp"
        /// and the Bitmap 
        /// "X4UButtonMask.bmp"
        /// to the button given by reference.
        /// <param name="button">the office button by reference</param>
        /// <param name="pictureName">the name of the embedded Picture</param>
        public void SetButtonPicture(ref OfficeCore.CommandBarButton button, Image pictureImage, Image maskImage)
        {
            try
            {
                if (pictureImage != null)
                {
                    button.Picture = ImageToIPictureDisp(pictureImage);
                }
                if (maskImage != null)
                {
                    button.Mask = ImageToIPictureDisp(maskImage);
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }



        /// <summary>
        /// This function sets the Picture and Mask  for Office Buttons
        /// it can be used in for all Office Applications not only for Outlook.
        /// Created in VS2005 with VSTO Tools, but works also with VS2003 without VSTO.
        /// </summary>
        /// <example>
        /// ButtonHelper buttonHelper = new ButtonHelper();
        /// buttonHelper.SetButtonPicture( ref _button, "X4UButton" );
        /// </example>
        /// this sample applies the embedded Bitmap 
        /// "X4UButtonPicture.bmp"
        /// and the Bitmap 
        /// "X4UButtonMask.bmp"
        /// to the button given by reference.
        /// <param name="button">the office button by reference</param>
        /// <param name="pictureName">the name of the embedded Picture</param>
        public void SetButtonPictureFromFile(ref OfficeCore.CommandBarButton button, string pictureFile, string maskFile)
        {
            try
            {

                // define the ButtonPicture and MASK as IPictureDisp
                stdole.IPictureDisp buttonFace = null;
                stdole.IPictureDisp buttonMask = null;

                // Load the Bitmaps 
                // convert the Images to stdole.IPictureDisp 
                buttonFace = ImageToIPictureDisp(Image.FromFile(pictureFile));
                buttonMask = ImageToIPictureDisp(Image.FromFile(maskFile));

                // apply the Face and Mask to the Button.
                // here is a BUG when setting this Picture in an Outlook MailItem Inspector
                // set the Picture before mask...
                button.Picture = buttonFace;
                button.Mask = buttonMask;

            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// This function converts an image to an IPictureDisp
        /// </summary>
        /// <param name="image">The Image to convert</param>
        /// <returns>the stdole.IPictureDisp object</returns>
        stdole.IPictureDisp ImageToIPictureDisp(System.Drawing.Image image)
        {
            return (stdole.IPictureDisp)AxHost.GetIPictureDispFromPicture(image);
        }

        public OfficeCore.CommandBarButton CreateButton(string buttonName, string tag, ref OfficeCore.CommandBar commandBar, bool beginGroup)
        {
            object missing = System.Reflection.Missing.Value;

            // Create Button
            OfficeCore.CommandBarButton button = (OfficeCore.CommandBarButton)commandBar.Controls.Add(OfficeCore.MsoControlType.msoControlButton, 1, missing, missing, 1);

            button.Caption = buttonName;
            button.Tag = tag;
            button.Style = OfficeCore.MsoButtonStyle.msoButtonIconAndCaption;
            button.BeginGroup = beginGroup;

            SetButtonPicture(ref button, buttonName);

            return button;

        }
    }
}
