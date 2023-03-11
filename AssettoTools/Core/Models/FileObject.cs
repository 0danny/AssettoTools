using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AssettoTools.Core.Tools.FileExplorer;

namespace AssettoTools.Core.Models
{
    public class FileObject
    {
        public string name { get; set; }

        public FileTypes fileType { get; set; }

        public string fileData { get; set; }

        public IniData headerData { get; set; }

        public string getFileData()
        {
            if (fileType == FileTypes.INI)
            {
                return headerData.ToString();
            }
            else
            {
                return fileData;
            }
        }
    }
}
