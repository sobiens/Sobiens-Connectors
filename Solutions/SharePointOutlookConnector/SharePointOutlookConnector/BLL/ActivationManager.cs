using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Management;
using System.Collections.Specialized;
using Sobiens.Office.SharePointOutlookConnector.Properties;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class ActivationManager
    {
        public static bool CheckActivation(string activationFile, StringCollection clientIDs)
        {
            foreach (string clientID in clientIDs)
            {
                TextReader fs = new StreamReader(activationFile,new System.Text.UTF8Encoding());
                string keyValue = fs.ReadToEnd();
                if (keyValue != String.Empty)
                {
                    byte[] SignedHashValue = StrToByteArraySpecial(keyValue);
                    /*
                BinaryFormatter F = new BinaryFormatter();
                byte[] SignedHashValue = (byte[])F.Deserialize(Fs);
                Fs.Close();
                    */
                    string publicKeyXML = ImageManager.GetInstance().GetPublicKey();

                    using (DSACryptoServiceProvider DSA = new DSACryptoServiceProvider())
                    {
                        byte[] clientIDBytes = StrToByteArray(clientID);
                        DSA.FromXmlString(publicKeyXML);
                        DSAParameters publicKeyInfo = DSA.ExportParameters(false);
                        bool verified = DSAVerifyHash(clientIDBytes, SignedHashValue, publicKeyInfo, "SHA1");
                        if (verified == true)
                            return true;
                    }
                }
            }
            return false;
        }

        // C# to convert a string to a byte array.
        public static byte[] StrToByteArraySpecial(string str)
        {
            string[] values = str.Split(new char[] { ',' });
            byte[] bytes = new byte[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                bytes[i] = byte.Parse(values[i].ToString());
            }
            return bytes;
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static bool CheckActivation()
        {
            StringCollection clientIDs = GetCurrentClientIDs();
            string applicationFolderPath = EUSettingsManager.GetInstance().GetCommonApplicationFolder() ;
            for (int i = 1; i < 5; i++)
            {
                string signedValueFilePath = applicationFolderPath + "\\SignedValue" + i + ".spoc";
                if (File.Exists(signedValueFilePath) == true)
                {
                    bool verified = CheckActivation(signedValueFilePath, clientIDs);
                    if (verified == true)
                        return true;
                }
            }
            return false;
        }

        public static void SaveGeneratedKeys(StringCollection generatedKeys)
        {
            string applicationFolderPath = EUSettingsManager.GetInstance().GetCommonApplicationFolder();
            for (int i = 0; i < generatedKeys.Count; i++)
            {
                string signedValueFilePath = applicationFolderPath + "\\SignedValue" + (i + 1) + ".spoc";
                TextWriter tw = new StreamWriter(signedValueFilePath);
                tw.Write(generatedKeys[i]);
                tw.Close();
            }
        }

        public static void Activate()
        {
        }

        public static StringCollection GetCurrentClientIDs()
        {
            StringCollection clientIDs = new StringCollection();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    clientIDs.Add("SDX" + mo["MacAddress"].ToString());
                }
                mo.Dispose();
            }
            return clientIDs;
        }

        public static byte[] DSASignHash(byte[] HashToSign, DSAParameters DSAKeyInfo, string HashAlg)
        {
            byte[] sig = null;

            try
            {
                // Create a new instance of DSACryptoServiceProvider.
                using (DSACryptoServiceProvider DSA = new DSACryptoServiceProvider())
                {
                    // Import the key information.
                    DSA.ImportParameters(DSAKeyInfo);

                    // Create an DSASignatureFormatter object and pass it the
                    // DSACryptoServiceProvider to transfer the private key.
                    DSASignatureFormatter DSAFormatter = new DSASignatureFormatter(DSA);

                    // Set the hash algorithm to the passed value.
                    DSAFormatter.SetHashAlgorithm(HashAlg);

                    // Create a signature for HashValue and return it.
                    sig = DSAFormatter.CreateSignature(HashToSign);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }

            return sig;
        }

        public static bool DSAVerifyHash(byte[] HashValue, byte[] SignedHashValue, DSAParameters DSAKeyInfo, string HashAlg)
        {
            bool verified = false;

            try
            {
                // Create a new instance of DSACryptoServiceProvider.
                using (DSACryptoServiceProvider DSA = new DSACryptoServiceProvider())
                {
                    // Import the key information.
                    DSA.ImportParameters(DSAKeyInfo);

                    // Create an DSASignatureDeformatter object and pass it the
                    // DSACryptoServiceProvider to transfer the private key.
                    DSASignatureDeformatter DSADeformatter = new DSASignatureDeformatter(DSA);

                    // Set the hash algorithm to the passed value.
                    DSADeformatter.SetHashAlgorithm(HashAlg);

                    // Verify signature and return the result.
                    verified = DSADeformatter.VerifySignature(HashValue, SignedHashValue);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }

            return verified;
        }

    }
}
