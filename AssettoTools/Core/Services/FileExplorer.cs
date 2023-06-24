using ACDBackend;
using AssettoTools.Core.Helper;
using AssettoTools.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using AssettoTools.ViewModels;
using System.Collections.ObjectModel;
using AssettoTools.Core.Interfaces;

namespace AssettoTools.Core.Services
{
    public class FileExplorer : IFileExplorer
    {
        private ACDWorker acdWorker = new();

        public void saveEntries(string filePath, List<FileObject> objects, bool hasACD)
        {
            acdWorker.saveEntries(filePath, objects, hasACD);
        }

        public ObservableCollection<FileObject> getEntries(string filePath)
        {
            return new ObservableCollection<FileObject>(acdWorker.getEntries(filePath));
        }
    }
}
