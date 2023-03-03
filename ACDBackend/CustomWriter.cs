using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDBackend
{
    public class CustomWriter
    {
        public void saveACD(string path, List<ACDEntry> entries)
        {
            //Get size of entries array.

            using (MemoryStream stream = new MemoryStream())
            {
                foreach (ACDEntry entry in entries)
                {
                    writeString(entry.name, stream); //Write the name of the file.
                    writeInt(entry.fileData.Length, stream); //Write the size of the file data.

                    //Encrypt bytes and write.
                    byte[] result = new byte[entry.fileData.Length * 4];
                    ACDEncryption.encrypt(entry.fileData, result);

                    //Write to the stream
                    stream.Write(result, 0, result.Length);
                }

                Logger($"Stream has been written, size: {stream.Length}, writing .acd to: {path}");

                File.WriteAllBytes(path, stream.ToArray());

                Logger($"File successfully written.");
            }
        }

        public int getEntriesSize(List<ACDEntry> entries)
        {
            int returnSize = 0;

            foreach (ACDEntry entry in entries)
            {
                returnSize += entry.name.Length + entry.fileData.Length;
            }

            Logger($"Calculated entries size: {returnSize}");

            return returnSize;
        }

        public void writeInt(int value, Stream stream)
        {
            byte[] intByte = BitConverter.GetBytes(value);

            stream.Write(intByte, 0, intByte.Length);
        }

        public void writeString(string value, Stream stream)
        {
            //Write the size of the string first.
            writeInt(value.Length, stream);

            //Get the string bytes
            byte[] stringBytes = Encoding.ASCII.GetBytes(value);

            //Write to the stream
            stream.Write(stringBytes, 0, stringBytes.Length);
        }

        public CustomWriter()
        {
            Logger("CustomWriter created.");
        }

        public void Logger(object data)
        {
            Trace.WriteLine($"[CustomWriter]: {data}");
        }
    }
}
