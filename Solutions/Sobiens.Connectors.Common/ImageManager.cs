using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common.Extensions;

namespace Sobiens.Connectors.Common
{
    public class ImageManager
    {
        private static ImageManager _instance = null;
        public static ImageManager GetInstance()
        {
            if (_instance == null)
                _instance = new ImageManager();
            return _instance;
        }

        public SC_Images _Images = null;
        public SC_Images Images
        {
            get
            {
                if (_Images == null)
                {
                    _Images = GetImages();
                }

                return _Images;
            }
        }

        private Hashtable ExtensionIcons = new Hashtable();

        public ImageManager()
        {
            InitializeExtensionIcons();
        }
        private void InitializeExtensionIcons()
        {
            ExtensionIcons.Add("accdb", "ICACCDB.GIF");
            ExtensionIcons.Add("asax", "ICASAX.GIF");
            ExtensionIcons.Add("ascx", "ICASCX.GIF");
            ExtensionIcons.Add("asmx", "ICASMX.GIF");
            ExtensionIcons.Add("asp", "ICASP.GIF");
            ExtensionIcons.Add("aspx", "ICASPX.GIF");
            ExtensionIcons.Add("bmp", "ICBMP.GIF");
            ExtensionIcons.Add("config", "ICCONFIG.GIF");
            ExtensionIcons.Add("css", "ICCSS.GIF");
            ExtensionIcons.Add("mdb", "ICDB.GIF");
            ExtensionIcons.Add("dib", "ICDIB.GIF");
            ExtensionIcons.Add("doc", "ICDOC.GIF");
            ExtensionIcons.Add("docx", "ICDOCX.GIF");
            ExtensionIcons.Add("docm", "ICDOCX.GIF");
            ExtensionIcons.Add("dot", "ICDOT.GIF");
            ExtensionIcons.Add("dotx", "ICDOTX.GIF");
            ExtensionIcons.Add("dotm", "ICDOTX.GIF");
            ExtensionIcons.Add("dwp", "ICDWP.GIF");
            ExtensionIcons.Add("dwt", "icdwt.gif");
            ExtensionIcons.Add("eml", "ICEML.GIF");
            ExtensionIcons.Add("gif", "ICGIF.GIF");
            ExtensionIcons.Add("png", "ICGIF.GIF");
            ExtensionIcons.Add("hlp", "ICHLP.GIF");
            ExtensionIcons.Add("htm", "ICHTM.GIF");
            ExtensionIcons.Add("inf", "ICINF.GIF");
            ExtensionIcons.Add("ini", "ICINI.GIF");
            ExtensionIcons.Add("jpg", "ICJPG.GIF");
            ExtensionIcons.Add("master", "ICMASTER.GIF");
            ExtensionIcons.Add("mht", "ICMHT.GIF");
            ExtensionIcons.Add("mhtl", "ICMHTML.GIF");
            ExtensionIcons.Add("msg", "ICMSG.GIF");
            ExtensionIcons.Add("msi", "ICMSI.GIF");
            ExtensionIcons.Add("ocx", "ICOCX.GIF");
            ExtensionIcons.Add("pdf", "ICPDF.GIF");
            ExtensionIcons.Add("pps", "ICPPS.GIF");
            ExtensionIcons.Add("ppt", "ICPPT.GIF");
            ExtensionIcons.Add("pptx", "ICPPT.GIF");
            ExtensionIcons.Add("pptm", "ICPPT.GIF");
            ExtensionIcons.Add("potx", "ICPPT.GIF");
            ExtensionIcons.Add("potm", "ICPPT.GIF");
            ExtensionIcons.Add("tif", "ICTIF.GIF");
            ExtensionIcons.Add("txt", "ICTXT.GIF");
            ExtensionIcons.Add("vst", "ICVST.GIF");
            ExtensionIcons.Add("vsx", "ICVSX.GIF");
            ExtensionIcons.Add("wma", "ICWMA.GIF");
            ExtensionIcons.Add("xls", "ICXLSX.GIF");
            ExtensionIcons.Add("xlsm", "ICXLSX.GIF");
            ExtensionIcons.Add("xltx", "ICXLSX.GIF");
            ExtensionIcons.Add("xltm", "ICXLSX.GIF");
            ExtensionIcons.Add("xlsx", "ICXLSX.GIF");
            ExtensionIcons.Add("zip", "ICZIP.GIF");
            ExtensionIcons.Add("list", "LIST.GIF");
        }
        public string GetSobiens20X20Image()
        {
            return "/Sobiens.Connectors.WPF.Controls;component/Images/Sobiens_20x20.gif";
        }
        public string GetFolderImage()
        {
            return "/Sobiens.Connectors.WPF.Controls;component/Images/SPFOLDER.gif";
        }
        public string GetWebImage()
        {
            return "/Sobiens.Connectors.WPF.Controls;component/Images/SPWeb.gif";
        }
        public string GetListImage()
        {
            return "/Sobiens.Connectors.WPF.Controls;component/Images/SPList.gif";
        }

