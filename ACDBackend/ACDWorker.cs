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
        public List<ACDEntry> getEntries(string _fileName)
        {
            //Setup worker
            string folderName = getFileName(_fileName);

            Trace.WriteLine($"Got folder name: {folderName}");

            //Setup encryption, using folder name as encryption key.
            ACDEncryption.setupEncryption(folderName);

            //Setup reader
            CustomReader reader = new CustomReader(_fileName);
            //Read data into array
            return reader.getEntries();
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
