using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ACDBackend
{
    public class ACDWorker
    {
        private CustomReader reader = new();
        private CustomWriter writer = new();

        public string dataPrefix { get; set; } = "\\data.acd";

        public string dataFolderPrefix { get; set; } = "\\data";

        public void saveEntries(string filePath, List<FileObject> entries, bool hasACD)
        {
            if(hasACD)
            {
                //Running this again for saveEntries just ensures that we have the write key.
                setupEncryption(filePath);

                if (entries == null)
                {
                    return;
                }

                writer.saveACD($"{filePath}\\{dataPrefix}", entries);
            }
            else
            {
                Logger("No ACD, writing directly to \\data.");

                //If there is no ACD, just write the files directly to the \\data folder.
                foreach(FileObject obj in entries)
                {
                    File.WriteAllText($"{filePath}\\{dataFolderPrefix}\\{obj.name}", obj.fileData);
                    Logger($"Writing file: {obj.name}");
                }
            }
        }


        /// <summary>
        /// Retrieves the entries from a specified file path.
        /// </summary>
        /// <param name="filePath">The path to the file or directory containing the ACD file or the data folder.</param>
        /// <returns>
        /// A tuple containing a list of ACDEntry objects and a boolean flag indicating how the entry data was collected.
        /// </returns>
        public List<FileObject> getEntries(string filePath)
        {
            //If the data folder already exists, just read the files from the folder.
            if(Directory.Exists($"{filePath}{dataFolderPrefix}"))
            {
                Logger($"Data folder for: {filePath} already exists, reading...");

                List<FileObject> entries = new();

                foreach (FileInfo file in new DirectoryInfo($"{filePath}{dataFolderPrefix}").GetFiles())
                {
                    entries.Add(new FileObject() { 
                        name = file.Name, 
                        fileData = File.ReadAllText(file.FullName),
                        fileType = parseType(file.Name)
                    });
                }

                return entries;
            }
            else
            {
                //Ensure the ACD actually exists before attempting to read it.

                if (File.Exists($"{filePath}{dataPrefix}"))
                {
                    setupEncryption(filePath);

                    //Setup reader
                    reader.prepareReader($"{filePath}{dataPrefix}");

                    //Read data into array
                    return reader.getEntries();
                }
                else
                {
                    Logger($"{filePath} contains no ACD or Data folder, skipping...");

                    return null;
                }
            } 
        }

        public static FileTypes parseType(string name)
        {
            FileTypes returnType = FileTypes.OTHER;

            try
            {
                //Get past the dot.
                returnType = (FileTypes)Enum.Parse(typeof(FileTypes), name.Trim().Split('.')[1], true);
            }
            catch (Exception) { }

            //If it shits itself, just return OTHER.

            return returnType;
        }

        public void setupEncryption(string filePath)
        {
            //Setup worker
            string folderName = getFolderName(filePath);

            Logger($"Got folder name: {folderName}");

            //Setup encryption, using folder name as encryption key.
            ACDEncryption.setupEncryption(folderName);
        }

        public string getFolderName(string acdFilename)
        {
            var name = Path.GetFileName(acdFilename) ?? "";
            return name.StartsWith("data", StringComparison.OrdinalIgnoreCase) ? Path.GetFileName(Path.GetDirectoryName(acdFilename)) : name;
        }

        public void Logger(object data)
        {
            Trace.WriteLine($"[ACDWorker]: {data}");
        }
    }

    public class FileObject
    {
        public string name { get; set; }

        public FileTypes fileType { get; set; }

        public string fileData { get; set; }
    }

    public enum FileTypes
    {
        INI,
        LUT,
        OTHER
    }
}