        public string GetUpFolderImage()
        {
            return "/Sobiens.Connectors.WPF.Controls;component/Images/UPFOLDER.GIF";
        }

        /*
        public string GetImageFromResource(string imageResourcePath)
        {
            Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            Stream file = thisExe.GetManifestResourceStream(imageResourcePath);
            return Image.FromStream(file);
        }
        public string GetPublicKey()
        {
            Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            Stream stream = thisExe.GetManifestResourceStream("Sobiens.Office.SharePointOutlookConnector.Resources.SPOCPublic.snk");
            StreamReader sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }
         */ 

        public string GetExtensionImageFromResource(string extensionName,bool isExtracted)
        {
            if (ExtensionIcons[extensionName] == null)
                return "/Sobiens.Connectors.WPF.Controls;component/Images/ExtensionIcons/ICO16.GIF";
            else
                if (isExtracted)
                    return "/Sobiens.Connectors.WPF.Controls;component/Images/ExtensionIconsExtracted/" + ExtensionIcons[extensionName];
                else return "/Sobiens.Connectors.WPF.Controls;component/Images/ExtensionIcons/" + ExtensionIcons[extensionName];
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public SC_Images GetImages()
        {
            string settingFilePath = GetImagesFilePath();
            if (File.Exists(settingFilePath) == false)
                return new SC_Images();
            else
                return SerializationManager.ReadSettings<SC_Images>(settingFilePath);
        }

        public bool AddImage(string imagePath, string category, out string errorMessage)
        {
            try
            {
                string folder = GetImagesFolder();
                FileInfo fileInfo = new FileInfo(imagePath);
                string newImageName = "img_" + Guid.NewGuid().ToString().Replace("-", string.Empty) + fileInfo.Extension;
                string newImagePath = folder + "\\" + newImageName;
                File.Copy(imagePath, newImagePath);
                SC_Image img = new SC_Image();
                img.ID = Guid.NewGuid();
                img.Category = category;
                img.Name = fileInfo.Name;
                img.ImagePath = newImagePath;
                lock (this.Images)
                {
                    this.Images.Add(img);
                }
                SaveConfiguration(this.Images);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool DeleteImage(Guid id, out string errorMessage)
        {
            try
            {
                SC_Image img = this.Images.Single(t => t.ID.Equals(id));
                File.Delete(img.ImagePath);
                lock (this.Images)
                {
                    this.Images.Remove(img);
                }

                SaveConfiguration(this.Images);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public void SaveConfiguration(SC_Images documentTemplates)
        {
            string settingFilePath = GetImagesFilePath();
            SerializationManager.SaveConfiguration<SC_Images>(documentTemplates, settingFilePath);
            _Images = null;
        }

        private string GetImagesFolder()
        {
            string folder = ConfigurationManager.GetInstance().GetConfigurationsFolder() + "\\Images";
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        private string GetImagesFilePath()
        {
            return ConfigurationManager.GetInstance().GetConfigurationsFolder() + "\\AppImages.xml";
        }

        private string GetAdministrativeImagesFilePath()
        {
            return ConfigurationManager.GetInstance().GetConfigurationsFolder() + "\\AppAdministrativeImages.xml";
        }

    }
}
