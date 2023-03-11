using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssettoTools.Core.Models
{
    public class CarObject
    {
        public string carName { get; set; }
        public string folderName { get; set; }
        public string fullPath { get; set; }
        public string[] previewImages { get; set; }
    }
}
