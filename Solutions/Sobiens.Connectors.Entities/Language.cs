using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.IO;

namespace Sobiens.Connectors.Entities
{
    public static class Languages
    {
        private static ResourceDictionary dict_;

        public static ResourceDictionary Dict
        {
            get
            {
                if (dict_ == null) GetLanguageDictionary();
                return dict_;
            }
            set
            {
                Dict = value;
            }
        }

        public static string Translate(string sentence)
        {
            string result=null;

            try
            {
                result = (string)Dict[sentence];
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString(), "Translation");
                result = sentence;
            }

            return result;

        }

        private static ResourceDictionary GetLanguageDictionary()
        {
            try
            {

                //Get the assembly information
                //System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();
                //Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
                //string path = Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());
                dict_ = new ResourceDictionary();

                switch (Thread.CurrentThread.CurrentCulture.ToString())
                {
                    case "en-US":
                        dict_.Source = new System.Uri("pack://application:,,,/Sobiens.Connectors.Entities;component/Resources/Languages/en-US.xaml",
                                      UriKind.Absolute);
                        break;
                    case "fr-FR":
                        dict_.Source = new System.Uri("pack://application:,,,/Sobiens.Connectors.Entities;component/Resources/Languages/fr-FR.xaml",
                                      UriKind.Absolute);
                        break;
                    default:
                        dict_.Source = new System.Uri("pack://application:,,,/Sobiens.Connectors.Entities;component/Resources/Languages/en-US.xaml",
                                      UriKind.Absolute);
                        break;
                }
                return dict_;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }
    }
}
