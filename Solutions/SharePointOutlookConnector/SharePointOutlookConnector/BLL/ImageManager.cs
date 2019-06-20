using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Security.Cryptography;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
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
            ExtensionIcons.Add("dot", "ICDOT.GIF");
            ExtensionIcons.Add("dotx", "ICDOTX.GIF");
            ExtensionIcons.Add("dwp", "ICDWP.GIF");
            ExtensionIcons.Add("dwt", "icdwt.gif");
            ExtensionIcons.Add("eml", "ICEML.GIF");
            ExtensionIcons.Add("gif", "ICGIF.GIF");
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
            ExtensionIcons.Add("pps", "ICPPS.GIF");
            ExtensionIcons.Add("ppt", "ICPPT.GIF");
            ExtensionIcons.Add("tif", "ICTIF.GIF");
            ExtensionIcons.Add("txt", "ICTXT.GIF");
            ExtensionIcons.Add("vst", "ICVST.GIF");
            ExtensionIcons.Add("vsx", "ICVSX.GIF");
            ExtensionIcons.Add("wma", "ICWMA.GIF");
            ExtensionIcons.Add("xlsx", "ICXLSX.GIF");
            ExtensionIcons.Add("zip", "ICZIP.GIF");
            ExtensionIcons.Add("list", "LIST.GIF");
        }
        public Image GetSobiens20X20Image()
        {
            return GetImageFromResource("Sobiens.Office.SharePointOutlookConnector.images.Sobiens_20x20.gif");
        }
        public Image GetFolderImage()
        {
            return GetImageFromResource("Sobiens.Office.SharePointOutlookConnector.images.folder.gif");
        }
        public Image GetWebImage()
        {
            return GetImageFromResource("Sobiens.Office.SharePointOutlookConnector.images.stsicon.gif");
        }
        public Image GetListImage()
        {
            return GetImageFromResource("Sobiens.Office.SharePointOutlookConnector.images.itgen.gif");
        }
        public Image GetImageFromResource(string imageResourcePath)
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
        public Image GetExtensionImageFromResource(string extensionName)
        {
            if (ExtensionIcons[extensionName] == null)
                return GetImageFromResource("Sobiens.Office.SharePointOutlookConnector.images.ExtensionIcons.ICO16.GIF");
            else
                return GetImageFromResource("Sobiens.Office.SharePointOutlookConnector.images.ExtensionIcons." + ExtensionIcons[extensionName]);
        }
    }
}
