using ACDBackend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssettoTools.Core.Interfaces
{
    public interface IFileExplorer
    {
        public void saveEntries(string filePath, List<FileObject> objects, bool hasACD);
        public ObservableCollection<FileObject> getEntries(string filePath);
    }
}
