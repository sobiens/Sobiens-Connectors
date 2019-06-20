using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities;
using Microsoft.Win32;
using System.Threading;

namespace registryUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string culture=Thread.CurrentThread.CurrentCulture.ToString();
            if (culture!="en-US")
                RenameSubKey(Registry.ClassesRoot, @"*\shell\Send To Office Connector", @"*\shell\" + translate("Send To Office Connector", culture));
            
            
        }

        static string translate(string txt,string culture)
        {
            switch (culture)
            {
                case "en-US":
                    return null;
                    break;
                case "fr-FR":
                    return "Envoyer à Office Connector";
                    break;
                default:
                    return null;
                    break;
            }
        }

       static bool Delete(RegistryKey rk, string KeyName)
        {
            try
            {
                rk.DeleteSubKeyTree(KeyName);

                return true;
            }
            catch (Exception e)
            {
                Console.Write(e + "\nDeleting registry " + KeyName.ToUpper());
                return false;
            }
        }
        static bool RenameSubKey(RegistryKey parentKey,
                   string subKeyName, string newSubKeyName)
        {
            CopyKey(parentKey, subKeyName, newSubKeyName);
            
            parentKey.DeleteSubKeyTree(subKeyName);

            return true;
        }
        
        static bool CopyKey(RegistryKey parentKey,
            string keyNameToCopy, string newKeyName)
        {
            //Create new key
            RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName);

            //Open the sourceKey we are copying from
            RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy);

            RecurseCopyKey(sourceKey, destinationKey);

            return true;
        }
        static void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            //copy all the values
            foreach (string valueName in sourceKey.GetValueNames())
            {
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }

            //For Each subKey 
            //Create a new subKey in destinationKey 
            //Call myself 
            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
            }
        }
    }
}
