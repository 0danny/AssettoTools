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

        public void saveEntries(string filePath, List<ACDEntry> entries)
        {
            //Running this again for saveEntries just ensures that we have the write key.
            setupEncryption(filePath);

            if (entries == null)
            {
                return;
            }

            writer.saveACD($"{filePath}\\data.acd", entries);
        }

        public List<ACDEntry> getEntries(string filePath)
        {
            setupEncryption(filePath);

            //Setup reader
            reader.prepareReader(filePath);

            //Read data into array
            return reader.getEntries();
        }

        public void setupEncryption(string filePath)
        {
            //Setup worker
            string folderName = getFolderName(filePath);

            Trace.WriteLine($"Got folder name: {folderName}");

            //Setup encryption, using folder name as encryption key.
            ACDEncryption.setupEncryption(folderName);
        }

        public string getFolderName(string acdFilename)
        {
            var name = Path.GetFileName(acdFilename) ?? "";
            return name.StartsWith("data", StringComparison.OrdinalIgnoreCase) ? Path.GetFileName(Path.GetDirectoryName(acdFilename)) : name;
        }
    }

    public class ACDEntry
    {
        public string name { get; set; }
        public string fileData { get; set; }
    }
}
