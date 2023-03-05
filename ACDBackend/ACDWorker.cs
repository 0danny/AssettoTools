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

        public void saveEntries(string filePath, List<ACDEntry> entries)
        {
            //Running this again for saveEntries just ensures that we have the write key.
            setupEncryption(filePath);

            if (entries == null)
            {
                return;
            }

            writer.saveACD($"{filePath}\\{dataPrefix}", entries);
        }

        public List<ACDEntry> getEntries(string filePath)
        {
            //If the data folder already exists, just read the files from the folder.
            if(Directory.Exists($"{filePath}{dataFolderPrefix}"))
            {
                Logger($"Data folder for: {filePath} already exists, reading...");

                List<ACDEntry> entries = new();

                foreach (FileInfo file in new DirectoryInfo($"{filePath}{dataFolderPrefix}").GetFiles())
                {
                    entries.Add(new ACDEntry() { name = file.Name, fileData = File.ReadAllText(file.FullName)});
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
                    Logger($"{filePath} contains no ACD or DATA folder, skipping...");

                    return null;
                }
            } 
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

    public class ACDEntry
    {
        public string name { get; set; }
        public string fileData { get; set; }
    }
}
