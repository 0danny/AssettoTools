using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDBackend
{
    public class ACDWorker
    {
        private CustomReader reader { get; set; }

        private List<ACDEntry> entryCache = new List<ACDEntry>();

        private string fileName { get; set; }
        private string folderName { get; set; }

        public ACDWorker(string _fileName)
        {
            //Setup worker
            fileName = _fileName;

            folderName = getFileName(fileName);

            Console.WriteLine($"Got folder name: {folderName}");

            //Setup encryption, using folder name as encryption key.
            ACDEncryption.setupEncryption(folderName);

            //Setup reader
            reader = new CustomReader(fileName);
            
            //Read data into array
            entryCache = reader.getEntries();

            //Print out the data
            foreach(ACDEntry entry in entryCache)
            {
                Console.WriteLine($"Filename: {entry.name}, Data: {entry.fileData}");
            }
        }

        public string getFileName(string acdFilename)
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
