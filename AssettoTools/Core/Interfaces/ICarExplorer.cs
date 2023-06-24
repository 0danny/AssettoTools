using AssettoTools.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssettoTools.Core.Interfaces
{
    public interface ICarExplorer
    {
        public Task<ObservableCollection<CarObject>> populateList(string filePath);
        public CarObject parseFolder(string carFolder);
        public string[] getImages(string carFolder);
        public string getUIJSON(string carFolder);
        public string getCarName(string carFolder);
        public bool precautionChecks(string carsPath);
    }
}
