using ACDBackend;
using AssettoTools.Core.Helper;
using AssettoTools.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using Microsoft.VisualBasic;
using IniParser.Parser;

namespace AssettoTools.Core.Tools
{
    public class FileExplorer
    {
        public enum FileTypes
        {
            INI,
            LUT,
            OTHER
        }

        private ACDWorker acdWorker = new();
        private IniDataParser iniParser = new();


        public FileObject parseEntry(ACDEntry entry)
        {
            FileObject fileObject = new();

            fileObject.name = entry.name;
            
            fileObject.headerData = iniParser.Parse(entry.fileData);

            return fileObject;
        }

        public List<FileObject> getEntries(string filePath)
        {
            List<FileObject> result = new();

            foreach (ACDEntry entry in acdWorker.getEntries(filePath))
            {
                FileTypes type = parseType(entry.name);
                
                if (type == FileTypes.INI)
                {
                    //Parse INI
                    FileObject parsed = parseEntry(entry);

                    parsed.fileType = type;

                    result.Add(parsed);
                }
                else
                {
                    //Anything else just stuff into fileData.

                    result.Add(new FileObject() { fileData = entry.fileData, fileType = type, name = entry.name });
                }
            }

            return result;
        }

        public FileTypes parseType(string name)
        {
            //Get past the dot.
            FileTypes returnType = FileTypes.OTHER;

            try
            {
                returnType = (FileTypes)Enum.Parse(typeof(FileTypes), name.Trim().Split('.')[1]);
            }
            catch (Exception) { }

            Logger.log("^^^ Don't worry about exceptions, its enum parsing failed.");

            //If it shits itself, just return OTHER.

            return returnType;
        }
    }
}
