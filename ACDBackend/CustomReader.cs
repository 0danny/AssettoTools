using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ACDBackend
{
    public class CustomReader
    {
        /// Author: Danny
        ///
        /// Date: 25/02/2023
        ///
        /// <summary>
        /// Reads the bytes within the array and keeps track of position.
        /// </summary>

        private byte[] data { get; set; }

        private string filePath { get; set; }

        private int position { get; set; } = 0;

        public void prepareReader(string filePath)
        {
            this.filePath = filePath;

            readFile();

            //They do this in content manager, not sure why, but I might aswell do it too lmao
            if (readInt() == -1111)
            {
                readInt();
            }
            else
            {
                resetPosition();
            }
        }

        public void readFile()
        {
            data = File.ReadAllBytes(filePath);

            Logger($"Read in {data.Length} bytes, with path: {filePath}");
        }

        public List<ACDEntry> getEntries()
        {
            List<ACDEntry> entryList = new();

            //Ensure we are not hitting EOF and still trying to read
            while(position < data.Length)
            {
                entryList.Add(new ACDEntry() { name = readString(), fileData = readEncryptedBytes() });
            }

            Logger($"EOF, returning: {entryList.Count} entries.");

            cleanUp();

            return entryList;
        }

        public void cleanUp()
        {
            resetPosition();

            data = null;

            filePath = "";
        }

        public void resetPosition()
        {
            Logger($"Resetting position: {position}");
            
            position = 0;
        }

        public string readEncryptedBytes()
        {
            Logger($"Current position: {position}");

            int length = readInt();

            Logger($"Size of entry: {length}");

            byte[] _buffer = new byte[length];

            for (int i = 0; i < length; i++)
            {
                _buffer[i] = readByte();
                skip(3);
            }

            ACDEncryption.decrypt(_buffer);

            return Encoding.Default.GetString(_buffer);
        }

        public int readInt()
        {
            return BitConverter.ToInt32(readBytes(4), 0);
        }

        public byte readByte()
        {
            byte returnData = data[position];
            addPosition();
            return returnData;
        }

        public byte[] readBytes(int count)
        {
            //Make space
            byte[] _buffer = new byte[count];

            //Checking endianess
            if (BitConverter.IsLittleEndian)
                Array.Reverse(_buffer);

            //Copying the bytes into the buffer
            Array.Copy(data, position, _buffer, 0, count);

            //Add the position to cursor
            addPosition(count);

            return _buffer;
        }


        public string readString()
        {
            //Length of string
            int length = readInt();

            Logger($"Length of string: {length}");

            //Cast to string and return it
            string returnString = Encoding.Default.GetString(readBytes(length));

            Logger($"Return string: {returnString}");

            return returnString;
        }

        public void skip(int amount)
        {
            position += amount;
        }

        public void addPosition(int amount = 1)
        {
            position += amount;
        }

        public void Logger(object data)
        {
            Trace.WriteLine($"[CustomReader]: {data}");
        }
    }
}
