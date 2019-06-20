using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;

namespace Sobiens.Connectors.Common
{
    public class StorageUtil
    {
#if General
        public static void SaveObject(string directoryName, string fileName, object savedObject, Action errorCallback)
        {
            if (savedObject == null) return;
            try
            {
                IsolatedStorageFile savegameStorage = null;
#if WINDOWS
                    savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
                if (!savegameStorage.DirectoryExists(directoryName))
                {
                    savegameStorage.CreateDirectory(directoryName);
                }
#else
                savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

                var path = Path.Combine(directoryName, fileName);


                // Specify the file path and options.
                using (var isoFileStream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, savegameStorage))
                {
                    //Write the data
                    using (var isoFileWriter = new StreamWriter(isoFileStream))
                    {
                        // Open the file, creating it if necessary
                        StringBuilder sb = new StringBuilder();
                        StringWriter sw = new StringWriter(sb);

                        // Convert the object to XML data and put it in the stream
                        XmlSerializer serializer = new XmlSerializer(savedObject.GetType());
                        serializer.Serialize(sw, savedObject);

                        isoFileWriter.Write(sb.ToString());
                        isoFileWriter.Close();
                    }
                }
            }
            catch (IsolatedStorageException)
            {
                errorCallback();
            }
        }

        public static T GetObject<T>(string directoryName, string fileName, Action errorCallback)
        {
            try
            {
                IsolatedStorageFile savegameStorage = null;
#if WINDOWS
                    savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();

                var path = Path.Combine(directoryName, fileName);

                if (savegameStorage.FileExists(path) == false)
                {
                    return default(T);
                }


                // Specify the file path and options.
                using (IsolatedStorageFileStream isoFileStream = savegameStorage.OpenFile(path, FileMode.Open))
                {
                    StringBuilder sb = new StringBuilder();
                    byte[] saveBytes = new byte[256];
                    int count = isoFileStream.Read(saveBytes, 0, 256);
                    while (count > 0)
                    {
                        string data = System.Text.Encoding.UTF8.GetString(saveBytes, 0, count);
                        sb.Append(data);
                        count = isoFileStream.Read(saveBytes, 0, 256);
                    }
                    StringReader sr = new StringReader(sb.ToString());
                    isoFileStream.Close();

                    // Convert the object to XML data and put it in the stream
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(sr);
                }

#else
                savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

                return default(T);

            }
            catch (IsolatedStorageException)
            {
                errorCallback();
                return default(T);
            }
        }
        #endif
    }
}
