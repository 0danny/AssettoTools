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
using AssettoTools.ViewModels;
using System.Collections.ObjectModel;

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

        public int getFileIndex(string name)
        {
            return MainWindowViewModel.Instance.FileItems.IndexOf(MainWindowViewModel.Instance.FileItems.Where(elem => elem.name == name).First());
        }

        public void reparseINIs()
        {
            foreach(FileObject file in MainWindowViewModel.Instance.FileItems)
            {
                if(file.fileType == FileTypes.INI)
                {
                    file.headerData = parseINI(file.fileData);
                }
            }
        }

        public IniData parseINI(string fileData)
        {
            try
            {
                return iniParser.Parse(fileData);
            }
            catch (Exception ex)
            {
                Utilities.showMessageBox($"There was an error parsing file, Error message: {ex.Message}");

                return null;
            }
        }

        public void saveEntries(string filePath, List<FileObject> objects)
        {
            //Need to convert FileObject to ACDEntry list to save.
            List<ACDEntry> converted = new();

            foreach(FileObject file in objects)
            {
                converted.Add(new ACDEntry() { fileData = file.fileData, name = file.name });
            }

            acdWorker.saveEntries(filePath, converted);
        }

        public List<FileObject> getEntries(string filePath)
        {
            List<FileObject> result = new();

            foreach (ACDEntry entry in acdWorker.getEntries(filePath))
            {
                FileTypes type = parseType(entry.name);

                FileObject returnObject = new();

                returnObject.name = entry.name;
                returnObject.fileType = type;
                returnObject.fileData = cleanFileData(entry.fileData);

                //Logger.log($"{returnObject.name} | Cleaned: {returnObject.fileData}");

                //Remove the // from the file, because it breaks the parsing.

                if (type == FileTypes.INI)
                {
                    //Parse INI
                    returnObject.headerData = parseINI(returnObject.fileData);
                }

                result.Add(returnObject);
            }

            return result;
        }

        public string cleanFileData(string data)
        {
            string[] lines = data.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > 0)
                {
                    if (lines[i].Trim()[0] == '/')
                    {
                        Logger.log($"Removing: {lines[i]}");
                        lines[i] = "";
                    }
                }
            }

            return string.Join('\n', lines);
        }

        public FileTypes parseType(string name)
        {
            //Get past the dot.
            FileTypes returnType = FileTypes.OTHER;

            try
            {
                returnType = (FileTypes)Enum.Parse(typeof(FileTypes), name.Trim().Split('.')[1], true);
            }
            catch (Exception) { }

            Logger.log("^^^ Don't worry about exceptions, its enum parsing failed.");

            //If it shits itself, just return OTHER.

            return returnType;
        }
    }
}
