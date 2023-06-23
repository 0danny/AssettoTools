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

namespace AssettoTools.Core.Tools
{
    public class FileExplorer
    {
        private ACDWorker acdWorker = new();

        public int getIndexOfItem(string name)
        {
            return MainWindowViewModel.Instance.FileItems.IndexOf(MainWindowViewModel.Instance.FileItems.FirstOrDefault(elem => elem.name == name));
        }

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
